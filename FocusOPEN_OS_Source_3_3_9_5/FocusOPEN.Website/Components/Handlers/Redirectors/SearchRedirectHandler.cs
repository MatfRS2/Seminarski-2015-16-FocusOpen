/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web.SessionState;
using FocusOPEN.Business;

namespace FocusOPEN.Website.Components.Handlers
{
	public class SearchRedirectHandler : BaseHandler, IRequiresSessionState
	{
		public override void ProcessRequest()
		{
			if (SessionInfo.Current.User.IsNull)
				Context.Response.Redirect("~/Login.aspx?returnUrl=" + UrlEncoder.Encode(Context.Request.RawUrl));

			// First setup the basic asset finder
			SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch.AssetFinder = SearchManager.GetBaseAssetFinder(SessionInfo.Current.User);

			// Then initialize it from the querystring
			SearchUrlMapper.SetupSearchFromRequest(Context.Request.QueryString, SessionInfo.Current.UserSessionInfo.SavedUserAssetSearch);

			// All set, send the user to the search results page
			Context.Response.Redirect("~/SearchResults.aspx");
		}
	}
}