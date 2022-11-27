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
	/// This object represents a null Country.
	/// </summary>
	[Serializable]
	public class NullCountry : Country
	{
		#region Singleton implementation

		private NullCountry()
		{
		}

		private static readonly NullCountry m_instance = new NullCountry();

		public static NullCountry Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Code of the Country object.
		/// </summary>
		public override string Code
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the Country object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Rank of the Country object.
		/// </summary>
		public override int Rank
		{
			get { return 0; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

