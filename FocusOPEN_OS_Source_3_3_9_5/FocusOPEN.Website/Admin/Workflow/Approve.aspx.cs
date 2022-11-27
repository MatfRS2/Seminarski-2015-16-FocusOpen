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
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class WorkflowApprove : BaseAdminWorkflowPage
	{
		#region Private Variables

		private AssetWorkflowUser m_AssetWorkflowUser;

		#endregion

		#region Accessors

		public AssetWorkflowUser AssetWorkflowUser
		{
			get
			{
				if (m_AssetWorkflowUser == null)
				{
					AssetWorkflowUserFinder finder = new AssetWorkflowUserFinder();
					finder.AssetWorkflowId = AssetWorkflowId;
					finder.UserId = CurrentUser.UserId.GetValueOrDefault();
					m_AssetWorkflowUser = AssetWorkflowUser.FindOne(finder);
				}

				return m_AssetWorkflowUser;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				ApproveRadioButton.Attributes.Add("onClick", "checkRadio()");
				RejectRadioButton.Attributes.Add("onClick", "checkRadio()");
				PendingRadioButton.Attributes.Add("onClick", "checkRadio()");
				InviteCommentsRadioButton.Attributes.Add("onClick", "checkRadio()");

				if (AssetWorkflowUser.IsNull)
					Response.Redirect("~/Admin/Assets/AssetList.aspx");

				if (AssetWorkflow.IsComplete)
				{
					MessageLabel.SetErrorMessage("Asset workflow is complete and comments have been disabled");
					MessageLabel.Pinned = true;
					DisableUI();
				}
				else
				{
					if (AssetWorkflowUser.AssetWorkflowUserStatus == AssetWorkflowUserStatus.Waiting)
					{
						MessageLabel.SetErrorMessage("You do not have permission to action this workflow yet");
						MessageLabel.Pinned = true;
						DisableUI();
					}
					else if (AssetWorkflowUser.AssetWorkflowUserStatus != AssetWorkflowUserStatus.Pending)
					{
						MessageLabel.SetErrorMessage("You have already actioned this workflow");
						MessageLabel.Pinned = true;
						DisableUI();
					}
				}

				AssetWorkflowUserList.Repeater.DataSource = AssetWorkflow.AssetWorkflowUserList;
				AssetWorkflowUserList.Repeater.DataBind();

				Asset asset = AssetWorkflow.Asset;

				AssetReferenceLiteral.Text = asset.AssetId.ToString();
				UploadUserEmailHyperLink.SetNameEmail(asset.UploadedByUser.FullName, asset.UploadedByUser.Email);
				PublicationDateLiteral.Text = asset.PublishDate.ToString(Global.DateFormat);
				DownloadAssetHyperLink.NavigateUrl = AssetFileUrlHelper.GetDownloadUrl(asset.AssetId);
				AssetThumbnail.Initialise(asset);
				AssetButtons1.Initialise(asset);
				AssetTypeCell.InnerText = asset.AssetType.Name;
				BrandCell.InnerText = asset.BrandName;
				ShortDescriptionCell.InnerHtml = SiteUtils.ConvertTextToHtml(asset.Description);
				UsageRestrictionsCell.InnerText = asset.UsageRestrictions;
				ResponseTextArea.Text = AssetWorkflowUser.Comments;

				SiteUtils.SetHyperLink("Assets.AssetForm", string.Format("?assetId={0}", asset.AssetId), EditAssetHyperLink);

				switch (AssetWorkflowUser.AssetWorkflowUserStatus)
				{
					case (AssetWorkflowUserStatus.Pending):

						PendingRadioButton.Checked = true;
						break;

					case (AssetWorkflowUserStatus.Approved):

						ApproveRadioButton.Checked = true;
						break;

					case (AssetWorkflowUserStatus.Rejected):

						RejectRadioButton.Checked = true;
						break;
				}

				AssetWorkflowLog.Bind(asset);
			}
		}

		protected void ProcessButton_Click(object sender, EventArgs e)
		{
			try
			{
				if (InviteCommentsRadioButton.Checked)
				{
					WorkflowManager.InviteUserToWorkflow(AssetWorkflowUser, InviteCommentsTextBox.Text, ResponseTextArea.Text);
					MessageLabel.SetSuccessMessage("Invitation sent successfully");
				}
				else
				{
					AssetWorkflowUserStatus assetWorkflowUserStatus;

					if (ApproveRadioButton.Checked)
					{
						assetWorkflowUserStatus = AssetWorkflowUserStatus.Approved;
					}
					else if (RejectRadioButton.Checked)
					{
						assetWorkflowUserStatus = AssetWorkflowUserStatus.Rejected;
					}
					else
					{
						assetWorkflowUserStatus = AssetWorkflowUserStatus.Pending;
					}

					AssetWorkflowUser.AssetWorkflowUserStatus = assetWorkflowUserStatus;
					AssetWorkflowUser.Comments = ResponseTextArea.Text.Trim();
					AssetWorkflowUser.LastUpdate = DateTime.Now;

					WorkflowManager.SaveWorkflowUserAndAdvance(AssetWorkflowUser);

					switch (assetWorkflowUserStatus)
					{
						case (AssetWorkflowUserStatus.Approved):
						case (AssetWorkflowUserStatus.Rejected):

							MessageLabel.SetSuccessMessage("Your comments have been saved.");
							DisableUI();
							break;

						case (AssetWorkflowUserStatus.Pending):

							MessageLabel.SetSuccessMessage("Comments saved. Please approve or reject this asset to advance the workflow.");
							break;
					}
				}

				AssetWorkflowLog.Bind(AssetWorkflow.Asset);
			}
			catch (BaseException bex)
			{
				MessageLabel.SetErrorMessage(bex.Message);
			}
		}

		#region Private Helper Methods

		private void DisableUI()
		{
			PendingRadioButton.Enabled = false;
			RejectRadioButton.Enabled = false;
			ApproveRadioButton.Enabled = false;
			InviteCommentsRadioButton.Enabled = false;
			InviteCommentsTextBox.Enabled = false;
			ResponseTextArea.Enabled = false;

			ProcessButton.Visible = false;
			CancelButton.Text = "View asset list";
			CancelButton.Prompt = string.Empty;
		}

		#endregion
	}
}