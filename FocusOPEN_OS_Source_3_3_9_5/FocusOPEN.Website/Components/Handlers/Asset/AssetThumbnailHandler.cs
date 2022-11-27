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
using System.Web;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AssetThumbnailHandler : BaseFileHandler, IReadOnlySessionState
	{
		public static string GetFixedThumbnailUrl(Asset asset)
		{
			string[] fixedThumbnailExtensions = { "gif", "jpg" };

			foreach (string ext in fixedThumbnailExtensions)
			{
				string fixedThumbnailFilename = "~/Images/Asset/Thumbnails/" + asset.AssetType.Name.ToLower() + "." + ext;
				string fixedThumbnailPath = HttpContext.Current.Server.MapPath(fixedThumbnailFilename);

				if (File.Exists(fixedThumbnailPath))
					return fixedThumbnailFilename;
			}

			return string.Empty;
		}

		public override void ProcessRequest()
		{
			int assetId = GetIdFromFilename();
			Asset asset = Asset.Get(assetId);

			string filename;

			if (asset.IsNull)
			{
				filename = "~/Images/Spacer.gif";
			}
			else if (!EntitySecurityManager.CanUserViewAssetThumbnail(SessionInfo.Current.User, asset))
			{
				filename = "~/Images/Spacer.gif";
			}
			else
			{
				// First check if we need to display a fixed thumbnail
				filename = GetFixedThumbnailUrl(asset);

				// No fixed thumbnail, so get real thumbnail
				if (filename == string.Empty)
				{
					if (asset.IsProcessed)
					{
						// Asset is processed, so show either the thumbnail or unavailable image
						AssetThumbnailInfo info = new AssetThumbnailInfo(asset);
						filename = (info.FileExists) ? info.FilePath : "~/Images/Asset/Thumbnails/Unavailable.gif";
					}
					else
					{
						// Otherwise, asset is not processed, so show the processing image
						filename = "~/Images/Asset/Thumbnails/Processing.gif";
					}
				}
			}

			// Fix relative mappings
			if (filename.StartsWith("~/"))
				filename = Context.Server.MapPath(filename);

			// Send the image
			WriteFileToResponseStream(filename);
		}
	}
}