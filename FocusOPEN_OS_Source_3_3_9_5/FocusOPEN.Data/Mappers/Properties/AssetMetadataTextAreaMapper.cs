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
	internal partial class AssetMetadataTextAreaMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetMetadataTextArea assetMetadataTextArea = AssetMetadataTextArea.New();

			// Table Fields
			assetMetadataTextArea.AssetMetadataTextAreaId = reader.GetInt32("AssetMetadataTextAreaId"); 
			assetMetadataTextArea.AssetId = reader.GetInt32("AssetId");
			assetMetadataTextArea.GroupNumber = reader.GetInt32("GroupNumber");
            assetMetadataTextArea.TextAreaValue = reader.GetString("TextAreaValue");
			
			assetMetadataTextArea.IsDirty = false;
			assetMetadataTextArea.ChangedProperties.Clear();
			
			return assetMetadataTextArea;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetMetadataTextArea>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetMetadataTextArea Update (AssetMetadataTextArea assetMetadataTextArea)
		{
 			if (!assetMetadataTextArea.IsDirty || assetMetadataTextArea.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetMetadataTextArea;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetMetadataTextArea.IsNew) 
			{
				// Adding
				command.CommandText = @"INSERT INTO [AssetMetadataTextArea] ([AssetId], [GroupNumber], [TextAreaValue]) VALUES (@assetId, @groupNumber, @textAreaValue) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
                command.CommandText = "UPDATE [AssetMetadataTextArea] SET [AssetId] = @assetId, [GroupNumber] = @groupNumber, [TextAreaValue] = @textAreaValue WHERE AssetMetadataTextAreaId = @assetMetadataTextAreaId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetMetadataTextArea.AssetId));
			command.Parameters.Add (CreateParameter("@groupNumber", assetMetadataTextArea.GroupNumber));
            command.Parameters.Add (CreateParameter("@textAreaValue", assetMetadataTextArea.TextAreaValue));

			if (assetMetadataTextArea.IsNew) 
			{
				assetMetadataTextArea.AssetMetadataTextAreaId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
                command.Parameters.Add(CreateParameter("@assetMetadataTextAreaId", assetMetadataTextArea.AssetMetadataTextAreaId));
				ExecuteCommand (command);
			}
			
			assetMetadataTextArea.IsDirty = false;
			assetMetadataTextArea.ChangedProperties.Clear();
			
			return assetMetadataTextArea;
		}

		public virtual void Delete (Nullable <Int32> assetMetadataTextAreaId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetMetadataTextArea] WHERE AssetMetadataTextAreaId = @assetMetadataTextAreaId";
			command.Parameters.Add(CreateParameter("@assetMetadataTextAreaId", assetMetadataTextAreaId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetMetadataTextArea object by assetMetadataTextAreaId
		/// </Summary>
		public virtual AssetMetadataTextArea Get (Nullable <Int32> assetMetadataTextAreaId)
		{
			IDbCommand command = GetGetCommand (assetMetadataTextAreaId);
			return (AssetMetadataTextArea) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetMetadataTextArea FindOne (AssetMetadataTextAreaFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetMetadataTextArea.Empty : entity as AssetMetadataTextArea;
		}
		
		public virtual EntityList <AssetMetadataTextArea> FindMany (AssetMetadataTextAreaFinder finder)
		{
			return (EntityList <AssetMetadataTextArea>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetMetadataTextArea> FindMany (AssetMetadataTextAreaFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetMetadataTextArea>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetMetadataTextAreaId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetMetadataTextArea] WHERE AssetMetadataTextAreaId = @assetMetadataTextAreaId";
			command.Parameters.Add(CreateParameter("@assetMetadataTextAreaId", assetMetadataTextAreaId)); 
			
			return command;
		}
	}
}

