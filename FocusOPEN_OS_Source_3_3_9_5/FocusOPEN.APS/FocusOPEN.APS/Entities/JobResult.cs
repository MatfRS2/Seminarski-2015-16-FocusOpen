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
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace FocusOPEN.APS
{
	[DataContract]
	public class JobResult
	{
		#region Constructor

		public JobResult(int assetId, string inputFile, string thumbnailPath, string previewPath, string additionalData, XElement metadataXml)
		{
			AssetId = assetId;
			InputFile = inputFile;
			ThumbnailPath = thumbnailPath;
			PreviewPath = previewPath;
			MetadataXml = metadataXml;
			AdditionalData = AdditionalData;
		}

		#endregion

		#region Properties

		[DataMember]
		public int AssetId { get; private set; }

		[DataMember]
		public string InputFile { get; private set; }

		[DataMember]
		public string ThumbnailPath { get; private set; }

		[DataMember]
		public string PreviewPath { get; private set; }

		[DataMember]
		public string AdditionalData { get; private set; }

		[DataMember]
		public XElement MetadataXml { get; private set; }

		#endregion

		public bool IsThumbnailGenerated()
		{
			return (!string.IsNullOrEmpty(ThumbnailPath) && File.Exists(ThumbnailPath));
		}

		public bool IsPreviewGenerated()
		{
			return (!string.IsNullOrEmpty(PreviewPath) && File.Exists(PreviewPath));
		}
	}
}