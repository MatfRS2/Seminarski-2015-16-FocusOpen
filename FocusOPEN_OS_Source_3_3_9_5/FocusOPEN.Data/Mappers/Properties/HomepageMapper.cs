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
	/// This object maps data between the database and a Homepage object.
	/// </summary>
	internal partial class HomepageMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			Homepage homepage = Homepage.New();

			// Table Fields
			homepage.HomepageId = reader.GetInt32("HomepageId"); 
			homepage.BrandId = reader.GetInt32("BrandId");
			homepage.IntroText = reader.GetString("IntroText");
			homepage.Url1 = reader.GetString("Url1");
			homepage.Url2 = reader.GetString("Url2");
			homepage.Url3 = reader.GetString("Url3");
			homepage.Url4 = reader.GetString("Url4");
			homepage.BumperPageHtml = reader.GetString("BumperPageHtml");
			homepage.BumperPageSkip = reader.GetBoolean("BumperPageSkip");
			homepage.CustomHtml = reader.GetString("CustomHtml");
			homepage.HomepageTypeId = reader.GetInt32("HomepageTypeId");
			homepage.IsPublished = reader.GetBoolean("IsPublished");
			homepage.LastModifiedByUserId = reader.GetInt32("LastModifiedByUserId");
			homepage.LastModifiedDate = reader.GetDateTime("LastModifiedDate");
			
			// View Fields
			homepage.BrandName = reader.GetString("BrandName");

			homepage.IsDirty = false;
			homepage.ChangedProperties.Clear();
			
			return homepage;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <Homepage>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual Homepage Update (Homepage homepage)
		{
 			if (!homepage.IsDirty || homepage.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return homepage;
			}
			
			IDbCommand command = CreateCommand();
			
			if (homepage.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [Homepage] ([BrandId], [IntroText], [Url1], [Url2], [Url3], [Url4], [BumperPageHtml], [BumperPageSkip], [CustomHtml], [HomepageTypeId], [IsPublished], [LastModifiedByUserId], [LastModifiedDate]) VALUES (@brandId, @introText, @url1, @url2, @url3, @url4, @bumperPageHtml, @bumperPageSkip, @customHtml, @homepageTypeId, @isPublished, @lastModifiedByUserId, @lastModifiedDate) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [Homepage] SET [BrandId] = @brandId, [IntroText] = @introText, [Url1] = @url1, [Url2] = @url2, [Url3] = @url3, [Url4] = @url4, [BumperPageHtml] = @bumperPageHtml, [BumperPageSkip] = @bumperPageSkip, [CustomHtml] = @customHtml, [HomepageTypeId] = @homepageTypeId, [IsPublished] = @isPublished, [LastModifiedByUserId] = @lastModifiedByUserId, [LastModifiedDate] = @lastModifiedDate WHERE HomepageId = @homepageId"; 
			}
			
			command.Parameters.Add (CreateParameter("@brandId", homepage.BrandId));
			command.Parameters.Add (CreateParameter("@introText", homepage.IntroText));
			command.Parameters.Add (CreateParameter("@url1", homepage.Url1));
			command.Parameters.Add (CreateParameter("@url2", homepage.Url2));
			command.Parameters.Add (CreateParameter("@url3", homepage.Url3));
			command.Parameters.Add (CreateParameter("@url4", homepage.Url4));
			command.Parameters.Add (CreateParameter("@bumperPageHtml", homepage.BumperPageHtml));
			command.Parameters.Add (CreateParameter("@bumperPageSkip", homepage.BumperPageSkip));
			command.Parameters.Add (CreateParameter("@customHtml", homepage.CustomHtml));
			command.Parameters.Add (CreateParameter("@homepageTypeId", homepage.HomepageTypeId));
			command.Parameters.Add (CreateParameter("@isPublished", homepage.IsPublished));
			command.Parameters.Add (CreateParameter("@lastModifiedByUserId", homepage.LastModifiedByUserId));
			command.Parameters.Add (CreateParameter("@lastModifiedDate", homepage.LastModifiedDate));

			if (homepage.IsNew) 
			{
				homepage.HomepageId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@homepageId", homepage.HomepageId));
				ExecuteCommand (command);
			}
			
			homepage.IsDirty = false;
			homepage.ChangedProperties.Clear();
			
			return homepage;
		}

		public virtual void Delete (Nullable <Int32> homepageId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [Homepage] WHERE HomepageId = @homepageId";
			command.Parameters.Add(CreateParameter("@homepageId", homepageId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single Homepage object by homepageId
		/// </Summary>
		public virtual Homepage Get (Nullable <Int32> homepageId)
		{
			IDbCommand command = GetGetCommand (homepageId);
			return (Homepage) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual Homepage FindOne (HomepageFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? Homepage.Empty : entity as Homepage;
		}
		
		public virtual EntityList <Homepage> FindMany (HomepageFinder finder)
		{
			return (EntityList <Homepage>) (base.FindMany(finder));
		}

		public virtual EntityList <Homepage> FindMany (HomepageFinder finder, int Page, int PageSize)
		{
			return (EntityList <Homepage>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> homepageId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [v_Homepage] WHERE HomepageId = @homepageId";
			command.Parameters.Add(CreateParameter("@homepageId", homepageId)); 
			
			return command;
		}
	}
}

