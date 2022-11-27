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
	public abstract class AbstractAssetFileInfo
	{
		#region Constructor

		protected AbstractAssetFileInfo(Asset asset, string subfolder)
		{
			Asset = asset;
			SubFolder = subfolder;
		}

		#endregion

		#region Simple Accessors

		public string SubFolder { get; private set; }

		public Asset Asset { get; private set; }

		#endregion

		public virtual string FolderPath
		{
			get
			{
				return Path.Combine(Asset.AssetFilePath.Path, SubFolder);
			}
		}

		public virtual string FilePath
		{
			get
			{
				return (from file in Directory.GetFiles(FolderPath, Asset.FileReference + ".*")
				        select file).FirstOrDefault() ?? string.Empty;
			}
		}

		public bool FileExists
		{
			get
			{
				return (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath));
			}
		}
	}
}
