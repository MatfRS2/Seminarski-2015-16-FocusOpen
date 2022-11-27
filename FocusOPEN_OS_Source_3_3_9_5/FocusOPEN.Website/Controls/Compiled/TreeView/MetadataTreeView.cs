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
using System.Linq;
using System.Web.UI.WebControls;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using FocusOPEN.Website.Components;

namespace FocusOPEN.Website.Controls
{
	public class MetadataTreeView : BaseTreeView<Metadata>
	{
		public int GroupNumber { get; set; }

        public SelectableMetadataSortType SortType
        {
            get
            {
                return SiteUtils.GetFromStore(ViewState, "SortType", SelectableMetadataSortType.AlphaNumeric);
            }
            set
            {
                ViewState["SortType"] = value;
            }
        }

	    #region BaseTreeView Implementation

		protected override IEnumerable<Metadata> GetRootList()
		{
			var list = (from m in MetadataCache.Instance.GetList(BrandId, GroupNumber)
			        where !m.ParentMetadataId.HasValue
			        select m).ToList();

		    return OrderMetas(list);
            
		}

	    private IEnumerable<Metadata> OrderMetas(IList<Metadata> list)
	    {
            if (SortType == SelectableMetadataSortType.AsDesigned)
                list = list.OrderBy(m => m.MetadataOrder).ToList();

	        return list;
	    }

	    protected override IEnumerable<Metadata> GetSubEntitities(Metadata entity)
	    {
	        return OrderMetas(entity.Children);
	    }

		protected override TreeNode GetNodeFromEntity(Metadata entity)
		{
			TreeNode node = new TreeNode
			                	{
			                		Text = entity.Name,
									Value = entity.MetadataId.ToString()
			                	};
			return node;
		}

		#endregion
	}
}