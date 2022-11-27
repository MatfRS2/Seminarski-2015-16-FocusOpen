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
	/// This object maps data between the database and a AssetWorkflowUser object.
	/// </summary>
	internal partial class AssetWorkflowUserMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetWorkflowUser assetWorkflowUser = AssetWorkflowUser.New();

			// Table Fields
			assetWorkflowUser.AssetWorkflowUserId = reader.GetInt32("AssetWorkflowUserId"); 
			assetWorkflowUser.AssetWorkflowId = reader.GetInt32("AssetWorkflowId");
			assetWorkflowUser.UserId = reader.GetInt32("UserId");
			assetWorkflowUser.Position = reader.GetInt32("Position");
			assetWorkflowUser.Comments = reader.GetString("Comments");
			assetWorkflowUser.AssetWorkflowUserStatusId = reader.GetInt32("AssetWorkflowUserStatusId");
			assetWorkflowUser.CreateDate = reader.GetDateTime("CreateDate");
			assetWorkflowUser.LastUpdate = reader.GetDateTime("LastUpdate");
			assetWorkflowUser.IsDeleted = reader.GetBoolean("IsDeleted");
			

			assetWorkflowUser.IsDirty = false;
			assetWorkflowUser.ChangedProperties.Clear();
			
			return assetWorkflowUser;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetWorkflowUser>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetWorkflowUser Update (AssetWorkflowUser assetWorkflowUser)
		{
 			if (!assetWorkflowUser.IsDirty || assetWorkflowUser.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetWorkflowUser;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetWorkflowUser.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetWorkflowUser] ([AssetWorkflowId], [UserId], [Position], [Comments], [AssetWorkflowUserStatusId], [CreateDate], [LastUpdate], [IsDeleted]) VALUES (@assetWorkflowId, @userId, @position, @comments, @assetWorkflowUserStatusId, @createDate, @lastUpdate, @isDeleted) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetWorkflowUser] SET [AssetWorkflowId] = @assetWorkflowId, [UserId] = @userId, [Position] = @position, [Comments] = @comments, [AssetWorkflowUserStatusId] = @assetWorkflowUserStatusId, [CreateDate] = @createDate, [LastUpdate] = @lastUpdate, [IsDeleted] = @isDeleted WHERE AssetWorkflowUserId = @assetWorkflowUserId"; 
			}
			
			command.Parameters.Add (CreateParameter("@assetWorkflowId", assetWorkflowUser.AssetWorkflowId));
			command.Parameters.Add (CreateParameter("@userId", assetWorkflowUser.UserId));
			command.Parameters.Add (CreateParameter("@position", assetWorkflowUser.Position));
			command.Parameters.Add (CreateParameter("@comments", assetWorkflowUser.Comments));
			command.Parameters.Add (CreateParameter("@assetWorkflowUserStatusId", assetWorkflowUser.AssetWorkflowUserStatusId));
			command.Parameters.Add (CreateParameter("@createDate", assetWorkflowUser.CreateDate));
			command.Parameters.Add (CreateParameter("@lastUpdate", assetWorkflowUser.LastUpdate));
			command.Parameters.Add (CreateParameter("@isDeleted", assetWorkflowUser.IsDeleted));

			if (assetWorkflowUser.IsNew) 
			{
				assetWorkflowUser.AssetWorkflowUserId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetWorkflowUserId", assetWorkflowUser.AssetWorkflowUserId));
				ExecuteCommand (command);
			}
			
			assetWorkflowUser.IsDirty = false;
			assetWorkflowUser.ChangedProperties.Clear();
			
			return assetWorkflowUser;
		}

		public virtual void Delete (Nullable <Int32> assetWorkflowUserId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetWorkflowUser] WHERE AssetWorkflowUserId = @assetWorkflowUserId";
			command.Parameters.Add(CreateParameter("@assetWorkflowUserId", assetWorkflowUserId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetWorkflowUser object by assetWorkflowUserId
		/// </Summary>
		public virtual AssetWorkflowUser Get (Nullable <Int32> assetWorkflowUserId)
		{
			IDbCommand command = GetGetCommand (assetWorkflowUserId);
			return (AssetWorkflowUser) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetWorkflowUser FindOne (AssetWorkflowUserFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetWorkflowUser.Empty : entity as AssetWorkflowUser;
		}
		
		public virtual EntityList <AssetWorkflowUser> FindMany (AssetWorkflowUserFinder finder)
		{
			return (EntityList <AssetWorkflowUser>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetWorkflowUser> FindMany (AssetWorkflowUserFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetWorkflowUser>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetWorkflowUserId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetWorkflowUser] WHERE AssetWorkflowUserId = @assetWorkflowUserId";
			command.Parameters.Add(CreateParameter("@assetWorkflowUserId", assetWorkflowUserId)); 
			
			return command;
		}
	}
}

