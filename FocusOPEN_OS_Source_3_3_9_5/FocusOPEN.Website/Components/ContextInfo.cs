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
using FocusOPEN.Business;

namespace FocusOPEN.Website.Components
{
	public static class ContextInfo
	{
		public static CartManager CartManager
		{
			get
			{
				CartManager cm = HttpContext.Current.Items["CartManager"] as CartManager;

				if (cm == null || cm.User.UserId != SessionInfo.Current.User.UserId)
				{
					cm = new CartManager(SessionInfo.Current.User);
					HttpContext.Current.Items["CartManager"] = cm;
				}

				return cm;
			}
		}

		public static LightboxManager LightboxManager
		{
			get
			{
				LightboxManager lm = HttpContext.Current.Items["LightboxManager"] as LightboxManager;

				if (lm == null || lm.User.UserId != SessionInfo.Current.User.UserId)
				{
					lm = new LightboxManager(SessionInfo.Current.User);
					HttpContext.Current.Items["LightboxManager"] = lm;
				}

				return lm;
			}
		}

		public static UserOrderManager UserOrderManager
		{
			get
			{
				UserOrderManager uom = HttpContext.Current.Items["UserOrderManager"] as UserOrderManager;

				if (uom == null || uom.User.UserId != SessionInfo.Current.User.UserId)
				{
					uom = new UserOrderManager(SessionInfo.Current.User);
					HttpContext.Current.Items["UserOrderManager"] = uom;
				}

				return uom;
			}
		}

        public static PluginManager PluginManager
        {
            get
            {
                PluginManager pm = HttpContext.Current.Items["PluginManager"] as PluginManager;

                if (pm == null)
                {
                    pm = new PluginManager();
                    HttpContext.Current.Items["PluginManager"] = pm;
                }

                return pm;
            }
        }
	}
}
