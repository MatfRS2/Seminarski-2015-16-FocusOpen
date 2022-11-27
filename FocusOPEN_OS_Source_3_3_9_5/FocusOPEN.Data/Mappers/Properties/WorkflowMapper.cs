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
	internal partial class WorkflowMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Workflow workflow = Workflow.New();

			// Table Fields
			workflow.WorkflowId = reader.GetInt32("WorkflowId"); 
			workflow.Name = reader.GetString("Name");
			workflow.BrandId = reader.GetInt32("BrandId");
			workflow.IsDeleted = reader.GetBoolean("IsDeleted");
			
			// View Fields
			workflow.BrandName = reader.GetString("BrandName");

			workflow.IsDirty = false;
			workflow.ChangedProperties.Clear();
			
			return workflow;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Workflow>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Workflow Update (Workflow workflow)
		{
 			if (!workflow.IsDirty || workflow.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return workflow;
			}
			
			IDbCommand command = CreateCommand();
			
			if (workflow.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Workflow] ([Name], [BrandId], [IsDeleted]) VALUES (@name, @brandId, @isDeleted) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Workflow] SET [Name] = @name, [BrandId] = @brandId, [IsDeleted] = @isDeleted WHERE WorkflowId = @workflowId"; 
			}
			
			command.Parameters.Add (CreateParameter("@name", workflow.Name));
			command.Parameters.Add (CreateParameter("@brandId", workflow.BrandId));
			command.Parameters.Add (CreateParameter("@isDeleted", workflow.IsDeleted));

			if (workflow.IsNew) 
			{
				workflow.WorkflowId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@workflowId", workflow.WorkflowId));
				ExecuteCommand (command);
			}
			
			workflow.IsDirty = false;
			workflow.ChangedProperties.Clear();
			
			return workflow;
		}

		public virtual void Delete (Nullable <Int32> workflowId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Workflow] WHERE WorkflowId = @workflowId";
			command.Parameters.Add(CreateParameter("@workflowId", workflowId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Workflow object by workflowId
		/// </Summary>
		public virtual Workflow Get (Nullable <Int32> workflowId)
		{
			IDbCommand command = GetGetCommand (workflowId);
			return (Workflow) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Workflow FindOne (WorkflowFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Workflow.Empty : entity as Workflow;
		}
		
		public virtual EntityList <Workflow> FindMany (WorkflowFinder finder)
		{
			return (EntityList <Workflow>) (base.FindMany(finder));
		}

		public virtual EntityList <Workflow> FindMany (WorkflowFinder finder, int Page, int PageSize)
		{
			return (EntityList <Workflow>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> workflowId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Workflow] WHERE WorkflowId = @workflowId";
			command.Parameters.Add(CreateParameter("@workflowId", workflowId)); 
			
			return command;
		}
	}
}

