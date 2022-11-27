/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	internal partial class LightboxAssetMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			LightboxAsset lightboxAsset = LightboxAsset.New();

			// Table Fields
			lightboxAsset.LightboxAssetId = reader.GetInt32("LightboxAssetId"); 
			lightboxAsset.LightboxId = reader.GetInt32("LightboxId");
			lightboxAsset.AssetId = reader.GetInt32("AssetId");
			lightboxAsset.Notes = reader.GetString("Notes");
			lightboxAsset.CreateDate = reader.GetDateTime("CreateDate");
			lightboxAsset.OrderNumber = reader.GetNullableInt32("OrderNumber");
			
			// View Fields

			lightboxAsset.IsDirty = false;
			lightboxAsset.ChangedProperties.Clear();
			
			return lightboxAsset;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <LightboxAsset>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual LightboxAsset Update (LightboxAsset lightboxAsset)
		{
 			if (!lightboxAsset.IsDirty || lightboxAsset.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return lightboxAsset;
			}
			
			IDbCommand command = CreateCommand();
			
			if (lightboxAsset.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [LightboxAsset] ([LightboxId], [AssetId], [Notes], [CreateDate], [OrderNumber]) VALUES (@lightboxId, @assetId, @notes, @createDate, @orderNumber) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [LightboxAsset] SET [LightboxId] = @lightboxId, [AssetId] = @assetId, [Notes] = @notes, [CreateDate] = @createDate, [OrderNumber] = @orderNumber WHERE LightboxAssetId = @lightboxAssetId"; 
			}
			
			command.Parameters.Add (CreateParameter("@lightboxId", lightboxAsset.LightboxId));
			command.Parameters.Add (CreateParameter("@assetId", lightboxAsset.AssetId));
			command.Parameters.Add (CreateParameter("@notes", lightboxAsset.Notes));
			command.Parameters.Add (CreateParameter("@createDate", lightboxAsset.CreateDate));
			command.Parameters.Add (CreateParameter("@orderNumber", lightboxAsset.OrderNumber));

			if (lightboxAsset.IsNew) 
			{
				lightboxAsset.LightboxAssetId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@lightboxAssetId", lightboxAsset.LightboxAssetId));
				ExecuteCommand (command);
			}
			
			lightboxAsset.IsDirty = false;
			lightboxAsset.ChangedProperties.Clear();
			
			return lightboxAsset;
		}

		public virtual void Delete (Nullable <Int32> lightboxAssetId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [LightboxAsset] WHERE LightboxAssetId = @lightboxAssetId";
			command.Parameters.Add(CreateParameter("@lightboxAssetId", lightboxAssetId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single LightboxAsset object by lightboxAssetId
		// </Summary>
		public virtual LightboxAsset Get (Nullable <Int32> lightboxAssetId)
		{
			IDbCommand command = GetGetCommand (lightboxAssetId);
			return (LightboxAsset) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual LightboxAsset FindOne (LightboxAssetFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? LightboxAsset.Empty : entity as LightboxAsset;
		}
		
		public virtual EntityList <LightboxAsset> FindMany (LightboxAssetFinder finder)
		{
			return (EntityList <LightboxAsset>) (base.FindMany(finder));
		}

		public virtual EntityList <LightboxAsset> FindMany (LightboxAssetFinder finder, int Page, int PageSize)
		{
			return (EntityList <LightboxAsset>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> lightboxAssetId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_LightboxAsset] WHERE LightboxAssetId = @lightboxAssetId";
			command.Parameters.Add(CreateParameter("@lightboxAssetId", lightboxAssetId)); 
			
			return command;
		}
	}
}

