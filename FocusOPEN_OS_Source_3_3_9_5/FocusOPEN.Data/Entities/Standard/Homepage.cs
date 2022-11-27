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
	/// This object represents the properties and methods of a Homepage.
	/// </summary>
	public partial class Homepage
	{
		#region Constructor
		
		protected Homepage()
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
		
		public static Homepage New ()
		{
			return new Homepage() ;
		}

		public static Homepage Empty
		{
			get { return NullHomepage.Instance; }
		}

		public static Homepage Get (Nullable <Int32> HomepageId)
		{
			Homepage Homepage = HomepageMapper.Instance.Get (HomepageId);
			return Homepage ?? Empty;
		}

		public static Homepage Update (Homepage homepage)
		{
			return HomepageMapper.Instance.Update(homepage) ;
		}

		public static void Delete (Nullable <Int32> HomepageId)
		{
			HomepageMapper.Instance.Delete (HomepageId);
		}

		public static EntityList <Homepage> FindMany (HomepageFinder finder, int Page, int PageSize)
		{
			return HomepageMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Homepage> FindMany (HomepageFinder finder)
		{
			return HomepageMapper.Instance.FindMany (finder);
		}

		public static Homepage FindOne (HomepageFinder finder)
		{
			Homepage Homepage = HomepageMapper.Instance.FindOne(finder);
			return Homepage ?? Empty;
		}

		public static int GetCount (HomepageFinder finder)
		{
			return HomepageMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
