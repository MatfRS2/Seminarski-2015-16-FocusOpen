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
	/// This object represents a null HomepageCategory.
	/// </summary>
	[Serializable]
	public class NullHomepageCategory : HomepageCategory
	{
		#region Singleton implementation

		private NullHomepageCategory()
		{
		}

		private static readonly NullHomepageCategory m_instance = new NullHomepageCategory();

		public static NullHomepageCategory Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the HomepageId of the HomepageCategory object.
		/// </summary>
		public override int HomepageId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the CategoryId of the HomepageCategory object.
		/// </summary>
		public override int CategoryId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the OrderBy of the HomepageCategory object.
		/// </summary>
		public override int OrderBy
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

