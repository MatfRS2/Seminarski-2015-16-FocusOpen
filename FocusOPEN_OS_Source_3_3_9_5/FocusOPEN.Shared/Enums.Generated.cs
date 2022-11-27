/**********************************************************************************************************************
	FocusOPEN Digital Asset Manager (TM) 
	(c) Daydream Interactive Limited 1995-2011, All Rights Reserved

	The name and trademarks of copyright holders may NOT be used in advertising or publicity pertaining to the software
	without specific, written prior permission. Title to copyright in this software and any associated documentation
	will at all times remain with copyright holders.

	Please refer to licences/focusopen.txt or http://www.digitalassetmanager.com for licensing information about this
	software. 
**********************************************************************************************************************/

namespace FocusOPEN.Shared
{
	public enum AssetFileType
	{
		AssetFile = 1,
		AssetFileZipped = 2,
		AssetPreview = 3,
		AssetThumbnail = 4,
		AttachedFile = 5
	}
	public enum AssetPublishStatus
	{
		NotApproved = 1,
		PendingApproval = 2,
		Approved = 3,
		NotPublished = 4,
		Published = 5
	}
	public enum AssetWorkflowUserStatus
	{
		Waiting = 1,
		Pending = 2,
		Approved = 3,
		Rejected = 4
	}
	public enum AuditAssetAction
	{
		AddedToCart = 1,
		AddedToLightbox = 2,
		ApprovedForDownload = 3,
		CataloguedAsset = 4,
		DeleteAsset = 5,
		DownloadedAssetFile = 6,
		DownloadedAttachedFile = 7,
		Ordered = 8,
		RejectedForDownload = 9,
		RemovedFromCart = 10,
		RemovedFromLightbox = 11,
		ReplacedAssetFile = 12,
		ReturnedInSearch = 13,
		SavedAsset = 14,
		UploadedAsset = 15,
		ViewedAssetDetail = 16
	}
	public enum AuditUserAction
	{
		AccountUpdate = 1,
		AddAssetType = 2,
		AddBrand = 3,
		AddCategory = 4,
		AddCompany = 5,
		AddFileExtension = 6,
		AddFilepath = 7,
		AddIpAddress = 8,
		AddLightbox = 9,
		AddMetadata = 10,
		AddToCart = 11,
		AddToLightbox = 12,
		AddUser = 13,
		AuthoriseOrder = 14,
		AuthoriseUser = 15,
		CatalogueAsset = 16,
		ChangeDefaultFilepath = 17,
		ChangePassword = 18,
		ConfirmEmailAddress = 19,
		DeleteAsset = 20,
		DeleteAssetType = 21,
		DeleteBrand = 22,
		DeleteCategory = 23,
		DeleteCompany = 24,
		DeleteFileExtension = 25,
		DeleteIpAddress = 26,
		DeleteKeyword = 27,
		DeleteMetadata = 28,
		DeleteUser = 29,
		DownloadAsset = 30,
		DownloadAttachedFile = 31,
		DuplicateFileExtension = 32,
		EditAsset = 33,
		EmptyCart = 34,
		Logout = 35,
		ModifyAssetType = 36,
		ModifyBrand = 37,
		ModifyCategory = 38,
		ModifyCompany = 39,
		ModifyFileExtension = 40,
		ModifyIpAddress = 41,
		ModifyLightbox = 42,
		ModifyMetadata = 43,
		ModifyUser = 44,
		OrderAssets = 45,
		PasswordReminder = 46,
		PreviewHomepage = 47,
		PublishHomepage = 48,
		ReactivateAccount = 49,
		Register = 50,
		RegisteredByAdmin = 51,
		RemoveFromCart = 52,
		RemoveFromLightbox = 53,
		RemoveLightbox = 54,
		ReplacedAssetFile = 55,
		Search = 56,
		SendLightbox = 57,
		SystemError = 58,
		UploadAsset = 59,
		UserLogin = 60,
		ViewedAssetDetail = 61,
		DownloadAssetFromContactSheet = 62,
		APICall = 63
	}
	public enum OrderItemStatus
	{
		Preapproved = 1,
		AwaitingApproval = 2,
		Approved = 3,
		Rejected = 4
	}
	public enum UserRole
	{
		Normal = 1,
		UploadUser = 2,
		BrandAdministrator = 3,
		SuperAdministrator = 4
	}
	public enum UserStatus
	{
		PendingAdminApproval = 1,
		PendingEmailConfirmation = 2,
		Approved = 3,
		Rejected = 4
	}
}