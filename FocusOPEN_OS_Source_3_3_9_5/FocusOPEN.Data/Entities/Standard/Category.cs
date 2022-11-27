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
	/// This object represents the properties and methods of a Category.
	/// </summary>
	public partial class Category
	{
		#region Constructor
		
		protected Category()
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
		
		public static Category New ()
		{
			return new Category() ;
		}

		public static Category Empty
		{
			get { return NullCategory.Instance; }
		}

		public static Category Get (Nullable <Int32> CategoryId)
		{
			Category Category = CategoryMapper.Instance.Get (CategoryId);
			return Category ?? Empty;
		}

		public static Category Update (Category category)
		{
			return CategoryMapper.Instance.Update(category) ;
		}

		public static void Delete (Nullable <Int32> CategoryId)
		{
			CategoryMapper.Instance.Delete (CategoryId);
		}

		public static EntityList <Category> FindMany (CategoryFinder finder, int Page, int PageSize)
		{
			return CategoryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Category> FindMany (CategoryFinder finder)
		{
			return CategoryMapper.Instance.FindMany (finder);
		}

		public static Category FindOne (CategoryFinder finder)
		{
			Category Category = CategoryMapper.Instance.FindOne(finder);
			return Category ?? Empty;
		}

		public static int GetCount (CategoryFinder finder)
		{
			return CategoryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
