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
	internal partial class PluginMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Plugin plugin = Plugin.New();

			// Table Fields
			plugin.PluginId = reader.GetInt32("PluginId"); 
			plugin.RegistrationKey = reader.GetGuid("RegistrationKey");
			plugin.RelativePath = reader.GetString("RelativePath");
			plugin.Filename = reader.GetString("Filename");
			plugin.Name = reader.GetString("Name");
			plugin.Checksum = reader.GetNullableInt32("Checksum");
			plugin.PluginType = reader.GetNullableInt32("PluginType");
			plugin.IsDefault = reader.GetNullableBoolean("IsDefault");
			

			plugin.IsDirty = false;
			plugin.ChangedProperties.Clear();
			
			return plugin;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Plugin>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Plugin Update (Plugin plugin)
		{
 			if (!plugin.IsDirty || plugin.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return plugin;
			}
			
			IDbCommand command = CreateCommand();
			
			if (plugin.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Plugins] ([RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (@registrationKey, @relativePath, @filename, @name, @checksum, @pluginType, @isDefault) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Plugins] SET [RegistrationKey] = @registrationKey, [RelativePath] = @relativePath, [Filename] = @filename, [Name] = @name, [Checksum] = @checksum, [PluginType] = @pluginType, [IsDefault] = @isDefault WHERE PluginId = @pluginId"; 
			}
			
			command.Parameters.Add (CreateParameter("@registrationKey", plugin.RegistrationKey));
			command.Parameters.Add (CreateParameter("@relativePath", plugin.RelativePath));
			command.Parameters.Add (CreateParameter("@filename", plugin.Filename));
			command.Parameters.Add (CreateParameter("@name", plugin.Name));
			command.Parameters.Add (CreateParameter("@checksum", plugin.Checksum));
			command.Parameters.Add (CreateParameter("@pluginType", plugin.PluginType));
			command.Parameters.Add (CreateParameter("@isDefault", plugin.IsDefault));

			if (plugin.IsNew) 
			{
				plugin.PluginId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@pluginId", plugin.PluginId));
				ExecuteCommand (command);
			}
			
			plugin.IsDirty = false;
			plugin.ChangedProperties.Clear();
			
			return plugin;
		}

		public virtual void Delete (Nullable <Int32> pluginId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Plugins] WHERE PluginId = @pluginId";
			command.Parameters.Add(CreateParameter("@pluginId", pluginId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Plugin object by pluginId
		// </Summary>
		public virtual Plugin Get (Nullable <Int32> pluginId)
		{
			IDbCommand command = GetGetCommand (pluginId);
			return (Plugin) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Plugin FindOne (PluginFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Plugin.Empty : entity as Plugin;
		}
		
		public virtual EntityList <Plugin> FindMany (PluginFinder finder)
		{
			return (EntityList <Plugin>) (base.FindMany(finder));
		}

		public virtual EntityList <Plugin> FindMany (PluginFinder finder, int Page, int PageSize)
		{
			return (EntityList <Plugin>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> pluginId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [Plugins] WHERE PluginId = @pluginId";
			command.Parameters.Add(CreateParameter("@pluginId", pluginId)); 
			
			return command;
		}
	}
}

