/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.Runtime.Serialization;

namespace FocusOPEN.APS
{
	[DataContract]
	public class Job
	{
		/// <summary>
		/// Gets or sets the Asset ID
		/// </summary>
		[DataMember]
		public int AssetId { get; set; }

		/// <summary>
		/// Gets or sets the path to the file to be processed
		/// </summary>
		[DataMember]
		public string InputPath { get; set; }

		/// <summary>
		/// Gets or sets the path to the watermark to be applied to the preview
		/// </summary>
		[DataMember]
		public string WatermarkPath { get; set; }

		/// <summary>
		/// Gets or sets the plugin to be used for processing
		/// If none is specified, will default to plugin based on file extension
		/// </summary>
		[DataMember]
		public string PluginName { get; set; }

		/// <summary>
		/// Boolean value specifying whether the thumbnail should be created
		/// </summary>
		[DataMember]
		public bool CreateThumbnail { get; set; }

		/// <summary>
		/// Boolean value specifying whether the preview should be created
		/// </summary>
		[DataMember]
		public bool CreatePreview { get; set; }

		/// <summary>
		/// Specifies a new height to use when generating the thumbnail and/or preview.
		/// If specified, this overrides the default value set in the APS configuration
		/// </summary>
		[DataMember]
		public int OverrideHeight { get; set; }

		/// <summary>
		/// Specifies a new width to use when generating the thumbnail and/or preview.
		/// If specified, this overrides the default value set in the APS configuration
		/// </summary>
		[DataMember]
		public int OverrideWidth { get; set; }

		/// <summary>
		/// URL which processing results should be posted to after processing is complete
		/// </summary>
		[DataMember]
		public string CallbackUrl { get; set; }

		/// <summary>
		/// Any other data that should be posted back to the callback URL after processing is complete
		/// </summary>
		[DataMember]
		public string AdditionalData { get; set; }
	}
}