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
	internal partial class OrderItemMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			OrderItem orderItem = OrderItem.New();

			// Table Fields
			orderItem.OrderItemId = reader.GetInt32("OrderItemId"); 
			orderItem.OrderId = reader.GetInt32("OrderId");
			orderItem.AssetId = reader.GetInt32("AssetId");
			orderItem.Notes = reader.GetString("Notes");
			orderItem.RequiredByDate = reader.GetNullableDateTime("RequiredByDate");
			orderItem.OrderItemStatusId = reader.GetInt32("OrderItemStatusId");
			orderItem.OrderItemStatusDate = reader.GetNullableDateTime("OrderItemStatusDate");
			orderItem.OrderItemStatusUserId = reader.GetNullableInt32("OrderItemStatusUserId");
			orderItem.AssignedToUserId = reader.GetNullableInt32("AssignedToUserId");
			orderItem.CreateDate = reader.GetDateTime("CreateDate");
			
			// View Fields
			orderItem.UserId = reader.GetInt32("UserId");
			orderItem.AssetUploadUserId = reader.GetInt32("AssetUploadUserId");
			orderItem.AssetBrandId = reader.GetInt32("AssetBrandId");

			orderItem.IsDirty = false;
			orderItem.ChangedProperties.Clear();
			
			return orderItem;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <OrderItem>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual OrderItem Update (OrderItem orderItem)
		{
 			if (!orderItem.IsDirty || orderItem.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return orderItem;
			}
			
			IDbCommand command = CreateCommand();
			
			if (orderItem.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [OrderItem] ([OrderId], [AssetId], [Notes], [RequiredByDate], [OrderItemStatusId], [OrderItemStatusDate], [OrderItemStatusUserId], [AssignedToUserId], [CreateDate]) VALUES (@orderId, @assetId, @notes, @requiredByDate, @orderItemStatusId, @orderItemStatusDate, @orderItemStatusUserId, @assignedToUserId, @createDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [OrderItem] SET [OrderId] = @orderId, [AssetId] = @assetId, [Notes] = @notes, [RequiredByDate] = @requiredByDate, [OrderItemStatusId] = @orderItemStatusId, [OrderItemStatusDate] = @orderItemStatusDate, [OrderItemStatusUserId] = @orderItemStatusUserId, [AssignedToUserId] = @assignedToUserId, [CreateDate] = @createDate WHERE OrderItemId = @orderItemId"; 
			}
			
			command.Parameters.Add (CreateParameter("@orderId", orderItem.OrderId));
			command.Parameters.Add (CreateParameter("@assetId", orderItem.AssetId));
			command.Parameters.Add (CreateParameter("@notes", orderItem.Notes));
			command.Parameters.Add (CreateParameter("@requiredByDate", orderItem.RequiredByDate));
			command.Parameters.Add (CreateParameter("@orderItemStatusId", orderItem.OrderItemStatusId));
			command.Parameters.Add (CreateParameter("@orderItemStatusDate", orderItem.OrderItemStatusDate));
			command.Parameters.Add (CreateParameter("@orderItemStatusUserId", orderItem.OrderItemStatusUserId));
			command.Parameters.Add (CreateParameter("@assignedToUserId", orderItem.AssignedToUserId));
			command.Parameters.Add (CreateParameter("@createDate", orderItem.CreateDate));

			if (orderItem.IsNew) 
			{
				orderItem.OrderItemId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@orderItemId", orderItem.OrderItemId));
				ExecuteCommand (command);
			}
			
			orderItem.IsDirty = false;
			orderItem.ChangedProperties.Clear();
			
			return orderItem;
		}

		public virtual void Delete (Nullable <Int32> orderItemId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [OrderItem] WHERE OrderItemId = @orderItemId";
			command.Parameters.Add(CreateParameter("@orderItemId", orderItemId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single OrderItem object by orderItemId
		/// </Summary>
		public virtual OrderItem Get (Nullable <Int32> orderItemId)
		{
			IDbCommand command = GetGetCommand (orderItemId);
			return (OrderItem) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual OrderItem FindOne (OrderItemFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? OrderItem.Empty : entity as OrderItem;
		}
		
		public virtual EntityList <OrderItem> FindMany (OrderItemFinder finder)
		{
			return (EntityList <OrderItem>) (base.FindMany(finder));
		}

		public virtual EntityList <OrderItem> FindMany (OrderItemFinder finder, int Page, int PageSize)
		{
			return (EntityList <OrderItem>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> orderItemId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_OrderItem] WHERE OrderItemId = @orderItemId";
			command.Parameters.Add(CreateParameter("@orderItemId", orderItemId)); 
			
			return command;
		}
	}
}

