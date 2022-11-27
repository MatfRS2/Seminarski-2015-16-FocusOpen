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
	/// This object represents the properties and methods of a BrandMetadataSelectableSetting.
	/// </summary>
	public partial class BrandMetadataSelectableSetting
	{
		#region Constructor
		
		protected BrandMetadataSelectableSetting()
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
		
		public static BrandMetadataSelectableSetting New ()
		{
			return new BrandMetadataSelectableSetting() ;
		}

		public static BrandMetadataSelectableSetting Empty
		{
			get { return NullBrandMetadataSelectableSetting.Instance; }
		}

		public static BrandMetadataSelectableSetting Get (Nullable <Int32> BrandMetadataSelectableSettingId)
		{
			BrandMetadataSelectableSetting BrandMetadataSelectableSetting = BrandMetadataSelectableSettingMapper.Instance.Get (BrandMetadataSelectableSettingId);
			return BrandMetadataSelectableSetting ?? Empty;
		}

		public static BrandMetadataSelectableSetting Update (BrandMetadataSelectableSetting metadataSetting)
		{
			return BrandMetadataSelectableSettingMapper.Instance.Update(metadataSetting) ;
		}

		public static void Delete (Nullable <Int32> BrandMetadataSelectableSettingId)
		{
			BrandMetadataSelectableSettingMapper.Instance.Delete (BrandMetadataSelectableSettingId);
		}

		public static EntityList <BrandMetadataSelectableSetting> FindMany (BrandMetadataSelectableSettingFinder finder, int Page, int PageSize)
		{
			return BrandMetadataSelectableSettingMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <BrandMetadataSelectableSetting> FindMany (BrandMetadataSelectableSettingFinder finder)
		{
			return BrandMetadataSelectableSettingMapper.Instance.FindMany (finder);
		}

		public static BrandMetadataSelectableSetting FindOne (BrandMetadataSelectableSettingFinder finder)
		{
			BrandMetadataSelectableSetting BrandMetadataSelectableSetting = BrandMetadataSelectableSettingMapper.Instance.FindOne(finder);
			return BrandMetadataSelectableSetting ?? Empty;
		}

		public static int GetCount (BrandMetadataSelectableSettingFinder finder)
		{
			return BrandMetadataSelectableSettingMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
