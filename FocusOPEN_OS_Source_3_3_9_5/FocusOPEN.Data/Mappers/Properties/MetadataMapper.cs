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
	internal partial class MetadataMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Metadata metadata = Metadata.New();

			// Table Fields
			metadata.MetadataId = reader.GetInt32("MetadataId"); 
			metadata.BrandId = reader.GetNullableInt32("BrandId");
			metadata.ParentMetadataId = reader.GetNullableInt32("ParentMetadataId");
			metadata.Name = reader.GetString("Name");
			metadata.ExternalRef = reader.GetString("ExternalRef");
			metadata.Synonyms = reader.GetString("Synonyms");
			metadata.GroupNumber = reader.GetInt32("GroupNumber");
            metadata.MetadataOrder = reader.GetInt32("MetadataOrder");
			metadata.IsDeleted = reader.GetBoolean("IsDeleted");
			
			// View Fields

			metadata.IsDirty = false;
			metadata.ChangedProperties.Clear();
			
			return metadata;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Metadata>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Metadata Update (Metadata metadata)
		{
 			if (!metadata.IsDirty || metadata.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return metadata;
			}
			
			IDbCommand command = CreateCommand();
			
			if (metadata.IsNew) 
			{
				// Adding
                command.CommandText = "INSERT INTO [Metadata] ([BrandId], [ParentMetadataId], [Name], [ExternalRef], [Synonyms], [GroupNumber], [IsDeleted], [MetadataOrder]) VALUES (@brandId, @parentMetadataId, @name, @externalRef, @synonyms, @groupNumber, @isDeleted, @metadataOrder) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
                command.CommandText = "UPDATE [Metadata] SET [BrandId] = @brandId, [ParentMetadataId] = @parentMetadataId, [Name] = @name, [ExternalRef] = @externalRef, [Synonyms] = @synonyms, [GroupNumber] = @groupNumber, [IsDeleted] = @isDeleted, [MetadataOrder] = @metadataOrder WHERE MetadataId = @metadataId"; 
			}
			
			command.Parameters.Add (CreateParameter("@brandId", metadata.BrandId));
			command.Parameters.Add (CreateParameter("@parentMetadataId", metadata.ParentMetadataId));
			command.Parameters.Add (CreateParameter("@name", metadata.Name));
			command.Parameters.Add (CreateParameter("@externalRef", metadata.ExternalRef));
			command.Parameters.Add (CreateParameter("@synonyms", metadata.Synonyms));
			command.Parameters.Add (CreateParameter("@groupNumber", metadata.GroupNumber));
			command.Parameters.Add (CreateParameter("@isDeleted", metadata.IsDeleted));
            command.Parameters.Add (CreateParameter("@metadataOrder", metadata.MetadataOrder));

			if (metadata.IsNew) 
			{
				metadata.MetadataId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@metadataId", metadata.MetadataId));
				ExecuteCommand (command);
			}
			
			metadata.IsDirty = false;
			metadata.ChangedProperties.Clear();
			
			return metadata;
		}

		public virtual void Delete (Nullable <Int32> metadataId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Metadata] WHERE MetadataId = @metadataId";
			command.Parameters.Add(CreateParameter("@metadataId", metadataId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Metadata object by metadataId
		/// </Summary>
		public virtual Metadata Get (Nullable <Int32> metadataId)
		{
			IDbCommand command = GetGetCommand (metadataId);
			return (Metadata) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Metadata FindOne (MetadataFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Metadata.Empty : entity as Metadata;
		}
		
		public virtual EntityList <Metadata> FindMany (MetadataFinder finder)
		{
			return (EntityList <Metadata>) (base.FindMany(finder));
		}

		public virtual EntityList <Metadata> FindMany (MetadataFinder finder, int Page, int PageSize)
		{
			return (EntityList <Metadata>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> metadataId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Metadata] WHERE MetadataId = @metadataId";
			command.Parameters.Add(CreateParameter("@metadataId", metadataId)); 
			
			return command;
		}
	}
}

