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
	/// This object maps data between the database and a LightboxBrand object.
	/// </summary>
	internal partial class LightboxBrandMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			LightboxBrand lightboxBrand = LightboxBrand.New();

			// Table Fields
			lightboxBrand.LightboxBrandId = reader.GetInt32("LightboxBrandId"); 
			lightboxBrand.LightboxId = reader.GetInt32("LightboxId");
			lightboxBrand.BrandId = reader.GetInt32("BrandId");
			

			lightboxBrand.IsDirty = false;
			lightboxBrand.ChangedProperties.Clear();
			
			return lightboxBrand;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <LightboxBrand>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual LightboxBrand Update (LightboxBrand lightboxBrand)
		{
 			if (!lightboxBrand.IsDirty || lightboxBrand.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return lightboxBrand;
			}
			
			IDbCommand command = CreateCommand();
			
			if (lightboxBrand.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [LightboxBrand] ([LightboxId], [BrandId]) VALUES (@lightboxId, @brandId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [LightboxBrand] SET [LightboxId] = @lightboxId, [BrandId] = @brandId WHERE LightboxBrandId = @lightboxBrandId"; 
			}
			
			command.Parameters.Add (CreateParameter("@lightboxId", lightboxBrand.LightboxId));
			command.Parameters.Add (CreateParameter("@brandId", lightboxBrand.BrandId));

			if (lightboxBrand.IsNew) 
			{
				lightboxBrand.LightboxBrandId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@lightboxBrandId", lightboxBrand.LightboxBrandId));
				ExecuteCommand (command);
			}
			
			lightboxBrand.IsDirty = false;
			lightboxBrand.ChangedProperties.Clear();
			
			return lightboxBrand;
		}

		public virtual void Delete (Nullable <Int32> lightboxBrandId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [LightboxBrand] WHERE LightboxBrandId = @lightboxBrandId";
			command.Parameters.Add(CreateParameter("@lightboxBrandId", lightboxBrandId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single LightboxBrand object by lightboxBrandId
		/// </Summary>
		public virtual LightboxBrand Get (Nullable <Int32> lightboxBrandId)
		{
			IDbCommand command = GetGetCommand (lightboxBrandId);
			return (LightboxBrand) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual LightboxBrand FindOne (LightboxBrandFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? LightboxBrand.Empty : entity as LightboxBrand;
		}
		
		public virtual EntityList <LightboxBrand> FindMany (LightboxBrandFinder finder)
		{
			return (EntityList <LightboxBrand>) (base.FindMany(finder));
		}

		public virtual EntityList <LightboxBrand> FindMany (LightboxBrandFinder finder, int Page, int PageSize)
		{
			return (EntityList <LightboxBrand>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> lightboxBrandId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [LightboxBrand] WHERE LightboxBrandId = @lightboxBrandId";
			command.Parameters.Add(CreateParameter("@lightboxBrandId", lightboxBrandId)); 
			
			return command;
		}
	}
}

