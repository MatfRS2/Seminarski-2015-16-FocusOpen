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
	/// This object represents the properties and methods of a Country.
	/// </summary>
	public partial class Country
	{
		#region Constructor
		
		protected Country()
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
		
		public static Country New ()
		{
			return new Country() ;
		}

		public static Country Empty
		{
			get { return NullCountry.Instance; }
		}

		public static Country Get (Nullable <Int32> CountryId)
		{
			Country Country = CountryMapper.Instance.Get (CountryId);
			return Country ?? Empty;
		}

		public static Country Update (Country country)
		{
			return CountryMapper.Instance.Update(country) ;
		}

		public static void Delete (Nullable <Int32> CountryId)
		{
			CountryMapper.Instance.Delete (CountryId);
		}

		public static EntityList <Country> FindMany (CountryFinder finder, int Page, int PageSize)
		{
			return CountryMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <Country> FindMany (CountryFinder finder)
		{
			return CountryMapper.Instance.FindMany (finder);
		}

		public static Country FindOne (CountryFinder finder)
		{
			Country Country = CountryMapper.Instance.FindOne(finder);
			return Country ?? Empty;
		}

		public static int GetCount (CountryFinder finder)
		{
			return CountryMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
