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
using System.Data;
using Daydream.Data;

namespace FocusOPEN.Data
{
	internal partial class BrandMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Brand brand = Brand.New();

			// Table Fields
			brand.BrandId = reader.GetInt32("BrandId"); 
			brand.Name = reader.GetString("Name");
			brand.ShortName = reader.GetString("ShortName");
			brand.ApplicationName = reader.GetString("ApplicationName");
			brand.OrganisationName = reader.GetString("OrganisationName");
			brand.IsMasterBrand = reader.GetBoolean("IsMasterBrand");
			brand.WebsiteUrl = reader.GetString("WebsiteUrl");
			brand.EmailFrom = reader.GetString("EmailFrom");
			brand.IsBrandSelectionAllowed = reader.GetBoolean("IsBrandSelectionAllowed");
			brand.DisablePoweredByLogo = reader.GetBoolean("DisablePoweredByLogo");
			brand.LoginPageUpperCopy = reader.GetString("LoginPageUpperCopy");
			brand.LoginPageLowerCopy = reader.GetString("LoginPageLowerCopy");
			brand.DefaultUsageRestrictionsCopy = reader.GetString("DefaultUsageRestrictionsCopy");
			brand.MyAccountCopy = reader.GetString("MyAccountCopy");
			brand.AdminCopy = reader.GetString("AdminCopy");
			brand.TermsConditionsCopy = reader.GetString("TermsConditionsCopy");
			brand.PrivacyPolicyCopy = reader.GetString("PrivacyPolicyCopy");
			brand.HideFilterSearch = reader.GetBoolean("HideFilterSearch");
			brand.HideCategorySearch = reader.GetBoolean("HideCategorySearch");
			brand.DirectDownloadEnabled = reader.GetBoolean("DirectDownloadEnabled");
			brand.IsDeleted = reader.GetBoolean("IsDeleted");
			brand.FilterMarkup = reader.GetString("FilterMarkup");
			
			// View Fields

			brand.IsDirty = false;
			brand.ChangedProperties.Clear();
			
			return brand;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Brand>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Brand Update (Brand brand)
		{
 			if (!brand.IsDirty || brand.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return brand;
			}
			
			IDbCommand command = CreateCommand();
			
			if (brand.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Brand] ([Name], [ShortName], [ApplicationName], [OrganisationName], [IsMasterBrand], [WebsiteUrl], [EmailFrom], [IsBrandSelectionAllowed], [DisablePoweredByLogo], [LoginPageUpperCopy], [LoginPageLowerCopy], [DefaultUsageRestrictionsCopy], [MyAccountCopy], [AdminCopy], [TermsConditionsCopy], [PrivacyPolicyCopy], [HideFilterSearch], [HideCategorySearch], [DirectDownloadEnabled], [IsDeleted], [FilterMarkup]) VALUES (@name, @shortName, @applicationName, @organisationName, @isMasterBrand, @websiteUrl, @emailFrom, @isBrandSelectionAllowed, @disablePoweredByLogo, @loginPageUpperCopy, @loginPageLowerCopy, @defaultUsageRestrictionsCopy, @myAccountCopy, @adminCopy, @termsConditionsCopy, @privacyPolicyCopy, @hideFilterSearch, @hideCategorySearch, @directDownloadEnabled, @isDeleted, @filterMarkup) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Brand] SET [Name] = @name, [ShortName] = @shortName, [ApplicationName] = @applicationName, [OrganisationName] = @organisationName, [IsMasterBrand] = @isMasterBrand, [WebsiteUrl] = @websiteUrl, [EmailFrom] = @emailFrom, [IsBrandSelectionAllowed] = @isBrandSelectionAllowed, [DisablePoweredByLogo] = @disablePoweredByLogo, [LoginPageUpperCopy] = @loginPageUpperCopy, [LoginPageLowerCopy] = @loginPageLowerCopy, [DefaultUsageRestrictionsCopy] = @defaultUsageRestrictionsCopy, [MyAccountCopy] = @myAccountCopy, [AdminCopy] = @adminCopy, [TermsConditionsCopy] = @termsConditionsCopy, [PrivacyPolicyCopy] = @privacyPolicyCopy, [HideFilterSearch] = @hideFilterSearch, [HideCategorySearch] = @hideCategorySearch, [DirectDownloadEnabled] = @directDownloadEnabled, [IsDeleted] = @isDeleted, [FilterMarkup] = @filterMarkup WHERE BrandId = @brandId"; 
			}
			
			command.Parameters.Add (CreateParameter("@name", brand.Name));
			command.Parameters.Add (CreateParameter("@shortName", brand.ShortName));
			command.Parameters.Add (CreateParameter("@applicationName", brand.ApplicationName));
			command.Parameters.Add (CreateParameter("@organisationName", brand.OrganisationName));
			command.Parameters.Add (CreateParameter("@isMasterBrand", brand.IsMasterBrand));
			command.Parameters.Add (CreateParameter("@websiteUrl", brand.WebsiteUrl));
			command.Parameters.Add (CreateParameter("@emailFrom", brand.EmailFrom));
			command.Parameters.Add (CreateParameter("@isBrandSelectionAllowed", brand.IsBrandSelectionAllowed));
			command.Parameters.Add (CreateParameter("@disablePoweredByLogo", brand.DisablePoweredByLogo));
			command.Parameters.Add (CreateParameter("@loginPageUpperCopy", brand.LoginPageUpperCopy));
			command.Parameters.Add (CreateParameter("@loginPageLowerCopy", brand.LoginPageLowerCopy));
			command.Parameters.Add (CreateParameter("@defaultUsageRestrictionsCopy", brand.DefaultUsageRestrictionsCopy));
			command.Parameters.Add (CreateParameter("@myAccountCopy", brand.MyAccountCopy));
			command.Parameters.Add (CreateParameter("@adminCopy", brand.AdminCopy));
			command.Parameters.Add (CreateParameter("@termsConditionsCopy", brand.TermsConditionsCopy));
			command.Parameters.Add (CreateParameter("@privacyPolicyCopy", brand.PrivacyPolicyCopy));
			command.Parameters.Add (CreateParameter("@hideFilterSearch", brand.HideFilterSearch));
			command.Parameters.Add (CreateParameter("@hideCategorySearch", brand.HideCategorySearch));
			command.Parameters.Add (CreateParameter("@directDownloadEnabled", brand.DirectDownloadEnabled));
			command.Parameters.Add (CreateParameter("@isDeleted", brand.IsDeleted));
			command.Parameters.Add (CreateParameter("@filterMarkup", brand.FilterMarkup));

			if (brand.IsNew) 
			{
				brand.BrandId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@brandId", brand.BrandId));
				ExecuteCommand (command);
			}
			
			brand.IsDirty = false;
			brand.ChangedProperties.Clear();
			
			return brand;
		}

		public virtual void Delete (Nullable <Int32> brandId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Brand] WHERE BrandId = @brandId";
			command.Parameters.Add(CreateParameter("@brandId", brandId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Brand object by brandId
		// </Summary>
		public virtual Brand Get (Nullable <Int32> brandId)
		{
			IDbCommand command = GetGetCommand (brandId);
			return (Brand) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Brand FindOne (BrandFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Brand.Empty : entity as Brand;
		}
		
		public virtual EntityList <Brand> FindMany (BrandFinder finder)
		{
			return (EntityList <Brand>) (base.FindMany(finder));
		}

		public virtual EntityList <Brand> FindMany (BrandFinder finder, int Page, int PageSize)
		{
			return (EntityList <Brand>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> brandId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Brand] WHERE BrandId = @brandId";
			command.Parameters.Add(CreateParameter("@brandId", brandId)); 
			
			return command;
		}
	}
}

