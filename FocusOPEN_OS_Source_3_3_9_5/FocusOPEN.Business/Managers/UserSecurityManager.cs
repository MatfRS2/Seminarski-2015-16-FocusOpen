/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Business
{
	public static class UserSecurityManager
	{
		/// <summary>
		/// Checks if the IP address belongs to the approved IP address list
		/// </summary>
		public static bool IsApprovedIpAddress(string ipAddress)
		{
			if (StringUtils.IsBlank(ipAddress))
				return false;

			IpAddressFinder ipRestrictionFinder = new IpAddressFinder { IpAddressValue = ipAddress };
			return (IpAddress.GetCount(ipRestrictionFinder) != 0);
		}

		/// <summary>
		/// Gets the approved company by email adddress or domain name.  If the
		/// parameter is an email address it, everything before the @ will automatically
		/// be ignored, and the remainder will be used to find the company.
		/// </summary>
		public static Company GetCompanyByDomain(string domainNameOrEmailAddress)
		{
			string domainName = domainNameOrEmailAddress;

			if (StringUtils.IsEmail(domainName))
				domainName = domainName.Substring(domainName.IndexOf('@') + 1);

			if (StringUtils.IsBlank(domainName))
				return Company.Empty;

			foreach (Company ac in CompanyCache.Instance.GetList())
			{
				if (StringUtils.IgnoreCaseCompare(ac.Domain, domainName))
					return ac;
			}

			return Company.Empty;
		}
	}
}