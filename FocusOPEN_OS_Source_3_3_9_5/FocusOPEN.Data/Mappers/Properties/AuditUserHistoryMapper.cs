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
	/// This object maps data between the database and a AuditUserHistory object.
	/// </summary>
	internal partial class AuditUserHistoryMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AuditUserHistory auditUserHistory = AuditUserHistory.New();

			// Table Fields
			auditUserHistory.AuditUserHistoryId = reader.GetInt32("AuditUserHistoryId"); 
			auditUserHistory.SessionId = reader.GetString("SessionId");
			auditUserHistory.IpAddress = reader.GetString("IpAddress");
			auditUserHistory.AuditUserActionId = reader.GetInt32("AuditUserActionId");
			auditUserHistory.UserId = reader.GetInt32("UserId");
			auditUserHistory.Notes = reader.GetString("Notes");
			auditUserHistory.Date = reader.GetDateTime("Date");
			
			// View Fields
			auditUserHistory.Description = reader.GetString("Description");
			auditUserHistory.UserName = reader.GetString("UserName");
			auditUserHistory.UserEmail = reader.GetString("UserEmail");
			auditUserHistory.PrimaryBrandId = reader.GetInt32("PrimaryBrandId");

			auditUserHistory.IsDirty = false;
			auditUserHistory.ChangedProperties.Clear();
			
			return auditUserHistory;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AuditUserHistory>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AuditUserHistory Update (AuditUserHistory auditUserHistory)
		{
 			if (!auditUserHistory.IsDirty || auditUserHistory.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return auditUserHistory;
			}
			
			IDbCommand command = CreateCommand();
			
			if (auditUserHistory.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AuditUserHistory] ([SessionId], [IpAddress], [AuditUserActionId], [UserId], [Notes], [Date]) VALUES (@sessionId, @ipAddress, @auditUserActionId, @userId, @notes, @date) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AuditUserHistory] SET [SessionId] = @sessionId, [IpAddress] = @ipAddress, [AuditUserActionId] = @auditUserActionId, [UserId] = @userId, [Notes] = @notes, [Date] = @date WHERE AuditUserHistoryId = @auditUserHistoryId"; 
			}
			
			command.Parameters.Add (CreateParameter("@sessionId", auditUserHistory.SessionId));
			command.Parameters.Add (CreateParameter("@ipAddress", auditUserHistory.IpAddress));
			command.Parameters.Add (CreateParameter("@auditUserActionId", auditUserHistory.AuditUserActionId));
			command.Parameters.Add (CreateParameter("@userId", auditUserHistory.UserId));
			command.Parameters.Add (CreateParameter("@notes", auditUserHistory.Notes));
			command.Parameters.Add (CreateParameter("@date", auditUserHistory.Date));

			if (auditUserHistory.IsNew) 
			{
				auditUserHistory.AuditUserHistoryId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@auditUserHistoryId", auditUserHistory.AuditUserHistoryId));
				ExecuteCommand (command);
			}
			
			auditUserHistory.IsDirty = false;
			auditUserHistory.ChangedProperties.Clear();
			
			return auditUserHistory;
		}

		public virtual void Delete (Nullable <Int32> auditUserHistoryId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AuditUserHistory] WHERE AuditUserHistoryId = @auditUserHistoryId";
			command.Parameters.Add(CreateParameter("@auditUserHistoryId", auditUserHistoryId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AuditUserHistory object by auditUserHistoryId
		/// </Summary>
		public virtual AuditUserHistory Get (Nullable <Int32> auditUserHistoryId)
		{
			IDbCommand command = GetGetCommand (auditUserHistoryId);
			return (AuditUserHistory) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AuditUserHistory FindOne (AuditUserHistoryFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AuditUserHistory.Empty : entity as AuditUserHistory;
		}
		
		public virtual EntityList <AuditUserHistory> FindMany (AuditUserHistoryFinder finder)
		{
			return (EntityList <AuditUserHistory>) (base.FindMany(finder));
		}

		public virtual EntityList <AuditUserHistory> FindMany (AuditUserHistoryFinder finder, int Page, int PageSize)
		{
			return (EntityList <AuditUserHistory>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> auditUserHistoryId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AuditUserHistory] WHERE AuditUserHistoryId = @auditUserHistoryId";
			command.Parameters.Add(CreateParameter("@auditUserHistoryId", auditUserHistoryId)); 
			
			return command;
		}
	}
}

