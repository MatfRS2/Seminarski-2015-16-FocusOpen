/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using Daydream.Data;
using FocusOPEN.Data;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class UserDropDownList : AbstractDropDownList
	{
		#region Accessors

		public int MinimumUserRoleId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "MinimumUserRoleId", 0);
			}
			set
			{
				ViewState["MinimumUserRoleId"] = value;
			}
		}

		public int MaximumUserRoleId
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "MaximumUserRoleId", 0);
			}
			set
			{
				ViewState["MaximumUserRoleId"] = value;
			}
		}

		public int? BrandId
		{
			get
			{
				return SiteUtils.GetFromStore<int?>(ViewState, "BrandId", null);
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
			UserFinder finder = new UserFinder();
			finder.SortExpressions.Add(new AscendingSort("FullName"));

			finder.MinimumUserRoleId = MinimumUserRoleId;
			finder.MaximumUserRoleId = MaximumUserRoleId;
			finder.PrimaryBrandId = BrandId.GetValueOrDefault();
			
			return User.FindMany(finder);
		}

		public override string GetDataTextField()
		{
			return "FullName";
		}

		public override string GetDataValueField()
		{
			return User.Columns.UserId.ToString();
		}

		#endregion
	}
}