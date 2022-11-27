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
	/// This object represents a null HomepageType.
	/// </summary>
	[Serializable]
	public class NullHomepageType : HomepageType
	{
		#region Singleton implementation

		private NullHomepageType()
		{
		}

		private static readonly NullHomepageType m_instance = new NullHomepageType();

		public static NullHomepageType Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the Description of the HomepageType object.
		/// </summary>
		public override string Description
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ShortName of the HomepageType object.
		/// </summary>
		public override string ShortName
		{
			get { return String.Empty; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

