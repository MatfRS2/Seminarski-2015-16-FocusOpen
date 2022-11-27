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
	/// This object represents the properties and methods of a Lightbox.
	/// </summary>
	public partial class Lightbox
	{
		#region Constructor
		
		protected Lightbox()
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
		
		public static Lightbox New ()
		{
			return new Lightbox() ;
		}

		public static Lightbox Empty
		{
			get { return NullLightbox.Instance; }
		}

		public static Lightbox Get (Nullable <Int32> LightboxId)
		{
			Lightbox Lightbox = LightboxMapper.Instance.Get (LightboxId);
			return Lightbox ?? Empty;
		}

		public static Lightbox Update (Lightbox lightbox)
		{
			Lightbox lb = LightboxMapper.Instance.Update(lightbox);

			LightboxBrandMapper.Instance.DeleteLightboxBrands(lb.LightboxId);

			foreach (Brand brand in lb.Brands)
			{
				LightboxBrand o = LightboxBrand.New();
				o.LightboxId = lb.LightboxId.GetValueOrDefault();
				o.BrandId = brand.BrandId.GetValueOrDefault();
				LightboxBrand.Update(o);
			}

			return lb;
		}

		public static void Delete (Nullable <Int32> LightboxId)
		{
			LightboxMapper.Instance.Delete (LightboxId);
		}

		public static EntityList <Lightbox> FindMany (LightboxFinder finder, int Page, int PageSize)
		{
			return LightboxMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Lightbox> FindMany (LightboxFinder finder)
		{
			return LightboxMapper.Instance.FindMany (finder);
		}

		public static Lightbox FindOne (LightboxFinder finder)
		{
			Lightbox Lightbox = LightboxMapper.Instance.FindOne(finder);
			return Lightbox ?? Empty;
		}

		public static int GetCount (LightboxFinder finder)
		{
			return LightboxMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
