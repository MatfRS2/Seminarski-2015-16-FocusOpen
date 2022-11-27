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
	/// This object represents the properties and methods of a BrandMetadataSetting.
	/// </summary>
	public partial class BrandMetadataSetting
	{
		#region Constructor
		
		protected BrandMetadataSetting()
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
		
		public static BrandMetadataSetting New ()
		{
			return new BrandMetadataSetting() ;
		}

		public static BrandMetadataSetting Empty
		{
			get { return NullBrandMetadataSetting.Instance; }
		}

		public static BrandMetadataSetting Get (Nullable <Int32> BrandMetadataSettingId)
		{
			BrandMetadataSetting BrandMetadataSetting = BrandMetadataSettingMapper.Instance.Get (BrandMetadataSettingId);
			return BrandMetadataSetting ?? Empty;
		}

		public static BrandMetadataSetting Update (BrandMetadataSetting metadataSetting)
		{
			return BrandMetadataSettingMapper.Instance.Update(metadataSetting) ;
		}

		public static void Delete (Nullable <Int32> BrandMetadataSettingId)
		{
			BrandMetadataSettingMapper.Instance.Delete (BrandMetadataSettingId);
		}

		public static EntityList <BrandMetadataSetting> FindMany (BrandMetadataSettingFinder finder, int Page, int PageSize)
		{
			return BrandMetadataSettingMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <BrandMetadataSetting> FindMany (BrandMetadataSettingFinder finder)
		{
			return BrandMetadataSettingMapper.Instance.FindMany (finder);
		}

		public static BrandMetadataSetting FindOne (BrandMetadataSettingFinder finder)
		{
			BrandMetadataSetting BrandMetadataSetting = BrandMetadataSettingMapper.Instance.FindOne(finder);
			return BrandMetadataSetting ?? Empty;
		}

		public static int GetCount (BrandMetadataSettingFinder finder)
		{
			return BrandMetadataSettingMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
