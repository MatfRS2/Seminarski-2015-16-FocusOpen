/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web;

namespace FocusOPEN.Business
{
	internal static class BusinessHelper
	{
		public static string GetCurrentIpAddress()
		{
			if (HttpContext.Current != null)
			{
				try
				{
					return HttpContext.Current.Request.UserHostAddress;
				}
				catch
				{
					return "-1.-1.-1.-1";
				}
			}
			return "0.0.0.0";
		}

		public static string GetCurrentSessionId()
		{
			if (HttpContext.Current != null)
			{
				try
				{
					return HttpContext.Current.Session.SessionID;
				}
				catch
				{
					return "__ERROR_GETTING_SESSION_ID__";
				}
			}
			return "__UNKNOWN_SESSION_ID__";
		}
	}
}