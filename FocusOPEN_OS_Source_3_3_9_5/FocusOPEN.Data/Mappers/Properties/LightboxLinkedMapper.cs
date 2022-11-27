/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2010, All Rights Reserved

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
	internal partial class LightboxLinkedMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			LightboxLinked lightboxLinked = LightboxLinked.New();

			// Table Fields
			lightboxLinked.LightboxLinkedId = reader.GetInt32("LightboxLinkedId"); 
			lightboxLinked.LightboxId = reader.GetInt32("LightboxId");
			lightboxLinked.UserId = reader.GetInt32("UserId");
			lightboxLinked.IsEditable = reader.GetNullableBoolean("IsEditable");
			lightboxLinked.ExpiryDate = reader.GetNullableDateTime("ExpiryDate");
			lightboxLinked.Disabled = reader.GetNullableBoolean("Disabled");
			

			lightboxLinked.IsDirty = false;
			lightboxLinked.ChangedProperties.Clear();
			
			return lightboxLinked;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <LightboxLinked>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual LightboxLinked Update (LightboxLinked lightboxLinked)
		{
 			if (!lightboxLinked.IsDirty || lightboxLinked.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return lightboxLinked;
			}
			
			IDbCommand command = CreateCommand();
			
			if (lightboxLinked.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [LightboxLinked] ([LightboxId], [UserId], [IsEditable], [ExpiryDate], [Disabled]) VALUES (@lightboxId, @userId, @isEditable, @expiryDate, @disabled) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [LightboxLinked] SET [LightboxId] = @lightboxId, [UserId] = @userId, [IsEditable] = @isEditable, [ExpiryDate] = @expiryDate, [Disabled] = @disabled WHERE LightboxLinkedId = @lightboxLinkedId"; 
			}
			
			command.Parameters.Add (CreateParameter("@lightboxId", lightboxLinked.LightboxId));
			command.Parameters.Add (CreateParameter("@userId", lightboxLinked.UserId));
			command.Parameters.Add (CreateParameter("@isEditable", lightboxLinked.IsEditable));
			command.Parameters.Add (CreateParameter("@expiryDate", lightboxLinked.ExpiryDate));
			command.Parameters.Add (CreateParameter("@disabled", lightboxLinked.Disabled));

			if (lightboxLinked.IsNew) 
			{
				lightboxLinked.LightboxLinkedId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@lightboxLinkedId", lightboxLinked.LightboxLinkedId));
				ExecuteCommand (command);
			}
			
			lightboxLinked.IsDirty = false;
			lightboxLinked.ChangedProperties.Clear();
			
			return lightboxLinked;
		}

		public virtual void Delete (Nullable <Int32> lightboxLinkedId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [LightboxLinked] WHERE LightboxLinkedId = @lightboxLinkedId";
			command.Parameters.Add(CreateParameter("@lightboxLinkedId", lightboxLinkedId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single LightboxLinked object by lightboxLinkedId
		// </Summary>
		public virtual LightboxLinked Get (Nullable <Int32> lightboxLinkedId)
		{
			IDbCommand command = GetGetCommand (lightboxLinkedId);
			return (LightboxLinked) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual LightboxLinked FindOne (LightboxLinkedFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? LightboxLinked.Empty : entity as LightboxLinked;
		}
		
		public virtual EntityList <LightboxLinked> FindMany (LightboxLinkedFinder finder)
		{
			return (EntityList <LightboxLinked>) (base.FindMany(finder));
		}

		public virtual EntityList <LightboxLinked> FindMany (LightboxLinkedFinder finder, int Page, int PageSize)
		{
			return (EntityList <LightboxLinked>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> lightboxLinkedId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [LightboxLinked] WHERE LightboxLinkedId = @lightboxLinkedId";
			command.Parameters.Add(CreateParameter("@lightboxLinkedId", lightboxLinkedId)); 
			
			return command;
		}
	}
}

