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

namespace FocusOPEN.Data
{
	public class HomepageTypeCache : BaseEntityCache<HomepageType>
	{
		#region Singleton

		private HomepageTypeCache()
		{
		}

		private static readonly HomepageTypeCache m_Instance = new HomepageTypeCache();

		public static HomepageTypeCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<HomepageType> GetData()
		{
			HomepageTypeFinder finder = new HomepageTypeFinder();
			finder.SortExpressions.Add(new AscendingSort(HomepageType.Columns.Description));
			return HomepageType.FindMany(finder);
		}

		protected override bool Compare(HomepageType entity, int id)
		{
			return entity.HomepageTypeId.GetValueOrDefault() == id;
		}

		protected override HomepageType Empty
		{
			get
			{
				return HomepageType.Empty;
			}
		}
		
		#endregion
	}
}