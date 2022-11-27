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
	/// This object represents a null Homepage.
	/// </summary>
	[Serializable]
	public class NullHomepage : Homepage
	{
		#region Singleton implementation

		private NullHomepage()
		{
		}

		private static readonly NullHomepage m_instance = new NullHomepage();

		public static NullHomepage Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the BrandId of the Homepage object.
		/// </summary>
		public override int BrandId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IntroText of the Homepage object.
		/// </summary>
		public override string IntroText
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Url1 of the Homepage object.
		/// </summary>
		public override string Url1
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Url2 of the Homepage object.
		/// </summary>
		public override string Url2
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Url3 of the Homepage object.
		/// </summary>
		public override string Url3
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Url4 of the Homepage object.
		/// </summary>
		public override string Url4
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the BumperPageHtml of the Homepage object.
		/// </summary>
		public override string BumperPageHtml
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the BumperPageSkip of the Homepage object.
		/// </summary>
		public override bool BumperPageSkip
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the CustomHtml of the Homepage object.
		/// </summary>
		public override string CustomHtml
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the HomepageTypeId of the Homepage object.
		/// </summary>
		public override int HomepageTypeId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsPublished of the Homepage object.
		/// </summary>
		public override bool IsPublished
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastModifiedByUserId of the Homepage object.
		/// </summary>
		public override int LastModifiedByUserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the LastModifiedDate of the Homepage object.
		/// </summary>
		public override DateTime LastModifiedDate
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

