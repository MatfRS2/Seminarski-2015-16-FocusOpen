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
	internal partial class AssetOrderHistoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetOrderHistory assetOrderHistory = AssetOrderHistory.New();

			// View Fields
            assetOrderHistory.AssetOrderHistoryId = reader.GetInt32("AssetOrderHistoryId");
            assetOrderHistory.AssetId = reader.GetInt32("AssetId");
            assetOrderHistory.OrderId = reader.GetInt32("OrderId");
            assetOrderHistory.OrderDate = reader.GetDateTime("OrderDate");
            assetOrderHistory.DeadlineDate = reader.GetDateTime("DeadlineDate");
            assetOrderHistory.UserEmail = reader.GetString("UserEmail");
            assetOrderHistory.UserId = reader.GetInt32("UserId");
            assetOrderHistory.UserName = reader.GetString("UserName");
            assetOrderHistory.Notes = reader.GetString("Notes");
            assetOrderHistory.OrderItemStatusDate = reader.GetDateTime("OrderItemStatusDate");
            assetOrderHistory.OrderItemStatusId = reader.GetInt32("OrderItemStatusId");


			assetOrderHistory.IsDirty = false;
			assetOrderHistory.ChangedProperties.Clear();
			
			return assetOrderHistory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetOrderHistory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetOrderHistory Update (AssetOrderHistory assetOrderHistory)
		{
            throw new NotImplementedException();
		}

		public virtual void Delete (Nullable <Int32> assetOrderHistoryId)
		{
            throw new NotImplementedException();
        }
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetOrderHistory object by assetOrderHistoryId
		/// </Summary>
		public virtual AssetOrderHistory Get (Nullable <Int32> assetOrderHistoryId)
		{
			IDbCommand command = GetGetCommand (assetOrderHistoryId);
			return (AssetOrderHistory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetOrderHistory FindOne (AssetOrderHistoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetOrderHistory.Empty : entity as AssetOrderHistory;
		}
		
		public virtual EntityList <AssetOrderHistory> FindMany (AssetOrderHistoryFinder finder)
		{
			return (EntityList <AssetOrderHistory>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetOrderHistory> FindMany (AssetOrderHistoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetOrderHistory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetOrderHistoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetOrderHistory] WHERE AssetOrderHistoryId = @assetOrderHistoryId";
			command.Parameters.Add(CreateParameter("@assetOrderHistoryId", assetOrderHistoryId)); 
			
			return command;
		}
	}
}

