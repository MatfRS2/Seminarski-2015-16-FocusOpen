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
using Daydream.Data;

namespace FocusOPEN.Data
{
	public class MetadataCache : BaseEntityCache<Metadata>
	{
		#region Singleton

		private MetadataCache()
		{
		}

		private static readonly MetadataCache m_Instance = new MetadataCache();

		public static MetadataCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<Metadata> GetData()
		{
			MetadataFinder finder = new MetadataFinder();
			finder.SortExpressions.Add(new AscendingSort(Metadata.Columns.Name));
			return Metadata.FindMany(finder);
		}

		protected override bool Compare(Metadata entity, int id)
		{
			return entity.MetadataId.GetValueOrDefault() == id;
		}

		protected override Metadata Empty
		{
			get
			{
				return Metadata.Empty;
			}
		}

		#endregion

		/// <summary>
		/// Gets all metadata items for the specified brand and group sorted by name
		/// </summary>
		public IEnumerable<Metadata> GetList(int brandId, int groupNumber)
		{
			return (from m in GetList()
					where m.BrandId == brandId && m.GroupNumber == groupNumber
					orderby m.Name
					select m).ToList();
		}

		/// <summary>
		/// Gets all metadata items that have the specified ID as their parent
		/// </summary>
		public IList<Metadata> GetList(int? parentId)
		{
			return (from m in GetList()
					where m.ParentMetadataId.GetValueOrDefault(Int32.MinValue) == parentId.GetValueOrDefault(Int32.MinValue)
					orderby m.Name
					select m).ToList();
		}
	}
}