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
	internal partial class AssetLinkMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetLink assetLink = AssetLink.New();

			// Table Fields
			assetLink.AssetLinkId = reader.GetInt32("AssetLinkId"); 
			assetLink.AssetId = reader.GetInt32("AssetId");
			assetLink.LinkedAssetId = reader.GetInt32("LinkedAssetId");
			
			// View Fields
			assetLink.LinkedAssetTitle = reader.GetString("LinkedAssetTitle");

			assetLink.IsDirty = false;
			assetLink.ChangedProperties.Clear();
			
			return assetLink;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetLink>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetLink Update (AssetLink assetLink)
		{
 			if (!assetLink.IsDirty || assetLink.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetLink;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetLink.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetLink] ([AssetId], [LinkedAssetId]) VALUES (@assetId, @linkedAssetId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetLink] SET [AssetId] = @assetId, [LinkedAssetId] = @linkedAssetId WHERE AssetLinkId = @assetLinkId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetLink.AssetId));
			command.Parameters.Add (CreateParameter("@linkedAssetId", assetLink.LinkedAssetId));

			if (assetLink.IsNew) 
			{
				assetLink.AssetLinkId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetLinkId", assetLink.AssetLinkId));
				ExecuteCommand (command);
			}
			
			assetLink.IsDirty = false;
			assetLink.ChangedProperties.Clear();
			
			return assetLink;
		}

		public virtual void Delete (Nullable <Int32> assetLinkId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetLink] WHERE AssetLinkId = @assetLinkId";
			command.Parameters.Add(CreateParameter("@assetLinkId", assetLinkId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetLink object by assetLinkId
		/// </Summary>
		public virtual AssetLink Get (Nullable <Int32> assetLinkId)
		{
			IDbCommand command = GetGetCommand (assetLinkId);
			return (AssetLink) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetLink FindOne (AssetLinkFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetLink.Empty : entity as AssetLink;
		}
		
		public virtual EntityList <AssetLink> FindMany (AssetLinkFinder finder)
		{
			return (EntityList <AssetLink>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetLink> FindMany (AssetLinkFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetLink>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetLinkId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetLink] WHERE AssetLinkId = @assetLinkId";
			command.Parameters.Add(CreateParameter("@assetLinkId", assetLinkId)); 
			
			return command;
		}
	}
}

