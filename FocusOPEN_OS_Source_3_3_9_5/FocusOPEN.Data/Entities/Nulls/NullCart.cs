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
	/// <summary>
	/// This object represents a null Cart.
	/// </summary>
	[Serializable]
	public class NullCart : Cart
	{
		#region Singleton implementation

		private NullCart()
		{
		}

		private static readonly NullCart m_instance = new NullCart();

		public static NullCart Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the UserId of the Cart object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the AssetId of the Cart object.
		/// </summary>
		public override int AssetId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Notes of the Cart object.
		/// </summary>
		public override string Notes
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the RequiredByDate of the Cart object.
		/// </summary>
		public override Nullable <DateTime> RequiredByDate
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the DateAdded of the Cart object.
		/// </summary>
		public override DateTime DateAdded
		{
			get { return DateTime.MinValue; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

