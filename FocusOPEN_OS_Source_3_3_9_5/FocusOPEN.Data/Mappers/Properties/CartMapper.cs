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
	/// This object maps data between the database and a Cart object.
	/// </summary>
	internal partial class CartMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Cart cart = Cart.New();

			// Table Fields
			cart.CartId = reader.GetInt32("CartId"); 
			cart.UserId = reader.GetInt32("UserId");
			cart.AssetId = reader.GetInt32("AssetId");
			cart.Notes = reader.GetString("Notes");
			cart.RequiredByDate = reader.GetNullableDateTime("RequiredByDate");
			cart.DateAdded = reader.GetDateTime("DateAdded");
			
			// View Fields

			cart.IsDirty = false;
			cart.ChangedProperties.Clear();
			
			return cart;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Cart>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Cart Update (Cart cart)
		{
 			if (!cart.IsDirty || cart.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return cart;
			}
			
			IDbCommand command = CreateCommand();
			
			if (cart.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Cart] ([UserId], [AssetId], [Notes], [RequiredByDate], [DateAdded]) VALUES (@userId, @assetId, @notes, @requiredByDate, @dateAdded) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Cart] SET [UserId] = @userId, [AssetId] = @assetId, [Notes] = @notes, [RequiredByDate] = @requiredByDate, [DateAdded] = @dateAdded WHERE CartId = @cartId"; 
			}
			
			command.Parameters.Add (CreateParameter("@userId", cart.UserId));
			command.Parameters.Add (CreateParameter("@assetId", cart.AssetId));
			command.Parameters.Add (CreateParameter("@notes", cart.Notes));
			command.Parameters.Add (CreateParameter("@requiredByDate", cart.RequiredByDate));
			command.Parameters.Add (CreateParameter("@dateAdded", cart.DateAdded));

			if (cart.IsNew) 
			{
				cart.CartId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@cartId", cart.CartId));
				ExecuteCommand (command);
			}
			
			cart.IsDirty = false;
			cart.ChangedProperties.Clear();
			
			return cart;
		}

		public virtual void Delete (Nullable <Int32> cartId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Cart] WHERE CartId = @cartId";
			command.Parameters.Add(CreateParameter("@cartId", cartId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Cart object by cartId
		/// </Summary>
		public virtual Cart Get (Nullable <Int32> cartId)
		{
			IDbCommand command = GetGetCommand (cartId);
			return (Cart) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Cart FindOne (CartFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Cart.Empty : entity as Cart;
		}
		
		public virtual EntityList <Cart> FindMany (CartFinder finder)
		{
			return (EntityList <Cart>) (base.FindMany(finder));
		}

		public virtual EntityList <Cart> FindMany (CartFinder finder, int Page, int PageSize)
		{
			return (EntityList <Cart>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> cartId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Cart] WHERE CartId = @cartId";
			command.Parameters.Add(CreateParameter("@cartId", cartId)); 
			
			return command;
		}
	}
}

