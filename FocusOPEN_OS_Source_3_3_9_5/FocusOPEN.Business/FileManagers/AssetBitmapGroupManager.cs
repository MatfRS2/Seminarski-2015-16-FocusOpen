using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using System.Xml.XPath;
using FocusOPEN.Business.APS;
using FocusOPEN.Data;
using FocusOPEN.Shared;
using log4net;

namespace FocusOPEN.Business
{
	public static class AssetBitmapGroupManager
	{
		private static readonly ILog m_Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string ConfigurationPath { get; set; }

		public static IList<AssetBitmapGroup> AssetBitmapGroups
		{
			get
			{
				return (from n in XDocument.Load(ConfigurationPath).XPathSelectElements("//AssetBitmapGroup")
				        let MaxWidth = Convert.ToInt32(n.Element("MaxWidth").Value)
				        let MaxHeight = Convert.ToInt32(n.Element("MaxHeight").Value)
				        select new AssetBitmapGroup
				               	{
				               		Reference = n.Element("Reference").Value,
				               		MaxSize = new Size(MaxWidth, MaxHeight)
				               	}
				       ).ToList();
			}
		}

		public static void DeleteAssetBitmapsFolder(Asset asset)
		{
			string path1 = Path.Combine(asset.AssetFilePath.Path, "AssetBitmapGroups");
			string path2 = Path.Combine(path1, asset.AssetId.ToString());

			if (Directory.Exists(path2))
			{
				try
				{
					Directory.Delete(path2);
					m_Logger.DebugFormat("Deleted bitmaps folder: {0}", path2);
				}
				catch (Exception ex)
				{
					m_Logger.Warn(string.Format("Error deleting bitmaps folder: {0}. Error: {1}", path2, ex.Message), ex);
				}
			}
		}

		/// <summary>
		/// Deletes the bitmaps folder and resubmits asset to regenerate all bitmaps
		/// </summary>
		public static void Generate(Asset asset)
		{
			DeleteAssetBitmapsFolder(asset);

			m_Logger.DebugFormat("Submitting all asset bitmap groups.  Asset: {0}, Number of Groups: {1}", asset.AssetId, AssetBitmapGroups.Count);

			foreach (var abg in AssetBitmapGroups)
				Generate(asset, abg);

			m_Logger.DebugFormat("Done resubmitting all asset bitmap groups for asset: {0}", asset.AssetId);
		}

		public static void Generate(Asset asset, AssetBitmapGroup abg)
		{
			if (abg == null)
				throw new NullReferenceException("AssetBitmapGroup cannot be null");

			if (!APSGateway.Instance.CanProcess(asset.FileExtension))
			{
				m_Logger.WarnFormat("APS cannot process extension '{0}'.  Asset Bitmap Group will not be generated", asset.FileExtension);
				return;
			}

			AssetFileInfo afi = new AssetFileInfo(asset);

			if (!afi.FileExists)
				return;
			
			Job job = new Job
			{
				AssetId = asset.AssetId.GetValueOrDefault(),
				InputPath = afi.FilePath,
				CreatePreview = false,
				CreateThumbnail = true,
				OverrideWidth = abg.MaxSize.Width,
				OverrideHeight = abg.MaxSize.Height,
				CallbackUrl = "~/BitmapProcessingCallback.ashx",
				AdditionalData = string.Format("<AdditionalData><AssetBitmapReference>{0}</AssetBitmapReference><Notify>False</Notify></AdditionalData>", abg.Reference)
			};

			long jobId = APSGateway.Instance.SubmitJob(job);

			m_Logger.DebugFormat("Asset Bitmap Generation submitted to APS with Job Id: {0}.  BitmapGroup reference: {1}, MaxWidth: {2}, MaxHeight: {3}", jobId, abg.Reference, abg.MaxSize.Width, abg.MaxSize.Height);
		}

		public static void SaveAssetBitmapFile(Asset asset, string assetBitmapReference, BinaryFile file)
		{
			if (asset.IsNew || asset.IsNull)
				return;

			if (file.IsEmpty)
				return;

			if (StringUtils.IsBlank(assetBitmapReference))
				return;

			var abg = new AssetBitmapInfo(asset, assetBitmapReference);

			if (abg.FileExists)
				File.Delete(abg.FilePath);

			if (!Directory.Exists(abg.FolderPath))
			{
				Directory.CreateDirectory(abg.FolderPath);
				m_Logger.DebugFormat("Created folder: {0}", abg.FolderPath);
			}

			string path = Path.Combine(abg.FolderPath, abg.Reference + "_" + abg.Asset.AssetId + "." + file.FileExtension);
			file.SaveAs(path);
		}

		public class AssetBitmapGroup
		{
			public string Reference { get; set; }
			public Size MaxSize { get; set; }
		}
	}
}