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
using System.Reflection;
using System.Text.RegularExpressions;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class CompanyManager
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static void EnsureCompanyExists(User user)
		{
			if (CompanyCache.Instance.GetList().Count >= 1)
				return;

			try
			{
				Company company = Company.New();

				company.Name = user.CompanyName;
				company.Domain = user.Email.Substring(user.Email.IndexOf("@") + 1);
				company.IsInternal = true;
				company.CreateDate = DateTime.Now;
				company.CreatedByUserId = user.UserId.GetValueOrDefault();

				SaveCompany(company);
			}
			catch (Exception ex)
			{
				m_Logger.Error(string.Format("Error ensuring company exists: {0}", ex.Message), ex);
			}
		}

		public static void SaveCompany(Company entity)
		{
			ErrorList errors = ValidateCompany(entity);

			if (errors.Count > 0)
				throw new InvalidCompanyException(errors, entity);

			Company.Update(entity);
			CacheManager.InvalidateCache("Company", CacheType.All);
		}

		private static ErrorList ValidateCompany(Company entity)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(entity.Name))
				errors.Add("name is required");

			if (StringUtils.IsBlank(entity.Domain))
			{
				errors.Add("domain is required");
			}
			else
			{
				const string pattern = @"[a-z0-9-\.]+";

				if (Regex.Match(entity.Domain, pattern, RegexOptions.IgnoreCase).Value != entity.Domain)
				{
					errors.Add("Invalid e-mail domain");
				}
				else
				{
					CompanyFinder finder = new CompanyFinder {Domain = entity.Domain};
					Company ac = Company.FindOne(finder);

					if (!ac.IsNull)
					{
						if (entity.IsNew)
							errors.Add("A company with that email domain already exists");

						if (!entity.CompanyId.Equals(ac.CompanyId))
							errors.Add("A company with that email domain already exists");
					}
				}
			}

			if (entity.BrandListLoaded && entity.BrandList.Count == 0)
				errors.Add("at least one brand must be selected");

			return errors;
		}

		public static void DeleteCompany(int companyId)
		{
			if (CompanyCache.Instance.GetList().Count <= 1)
				throw new SystemException("There must be at least one company");

			// Otherwise, all good. Delete company.
			Company.Delete(companyId);

			// Invalidate cache.
			CacheManager.InvalidateCache("Company", CacheType.All);
		}
	}
}