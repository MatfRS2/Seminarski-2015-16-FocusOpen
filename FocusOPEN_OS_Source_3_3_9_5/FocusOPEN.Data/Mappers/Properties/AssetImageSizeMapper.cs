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
	/// This object maps data between the database and a AssetImageSize object.
	/// </summary>
	internal partial class AssetImageSizeMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			AssetImageSize assetImageSize = AssetImageSize.New();

			// Table Fields
			assetImageSize.AssetImageSizeId = reader.GetInt32("AssetImageSizeId"); 
			assetImageSize.Description = reader.GetString("Description");
			assetImageSize.Height = reader.GetInt32("Height");
			assetImageSize.Width = reader.GetInt32("Width");
			assetImageSize.DotsPerInch = reader.GetInt32("DotsPerInch");
			assetImageSize.FileSuffix = reader.GetString("FileSuffix");
			

			assetImageSize.IsDirty = false;
			assetImageSize.ChangedProperties.Clear();
			
			return assetImageSize;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <AssetImageSize>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual AssetImageSize Update (AssetImageSize assetImageSize)
		{
 			if (!assetImageSize.IsDirty || assetImageSize.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return assetImageSize;
			}
			
			IDbCommand command = CreateCommand();
			
			if (assetImageSize.IsNew) 
			{
				// Adding
				command.CommandText = "INSERT INTO [AssetImageSize] ([Description], [Height], [Width], [DotsPerInch], [FileSuffix]) VALUES (@description, @height, @width, @dotsPerInch, @fileSuffix) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
				command.CommandText = "UPDATE [AssetImageSize] SET [Description] = @description, [Height] = @height, [Width] = @width, [DotsPerInch] = @dotsPerInch, [FileSuffix] = @fileSuffix WHERE AssetImageSizeId = @assetImageSizeId"; 
			}
			
			command.Parameters.Add (CreateParameter("@description", assetImageSize.Description));
			command.Parameters.Add (CreateParameter("@height", assetImageSize.Height));
			command.Parameters.Add (CreateParameter("@width", assetImageSize.Width));
			command.Parameters.Add (CreateParameter("@dotsPerInch", assetImageSize.DotsPerInch));
			command.Parameters.Add (CreateParameter("@fileSuffix", assetImageSize.FileSuffix));

			if (assetImageSize.IsNew) 
			{
				assetImageSize.AssetImageSizeId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@assetImageSizeId", assetImageSize.AssetImageSizeId));
				ExecuteCommand (command);
			}
			
			assetImageSize.IsDirty = false;
			assetImageSize.ChangedProperties.Clear();
			
			return assetImageSize;
		}

		public virtual void Delete (Nullable <Int32> assetImageSizeId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [AssetImageSize] WHERE AssetImageSizeId = @assetImageSizeId";
			command.Parameters.Add(CreateParameter("@assetImageSizeId", assetImageSizeId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single AssetImageSize object by assetImageSizeId
		/// </Summary>
		public virtual AssetImageSize Get (Nullable <Int32> assetImageSizeId)
		{
			IDbCommand command = GetGetCommand (assetImageSizeId);
			return (AssetImageSize) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual AssetImageSize FindOne (AssetImageSizeFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? AssetImageSize.Empty : entity as AssetImageSize;
		}
		
		public virtual EntityList <AssetImageSize> FindMany (AssetImageSizeFinder finder)
		{
			return (EntityList <AssetImageSize>) (base.FindMany(finder));
		}

		public virtual EntityList <AssetImageSize> FindMany (AssetImageSizeFinder finder, int Page, int PageSize)
		{
			return (EntityList <AssetImageSize>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> assetImageSizeId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [AssetImageSize] WHERE AssetImageSizeId = @assetImageSizeId";
			command.Parameters.Add(CreateParameter("@assetImageSizeId", assetImageSizeId)); 
			
			return command;
		}
	}
}

