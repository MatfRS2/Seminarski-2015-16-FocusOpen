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
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class BrandManager
	{
		#region Private Variables

		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

	    private static readonly string m_defaultFilterMarkup =

@"<table>
    <tr>
    <td valign=""top"">
    <div class=""CatFiltersColumn"">
        <!-- 1st column  -->
        <group>
            <number>1</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>4</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>7</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>10</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <!-- end of 1st column  -->
    </div>
   </td>
   <td valign=""top"">
    <div class=""CatFiltersColumn"">
        <!-- 2nd column  -->
        <group>
            <number>2</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>5</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>8</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>11</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />

        <!-- end of 2nd column  -->
    </div>
    </td>
    <td valign=""top"">
    <div class=""CatFiltersColumn"">
        <!-- 3rd column  -->
        <group>
            <number>3</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>6</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>9</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <group>
            <number>12</number>
            <labelStyle>text-transform: lowercase</labelStyle>
            <inputStyle>formInput W125</inputStyle>
            
            <inputBlankText>All</inputBlankText>
        </group>
        <br />
        <!-- end of 3rd column  -->
    </div>
    </td>
  </tr>
</table>
";

        
		#endregion

		#region Properties

		/// <summary>
		/// Boolean value specifying whether system only has a single brand
		/// </summary>
		public static bool IsSingleBrandMode
		{
			get
			{
				return (BrandCache.Instance.GetList().Count == 1);
			}
		}

		/// <summary>
		/// Boolean value specifying whether system has multiple brands
		/// </summary>
		public static bool IsMultipleBrandMode
		{
			get
			{
				return (BrandCache.Instance.GetList().Count > 1);
			}
		}

		#endregion

		#region Public Methods

		public static void Save(Brand brand, BinaryFile file, BinaryFile watermarkImage)
		{
			Validate(brand, file, watermarkImage);

			Brand.Update(brand);

			if (!IsWebsiteUrlAccessible(brand.WebsiteUrl))
				m_Logger.WarnFormat("The brand {0} has been saved with URL: {1}, but this is not accessible from the local machine, which can result in undesired behavior (for example, asset processing functionality may not work).  To resolve, check DNS or edit the hosts file.", brand.Name, brand.WebsiteUrl);

			CategoryManager.CheckRootCategory(brand.BrandId.GetValueOrDefault());

			CacheManager.InvalidateCache("Brand", CacheType.All);
		}

		public static void ChangeMasterBrand(int masterBrandId)
		{
			foreach (Brand brand in BrandCache.Instance.GetList())
			{
				brand.IsMasterBrand = (brand.BrandId == masterBrandId);
				Brand.Update(brand);
			}

			CacheManager.InvalidateCache("Brand", CacheType.All);
		}

		public static void Delete(int brandId)
		{
			Brand brand = Brand.Get(brandId);
			brand.IsDeleted = true;
			Brand.Update(brand);
			CacheManager.InvalidateCache("Brand", CacheType.All);
		}

		public static BrandMetadataSetting RenameMetadataFieldName(int brandMetadataSettingId, string fieldName)
		{
			fieldName = fieldName.Trim();

			if (StringUtils.IsBlank(fieldName))
				throw new SystemException("Field name cannot be empty");

			BrandMetadataSetting setting = BrandMetadataSetting.Get(brandMetadataSettingId);

			if (setting.IsNull)
				throw new SystemException("Metadata setting not found");

			BrandMetadataSettingFinder finder = new BrandMetadataSettingFinder {BrandId = setting.BrandId, FieldName = fieldName};
			int count = BrandMetadataSetting.GetCount(finder);

			if (count > 0)
				throw new SystemException(string.Format("Another metadata setting with the name '{0}' already exists in this brand", fieldName));

			setting.FieldName = fieldName;
			BrandMetadataSetting.Update(setting);
			
			CacheManager.InvalidateCache("Brand", CacheType.All);

			return setting;
		}

        /// <summary>
        /// returns the brand markup 
        /// </summary>
        /// <param name="brandId"></param>
        /// <returns></returns>
        public static string GetBrandFilterMarkup(int brandId)
        {
            var brand = Brand.Get(brandId);

            //return the default if markup is not set (yet or at all)
            return string.IsNullOrEmpty(brand.FilterMarkup.Trim())
                                                ? m_defaultFilterMarkup 
                                                : brand.FilterMarkup;
        }

		#endregion

		#region Private Methods

		private static void Validate(Brand entity, BinaryFile file, BinaryFile watermarkImage)
		{
			ErrorList errors = new ErrorList();

			if (StringUtils.IsBlank(entity.Name))
				errors.Add("Name is required");

			if (StringUtils.IsBlank(entity.ShortName))
				errors.Add("Short Name is required");

			if (StringUtils.IsBlank(entity.ApplicationName))
				errors.Add("Application Name is required");

			if (StringUtils.IsBlank(entity.OrganisationName))
				errors.Add("Organisation Name is required");

			if (StringUtils.IsBlank(entity.WebsiteUrl))
			{
				errors.Add("Website URL is required");
			}
			else if (!entity.WebsiteUrl.ToLower().StartsWith("http://"))
			{
				errors.Add("Website URL must start with http://");
			}

			if (StringUtils.IsBlank(entity.EmailFrom))
			{
				errors.Add("Email From is required");
			}
			else if (!StringUtils.IsEmail(entity.EmailFrom))
			{
				errors.Add("Email From must be a valid email address");
			}

			if (StringUtils.IsBlank(entity.LoginPageUpperCopy))
				errors.Add("Login page upper copy is required");

			if (StringUtils.IsBlank(entity.LoginPageLowerCopy))
				errors.Add("Login page lower copy is required");

			if (StringUtils.IsBlank(entity.DefaultUsageRestrictionsCopy))
				errors.Add("Default Usage Restrictions notice is required");

			if (StringUtils.IsBlank(entity.MyAccountCopy))
				errors.Add("My Account copy is required");

			if (StringUtils.IsBlank(entity.AdminCopy))
				errors.Add("Admin copy is required");

			if (StringUtils.IsBlank(entity.TermsConditionsCopy))
				errors.Add("Terms and Conditions copy is required");

			if (StringUtils.IsBlank(entity.PrivacyPolicyCopy))
				errors.Add("Privacy Policy copy is required");

			if (entity.IsMasterBrand)
			{
				BrandFinder finder = new BrandFinder {IsMasterBrand = true};
				List<Brand> masterBrandList = Brand.FindMany(finder);

				if (masterBrandList.Count > 1)
					errors.Add("Only one master brand is allowed");
				
				if (masterBrandList.Count == 1 && entity.IsNew)
					errors.Add("Only one master brand is allowed.  Current master brand is: " + masterBrandList[0].Name);

				if (masterBrandList.Count == 1 && !entity.IsNew && !masterBrandList[0].BrandId.Equals(entity.BrandId))
					errors.Add("Only one master brand is allowed.  Current master brand is: " + masterBrandList[0].Name);
			}

			if (!file.IsEmpty && file.FileExtension != "zip")
				errors.Add("UI file pack must be a zip file");

			if (!watermarkImage.IsEmpty && !GeneralUtils.ValueIsInList(watermarkImage.FileExtension, "gif", "jpg", "png"))
				errors.Add("Watermark must be a GIF, JPG or PNG image");

			if (errors.Count > 0)
				throw new InvalidBrandException(errors, entity);
		}

		private static bool IsWebsiteUrlAccessible(string url)
		{
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					if (response.StatusCode == HttpStatusCode.OK)
						return true;
			}
			catch (WebException)
			{
			}

			return false;
		}

		#endregion
	}
}