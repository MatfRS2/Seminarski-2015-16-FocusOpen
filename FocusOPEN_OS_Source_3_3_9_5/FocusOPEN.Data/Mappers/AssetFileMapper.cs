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
using System.IO;
using Daydream.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Data
{
	internal partial class AssetFileMapper
	{
		#region Singleton

		private static AssetFileMapper m_Instance = null;

		private AssetFileMapper()
		{
		}

		public static AssetFileMapper Instance
		{
			get
			{
				if (m_Instance == null)
					m_Instance = new AssetFileMapper();

				return m_Instance;
			}
		}

		#endregion

		public void AddUpdateFileContents(int assetId, string filename, AssetFileType assetFileType, ByteArray byteArray, bool forceAdd)
		{
			IDbCommand command = CreateCommand();
			command.CommandType = CommandType.StoredProcedure;
			command.CommandText = "usp_AddUpdateAssetFile";

			// Add AssetId Parameter
			IDbDataParameter assetIdParameter = m_factory.CreateParameter("@AssetId", assetId);
			assetIdParameter.DbType = DbType.Int32;
			command.Parameters.Add(assetIdParameter);

			// Add ContentBytes Parameter
			IDbDataParameter contentBytesParameter = m_factory.CreateParameter("@FileContent", byteArray.ContentBytes);
			contentBytesParameter.DbType = DbType.Binary;
			command.Parameters.Add(contentBytesParameter);

			// Add Filename parameter
			IDbDataParameter fileNameParameter = m_factory.CreateParameter("@FileName", filename);
			fileNameParameter.DbType = DbType.String;
			command.Parameters.Add(fileNameParameter);

			// Add FileExtension parameter
			IDbDataParameter fileExtensionParameter = m_factory.CreateParameter("@FileExtension", (Path.GetExtension(filename) ?? string.Empty).ToLower());
			fileExtensionParameter.DbType = DbType.String;
			command.Parameters.Add(fileExtensionParameter);

			// Add AssetFileType parameter
			IDbDataParameter fileTypeParameter = m_factory.CreateParameter("@AssetFileTypeId", Convert.ToInt32(assetFileType));
			fileTypeParameter.DbType = DbType.Int32;
			command.Parameters.Add(fileTypeParameter);

			// Add ForceAdd parameter
			IDbDataParameter forceAddParameter = m_factory.CreateParameter("@ForceAdd", (forceAdd) ? 1 : 0);
			fileTypeParameter.DbType = DbType.Int32;
			command.Parameters.Add(forceAddParameter);

			ExecuteCommand(command);
		}

		/// <summary>
		/// Copies attached files from one asset to another.
		/// </summary>
		/// <param name="templateAssetId">The asset from which attached files should be copied.</param>
		/// <param name="childAssetId">The asset to which attached files should be copied.</param>
		public void CopyAttachedFiles(int templateAssetId, int childAssetId)
		{
			string commandText = "INSERT INTO AssetFile ([AssetId], [FileContent], [FileName], [FileExtension], [AssetFileTypeId], [LastUpdate])" + Environment.NewLine;
			commandText += string.Format("SELECT {0} AS [AssetId], [FileContent], [FileName], [FileExtension], [AssetFileTypeId], GETDATE() AS [LastUpdate] FROM [AssetFile] WHERE ([AssetId] = @AssetId) AND ([AssetFileTypeId] = @AssetFileTypeId)", childAssetId);

			IDbCommand command = CreateCommand();
			command.CommandType = CommandType.Text;
			command.CommandText = commandText;

			IDbDataParameter param1 = m_factory.CreateParameter("@AssetId", templateAssetId);
			param1.DbType = DbType.Int32;
			command.Parameters.Add(param1);

			IDbDataParameter param2 = m_factory.CreateParameter("@AssetFileTypeId", Convert.ToInt32(AssetFileType.AttachedFile));
			param2.DbType = DbType.Int32;
			command.Parameters.Add(param2);

			ExecuteCommand(command);
		}
	}
}