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
    /// This object maps data between the database and a LightboxSent object.
    /// </summary>
	internal partial class LightboxSentMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			LightboxSent lightboxSent = LightboxSent.New();

			// Table Fields
			lightboxSent.LightboxSentId = reader.GetInt32("LightboxSentId"); 
			lightboxSent.LightboxId = reader.GetInt32("LightboxId");
			lightboxSent.CreatedLightboxId = reader.GetNullableInt32("CreatedLightboxId");
			lightboxSent.SenderId = reader.GetInt32("SenderId");
			lightboxSent.RecipientEmail = reader.GetString("RecipientEmail");
			lightboxSent.Subject = reader.GetString("Subject");
			lightboxSent.Message = reader.GetString("Message");
			lightboxSent.DateSent = reader.GetDateTime("DateSent");
			lightboxSent.ExpiryDate = reader.GetNullableDateTime("ExpiryDate");
			lightboxSent.DownloadLinks = reader.GetNullableBoolean("DownloadLinks");
			lightboxSent.LightboxLinkedId = reader.GetNullableInt32("LightboxLinkedId");
			

			lightboxSent.IsDirty = false;
			lightboxSent.ChangedProperties.Clear();
			
			return lightboxSent;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <LightboxSent>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual LightboxSent Update (LightboxSent lightboxSent)
		{
 			if (!lightboxSent.IsDirty || lightboxSent.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return lightboxSent;
			}
			
			IDbCommand command = CreateCommand();
			
			if (lightboxSent.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [LightboxSent] ([LightboxId], [CreatedLightboxId], [SenderId], [RecipientEmail], [Subject], [Message], [DateSent], [ExpiryDate], [DownloadLinks], [LightboxLinkedId]) VALUES (@lightboxId, @createdLightboxId, @senderId, @recipientEmail, @subject, @message, @dateSent, @expiryDate, @downloadLinks, @lightboxLinkedId) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [LightboxSent] SET [LightboxId] = @lightboxId, [CreatedLightboxId] = @createdLightboxId, [SenderId] = @senderId, [RecipientEmail] = @recipientEmail, [Subject] = @subject, [Message] = @message, [DateSent] = @dateSent, [ExpiryDate] = @expiryDate, [DownloadLinks] = @downloadLinks, [LightboxLinkedId] = @lightboxLinkedId WHERE LightboxSentId = @lightboxSentId"; 
			}
			
			command.Parameters.Add (CreateParameter("@lightboxId", lightboxSent.LightboxId));
			command.Parameters.Add (CreateParameter("@createdLightboxId", lightboxSent.CreatedLightboxId));
			command.Parameters.Add (CreateParameter("@senderId", lightboxSent.SenderId));
			command.Parameters.Add (CreateParameter("@recipientEmail", lightboxSent.RecipientEmail));
			command.Parameters.Add (CreateParameter("@subject", lightboxSent.Subject));
			command.Parameters.Add (CreateParameter("@message", lightboxSent.Message));
			command.Parameters.Add (CreateParameter("@dateSent", lightboxSent.DateSent));
			command.Parameters.Add (CreateParameter("@expiryDate", lightboxSent.ExpiryDate));
			command.Parameters.Add (CreateParameter("@downloadLinks", lightboxSent.DownloadLinks));
			command.Parameters.Add (CreateParameter("@lightboxLinkedId", lightboxSent.LightboxLinkedId));

			if (lightboxSent.IsNew) 
			{
				lightboxSent.LightboxSentId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@lightboxSentId", lightboxSent.LightboxSentId));
				ExecuteCommand (command);
			}
			
			lightboxSent.IsDirty = false;
			lightboxSent.ChangedProperties.Clear();
			
			return lightboxSent;
		}

		public virtual void Delete (Nullable <Int32> lightboxSentId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [LightboxSent] WHERE LightboxSentId = @lightboxSentId";
			command.Parameters.Add(CreateParameter("@lightboxSentId", lightboxSentId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single LightboxSent object by lightboxSentId
		// </Summary>
		public virtual LightboxSent Get (Nullable <Int32> lightboxSentId)
		{
			IDbCommand command = GetGetCommand (lightboxSentId);
			return (LightboxSent) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual LightboxSent FindOne (LightboxSentFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? LightboxSent.Empty : entity as LightboxSent;
		}
		
		public virtual EntityList <LightboxSent> FindMany (LightboxSentFinder finder)
		{
			return (EntityList <LightboxSent>) (base.FindMany(finder));
		}

		public virtual EntityList <LightboxSent> FindMany (LightboxSentFinder finder, int Page, int PageSize)
		{
			return (EntityList <LightboxSent>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> lightboxSentId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [LightboxSent] WHERE LightboxSentId = @lightboxSentId";
			command.Parameters.Add(CreateParameter("@lightboxSentId", lightboxSentId)); 
			
			return command;
		}
	}
}

