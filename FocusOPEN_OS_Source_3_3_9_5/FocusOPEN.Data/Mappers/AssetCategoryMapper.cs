/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Data;

namespace FocusOPEN.Data
{
	/// <summary>
	/// This object maps data between the database and a AssetCategory object.
	/// </summary>
	internal partial class AssetCategoryMapper
	{
		#region Singleton behaviour

		private AssetCategoryMapper()
		{
		}
		
		private static readonly AssetCategoryMapper m_instance = new AssetCategoryMapper();
		public static AssetCategoryMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

        internal void DeleteAssetCategories(int assetId)
        {
            IDbCommand command = CreateCommand();
            command.CommandText = "DELETE FROM [AssetCategory] WHERE (AssetId = @assetId)";
            command.Parameters.Add(CreateParameter("@assetId", assetId));
            ExecuteCommand(command);
        }
	}
}