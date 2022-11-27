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
using System.Security;
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AssetFileHandler : BaseFileHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			// Get querystring values
			int assetId = GetIdFromFilename();
			int assetImageSizeId = WebUtils.GetIntRequestParam("assetImageSizeId", 0);
			DownloadFormat downloadFormat = GeneralUtils.ParseEnum(WebUtils.GetRequestParam("AssetImageFormat"), DownloadFormat.Original);
			bool logDownload = (WebUtils.GetIntRequestParam("logDownload", 1) == 1);
			bool original = (WebUtils.GetIntRequestParam("original", 0) == 1);

			// Downloads should ALWAYS be logged from normal users, regardless of the querystring
			if (SessionInfo.Current.User.UserRole == UserRole.Normal)
				logDownload = true;

			// Ensure asset id is specified
			if (assetId == 0)
				throw new HttpException(500, "Invalid asset id");

			// Get asset
			Asset asset = Asset.Get(assetId);

			// Make sure asset is valid
			if (asset.IsNull)
			{
				Context.Response.Redirect("~/Errors/AssetMissing.htm");
				return;
			}

			// Check if user can download asset
			bool isAuthorised = EntitySecurityManager.CanUserDownloadAsset(SessionInfo.Current.User, asset);

			if (!isAuthorised)
				throw new SecurityException("Access denied");

			// Get the asset file info
			AssetFileInfo info = new AssetFileInfo(asset);

			// Ensure file exists
			if (!info.FileExists)
			{
				Context.Response.Redirect("~/Errors/AssetFileMissing.htm");
				return;
			}

			// Asset file path
			string path = info.FilePath;

			// Update the audit history
			if (logDownload)
			{
				AuditLogManager.LogAssetAction(assetId, SessionInfo.Current.User, AuditAssetAction.DownloadedAssetFile);
				AuditLogManager.LogUserAction(SessionInfo.Current.User, AuditUserAction.DownloadAsset, string.Format("Downloaded asset with AssetId: {0}", assetId));
			}

            DownloadAsset(asset, path, original, downloadFormat, assetImageSizeId);
        }


        #region Protected Methods

        /// <summary>
        /// Downloads the asset.
        /// </summary>
        protected void DownloadAsset(Asset asset, string path, bool original, DownloadFormat downloadFormat, int assetImageSizeId)
        {
            // Get the asset filename
            string filename = asset.Filename;

            // If the zip asset files functionality is enabled and the user is download the current version
            // then send them the zipped asset file instead, so update the path and filename accordingly
            if (AssetFileManager.ZipAssetFiles && !original)
            {
                // First get the path to the zipped asset file
                ZippedAssetFileInfo zippedFileInfo = new ZippedAssetFileInfo(asset);

                if (zippedFileInfo.FileExists)
                {
                    path = zippedFileInfo.FilePath;
                    filename = Path.GetFileNameWithoutExtension(asset.Filename) + ".zip";
                }
            }

            // Get the resized image if asset is an image and an image size has been specified
            if (assetImageSizeId != 0 || downloadFormat != DownloadFormat.Original)
            {
                path = AssetImageManager.GetResizedAssetImage(asset, assetImageSizeId, downloadFormat, true);
                filename = Path.GetFileName(path);
            }

            WriteFileToResponseStream(path, filename, true);

        }



        #endregion


    }
}