using System.IO;
using System.Linq;
using FocusOPEN.Data;

namespace FocusOPEN.Business
{
	public class AssetBitmapInfo : AbstractAssetFileInfo
	{
		public string Reference { get; set; }
		
		public AssetBitmapInfo(Asset asset, string reference) : base(asset, "AssetBitmapGroups")
		{
			Reference = reference;
		}

		public override string FolderPath
		{
			get
			{
				string subFolder = Path.Combine(SubFolder, Asset.AssetId.ToString());
				return Path.Combine(Asset.AssetFilePath.Path, subFolder);
			}
		}

		public override string FilePath
		{
			get
			{
				if (!Directory.Exists(FolderPath))
					return string.Empty;

				string reference = string.Concat(Reference, "_", Asset.AssetId);

				return (from file in Directory.GetFiles(FolderPath, reference + ".*")
						select file).FirstOrDefault() ?? string.Empty;
			}
		}
	}
}