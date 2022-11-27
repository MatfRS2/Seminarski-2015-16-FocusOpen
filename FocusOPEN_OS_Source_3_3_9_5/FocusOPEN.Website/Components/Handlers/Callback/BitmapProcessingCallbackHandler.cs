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
using FocusOPEN.Business;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class BitmapProcessingCallbackHandler : AbstractProcessingCallbackHandler
	{
		protected override void ProcessFiles()
		{
			if (StringUtils.IsBlank(AdditionalData.AssetBitmapReference))
			{
				Context.Response.Write("ERROR - Asset Bitmap Reference is missing.");
				return;
			}

			List<String> warningsList = new List<String>();

			if (StringUtils.IsBlank(PreviewPath))
			{
				m_Logger.Debug("No preview file to save");
				warningsList.Add("No preview file to save");
			}
			else if (!File.Exists(PreviewPath))
			{
				string error = string.Format("Preview file supplied does not exist: {0}", PreviewPath);

				m_Logger.Warn(error);
				warningsList.Add(error);
			}
			else
			{
				try
				{
					AssetBitmapGroupManager.SaveAssetBitmapFile(Asset, AdditionalData.AssetBitmapReference, new BinaryFile(PreviewPath, BinaryFile.SaveMode.Move));
					m_Logger.Debug("Saved Asset bitmap file (preview)");
				}
				catch (Exception ex)
				{
					string error = string.Format("Error saving asset bitmap preview file {0}.  Error: {1}", PreviewPath, ex.Message);

					m_Logger.Warn(error, ex);
					warningsList.Add(error);
				}
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
					AssetBitmapGroupManager.SaveAssetBitmapFile(Asset, AdditionalData.AssetBitmapReference, new BinaryFile(ThumbnailPath, BinaryFile.SaveMode.Move));
					m_Logger.Debug("Saved Asset bitmap file (thumbnail)");
				}
				catch (Exception ex)
				{
					string error = string.Format("Error saving asset bitmap thumbnail file {0}.  Error: {1}", ThumbnailPath, ex.Message);

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
	}
}