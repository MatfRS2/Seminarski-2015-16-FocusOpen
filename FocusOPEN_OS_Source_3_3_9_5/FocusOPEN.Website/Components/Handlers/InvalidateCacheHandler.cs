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
using System.Reflection;
using FocusOPEN.Business;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Website.Components.Handlers
{
	public class InvalidateCacheHandler : BaseHandler
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public override void ProcessRequest()
		{
			string section = WebUtils.GetRequestParam("section", string.Empty).ToLower();

			m_Logger.DebugFormat("Called InvalidateCacheHandler.  Section: {0}", section);

			CacheManager.InvalidateCache(section, CacheType.Local);

			Context.Response.Write("Invalidated cache: " + section + ", " + DateTime.Now.ToString("u"));
		}
	}
}