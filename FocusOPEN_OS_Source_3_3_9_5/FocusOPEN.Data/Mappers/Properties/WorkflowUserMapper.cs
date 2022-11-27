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
	internal partial class WorkflowUserMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			WorkflowUser workflowUser = WorkflowUser.New();

			// Table Fields
			workflowUser.WorkflowUserId = reader.GetInt32("WorkflowUserId"); 
			workflowUser.WorkflowId = reader.GetInt32("WorkflowId");
			workflowUser.UserId = reader.GetInt32("UserId");
			workflowUser.Position = reader.GetInt32("Position");
			workflowUser.DateAdded = reader.GetDateTime("DateAdded");
			
			// View Fields
			workflowUser.UserFullName = reader.GetString("UserFullName");

			workflowUser.IsDirty = false;
			workflowUser.ChangedProperties.Clear();
			
			return workflowUser;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <WorkflowUser>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual WorkflowUser Update (WorkflowUser workflowUser)
		{
 			if (!workflowUser.IsDirty || workflowUser.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return workflowUser;
			}
			
			IDbCommand command = CreateCommand();
			
			if (workflowUser.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [WorkflowUser] ([WorkflowId], [UserId], [Position], [DateAdded]) VALUES (@workflowId, @userId, @position, @dateAdded) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [WorkflowUser] SET [WorkflowId] = @workflowId, [UserId] = @userId, [Position] = @position, [DateAdded] = @dateAdded WHERE WorkflowUserId = @workflowUserId"; 
			}
			
			command.Parameters.Add (CreateParameter("@workflowId", workflowUser.WorkflowId));
			command.Parameters.Add (CreateParameter("@userId", workflowUser.UserId));
			command.Parameters.Add (CreateParameter("@position", workflowUser.Position));
			command.Parameters.Add (CreateParameter("@dateAdded", workflowUser.DateAdded));

			if (workflowUser.IsNew) 
			{
				workflowUser.WorkflowUserId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@workflowUserId", workflowUser.WorkflowUserId));
				ExecuteCommand (command);
			}
			
			workflowUser.IsDirty = false;
			workflowUser.ChangedProperties.Clear();
			
			return workflowUser;
		}

		public virtual void Delete (Nullable <Int32> workflowUserId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [WorkflowUser] WHERE WorkflowUserId = @workflowUserId";
			command.Parameters.Add(CreateParameter("@workflowUserId", workflowUserId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single WorkflowUser object by workflowUserId
		/// </Summary>
		public virtual WorkflowUser Get (Nullable <Int32> workflowUserId)
		{
			IDbCommand command = GetGetCommand (workflowUserId);
			return (WorkflowUser) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual WorkflowUser FindOne (WorkflowUserFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? WorkflowUser.Empty : entity as WorkflowUser;
		}
		
		public virtual EntityList <WorkflowUser> FindMany (WorkflowUserFinder finder)
		{
			return (EntityList <WorkflowUser>) (base.FindMany(finder));
		}

		public virtual EntityList <WorkflowUser> FindMany (WorkflowUserFinder finder, int Page, int PageSize)
		{
			return (EntityList <WorkflowUser>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> workflowUserId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_WorkflowUser] WHERE WorkflowUserId = @workflowUserId";
			command.Parameters.Add(CreateParameter("@workflowUserId", workflowUserId)); 
			
			return command;
		}
	}
}

