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
	/// This object maps data between the database and a Company object.
	/// </summary>
	internal partial class CompanyMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Company company = Company.New();

			// Table Fields
			company.CompanyId = reader.GetInt32("CompanyId"); 
			company.Name = reader.GetString("Name");
			company.Brands = reader.GetString("Brands");
			company.Domain = reader.GetString("Domain");
			company.IsInternal = reader.GetBoolean("IsInternal");
			company.CreatedByUserId = reader.GetInt32("CreatedByUserId");
			company.CreateDate = reader.GetDateTime("CreateDate");
			
			// View Fields

			company.IsDirty = false;
			company.ChangedProperties.Clear();
			
			return company;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Company>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Company Update (Company company)
		{
 			if (!company.IsDirty || company.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return company;
			}
			
			IDbCommand command = CreateCommand();
			
			if (company.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Company] ([Name], [Brands], [Domain], [IsInternal], [CreatedByUserId], [CreateDate]) VALUES (@name, @brands, @domain, @isInternal, @createdByUserId, @createDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Company] SET [Name] = @name, [Brands] = @brands, [Domain] = @domain, [IsInternal] = @isInternal, [CreatedByUserId] = @createdByUserId, [CreateDate] = @createDate WHERE CompanyId = @companyId"; 
			}
			
			command.Parameters.Add (CreateParameter("@name", company.Name));
			command.Parameters.Add (CreateParameter("@brands", company.Brands));
			command.Parameters.Add (CreateParameter("@domain", company.Domain));
			command.Parameters.Add (CreateParameter("@isInternal", company.IsInternal));
			command.Parameters.Add (CreateParameter("@createdByUserId", company.CreatedByUserId));
			command.Parameters.Add (CreateParameter("@createDate", company.CreateDate));

			if (company.IsNew) 
			{
				company.CompanyId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@companyId", company.CompanyId));
				ExecuteCommand (command);
			}
			
			company.IsDirty = false;
			company.ChangedProperties.Clear();
			
			return company;
		}

		public virtual void Delete (Nullable <Int32> companyId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Company] WHERE CompanyId = @companyId";
			command.Parameters.Add(CreateParameter("@companyId", companyId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Company object by companyId
		/// </Summary>
		public virtual Company Get (Nullable <Int32> companyId)
		{
			IDbCommand command = GetGetCommand (companyId);
			return (Company) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Company FindOne (CompanyFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Company.Empty : entity as Company;
		}
		
		public virtual EntityList <Company> FindMany (CompanyFinder finder)
		{
			return (EntityList <Company>) (base.FindMany(finder));
		}

		public virtual EntityList <Company> FindMany (CompanyFinder finder, int Page, int PageSize)
		{
			return (EntityList <Company>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> companyId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Company] WHERE CompanyId = @companyId";
			command.Parameters.Add(CreateParameter("@companyId", companyId)); 
			
			return command;
		}
	}
}

