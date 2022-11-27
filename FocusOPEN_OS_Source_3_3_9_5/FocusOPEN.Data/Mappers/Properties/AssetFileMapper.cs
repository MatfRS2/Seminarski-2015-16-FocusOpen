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
	internal partial class AssetFileMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetFile assetFile = AssetFile.New();

			// Table Fields
			assetFile.AssetFileId = reader.GetInt32("AssetFileId"); 
			assetFile.AssetId = reader.GetInt32("AssetId");
			assetFile.FileContent = reader.GetBytes("FileContent");
			assetFile.Filename = reader.GetString("Filename");
			assetFile.FileExtension = reader.GetString("FileExtension");
			assetFile.AssetFileTypeId = reader.GetInt32("AssetFileTypeId");
			assetFile.LastUpdate = reader.GetDateTime("LastUpdate");
			

			assetFile.IsDirty = false;
			assetFile.ChangedProperties.Clear();
			
			return assetFile;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetFile>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetFile Update (AssetFile assetFile)
		{
 			if (!assetFile.IsDirty || assetFile.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetFile;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetFile.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetFile] ([AssetId], [FileContent], [Filename], [FileExtension], [AssetFileTypeId], [LastUpdate]) VALUES (@assetId, @fileContent, @filename, @fileExtension, @assetFileTypeId, @lastUpdate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetFile] SET [AssetId] = @assetId, [FileContent] = @fileContent, [Filename] = @filename, [FileExtension] = @fileExtension, [AssetFileTypeId] = @assetFileTypeId, [LastUpdate] = @lastUpdate WHERE AssetFileId = @assetFileId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetFile.AssetId));
			command.Parameters.Add (CreateParameter("@fileContent", assetFile.FileContent));
			command.Parameters.Add (CreateParameter("@filename", assetFile.Filename));
			command.Parameters.Add (CreateParameter("@fileExtension", assetFile.FileExtension));
			command.Parameters.Add (CreateParameter("@assetFileTypeId", assetFile.AssetFileTypeId));
			command.Parameters.Add (CreateParameter("@lastUpdate", assetFile.LastUpdate));

			if (assetFile.IsNew) 
			{
				assetFile.AssetFileId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetFileId", assetFile.AssetFileId));
				ExecuteCommand (command);
			}
			
			assetFile.IsDirty = false;
			assetFile.ChangedProperties.Clear();
			
			return assetFile;
		}

		public virtual void Delete (Nullable <Int32> assetFileId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetFile] WHERE AssetFileId = @assetFileId";
			command.Parameters.Add(CreateParameter("@assetFileId", assetFileId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetFile object by assetFileId
		/// </Summary>
		public virtual AssetFile Get (Nullable <Int32> assetFileId)
		{
			IDbCommand command = GetGetCommand (assetFileId);
			return (AssetFile) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetFile FindOne (AssetFileFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetFile.Empty : entity as AssetFile;
		}
		
		public virtual EntityList <AssetFile> FindMany (AssetFileFinder finder)
		{
			return (EntityList <AssetFile>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetFile> FindMany (AssetFileFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetFile>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetFileId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetFile] WHERE AssetFileId = @assetFileId";
			command.Parameters.Add(CreateParameter("@assetFileId", assetFileId)); 
			
			return command;
		}
	}
}

