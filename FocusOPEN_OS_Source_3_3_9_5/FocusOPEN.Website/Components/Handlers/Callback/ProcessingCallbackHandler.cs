/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalAssetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class ProcessingCallbackHandler : AbstractProcessingCallbackHandler
	{
		protected override void ProcessFiles()
		{
			if (!StringUtils.IsBlank(AdditionalData.AssetBitmapReference))
			{
				Context.Response.Write("ERROR - Asset Bitmap Reference is not blank. Use BitmapProcessingCallbackHandler instead.");
				return;
			}

			if (Asset.IsProcessed)
			{
				Context.Response.Write("ERROR - Asset has already been processed");
				return;
			}

			List<String> warningsList = new List<String>();

#if DEBUG
			// Save the returned metadata XML
			string metadataXmlPath = Path.GetTempFileName() + "_" + AssetId + ".xml";
			File.WriteAllText(metadataXmlPath, MetadataXml);
			m_Logger.DebugFormat("Saved Metadata XML to: {0}", metadataXmlPath);
#endif

			try
			{
				// Ensure we're saving valid metadata XML by trying to load it before saving

				XmlDocument doc = new XmlDocument();
				doc.LoadXml(MetadataXml);

				// Save Asset metadata XML
				Asset.MetadataXml = MetadataXml;
				Asset.MetadataSearchVals = GetMetadataString(Asset.GetFileMetadata(true));
			}
			catch (Exception ex)
			{
				Asset.MetadataXml = string.Empty;
				Asset.MetadataSearchVals = string.Empty;

				string error = string.Format("Error parsing metadata returned from APS for Asset: {0}.  Error: {1}", AssetId, ex.Message);

				ExceptionHandler.HandleException(ex, error);

				warningsList.Add(error);
			}

			// Mark Asset as processed
			Asset.IsProcessed = true;

			// Save other FileDataItems (FDI)
			Asset.Height = WebUtils.GetIntRequestParam("FDI_Height", 0);
			Asset.Width = WebUtils.GetIntRequestParam("FDI_Width", 0);
			Asset.Duration = WebUtils.GetIntRequestParam("FDI_Duration", 0);
			
			// Update database
			Asset.Update(Asset);
			Asset.SaveAssetMetadata(Asset);

			m_Logger.Debug("Updated Asset data");

			// Save preview
			if (StringUtils.IsBlank(PreviewPath))
			{
				m_Logger.Debug("No preview file to save");
				warningsList.Add("No preview file to save");
			}
			else if (File.Exists(PreviewPath))
			{
				try
				{
					AssetFileManager.SaveAssetFile(Asset, new BinaryFile(PreviewPath, BinaryFile.SaveMode.Move), AssetFileType.AssetPreview);
					m_Logger.Debug("Saved Asset preview file");
				}
				catch (Exception ex)
				{
					string error = string.Format("Error saving preview file {0}.  Error: {1}", PreviewPath, ex.Message);
					
					m_Logger.Warn(error, ex);
					warningsList.Add(error);
				}
			}
			else
			{
				string error = string.Format("Preview file supplied does not exist: {0}", PreviewPath);

				m_Logger.Warn(error);
				warningsList.Add(error);
			}

			// Save thumbnail
			if (StringUtils.IsBlank(ThumbnailPath))
			{
				m_Logger.Warn("No thumbnail file to save");
				warningsList.Add("No thumbnail file to save");
			}
			else if (File.Exists(ThumbnailPath))
			{
				try
				{
					AssetFileManager.SaveAssetFile(Asset, new BinaryFile(ThumbnailPath, BinaryFile.SaveMode.Move), AssetFileType.AssetThumbnail);
					m_Logger.Debug("Saved Asset thumbnail file");
				}
				catch (Exception ex)
				{
					string error = string.Format("Error saving thumbnail file {0}.  Error: {1}", ThumbnailPath, ex.Message);
					
					m_Logger.Warn(error, ex);
					warningsList.Add(error);
				}
			}
			else
			{
				string error = string.Format("Thumbnail file supplied does not exist: {0}", ThumbnailPath);

				m_Logger.Warn(error);
				warningsList.Add(error);
			}

			// Republish the Asset
			if (ExternalPublisher.Instance.IsPublished(AssetId))
				ExternalPublisher.Instance.Publish(AssetId);

			// Notify uploader if required
			if (AdditionalData.Notify)
				NotifyEngine.AssetProcessingComplete(Asset);

			// All done
			WriteLine("OK");
			WriteLine(Environment.NewLine);

			// Write some debug info
			WriteLine("Date: {0}", DateTime.Now);
			WriteLine("Server: {0}", Context.Server.MachineName);
			WriteLine("Notify: {0}", AdditionalData.Notify);
			WriteLine(Environment.NewLine);

			// Write warning count
			WriteLine("Warning Message Count: {0}", warningsList.Count);

			// Write warning messages
			if (warningsList.Count > 0)
			{
				WriteLine(Environment.NewLine);
				warningsList.ForEach(WriteLine);
			}
		}

		private static string GetMetadataString(IEnumerable<KeyValuePair<string, string>> list)
		{
			StringBuilder sb = new StringBuilder();

			foreach (KeyValuePair<string, string> kvp in list)
				sb.Append(kvp.Value + Environment.NewLine);

			return sb.ToString();
		}
	}
}