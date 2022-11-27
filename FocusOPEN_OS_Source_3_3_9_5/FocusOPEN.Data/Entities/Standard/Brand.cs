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
	/// This object represents the properties and methods of a Brand.
	/// </summary>
	public partial class Brand
	{
		#region Constructor
		
		protected Brand()
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
		
		public static Brand New ()
		{
			return new Brand() ;
		}

		public static Brand Empty
		{
			get { return NullBrand.Instance; }
		}

		public static Brand Get (Nullable <Int32> BrandId)
		{
			Brand Brand = BrandMapper.Instance.Get (BrandId);
			return Brand ?? Empty;
		}

		public static Brand Update (Brand brand)
		{
			return BrandMapper.Instance.Update(brand) ;
		}

		public static void Delete (Nullable <Int32> BrandId)
		{
			BrandMapper.Instance.Delete (BrandId);
		}

		public static EntityList <Brand> FindMany (BrandFinder finder, int Page, int PageSize)
		{
			return BrandMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Brand> FindMany (BrandFinder finder)
		{
			return BrandMapper.Instance.FindMany (finder);
		}

		public static Brand FindOne (BrandFinder finder)
		{
			Brand Brand = BrandMapper.Instance.FindOne(finder);
			return Brand ?? Empty;
		}

		public static int GetCount (BrandFinder finder)
		{
			return BrandMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
