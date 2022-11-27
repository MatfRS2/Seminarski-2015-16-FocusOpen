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
	public class CountryCache : BaseEntityCache<Country>
	{
		#region Singleton

		private CountryCache()
		{
		}

		private static readonly CountryCache m_Instance = new CountryCache();

		public static CountryCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<Country> GetData()
		{
			CountryFinder finder = new CountryFinder();
			finder.SortExpressions.Add(new AscendingSort(Country.Columns.Rank));
			finder.SortExpressions.Add(new AscendingSort(Country.Columns.Name));
			return Country.FindMany(finder);
		}

		protected override bool Compare(Country entity, int id)
		{
			return entity.CountryId.GetValueOrDefault() == id;
		}

		protected override Country Empty
		{
			get
			{
				return Country.Empty;
			}
		}

		#endregion
	}
}
