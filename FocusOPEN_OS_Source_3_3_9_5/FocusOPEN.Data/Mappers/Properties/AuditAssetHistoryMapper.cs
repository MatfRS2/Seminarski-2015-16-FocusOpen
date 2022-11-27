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
	internal partial class AuditAssetHistoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AuditAssetHistory auditAssetHistory = AuditAssetHistory.New();

			// Table Fields
			auditAssetHistory.AuditAssetHistoryId = reader.GetInt32("AuditAssetHistoryId"); 
			auditAssetHistory.SessionId = reader.GetString("SessionId");
			auditAssetHistory.IpAddress = reader.GetString("IpAddress");
			auditAssetHistory.AuditAssetActionId = reader.GetInt32("AuditAssetActionId");
			auditAssetHistory.UserId = reader.GetInt32("UserId");
			auditAssetHistory.AssetId = reader.GetInt32("AssetId");
			auditAssetHistory.Notes = reader.GetString("Notes");
			auditAssetHistory.Date = reader.GetDateTime("Date");
			
			// View Fields
			auditAssetHistory.Description = reader.GetString("Description");
			auditAssetHistory.UserPrimaryBrandId = reader.GetInt32("UserPrimaryBrandId");
			auditAssetHistory.AssetBrandId = reader.GetInt32("AssetBrandId");
			auditAssetHistory.UserName = reader.GetString("UserName");
			auditAssetHistory.UserEmail = reader.GetString("UserEmail");
			auditAssetHistory.Title = reader.GetString("Title");

			auditAssetHistory.IsDirty = false;
			auditAssetHistory.ChangedProperties.Clear();
			
			return auditAssetHistory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AuditAssetHistory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AuditAssetHistory Update (AuditAssetHistory auditAssetHistory)
		{
 			if (!auditAssetHistory.IsDirty || auditAssetHistory.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return auditAssetHistory;
			}
			
			IDbCommand command = CreateCommand();
			
			if (auditAssetHistory.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AuditAssetHistory] ([SessionId], [IpAddress], [AuditAssetActionId], [UserId], [AssetId], [Notes], [Date]) VALUES (@sessionId, @ipAddress, @auditAssetActionId, @userId, @assetId, @notes, @date) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AuditAssetHistory] SET [SessionId] = @sessionId, [IpAddress] = @ipAddress, [AuditAssetActionId] = @auditAssetActionId, [UserId] = @userId, [AssetId] = @assetId, [Notes] = @notes, [Date] = @date WHERE AuditAssetHistoryId = @auditAssetHistoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@sessionId", auditAssetHistory.SessionId));
			command.Parameters.Add (CreateParameter("@ipAddress", auditAssetHistory.IpAddress));
			command.Parameters.Add (CreateParameter("@auditAssetActionId", auditAssetHistory.AuditAssetActionId));
			command.Parameters.Add (CreateParameter("@userId", auditAssetHistory.UserId));
			command.Parameters.Add (CreateParameter("@assetId", auditAssetHistory.AssetId));
			command.Parameters.Add (CreateParameter("@notes", auditAssetHistory.Notes));
			command.Parameters.Add (CreateParameter("@date", auditAssetHistory.Date));

			if (auditAssetHistory.IsNew) 
			{
				auditAssetHistory.AuditAssetHistoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@auditAssetHistoryId", auditAssetHistory.AuditAssetHistoryId));
				ExecuteCommand (command);
			}
			
			auditAssetHistory.IsDirty = false;
			auditAssetHistory.ChangedProperties.Clear();
			
			return auditAssetHistory;
		}

		public virtual void Delete (Nullable <Int32> auditAssetHistoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AuditAssetHistory] WHERE AuditAssetHistoryId = @auditAssetHistoryId";
			command.Parameters.Add(CreateParameter("@auditAssetHistoryId", auditAssetHistoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AuditAssetHistory object by auditAssetHistoryId
		/// </Summary>
		public virtual AuditAssetHistory Get (Nullable <Int32> auditAssetHistoryId)
		{
			IDbCommand command = GetGetCommand (auditAssetHistoryId);
			return (AuditAssetHistory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AuditAssetHistory FindOne (AuditAssetHistoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AuditAssetHistory.Empty : entity as AuditAssetHistory;
		}
		
		public virtual EntityList <AuditAssetHistory> FindMany (AuditAssetHistoryFinder finder)
		{
			return (EntityList <AuditAssetHistory>) (base.FindMany(finder));
		}

		public virtual EntityList <AuditAssetHistory> FindMany (AuditAssetHistoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <AuditAssetHistory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> auditAssetHistoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AuditAssetHistory] WHERE AuditAssetHistoryId = @auditAssetHistoryId";
			command.Parameters.Add(CreateParameter("@auditAssetHistoryId", auditAssetHistoryId)); 
			
			return command;
		}
	}
}

