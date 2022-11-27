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
	/// This object maps data between the database and a Country object.
	/// </summary>
	internal partial class CountryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Country country = Country.New();

			// Table Fields
			country.CountryId = reader.GetInt32("CountryId"); 
			country.Code = reader.GetString("Code");
			country.Name = reader.GetString("Name");
			country.Rank = reader.GetInt32("Rank");
			

			country.IsDirty = false;
			country.ChangedProperties.Clear();
			
			return country;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Country>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Country Update (Country country)
		{
 			if (!country.IsDirty || country.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return country;
			}
			
			IDbCommand command = CreateCommand();
			
			if (country.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Country] ([Code], [Name], [Rank]) VALUES (@code, @name, @rank) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Country] SET [Code] = @code, [Name] = @name, [Rank] = @rank WHERE CountryId = @countryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@code", country.Code));
			command.Parameters.Add (CreateParameter("@name", country.Name));
			command.Parameters.Add (CreateParameter("@rank", country.Rank));

			if (country.IsNew) 
			{
				country.CountryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@countryId", country.CountryId));
				ExecuteCommand (command);
			}
			
			country.IsDirty = false;
			country.ChangedProperties.Clear();
			
			return country;
		}

		public virtual void Delete (Nullable <Int32> countryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Country] WHERE CountryId = @countryId";
			command.Parameters.Add(CreateParameter("@countryId", countryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Country object by countryId
		/// </Summary>
		public virtual Country Get (Nullable <Int32> countryId)
		{
			IDbCommand command = GetGetCommand (countryId);
			return (Country) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Country FindOne (CountryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Country.Empty : entity as Country;
		}
		
		public virtual EntityList <Country> FindMany (CountryFinder finder)
		{
			return (EntityList <Country>) (base.FindMany(finder));
		}

		public virtual EntityList <Country> FindMany (CountryFinder finder, int Page, int PageSize)
		{
			return (EntityList <Country>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> countryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [Country] WHERE CountryId = @countryId";
			command.Parameters.Add(CreateParameter("@countryId", countryId)); 
			
			return command;
		}
	}
}

