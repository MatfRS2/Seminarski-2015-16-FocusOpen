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
	public class DateRange : INullable
	{
		public static DateRange Get(DateTime? startDate, DateTime? endDate)
		{
			if (startDate == null && endDate == null)
				return Empty;

			return new DateRange(startDate, endDate);
		}

		#region Constructors

		protected DateRange()
		{
			EndDate = null;
			StartDate = null;
		}

		private DateRange(DateTime? startDate, DateTime? endDate)
		{
			StartDate = startDate;
			EndDate = endDate;
		}

		#endregion

		#region Properties

		public DateTime? StartDate { get; private set; }

		public DateTime? EndDate { get; private set; }

		public static DateRange Empty
		{
			get
			{
				return NullDateRange.Instance;
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