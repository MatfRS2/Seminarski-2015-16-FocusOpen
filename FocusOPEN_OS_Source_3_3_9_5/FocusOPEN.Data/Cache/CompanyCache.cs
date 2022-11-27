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
	public class CompanyCache : BaseEntityCache<Company>
	{
		#region Singleton

		private CompanyCache()
		{
		}

		private static readonly CompanyCache m_Instance = new CompanyCache();

		public static CompanyCache Instance
		{
			get
			{
				return m_Instance;
			}
		}

		#endregion

		#region BaseEntityCache Implementation

		protected override List<Company> GetData()
		{
			CompanyFinder finder = new CompanyFinder();
			finder.SortExpressions.Add(new AscendingSort(Company.Columns.Name));
			finder.SortExpressions.Add(new AscendingSort(Company.Columns.Domain));
			return Company.FindMany(finder);
		}

		protected override bool Compare(Company entity, int id)
		{
			return entity.CompanyId.GetValueOrDefault() == id;
		}

		protected override Company Empty
		{
			get
			{
				return Company.Empty;
			}
		}
		
		#endregion
	}
}