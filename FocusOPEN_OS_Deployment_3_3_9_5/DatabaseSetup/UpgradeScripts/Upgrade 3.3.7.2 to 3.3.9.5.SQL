/*
 Upgrade FocusOPEN 3.3.7.1 (/.2) database schema to 3.3.9.5
*/

IF EXISTS(SELECT * FROM sysobjects where name = 'v_Asset')
	DROP VIEW dbo.v_Asset
GO

ALTER TABLE [dbo].[AssetLink] DROP
	CONSTRAINT [FK_AssetLink_Asset],
	CONSTRAINT [FK_AssetLink_LinkedAsset]
GO

ALTER TABLE [dbo].[Asset] DROP CONSTRAINT [DF_Asset_DownloadApprovalRequired]
GO

ALTER TABLE [dbo].[Asset] DROP CONSTRAINT [DF_Asset_HideFromExternalUsers]
GO

ALTER TABLE [dbo].[BrandMetadataSetting] DROP CONSTRAINT [DF_BrandMetadataSetting_OtherData]
GO

DROP PROCEDURE [dbo].[usp_GetAssetAuditHistory]
GO

CREATE FUNCTION [dbo].[GenerateUserAPIToken]
(
	@email varchar(150)
)
RETURNS varchar(32) 
AS
BEGIN
	
	DECLARE @Result varchar(32)

	SELECT @Result= master.dbo.fn_varbintohexsubstring(0, HASHBYTES('md5', @email + CONVERT(varchar(23),GETDATE(),126)), 1, 0)

	RETURN @Result
END
GO

CREATE FUNCTION [dbo].[GetShadowDate]
(
	@productionYear INT = NULL,
	@productionMonth INT = NULL,
	@productionDay INT = NULL
)
RETURNS DATETIME WITH SCHEMABINDING
AS
BEGIN

	DECLARE @returnDate DATETIME
	DECLARE @dateString VARCHAR(100)
	
	IF (ISNULL(@productionYear, 0) = 0 OR @productionYear < 1753)
	BEGIN
		SELECT @returnDate = CONVERT(DATETIME, '1753-01-01 00:00:00', 120)
	END
	ELSE IF (ISNULL(@productionMonth, 0) = 0)
	BEGIN
		SELECT @dateString = CAST(@productionYear as VARCHAR(4)) + '-01-01 00:00:00'
		SELECT @returnDate = CONVERT(DATETIME, @dateString, 120)
	END
	ELSE IF (ISNULL(@productionDay, 0) = 0)
	BEGIN
		SELECT @dateString = CAST(@productionYear as VARCHAR(4)) + '-' + CAST(@productionMonth as VARCHAR(2)) + '-01 00:00:00'
		SELECT @returnDate = CONVERT(DATETIME, @dateString, 120)
	END
	ELSE
	BEGIN
		SELECT @dateString = CAST(@productionYear as VARCHAR(4)) + '-' + CAST(@productionMonth as VARCHAR(2)) + '-' + CAST(@productionDay as VARCHAR(2)) + ' 00:00:00'
		SELECT @returnDate = CONVERT(DATETIME, @dateString, 120)
	END
	
	RETURN CONVERT(DATETIME, @returnDate, 120)
	
END
GO

CREATE TABLE [dbo].[Plugins]
(
[PluginId] [int] NOT NULL IDENTITY(1, 1),
[RegistrationKey] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Plugins_RegistrationKey] DEFAULT (newid()),
[RelativePath] [varchar] (800) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Filename] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Name] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Checksum] [int] NULL,
[PluginType] [int] NULL,
[IsDefault] [bit] NULL
)
GO

ALTER TABLE [dbo].[Plugins] ADD CONSTRAINT [PK_Plugins] PRIMARY KEY CLUSTERED  ([PluginId])
GO

CREATE UNIQUE NONCLUSTERED INDEX [IX_Plugins] ON [dbo].[Plugins] ([RegistrationKey])
GO

CREATE TABLE [dbo].[AssetMetadataTextArea]
(
[AssetMetadataTextAreaId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[GroupNumber] [int] NOT NULL,
[TextAreaValue] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[AssetMetadataTextArea] ADD CONSTRAINT [PK_AssetMetadataTextArea] PRIMARY KEY CLUSTERED  ([AssetMetadataTextAreaId])
GO

CREATE TABLE [dbo].[AssetMetadataTextField]
(
[AssetMetadataTextFieldId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[GroupNumber] [int] NOT NULL,
[TextFieldValue] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO

ALTER TABLE [dbo].[AssetMetadataTextField] ADD CONSTRAINT [PK_AssetMetadataTextField] PRIMARY KEY CLUSTERED  ([AssetMetadataTextFieldId])
GO

ALTER TABLE [dbo].[Brand] ADD
[FilterMarkup] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_FilterMarkup] DEFAULT ('')
GO

ALTER TABLE [dbo].[Asset] ADD
[ProductionDay] [int] NULL,
[InternalUsers_DownloadApprovalRequired] [bit] NOT NULL CONSTRAINT [DF_Asset_InternalUsers_DownloadApprovalRequired] DEFAULT ((0)),
[InternalUsers_HideFromUsers] [bit] NOT NULL CONSTRAINT [DF_Asset_InternalUsers_HideFromUsers] DEFAULT ((0)),
[ExternalUsers_DownloadApprovalRequired] [bit] NOT NULL CONSTRAINT [DF_Asset_ExternalUsers_DownloadApprovalRequired] DEFAULT ((0)),
[ExternalUsers_HideFromUsers] [bit] NOT NULL CONSTRAINT [DF_Asset_ExternalUsers_HideFromUsers] DEFAULT ((0)),
[Plugin] [uniqueidentifier] NULL
GO

ALTER TABLE [dbo].[Asset] DROP
	COLUMN [DownloadApprovalRequired],
	COLUMN [HideFromExternalUsers]
GO

ALTER VIEW dbo.v_LightboxAsset
AS
SELECT     LA.LightboxAssetId, LA.LightboxId, LA.AssetId, LA.Notes, LA.CreateDate, LA.OrderNumber
FROM      dbo.LightboxAsset AS LA INNER JOIN
                  dbo.Asset AS A ON LA.AssetId = A.AssetId
WHERE    (A.IsDeleted = 0)
GO

ALTER TABLE [dbo].[AssetTypeFileExtension] ADD
	[Plugin] [uniqueidentifier] NULL
GO

ALTER VIEW [dbo].[v_Brand]
AS
SELECT     BrandId, Name, ShortName, ApplicationName, OrganisationName, IsMasterBrand, WebsiteUrl, EmailFrom, IsBrandSelectionAllowed, LoginPageUpperCopy, LoginPageLowerCopy, 
                  DisablePoweredByLogo, DefaultUsageRestrictionsCopy, MyAccountCopy, AdminCopy, TermsConditionsCopy, PrivacyPolicyCopy, HideFilterSearch, HideCategorySearch, 
                  DirectDownloadEnabled, IsDeleted, FilterMarkup
FROM      dbo.Brand AS B
WHERE    (IsDeleted = 0)
GO

ALTER TABLE [dbo].[User] ADD
	[SessionAPIToken] [varchar] (38) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[LastAPIOperation] [datetime] NULL,
	[UserAPIToken] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__User__UserAPITok__750E476F] DEFAULT ([dbo].[GenerateUserAPIToken]('default@user.com'))
GO

ALTER VIEW [dbo].[v_User] AS
    SELECT  U.UserId,
            U.FirstName,
            U.LastName,
            U.FirstName + ' ' + U.LastName AS FullName,
            U.Email,
            U.[Password],
            U.PasswordSalt,
            U.PasswordExpiryDate,
            U.IsPasswordNonExpiring,
            dbo.IsExpired(U.PasswordExpiryDate, U.IsPasswordNonExpiring, NULL) AS IsPasswordExpired,
            U.IsEmployee,
            U.IsEngineer,
            U.CompanyName,
            U.PrimaryBrandId,
            U.CountryId,
            U.PhoneNumber,
            U.MobileNumber,
            U.RegisterDate,
            U.LastLoginDate,
            U.AccountExpiryDate,
            U.IsAccountNonExpiring,
            dbo.IsExpired(U.AccountExpiryDate, U.IsAccountNonExpiring, NULL) AS IsAccountExpired,
            U.IsSuspended,
            U.UserRoleId,
            U.UserStatusId,
            U.UseWorkflow,
            U.WorkflowId,
            U.Notes,
            U.IsAllowedExternalAccess,
            U.EnableFilePathIngestion,
            U.ApproverUserId,
            B.Name AS BrandName,
            US.Name AS UserStatusName,
            UR.Name AS UserRoleName,
            U.CatalogueAssetIdList,
            U.[Guid],
            U.IsDeleted,
            U.LastAPIOperation,
            U.UserAPIToken,
            U.SessionAPIToken
    FROM    dbo.[User] AS U
            INNER JOIN dbo.UserStatus AS US ON U.UserStatusId = US.UserStatusId
            INNER JOIN dbo.UserRole AS UR ON U.UserRoleId = UR.UserRoleId
            LEFT OUTER JOIN dbo.Brand AS B ON U.PrimaryBrandId = B.BrandId
    WHERE   ( U.IsDeleted = 0 )
GO

ALTER TABLE [dbo].[Metadata] ADD
	[MetadataOrder] [int] NOT NULL CONSTRAINT [DF_Metadata_MetadataOrder] DEFAULT ((1))
GO

ALTER VIEW [dbo].[v_Metadata]
AS
SELECT     MetadataId, BrandId, ParentMetadataId, Name, ExternalRef, Synonyms, GroupNumber, IsDeleted, MetadataOrder
FROM      dbo.Metadata AS M
WHERE    (IsDeleted = 0)
GO

ALTER TABLE [dbo].[BrandMetadataSetting] ADD
[UiControlType] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_UIControlType] DEFAULT ((0)),
[IsCustom] [bit] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_IsCustom] DEFAULT ((0)),
[CatalogueOrderNum] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_CatalogueOrderNum] DEFAULT ((0)),
[AssetDetailOrderNum] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_AssetDetailOrderNum] DEFAULT ((0))
GO

ALTER TABLE [dbo].[BrandMetadataSetting] DROP
COLUMN [AllowMultiple]
GO

CREATE TABLE [dbo].[BrandMetadataSelectableSetting]
(
[BrandMetadataSelectableSettingId] [int] NOT NULL IDENTITY(1, 1),
[BrandMetadataSettingId] [int] NOT NULL,
[SelectableType] [int] NOT NULL,
[Depth] [int] NOT NULL,
[IsLinear] [bit] NOT NULL,
[SortType] [int] NOT NULL,
[AllowMultiple] [bit] NOT NULL,
[OrderType] [int] NOT NULL,
[ColumnCount] [int] NOT NULL,
[FilterGroup] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSelectableSetting_FilterGroup] DEFAULT ((0)),
[FilterSelectableType] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSelectableSetting_FilterSelectableType] DEFAULT ((1)),
[FilterDepth] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSelectableSetting_FilterDepth] DEFAULT ((3))
)
GO

ALTER TABLE [dbo].[BrandMetadataSelectableSetting] ADD CONSTRAINT [PK_BrandMetadataSelectableSetting] PRIMARY KEY CLUSTERED  ([BrandMetadataSelectableSettingId])
GO

ALTER VIEW [dbo].[v_AssetTypeFileExtension]
AS
SELECT     ATFE.AssetTypeFileExtensionId, ATFE.Extension, ATFE.Name, ATFE.AssetTypeId, ATFE.IconImage, ATFE.IconFilename, ATFE.IsVisible, AT.Name AS AssetTypeName, ATFE.Plugin
FROM      dbo.AssetType AS AT RIGHT OUTER JOIN
                  dbo.AssetTypeFileExtension AS ATFE ON AT.AssetTypeId = ATFE.AssetTypeId
GO

ALTER FUNCTION [dbo].[GetProductionDate]
(
	@productionYear INT,
	@productionMonth INT,
	@productionDay INT
)
RETURNS DATETIME WITH SCHEMABINDING
AS
BEGIN

	-- Our constructed date
	DECLARE @date DATETIME
	
	-- Initialize to NULL
	SELECT @date = NULL
	
	-- Default day to 1 if not specified
	SELECT @productionDay = ISNULL(@productionDay, 1)
	
	-- Default production month to jan if not specified
	SELECT @productionMonth = ISNULL(@productionMonth, 1)

	-- We must have a year that is valid to get the production date
	-- as this forms the most broad part of the date (ie. without
	-- a year, the day and month values are meaningless)
	IF (
			@productionYear >= 1753
			AND
			@productionYear <= 9999
			AND
			@productionMonth > 0
			AND
			@productionMonth <= 12
		)
	BEGIN
		-- Date parts
		DECLARE @day INT
		DECLARE @month INT
		DECLARE @year INT

		-- Initialize date parts
		SELECT @day = @productionDay
		SELECT @month = @productionMonth
		SELECT @year = @productionYear

		-- Get the month name
		DECLARE @monthName VARCHAR(30)
		SELECT @monthName = DATENAME(MONTH, DATEADD(MONTH, @month, 0)-1)

		-- Construct the date
		SELECT @date = CONVERT(VARCHAR(2), @day) + ' ' + @monthName + ' ' + CONVERT(VARCHAR(4), @year)
	END

	RETURN @date

END
GO

ALTER TABLE [dbo].[AssetMetadataTextArea] ADD
CONSTRAINT [FK_AssetMetadataTextArea_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId])
GO

ALTER TABLE [dbo].[AssetMetadataTextField] ADD
CONSTRAINT [FK_AssetMetadataTextField_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId])
GO

ALTER TABLE [dbo].[BrandMetadataSelectableSetting] ADD
CONSTRAINT [FK_BrandMetadataSelectableSetting_BrandMetadataSetting] FOREIGN KEY ([BrandMetadataSettingId]) REFERENCES [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId])
GO

CREATE VIEW v_Asset WITH SCHEMABINDING AS
SELECT
	A.AssetId,
	A.AssetTypeId,
	AT.Name AS AssetTypeName,
	A.UploadDate,
	A.Filename,
	dbo.GetFileExtension(A.Filename) AS 'FileExtension',
	A.FileSize,
	A.FileHash,
	A.TemplateAssetId,
	A.WorkflowId,
	A.Title, 
	A.ProjectCode,
	A.Originator,
	A.Description,
	A.Keywords,
	A.ContactEmail,
	A.ProductionDay,
	A.ProductionMonth,
	A.ProductionYear,
	dbo.GetShadowDate(A.ProductionYear, A.ProductionMonth, A.ProductionDay) AS 'ShadowDate',
	A.BrandId,
	A.CopyrightOwner,
	A.UsageRestrictions,
	A.RestrictAttachedFiles,
	A.WatermarkPreview,
	A.InternalUsers_DownloadApprovalRequired,
	A.InternalUsers_HideFromUsers,
	A.ExternalUsers_DownloadApprovalRequired,
	A.ExternalUsers_HideFromUsers,
	A.AssetFilePathId,
	A.PublishDate,
	A.ExpiryDate,
	A.IsProcessed,
	A.AssetPublishStatusId,
	CASE A.AssetPublishStatusId WHEN 5 THEN 'Y' WHEN 2 THEN 'Pending' ELSE 'N' END AS AssetPublishStatusName,
	A.UploadedByUserId,
	U.FirstName + ' ' + U.LastName AS UploadedByUserName,
	A.CreateDate,
	A.LastUpdate,
	A.IsDeleted,
	A.AssetCategories,
	A.AssetMetadataVals,
	A.MetadataXml,
	A.MetadataSearchVals,
	A.DownloadCount,
	A.PopularityRank,
	A.Height,
	A.Width,
	A.Duration,
	A.Plugin,
	B.[Name]	AS 'BrandName',
	AT.Name + char(13) +
	A.Filename + char(13) +
	A.Title + char(13) +
	A.ProjectCode + char(13) +
	A.Originator + char(13) +
	A.Description + char(13) +
	A.Keywords + char(13) +
	A.ContactEmail + char(13) +
	A.AssetCategories + char(13) +
	A.AssetMetadataVals + char(13) +
	A.MetadataSearchVals + char(13) +
	ISNULL(B.Name, '') + char(13) +
	A.CopyrightOwner + char(13) +
	A.UsageRestrictions + char(13) +
	U.FirstName + char(13) +
	U.LastName
	AS 'SearchableData'
FROM
	dbo.Asset AS A
	INNER JOIN dbo.[AssetType] AT ON A.AssetTypeId = AT.AssetTypeId
	INNER JOIN dbo.[User] U ON A.UploadedByUserId = U.UserId
	INNER JOIN dbo.[Brand] B ON A.BrandId = B.BrandId
WHERE
  (A.IsDeleted = 0)
GO

CREATE UNIQUE CLUSTERED INDEX IDX_AssetId ON v_Asset(AssetId)
GO

EXEC sp_fulltext_table @tabname	= 'dbo.v_Asset', @action = 'create', @ftcat	= 'FocusOPENFullTextCatalog', @keyname = 'IDX_AssetId'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'AssetTypeName', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'CopyrightOwner', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'Description', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'Keywords', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'Filename', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'FileHash', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'Originator', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'ProjectCode', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'Title', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'UploadedByUserName', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'BrandName', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'AssetCategories', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'AssetMetadataVals', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'MetadataSearchVals', @action = 'add'
EXEC sp_fulltext_column @tabname = 'dbo.v_Asset', @colname = 'SearchableData', @action = 'add'
EXEC sp_fulltext_table 'dbo.v_Asset', 'start_full'
EXEC sp_fulltext_table 'dbo.v_Asset', 'start_change_tracking' 
EXEC sp_fulltext_table 'dbo.v_Asset', 'start_background_updateindex' 




BEGIN TRANSACTION
ALTER TABLE [dbo].[AssetTypeFileExtension] DROP CONSTRAINT [FK_AssetTypeFileExtension_AssetType]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Country]
ALTER TABLE [dbo].[AuditUserHistory] DROP CONSTRAINT [FK_AuditUserHistory_AuditUserAction]
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=1, [AssetDetailOrderNum]=1 WHERE [BrandMetadataSettingId]=1
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=2, [AssetDetailOrderNum]=2 WHERE [BrandMetadataSettingId]=2
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=3, [AssetDetailOrderNum]=3 WHERE [BrandMetadataSettingId]=3
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=4, [AssetDetailOrderNum]=4 WHERE [BrandMetadataSettingId]=4
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=5, [AssetDetailOrderNum]=5 WHERE [BrandMetadataSettingId]=5
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=6, [AssetDetailOrderNum]=6 WHERE [BrandMetadataSettingId]=6
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=7, [AssetDetailOrderNum]=7 WHERE [BrandMetadataSettingId]=7
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=8, [AssetDetailOrderNum]=8 WHERE [BrandMetadataSettingId]=8
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=9, [AssetDetailOrderNum]=9 WHERE [BrandMetadataSettingId]=9
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=10, [AssetDetailOrderNum]=10 WHERE [BrandMetadataSettingId]=10
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=11, [AssetDetailOrderNum]=11 WHERE [BrandMetadataSettingId]=11
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=12, [AssetDetailOrderNum]=12 WHERE [BrandMetadataSettingId]=12
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=13, [AssetDetailOrderNum]=13 WHERE [BrandMetadataSettingId]=13
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=14, [AssetDetailOrderNum]=14 WHERE [BrandMetadataSettingId]=14
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=15, [AssetDetailOrderNum]=15 WHERE [BrandMetadataSettingId]=15
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=16, [AssetDetailOrderNum]=16 WHERE [BrandMetadataSettingId]=16
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=17, [AssetDetailOrderNum]=17 WHERE [BrandMetadataSettingId]=17
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=18, [AssetDetailOrderNum]=18 WHERE [BrandMetadataSettingId]=18
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=19, [AssetDetailOrderNum]=19 WHERE [BrandMetadataSettingId]=19
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=20, [AssetDetailOrderNum]=20 WHERE [BrandMetadataSettingId]=20
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=21, [AssetDetailOrderNum]=21 WHERE [BrandMetadataSettingId]=21
UPDATE [dbo].[BrandMetadataSetting] SET [UiControlType]=3, [IsCustom]=1, [CatalogueOrderNum]=22, [AssetDetailOrderNum]=22 WHERE [BrandMetadataSettingId]=22
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=23, [AssetDetailOrderNum]=23 WHERE [BrandMetadataSettingId]=23
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=24, [AssetDetailOrderNum]=24 WHERE [BrandMetadataSettingId]=24
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=25, [AssetDetailOrderNum]=25 WHERE [BrandMetadataSettingId]=25
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=26, [AssetDetailOrderNum]=26 WHERE [BrandMetadataSettingId]=26
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=27, [AssetDetailOrderNum]=27 WHERE [BrandMetadataSettingId]=27
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=28, [AssetDetailOrderNum]=28 WHERE [BrandMetadataSettingId]=28
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=29, [AssetDetailOrderNum]=29 WHERE [BrandMetadataSettingId]=29
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=30, [AssetDetailOrderNum]=30 WHERE [BrandMetadataSettingId]=30
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=31, [AssetDetailOrderNum]=31 WHERE [BrandMetadataSettingId]=31
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=32, [AssetDetailOrderNum]=32 WHERE [BrandMetadataSettingId]=32
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=33, [AssetDetailOrderNum]=33 WHERE [BrandMetadataSettingId]=33
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=34, [AssetDetailOrderNum]=34 WHERE [BrandMetadataSettingId]=34
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=35, [AssetDetailOrderNum]=35 WHERE [BrandMetadataSettingId]=35
UPDATE [dbo].[BrandMetadataSetting] SET [CatalogueOrderNum]=36, [AssetDetailOrderNum]=36 WHERE [BrandMetadataSettingId]=36
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=1
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=2
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=3
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=4
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=6
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=7
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=8
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=9
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='69971c70-6415-46bf-aa76-dbadda5b4f06' WHERE [AssetTypeFileExtensionId]=10
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=11
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=12
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=13
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=14
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=15
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=17
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='81fb4b97-5b58-441a-89a3-df0708862dc7' WHERE [AssetTypeFileExtensionId]=18
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='69971c70-6415-46bf-aa76-dbadda5b4f06' WHERE [AssetTypeFileExtensionId]=19
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='198818c2-6012-486c-a5bd-e40a67dcfe71' WHERE [AssetTypeFileExtensionId]=20
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='6101ce70-f2ea-4a8b-ae6e-900f8795f248' WHERE [AssetTypeFileExtensionId]=26
UPDATE [dbo].[AssetTypeFileExtension] SET [Plugin]='6101ce70-f2ea-4a8b-ae6e-900f8795f248' WHERE [AssetTypeFileExtensionId]=27
UPDATE [dbo].[Country] SET [Name]='Great Britain' WHERE [CountryId]=74
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (63, 'API Call')
SET IDENTITY_INSERT [dbo].[Plugins] ON
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (1, '198818c2-6012-486c-a5bd-e40a67dcfe71', 'Images', 'Image Preview.xml', 'Image Preview', -1814607180, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (2, '6101ce70-f2ea-4a8b-ae6e-900f8795f248', 'MP3', 'MP3 Preview.xml', 'MP3 Preview', 509845421, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (3, '69971c70-6415-46bf-aa76-dbadda5b4f06', 'Flash', 'Flash Preview.xml', 'Flash Preview', 761263218, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (4, '81fb4b97-5b58-441a-89a3-df0708862dc7', 'FlowPlayer', 'FlowPlayer Preview.xml', 'Flowplayer Preview', -2004403416, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (5, '3445ec3c-3c66-4f05-8ddb-193cfd8078cf', 'FlowPlayer_Streaming', 'FlowPlayer_Streaming.xml', 'Flowplayer Streaming Preview', -559134806, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Plugins] OFF
ALTER TABLE [dbo].[AssetTypeFileExtension] ADD CONSTRAINT [FK_AssetTypeFileExtension_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [dbo].[AssetType] ([AssetTypeId])
ALTER TABLE [dbo].[User] WITH NOCHECK ADD CONSTRAINT [FK_User_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId])
ALTER TABLE [dbo].[AuditUserHistory] WITH NOCHECK ADD CONSTRAINT [FK_AuditUserHistory_AuditUserAction] FOREIGN KEY ([AuditUserActionId]) REFERENCES [dbo].[AuditUserAction] ([AuditUserActionId])
COMMIT TRANSACTION
GO