/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.SessionState;
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;
using log4net;

namespace FocusOPEN.Website.MyAccount
{
	public partial class ViewLightbox : BasePageablePage
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int firstAssetId=-1;
        private int lastAssetId=-1;
        private int totalItems = -1;
        IOrderedEnumerable<LightboxAsset> validItems;

		#region Constructor

		protected ViewLightbox()
		{
			PageStatePersisterType = PersisterType.ZipCompressed;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// Gets the current lightbox ID, as selected in the dropdown
		/// </summary>
		private int CurrentLightboxId
		{
			get
			{
				int lightboxId = UserLightboxDropDownList1.SelectedId;

				if (lightboxId == 0)
					lightboxId = -1;

				return lightboxId;
			}
		}

		/// <summary>
		/// Gets the current lightbox.
		/// </summary>
		/// <value>The current lightbox.</value>
		private Lightbox CurrentLightbox
		{
			get
			{
				return ContextInfo.LightboxManager.GetLightboxById(CurrentLightboxId);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the current user can manage the current lightbox.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance can manage lightbox; otherwise, <c>false</c>.
		/// </value>
		private bool CanManageLightbox
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "CanManageLightbox", false);
			}
			set
			{
				ViewState["CanManageLightbox"] = value;
			}
		}

		/// <summary>
		/// Returns the ID's of selected assets
        /// needs to be static as to be called from a Web Method 
		/// </summary>
		private static List<Int32> SelectedAssetIdList
		{
			get
			{
                HttpSessionState session = HttpContext.Current.Session;
                List<Int32> list = session["SelectedAssetIdList"] as List<Int32>;

				if (list == null)
				{
					list = new List<Int32>();
                    session["SelectedAssetIdList"] = list;
				}

				return list;
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether all assets in the lightbox are selected
		/// </summary>
		/// <value><c>true</c> if [all assets selected]; otherwise, <c>false</c>.</value>
		private bool AllAssetsSelected
		{
			get
			{
				return (SelectAllAssetsInLightbox1.Checked || SelectAllAssetsInLightbox2.Checked);
			}
			set
			{
				SelectAllAssetsInLightbox1.Checked = value;
				SelectAllAssetsInLightbox2.Checked = value;

				if (value)
				{
					foreach (Asset asset in CurrentLightbox.GetAssetList())
					{
						int assetId = asset.AssetId.GetValueOrDefault();

						if (!SelectedAssetIdList.Contains(assetId))
							SelectedAssetIdList.Add(assetId);
					}
				}
			}
		}

		protected override ContextType AssetContext
		{
			get
			{
				return ContextType.Lightbox;
			}
		}

		#endregion

		#region Handled Events

		protected void Page_Init(object sender, EventArgs e)
		{
			// Force the dropdowns to refresh when lightboxes are updated
			ContextInfo.LightboxManager.LightboxListChanged += delegate
			                                                   	{
			                                                   		int currentLightboxId = UserLightboxDropDownList1.SelectedId;
			                                                   		UserLightboxDropDownList1.RefreshFromDataSource();
			                                                   		UserLightboxDropDownList1.SafeSelectValue(currentLightboxId);

			                                                   		foreach (UserLightboxDropDownList dd in new[] {RemoveLightboxDropDownList, MergeLightboxSourceDropDownList, MergeLightboxTargetDropDownList, DuplicateLightboxDropDownList, RenameLightboxDropDownList, SendLightboxDropDownList, SetDefaultLightboxDropDownList})
			                                                   			dd.RefreshFromDataSource();
			                                                   	};
		}

		protected void Page_Load(object sender, EventArgs e)
		{
            if (!Page.IsPostBack)
            {
                // Select the default pagesize
                PageSizeDropDownList1.SafeSelectValue(SessionInfo.Current.UserSessionInfo.LightboxPageSize);

                // Get the lightbox id from the querystring or user's persistent lightbox
                int lightboxId = WebUtils.GetIntRequestParam("lightboxId", SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId);

                // Get the lightbox matching the retrieved lightboxId
                Lightbox lightbox = ContextInfo.LightboxManager.GetLightboxById(lightboxId);

                // Revert to the default lightbox if a valid lightbox was not found
                if (lightbox.IsNull)
                    lightbox = ContextInfo.LightboxManager.GetDefaultLightbox();

                // Display the lightbox
                DisplayLightbox(lightbox);

                string action = WebUtils.GetRequestParam("action", string.Empty);

                switch (action)
                {
                    case "new":

                        ShowMainPanel("lightboxes");
                        ShowActionPanel("lightbox.new");
                        break;

                    case "send":

                        ShowMainPanel("lightboxes");
                        ShowActionPanel("lightbox.send");
                        break;
                }
            }
            AssetIDPanelField.Value = "-1";
        }

		protected void Page_PreRender(object sender, EventArgs e)
		{
			// Disable the 'Move/Copy assets to another lightbox' link if we only have one lightbox
			MoveCopyAssetsToAnotherLightbox.Visible = (ContextInfo.LightboxManager.UserLightboxes.Count > 1 || (!ContextInfo.LightboxManager.ContainsLightbox(CurrentLightboxId)));
			MergeLightboxLinkButton.Visible = (ContextInfo.LightboxManager.UserLightboxes.Count > 1);
			RemoveLightboxLinkButton.Visible = (ContextInfo.LightboxManager.UserLightboxes.Count > 1);

			// Only allow saving of all notes if user can manage lightbox
			SaveAllNotesWrapper1.Visible = SaveAllNotesWrapper2.Visible = CanManageLightbox;
		}

		protected void UserLightboxDropDownList1_SelectedIndexChanged(object sender, EventArgs e)
		{
			AllAssetsSelected = false;
			SelectedAssetIdList.Clear();

			HideActionPanels();
			DisplaySelectedLightbox();
		}

		protected void ToggleNotesLinkButton_Click(object sender, EventArgs e)
		{
			ToggleNotes(!NotesPanel.Visible);
		}

		protected void ToggleControlPanelVisibility(object sender, CommandEventArgs e)
		{
			string panelOn = e.CommandArgument.ToString();
			ShowMainPanel(panelOn);
		}

		protected void PageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			Bind(1);
		}

		protected void Page_IndexChanged(object sender, CommandEventArgs e)
		{
			int pg = Convert.ToInt32(e.CommandArgument);
			Bind(pg);
		}

		protected void LightboxActionLinkButton_Command(object sender, CommandEventArgs e)
		{
			HideActionPanels();
			ShowActionPanel(e.CommandArgument.ToString());
		}

		protected void LightboxActionButton_Command(object sender, CommandEventArgs e)
		{
			string commandArgument = e.CommandArgument.ToString();
			ProcessControlPanelAction(commandArgument);
		}

		protected void CancelAction_Click(object sender, EventArgs e)
		{
			HideActionPanels();
		}

		protected void IsPublicRadioButtonGroup_Click(object sender, EventArgs e)
		{
			// Only display the brand selector when the lightbox and the user editing is a super-admin with access to more than one brand
			BrandSelectorRow.Visible = ShouldDisplayBrandSelector(IsPublicRadioButton.Checked);
		}

		protected void SaveNotes_Click(object sender, EventArgs e)
		{
			Button btn = (Button) sender;

			foreach (RepeaterItem ri in LightboxAssetsRepeater.Items)
			{
				if (GeneralUtils.ValueIsInList(ri.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
				{
					Button SaveNotesButton = (Button) ri.FindControl("SaveNotesButton");

					if (SaveNotesButton.UniqueID == btn.UniqueID)
					{
						SaveNotes(ri);
						FeedbackLabel1.SetSuccessMessage("notes updated");
						break;
					}
				}
			}
		}

		protected void OrderAssetDown_Click(object sender, EventArgs e)
		{
			Button OrderAssetButton = (Button) sender;
			int? assetId = GetAssetIdByControl(OrderAssetButton);

			if (assetId.HasValue)
			{
				int iIndex = ((CurrentPage - 1)*PageSize) + Int32.Parse(OrderAssetButton.CommandArgument);
                ContextInfo.LightboxManager.ReorderLightboxAsset(CurrentLightboxId, assetId.Value, iIndex + 1);
                LoadValidItems();
                SaveNotesToObject();

                Bind(CurrentPage);

                bool changedPage = iIndex % PageSize == PageSize-1 && CurrentPage < TotalPages ;
                if (changedPage)
                {
                    TopPager.RaisePostBackEvent(CurrentPage + 1);
                }

                //ScriptManager.RegisterStartupScript(LightboxAssetsRepeater, typeof(Repeater), "hash", "window.location.hash = '#Asset" + assetId + "';", true);
                int assetIdfromPanel = -1;
                MovedInARow.Value = "false";
                if (Int32.TryParse(AssetIDPanelField.Value, out assetIdfromPanel) && assetIdfromPanel > 0 &&
                    assetIdfromPanel == assetId.Value && !changedPage)
                {
                    MovedInARow.Value = "down";
                }
                AssetIDPanelField.Value = assetId.Value.ToString();
            }
        }

		protected void OrderAssetUp_Click(object sender, EventArgs e)
		{
			Button OrderAssetButton = (Button) sender;
			int? assetId = GetAssetIdByControl(OrderAssetButton);

			if (assetId.HasValue)
			{
				int iIndex = ((CurrentPage - 1)*PageSize) + Int32.Parse(OrderAssetButton.CommandArgument);

				if (iIndex > 0 )
				{
                    ContextInfo.LightboxManager.ReorderLightboxAsset(CurrentLightboxId, assetId.Value, iIndex - 1);
                    LoadValidItems();
                    SaveNotesToObject();
                    Bind(CurrentPage);
				}

                bool changedPage = iIndex % PageSize == 0 && CurrentPage > 1;
                MovedInARow.Value = "false";
                if (changedPage)
                {
                    TopPager.RaisePostBackEvent(CurrentPage - 1);
                    //AssetIDPanelField.Value = "-1";
                }
                else
                {
                    int assetIdfromPanel = -1;
                    if (Int32.TryParse(AssetIDPanelField.Value, out assetIdfromPanel) && assetIdfromPanel > 0 &&
                        assetIdfromPanel == assetId.Value)
                    {
                        MovedInARow.Value = "up";
                    }
                }
                AssetIDPanelField.Value = assetId.Value.ToString();
            }
        }

		protected void AssetButtons1_LightboxButtonClicked(object sender, EventArgs e)
		{
			Bind(CurrentPage);
		}

		protected void EditLightboxNotesLinkButton_Click(object sender, EventArgs e)
		{
			ToggleEditLightboxNotesPanel(true);
		}

		protected void SaveLightboxNotesButton_Click(object sender, EventArgs e)
		{
			try
			{
				CurrentLightbox.Name = LightboxTitleTextBox.Text.Trim();
				CurrentLightbox.Summary = LightboxSummaryTextBox.Text.Trim();
				CurrentLightbox.IsPublic = IsPublicRadioButton.Checked;
				CurrentLightbox.Notes = LightboxNotesTextBox.Text.Trim();

				CurrentLightbox.Brands.Clear();

				if (CurrentLightbox.IsPublic)
				{
					if (BrandSelectorRow.Visible)
					{
						foreach (ListItem item in BrandCheckBoxList1.Items)
						{
							if (item.Selected)
							{
								int brandId = Convert.ToInt32(item.Value);
								CurrentLightbox.Brands.Add(BrandCache.Instance.GetById(brandId));
							}
						}
					}
					else
					{
						CurrentLightbox.Brands.Add(CurrentUser.PrimaryBrand);
					}
				}

				ContextInfo.LightboxManager.SaveLightbox(CurrentLightbox);
				DisplayLightboxNotes(CurrentLightbox);

				AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ModifyLightbox, string.Empty);

				LightboxTextPanel.Visible = true;
				LightboxEditTextPanel.Visible = false;
			}
			catch (InvalidLightboxException ilex)
			{
				LightboxEditMessageLabel.SetErrorMessage(ilex.Message);
			}
		}

		protected void CancelLightboxNotesButton_Click(object sender, EventArgs e)
		{
			ToggleEditLightboxNotesPanel(false);
		}

		protected void LightboxAssetsRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					LightboxAsset lba = (LightboxAsset) e.Item.DataItem;
					Asset asset = lba.Asset;

					HiddenField AssetIdHiddenField = (HiddenField) e.Item.FindControl("AssetIdHiddenField");
					AssetIdHiddenField.Value = asset.AssetId.ToString();

					HiddenField LightboxAssetIdHiddenField = (HiddenField) e.Item.FindControl("LightboxAssetIdHiddenField");
					LightboxAssetIdHiddenField.Value = lba.LightboxAssetId.ToString();

					AssetThumbnail AssetThumbnail11 = (AssetThumbnail) e.Item.FindControl("AssetThumbnail1");
					AssetThumbnail11.Initialise(asset);

					AssetButtons AssetButtons1 = (AssetButtons) e.Item.FindControl("AssetButtons1");
					AssetButtons1.LightboxId = CurrentLightboxId;
					AssetButtons1.Initialise(asset);

					HtmlTableCell DescriptionCell = (HtmlTableCell) e.Item.FindControl("DescriptionCell");
					DescriptionCell.InnerHtml = SiteUtils.ConvertTextToHtml(asset.Description);
					SiteUtils.PopulateBlankControl(DescriptionCell);

					Label AssetTypeLabel = (Label) e.Item.FindControl("AssetTypeLabel");
					AssetTypeLabel.Text = asset.AssetType.Name;

					HtmlTableRow NotesRow = (HtmlTableRow) e.Item.FindControl("NotesRow");
					NotesRow.Visible = CanManageLightbox;

					Panel SaveNotesButtonPanel = (Panel) e.Item.FindControl("SaveNotesButtonPanel");
					SaveNotesButtonPanel.Visible = CanManageLightbox;

					TextBox NotesTextBox = (TextBox) e.Item.FindControl("NotesTextBox");
					NotesTextBox.Text = lba.Notes;

					CheckBox SelectAssetCheckBox = (CheckBox) e.Item.FindControl("SelectAssetCheckBox");
					SelectAssetCheckBox.Checked = IsAssetSelected(asset.AssetId.GetValueOrDefault());

					Panel OrderAssetButtonPanel = (Panel) e.Item.FindControl("OrderAssetButtonPanel");
					OrderAssetButtonPanel.Visible = CanManageLightbox;

					Button OrderAssetUpButton = (Button) e.Item.FindControl("OrderAssetUpButton");
					OrderAssetUpButton.CommandArgument = e.Item.ItemIndex.ToString();

					Button OrderAssetDownButton = (Button) e.Item.FindControl("OrderAssetDownButton");
					OrderAssetDownButton.CommandArgument = e.Item.ItemIndex.ToString();

                    int index = PageSize * (CurrentPage-1) + e.Item.ItemIndex;
                    //index == 0
                    if ( asset.AssetId == firstAssetId)
                    {
                        OrderAssetUpButton.Visible = false;
                    }
                    if ( asset.AssetId == lastAssetId )
                    {
                        OrderAssetDownButton.Visible = false;
                    }

					break;
			}
		}

		protected void RemoveSelectedAssetsLinkButton_Click(object sender, EventArgs e)
		{
			ProcessControlPanelAction("assets.remove");
		}

		protected void SendLightboxCCDropDownList_SelectedIndexChanged(object sender, EventArgs e)
		{
			AppendSelectionToCC();
		}

		#endregion


        #region Web Methods

        [WebMethod]
        public static void SetAssetSelected(int assetId, bool selected)
        {
            if (selected)
            {
                if (!SelectedAssetIdList.Contains(assetId))
                    SelectedAssetIdList.Add(assetId);
            }
            else
            {
                if (SelectedAssetIdList.Contains(assetId))
                    SelectedAssetIdList.Remove(assetId);
            }

        }

        [WebMethod]
        public static void SelectAllAssets(int lightboxId)
        {
            SetAllAssetsSelected(lightboxId, true);
        }

        [WebMethod]
        public static void DeselectAllAssets(int lightboxId)
        {
            SetAllAssetsSelected(lightboxId, false);
        }
        
        [WebMethod]
		public static bool SaveNotes(int lightboxAssetId, string notes)
		{
			LightboxAsset lba = LightboxAsset.Get(lightboxAssetId);

			if (!lba.IsNull && EntitySecurityManager.CanManageLightbox(CurrentUser, lba.Lightbox))
			{
				lba.Notes = notes;
				LightboxAsset.Update(lba);

				return true;
			}

			return false;
		}

        #endregion
        
        #region Helper Methods

        private static void SetAllAssetsSelected(int lightboxId, bool selected)
        {
            Lightbox lightbox = Lightbox.Get(lightboxId);
            if (selected)
            {
                foreach (Asset asset in lightbox.GetAssetList())
                {
                    int assetId = asset.AssetId.GetValueOrDefault();

                    if (!SelectedAssetIdList.Contains(assetId))
                        SelectedAssetIdList.Add(assetId);
                }
            }
            else
            {
                SelectedAssetIdList.Clear();
            }
        }

		private static void SaveNotes(RepeaterItem ri)
		{
			HiddenField LightboxAssetIdHiddenField = (HiddenField) ri.FindControl("LightboxAssetIdHiddenField");
			int lightboxAssetId = Convert.ToInt32(LightboxAssetIdHiddenField.Value);

			TextBox NotesTextBox = (TextBox) ri.FindControl("NotesTextBox");
			string notes = NotesTextBox.Text.Trim();

			SaveNotes(lightboxAssetId, notes);
		}

		/// <summary>
		/// Determines whether the brand selector should be displayed for the currently displayed lightbox
		/// </summary>
		/// <param name="isPublic">Boolean value specifying whether the lightbox is public</param>
		private static bool ShouldDisplayBrandSelector(bool isPublic)
		{
			return (isPublic &&
			        CurrentUser.UserRole == UserRole.SuperAdministrator &&
			        CurrentUser.Brands.Count > 1);
		}

		/// <summary>
		/// Toggles the edit lightbox notes panel.
		/// </summary>
		/// <param name="showEditPanel">if set to <c>true</c> [show edit panel].</param>
		private void ToggleEditLightboxNotesPanel(bool showEditPanel)
		{
			LightboxTextPanel.Visible = !showEditPanel;
			LightboxEditTextPanel.Visible = showEditPanel;
		}

		/// <summary>
		/// Toggles the notes panel
		/// </summary>
		private void ToggleNotes(bool show)
		{
			if (show)
			{
				NotesPanel.Visible = true;
				ToggleNotesLinkButton.Text = "hide notes [-]";
			}
			else
			{
				NotesPanel.Visible = false;
				ToggleNotesLinkButton.Text = "show notes [+]";
			}
		}

		private bool IsAssetSelectedOnPage(int assetId)
		{
			foreach (RepeaterItem ri in LightboxAssetsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						HiddenField AssetIdHiddenField = (HiddenField) ri.FindControl("AssetIdHiddenField");
						int currentAssetId = NumericUtils.ParseInt32(AssetIdHiddenField.Value, 0);

						if (currentAssetId == assetId)
						{
							CheckBox SelectAssetCheckBox = (CheckBox) ri.FindControl("SelectAssetCheckBox");
							return (SelectAssetCheckBox.Checked);
						}

						break;
				}
			}

			return false;
		}

		private bool IsAssetSelected(int assetId)
		{
			return (AllAssetsSelected || SelectedAssetIdList.Contains(assetId) || IsAssetSelectedOnPage(assetId));
		}

		private void ToggleSelectedAssets()
		{
			foreach (RepeaterItem ri in LightboxAssetsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						CheckBox SelectAssetCheckBox = (CheckBox) ri.FindControl("SelectAssetCheckBox");
						SelectAssetCheckBox.Checked = AllAssetsSelected;

						break;
				}
			}
		}

		private void ShowMainPanel(string name)
		{
			LightboxControlPanel.Visible = (name == "lightboxes");
			AssetControlPanel.Visible = !LightboxControlPanel.Visible;
		}

		private void HideActionPanels()
		{
			NewLightboxPanel.Visible = false;
			RemoveLightboxPanel.Visible = false;
			MergeLightboxPanel.Visible = false;
			RenameLightboxPanel.Visible = false;
			DuplicateLightboxPanel.Visible = false;
			SendLightboxPanel.Visible = false;
			SetDefaultLightboxPanel.Visible = false;

			RemoveAssetsPanel.Visible = false;
			MoveCopyAssetsPanel.Visible = false;
			MoveCopyAssetsToCartPanel.Visible = false;

			NewLightboxNameTextBox.Text = string.Empty;
			DuplicateLightboxNameTextBox.Text = string.Empty;
			RenameLightboxTextBox.Text = string.Empty;

			SendLightboxDropDownList.SelectedIndex = 0;
			SendLightboxMessageSubject.Text = string.Empty;
			SendLightboxMessageBody.Text = string.Empty;
			SendLightboxRecipientEmail.Text = string.Empty;
			SendLightboxCCEmail.Text = string.Empty;
			SendLightboxRecipientDropDownList.SelectedIndex = 0;
			SendLightboxCCDropDownList.SelectedIndex = 0;
			SendLightboxExpiryDate.Clear();
		}

		private static string GetLightboxNameForTextBox(string name)
		{
			// Remove "(default)";
			string newName = name.Replace("(default)", string.Empty);

			// Remove sender name
			newName = LightboxManager.StripSenderName(newName);

			// Remove whitespace
			newName = newName.Trim();

			// Done processing.
			return newName;
		}

		private void ShowActionPanel(string action)
		{
			switch (action)
			{
				case "lightbox.new":
					NewLightboxPanel.Visible = true;
					break;

				case "lightbox.remove":
					RemoveLightboxDropDownList.SafeSelectValue(CurrentLightboxId);
					RemoveLightboxPanel.Visible = true;
					break;

				case "lightbox.merge":
					MergeLightboxSourceDropDownList.SafeSelectValue(CurrentLightboxId);
					MergeLightboxPanel.Visible = true;
					break;

				case "lightbox.duplicate":
					DuplicateLightboxDropDownList.SafeSelectValue(CurrentLightboxId);
					DuplicateLightboxNameTextBox.Text = string.Format("{0} (copy)", GetLightboxNameForTextBox(DuplicateLightboxDropDownList.SelectedItem.Text));
					DuplicateLightboxPanel.Visible = true;
					break;

				case "lightbox.rename":
					RenameLightboxDropDownList.SafeSelectValue(CurrentLightboxId);
					RenameLightboxTextBox.Text = GetLightboxNameForTextBox(RenameLightboxDropDownList.SelectedItem.Text);
					RenameLightboxPanel.Visible = true;
					break;

				case "lightbox.send":
					SendLightboxDropDownList.SafeSelectValue(CurrentLightboxId);
					SendLightboxMessageSubject.Text = "You have received a lightbox";
					//only super admin users get option to enable contact sheet download
					SendLightboxDownloadLinksWrapper.Visible = (CurrentUser.UserRole == UserRole.SuperAdministrator);
					SendLightboxPanel.Visible = true;
					break;

				case "lightbox.makedefault":
					SetDefaultLightboxDropDownList.SafeSelectValue(CurrentLightboxId);
					SetDefaultLightboxPanel.Visible = true;
					break;

				case "assets.remove":
					RemoveAssetsPanel.Visible = true;
					break;

				case "assets.movecopylightbox":

					// Refresh the lightbox list
					MoveCopyAssetsLightboxDropDownList.RefreshFromDataSource();

					// Check if the list contains the current lightbox ID
					ListItem li = MoveCopyAssetsLightboxDropDownList.Items.FindByValue(CurrentLightboxId.ToString());

					// Remove it if found (can't copy/move assets to the same lightbox)
					if (li != null)
						MoveCopyAssetsLightboxDropDownList.Items.Remove(li);

					// Disable the controls if no lightboxes
					// We should never hit this, due to another check to hide the
					// button for this panel if we only have one lightbox, but this
					// is a secondary check (or in case we decide to put the button back)
					if (MoveCopyAssetsLightboxDropDownList.Items.Count == 0)
					{
						MoveCopyAssetsLightboxDropDownList.Items.Add("No lightboxes");
						MoveCopyAssetsLightboxDropDownList.Enabled = false;
						MoveCopyAssetsButton.Enabled = false;
					}
					else
					{
						MoveCopyAssetsLightboxDropDownList.Enabled = true;
						MoveCopyAssetsButton.Enabled = true;
					}

					MoveCopyAssetsPanel.Visible = true;
					break;

				case "assets.movecopycart":
					MoveCopyAssetsToCartPanel.Visible = true;
					break;
			}
		}

		/// <summary>
		/// Displays the currently selected lightbox (in the dropdown)
		/// </summary>
		private void DisplaySelectedLightbox()
		{
			DisplayLightbox(CurrentLightbox);
		}

		private void DisplayLightbox(Lightbox lb)
		{
			// Get the lightbox ID
			int lightboxId = lb.LightboxId.GetValueOrDefault();

			// User can edit if lightbox if they own it or are a superadmin or its linked and editable
			CanManageLightbox = EntitySecurityManager.CanManageLightbox(CurrentUser, lb);

			// Disable options not available to lightbox owners
			RemoveSelectedAssetsLinkButton1.Visible = CanManageLightbox;
			RemoveSelectedAssetsLinkButton2.Visible = CanManageLightbox;
			EditLightboxNotesButtonWrapper.Visible = CanManageLightbox;
			RemoveAssetsFromLightboxLinkButton.Visible = CanManageLightbox;
			MoveCopyAssetsLightboxRadioButtonList.Items[0].Enabled = CanManageLightbox;
			MoveCopyAssetsCartRadioButtonList.Items[0].Enabled = CanManageLightbox;

			// Change the persistent cart lightbox to the selected one if can manage it
			// and it's our own lightbox (eg. not a public one owned by another user)
			if (CanManageLightbox && ContextInfo.LightboxManager.ContainsLightbox(lightboxId))
				SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId = lightboxId;

			// No assets selected as we've changed lightbox
			AllAssetsSelected = false;
			SelectedAssetIdList.Clear();

			// Display the lightbox information
			DisplayLightboxNotes(lb);

			// Hide (collapse) the lightbox notes
			ToggleNotes(false);

			// Hide the edit lightbox info panel
			ToggleEditLightboxNotesPanel(false);

			Bind(1);

			// Select this lightbox in the dropdown
			UserLightboxDropDownList1.SafeSelectValue(lightboxId);
		}

		/// <summary>
		/// Displays the specified lightbox details
		/// </summary>
		private void DisplayLightboxNotes(Lightbox lb)
		{
			// Labels
			LightboxTitleLabel.Text = lb.Name;
			LightboxSummaryLabel.Text = GeneralUtils.GetNonEmptyString(lb.Summary, "No summary information has been added");
			LightboxStatusLabel.Text = (lb.IsPublic) ? "Public" : "Private";
			LinkToLightboxHyperlink.NavigateUrl = "~/MyAccount/ViewLightbox.aspx?LightboxId=" + lb.LightboxId;
			LightboxNotesLabel.Text = lb.Notes;

			// Form inputs (for editing)
			LightboxTitleTextBox.Text = lb.Name;
			LightboxSummaryTextBox.Text = lb.Summary;
			IsPublicRadioButton.Checked = lb.IsPublic;
			IsPrivateRadioButton.Checked = !lb.IsPublic;
			BrandSelectorRow.Visible = ShouldDisplayBrandSelector(lb.IsPublic);
			LightboxEditStatusLabel.Text = (lb.IsPublic) ? "Public" : "Private";
			LightboxNotesTextBox.Text = lb.Notes;

			if (CurrentUser.UserRole == UserRole.SuperAdministrator)
			{
				// Deselect all brands
				foreach (ListItem li in BrandCheckBoxList1.Items)
					li.Selected = false;

				// Select brands to which this lightbox is assigned
				foreach (Brand brand in lb.Brands)
				{
					ListItem li = BrandCheckBoxList1.Items.FindByValue(brand.BrandId.ToString());

					if (li != null)
						li.Selected = true;
				}
			}

			// Hide the public/private radio buttons for non-superadmins & non-brand-admins
			// as they cannot change the status of a lightbox
			if (!GeneralUtils.ValueIsInList(CurrentUser.UserRole, UserRole.SuperAdministrator, UserRole.BrandAdministrator))
			{
				IsPublicRadioButton.Visible = false;
				IsPrivateRadioButton.Visible = false;
				LightboxEditStatusLabel.Visible = true;
			}

			UserLightboxDropDownList1.SafeSelectValue(lb.LightboxId);
		}

		private void Bind(int page)
		{
            LoadValidItems();
            
			// Set the page size
			SessionInfo.Current.UserSessionInfo.LightboxPageSize = PageSize;

            // 0 - 1
            if (PageSize * (page - 1) >= totalItems & (page>1) )
            {
                page = (int)Math.Ceiling( (double)totalItems / PageSize );
            }

			// Bind the asset list to relevant page
			LightboxAssetsRepeater.DataSource = validItems.Skip((page - 1)*PageSize).Take(PageSize);
			LightboxAssetsRepeater.DataBind();

			// Setup the top pager
			TopPager.CurrentPage = page;
			TopPager.PageSize = PageSize;
			TopPager.ItemCount = totalItems;
			TopPager.CalculatePageCount();

			// Setup the bottom pager
			BottomPager.CurrentPage = TopPager.CurrentPage;
			BottomPager.PageSize = TopPager.PageSize;
			BottomPager.ItemCount = TopPager.ItemCount;
			BottomPager.CalculatePageCount();

			// Setup page variables
			CurrentPage = page;
			AssetCount = totalItems;
			TotalPages = TopPager.PageCount;

			if (totalItems == 0)
			{
				FeedbackLabel1.SetErrorMessage("no assets have been added to this lightbox");
				FeedbackLabel1.Pinned = true;

				EmptyLightboxPanel.Visible = true;
				LightboxAssetsPanel.Visible = false;
            }
			else
			{
				FeedbackLabel1.Visible = false;
				FeedbackLabel1.Pinned = false;

				EmptyLightboxPanel.Visible = false;
				LightboxAssetsPanel.Visible = true;
            }
		}

        private void LoadValidItems()
        {
            LoadValidItems(false);
        }

        private void LoadValidItems(bool force)
        {
            if (validItems == null || force)
            {
                // First get all of the assets in the lightbox
                LightboxAssetFinder finder = new LightboxAssetFinder { LightboxId = CurrentLightboxId };
                List<LightboxAsset> items = LightboxAsset.FindMany(finder);

                // Filter out those assets that are not accessible by the current user
                // and order by the ordernumber value
                validItems = (from item in items
                              where EntitySecurityManager.CanViewAssetInfo(CurrentUser, item.Asset)
                              orderby item.OrderNumber.GetValueOrDefault(9999), item.LightboxAssetId
                              select item);

                // Get the asset count
                totalItems = validItems.Count();

                if (totalItems > 0)
                {
                    firstAssetId = validItems.First().AssetId;
                    lastAssetId = validItems.Last().AssetId;
                }
            }
        }
        
        private void ProcessControlPanelAction(string commandArgument)
		{
			switch (commandArgument)
			{
					#region Asset Commmands

				case "assets.remove":

					if (SelectedAssetIdList.Count == 0)
					{
						RemoveAssetsFeedbackLabel.SetErrorMessage("no assets selected");
					}
					else if (!EntitySecurityManager.CanManageLightbox(CurrentUser, CurrentLightbox))
					{
						FeedbackLabel1.SetErrorMessage("you cannot remove assets from this lightbox");
						m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to remove assets from LightboxId: {2} but was denied permission", CurrentUser.FullName, CurrentUser.UserId, CurrentLightbox.LightboxId);
					}
					else
					{
						foreach (int assetId in SelectedAssetIdList)
							ContextInfo.LightboxManager.RemoveAssetFromLightbox(CurrentLightboxId, assetId);

						SelectedAssetIdList.Clear();

                        LoadValidItems(true);

						Bind(CurrentPage);

						FeedbackLabel1.SetSuccessMessage("assets removed successfully");

						HideActionPanels();
					}

					break;

				case "assets.movecopylightbox":

					if (SelectedAssetIdList.Count == 0)
					{
						MoveCopyAssetsFeedbackLabel.SetErrorMessage("no assets selected");
					}
					else if (MoveCopyAssetsLightboxDropDownList.SelectedId == CurrentLightboxId)
					{
						MoveCopyAssetsFeedbackLabel.SetErrorMessage("cannot move/copy assets to the same lightbox");
					}
					else
					{
						bool isMove = (MoveCopyAssetsLightboxRadioButtonList.SelectedValue == "move");

						if (isMove && !EntitySecurityManager.CanManageLightbox(CurrentUser, CurrentLightbox))
						{
							MoveCopyAssetsFeedbackLabel.SetErrorMessage("cannot move assets in this lightbox to another lightbox (only copy)");
							m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to move assets from LightboxId: {2} but was denied permission", CurrentUser.FullName, CurrentUser.UserId, CurrentLightbox.LightboxId);
						}
						else
						{
							int targetLightboxId = MoveCopyAssetsLightboxDropDownList.SelectedId;

							Lightbox targetLightbox = ContextInfo.LightboxManager.GetLightboxById(targetLightboxId);
							//check that user is able to add assets to the target lightbox
							if (!targetLightbox.IsNull && EntitySecurityManager.CanManageLightbox(CurrentUser, targetLightbox))
							{
								EntityList<LightboxAsset> lightboxAssets = CurrentLightbox.GetLightboxAssetList();
								foreach (int assetId in SelectedAssetIdList)
								{
									LightboxAsset lba = lightboxAssets.FirstOrDefault(a => a.AssetId == assetId);

									if (lba != null) //check that not trying to add a null lightboxasset
									{
										ContextInfo.LightboxManager.AddAssetToLightbox(targetLightboxId, lba);

										if (isMove)
										{
											string notes = string.Format("Moved to LightboxId: {0}", targetLightboxId);
											ContextInfo.LightboxManager.RemoveAssetFromLightbox(CurrentLightboxId, assetId, notes);
										}
									}
								}

								SelectedAssetIdList.Clear();

								Bind(CurrentPage);

								FeedbackLabel1.SetSuccessMessage(string.Format("assets {0} to lightbox successfully", isMove ? "moved" : "copied"));

								HideActionPanels();
							}
							else
							{
								MoveCopyAssetsFeedbackLabel.SetErrorMessage("cannot move or copy assets to this lightbox");
								m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to move assets to LightboxId: {2} but was denied permission", CurrentUser.FullName, CurrentUser.UserId, targetLightboxId);
							}
						}
					}

					break;

				case "assets.movecopycart":

					if (SelectedAssetIdList.Count == 0)
					{
						MoveCopyAssetsToCartFeedbackLabel.SetErrorMessage("no assets selected");
					}
					else
					{
						bool isMove = (MoveCopyAssetsCartRadioButtonList.SelectedValue == "move");

						if (isMove && !EntitySecurityManager.CanManageLightbox(CurrentUser, CurrentLightbox))
						{
							MoveCopyAssetsFeedbackLabel.SetErrorMessage("cannot move assets in this lightbox to cart (only copy)");
							m_Logger.DebugFormat("User: {0} (UserId: {1}) tried to move assets from LightboxId: {2} to cart but was denied permission", CurrentUser.FullName, CurrentUser.UserId, CurrentLightbox.LightboxId);
						}
						else
						{
							foreach (int assetId in SelectedAssetIdList)
							{
								ContextInfo.CartManager.AddAssetToCart(assetId);

								if (isMove)
									ContextInfo.LightboxManager.RemoveAssetFromLightbox(CurrentLightboxId, assetId, "Moved to cart");
							}

							SelectedAssetIdList.Clear();

							Bind(CurrentPage);

							FeedbackLabel1.SetSuccessMessage(string.Format("assets {0} to cart successfully", isMove ? "moved" : "copied"));

							HideActionPanels();
						}
					}

					break;

					#endregion

					#region Lightbox Commands

				case "lightbox.new":

					try
					{
						Lightbox lb = ContextInfo.LightboxManager.CreateLightbox(NewLightboxNameTextBox.Text, MakeNewLightboxDefaultCheckBox.Checked);

						FeedbackLabel1.SetSuccessMessage("lightbox created successfully");
						HideActionPanels();

						// Switch the selected lightbox in the persistent cart to this one
						SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId = lb.LightboxId.GetValueOrDefault();

						DisplayLightbox(lb);
					}
					catch (InvalidLightboxException ilex)
					{
						NewLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

				case "lightbox.remove":

					try
					{
						ContextInfo.LightboxManager.RemoveLightbox(RemoveLightboxDropDownList.SelectedId);
						FeedbackLabel1.SetSuccessMessage("lightbox removed successfully");
						HideActionPanels();
						DisplaySelectedLightbox();
					}
					catch (InvalidLightboxException ilex)
					{
						RemoveLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

				case "lightbox.merge":

					try
					{
						ContextInfo.LightboxManager.MergeLightbox(MergeLightboxSourceDropDownList.SelectedId, MergeLightboxTargetDropDownList.SelectedId, MergeLightboxRemoveSourceCheckBox.Checked);
						FeedbackLabel1.SetSuccessMessage("lightboxes merged successfully");
						HideActionPanels();

						UserLightboxDropDownList1.SafeSelectValue(MergeLightboxTargetDropDownList.SelectedId);
						DisplaySelectedLightbox();
					}
					catch (InvalidLightboxException ilex)
					{
						MergeLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

				case "lightbox.duplicate":

					try
					{
						Lightbox lb = ContextInfo.LightboxManager.DuplicateLightbox(DuplicateLightboxDropDownList.SelectedId, DuplicateLightboxNameTextBox.Text);
						FeedbackLabel1.SetSuccessMessage("lightbox duplicated successfully");
						HideActionPanels();
						DisplayLightbox(lb);
					}
					catch (InvalidLightboxException ilex)
					{
						DuplicateLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

				case "lightbox.rename":

					try
					{
						// Get the ID of the lightbox currently being viewed.  This is because renaming a lightbox
						// will cause the lightbox dropdown to refresh, losing its selected index.
						int currentLightboxId = UserLightboxDropDownList1.SelectedId;

						// Rename the lightbox to its new name
						ContextInfo.LightboxManager.RenameLightbox(RenameLightboxDropDownList.SelectedId, RenameLightboxTextBox.Text);
						FeedbackLabel1.SetSuccessMessage("lightbox renamed successfully");

						// Hide the action panels as we are done with rename
						HideActionPanels();

						// Select the same lightbox as before
						UserLightboxDropDownList1.SafeSelectValue(currentLightboxId);

						// Display it (in case the name changed)
						DisplaySelectedLightbox();
					}
					catch (InvalidLightboxException ilex)
					{
						RenameLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

				case "lightbox.send":

					string recipient = StringUtils.IsBlank(SendLightboxRecipientEmail.Text) ? SendLightboxRecipientDropDownList.SelectedValue : SendLightboxRecipientEmail.Text;

					//check no additional emails to append to the cc string
					AppendSelectionToCC();
					string cc = SendLightboxCCEmail.Text;

					try
					{
						ContextInfo.LightboxManager.SendLightbox(SendLightboxDropDownList.SelectedId, SendLightboxMessageSubject.Text, SendLightboxMessageBody.Text, recipient, cc, SendLightboxExpiryDate.SelectedDate, SendLightboxDownloadLinksCheckBox.Checked, (SendLightboxTypeRadioButtonList.SelectedValue == "link"), SendLightboxEnableLinkedEditCheckBox.Checked);
						FeedbackLabel1.SetSuccessMessage("lightbox sent successfully");
						HideActionPanels();
					}
					catch (InvalidLightboxException ilex)
					{
						if (ilex.Errors.Count > 0)
						{
							SendLightboxFeedbackLabel.SetErrorMessage("the following errors occured:", ilex.Errors);
						}
						else
						{
							SendLightboxFeedbackLabel.SetErrorMessage("the following error occured:", ilex.Message);
						}
					}

					break;

				case "lightbox.makedefault":

					try
					{
						int currentLightboxId = CurrentLightboxId;
						int lightboxId = SetDefaultLightboxDropDownList.SelectedId;

						ContextInfo.LightboxManager.SetDefaultLightbox(lightboxId);
						FeedbackLabel1.SetSuccessMessage("default lightbox updated successfully");
						HideActionPanels();

						// Select the lightbox we were viewing before
						UserLightboxDropDownList1.SafeSelectValue(currentLightboxId);

						// Switch the selected lightbox in the persistent cart to this one
						SessionInfo.Current.UserSessionInfo.PersistentLightboxCartInfo.SelectedLightboxId = lightboxId;
					}
					catch (InvalidLightboxException ilex)
					{
						SetDefaultLightboxFeedbackLabel.SetErrorMessage(ilex.Message);
					}

					break;

					#endregion

				default:
					throw new ArgumentException("Unknown command", commandArgument);
			}
		}

		private int? GetAssetIdByControl(Control control)
		{
			int? assetId = null;

			foreach (RepeaterItem ri in LightboxAssetsRepeater.Items)
			{
				switch (ri.ItemType)
				{
					case (ListItemType.Item):
					case (ListItemType.AlternatingItem):

						Control cb = ri.FindControl(control.ID);

						if (cb != null && cb.UniqueID == control.UniqueID)
						{
							HiddenField AssetIdHiddenField = (HiddenField) ri.FindControl("AssetIdHiddenField");
							assetId = Convert.ToInt32(AssetIdHiddenField.Value);
						}

						break;
				}
			}

			return assetId;
		}

		/// <summary>
		/// Appends any selection in the CC dropdown list to the CC textbox
		/// </summary>
		private void AppendSelectionToCC()
		{
			if (!String.IsNullOrEmpty(SendLightboxCCDropDownList.SelectedValue))
			{
				string ccExtra = SendLightboxCCDropDownList.SelectedValue.Trim();

				//check that email address doesn't already exist in the list
				if (SendLightboxCCEmail.Text.IndexOf(ccExtra, StringComparison.OrdinalIgnoreCase) < 0)
				{
					//make sure email separator has been added if required
					if (SendLightboxCCEmail.Text.Trim().Length > 0 && (!SendLightboxCCEmail.Text.EndsWith(";") && !SendLightboxCCEmail.Text.EndsWith(",")))
					{
						SendLightboxCCEmail.Text += ";";
					}
					SendLightboxCCEmail.Text += ccExtra;
				}
				SendLightboxDropDownList.SelectedIndex = -1;
			}
		}

        // needed to persist unsaved notes
        private void SaveNotesToObject()
        {
            Dictionary<int, string> notes = new Dictionary<int, string>();
            foreach (RepeaterItem assetRow in LightboxAssetsRepeater.Items)
            {
                TextArea notesBox = (TextArea)assetRow.FindControl("NotesTextBox");
                int? assetId = GetAssetIdByControl(notesBox);
                if (assetId.HasValue)
                {
                    LightboxAsset currentItem = (from item in validItems
                                                 where item.AssetId == assetId.Value
                                                 select item).First();
                    currentItem.Notes = notesBox.Text;
                }
            }
        }
        #endregion
	}
}