/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Text.RegularExpressions;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class IpAddressManager
	{
		public static IpAddress Add(string text)
		{
			IpAddress entity = IpAddress.New();
			entity.IpAddressValue = text;
			Validate(entity);
			return IpAddress.Update(entity);
		}

		public static void Update(int id, string text)
		{
			IpAddress entity = IpAddress.Get(id);
			entity.IpAddressValue = text;
			Validate(entity);
			IpAddress.Update(entity);
		}

		private static void Validate(IpAddress entity)
		{
			if (StringUtils.IsBlank(entity.IpAddressValue))
				throw new InvalidIpAddressException("IP Address cannot be blank");

			const string pattern = @"[0-9]{1,3}(.[0-9]{1,3}){3,3}";
			if (!Regex.IsMatch(entity.IpAddressValue, pattern))
				throw new InvalidIpAddressException("The entered value does not appear to be a valid IP address");

			IpAddressFinder finder = new IpAddressFinder();
			finder.IpAddressValue = entity.IpAddressValue;
			IpAddress ipAddress = IpAddress.FindOne(finder);

			if (!ipAddress.IsNull)
			{
				if (entity.IsNew)
					throw new InvalidIpAddressException("That IP Address already exists");

				if (entity.IpAddressId.Equals(ipAddress.IpAddressId))
					throw new InvalidIpAddressException("That IP Address already exists");

			}
		}
	}
}