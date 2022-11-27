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
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	[Serializable]
	public class UserIdFinder : AbstractFinder
	{
		#region Private Variables

		public UserIdFinder()
		{
			BrandId = null;
		}

		#endregion

		#region Accessors

		public string CompanyName { get; set; }

		public int? BrandId { get; set; }

		#endregion

		#region AbstractFinder Implementation

		protected override void SetSearchCriteria(ref SearchBuilder sb)
		{
			SortExpressions.Clear();
			sb.SortExpressions.Clear();
			sb.TableAlias = "U";
			sb.Fields.Add("UserId");

			if (!StringUtils.IsBlank(CompanyName) && CompanyName.ToLower() != "all")
				sb.Criteria.Add(string.Format("CompanyName='{0}'", SqlUtils.SafeValue(CompanyName)));

			if (BrandId.HasValue)
			{
				switch (BrandId.Value)
				{
					case 0:
						sb.Criteria.Add(string.Format("{0} IS NULL", User.Columns.PrimaryBrandId));
						break;

					default:
						sb.Criteria.Add(string.Format("{0}={1}", User.Columns.PrimaryBrandId, BrandId.Value));
						break;
				}
			}
		}

		public override string Table
		{
			get
			{
				return "[v_User]";
			}
		}

		#endregion
	}
}