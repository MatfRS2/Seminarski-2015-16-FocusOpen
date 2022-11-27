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
	/// This object represents a null Company.
	/// </summary>
	[Serializable]
	public class NullCompany : Company
	{
		#region Singleton implementation

		private NullCompany()
		{
		}

		private static readonly NullCompany m_instance = new NullCompany();

		public static NullCompany Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Name of the Company object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Brands of the Company object.
		/// </summary>
		public override string Brands
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Domain of the Company object.
		/// </summary>
		public override string Domain
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsInternal of the Company object.
		/// </summary>
		public override bool IsInternal
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreatedByUserId of the Company object.
		/// </summary>
		public override int CreatedByUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the Company object.
		/// </summary>
		public override DateTime CreateDate
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

