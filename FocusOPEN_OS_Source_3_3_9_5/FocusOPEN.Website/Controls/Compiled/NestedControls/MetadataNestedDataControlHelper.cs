using System.Collections.Generic;
using System.Linq;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Controls
{
	/// <summary>
	/// Implementation that caters for Metadata based selecatble list controls
	/// </summary>
	public class MetadataNestedDataControlHelper : NestedDataControlHelper<Metadata>
	{
		public bool m_LinearOutlay;

		public MetadataNestedDataControlHelper(int brand, int group, SelectableMetadataSortType sort, bool linear)
		{
			getParentList = GetParentList;
			getChildren = GetChildren;
			getName = GetEntityName;
			getId = GetEntityId;
			prefix = " ";

			LinearOutlay = linear;
			SortType = sort;
			GroupNumber = group;
			BrandId = brand;
		}

		public bool LinearOutlay
		{
			get
			{
				return m_LinearOutlay;
			}
			set
			{
				if (value)
					prefix = string.Empty;

				m_LinearOutlay = value;
			}
		}

		public SelectableMetadataSortType SortType { get; set; }
		public int GroupNumber { get; set; }
		public int BrandId { get; set; }

		public IEnumerable<Metadata> GetParentList()
		{
			var entries = (from m in MetadataCache.Instance.GetList(BrandId, GroupNumber)
			               where !m.ParentMetadataId.HasValue
			               select m)
				.ToList();

            return OrderMetas(entries);
		}

        private IEnumerable<Metadata> OrderMetas(IList<Metadata> list)
        {
            if (SortType == SelectableMetadataSortType.AsDesigned)
                list = list.OrderBy(m => m.MetadataOrder).ToList();
            
            return list;
        }

		public IEnumerable<Metadata> GetChildren(Metadata entity)
		{
            return OrderMetas(entity.Children);
		}

		public string GetEntityName(Metadata entity)
		{
			return entity.Name;
		}

		public int GetEntityId(Metadata entity)
		{
			return entity.MetadataId.GetValueOrDefault();
		}
	}
}