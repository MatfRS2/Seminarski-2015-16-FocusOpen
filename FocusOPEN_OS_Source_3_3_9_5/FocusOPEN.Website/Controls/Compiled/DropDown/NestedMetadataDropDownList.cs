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
	public class NestedMetadataDropDownList : AbstractNestedDropDownList<Metadata>
	{
		#region Accessors

		public int GroupNumber
		{
			get
			{
				return SiteUtils.GetFromStore(ViewState, "GroupNumber", 0);
			}
			set
			{
				ViewState["GroupNumber"] = value;
			}
		}

		#endregion

		#region Constructor

		public NestedMetadataDropDownList()
		{
			BrandId = 0;
			GroupNumber = 0;
		}

		#endregion

		#region AbstractNestedDropDownList Implementation

		protected override IEnumerable<Metadata> GetParentList()
		{
			return (from m in MetadataCache.Instance.GetList(BrandId, GroupNumber)
			        where !m.ParentMetadataId.HasValue
			        select m).ToList();
		}

		protected override IEnumerable<Metadata> GetChildren(Metadata entity)
		{
			return entity.Children;
		}

		protected override string GetName(Metadata entity)
		{
			return entity.Name;
		}

		protected override int GetId(Metadata entity)
		{
			return entity.MetadataId.GetValueOrDefault();
		}

		#endregion
	}
}