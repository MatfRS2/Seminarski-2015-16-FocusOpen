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
	/// This object maps data between the database and a AssetWorkflowCommenter object.
	/// </summary>
	internal partial class AssetWorkflowCommenterMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetWorkflowCommenter assetWorkflowCommenter = AssetWorkflowCommenter.New();

			// Table Fields
			assetWorkflowCommenter.AssetWorkflowCommenterId = reader.GetInt32("AssetWorkflowCommenterId"); 
			assetWorkflowCommenter.UserId = reader.GetInt32("UserId");
			assetWorkflowCommenter.InvitingAssetWorkflowUserId = reader.GetInt32("InvitingAssetWorkflowUserId");
			assetWorkflowCommenter.InvitingUserMessage = reader.GetString("InvitingUserMessage");
			assetWorkflowCommenter.Comments = reader.GetString("Comments");
			assetWorkflowCommenter.CreateDate = reader.GetDateTime("CreateDate");
			assetWorkflowCommenter.LastUpdate = reader.GetDateTime("LastUpdate");
			
			// View Fields
			assetWorkflowCommenter.InvitingUserId = reader.GetInt32("InvitingUserId");
			assetWorkflowCommenter.AssetWorkflowId = reader.GetInt32("AssetWorkflowId");

			assetWorkflowCommenter.IsDirty = false;
			assetWorkflowCommenter.ChangedProperties.Clear();
			
			return assetWorkflowCommenter;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetWorkflowCommenter>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetWorkflowCommenter Update (AssetWorkflowCommenter assetWorkflowCommenter)
		{
 			if (!assetWorkflowCommenter.IsDirty || assetWorkflowCommenter.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetWorkflowCommenter;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetWorkflowCommenter.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetWorkflowCommenter] ([UserId], [InvitingAssetWorkflowUserId], [InvitingUserMessage], [Comments], [CreateDate], [LastUpdate]) VALUES (@userId, @invitingAssetWorkflowUserId, @invitingUserMessage, @comments, @createDate, @lastUpdate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetWorkflowCommenter] SET [UserId] = @userId, [InvitingAssetWorkflowUserId] = @invitingAssetWorkflowUserId, [InvitingUserMessage] = @invitingUserMessage, [Comments] = @comments, [CreateDate] = @createDate, [LastUpdate] = @lastUpdate WHERE AssetWorkflowCommenterId = @assetWorkflowCommenterId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", assetWorkflowCommenter.UserId));
			command.Parameters.Add (CreateParameter("@invitingAssetWorkflowUserId", assetWorkflowCommenter.InvitingAssetWorkflowUserId));
			command.Parameters.Add (CreateParameter("@invitingUserMessage", assetWorkflowCommenter.InvitingUserMessage));
			command.Parameters.Add (CreateParameter("@comments", assetWorkflowCommenter.Comments));
			command.Parameters.Add (CreateParameter("@createDate", assetWorkflowCommenter.CreateDate));
			command.Parameters.Add (CreateParameter("@lastUpdate", assetWorkflowCommenter.LastUpdate));

			if (assetWorkflowCommenter.IsNew) 
			{
				assetWorkflowCommenter.AssetWorkflowCommenterId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetWorkflowCommenterId", assetWorkflowCommenter.AssetWorkflowCommenterId));
				ExecuteCommand (command);
			}
			
			assetWorkflowCommenter.IsDirty = false;
			assetWorkflowCommenter.ChangedProperties.Clear();
			
			return assetWorkflowCommenter;
		}

		public virtual void Delete (Nullable <Int32> assetWorkflowCommenterId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetWorkflowCommenter] WHERE AssetWorkflowCommenterId = @assetWorkflowCommenterId";
			command.Parameters.Add(CreateParameter("@assetWorkflowCommenterId", assetWorkflowCommenterId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetWorkflowCommenter object by assetWorkflowCommenterId
		/// </Summary>
		public virtual AssetWorkflowCommenter Get (Nullable <Int32> assetWorkflowCommenterId)
		{
			IDbCommand command = GetGetCommand (assetWorkflowCommenterId);
			return (AssetWorkflowCommenter) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetWorkflowCommenter FindOne (AssetWorkflowCommenterFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetWorkflowCommenter.Empty : entity as AssetWorkflowCommenter;
		}
		
		public virtual EntityList <AssetWorkflowCommenter> FindMany (AssetWorkflowCommenterFinder finder)
		{
			return (EntityList <AssetWorkflowCommenter>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetWorkflowCommenter> FindMany (AssetWorkflowCommenterFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetWorkflowCommenter>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetWorkflowCommenterId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_AssetWorkflowCommenter] WHERE AssetWorkflowCommenterId = @assetWorkflowCommenterId";
			command.Parameters.Add(CreateParameter("@assetWorkflowCommenterId", assetWorkflowCommenterId)); 
			
			return command;
		}
	}
}

