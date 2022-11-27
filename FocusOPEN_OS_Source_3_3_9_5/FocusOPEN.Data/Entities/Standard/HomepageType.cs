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
	/// This object represents the properties and methods of a HomepageType.
	/// </summary>
	public partial class HomepageType
	{
		#region Constructor
		
		protected HomepageType()
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
		
		public static HomepageType New ()
		{
			return new HomepageType() ;
		}

		public static HomepageType Empty
		{
			get { return NullHomepageType.Instance; }
		}

		public static HomepageType Get (Nullable <Int32> HomepageTypeId)
		{
			HomepageType HomepageType = HomepageTypeMapper.Instance.Get (HomepageTypeId);
			return HomepageType ?? Empty;
		}

		public static HomepageType Update (HomepageType homepageType)
		{
			return HomepageTypeMapper.Instance.Update(homepageType) ;
		}

		public static void Delete (Nullable <Int32> HomepageTypeId)
		{
			HomepageTypeMapper.Instance.Delete (HomepageTypeId);
		}

		public static EntityList <HomepageType> FindMany (HomepageTypeFinder finder, int Page, int PageSize)
		{
			return HomepageTypeMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <HomepageType> FindMany (HomepageTypeFinder finder)
		{
			return HomepageTypeMapper.Instance.FindMany (finder);
		}

		public static HomepageType FindOne (HomepageTypeFinder finder)
		{
			HomepageType HomepageType = HomepageTypeMapper.Instance.FindOne(finder);
			return HomepageType ?? Empty;
		}

		public static int GetCount (HomepageTypeFinder finder)
		{
			return HomepageTypeMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
