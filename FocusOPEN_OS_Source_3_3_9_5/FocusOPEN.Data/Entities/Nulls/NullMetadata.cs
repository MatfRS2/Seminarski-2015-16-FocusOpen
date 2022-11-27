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
	public class NullMetadata : Metadata
	{
		#region Singleton implementation

		private NullMetadata()
		{
		}

		private static readonly NullMetadata m_instance = new NullMetadata();

		public static NullMetadata Instance
		{
			get { return m_instance; }
		}

		#endregion

		#region Accessors
		
		/// <summary>
		/// Returns the BrandId of the Metadata object.
		/// </summary>
		public override Nullable <Int32> BrandId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the ParentMetadataId of the Metadata object.
		/// </summary>
		public override Nullable <Int32> ParentMetadataId
		{
			get { return null; }
			set { ; }
		}

		/// <summary>
		/// Returns the Name of the Metadata object.
		/// </summary>
		public override string Name
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the ExternalRef of the Metadata object.
		/// </summary>
		public override string ExternalRef
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the Synonyms of the Metadata object.
		/// </summary>
		public override string Synonyms
		{
			get { return String.Empty; }
			set { ; }
		}

		/// <summary>
		/// Returns the GroupNumber of the Metadata object.
		/// </summary>
		public override int GroupNumber
		{
			get { return 0; }
			set { ; }
		}

		/// <summary>
		/// Returns the IsDeleted of the Metadata object.
		/// </summary>
		public override bool IsDeleted
		{
			get { return false; }
			set { ; }
		}
		#endregion
	
		public override bool IsNull
		{
			get { return true; }
		}
	}
}

