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
	/// This object maps data between the database and a IpAddress object.
	/// </summary>
	internal partial class IpAddressMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			IpAddress ipAddress = IpAddress.New();

			// Table Fields
			ipAddress.IpAddressId = reader.GetInt32("IpAddressId"); 
			ipAddress.IpAddressValue = reader.GetString("IpAddressValue");
			

			ipAddress.IsDirty = false;
			ipAddress.ChangedProperties.Clear();
			
			return ipAddress;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <IpAddress>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual IpAddress Update (IpAddress ipAddress)
		{
 			if (!ipAddress.IsDirty || ipAddress.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return ipAddress;
			}
			
			IDbCommand command = CreateCommand();
			
			if (ipAddress.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [IpAddress] ([IpAddressValue]) VALUES (@ipAddressValue) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [IpAddress] SET [IpAddressValue] = @ipAddressValue WHERE IpAddressId = @ipAddressId"; 
			}
			
			command.Parameters.Add (CreateParameter("@ipAddressValue", ipAddress.IpAddressValue));

			if (ipAddress.IsNew) 
			{
				ipAddress.IpAddressId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@ipAddressId", ipAddress.IpAddressId));
				ExecuteCommand (command);
			}
			
			ipAddress.IsDirty = false;
			ipAddress.ChangedProperties.Clear();
			
			return ipAddress;
		}

		public virtual void Delete (Nullable <Int32> ipAddressId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [IpAddress] WHERE IpAddressId = @ipAddressId";
			command.Parameters.Add(CreateParameter("@ipAddressId", ipAddressId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single IpAddress object by ipAddressId
		/// </Summary>
		public virtual IpAddress Get (Nullable <Int32> ipAddressId)
		{
			IDbCommand command = GetGetCommand (ipAddressId);
			return (IpAddress) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual IpAddress FindOne (IpAddressFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? IpAddress.Empty : entity as IpAddress;
		}
		
		public virtual EntityList <IpAddress> FindMany (IpAddressFinder finder)
		{
			return (EntityList <IpAddress>) (base.FindMany(finder));
		}

		public virtual EntityList <IpAddress> FindMany (IpAddressFinder finder, int Page, int PageSize)
		{
			return (EntityList <IpAddress>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> ipAddressId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [IpAddress] WHERE IpAddressId = @ipAddressId";
			command.Parameters.Add(CreateParameter("@ipAddressId", ipAddressId)); 
			
			return command;
		}
	}
}

