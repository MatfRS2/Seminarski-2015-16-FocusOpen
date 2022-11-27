/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public partial class AssetWorkflowLog : BaseUserControl
	{
		public void Bind(Asset asset)
		{
			Bind(WorkflowManager.GetWorkflowInfo(asset).WorkflowInfoItemList);
		}

		public void Bind(List<WorkflowInfoItem> items)
		{
			WorkflowInfoRepeater.DataSource = items;
			WorkflowInfoRepeater.DataBind();
		}

		protected void WorkflowInfoRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
		{
			switch (e.Item.ItemType)
			{
				case (ListItemType.Item):
				case (ListItemType.AlternatingItem):

					WorkflowInfoItem wii = (WorkflowInfoItem)e.Item.DataItem;

					if (wii.IsComment)
					{
						if (StringUtils.IsBlank(wii.Comments))
						{
							PlaceHolder EmptyCommentsPlaceHolder = (PlaceHolder)e.Item.FindControl("EmptyCommentsPlaceHolder");
							EmptyCommentsPlaceHolder.Visible = true;

							EmailHyperLink EmptyCommentsInvitedUserHyperLink = (EmailHyperLink) e.Item.FindControl("EmptyCommentsInvitedUserHyperLink");
							EmptyCommentsInvitedUserHyperLink.SetNameEmail(wii.AssetWorkflowCommenter.User.FullName, wii.AssetWorkflowCommenter.User.Email);

							EmailHyperLink EmptyCommentsInvitingUserHyperLink = (EmailHyperLink) e.Item.FindControl("EmptyCommentsInvitingUserHyperLink");
							EmptyCommentsInvitingUserHyperLink.SetNameEmail(wii.AssetWorkflowCommenter.InvitingUser.FullName, wii.AssetWorkflowCommenter.InvitingUser.Email);

							Literal EmptyCommentsDateLiteral = (Literal) e.Item.FindControl("EmptyCommentsDateLiteral");
							EmptyCommentsDateLiteral.Text = wii.AssetWorkflowCommenter.CreateDate.ToString(Global.DateFormat);

							InitialiseComments(e.Item, "EmptyComments", wii.AssetWorkflowCommenter.InvitingUserMessage);
						}
						else
						{
							PlaceHolder CommentsPlaceHolder = (PlaceHolder) e.Item.FindControl("CommentsPlaceHolder");
							CommentsPlaceHolder.Visible = true;

							EmailHyperLink CommentsUserHyperLink = (EmailHyperLink) e.Item.FindControl("CommentsUserHyperLink");
							CommentsUserHyperLink.SetNameEmail(wii.UserName, wii.UserEmail);

							Literal CommentsDateLiteral = (Literal) e.Item.FindControl("CommentsDateLiteral");
							CommentsDateLiteral.Text = wii.Date.ToString(Global.DateFormat);

							Literal CommentsInviterNameLiteral = (Literal) e.Item.FindControl("CommentsInviterNameLiteral");
							CommentsInviterNameLiteral.Text = wii.AssetWorkflowCommenter.InvitingUser.FullName;

							Literal CommentsInviterMessageLiteral = (Literal) e.Item.FindControl("CommentsInviterMessageLiteral");
							CommentsInviterMessageLiteral.Text = wii.AssetWorkflowCommenter.InvitingUserMessage;

							Literal CommentsInvitedUserNameLiteral = (Literal) e.Item.FindControl("CommentsInvitedUserNameLiteral");
							CommentsInvitedUserNameLiteral.Text = wii.AssetWorkflowCommenter.User.FullName;

							InitialiseComments(e.Item, "Comments", wii.Comments);
						}
					}
					else
					{
						switch (wii.AssetWorkflowUserStatus)
						{
							case (AssetWorkflowUserStatus.Pending):

								PlaceHolder PendingPlaceHolder = (PlaceHolder)e.Item.FindControl("PendingPlaceHolder");
								PendingPlaceHolder.Visible = true;

								EmailHyperLink PendingUserHyperLink = (EmailHyperLink)e.Item.FindControl("PendingUserHyperLink");
								PendingUserHyperLink.SetNameEmail(wii.UserName, wii.UserEmail);

								Literal PendingCurrentPositionLiteral = (Literal)e.Item.FindControl("PendingCurrentPositionLiteral");
								PendingCurrentPositionLiteral.Text = wii.Position.ToString();

								Literal PendingTotalPositionsCountLiteral = (Literal)e.Item.FindControl("PendingTotalPositionsCountLiteral");
								PendingTotalPositionsCountLiteral.Text = wii.WorkflowInfo.TotalWorkflowUserCount.ToString();

								InitialiseComments(e.Item, "Pending", wii.Comments);

								break;

							case (AssetWorkflowUserStatus.Waiting):

								PlaceHolder WaitingPlaceHolder = (PlaceHolder)e.Item.FindControl("WaitingPlaceHolder");
								WaitingPlaceHolder.Visible = true;

								EmailHyperLink WaitingUserHyperLink = (EmailHyperLink)e.Item.FindControl("WaitingUserHyperLink");
								WaitingUserHyperLink.SetNameEmail(wii.UserName, wii.UserEmail);

								Literal WaitingCurrentPositionLiteral = (Literal)e.Item.FindControl("WaitingCurrentPositionLiteral");
								WaitingCurrentPositionLiteral.Text = wii.Position.ToString();

								Literal WaitingTotalPositionsCountLiteral = (Literal)e.Item.FindControl("WaitingTotalPositionsCountLiteral");
								WaitingTotalPositionsCountLiteral.Text = wii.WorkflowInfo.TotalWorkflowUserCount.ToString();

								break;

							case (AssetWorkflowUserStatus.Approved):

								PlaceHolder ApprovedPlaceHolder = (PlaceHolder)e.Item.FindControl("ApprovedPlaceHolder");
								ApprovedPlaceHolder.Visible = true;

								EmailHyperLink ApprovedUserHyperLink = (EmailHyperLink)e.Item.FindControl("ApprovedUserHyperLink");
								ApprovedUserHyperLink.SetNameEmail(wii.UserName, wii.UserEmail);

								Literal ApprovedDateLiteral = (Literal)e.Item.FindControl("ApprovedDateLiteral");
								ApprovedDateLiteral.Text = wii.Date.ToString(Global.DateFormat);

								InitialiseComments(e.Item, "Approved", wii.Comments);

								break;

							case (AssetWorkflowUserStatus.Rejected):

								PlaceHolder RejectedPlaceHolder = (PlaceHolder)e.Item.FindControl("RejectedPlaceHolder");
								RejectedPlaceHolder.Visible = true;

								EmailHyperLink RejectedUserHyperLink = (EmailHyperLink)e.Item.FindControl("RejectedUserHyperLink");
								RejectedUserHyperLink.SetNameEmail(wii.UserName, wii.UserEmail);

								Literal RejectedDateLiteral = (Literal)e.Item.FindControl("RejectedDateLiteral");
								RejectedDateLiteral.Text = wii.Date.ToString(Global.DateFormat);

								InitialiseComments(e.Item, "Rejected", wii.Comments);

								break;
						}
					}

					break;
			}
		}

		private static void InitialiseComments(Control item, string prefix, string comments)
		{
			HtmlGenericControl CommentsText = (HtmlGenericControl)item.FindControl(prefix + "CommentText");
			EnhancedHyperLink ShowCommentsHyperLink = (EnhancedHyperLink)item.FindControl(prefix + "ShowCommentsHyperLink");
			Literal CommentTextLiteral = item.FindControl(prefix + "CommentTextLiteral") as Literal;

			if (!StringUtils.IsBlank(comments))
			{
				if (CommentTextLiteral == null)
				{
					CommentsText.InnerText = comments;
				}
				else
				{
					CommentTextLiteral.Text = comments;
				}

				ShowCommentsHyperLink.NavigateUrl = string.Format("javascript:toggleComments('{0}', '{1}')", ShowCommentsHyperLink.ClientID, CommentsText.ClientID);
			}
			else
			{
				CommentsText.Visible = false;
				ShowCommentsHyperLink.Visible = false;
			}
		}
	}
}