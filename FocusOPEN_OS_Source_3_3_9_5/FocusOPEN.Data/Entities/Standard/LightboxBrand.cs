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
	/// This object represents the properties and methods of a LightboxBrand.
	/// </summary>
	public partial class LightboxBrand
	{
		#region Constructor
		
		protected LightboxBrand()
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
		
		public static LightboxBrand New ()
		{
			return new LightboxBrand() ;
		}

		public static LightboxBrand Empty
		{
			get { return NullLightboxBrand.Instance; }
		}

		public static LightboxBrand Get (Nullable <Int32> LightboxBrandId)
		{
			LightboxBrand LightboxBrand = LightboxBrandMapper.Instance.Get (LightboxBrandId);
			return LightboxBrand ?? Empty;
		}

		public static LightboxBrand Update (LightboxBrand lightboxBrand)
		{
			return LightboxBrandMapper.Instance.Update(lightboxBrand) ;
		}

		public static void Delete (Nullable <Int32> LightboxBrandId)
		{
			LightboxBrandMapper.Instance.Delete (LightboxBrandId);
		}

		public static EntityList <LightboxBrand> FindMany (LightboxBrandFinder finder, int Page, int PageSize)
		{
			return LightboxBrandMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <LightboxBrand> FindMany (LightboxBrandFinder finder)
		{
			return LightboxBrandMapper.Instance.FindMany (finder);
		}

		public static LightboxBrand FindOne (LightboxBrandFinder finder)
		{
			LightboxBrand LightboxBrand = LightboxBrandMapper.Instance.FindOne(finder);
			return LightboxBrand ?? Empty;
		}

		public static int GetCount (LightboxBrandFinder finder)
		{
			return LightboxBrandMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
