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
	/// This object maps data between the database and a AuditAssetSearchResult object.
	/// </summary>
	internal partial class AuditAssetSearchResultMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AuditAssetSearchResult auditAssetSearchResult = AuditAssetSearchResult.New();

			// Table Fields
			auditAssetSearchResult.AuditAssetSearchResultId = reader.GetInt32("AuditAssetSearchResultId"); 
			auditAssetSearchResult.AuditAssetSearchId = reader.GetInt32("AuditAssetSearchId");
			auditAssetSearchResult.AssetId = reader.GetInt32("AssetId");
			auditAssetSearchResult.Date = reader.GetDateTime("Date");
			

			auditAssetSearchResult.IsDirty = false;
			auditAssetSearchResult.ChangedProperties.Clear();
			
			return auditAssetSearchResult;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AuditAssetSearchResult>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AuditAssetSearchResult Update (AuditAssetSearchResult auditAssetSearchResult)
		{
 			if (!auditAssetSearchResult.IsDirty || auditAssetSearchResult.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return auditAssetSearchResult;
			}
			
			IDbCommand command = CreateCommand();
			
			if (auditAssetSearchResult.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AuditAssetSearchResult] ([AuditAssetSearchId], [AssetId], [Date]) VALUES (@auditAssetSearchId, @assetId, @date) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AuditAssetSearchResult] SET [AuditAssetSearchId] = @auditAssetSearchId, [AssetId] = @assetId, [Date] = @date WHERE AuditAssetSearchResultId = @auditAssetSearchResultId"; 
			}
			
			command.Parameters.Add (CreateParameter("@auditAssetSearchId", auditAssetSearchResult.AuditAssetSearchId));
			command.Parameters.Add (CreateParameter("@assetId", auditAssetSearchResult.AssetId));
			command.Parameters.Add (CreateParameter("@date", auditAssetSearchResult.Date));

			if (auditAssetSearchResult.IsNew) 
			{
				auditAssetSearchResult.AuditAssetSearchResultId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@auditAssetSearchResultId", auditAssetSearchResult.AuditAssetSearchResultId));
				ExecuteCommand (command);
			}
			
			auditAssetSearchResult.IsDirty = false;
			auditAssetSearchResult.ChangedProperties.Clear();
			
			return auditAssetSearchResult;
		}

		public virtual void Delete (Nullable <Int32> auditAssetSearchResultId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AuditAssetSearchResult] WHERE AuditAssetSearchResultId = @auditAssetSearchResultId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchResultId", auditAssetSearchResultId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AuditAssetSearchResult object by auditAssetSearchResultId
		/// </Summary>
		public virtual AuditAssetSearchResult Get (Nullable <Int32> auditAssetSearchResultId)
		{
			IDbCommand command = GetGetCommand (auditAssetSearchResultId);
			return (AuditAssetSearchResult) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AuditAssetSearchResult FindOne (AuditAssetSearchResultFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AuditAssetSearchResult.Empty : entity as AuditAssetSearchResult;
		}
		
		public virtual EntityList <AuditAssetSearchResult> FindMany (AuditAssetSearchResultFinder finder)
		{
			return (EntityList <AuditAssetSearchResult>) (base.FindMany(finder));
		}

		public virtual EntityList <AuditAssetSearchResult> FindMany (AuditAssetSearchResultFinder finder, int Page, int PageSize)
		{
			return (EntityList <AuditAssetSearchResult>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> auditAssetSearchResultId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AuditAssetSearchResult] WHERE AuditAssetSearchResultId = @auditAssetSearchResultId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchResultId", auditAssetSearchResultId)); 
			
			return command;
		}
	}
}

