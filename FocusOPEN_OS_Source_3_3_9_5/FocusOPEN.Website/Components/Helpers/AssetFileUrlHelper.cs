/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Website.Components
{
	public static class AssetFileUrlHelper
	{
		/// <summary>
		/// Gets the URL to the asset thumbnail
		/// Eg. ~/AssetThumbnail.[assetId].ashx
		/// </summary>
		public static string GetThumbnailUrl(int? assetId)
		{
			return "~/AssetThumbnail." + assetId + ".ashx";
		}

		/// <summary>
		/// Gets the URL to the asset preview
		/// Eg. ~/AssetPreview.[assetId].ashx
		/// </summary>
		public static string GetPreviewUrl(int? assetId)
		{
			return "~/AssetPreview." + assetId + ".ashx";
		}

		/// <summary>
		/// Gets the URL to download the asset file
		/// Eg. ~/AssetFile.[assetId].ashx
		/// </summary>
		public static string GetDownloadUrl(int? assetId)
		{
			return "~/AssetFile." + assetId + ".ashx";
		}

		/// <summary>
		/// Gets the URL to the asset file externally
		/// </summary>
        public static string GetExternalDownloadUrl(int? assetId, int? lightboxSentId, int senderUserId, long dateSentTicks)
        {
            return string.Format("~/AssetFileExt." + assetId + ".ashx?lsid={0}&suid={1}&dst={2}", lightboxSentId, senderUserId, dateSentTicks);
        }
	}
}