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
	/// This object represents a null Lightbox.
	/// </summary>
	[Serializable]
	public class NullLightbox : Lightbox
	{
		#region Singleton implementation

		private NullLightbox()
		{
		}

		private static readonly NullLightbox m_instance = new NullLightbox();

		public static NullLightbox Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the UserId of the Lightbox object.
		/// </summary>
		public override int UserId
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the Lightbox object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Summary of the Lightbox object.
		/// </summary>
		public override string Summary
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Notes of the Lightbox object.
		/// </summary>
		public override string Notes
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsPublic of the Lightbox object.
		/// </summary>
		public override bool IsPublic
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDefault of the Lightbox object.
		/// </summary>
		public override bool IsDefault
		{
			get { return false; }
			set { ; }
		}

		/// <summary>
		/// Returns the CreateDate of the Lightbox object.
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

