/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web;
using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AssetPreviewHandler : BaseFileHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			int assetId = GetIdFromFilename();
			Asset asset = Asset.Get(assetId);

			if (asset.IsNull)
				throw new HttpException(500, "Asset does not exist");

			string filename = string.Empty;

			if (EntitySecurityManager.CanUserViewAssetPreview(SessionInfo.Current.User, asset))
			{
				AssetPreviewInfo info = new AssetPreviewInfo(asset);
				filename = info.FilePath;
			}

			if (filename == string.Empty)
			{
				filename = (asset.IsProcessed) ? "Unavailable.gif" : "Processing.gif";
				filename = Context.Server.MapPath("~/Images/Asset/Previews/" + filename);
			}

			WriteFileToResponseStream(filename);
		}
	}
}