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
	internal partial class BrandMetadataSelectableSettingMapper : AbstractDataMapper
	{
		#region AbstractDataMapper Implementation
	
		protected override object ReadRow(IRowReader reader)
		{
			BrandMetadataSelectableSetting brandMetadataSelectableSetting = BrandMetadataSelectableSetting.New();

			// Table Fields
			brandMetadataSelectableSetting.BrandMetadataSelectableSettingId = reader.GetInt32("BrandMetadataSelectableSettingId");

            brandMetadataSelectableSetting.BrandMetadataSettingId = reader.GetInt32("BrandMetadataSettingId");
            brandMetadataSelectableSetting.SelectableType = reader.GetInt32("SelectableType");
            brandMetadataSelectableSetting.Depth = reader.GetInt32("Depth");
            brandMetadataSelectableSetting.IsLinear = reader.GetBoolean("IsLinear");
            brandMetadataSelectableSetting.SortType = reader.GetInt32("SortType");
            brandMetadataSelectableSetting.AllowMultiple = reader.GetBoolean("AllowMultiple");
            brandMetadataSelectableSetting.OrderType = reader.GetInt32("OrderType");
            brandMetadataSelectableSetting.ColumnCount = reader.GetInt32("ColumnCount");
            brandMetadataSelectableSetting.FilterGroup = reader.GetInt32("FilterGroup");
            brandMetadataSelectableSetting.FilterSelectableType = reader.GetInt32("FilterSelectableType");
            brandMetadataSelectableSetting.FilterDepth = reader.GetInt32("FilterDepth");

			brandMetadataSelectableSetting.IsDirty = false;
			brandMetadataSelectableSetting.ChangedProperties.Clear();
			
			return brandMetadataSelectableSetting;
		}

		protected override IEntityList CreateObjectList()
		{
			return new EntityList <BrandMetadataSelectableSetting>();
		}
		
		public override IEntity GetFromKey (object key)
		{
			return Get ((Nullable <Int32>)key);
		}
		
		#endregion
		
		#region Create, Update and Delete Methods
		
		public virtual BrandMetadataSelectableSetting Update (BrandMetadataSelectableSetting brandMetadataSelectableSetting)
		{
 			if (!brandMetadataSelectableSetting.IsDirty || brandMetadataSelectableSetting.IsNull)
			{
				// Nothing to do - no point hammering the database unnecessarily
				return brandMetadataSelectableSetting;
			}
			
			IDbCommand command = CreateCommand();
			
			if (brandMetadataSelectableSetting.IsNew) 
			{
				// Adding
                command.CommandText 
                        = @"INSERT INTO [BrandMetadataSelectableSetting] 
                                ([BrandMetadataSettingId], 
                                 [SelectableType], 
                                 [Depth], 
                                 [IsLinear], 
                                 [SortType], 
                                 [AllowMultiple], 
                                 [OrderType], 
                                 [ColumnCount],
                                 [FilterGroup],
                                 [FilterSelectableType],
                                 [FilterDepth]

                                 ) VALUES (
                                 @brandMetadataSettingId, 
                                 @selectableType		, 
                                 @depth				, 
                                 @isLinear			, 
                                 @sortType			, 
                                 @allowMultiple		, 
                                 @orderType			, 
                                 @columnCount       ,
                                 @filterGroup       ,
                                 @filterSelectableType,       
                                 @filterDepth       
                                    ) ; SELECT @@identity AS NewId;"; 
			}
			else 
			{
				// Updating
                command.CommandText = @"UPDATE [BrandMetadataSelectableSetting] 
                                        SET 
                                            [BrandMetadataSettingId]=   @brandMetadataSettingId, 
                                            [SelectableType]=  @selectableType, 
                                            [Depth]=  @depth, 
                                            [IsLinear]=  @isLinear, 
                                            [SortType]=  @sortType, 
                                            [AllowMultiple]=  @allowMultiple, 
                                            [OrderType]=  @orderType, 
                                            [ColumnCount]=  @columnCount,				 
                                            [FilterGroup]=  @filterGroup,				 
                                            [FilterSelectableType]=  @filterSelectableType,				 
                                            [FilterDepth]=  @filterDepth				 

                                        WHERE BrandMetadataSelectableSettingId = @brandMetadataSelectableSettingId"; 
			}

            command.Parameters.Add(CreateParameter("@brandMetadataSettingId", brandMetadataSelectableSetting.BrandMetadataSettingId));
            command.Parameters.Add(CreateParameter("@selectableType", brandMetadataSelectableSetting.SelectableType));
            command.Parameters.Add(CreateParameter("@depth", brandMetadataSelectableSetting.Depth));
            command.Parameters.Add(CreateParameter("@isLinear", brandMetadataSelectableSetting.IsLinear));
            command.Parameters.Add(CreateParameter("@sortType", brandMetadataSelectableSetting.SortType));
            command.Parameters.Add(CreateParameter("@allowMultiple", brandMetadataSelectableSetting.AllowMultiple));
            command.Parameters.Add(CreateParameter("@orderType", brandMetadataSelectableSetting.OrderType));
            command.Parameters.Add(CreateParameter("@columnCount", brandMetadataSelectableSetting.ColumnCount));
            command.Parameters.Add(CreateParameter("@filterGroup", brandMetadataSelectableSetting.FilterGroup));
            command.Parameters.Add(CreateParameter("@filterSelectableType", brandMetadataSelectableSetting.FilterSelectableType));
            command.Parameters.Add(CreateParameter("@filterDepth", brandMetadataSelectableSetting.FilterDepth));

			if (brandMetadataSelectableSetting.IsNew) 
			{
				brandMetadataSelectableSetting.BrandMetadataSelectableSettingId = Convert.ToInt32(ExecScalar (command));
			}
			else
			{
				command.Parameters.Add (CreateParameter("@brandMetadataSelectableSettingId", brandMetadataSelectableSetting.BrandMetadataSelectableSettingId));
				ExecuteCommand (command);
			}
			
			brandMetadataSelectableSetting.IsDirty = false;
			brandMetadataSelectableSetting.ChangedProperties.Clear();
			
			return brandMetadataSelectableSetting;
		}

		public virtual void Delete (Nullable <Int32> brandMetadataSelectableSettingId)
		{
			IDbCommand command = CreateCommand ();
			command.CommandText = "DELETE FROM [BrandMetadataSelectableSetting] WHERE BrandMetadataSelectableSettingId = @brandMetadataSelectableSettingId";
			command.Parameters.Add(CreateParameter("@brandMetadataSelectableSettingId", brandMetadataSelectableSettingId)) ;

			ExecuteCommand(command);
		}
		
		#endregion

		#region Find Methods
		
		/// <Summary>
		/// Get a single BrandMetadataSelectableSetting object by brandMetadataSelectableSettingId
		/// </Summary>
		public virtual BrandMetadataSelectableSetting Get (Nullable <Int32> brandMetadataSelectableSettingId)
		{
			IDbCommand command = GetGetCommand (brandMetadataSelectableSettingId);
			return (BrandMetadataSelectableSetting) LendReader (command, new SingleRowReader(this));
		}
		
		public virtual BrandMetadataSelectableSetting FindOne (BrandMetadataSelectableSettingFinder finder)
		{
			IEntity entity = base.FindOne(finder);
			return (entity == null) ? BrandMetadataSelectableSetting.Empty : entity as BrandMetadataSelectableSetting;
		}
		
		public virtual EntityList <BrandMetadataSelectableSetting> FindMany (BrandMetadataSelectableSettingFinder finder)
		{
			return (EntityList <BrandMetadataSelectableSetting>) (base.FindMany(finder));
		}

		public virtual EntityList <BrandMetadataSelectableSetting> FindMany (BrandMetadataSelectableSettingFinder finder, int Page, int PageSize)
		{
			return (EntityList <BrandMetadataSelectableSetting>) (base.FindMany(finder, Page, PageSize));
		}
		
		#endregion
		
		private IDbCommand GetGetCommand(Nullable <Int32> brandMetadataSelectableSettingId)
		{
			IDbCommand command = CreateCommand ();
			
			command.CommandText = "SELECT * FROM [BrandMetadataSelectableSetting] WHERE BrandMetadataSelectableSettingId = @brandMetadataSelectableSettingId";
			command.Parameters.Add(CreateParameter("@brandMetadataSelectableSettingId", brandMetadataSelectableSettingId)); 
			
			return command;
		}
	}
}

