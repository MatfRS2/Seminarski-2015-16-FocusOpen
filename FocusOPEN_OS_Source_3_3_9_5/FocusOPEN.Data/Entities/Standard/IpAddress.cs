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
	/// This object represents the properties and methods of a IpAddress.
	/// </summary>
	public partial class IpAddress
	{
		#region Constructor
		
		protected IpAddress()
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
		
		public static IpAddress New ()
		{
			return new IpAddress() ;
		}

		public static IpAddress Empty
		{
			get { return NullIpAddress.Instance; }
		}

		public static IpAddress Get (Nullable <Int32> IpAddressId)
		{
			IpAddress IpAddress = IpAddressMapper.Instance.Get (IpAddressId);
			return IpAddress ?? Empty;
		}

		public static IpAddress Update (IpAddress approvedIpAddress)
		{
			return IpAddressMapper.Instance.Update(approvedIpAddress) ;
		}

		public static void Delete (Nullable <Int32> IpAddressId)
		{
			IpAddressMapper.Instance.Delete (IpAddressId);
		}

		public static EntityList <IpAddress> FindMany (IpAddressFinder finder, int Page, int PageSize)
		{
			return IpAddressMapper.Instance.FindMany (finder, Page, PageSize);
		}

		public static EntityList <IpAddress> FindMany (IpAddressFinder finder)
		{
			return IpAddressMapper.Instance.FindMany (finder);
		}

		public static IpAddress FindOne (IpAddressFinder finder)
		{
			IpAddress IpAddress = IpAddressMapper.Instance.FindOne(finder);
			return IpAddress ?? Empty;
		}

		public static int GetCount (IpAddressFinder finder)
		{
			return IpAddressMapper.Instance.GetCount(finder); 
		}
		
		#endregion
	}
}
