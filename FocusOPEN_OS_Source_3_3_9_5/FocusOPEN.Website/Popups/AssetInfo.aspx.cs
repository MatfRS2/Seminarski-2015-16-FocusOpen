/**********************************************************************************************************************
	FocusOPEN Digital Attachedsset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;
using FocusOPEN.Website.Controls;

namespace FocusOPEN.Website
{
    public partial class AssetInfoPopup : BaseSecuredPage
    {
        #region Accessors

        private static SavedUserAssetSearch SavedUserAssetSearch
        {
            get
            {
                return SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch;
            }
        }


        private Asset CurrentAsset
        {
            get
            {
                Asset asset = Context.Items["Asset"] as Asset;

                if (asset == null)
                {
                    int assetId = WebUtils.GetIntRequestParam("AssetId", 0);
                    asset = Asset.Get(assetId);
                }

                return asset;
            }
        }

        protected override ContextType AssetContext
        {
            get
            {
                return SessionInfo.Current.AdminSessionInfo.AssetContext;
            }
        }


        #endregion

        #region Handled Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (CurrentAsset.IsNull)
                {
                    Page.ClientScript.RegisterClientScriptBlock(GetType(), "close", "self.close();", true);
                    return;
                }

                // Get the asset ID
                int assetId = CurrentAsset.AssetId.GetValueOrDefault();

                // Get the asset type info
                AssetTypeInfo assetType = AssetTypeInfo.Get(CurrentAsset.FileExtension);


                //-----------------------------------------------------------------------------------------------------
                // Set up UI elements based on asset type
                //-----------------------------------------------------------------------------------------------------
                OrientationRow.Visible = assetType.HasOrientation;
                DurationRow.Visible = assetType.HasDuration;
                DimensionsRow.Visible = assetType.HasDimensions;

                //-----------------------------------------------------------------------------------------------------
                // Set up asset breadcrumbs based on category
                //-----------------------------------------------------------------------------------------------------
                AssetBreadcrumb.CategoryId = CurrentAsset.PrimaryCategoryId;

                //-----------------------------------------------------------------------------------------------------
                // Update the audit log
                //-----------------------------------------------------------------------------------------------------
                AuditLogManager.LogAssetAction(assetId, CurrentUser, AuditAssetAction.ViewedAssetDetail);
                AuditLogManager.LogUserAction(CurrentUser, AuditUserAction.ViewedAssetDetail, string.Format("Viewed asset detail for AssetId: {0}", assetId));

                //-----------------------------------------------------------------------------------------------------
                // Initialise the asset preview and buttons
                //-----------------------------------------------------------------------------------------------------
                AssetPreview1.Asset = CurrentAsset;
                AssetButtons1.Initialise(CurrentAsset);

                //-----------------------------------------------------------------------------------------------------
                // Bind categories list
                //-----------------------------------------------------------------------------------------------------
                AssetCategoriesContainer.Visible = (CurrentAsset.CategoryList.Count > 0);
                CategoriesRepeater.DataSource = CurrentAsset.CategoryList;
                CategoriesRepeater.DataBind();

                //-----------------------------------------------------------------------------------------------------
                // Bind attached files
                //-----------------------------------------------------------------------------------------------------
                List<AssetFile> attachedFiles = CurrentAsset.GetAttachedFiles();
                AttachedFilesRow.Visible = (attachedFiles.Count > 0);
                AttachedFilesDataList.DataSource = attachedFiles;
                AttachedFilesDataList.DataBind();

                //-----------------------------------------------------------------------------------------------------
                // Bind linked assets
                //-----------------------------------------------------------------------------------------------------
				LinkedAssetsRow.Visible = (CurrentAsset.ReciprocalLinkedAssetList.Count > 0);
				LinkedAssetsRepeater.DataSource = CurrentAsset.ReciprocalLinkedAssetList;
                LinkedAssetsRepeater.DataBind();

                //-----------------------------------------------------------------------------------------------------
                // Set up the file type icon
                //-----------------------------------------------------------------------------------------------------
                FileTypeIconImage.ImageUrl = SiteUtils.GetFileTypeImageUrl(CurrentAsset.FileExtension);
                FileTypeIconImage.ToolTip = CurrentAsset.Filename;

                //-----------------------------------------------------------------------------------------------------
                // Brand controlled metadata
                //-----------------------------------------------------------------------------------------------------
                AssetIdCell.InnerText = CurrentAsset.AssetId.ToString();
                DateUploadedLabel.Text = CurrentAsset.UploadDate.ToString(Global.DateFormat);
                FilenameCell.InnerText = FileUtils.GetTruncatedFilename(CurrentAsset.Filename, 25);
                FileHashLabel.Text = StringUtils.IsBlank(CurrentAsset.FileHash) ? "[Not Available]" : CurrentAsset.FileHash.Substring(0, 15) + " ...";
                FileHashLabel.ToolTip = CurrentAsset.FileHash;
                FilesizeCell.InnerText = FileUtils.FriendlyFileSize(CurrentAsset.FileSize);
                AssetTypeCell.InnerText = CurrentAsset.AssetType.Name;
                DateProducedCell.InnerText = CurrentAsset.GetProductionDate();
                OriginatorCell.InnerText = CurrentAsset.Originator;
                TitleCell.InnerText = CurrentAsset.Title;
                ProjectCodeCell.InnerText = CurrentAsset.ProjectCode;
                BrandCell.InnerText = CurrentAsset.Brand.Name;
                
                AssetDescriptionContainer.InnerHtml = SiteUtils.ConvertTextToHtml(CurrentAsset.Description);
                AdditionalKeywordsCell.InnerText = CurrentAsset.Keywords;
                CopyrightOwnerCell.InnerHtml = StringUtils.IgnoreCaseCompare(CurrentAsset.CopyrightOwner, WebsiteBrandManager.GetBrand().OrganisationName) ? "(c) " + CurrentAsset.CopyrightOwner : CurrentAsset.CopyrightOwner;
                UsageRestrictionsCell.InnerHtml = CurrentAsset.UsageRestrictions;
                ContactEmailHyperLink.EmailAddress = CurrentAsset.ContactEmail;
                PublicationDateCell.InnerText = CurrentAsset.PublishDate.ToString(Global.DateFormat);
                ExpiryDateCell.InnerText = CurrentAsset.ExpiryDate.ToString(Global.DateFormat);

                // order metas according to their meta settings asset detail order numbers
                TrBrand.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.BRAND).AssetDetailOrderNum.ToString();
                TrAssetType.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.ASSET_TYPE).AssetDetailOrderNum.ToString();
                TrFilename.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.FILENAME).AssetDetailOrderNum.ToString();
                TrFileSize.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.FILESIZE).AssetDetailOrderNum.ToString();
                TrFileHash.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.FILEHASH).AssetDetailOrderNum.ToString();
                TrDateUploaded.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_UPLOADED).AssetDetailOrderNum.ToString();
                TrTitle.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.TITLE).AssetDetailOrderNum.ToString();
                TrProjectCode.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.PROJECT_CODE).AssetDetailOrderNum.ToString();
                TrOriginator.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.ORIGINATOR).AssetDetailOrderNum.ToString();
                TrDateProduced.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.DATE_PRODUCED).AssetDetailOrderNum.ToString();
                TrContactEmail.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.CONTACT_EMAIL).AssetDetailOrderNum.ToString();
                TrCopyrightOwner.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.COPYRIGHT_OWNER).AssetDetailOrderNum.ToString();
                TrRestrictedDownload.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.DOWNLOAD_RESTRICTIONS).AssetDetailOrderNum.ToString();
                TrUsageRestrictions.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.USAGE_RESTRICTIONS).AssetDetailOrderNum.ToString();
                TrAdditionalKeywords.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.ADDITIONAL_KEYWORDS).AssetDetailOrderNum.ToString();
                TrPublicationDate.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.PUBLICATION_DATE).AssetDetailOrderNum.ToString();
                TrExpiryDate.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.EXPIRY_DATE).AssetDetailOrderNum.ToString();
                LinkedAssetsRow.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.LINKED_ASSETS).AssetDetailOrderNum.ToString();
                AttachedFilesRow.Attributes["class"] = CurrentAsset.Brand.GetMetadataSetting(BrandMetadataSettings.ATTACHED_FILES).AssetDetailOrderNum.ToString();

                var allSettings = BrandMetadataSettingManager.GetCustomMetadataSettings(CurrentAsset.Brand.BrandId.GetValueOrDefault());
                var lastNum = allSettings.OrderBy(s => s.AssetDetailOrderNum).LastOrDefault().AssetDetailOrderNum;

                DimensionsRow.Attributes["class"]   = (lastNum++).ToString();
                DurationRow.Attributes["class"]     = (lastNum++).ToString();
                OrientationRow.Attributes["class"]  = (lastNum++).ToString(); 
                
                //-----------------------------------------------------------------------------------------------------
                // Other stuff
                //-----------------------------------------------------------------------------------------------------
                OrientationCell.InnerText = CurrentAsset.GetOrientation();
                DurationCell.InnerText = SiteUtils.FriendlyDuration(CurrentAsset.Duration.GetValueOrDefault(), "Unknown");
                DimensionsCell.InnerText = CurrentAsset.GetDimensions();
                RestrictedDownloadCell.InnerText = EntitySecurityManager.IsAssetRestricted(CurrentUser, CurrentAsset) ? "Yes" : "No";

                // Only show file metadata link if we have some available
                // ShowFileMeta data returns true if there is File metadata to display.
				FileMetadataLinkButton.Visible = ShowFileMetadata();
            	FileMetadataLinkDivider.Visible = FileMetadataLinkButton.Visible;

                //-----------------------------------------------------------------------------------------------------
                // Setup security in UI: Only display the edit and status links of the user has access
                //-----------------------------------------------------------------------------------------------------
                if (EntitySecurityManager.CanManageAsset(CurrentUser, CurrentAsset))
                {
                    string editUrl = "~/Admin/Assets/AssetForm.aspx?assetId=" + CurrentAsset.AssetId;
                    EditHyperLink.NavigateUrl = editUrl;
                    EditHyperLink.Attributes.Add("onClick", string.Format("GetParentWindow().location.href='{0}';self.close();return false;", ResolveUrl(editUrl)));

                    string statsUrl = "~/Admin/Reports/AssetStats.aspx?assetId=" + assetId;
                    StatsHyperLink.NavigateUrl = statsUrl;
                    StatsHyperLink.Attributes.Add("onClick", string.Format("GetParentWindow().location.href='{0}';self.close();return false;", ResolveUrl(statsUrl)));

                    string logHyperlink = "~/Admin/Reports/AssetAuditTrail.aspx?AssetId=" + CurrentAsset.AssetId;
                    LogHyperLink.NavigateUrl = logHyperlink;
                    LogHyperLink.Attributes.Add("onClick", string.Format("GetParentWindow().location.href='{0}';self.close();return false;", ResolveUrl(logHyperlink)));
                }
                else
                {
                    AssetLinksContainer.Visible = false;
                }

                //-----------------------------------------------------------------------------------------------------
                // Control access or AssetOrderHistory links.
                //-----------------------------------------------------------------------------------------------------
                if (!EntitySecurityManager.CanViewAssetOrderHistory(CurrentUser, CurrentAsset))
                {
                    // Get the asset order history, if there isn't any order history
                    // then hide the link.
                    // Set visiblity of order history link
                    OrderHistoryDivider.Visible = false;
                    OrderHistoryLinkButton.Visible = false;
                }
                else
                {
                    // Get the asset order history, if there isn't any order history
                    // then hide the link.
                    if (!ShowOrderHistory())
                    {
                        // Set visiblity of order history link
                        OrderHistoryDivider.Visible = false;
                        OrderHistoryLinkButton.Visible = false;
                    }
                }

                //-----------------------------------------------------------------------------------------------------
                // Populate blank cells
                //-----------------------------------------------------------------------------------------------------
                SiteUtils.PopulateBlankControl(AssetDescriptionContainer);
                SiteUtils.PopulateBlankControl(FilenameCell);
                SiteUtils.PopulateBlankControl(FilesizeCell);
                SiteUtils.PopulateBlankControl(TitleCell);
                SiteUtils.PopulateBlankControl(DurationCell);
                SiteUtils.PopulateBlankControl(ProjectCodeCell);
                SiteUtils.PopulateBlankControl(OriginatorCell);
                SiteUtils.PopulateBlankControl(DateProducedCell);
                SiteUtils.PopulateBlankControl(CopyrightOwnerCell);
                SiteUtils.PopulateBlankControl(OrientationCell);
                SiteUtils.PopulateBlankControl(UsageRestrictionsCell);
            }

            //sort metas on every request
            ClientScript.RegisterStartupScript(GetType(), "SortMetas2", "SortMetas();", true);

            //bind repeater on every request as otherwise labels stuff is lost
            MetadataRepeater.DataSource = BrandMetadataSettingManager.GetCustomMetadataSettings(CurrentAsset.BrandId);
            MetadataRepeater.DataBind();
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string prefix = (AssetFeedbackFormPanel.Visible) ? "-" : "+";
            AssetFeedbackFormToggleLink.Text = string.Format("[{0}] Feedback about this asset?", prefix);
        }

        protected void MetadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var setting = (BrandMetadataSetting)e.Item.DataItem;

                var metadataVisibilityWrapper = (MetadataVisibilityWrapper)e.Item.FindControl("MetadataVisibilityWrapper");
                var cell = (HtmlTableCell)e.Item.FindControl("Metadata_Cell");
                var brandMetadataLabel = (BrandMetadataLabel)e.Item.FindControl("BrandMetadataLabel");
                var trRepeaterMeta = (HtmlTableRow)e.Item.FindControl("TrRepeaterMeta");

                //set order number
                trRepeaterMeta.Attributes["class"] = setting.AssetDetailOrderNum.ToString();

                //set field id for the wrapper and label
                metadataVisibilityWrapper.FieldName = brandMetadataLabel.FieldName = setting.FieldId;

				if (setting.UiControlType == (int)BrandMetadataUiControlType.TextArea ||
					setting.UiControlType == (int)BrandMetadataUiControlType.TextField ||
					setting.SelectableSetting.SelectableType == (int)SelectableMetadataType.PresetTextArea)
				{
					cell.InnerText = CurrentAsset.GetStringValForGroup(setting.GroupNumber);
				}
				else if (setting.UiControlType == (int)BrandMetadataUiControlType.Select)
				{
					cell.InnerText = CurrentAsset.GetMetadataForGroup(setting.GroupNumber).Select(m => m.Name).ToCommaDelimitedList().EmptyValue("Not Specified");
				}
            }
        }

        protected void CategoriesRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            Breadcrumb_SelectCategory(source, e);
        }

        protected void CategoriesRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
            {
                // Get the category
                Category category = (Category)e.Item.DataItem;

                // Get the link button
                LinkButton categoryLinkButton = (LinkButton)e.Item.FindControl("CategoryLinkButton");

                if (!category.ParentCategory.IsNull)
                {
                    categoryLinkButton.CommandArgument = category.CategoryId.ToString();
                    categoryLinkButton.Text = category.Name;
                    categoryLinkButton.ToolTip = SiteUtils.GetFullCategoryName(category);
                    categoryLinkButton.Visible = true;
                }

            }
        }

        protected void AssetButtons_Click(object sender, EventArgs e)
        {
            // Force the opener to reload.  We're not using GetParentWindow().location.reload(true)
            // as this causes some strange behaviour, and also displays a 'repost form data' dialogue
            // box in the browser which is not necessary

            const string script = "if (window.opener) GetParentWindow().location.href=GetParentWindow().location.href;";
            Page.ClientScript.RegisterStartupScript(GetType(), "reloadscript", script, true);
        }

        protected void Breadcrumb_SelectCategory(object sender, RepeaterCommandEventArgs e)
        {
            int categoryId = int.Parse(e.CommandArgument.ToString());

            SavedUserAssetSearch.CategoriesOpen = true;
            SavedUserAssetSearch.FilterOpen = false;
            SavedUserAssetSearch.AssetFinder.GeneralKeyword = string.Empty;
            SavedUserAssetSearch.SelectCategory(categoryId);

            string script = string.Format("window.onload = opener.document.location.href='{0}'; self.close()", Page.ResolveUrl("~/SearchResults.aspx"));
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "return", script, true);
        }

        protected void AttachedFilesDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
            {
                AssetFile file = (AssetFile)e.Item.DataItem;

				HyperLink DownloadAttachedFileHyperLink = (HyperLink)e.Item.FindControl("DownloadAttachedFileHyperLink");
                DownloadAttachedFileHyperLink.Text = FileUtils.GetTruncatedFilename(file.Filename, 25);
            	DownloadAttachedFileHyperLink.NavigateUrl = string.Format("~/AttachedFile.{0}.ashx?AssetId={1}", file.AssetFileId, file.AssetId);
                DownloadAttachedFileHyperLink.Enabled = (!CurrentAsset.RestrictAttachedFiles || EntitySecurityManager.CanUserDownloadAsset(CurrentUser, CurrentAsset));
            }
        }

        protected void AssetFileMetadataRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (GeneralUtils.ValueIsInList(e.Item.ItemType, ListItemType.Item, ListItemType.AlternatingItem))
            {
                KeyValuePair<string, string> kvp = (KeyValuePair<string, string>)e.Item.DataItem;

                Literal MetadataNameLiteral = (Literal)e.Item.FindControl("MetadataNameLiteral");
                MetadataNameLiteral.Text = kvp.Key;

                Literal MetadataValueLiteral = (Literal)e.Item.FindControl("MetadataValueLiteral");
                MetadataValueLiteral.Text = kvp.Value;
            }
        }

        protected bool ShowFileMetadata()
        {
			if (!WebsiteBrandManager.GetBrand().GetMetadataSetting(BrandMetadataSettings.SHOW_FILE_METADATA).OnAssetDetail)
				return false;

            var metadataList = CurrentAsset.GetFileMetadata(true);

            if (metadataList.Count > 0)
            {
                AssetFileMetadataRepeater.DataSource = metadataList;
                AssetFileMetadataRepeater.DataBind();
                return true;
            }
            
			return false;
        }

    	private bool ShowOrderHistory()
        {
            List<AssetOrderHistory> orderHistory = CurrentAsset.OrderHistory;

            if (orderHistory.Count > 0)
            {
                OrderHistoryListView.DataSource = orderHistory;
                OrderHistoryListView.DataBind();
                return true;
            }
            
			// Signal that we don't need to show the order history link.
			return false;
        }

        protected void ToggleOrderDetailButton_Click(object sender, EventArgs e)
        {
        	LinkButton link = (LinkButton) sender;
            Panel panel = link.FindControl("OrderHistoryDetailPanel") as Panel;
            
			if (panel != null)
			{
				panel.Visible = (!panel.Visible);
				link.Text = panel.Visible ? "[-] hide order details" : "[+] show order details";
			}
        }

        protected void PanelChoiceCommand_Click(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "ShowMetadata":
                    MetaDataLinkButton.CssClass = "NoDecoration Black Bold BodyTxt";
                    FileMetadataLinkButton.CssClass = "Black BodyTxt";
                    OrderHistoryLinkButton.CssClass = "Black BodyTxt";
                    AssetMetadataPanel.Visible = true;
                    AssetFileMetadataPanel.Visible = false;
                    AssetOrderHistoryPanel.Visible = false;
                    break;

                case "ShowFileMetadata":
                    MetaDataLinkButton.CssClass = "Black BodyTxt";
                    FileMetadataLinkButton.CssClass = "NoDecoration Black Bold BodyTxt";
                    OrderHistoryLinkButton.CssClass = "Black BodyTxt";
                    AssetMetadataPanel.Visible = false;
                    AssetFileMetadataPanel.Visible = true;
                    AssetOrderHistoryPanel.Visible = false;
                    break;

                case "ShowOrderHistory":
                    MetaDataLinkButton.CssClass = "Black BodyTxt";
                    FileMetadataLinkButton.CssClass = "Black BodyTxt";
                    OrderHistoryLinkButton.CssClass = "NoDecoration Black Bold BodyTxt";
                    AssetMetadataPanel.Visible = false;
                    AssetFileMetadataPanel.Visible = false;
                    AssetOrderHistoryPanel.Visible = true;
                    break;
            }
        }

        protected void DeleteLinkButton_Click(object sender, EventArgs e)
        {
            AssetManager.DeleteAsset(CurrentUser, CurrentAsset.AssetId.GetValueOrDefault());
            Page.ClientScript.RegisterClientScriptBlock(GetType(), "close", "self.close();", true);
        }

        protected void LinkedAssetsRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Asset linkedAsset = (Asset)e.Item.DataItem;

                if (!linkedAsset.IsNull)
                {
                    LinkButton LinkedAssetTitleLinkButton = (LinkButton)e.Item.FindControl("LinkedAssetTitleLinkButton");
                    LinkedAssetTitleLinkButton.Text = linkedAsset.Title + " (" + linkedAsset.AssetId + ")";
                    LinkedAssetTitleLinkButton.OnClientClick = (linkedAsset.IsPublishedAndAvailable()) ? SiteUtils.GetShowAssetInfoUrl(linkedAsset.AssetId.GetValueOrDefault()) : "javascript:alert('Asset not published yet'); return false;";
                }
            }
        }

        protected void AssetFeedbackFormToggleLink_Click(object sender, EventArgs e)
        {
            AssetFeedbackFormPanel.Visible = (!AssetFeedbackFormPanel.Visible);
        }

        protected void AssetFeedbackCancelButton_Click(object sender, EventArgs e)
        {
            AssetFeedbackTextBox.Text = string.Empty;
            AssetFeedbackFormPanel.Visible = false;
        }

        protected void AssetFeedbackSendButton_Click(object sender, EventArgs e)
        {
            if (StringUtils.IsBlank(AssetFeedbackTextBox.Text))
                return;

            NotifyEngine.SendAssetFeedback(CurrentAsset, CurrentUser, AssetFeedbackTextBox.Text);

            AssetFeedbackTextBox.Text = string.Empty;
            AssetFeedbackFormPanel.Visible = false;

            ScriptManager.RegisterStartupScript(Page, GetType(), "Msg", "alert('Thank you for your feedback')", true);
        }

        protected void OrderHistoryListView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
				AssetOrderHistory orderItem = (AssetOrderHistory)((ListViewDataItem)e.Item).DataItem;

            	// Sort out the ordered by display, if the current user is admin show as link, otherwise just a label.
                LinkButton lnkBtn = e.Item.FindControl("UserNameLinkButton") as LinkButton;
                Label lbl = e.Item.FindControl("OrderByValueLabel") as Label;

                if (CurrentUser.UserRole == UserRole.SuperAdministrator)
                {
                    if (lnkBtn != null)
                    {
                        lnkBtn.OnClientClick = String.Format("opener.location.href = ResolveUrl('~/Admin/Users/UserForm.aspx?userId={0}');javascript:self.close();", orderItem.UserId);
                        lnkBtn.Text = orderItem.User.FullName;
                        lnkBtn.Visible = true;
                    }
                    if (lbl != null)
						lbl.Visible = false;
                }
                else
                {
                    if (lbl != null)
                    {
                        lbl.Text = orderItem.User.FullName;
                        lbl.Visible = true;
                    }
                    if (lnkBtn != null)
						lnkBtn.Visible = false;
                }

                lbl = e.Item.FindControl("OrderDateValueLabel") as Label;
                if (lbl != null) lbl.Text = DateHelper.GetDateString(orderItem.OrderDate, "N/A");

                lbl = e.Item.FindControl("DeadlineDateValueLabel") as Label;
                if (lbl != null) lbl.Text = DateHelper.GetDateString(orderItem.DeadlineDate, "N/A");

                lbl = e.Item.FindControl("NotesValueLabel") as Label;
                if (lbl != null)
                    lbl.Text = (orderItem.Notes != String.Empty? orderItem.Notes: "Not specified");

                lbl = e.Item.FindControl("ApprovedYesNoValueLabel") as Label;
                if (lbl != null) lbl.Text = orderItem.ApprovedYesNo;

                lbl = e.Item.FindControl("OrderItemStatusDateValueLabel") as Label;
                if (lbl != null) lbl.Text = DateHelper.GetDateString(orderItem.OrderItemStatusDate, "N/A");

                Panel pnl = e.Item.FindControl("ViewOrderPanel") as Panel;
				
				if (pnl != null)
				{
					if (GeneralUtils.ValueIsInList(CurrentUser.UserRole, UserRole.SuperAdministrator, UserRole.BrandAdministrator))
					{
						lnkBtn = e.Item.FindControl("ViewOrderLink") as LinkButton;
						if (lnkBtn != null)
						{
							lnkBtn.OnClientClick = string.Format("opener.location.href = ResolveUrl('~/Admin/Orders/OrderForm.aspx?orderId={0}');javascript:self.close();", orderItem.OrderId);
						}
						pnl.Visible = true;
					}
					else
					{
						pnl.Visible = false;
					}
				}
            }
        }

        #endregion
    }
}