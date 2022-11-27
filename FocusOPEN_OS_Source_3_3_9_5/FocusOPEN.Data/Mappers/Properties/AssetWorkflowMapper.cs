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
	/// This object maps data between the database and a AssetWorkflow object.
	/// </summary>
	internal partial class AssetWorkflowMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetWorkflow assetWorkflow = AssetWorkflow.New();

			// Table Fields
			assetWorkflow.AssetWorkflowId = reader.GetInt32("AssetWorkflowId"); 
			assetWorkflow.AssetId = reader.GetInt32("AssetId");
			assetWorkflow.SubmittedByUserId = reader.GetInt32("SubmittedByUserId");
			assetWorkflow.CreateDate = reader.GetDateTime("CreateDate");
			assetWorkflow.IsComplete = reader.GetBoolean("IsComplete");
			

			assetWorkflow.IsDirty = false;
			assetWorkflow.ChangedProperties.Clear();
			
			return assetWorkflow;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetWorkflow>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetWorkflow Update (AssetWorkflow assetWorkflow)
		{
 			if (!assetWorkflow.IsDirty || assetWorkflow.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetWorkflow;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetWorkflow.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetWorkflow] ([AssetId], [SubmittedByUserId], [CreateDate], [IsComplete]) VALUES (@assetId, @submittedByUserId, @createDate, @isComplete) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetWorkflow] SET [AssetId] = @assetId, [SubmittedByUserId] = @submittedByUserId, [CreateDate] = @createDate, [IsComplete] = @isComplete WHERE AssetWorkflowId = @assetWorkflowId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetId", assetWorkflow.AssetId));
			command.Parameters.Add (CreateParameter("@submittedByUserId", assetWorkflow.SubmittedByUserId));
			command.Parameters.Add (CreateParameter("@createDate", assetWorkflow.CreateDate));
			command.Parameters.Add (CreateParameter("@isComplete", assetWorkflow.IsComplete));

			if (assetWorkflow.IsNew) 
			{
				assetWorkflow.AssetWorkflowId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetWorkflowId", assetWorkflow.AssetWorkflowId));
				ExecuteCommand (command);
			}
			
			assetWorkflow.IsDirty = false;
			assetWorkflow.ChangedProperties.Clear();
			
			return assetWorkflow;
		}

		public virtual void Delete (Nullable <Int32> assetWorkflowId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetWorkflow] WHERE AssetWorkflowId = @assetWorkflowId";
			command.Parameters.Add(CreateParameter("@assetWorkflowId", assetWorkflowId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetWorkflow object by assetWorkflowId
		/// </Summary>
		public virtual AssetWorkflow Get (Nullable <Int32> assetWorkflowId)
		{
			IDbCommand command = GetGetCommand (assetWorkflowId);
			return (AssetWorkflow) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetWorkflow FindOne (AssetWorkflowFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetWorkflow.Empty : entity as AssetWorkflow;
		}
		
		public virtual EntityList <AssetWorkflow> FindMany (AssetWorkflowFinder finder)
		{
			return (EntityList <AssetWorkflow>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetWorkflow> FindMany (AssetWorkflowFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetWorkflow>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetWorkflowId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetWorkflow] WHERE AssetWorkflowId = @assetWorkflowId";
			command.Parameters.Add(CreateParameter("@assetWorkflowId", assetWorkflowId)); 
			
			return command;
		}
	}
}

