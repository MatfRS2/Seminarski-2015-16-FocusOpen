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
	/// This object maps data between the database and a AuditAssetSearch object.
	/// </summary>
	internal partial class AuditAssetSearchMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AuditAssetSearch auditAssetSearch = AuditAssetSearch.New();

			// Table Fields
			auditAssetSearch.AuditAssetSearchId = reader.GetInt32("AuditAssetSearchId"); 
			auditAssetSearch.SessionId = reader.GetString("SessionId");
			auditAssetSearch.IpAddress = reader.GetString("IpAddress");
			auditAssetSearch.UserId = reader.GetInt32("UserId");
			auditAssetSearch.Date = reader.GetDateTime("Date");
			
			// View Fields
			auditAssetSearch.SearchKeyword = reader.GetString("SearchKeyword");
			auditAssetSearch.AssetId = reader.GetInt32("AssetId");
			auditAssetSearch.UserName = reader.GetString("UserName");

			auditAssetSearch.IsDirty = false;
			auditAssetSearch.ChangedProperties.Clear();
			
			return auditAssetSearch;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AuditAssetSearch>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AuditAssetSearch Update (AuditAssetSearch auditAssetSearch)
		{
 			if (!auditAssetSearch.IsDirty || auditAssetSearch.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return auditAssetSearch;
			}
			
			IDbCommand command = CreateCommand();
			
			if (auditAssetSearch.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AuditAssetSearch] ([SessionId], [IpAddress], [UserId], [Date]) VALUES (@sessionId, @ipAddress, @userId, @date) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AuditAssetSearch] SET [SessionId] = @sessionId, [IpAddress] = @ipAddress, [UserId] = @userId, [Date] = @date WHERE AuditAssetSearchId = @auditAssetSearchId"; 
			}
			
			command.Parameters.Add (CreateParameter("@sessionId", auditAssetSearch.SessionId));
			command.Parameters.Add (CreateParameter("@ipAddress", auditAssetSearch.IpAddress));
			command.Parameters.Add (CreateParameter("@userId", auditAssetSearch.UserId));
			command.Parameters.Add (CreateParameter("@date", auditAssetSearch.Date));

			if (auditAssetSearch.IsNew) 
			{
				auditAssetSearch.AuditAssetSearchId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@auditAssetSearchId", auditAssetSearch.AuditAssetSearchId));
				ExecuteCommand (command);
			}
			
			auditAssetSearch.IsDirty = false;
			auditAssetSearch.ChangedProperties.Clear();
			
			return auditAssetSearch;
		}

		public virtual void Delete (Nullable <Int32> auditAssetSearchId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AuditAssetSearch] WHERE AuditAssetSearchId = @auditAssetSearchId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchId", auditAssetSearchId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AuditAssetSearch object by auditAssetSearchId
		/// </Summary>
		public virtual AuditAssetSearch Get (Nullable <Int32> auditAssetSearchId)
		{
			IDbCommand command = GetGetCommand (auditAssetSearchId);
			return (AuditAssetSearch) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AuditAssetSearch FindOne (AuditAssetSearchFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AuditAssetSearch.Empty : entity as AuditAssetSearch;
		}
		
		public virtual EntityList <AuditAssetSearch> FindMany (AuditAssetSearchFinder finder)
		{
			return (EntityList <AuditAssetSearch>) (base.FindMany(finder));
		}

		public virtual EntityList <AuditAssetSearch> FindMany (AuditAssetSearchFinder finder, int Page, int PageSize)
		{
			return (EntityList <AuditAssetSearch>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> auditAssetSearchId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AuditAssetSearch] WHERE AuditAssetSearchId = @auditAssetSearchId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchId", auditAssetSearchId)); 
			
			return command;
		}
	}
}

