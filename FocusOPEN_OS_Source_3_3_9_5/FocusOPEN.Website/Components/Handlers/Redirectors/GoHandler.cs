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
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components.Handlers
{
	public class GoHandler : BaseHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			string path = Context.Request.PathInfo.ToLower();
			string prefix = GetPrefixFromPath(path);

			switch (prefix)
			{
				case "aaf":

					//----------------------------
					// AAF:	Adjust Asset Files
					//----------------------------
					// AID:	AssetId
					// APID:AssetProcessId
					//----------------------------

					string pattern = string.Format(@"/{0}/AID(?<AssetId>[\d]+)-APID(?<AssetProcessId>[\d]+)/", prefix);
					Hashtable matches = Parse(pattern, path);

					if (matches.Count == 2)
					{
						int assetId = Convert.ToInt32(matches["AssetId"]);
						int assetProcessId = Convert.ToInt32(matches["AssetProcessId"]);

						string url = string.Format("~/Admin/Assets/AdjustAsset.aspx?assetId={0}&assetProcessId={1}", assetId, assetProcessId);
						Context.Response.Redirect(url, false);

						return;
					}
					
					break;

				case "aaw":

					//----------------------------
					// AAW:	Approve Asset Workflow
					//----------------------------
					// AWFID: AssetWorkflowId
					//----------------------------

					pattern = string.Format(@"/{0}/AWFID(?<AssetWorkflowId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int assetWorkflowId = Convert.ToInt32(matches["AssetWorkflowId"]);

						string url = string.Format("~/Admin/Workflow/Approve.aspx?AssetWorkflowId={0}", assetWorkflowId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "aci":

					//----------------------------
					// ACI:	Asset Commenter Invited
					//----------------------------
					// AWFID: AssetWorkflowId
					//----------------------------

					pattern = string.Format(@"/{0}/AWFID(?<AssetWorkflowId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int assetWorkflowId = Convert.ToInt32(matches["AssetWorkflowId"]);

						string url = string.Format("~/Admin/Workflow/Comment.aspx?AssetWorkflowId={0}", assetWorkflowId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "aeu":

					//----------------------------
					// AEU:	Approve external user
					//----------------------------
					// U:	UserId
					//----------------------------

					pattern = string.Format(@"/{0}/U(?<UserId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int userId = Convert.ToInt32(matches["UserId"]);

						string url = string.Format("~/Admin/Users/UserApproval.aspx?userId={0}", userId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "auo":

					//----------------------------
					// AUO:	Approve user order
					//----------------------------
					// O:	OrderId
					//----------------------------

					pattern = string.Format(@"/{0}/O(?<OrderId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int orderId = Convert.ToInt32(matches["OrderId"]);

						string url = string.Format("~/Admin/Orders/OrderForm.aspx?orderId={0}", orderId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "cua":

					//----------------------------
					// CUA: Confirm User Account
					//----------------------------
					// U:	UserId
					// E:	Hashed user email address
					// G:	Account GUID
					//----------------------------

					pattern = string.Format(@"/{0}/U(?<UserId>[\d]+)-E(?<HashedEmail>[^-]*)-G(?<GUID>[\d\D]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 3)
					{
						int userId = Convert.ToInt32(matches["UserId"]);
						string email = matches["HashedEmail"].ToString();
						string guid = matches["GUID"].ToString();

						User user = UserManager.GetUser(userId, email, guid);
						UserManager.ConfirmEmailAddress(user);

						Context.Response.Redirect("~/Login.aspx?message=EmailConfirmed", false);

						return;
					}

					break;

				case "rua":

					//----------------------------
					// RUA: Reactivate User Account
					//----------------------------
					// U:	UserId
					// E:	Hashed user email address
					// G:	Account GUID
					//----------------------------

					pattern = string.Format(@"/{0}/U(?<UserId>[\d]+)-E(?<HashedEmail>[^-]*)-G(?<GUID>[\d\D]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 3)
					{
						int userId = Convert.ToInt32(matches["UserId"]);
						string email = matches["HashedEmail"].ToString();
						string guid = matches["GUID"].ToString();

						User user = UserManager.GetUser(userId, email, guid);
						UserManager.ReactivateUser(user);

						Context.Response.Redirect("~/Login.aspx?message=AccountReactivated", false);

						return;
					}

					break;

				case "eua":

					//----------------------------
					// EUA:	Edit user account
					//----------------------------
					// U:	UserId
					//----------------------------

					pattern = string.Format(@"/{0}/U(?<UserId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int userId = Convert.ToInt32(matches["UserId"]);

						string url = string.Format("~/Admin/Users/UserForm.aspx?userId={0}", userId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "rad":

					//----------------------------
					// RAD:	Review Asset Details
					//----------------------------
					// AID: AssetId
					//----------------------------

					pattern = string.Format(@"/{0}/AID(?<AssetId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						int assetId = Convert.ToInt32(matches["AssetId"]);

						string url = string.Format("~/Admin/Assets/AssetForm.aspx?AssetId={0}", assetId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "vad":

					//----------------------------
					// VAD:	View Asset Detail
					//----------------------------
					// AID:	AssetId
					//----------------------------

					pattern = string.Format(@"/{0}/AID(?<AssetId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						string url = string.Format("~/Default.aspx?action=detail&assetId={0}", Convert.ToInt32(matches["AssetId"]));
						Context.Response.Redirect(url, false);

						return;
					}


					break;

				case "vcs":

					//----------------------------
					// VCS:		View contact sheet
					//----------------------------
					// LSID:	LightboxSentId
					// SUID:	SenderUserId
					// DST:		DateSentTicks
					//----------------------------

					pattern = string.Format(@"/{0}/LSID(?<LightboxSentId>[\d]+)-SUID(?<SenderUserId>[\d^-]*)-DST(?<DateSentTicks>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 3)
					{
						int lightboxSentId = Convert.ToInt32(matches["LightboxSentId"]);
						int senderUserId = Convert.ToInt32(matches["SenderUserId"]);
						long dateSentTicks = Convert.ToInt64(matches["DateSentTicks"]);

						string url = string.Format("~/ViewContactSheet.aspx?lsid={0}&suid={1}&dst={2}", lightboxSentId, senderUserId, dateSentTicks);
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "vlb":

					//----------------------------
					// VLB:	View Lightbox
					//----------------------------
					// LID:	LightboxId
					//----------------------------

					pattern = string.Format(@"/{0}/LID(?<LightboxId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						string url = string.Format("~/MyAccount/ViewLightbox.aspx?lightboxId={0}", Convert.ToInt32(matches["LightboxId"]));
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "vwo":

					//----------------------------
					// VWO:	View Order
					//----------------------------
					// O:	OrderId
					//----------------------------

					pattern = string.Format(@"/{0}/O(?<OrderId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 1)
					{
						string url = string.Format("~/MyAccount/ViewOrders.aspx?orderId={0}", Convert.ToInt32(matches["OrderId"]));
						Context.Response.Redirect(url, false);

						return;
					}

					break;

				case "voi":

					//----------------------------
					// VOI:	View Order Item
					//----------------------------
					// O:	OrderId
					// OI:	OrderItemId
					//----------------------------

					pattern = string.Format(@"/{0}/O(?<OrderId>[\d]+)-OI(?<OrderItemId>[\d]+)/", prefix);
					matches = Parse(pattern, path);

					if (matches.Count == 2)
					{
						int orderId = Convert.ToInt32(matches["OrderId"]);
						int orderItemId = Convert.ToInt32(matches["OrderItemId"]);

						string url = string.Format("~/MyAccount/ViewOrders.aspx?orderId={0}&orderItemId={1}", orderId, orderItemId);
						Context.Response.Redirect(url, false);

						return;
					}

					break;
			}

			Context.Response.Write("Nothing matched.  Redirecting...");
			Context.Response.AddHeader("refresh", "3;" + VirtualPathUtility.ToAbsolute("~/Default.aspx"));
		}

		#region Helper Methods

		private static string GetPrefixFromPath(string path)
		{
			const string pattern = @"^/(?<code>[\D]+)/";
			Hashtable matches = Parse(pattern, path);

			if (matches.Count == 1)
				return matches["code"].ToString();

			return string.Empty;
		}

		/// <summary>
		/// Parses the path using the specified regular expression and returns
		/// a hashtable of all the groups and their matched values
		/// </summary>
		private static Hashtable Parse(string pattern, string path)
		{
			Hashtable ht = new Hashtable();

			Regex re = new Regex(pattern, RegexOptions.IgnoreCase);
			Match match = re.Match(path);

			if (match.Success)
			{
				for (int i = 1; i < match.Groups.Count; i++)
				{
					Group g = match.Groups[i];

					string name = re.GroupNameFromNumber(i);
					string val = g.Value;

					ht[name] = val;
				}
			}

			return ht;
		}

		#endregion
	}
}