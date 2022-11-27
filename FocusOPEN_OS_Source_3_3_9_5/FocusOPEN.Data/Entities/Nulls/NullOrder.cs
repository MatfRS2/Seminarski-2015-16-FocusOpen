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

namespace FocusOPEN.Data
{
	[Serializable]
	public class NullOrder : Order
	{
		#region Singleton implementation

		private NullOrder()
		{
		}

		private static readonly NullOrder m_instance = new NullOrder();

		public static NullOrder Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the UserId of the Order object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the OrderDate of the Order object.
		/// </summary>
		public override DateTime OrderDate
		{
			get { return DateTime.MinValue; }
			set { ; }
		}

		/// <summary>
		/// Returns the CompletionDate of the Order object.
		/// </summary>
		public override Nullable <DateTime> CompletionDate
		{
			get { return null; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

