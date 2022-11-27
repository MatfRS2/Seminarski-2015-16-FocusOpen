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
using System.Collections.Generic;
using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class PreviousLightboxRecipientDropDownList : AbstractDropDownList
	{
		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			List<String> data = new List<String>();

			LightboxSentFinder finder = new LightboxSentFinder();
			finder.SenderId = SessionInfo.Current.User.UserId.GetValueOrDefault();
			EntityList<LightboxSent> sentLightboxes = LightboxSent.FindMany(finder);

			foreach (LightboxSent lbs in sentLightboxes)
			{
				if (!data.Contains(lbs.RecipientEmail.ToLower()))
					data.Add(lbs.RecipientEmail.ToLower());
			}

			return data;
		}

		#endregion
	}
}