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
	internal partial class AssetMetadataTextFieldMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetMetadataTextField assetMetadataTextField = AssetMetadataTextField.New();

            // Table Fields
            assetMetadataTextField.AssetMetadataTextFieldId = reader.GetInt32("AssetMetadataTextFieldId");
            assetMetadataTextField.AssetId = reader.GetInt32("AssetId");
            assetMetadataTextField.GroupNumber = reader.GetInt32("GroupNumber");
            assetMetadataTextField.TextFieldValue = reader.GetString("TextFieldValue");
			

			assetMetadataTextField.IsDirty = false;
			assetMetadataTextField.ChangedProperties.Clear();
			
			return assetMetadataTextField;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetMetadataTextField>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetMetadataTextField Update (AssetMetadataTextField assetMetadataTextField)
		{
 			if (!assetMetadataTextField.IsDirty || assetMetadataTextField.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetMetadataTextField;
			}
			
			IDbCommand command = CreateCommand();

            if (assetMetadataTextField.IsNew)
            {
                // Adding
                command.CommandText = @"INSERT INTO [AssetMetadataTextField] ([AssetId], [GroupNumber], [TextFieldValue]) VALUES (@assetId, @groupNumber, @textFieldValue) ; SELECT @@identity AS NewId;";
            }
            else
            {
                // Updating
                command.CommandText = "UPDATE [AssetMetadataTextField] SET [AssetId] = @assetId, [GroupNumber] = @groupNumber, [TextFieldValue] = @textFieldValue WHERE AssetMetadataTextFieldId = @assetMetadataTextFieldId";
            }

            command.Parameters.Add(CreateParameter("@assetId", assetMetadataTextField.AssetId));
            command.Parameters.Add(CreateParameter("@groupNumber", assetMetadataTextField.GroupNumber));
            command.Parameters.Add(CreateParameter("@textFieldValue", assetMetadataTextField.TextFieldValue));

			if (assetMetadataTextField.IsNew) 
			{
				assetMetadataTextField.AssetMetadataTextFieldId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetMetadataTextFieldId", assetMetadataTextField.AssetMetadataTextFieldId));
				ExecuteCommand (command);
			}
			
			assetMetadataTextField.IsDirty = false;
			assetMetadataTextField.ChangedProperties.Clear();
			
			return assetMetadataTextField;
		}

		public virtual void Delete (Nullable <Int32> assetMetadataTextFieldId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetMetadataTextField] WHERE AssetMetadataTextFieldId = @assetMetadataTextFieldId";
			command.Parameters.Add(CreateParameter("@assetMetadataTextFieldId", assetMetadataTextFieldId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetMetadataTextField object by assetMetadataTextFieldId
		/// </Summary>
		public virtual AssetMetadataTextField Get (Nullable <Int32> assetMetadataTextFieldId)
		{
			IDbCommand command = GetGetCommand (assetMetadataTextFieldId);
			return (AssetMetadataTextField) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetMetadataTextField FindOne (AssetMetadataTextFieldFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetMetadataTextField.Empty : entity as AssetMetadataTextField;
		}
		
		public virtual EntityList <AssetMetadataTextField> FindMany (AssetMetadataTextFieldFinder finder)
		{
			return (EntityList <AssetMetadataTextField>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetMetadataTextField> FindMany (AssetMetadataTextFieldFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetMetadataTextField>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetMetadataTextFieldId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetMetadataTextField] WHERE AssetMetadataTextFieldId = @assetMetadataTextFieldId";
			command.Parameters.Add(CreateParameter("@assetMetadataTextFieldId", assetMetadataTextFieldId)); 
			
			return command;
		}
	}
}

