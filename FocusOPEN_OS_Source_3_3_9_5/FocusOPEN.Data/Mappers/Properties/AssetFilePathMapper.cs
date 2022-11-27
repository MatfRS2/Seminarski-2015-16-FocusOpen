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
	/// This object maps data between the database and a AssetFilePath object.
	/// </summary>
	internal partial class AssetFilePathMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetFilePath assetFilePath = AssetFilePath.New();

			// Table Fields
			assetFilePath.AssetFilePathId = reader.GetInt32("AssetFilePathId"); 
			assetFilePath.Path = reader.GetString("Path");
			assetFilePath.IsDefault = reader.GetBoolean("IsDefault");
			

			assetFilePath.IsDirty = false;
			assetFilePath.ChangedProperties.Clear();
			
			return assetFilePath;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetFilePath>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetFilePath Update (AssetFilePath assetFilePath)
		{
 			if (!assetFilePath.IsDirty || assetFilePath.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetFilePath;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetFilePath.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetFilePath] ([Path], [IsDefault]) VALUES (@path, @isDefault) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetFilePath] SET [Path] = @path, [IsDefault] = @isDefault WHERE AssetFilePathId = @assetFilePathId"; 
			}
			
			command.Parameters.Add (CreateParameter("@path", assetFilePath.Path));
			command.Parameters.Add (CreateParameter("@isDefault", assetFilePath.IsDefault));

			if (assetFilePath.IsNew) 
			{
				assetFilePath.AssetFilePathId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetFilePathId", assetFilePath.AssetFilePathId));
				ExecuteCommand (command);
			}
			
			assetFilePath.IsDirty = false;
			assetFilePath.ChangedProperties.Clear();
			
			return assetFilePath;
		}

		public virtual void Delete (Nullable <Int32> assetFilePathId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetFilePath] WHERE AssetFilePathId = @assetFilePathId";
			command.Parameters.Add(CreateParameter("@assetFilePathId", assetFilePathId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetFilePath object by assetFilePathId
		/// </Summary>
		public virtual AssetFilePath Get (Nullable <Int32> assetFilePathId)
		{
			IDbCommand command = GetGetCommand (assetFilePathId);
			return (AssetFilePath) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetFilePath FindOne (AssetFilePathFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetFilePath.Empty : entity as AssetFilePath;
		}
		
		public virtual EntityList <AssetFilePath> FindMany (AssetFilePathFinder finder)
		{
			return (EntityList <AssetFilePath>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetFilePath> FindMany (AssetFilePathFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetFilePath>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetFilePathId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetFilePath] WHERE AssetFilePathId = @assetFilePathId";
			command.Parameters.Add(CreateParameter("@assetFilePathId", assetFilePathId)); 
			
			return command;
		}
	}
}

