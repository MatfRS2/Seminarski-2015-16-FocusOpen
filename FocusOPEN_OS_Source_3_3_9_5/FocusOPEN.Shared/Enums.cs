/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

using System.ComponentModel;
using System;

namespace FocusOPEN.Shared
{
	public enum AssetPublicationStatus
	{
		AllAssets,

		[Description("Pre-Publication Assets")]
		PrepublicationDateAssets,

		PendingAssets,

		PublishedAssets,

		UnpublishedAssets,

		ExpiredAssets,
	}

	public enum AssetStatus
	{
		Available,
		Withdrawn,
		Expired
	}

	public enum TimeFrame
	{
		SinceUpload,

		[Description("Last 12 Months")]
		Last12Months,

		[Description("Last 6 Months")]
		Last6Months,

		[Description("Last 3 Months")]
		Last3Months,

		LastMonth
	}
	
	public enum OrderBy
	{
		Relevance,
		Popularity,
		Date,
		Custom
	}
	
	public enum DownloadFormat
	{
		Original,
		JPEG,
		TIFF,
		PNG
	}

    [FlagsAttribute]
    public enum PluginStatus
    {
        None = 0,
        InvalidFormat = 1,
        PluginMissing = 2,
        ResourcesMissing = 4,
        ChecksumMismatch = 8,
        NoStandardContext = 16
    }

    public enum BrandMetadataUiControlType
    {
        [Description("Text Field")]
        TextField = 1,

        [Description("Text Area")]
        TextArea = 2,

        [Description("Select")]
        Select
    }

    [SerializableAttribute]
    public enum ContextType
    {
        Standard,
        Lightbox,
        Cart,
        SearchResults,
        OrderManager,
        ViewAssetList,
        CatalogueAsset,
        AdjustAsset,
        AssetStatistics
    }

    public enum RegistrationEmailFormatType
    {
        Empty,
        InternalUsers,
        AllUsers
    }

	public enum AssetRestrictionType
	{
		HideFromUsers,
		DownloadApprovalRequired
	}
}