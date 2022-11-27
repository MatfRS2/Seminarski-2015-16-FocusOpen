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
	/// This object maps data between the database and a HomepageType object.
	/// </summary>
	internal partial class HomepageTypeMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			HomepageType homepageType = HomepageType.New();

			// Table Fields
			homepageType.HomepageTypeId = reader.GetInt32("HomepageTypeId"); 
			homepageType.Description = reader.GetString("Description");
			homepageType.ShortName = reader.GetString("ShortName");
			

			homepageType.IsDirty = false;
			homepageType.ChangedProperties.Clear();
			
			return homepageType;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <HomepageType>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual HomepageType Update (HomepageType homepageType)
		{
 			if (!homepageType.IsDirty || homepageType.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return homepageType;
			}
			
			IDbCommand command = CreateCommand();
			
			if (homepageType.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [HomepageType] ([Description], [ShortName]) VALUES (@description, @shortName) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [HomepageType] SET [Description] = @description, [ShortName] = @shortName WHERE HomepageTypeId = @homepageTypeId"; 
			}
			
			command.Parameters.Add (CreateParameter("@description", homepageType.Description));
			command.Parameters.Add (CreateParameter("@shortName", homepageType.ShortName));

			if (homepageType.IsNew) 
			{
				homepageType.HomepageTypeId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@homepageTypeId", homepageType.HomepageTypeId));
				ExecuteCommand (command);
			}
			
			homepageType.IsDirty = false;
			homepageType.ChangedProperties.Clear();
			
			return homepageType;
		}

		public virtual void Delete (Nullable <Int32> homepageTypeId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [HomepageType] WHERE HomepageTypeId = @homepageTypeId";
			command.Parameters.Add(CreateParameter("@homepageTypeId", homepageTypeId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single HomepageType object by homepageTypeId
		/// </Summary>
		public virtual HomepageType Get (Nullable <Int32> homepageTypeId)
		{
			IDbCommand command = GetGetCommand (homepageTypeId);
			return (HomepageType) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual HomepageType FindOne (HomepageTypeFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? HomepageType.Empty : entity as HomepageType;
		}
		
		public virtual EntityList <HomepageType> FindMany (HomepageTypeFinder finder)
		{
			return (EntityList <HomepageType>) (base.FindMany(finder));
		}

		public virtual EntityList <HomepageType> FindMany (HomepageTypeFinder finder, int Page, int PageSize)
		{
			return (EntityList <HomepageType>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> homepageTypeId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [HomepageType] WHERE HomepageTypeId = @homepageTypeId";
			command.Parameters.Add(CreateParameter("@homepageTypeId", homepageTypeId)); 
			
			return command;
		}
	}
}

