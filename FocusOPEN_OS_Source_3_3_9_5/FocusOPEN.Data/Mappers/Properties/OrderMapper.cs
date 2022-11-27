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
	internal partial class OrderMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Order order = Order.New();

			// Table Fields
			order.OrderId = reader.GetInt32("OrderId"); 
			order.UserId = reader.GetInt32("UserId");
			order.OrderDate = reader.GetDateTime("OrderDate");
			order.CompletionDate = reader.GetNullableDateTime("CompletionDate");
			
			// View Fields
			order.UserName = reader.GetString("UserName");
			order.UserEmail = reader.GetString("UserEmail");
			order.UserPrimaryBrandId = reader.GetInt32("UserPrimaryBrandId");
			order.UserPrimaryBrandName = reader.GetString("UserPrimaryBrandName");
			order.AssetCount = reader.GetNullableInt32("AssetCount");
			order.IsPending = reader.GetNullableBoolean("IsPending");
			order.DeadlineDate = reader.GetNullableDateTime("DeadlineDate");

			order.IsDirty = false;
			order.ChangedProperties.Clear();
			
			return order;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Order>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Order Update (Order order)
		{
 			if (!order.IsDirty || order.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return order;
			}
			
			IDbCommand command = CreateCommand();
			
			if (order.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Order] ([UserId], [OrderDate], [CompletionDate]) VALUES (@userId, @orderDate, @completionDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Order] SET [UserId] = @userId, [OrderDate] = @orderDate, [CompletionDate] = @completionDate WHERE OrderId = @orderId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", order.UserId));
			command.Parameters.Add (CreateParameter("@orderDate", order.OrderDate));
			command.Parameters.Add (CreateParameter("@completionDate", order.CompletionDate));

			if (order.IsNew) 
			{
				order.OrderId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@orderId", order.OrderId));
				ExecuteCommand (command);
			}
			
			order.IsDirty = false;
			order.ChangedProperties.Clear();
			
			return order;
		}

		public virtual void Delete (Nullable <Int32> orderId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Order] WHERE OrderId = @orderId";
			command.Parameters.Add(CreateParameter("@orderId", orderId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Order object by orderId
		/// </Summary>
		public virtual Order Get (Nullable <Int32> orderId)
		{
			IDbCommand command = GetGetCommand (orderId);
			return (Order) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Order FindOne (OrderFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Order.Empty : entity as Order;
		}
		
		public virtual EntityList <Order> FindMany (OrderFinder finder)
		{
			return (EntityList <Order>) (base.FindMany(finder));
		}

		public virtual EntityList <Order> FindMany (OrderFinder finder, int Page, int PageSize)
		{
			return (EntityList <Order>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> orderId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Order] WHERE OrderId = @orderId";
			command.Parameters.Add(CreateParameter("@orderId", orderId)); 
			
			return command;
		}
	}
}

