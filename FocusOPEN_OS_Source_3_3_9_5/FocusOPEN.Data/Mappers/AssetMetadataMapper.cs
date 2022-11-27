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
	/// This object maps data between the database and a AssetMetadata object.
	/// </summary>
	internal partial class AssetMetadataMapper
	{
		#region Singleton behaviour

		private AssetMetadataMapper()
		{
		}
		
		private static readonly AssetMetadataMapper m_instance = new AssetMetadataMapper();
		public static AssetMetadataMapper Instance
		{
			get
			{
				return m_instance;
			}
		}

		#endregion

		public void DeleteAllAssetMetadata(int assetId)
		{
			IDbCommand command = CreateCommand();
			command.CommandText = "DELETE FROM [AssetMetadata] WHERE (AssetId = @assetId)";
			command.Parameters.Add(CreateParameter("@assetId", assetId));
			ExecuteCommand(command);

            command.CommandText = "DELETE FROM [AssetMetadataTextArea] WHERE (AssetId = @assetId)";
            ExecuteCommand(command);

            command.CommandText = "DELETE FROM [AssetMetadataTextField] WHERE (AssetId = @assetId)";
            ExecuteCommand(command);


		}
	}
}

