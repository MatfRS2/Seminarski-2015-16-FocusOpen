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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls.Admin
{
	public partial class AssetList : BaseUserControl
	{
		public CommandEventHandler Delete;
		public CommandEventHandler SortChanged;

		public SortableRepeater Repeater
		{
			get
			{
				EnsureChildControls();
				return AssetsRepeater;
			}
		}

		public bool AllowDelete
		{
			set
			{
				ViewState["AllowDelete"] = value;
			}
			get
			{
				return SiteUtils.GetFromStore(ViewState, "AllowDelete", true);
			}
		}

		public bool ShowCheckBoxes
		{
			set
			{
				ViewState["ShowCheckBoxes"] = value;
			}
			get
			{
				return SiteUtils.GetFromStore(ViewState, "ShowCheckBoxes", false);
			}
		}

		public bool ShowThumbnails
		{
			set
			{
				ViewState["ShowThumbnails"] = value;
			}
			get
			{
				return SiteUtils.GetFromStore(ViewState, "ShowThumbnails", false);
			}
		}

		protected void SortListLinkButton_Command(object sender, CommandEventArgs e)
		{
			if (SortChanged != null)
				SortChanged(sender, e);
		}

		protected void DeleteLinkButton_OnCommand(object sender, CommandEventArgs e)
		{
			if (Delete != null)
				Delete(sender, e);
		}

		protected void AssetsRepeater_OnItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case ListItemType.Header:

					HtmlTableCell CheckBoxHeaderCell = (HtmlTableCell)e.Item.FindControl("CheckBoxHeaderCell");
					CheckBoxHeaderCell.Visible = ShowCheckBoxes;

					HtmlTableCell ThumbnailHeaderCell = (HtmlTableCell)e.Item.FindControl("ThumbnailHeaderCell");
					ThumbnailHeaderCell.Visible = ShowThumbnails;

					break;

				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					Asset asset = (Asset)e.Item.DataItem;

					HtmlTableCell CheckBoxCell = (HtmlTableCell)e.Item.FindControl("CheckBoxCell");
					CheckBoxCell.Visible = ShowCheckBoxes;

					HtmlTableCell ThumbnailCell = (HtmlTableCell)e.Item.FindControl("ThumbnailCell");
					ThumbnailCell.Visible = ShowThumbnails;

					if (ThumbnailCell.Visible)
					{
						AssetThumbnail assetThumbnail = (AssetThumbnail)e.Item.FindControl("AssetThumbnail");
						assetThumbnail.Initialise(asset);
					}

					HiddenField AssetIdHiddenField = (HiddenField)e.Item.FindControl("AssetIdHiddenField");
					AssetIdHiddenField.Value = asset.AssetId.ToString();

					HyperLink AssetIdHyperLink = (HyperLink)e.Item.FindControl("AssetIdHyperLink");
					AssetIdHyperLink.Text = asset.AssetId.ToString();
					AssetIdHyperLink.NavigateUrl = string.Format("~/Admin/Assets/AssetForm.aspx?assetId={0}&source=assetlist", asset.AssetId);

					Label AssetTypeLabel = (Label)e.Item.FindControl("AssetTypeLabel");
					AssetTypeLabel.Text = asset.AssetType.Name;

					Label UploadedByUserNameLabel = (Label)e.Item.FindControl("UploadedByUserNameLabel");
					UploadedByUserNameLabel.Text = asset.UploadedByUser.FullName;

					Label TitleLabel = (Label)e.Item.FindControl("TitleLabel");
					TitleLabel.Text = asset.Title;
					SiteUtils.PopulateBlankControl(TitleLabel);

					Label FileSizeLabel = (Label)e.Item.FindControl("FileSizeLabel");
					FileSizeLabel.Text = FileUtils.FriendlyFileSize(asset.FileSize);

					Label ProductionDateLabel = (Label)e.Item.FindControl("ProductionDateLabel");
					ProductionDateLabel.Text = asset.GetProductionDate();
					SiteUtils.PopulateBlankControl(ProductionDateLabel);

					Label IsExpiredLabel = (Label)e.Item.FindControl("IsExpiredLabel");
					IsExpiredLabel.Text = (asset.ExpiryDate < DateTime.Now) ? "Y" : "N";

					if (IsExpiredLabel.Text == "Y")
						IsExpiredLabel.CssClass = "bold";

					Label ExpiryDateLabel = (Label)e.Item.FindControl("ExpiryDateLabel");
					ExpiryDateLabel.Text = asset.ExpiryDate.ToString(Global.DateFormat);

					HyperLink IsPublishedHyperLink = (HyperLink)e.Item.FindControl("IsPublishedHyperLink");
					IsPublishedHyperLink.Text = asset.AssetPublishStatusName;

					PromptLinkButton DeleteLinkButton = (PromptLinkButton)e.Item.FindControl("DeleteLinkButton");
					DeleteLinkButton.CommandArgument = asset.AssetId.ToString();

					EnhancedHyperLink AuditLogHyperLink = (EnhancedHyperLink)e.Item.FindControl("AuditLogHyperLink");
					SiteUtils.SetHyperLink("Reports.AssetAuditTrail", string.Format("?AssetId={0}&Source=AssetList", asset.AssetId), AuditLogHyperLink);

					switch (asset.AssetPublishStatus)
					{
						case (AssetPublishStatus.Published):

							IsPublishedHyperLink.CssClass += " Bold";
							break;

						case (AssetPublishStatus.PendingApproval):

							IsPublishedHyperLink.CssClass += " Bold";

							if (asset.AssetWorkflowList.Count > 0)
							{
								AssetWorkflow aw = asset.AssetWorkflowList[0];

								// Only link the pending text to the approval screen if the current user is in the workflow

								AssetWorkflowUser cawfu = aw.AssetWorkflowUserList.Find(awfu => awfu.UserId == CurrentUser.UserId.GetValueOrDefault());

								if (cawfu != null)
								{
									SiteUtils.SetHyperLink("Workflow.Approve", string.Format("?AssetWorkflowId={0}", aw.AssetWorkflowId), IsPublishedHyperLink);
								}
								else
								{
									AssetWorkflowCommenter awc = aw.AssetWorkflowCommenterList.Find(awfc => awfc.UserId == CurrentUser.UserId.GetValueOrDefault());

									if (awc != null)
										SiteUtils.SetHyperLink("Workflow.Comment", string.Format("?AssetWorkflowId={0}", aw.AssetWorkflowId), IsPublishedHyperLink);
								}
							}

							break;
					}

					break;
			}
		}
	}
}