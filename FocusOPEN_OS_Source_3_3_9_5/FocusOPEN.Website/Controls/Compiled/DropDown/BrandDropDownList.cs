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
using System.Linq;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class BrandDropDownList : AbstractDropDownList
	{
		/// <summary>
		/// Boolean value specifying whether all brands should be displayed
		/// or only those accessible to the currently logged in user.
		/// </summary>
		public bool Restrict { get; set; }

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			List<Brand> list = new List<Brand>();
			list.AddRange(BrandCache.Instance.GetList());

			if (Restrict)
			{
				// List is restricted so remove brands that the user cannot access
				list.RemoveAll(b => !SessionInfo.Current.User.CanAccessBrand(b.BrandId.GetValueOrDefault()));
			}

			list.Sort((a,b) => a.Name.CompareTo(b.Name));

			return list;
		}

		public override string GetDataTextField()
		{
			return Brand.Columns.Name.ToString();
		}

		public override string GetDataValueField()
		{
			return Brand.Columns.BrandId.ToString();
		}

		#endregion
	}
}