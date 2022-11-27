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
	/// This object maps data between the database and a AuditAssetSearchKeyword object.
	/// </summary>
	internal partial class AuditAssetSearchKeywordMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AuditAssetSearchKeyword auditAssetSearchKeyword = AuditAssetSearchKeyword.New();

			// Table Fields
			auditAssetSearchKeyword.AuditAssetSearchKeywordId = reader.GetInt32("AuditAssetSearchKeywordId"); 
			auditAssetSearchKeyword.AuditAssetSearchId = reader.GetInt32("AuditAssetSearchId");
			auditAssetSearchKeyword.SearchKeyword = reader.GetString("SearchKeyword");
			

			auditAssetSearchKeyword.IsDirty = false;
			auditAssetSearchKeyword.ChangedProperties.Clear();
			
			return auditAssetSearchKeyword;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AuditAssetSearchKeyword>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AuditAssetSearchKeyword Update (AuditAssetSearchKeyword auditAssetSearchKeyword)
		{
 			if (!auditAssetSearchKeyword.IsDirty || auditAssetSearchKeyword.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return auditAssetSearchKeyword;
			}
			
			IDbCommand command = CreateCommand();
			
			if (auditAssetSearchKeyword.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AuditAssetSearchKeyword] ([AuditAssetSearchId], [SearchKeyword]) VALUES (@auditAssetSearchId, @searchKeyword) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AuditAssetSearchKeyword] SET [AuditAssetSearchId] = @auditAssetSearchId, [SearchKeyword] = @searchKeyword WHERE AuditAssetSearchKeywordId = @auditAssetSearchKeywordId"; 
			}
			
			command.Parameters.Add (CreateParameter("@auditAssetSearchId", auditAssetSearchKeyword.AuditAssetSearchId));
			command.Parameters.Add (CreateParameter("@searchKeyword", auditAssetSearchKeyword.SearchKeyword));

			if (auditAssetSearchKeyword.IsNew) 
			{
				auditAssetSearchKeyword.AuditAssetSearchKeywordId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@auditAssetSearchKeywordId", auditAssetSearchKeyword.AuditAssetSearchKeywordId));
				ExecuteCommand (command);
			}
			
			auditAssetSearchKeyword.IsDirty = false;
			auditAssetSearchKeyword.ChangedProperties.Clear();
			
			return auditAssetSearchKeyword;
		}

		public virtual void Delete (Nullable <Int32> auditAssetSearchKeywordId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AuditAssetSearchKeyword] WHERE AuditAssetSearchKeywordId = @auditAssetSearchKeywordId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchKeywordId", auditAssetSearchKeywordId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AuditAssetSearchKeyword object by auditAssetSearchKeywordId
		/// </Summary>
		public virtual AuditAssetSearchKeyword Get (Nullable <Int32> auditAssetSearchKeywordId)
		{
			IDbCommand command = GetGetCommand (auditAssetSearchKeywordId);
			return (AuditAssetSearchKeyword) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AuditAssetSearchKeyword FindOne (AuditAssetSearchKeywordFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AuditAssetSearchKeyword.Empty : entity as AuditAssetSearchKeyword;
		}
		
		public virtual EntityList <AuditAssetSearchKeyword> FindMany (AuditAssetSearchKeywordFinder finder)
		{
			return (EntityList <AuditAssetSearchKeyword>) (base.FindMany(finder));
		}

		public virtual EntityList <AuditAssetSearchKeyword> FindMany (AuditAssetSearchKeywordFinder finder, int Page, int PageSize)
		{
			return (EntityList <AuditAssetSearchKeyword>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> auditAssetSearchKeywordId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AuditAssetSearchKeyword] WHERE AuditAssetSearchKeywordId = @auditAssetSearchKeywordId";
			command.Parameters.Add(CreateParameter("@auditAssetSearchKeywordId", auditAssetSearchKeywordId)); 
			
			return command;
		}
	}
}

