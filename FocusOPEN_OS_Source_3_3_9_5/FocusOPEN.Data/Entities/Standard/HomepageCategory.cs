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
	/// This object represents the properties and methods of a HomepageCategory.
	/// </summary>
	public partial class HomepageCategory
	{
		#region Constructor
		
		protected HomepageCategory()
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
		
		public static HomepageCategory New ()
		{
			return new HomepageCategory() ;
		}

		public static HomepageCategory Empty
		{
			get { return NullHomepageCategory.Instance; }
		}

		public static HomepageCategory Get (Nullable <Int32> HomepageCategoryId)
		{
			HomepageCategory HomepageCategory = HomepageCategoryMapper.Instance.Get (HomepageCategoryId);
			return HomepageCategory ?? Empty;
		}

		public static HomepageCategory Update (HomepageCategory homepageCategory)
		{
			return HomepageCategoryMapper.Instance.Update(homepageCategory) ;
		}

		public static void Delete (Nullable <Int32> HomepageCategoryId)
		{
			HomepageCategoryMapper.Instance.Delete (HomepageCategoryId);
		}

		public static EntityList <HomepageCategory> FindMany (HomepageCategoryFinder finder, int Page, int PageSize)
		{
			return HomepageCategoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <HomepageCategory> FindMany (HomepageCategoryFinder finder)
		{
			return HomepageCategoryMapper.Instance.FindMany (finder);
		}

		public static HomepageCategory FindOne (HomepageCategoryFinder finder)
		{
			HomepageCategory HomepageCategory = HomepageCategoryMapper.Instance.FindOne(finder);
			return HomepageCategory ?? Empty;
		}

		public static int GetCount (HomepageCategoryFinder finder)
		{
			return HomepageCategoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
