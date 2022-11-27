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
using System.Linq;
using FocusOPEN.Data;

namespace FocusOPEN.Business
{
	public class AssetThumbnailInfo : AbstractAssetFileInfo
	{
		public AssetThumbnailInfo(Asset asset) : base(asset, "AssetThumbnails")
		{
		}

		public override string FilePath
		{
			get
			{
				string path = base.FilePath;

				if (string.IsNullOrEmpty(path))
				{
					// Specify which image extensions are valid for thumbnails
					// These are just images, only JPG and GIF for now.
					string[] thumbnailExtensions = { "jpg", "gif" };

					// Assume thumbnails will be JPG's
					string extension = "jpg";

					// Check if the asset file extension is the same as that of a supported extension
					// This is so we can make the thumbnail the same format - eg. animated GIF's
					if (thumbnailExtensions.Any(ext => (ext == Asset.FileExtension)))
						extension = Asset.FileExtension;

					// Generate the full path to the thumbnail image
					path = Path.Combine(FolderPath, Asset.FileReference) + "." + extension;
				}

				return path;
			}
		}
	}
}