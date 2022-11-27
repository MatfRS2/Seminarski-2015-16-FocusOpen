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

namespace Daydream.Data
{
	[Serializable]
	public class PagingInfo : INullable
	{
		#region Constructor

		public PagingInfo(int totalRecords, int currentPage, int pageSize)
		{
			TotalRecords = totalRecords;
			CurrentPage = currentPage;
			PageSize = pageSize;
		}

		#endregion

		#region Simple Accessor Properties

		/// <summary>
		/// The total number of records in the data set
		/// </summary>
		public int TotalRecords { get; set; }

		/// <summary>
		/// The number (zero-indexed) of the current page in the data set
		/// </summary>
		public int CurrentPage { get; set; }

		/// <summary>
		/// The number of records per page in the data set
		/// </summary>
		public int PageSize { get; set; }

		#endregion

		#region Calculated properties

		/// <summary>
		/// The number (zero-indexed) of the last page in the data set
		/// </summary>
		public int LastPage
		{
			get
			{
				return TotalPages - 1;
			}
		}

		/// <summary>
		/// The total number of pages in the data set
		/// </summary>
		public int TotalPages
		{
			get
			{
				if (TotalRecords == 0 || PageSize == 0)
					return 1;

				int remainder = TotalRecords%PageSize;

				if (remainder == 0)
					return TotalRecords/PageSize;

				return ((TotalRecords - remainder)/PageSize) + 1;
			}
		}

		/// <summary>
		/// The number of the first record on the current page of the data set
		/// </summary>
		public int StartRecord
		{
			get
			{
				return (CurrentPage*PageSize) + 1;
			}
		}

		/// <summary>
		/// The number of the last record on the current page of the data set
		/// </summary>
		public int EndRecord
		{
			get
			{
				return Math.Min((StartRecord + PageSize) - 1, TotalRecords);
			}
		}

		#endregion

		#region INullable Members

		public virtual Boolean IsNull
		{
			get
			{
				return false;
			}
		}

		#endregion
	}
}