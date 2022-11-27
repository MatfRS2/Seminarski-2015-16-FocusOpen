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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	public partial class LightboxFinder
	{
		#region Private Variables

		public LightboxFinder()
		{
			BrandIdList = new List<int>();
			UserIdOrPublic = null;
		}

		#endregion

		#region Accessors

		/// <summary>
		/// List of brand ID's that lightboxes need to be available to
		/// </summary>
		public List<int> BrandIdList { get; private set; }

		/// <summary>
		/// Lightbox must belong to the user with this user id OR be public
		/// </summary>
		public int? UserIdOrPublic { get; set; }

		#endregion

		protected void SetCustomSearchCriteria(ref SearchBuilder sb)
		{
			if (UserIdOrPublic.HasValue)
			{
				string criteria = string.Format("({0} = {1} OR {2}=1)", Lightbox.Columns.UserId, UserIdOrPublic, Lightbox.Columns.IsPublic);
				sb.Criteria.Add(criteria);
			}

			if (BrandIdList.Count > 0)
			{
				string criteria = string.Format("LightboxId IN (SELECT LightboxId FROM LightboxBrand WHERE (BrandId IN ({0})))", BrandIdList.ToCommaDelimitedList());
				sb.Criteria.Add(criteria);
			}
		}
	}
}