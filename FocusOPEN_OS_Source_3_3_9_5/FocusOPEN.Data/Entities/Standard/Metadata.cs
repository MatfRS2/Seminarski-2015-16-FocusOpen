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
using Daydream.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object represents the properties and methods of a Metadata.
	/// </summary>
	public partial class Metadata
	{
		#region Constructor
		
		protected Metadata()
		{
		}
		
		#endregion
		
		#region INullable Implementation
		
		public override bool IsNull
		{
			get
			{
				return false;
			}
		}
		
		#endregion
		
		#region ICloneable Implementation
	
		public object Clone()
		{
			return MemberwiseClone();
		}
		
		#endregion

		#region Static Members
		
		public static Metadata New ()
		{
			return new Metadata() ;
		}

		public static Metadata Empty
		{
			get { return NullMetadata.Instance; }
		}

		public static Metadata Get (Nullable <Int32> MetadataId)
		{
			Metadata Metadata = MetadataMapper.Instance.Get (MetadataId);
			return Metadata ?? Empty;
		}

		public static Metadata Update (Metadata metadata)
		{
			return MetadataMapper.Instance.Update(metadata) ;
		}

		public static void Delete (Nullable <Int32> MetadataId)
		{
			MetadataMapper.Instance.Delete (MetadataId);
		}

		public static EntityList <Metadata> FindMany (MetadataFinder finder, int Page, int PageSize)
		{
			return MetadataMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Metadata> FindMany (MetadataFinder finder)
		{
			return MetadataMapper.Instance.FindMany (finder);
		}

		public static Metadata FindOne (MetadataFinder finder)
		{
			Metadata Metadata = MetadataMapper.Instance.FindOne(finder);
			return Metadata ?? Empty;
		}

		public static int GetCount (MetadataFinder finder)
		{
			return MetadataMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
