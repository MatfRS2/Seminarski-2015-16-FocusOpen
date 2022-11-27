/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data;
using FocusOPEN.Shared;
using System;

namespace FocusOPEN.Website.Components
{
	/// <summary>
	/// Forms the foundation for all secured pages, whether they
	/// are for the front-end area, or the admin area
	/// </summary>
	public abstract class BaseSecuredPage : BasePage
	{
		protected static User CurrentUser
		{
			get
			{
				return SessionInfo.Current.User;
			}
		}

        protected virtual ContextType AssetContext
        {
            get
            {
                return ContextType.Standard;
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            SessionInfo.Current.AdminSessionInfo.AssetContext = AssetContext;
            base.OnLoad(e);
        }

	}
}