/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Web.SessionState;
using FocusOPEN.Business;
using FocusOPEN.Data;
using FocusOPEN.Shared;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AssetBitmapGroupHandler : BaseFileHandler, IReadOnlySessionState
	{
		public override void ProcessRequest()
		{
			string[] tokens = GetTokensFromFilename();

			if (tokens.Length != 3)
			{
				Context.Response.Write("Invalid number of tokens");
				return;
			}

			// Get required tokens
			int assetId = NumericUtils.ParseInt32(tokens[1], 0);
			string reference = tokens[2];

			// Invalid or missing Asset ID
			if (assetId <= 0)
			{
				UseUnavailableImage();
				return;
			}

			// Get the asset
			var asset = Asset.Get(assetId);

			// Asset not found
			if (asset.IsNull)
			{
				UseUnavailableImage();
				return;
			}

			// Ensure user can view asset preview (as asset bitmaps are effectively previews)
			if (!EntitySecurityManager.CanUserViewAssetPreview(SessionInfo.Current.User, asset))
			{
				UseUnavailableImage();
				return;
			}

			// Missing reference
			if (string.IsNullOrEmpty(reference))
			{
				UseUnavailableImage();
				return;
			}

			// Get the bitmap info
			var abi = new AssetBitmapInfo(asset, reference);

			// Use unavailable image if file doesn't exist
			if (!abi.FileExists)
			{
				// Bitmap file not found
				UseUnavailableImage();
				return;
			}

			// Request bitmap found, send it
			WriteFileToResponseStream(abi.FilePath);
		}

		private void UseUnavailableImage()
		{
			var filename = Context.Server.MapPath("~/Images/Asset/Previews/Unavailable.gif");
			WriteFileToResponseStream(filename);
		}
	}
}