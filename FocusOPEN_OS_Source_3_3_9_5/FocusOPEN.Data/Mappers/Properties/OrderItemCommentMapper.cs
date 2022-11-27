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
	/// This object maps data between the database and a OrderItemComment object.
	/// </summary>
	internal partial class OrderItemCommentMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			OrderItemComment orderItemComment = OrderItemComment.New();

			// Table Fields
			orderItemComment.OrderItemCommentId = reader.GetInt32("OrderItemCommentId"); 
			orderItemComment.OrderItemId = reader.GetInt32("OrderItemId");
			orderItemComment.UserId = reader.GetInt32("UserId");
			orderItemComment.CommentText = reader.GetString("CommentText");
			orderItemComment.CommentDate = reader.GetDateTime("CommentDate");
			
			// View Fields
			orderItemComment.UserFullName = reader.GetString("UserFullName");

			orderItemComment.IsDirty = false;
			orderItemComment.ChangedProperties.Clear();
			
			return orderItemComment;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <OrderItemComment>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual OrderItemComment Update (OrderItemComment orderItemComment)
		{
 			if (!orderItemComment.IsDirty || orderItemComment.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return orderItemComment;
			}
			
			IDbCommand command = CreateCommand();
			
			if (orderItemComment.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [OrderItemComment] ([OrderItemId], [UserId], [CommentText], [CommentDate]) VALUES (@orderItemId, @userId, @commentText, @commentDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [OrderItemComment] SET [OrderItemId] = @orderItemId, [UserId] = @userId, [CommentText] = @commentText, [CommentDate] = @commentDate WHERE OrderItemCommentId = @orderItemCommentId"; 
			}
			
			command.Parameters.Add (CreateParameter("@orderItemId", orderItemComment.OrderItemId));
			command.Parameters.Add (CreateParameter("@userId", orderItemComment.UserId));
			command.Parameters.Add (CreateParameter("@commentText", orderItemComment.CommentText));
			command.Parameters.Add (CreateParameter("@commentDate", orderItemComment.CommentDate));

			if (orderItemComment.IsNew) 
			{
				orderItemComment.OrderItemCommentId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@orderItemCommentId", orderItemComment.OrderItemCommentId));
				ExecuteCommand (command);
			}
			
			orderItemComment.IsDirty = false;
			orderItemComment.ChangedProperties.Clear();
			
			return orderItemComment;
		}

		public virtual void Delete (Nullable <Int32> orderItemCommentId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [OrderItemComment] WHERE OrderItemCommentId = @orderItemCommentId";
			command.Parameters.Add(CreateParameter("@orderItemCommentId", orderItemCommentId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single OrderItemComment object by orderItemCommentId
		/// </Summary>
		public virtual OrderItemComment Get (Nullable <Int32> orderItemCommentId)
		{
			IDbCommand command = GetGetCommand (orderItemCommentId);
			return (OrderItemComment) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual OrderItemComment FindOne (OrderItemCommentFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? OrderItemComment.Empty : entity as OrderItemComment;
		}
		
		public virtual EntityList <OrderItemComment> FindMany (OrderItemCommentFinder finder)
		{
			return (EntityList <OrderItemComment>) (base.FindMany(finder));
		}

		public virtual EntityList <OrderItemComment> FindMany (OrderItemCommentFinder finder, int Page, int PageSize)
		{
			return (EntityList <OrderItemComment>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> orderItemCommentId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_OrderItemComment] WHERE OrderItemCommentId = @orderItemCommentId";
			command.Parameters.Add(CreateParameter("@orderItemCommentId", orderItemCommentId)); 
			
			return command;
		}
	}
}

