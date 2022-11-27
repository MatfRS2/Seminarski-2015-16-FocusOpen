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
using Daydream.Data;

namespace FocusOPEN.Website.Components.Handlers
{
    /// <summary>
    /// Allows external users to download assets from a contact sheet.
    /// </summary>
    public class AssetFileExtHandler : AssetFileHandler
	{
		public override void ProcessRequest()
		{

            if (!SessionInfo.Current.User.IsNull)
            {
                //if valid user then process request
                //using normal AssetFileHandler
                base.ProcessRequest();
            }
            else
            {

                // Get querystring values
                int assetId = GetIdFromFilename();
                int assetImageSizeId = WebUtils.GetIntRequestParam("assetImageSizeId", 0);
                DownloadFormat downloadFormat = GeneralUtils.ParseEnum(WebUtils.GetRequestParam("AssetImageFormat"), DownloadFormat.Original);
                bool original = (WebUtils.GetIntRequestParam("original", 0) == 1);
                int lightboxSentId = WebUtils.GetIntRequestParam("lsid", 0);
                int senderId = WebUtils.GetIntRequestParam("suid", 0);
                string ticks = WebUtils.GetRequestParam("dst", string.Empty);

                // Ensure asset id is specified
                if (assetId == 0 || lightboxSentId == 0)
                {
                    InvalidRequest();
                    return;
                }


                // Get the lightbox
                LightboxSent lightboxSent = LightboxSent.Get(lightboxSentId);

                //check that it's a valid lightboxsent object
                if (lightboxSent.IsNull)
                {
                    InvalidRequest();
                    return;
                }


                // Check posted data - ensure that the sender id and ticks match (ie. to ensure user is not messing with the querystring)
                if (!lightboxSent.SenderId.Equals(senderId) || ticks.Length < 6 || !lightboxSent.DateSent.Ticks.ToString().Substring(0, 6).Equals(ticks.Substring(0, 6)))
                {
                    InvalidRequest();
                    return;
                }


                // Make sure sender is a super user
                if (lightboxSent.Sender.UserRole != UserRole.SuperAdministrator)
                {
                    InvalidRequest();
                    return;
                }


                //verify that lightbox has download links enabled
                if (!lightboxSent.DownloadLinks.GetValueOrDefault(false))
                {
                    InvalidRequest();
                    return;
                }


                //check that asset exists in the lightbox being sent
                LightboxAssetFinder finder = new LightboxAssetFinder { LightboxId = lightboxSent.LightboxId, AssetId = assetId };
                LightboxAsset lightboxAsset = LightboxAsset.FindOne(finder);

                if (lightboxAsset.IsNull)
                {
                    InvalidRequest();
                    return;
                }


                // Get the asset file info
                AssetFileInfo info = new AssetFileInfo(lightboxAsset.Asset);

                // Ensure file exists
                if (!info.FileExists)
                {
                    InvalidRequest();
                    return;
                }

                // Asset file path
                string path = info.FilePath;

                // Always update the audit history for external downloads
                AuditLogManager.LogAssetAction(assetId, lightboxSent.Sender, AuditAssetAction.DownloadedAssetFile);
                AuditLogManager.LogUserAction(lightboxSent.Sender, AuditUserAction.DownloadAssetFromContactSheet, string.Format("Downloaded asset {0} via contact sheet, sent by: {1} download by: {2}", assetId, lightboxSent.Sender.FullName, lightboxSent.RecipientEmail));

                DownloadAsset(lightboxAsset.Asset, path, original, downloadFormat, assetImageSizeId);

            }

        }


        #region Private Methods

        private void InvalidRequest()
        {
            Context.Response.Redirect("~/Errors/DownloadNotAllowed.htm");
         

        }




        #endregion


    }
}