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
using FocusOPEN.Data;

namespace FocusOPEN.Website.Components.Handlers
{
	public class AssetTypeFileExtensionHandler : BaseFileHandler
	{
		public override void ProcessRequest()
		{
			string token = GetTokenFromFilename();
			AssetTypeFileExtension o = AssetTypeFileExtensionCache.Instance.GetByExtension(token);

			if (!o.IsNull && o.IconImage != null && o.IconImage.Length > 0)
			{
				SiteUtils.SendFile(o.IconFilename, o.IconImage, false);
				return;
			}

			// Look for an icon for this file extension on disk
			string path = Context.Server.MapPath("~/Images/Icons/File/" + token + ".gif");

			// Finally, resort to generic
			if (!File.Exists(path))
				path = Context.Server.MapPath("~/Images/Icons/File/Generic.gif");

			// Enure icon exists
			if (!File.Exists(path))
				throw new FileNotFoundException(Path.GetFileName(path));

			WriteFileToResponseStream(path, null, false);
			return;
		}
	}
}