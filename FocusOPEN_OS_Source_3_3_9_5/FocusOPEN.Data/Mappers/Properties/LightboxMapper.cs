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
	/// This object maps data between the database and a Lightbox object.
	/// </summary>
	internal partial class LightboxMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Lightbox lightbox = Lightbox.New();

			// Table Fields
			lightbox.LightboxId = reader.GetInt32("LightboxId"); 
			lightbox.UserId = reader.GetInt32("UserId");
			lightbox.Name = reader.GetString("Name");
			lightbox.Summary = reader.GetString("Summary");
			lightbox.Notes = reader.GetString("Notes");
			lightbox.IsPublic = reader.GetBoolean("IsPublic");
			lightbox.IsDefault = reader.GetBoolean("IsDefault");
			lightbox.CreateDate = reader.GetDateTime("CreateDate");
			

			lightbox.IsDirty = false;
			lightbox.ChangedProperties.Clear();
			
			return lightbox;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Lightbox>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Lightbox Update (Lightbox lightbox)
		{
 			if (!lightbox.IsDirty || lightbox.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return lightbox;
			}
			
			IDbCommand command = CreateCommand();
			
			if (lightbox.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Lightbox] ([UserId], [Name], [Summary], [Notes], [IsPublic], [IsDefault], [CreateDate]) VALUES (@userId, @name, @summary, @notes, @isPublic, @isDefault, @createDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Lightbox] SET [UserId] = @userId, [Name] = @name, [Summary] = @summary, [Notes] = @notes, [IsPublic] = @isPublic, [IsDefault] = @isDefault, [CreateDate] = @createDate WHERE LightboxId = @lightboxId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", lightbox.UserId));
			command.Parameters.Add (CreateParameter("@name", lightbox.Name));
			command.Parameters.Add (CreateParameter("@summary", lightbox.Summary));
			command.Parameters.Add (CreateParameter("@notes", lightbox.Notes));
			command.Parameters.Add (CreateParameter("@isPublic", lightbox.IsPublic));
			command.Parameters.Add (CreateParameter("@isDefault", lightbox.IsDefault));
			command.Parameters.Add (CreateParameter("@createDate", lightbox.CreateDate));

			if (lightbox.IsNew) 
			{
				lightbox.LightboxId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@lightboxId", lightbox.LightboxId));
				ExecuteCommand (command);
			}
			
			lightbox.IsDirty = false;
			lightbox.ChangedProperties.Clear();
			
			return lightbox;
		}

		public virtual void Delete (Nullable <Int32> lightboxId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Lightbox] WHERE LightboxId = @lightboxId";
			command.Parameters.Add(CreateParameter("@lightboxId", lightboxId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Lightbox object by lightboxId
		/// </Summary>
		public virtual Lightbox Get (Nullable <Int32> lightboxId)
		{
			IDbCommand command = GetGetCommand (lightboxId);
			return (Lightbox) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Lightbox FindOne (LightboxFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Lightbox.Empty : entity as Lightbox;
		}
		
		public virtual EntityList <Lightbox> FindMany (LightboxFinder finder)
		{
			return (EntityList <Lightbox>) (base.FindMany(finder));
		}

		public virtual EntityList <Lightbox> FindMany (LightboxFinder finder, int Page, int PageSize)
		{
			return (EntityList <Lightbox>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> lightboxId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [Lightbox] WHERE LightboxId = @lightboxId";
			command.Parameters.Add(CreateParameter("@lightboxId", lightboxId)); 
			
			return command;
		}
	}
}

