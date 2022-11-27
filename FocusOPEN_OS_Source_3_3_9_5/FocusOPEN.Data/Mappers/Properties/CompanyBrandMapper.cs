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
	/// <summary>
	/// This object maps data between the database and a CompanyBrand object.
	/// </summary>
	internal partial class CompanyBrandMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			CompanyBrand companyBrand = CompanyBrand.New();

			// Table Fields
			companyBrand.CompanyBrandId = reader.GetInt32("CompanyBrandId"); 
			companyBrand.CompanyId = reader.GetInt32("CompanyId");
			companyBrand.BrandId = reader.GetInt32("BrandId");
			

			companyBrand.IsDirty = false;
			companyBrand.ChangedProperties.Clear();
			
			return companyBrand;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <CompanyBrand>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual CompanyBrand Update (CompanyBrand companyBrand)
		{
 			if (!companyBrand.IsDirty || companyBrand.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return companyBrand;
			}
			
			IDbCommand command = CreateCommand();
			
			if (companyBrand.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [CompanyBrand] ([CompanyId], [BrandId]) VALUES (@companyId, @brandId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [CompanyBrand] SET [CompanyId] = @companyId, [BrandId] = @brandId WHERE CompanyBrandId = @companyBrandId"; 
			}
			
			command.Parameters.Add (CreateParameter("@companyId", companyBrand.CompanyId));
			command.Parameters.Add (CreateParameter("@brandId", companyBrand.BrandId));

			if (companyBrand.IsNew) 
			{
				companyBrand.CompanyBrandId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@companyBrandId", companyBrand.CompanyBrandId));
				ExecuteCommand (command);
			}
			
			companyBrand.IsDirty = false;
			companyBrand.ChangedProperties.Clear();
			
			return companyBrand;
		}

		public virtual void Delete (Nullable <Int32> companyBrandId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [CompanyBrand] WHERE CompanyBrandId = @companyBrandId";
			command.Parameters.Add(CreateParameter("@companyBrandId", companyBrandId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single CompanyBrand object by companyBrandId
		/// </Summary>
		public virtual CompanyBrand Get (Nullable <Int32> companyBrandId)
		{
			IDbCommand command = GetGetCommand (companyBrandId);
			return (CompanyBrand) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual CompanyBrand FindOne (CompanyBrandFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? CompanyBrand.Empty : entity as CompanyBrand;
		}
		
		public virtual EntityList <CompanyBrand> FindMany (CompanyBrandFinder finder)
		{
			return (EntityList <CompanyBrand>) (base.FindMany(finder));
		}

		public virtual EntityList <CompanyBrand> FindMany (CompanyBrandFinder finder, int Page, int PageSize)
		{
			return (EntityList <CompanyBrand>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> companyBrandId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [CompanyBrand] WHERE CompanyBrandId = @companyBrandId";
			command.Parameters.Add(CreateParameter("@companyBrandId", companyBrandId)); 
			
			return command;
		}
	}
}

