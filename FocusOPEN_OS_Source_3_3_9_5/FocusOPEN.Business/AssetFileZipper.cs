/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.IO;
using System.Reflection;
using FocusOPEN.Data;
using ICSharpCode.SharpZipLib.Zip;
using log4net;

namespace FocusOPEN.Business
{
	public class AssetFileZipper
	{
		#region Private Variables
		
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		private readonly Asset m_Asset;

		#endregion

		#region Accessors

		private string ZipFilePath
		{
			get
			{
				string filename = m_Asset.AssetId.ToString().PadLeft(7, '0') + ".zip";
				string directory = Path.Combine(m_Asset.AssetFilePath.Path, "AssetFilesZipped");
				return Path.Combine(directory, filename);
			}
		}

		#endregion

		private AssetFileZipper(Asset asset)
		{
			m_Asset = asset;
		}

		private void CreateZip()
		{
			// Delete the old zip file
			if (File.Exists(ZipFilePath))
			{
				m_Logger.Debug("Deleted old asset zip file");
				File.Delete(ZipFilePath);
			}

			// Get the asset file info
			AssetFileInfo fileInfo = new AssetFileInfo(m_Asset);

			// Ensure asset file exists before continuing
			if (!fileInfo.FileExists)
				return;

			using (ZipOutputStream zos = new ZipOutputStream(File.Create(ZipFilePath)))
			{
				zos.SetLevel(9);

				ZipEntry ze = new ZipEntry(m_Asset.Filename);
				zos.PutNextEntry(ze);

				using (FileStream fs = File.OpenRead(fileInfo.FilePath))
				{
					int sourceBytes;
					byte[] buffer = new byte[10000];
					do
					{
						sourceBytes = fs.Read(buffer, 0, buffer.Length);
						zos.Write(buffer, 0, sourceBytes);
					}
					while (sourceBytes > 0);
				}

				m_Logger.DebugFormat("Added asset file: {0} to zip: {1}", fileInfo.FilePath, ZipFilePath);

				foreach (AssetFile file in m_Asset.GetAttachedFiles())
				{
					ZipEntry fze = new ZipEntry(file.Filename);
					zos.PutNextEntry(fze);
					zos.Write(file.FileContent, 0, file.FileContent.Length);

					m_Logger.DebugFormat("Added attached file: {0} to zip: {1}", file.Filename, ZipFilePath);
				}
			}
		}

		public static void CreateZip(Asset asset)
		{
			AssetFileZipper zipper = new AssetFileZipper(asset);
			zipper.CreateZip();
		}
	}
}