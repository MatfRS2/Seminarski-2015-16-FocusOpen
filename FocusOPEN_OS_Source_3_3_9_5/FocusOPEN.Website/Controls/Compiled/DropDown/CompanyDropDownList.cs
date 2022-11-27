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
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class CompanyDropDownList : AbstractDictionaryDropDownList
	{
		public enum RestrictionMode
		{
			None,
			Internal,
			External
		}

		#region Properties

		public bool ShowDomain { get; set; }

		public RestrictionMode Restriction
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "Restriction", RestrictionMode.None);
			}
			set
			{
				ViewState["Restriction"] = value;
			}
		}

		public int BrandId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "BrandId", 0);
			}
			set
			{
				ViewState["BrandId"] = value;
			}
		}

		#endregion

		#region AbstractDropDownList Implementation

		public override object GetDataSource()
		{
			List<Company> list = new List<Company>();
			list.AddRange(CompanyCache.Instance.GetList());

			if (Restriction == RestrictionMode.Internal)
				list.RemoveAll(c => !c.IsInternal);

			if (Restriction == RestrictionMode.External)
				list.RemoveAll(c => c.IsInternal);

			if (BrandId > 0)
				list.RemoveAll(c => !c.BrandList.Any(b => b.BrandId == BrandId));

			return from c in list
			       select new {Id = c.CompanyId.GetValueOrDefault(), Name = GetName(c)};
		}

		public override string GetDataTextField()
		{
			return "Name";
		}

		public override string GetDataValueField()
		{
			return "Id";
		}

		#endregion

		private string GetName(Company company)
		{
			if (ShowDomain)
				return string.Concat(company.Name, " (", company.Domain, ")");

			return company.Name;
		}
	}
}