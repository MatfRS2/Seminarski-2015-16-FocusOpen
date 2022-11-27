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
using Daydream.Data;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Admin
{
	public partial class WorkflowComment : BaseAdminWorkflowPage
	{
		#region Private Variables

		private AssetWorkflowCommenter m_AssetWorkflowCommenter;

		#endregion

		#region Accessors

		public AssetWorkflowCommenter AssetWorkflowCommenter
		{
			get
			{
				if (m_AssetWorkflowCommenter == null)
				{
					// Initialise the finder
					AssetWorkflowCommenterFinder finder = new AssetWorkflowCommenterFinder();

					// Find comments for this workflow and user
					finder.AssetWorkflowId = AssetWorkflowId;
					finder.UserId = CurrentUser.UserId.GetValueOrDefault();

					// Order by descending so that we only get the newest record, in case this
					// commenter has been invited more than once.
					finder.SortExpressions.Add(new DescendingSort(AssetWorkflowCommenter.Columns.CreateDate));

					// Get it
					m_AssetWorkflowCommenter = AssetWorkflowCommenter.FindOne(finder);
				}

				return m_AssetWorkflowCommenter;
			}
		}

		#endregion

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (AssetWorkflowCommenter.IsNull)
					Response.Redirect("~/Admin/Assets/AssetList.aspx");

				if (!StringUtils.IsBlank(AssetWorkflowCommenter.Comments))
				{
					MessageLabel.SetErrorMessage("You have already commented on this workflow");
					MessageLabel.Pinned = true;
					DisableUI();
				}

				if (AssetWorkflow.IsComplete)
				{
					MessageLabel.SetErrorMessage("Asset workflow is complete and comments have been disabled");
					MessageLabel.Pinned = true;
					DisableUI();
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
				InvitingUserName.Text = AssetWorkflowCommenter.InvitingUser.FullName;
				InvitingUserMessageCell.InnerText = AssetWorkflowCommenter.InvitingUserMessage;
				ResponseTextArea.Text = AssetWorkflowCommenter.Comments;

				SiteUtils.SetHyperLink("Assets.AssetForm", string.Format("?assetId={0}", asset.AssetId), EditAssetHyperLink);

				AssetWorkflowLog.Bind(AssetWorkflow.Asset);
			}
		}

		protected void ProcessButton_Click(object sender, EventArgs e)
		{
			AssetWorkflowCommenter.Comments = ResponseTextArea.Text.Trim();
			AssetWorkflowCommenter.LastUpdate = DateTime.Now;

			try
			{
				WorkflowManager.SaveAssetWorkflowCommenter(AssetWorkflowCommenter);
				MessageLabel.SetSuccessMessage("Comments saved successfully");
				DisableUI();
				AssetWorkflowLog.Bind(AssetWorkflow.Asset);
			}
			catch (ValidationException vex)
			{
				MessageLabel.SetErrorMessage(vex.Message);
			}
		}

		private void DisableUI()
		{
			ResponseTextArea.Enabled = false;
			ProcessButton.Visible = false;
			CancelButton.Text = "Admin Home";
			CancelButton.Prompt = string.Empty;
		}
	}
}