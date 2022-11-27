SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS ON
GO
IF EXISTS (SELECT * FROM tempdb..sysobjects WHERE id=OBJECT_ID('tempdb..#tmpErrors')) DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
PRINT N'Creating [dbo].[AuditAssetHistory]'
GO
CREATE TABLE [dbo].[AuditAssetHistory]
(
[AuditAssetHistoryId] [int] NOT NULL IDENTITY(1, 1),
[SessionId] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IpAddress] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuditAssetActionId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[AssetId] [int] NOT NULL,
[Notes] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Date] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditAssetView] on [dbo].[AuditAssetHistory]'
GO
ALTER TABLE [dbo].[AuditAssetHistory] ADD CONSTRAINT [PK_AuditAssetView] PRIMARY KEY CLUSTERED  ([AuditAssetHistoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditAssetAction]'
GO
CREATE TABLE [dbo].[AuditAssetAction]
(
[AuditAssetActionId] [int] NOT NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditAssetAction] on [dbo].[AuditAssetAction]'
GO
ALTER TABLE [dbo].[AuditAssetAction] ADD CONSTRAINT [PK_AuditAssetAction] PRIMARY KEY CLUSTERED  ([AuditAssetActionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditAssetSearchKeyword]'
GO
CREATE TABLE [dbo].[AuditAssetSearchKeyword]
(
[AuditAssetSearchKeywordId] [int] NOT NULL IDENTITY(1, 1),
[AuditAssetSearchId] [int] NOT NULL,
[SearchKeyword] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditAssetSearchKeyword] on [dbo].[AuditAssetSearchKeyword]'
GO
ALTER TABLE [dbo].[AuditAssetSearchKeyword] ADD CONSTRAINT [PK_AuditAssetSearchKeyword] PRIMARY KEY CLUSTERED  ([AuditAssetSearchKeywordId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditAssetSearch]'
GO
CREATE TABLE [dbo].[AuditAssetSearch]
(
[AuditAssetSearchId] [int] NOT NULL IDENTITY(1, 1),
[SessionId] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IpAddress] [varchar] (16) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UserId] [int] NOT NULL,
[Date] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditAssetSearch] on [dbo].[AuditAssetSearch]'
GO
ALTER TABLE [dbo].[AuditAssetSearch] ADD CONSTRAINT [PK_AuditAssetSearch] PRIMARY KEY CLUSTERED  ([AuditAssetSearchId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetCategory]'
GO
CREATE TABLE [dbo].[AssetCategory]
(
[AssetCategoryId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[CategoryId] [int] NOT NULL,
[IsPrimary] [bit] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetCategory] on [dbo].[AssetCategory]'
GO
ALTER TABLE [dbo].[AssetCategory] ADD CONSTRAINT [PK_AssetCategory] PRIMARY KEY CLUSTERED  ([AssetCategoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AssetCategory] on [dbo].[AssetCategory]'
GO
CREATE NONCLUSTERED INDEX [IX_AssetCategory] ON [dbo].[AssetCategory] ([AssetId], [CategoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GetShadowDate]'
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GetFileExtension]'
GO
CREATE FUNCTION [dbo].[GetFileExtension]
(
	@filename VARCHAR(300)
)
RETURNS VARCHAR(10) WITH SCHEMABINDING
AS
BEGIN
	DECLARE @Result VARCHAR(10)
	SELECT @Result = ''

	-- First lowercase the filename and remove spaces
	DECLARE @fn VARCHAR(255)
	SELECT @fn = LOWER(RTRIM(LTRIM(@filename)))
	
	-- Now reverse it.  This is so we only
	-- get everything after the last period
	DECLARE @rfn VARCHAR(255)
	SELECT @rfn = REVERSE(@fn)
	
	-- Get the index of the dot
	DECLARE @dotIndex INT
	SELECT @dotIndex = CHARINDEX('.', @rfn)
	
	IF (@dotIndex > 0)
	BEGIN
		-- Get everything up to the first period
		-- This will get the extension, as the
		-- filename has been reversed
		SELECT @Result = SUBSTRING(@rfn, 0, @dotIndex)
		
		-- Now we need to reverse it back, so that
		-- the extension is correct (as it's backwards)
		SELECT @Result = REVERSE(@Result)
	END

	RETURN @Result
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[LightboxAsset]'
GO
CREATE TABLE [dbo].[LightboxAsset]
(
[LightboxAssetId] [int] NOT NULL IDENTITY(1, 1),
[LightboxId] [int] NOT NULL,
[AssetId] [int] NOT NULL,
[Notes] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreateDate] [smalldatetime] NOT NULL,
[OrderNumber] [int] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_LightboxAsset] on [dbo].[LightboxAsset]'
GO
ALTER TABLE [dbo].[LightboxAsset] ADD CONSTRAINT [PK_LightboxAsset] PRIMARY KEY CLUSTERED  ([LightboxAssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_LightboxAsset] on [dbo].[LightboxAsset]'
GO
CREATE NONCLUSTERED INDEX [IX_LightboxAsset] ON [dbo].[LightboxAsset] ([LightboxId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Cart]'
GO
CREATE TABLE [dbo].[Cart]
(
[CartId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[AssetId] [int] NOT NULL,
[Notes] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequiredByDate] [smalldatetime] NULL,
[DateAdded] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Cart] on [dbo].[Cart]'
GO
ALTER TABLE [dbo].[Cart] ADD CONSTRAINT [PK_Cart] PRIMARY KEY CLUSTERED  ([CartId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetWorkflowCommenter]'
GO
CREATE TABLE [dbo].[AssetWorkflowCommenter]
(
[AssetWorkflowCommenterId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[InvitingAssetWorkflowUserId] [int] NOT NULL,
[InvitingUserMessage] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Comments] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CreateDate] [datetime] NOT NULL,
[LastUpdate] [datetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetWorkflowCommenter] on [dbo].[AssetWorkflowCommenter]'
GO
ALTER TABLE [dbo].[AssetWorkflowCommenter] ADD CONSTRAINT [PK_AssetWorkflowCommenter] PRIMARY KEY CLUSTERED  ([AssetWorkflowCommenterId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GenerateUserAPIToken]'
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Order]'
GO
CREATE TABLE [dbo].[Order]
(
[OrderId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[OrderDate] [smalldatetime] NOT NULL,
[CompletionDate] [smalldatetime] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Order] on [dbo].[Order]'
GO
ALTER TABLE [dbo].[Order] ADD CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED  ([OrderId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[OrderItem]'
GO
CREATE TABLE [dbo].[OrderItem]
(
[OrderItemId] [int] NOT NULL IDENTITY(1, 1),
[OrderId] [int] NOT NULL,
[AssetId] [int] NOT NULL,
[Notes] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RequiredByDate] [smalldatetime] NULL,
[OrderItemStatusId] [int] NOT NULL,
[OrderItemStatusDate] [smalldatetime] NULL,
[OrderItemStatusUserId] [int] NULL,
[AssignedToUserId] [int] NULL,
[CreateDate] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_OrderItem] on [dbo].[OrderItem]'
GO
ALTER TABLE [dbo].[OrderItem] ADD CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED  ([OrderItemId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[UserRole]'
GO
CREATE TABLE [dbo].[UserRole]
(
[UserRoleId] [int] NOT NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_UserType] on [dbo].[UserRole]'
GO
ALTER TABLE [dbo].[UserRole] ADD CONSTRAINT [PK_UserType] PRIMARY KEY CLUSTERED  ([UserRoleId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[UserStatus]'
GO
CREATE TABLE [dbo].[UserStatus]
(
[UserStatusId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_UserStatus] on [dbo].[UserStatus]'
GO
ALTER TABLE [dbo].[UserStatus] ADD CONSTRAINT [PK_UserStatus] PRIMARY KEY CLUSTERED  ([UserStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[IsExpired]'
GO
CREATE FUNCTION [dbo].[IsExpired]
(
	@expiryDate SMALLDATETIME,
	@isNonExpiring BIT,
	@now SMALLDATETIME = NULL
)
RETURNS BIT
AS
BEGIN

	IF (@now IS NULL)
		SELECT @now = GETDATE()

	DECLARE @returnVal BIT
	SELECT @returnVal = 1

	IF (@isNonExpiring = 1)
		SELECT @returnVal = 0
	
	ELSE IF (@expiryDate > @now)
		SELECT @returnVal = 0

	RETURN @returnVal
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetLink]'
GO
CREATE TABLE [dbo].[AssetLink]
(
[AssetLinkId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[LinkedAssetId] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetLink] on [dbo].[AssetLink]'
GO
ALTER TABLE [dbo].[AssetLink] ADD CONSTRAINT [PK_AssetLink] PRIMARY KEY CLUSTERED  ([AssetLinkId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditUserAction]'
GO
CREATE TABLE [dbo].[AuditUserAction]
(
[AuditUserActionId] [int] NOT NULL,
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditUserAction] on [dbo].[AuditUserAction]'
GO
ALTER TABLE [dbo].[AuditUserAction] ADD CONSTRAINT [PK_AuditUserAction] PRIMARY KEY CLUSTERED  ([AuditUserActionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AuditUserAction] on [dbo].[AuditUserAction]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AuditUserAction] ON [dbo].[AuditUserAction] ([AuditUserActionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetProcess]'
GO
CREATE TABLE [dbo].[AssetProcess]
(
[AssetProcessId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[UploadedByUserId] [int] NOT NULL,
[SendEmailOnCompletion] [bit] NOT NULL,
[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_AssetProcess_CreateDate] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetProcess] on [dbo].[AssetProcess]'
GO
ALTER TABLE [dbo].[AssetProcess] ADD CONSTRAINT [PK_AssetProcess] PRIMARY KEY CLUSTERED  ([AssetProcessId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[HomepageType]'
GO
CREATE TABLE [dbo].[HomepageType]
(
[HomepageTypeId] [int] NOT NULL,
[Description] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShortName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_HomepageType_TemplateFilename] DEFAULT ('')
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_HomepageType] on [dbo].[HomepageType]'
GO
ALTER TABLE [dbo].[HomepageType] ADD CONSTRAINT [PK_HomepageType] PRIMARY KEY CLUSTERED  ([HomepageTypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[LightboxSent]'
GO
CREATE TABLE [dbo].[LightboxSent]
(
[LightboxSentId] [int] NOT NULL IDENTITY(1, 1),
[LightboxId] [int] NOT NULL,
[CreatedLightboxId] [int] NULL,
[SenderId] [int] NOT NULL,
[RecipientEmail] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Subject] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Message] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[DateSent] [smalldatetime] NOT NULL,
[ExpiryDate] [smalldatetime] NULL,
[DownloadLinks] [bit] NOT NULL CONSTRAINT [DF_LightboxSent_DownloadLinks] DEFAULT ((0)),
[LightboxLinkedId] [int] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_LightboxSent] on [dbo].[LightboxSent]'
GO
ALTER TABLE [dbo].[LightboxSent] ADD CONSTRAINT [PK_LightboxSent] PRIMARY KEY CLUSTERED  ([LightboxSentId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Lightbox]'
GO
CREATE TABLE [dbo].[Lightbox]
(
[LightboxId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[Name] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Summary] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Notes] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsPublic] [bit] NOT NULL CONSTRAINT [DF_Lightbox_IsReadOnly] DEFAULT ((0)),
[IsDefault] [bit] NOT NULL,
[CreateDate] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Lightbox] on [dbo].[Lightbox]'
GO
ALTER TABLE [dbo].[Lightbox] ADD CONSTRAINT [PK_Lightbox] PRIMARY KEY CLUSTERED  ([LightboxId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Plugins]'
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Plugins] on [dbo].[Plugins]'
GO
ALTER TABLE [dbo].[Plugins] ADD CONSTRAINT [PK_Plugins] PRIMARY KEY CLUSTERED  ([PluginId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_Plugins] on [dbo].[Plugins]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Plugins] ON [dbo].[Plugins] ([RegistrationKey])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetFilePath]'
GO
CREATE TABLE [dbo].[AssetFilePath]
(
[AssetFilePathId] [int] NOT NULL IDENTITY(1, 1),
[Path] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsDefault] [bit] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetFilePath] on [dbo].[AssetFilePath]'
GO
ALTER TABLE [dbo].[AssetFilePath] ADD CONSTRAINT [PK_AssetFilePath] PRIMARY KEY CLUSTERED  ([AssetFilePathId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetPublishStatus]'
GO
CREATE TABLE [dbo].[AssetPublishStatus]
(
[AssetPublishStatusId] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetPublishStatus] on [dbo].[AssetPublishStatus]'
GO
ALTER TABLE [dbo].[AssetPublishStatus] ADD CONSTRAINT [PK_AssetPublishStatus] PRIMARY KEY CLUSTERED  ([AssetPublishStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetFileType]'
GO
CREATE TABLE [dbo].[AssetFileType]
(
[AssetFileTypeId] [int] NOT NULL,
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetFileType] on [dbo].[AssetFileType]'
GO
ALTER TABLE [dbo].[AssetFileType] ADD CONSTRAINT [PK_AssetFileType] PRIMARY KEY CLUSTERED  ([AssetFileTypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetMetadataTextArea]'
GO
CREATE TABLE [dbo].[AssetMetadataTextArea]
(
[AssetMetadataTextAreaId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[GroupNumber] [int] NOT NULL,
[TextAreaValue] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetMetadataTextArea] on [dbo].[AssetMetadataTextArea]'
GO
ALTER TABLE [dbo].[AssetMetadataTextArea] ADD CONSTRAINT [PK_AssetMetadataTextArea] PRIMARY KEY CLUSTERED  ([AssetMetadataTextAreaId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetMetadataTextField]'
GO
CREATE TABLE [dbo].[AssetMetadataTextField]
(
[AssetMetadataTextFieldId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[GroupNumber] [int] NOT NULL,
[TextFieldValue] [nvarchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetMetadataTextField] on [dbo].[AssetMetadataTextField]'
GO
ALTER TABLE [dbo].[AssetMetadataTextField] ADD CONSTRAINT [PK_AssetMetadataTextField] PRIMARY KEY CLUSTERED  ([AssetMetadataTextFieldId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetWorkflow]'
GO
CREATE TABLE [dbo].[AssetWorkflow]
(
[AssetWorkflowId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[SubmittedByUserId] [int] NOT NULL,
[CreateDate] [smalldatetime] NOT NULL,
[IsComplete] [bit] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetWorkflow_1] on [dbo].[AssetWorkflow]'
GO
ALTER TABLE [dbo].[AssetWorkflow] ADD CONSTRAINT [PK_AssetWorkflow_1] PRIMARY KEY CLUSTERED  ([AssetWorkflowId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetWorkflowUserStatus]'
GO
CREATE TABLE [dbo].[AssetWorkflowUserStatus]
(
[AssetWorkflowUserStatusId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetWorkflowUserStatus] on [dbo].[AssetWorkflowUserStatus]'
GO
ALTER TABLE [dbo].[AssetWorkflowUserStatus] ADD CONSTRAINT [PK_AssetWorkflowUserStatus] PRIMARY KEY CLUSTERED  ([AssetWorkflowUserStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[CompanyBrand]'
GO
CREATE TABLE [dbo].[CompanyBrand]
(
[CompanyBrandId] [int] NOT NULL IDENTITY(1, 1),
[CompanyId] [int] NOT NULL,
[BrandId] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_CompanyBrand] on [dbo].[CompanyBrand]'
GO
ALTER TABLE [dbo].[CompanyBrand] ADD CONSTRAINT [PK_CompanyBrand] PRIMARY KEY CLUSTERED  ([CompanyBrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[HomepageCategory]'
GO
CREATE TABLE [dbo].[HomepageCategory]
(
[HomepageCategoryId] [int] NOT NULL IDENTITY(1, 1),
[HomepageId] [int] NOT NULL,
[CategoryId] [int] NOT NULL,
[OrderBy] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_HomepageCategory] on [dbo].[HomepageCategory]'
GO
ALTER TABLE [dbo].[HomepageCategory] ADD CONSTRAINT [PK_HomepageCategory] PRIMARY KEY CLUSTERED  ([HomepageCategoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[LightboxBrand]'
GO
CREATE TABLE [dbo].[LightboxBrand]
(
[LightboxBrandId] [int] NOT NULL IDENTITY(1, 1),
[LightboxId] [int] NOT NULL,
[BrandId] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_LightboxBrand] on [dbo].[LightboxBrand]'
GO
ALTER TABLE [dbo].[LightboxBrand] ADD CONSTRAINT [PK_LightboxBrand] PRIMARY KEY CLUSTERED  ([LightboxBrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[LightboxLinked]'
GO
CREATE TABLE [dbo].[LightboxLinked]
(
[LightboxLinkedId] [int] NOT NULL IDENTITY(1, 1),
[LightboxId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[IsEditable] [bit] NULL,
[ExpiryDate] [datetime] NULL,
[Disabled] [bit] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_LightboxLinked] on [dbo].[LightboxLinked]'
GO
ALTER TABLE [dbo].[LightboxLinked] ADD CONSTRAINT [PK_LightboxLinked] PRIMARY KEY CLUSTERED  ([LightboxLinkedId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Country]'
GO
CREATE TABLE [dbo].[Country]
(
[CountryId] [int] NOT NULL IDENTITY(1, 1),
[Code] [char] (2) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Rank] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Country] on [dbo].[Country]'
GO
ALTER TABLE [dbo].[Country] ADD CONSTRAINT [PK_Country] PRIMARY KEY CLUSTERED  ([CountryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[UserBrand]'
GO
CREATE TABLE [dbo].[UserBrand]
(
[UserBrandId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[BrandId] [int] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_UserBrand] on [dbo].[UserBrand]'
GO
ALTER TABLE [dbo].[UserBrand] ADD CONSTRAINT [PK_UserBrand] PRIMARY KEY CLUSTERED  ([UserBrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[UserPasswordHistory]'
GO
CREATE TABLE [dbo].[UserPasswordHistory]
(
[UserPasswordHistoryId] [int] NOT NULL IDENTITY(1, 1),
[UserId] [int] NOT NULL,
[Password] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Date] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_UserPasswordHistory] on [dbo].[UserPasswordHistory]'
GO
ALTER TABLE [dbo].[UserPasswordHistory] ADD CONSTRAINT [PK_UserPasswordHistory] PRIMARY KEY CLUSTERED  ([UserPasswordHistoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Company]'
GO
CREATE TABLE [dbo].[Company]
(
[CompanyId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Brands] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Company_Brands] DEFAULT (''),
[Domain] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsInternal] [bit] NOT NULL CONSTRAINT [DF_Company_IsInternal] DEFAULT ((0)),
[CreatedByUserId] [int] NOT NULL,
[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Company_CreateDate] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Company] on [dbo].[Company]'
GO
ALTER TABLE [dbo].[Company] ADD CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED  ([CompanyId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Company]'
GO
CREATE VIEW dbo.v_Company
AS
SELECT     CompanyId, Name, Brands, Domain, IsInternal, CreatedByUserId, CreateDate
FROM      dbo.Company AS C
WHERE    (CompanyId > 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[WorkflowUser]'
GO
CREATE TABLE [dbo].[WorkflowUser]
(
[WorkflowUserId] [int] NOT NULL IDENTITY(1, 1),
[WorkflowId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[Position] [int] NOT NULL,
[DateAdded] [smalldatetime] NOT NULL CONSTRAINT [DF_WorkflowUser_DateAdded] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_WorkflowUser] on [dbo].[WorkflowUser]'
GO
ALTER TABLE [dbo].[WorkflowUser] ADD CONSTRAINT [PK_WorkflowUser] PRIMARY KEY CLUSTERED  ([WorkflowUserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[User]'
GO
CREATE TABLE [dbo].[User]
(
[UserId] [int] NOT NULL IDENTITY(1, 1),
[FirstName] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[LastName] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Email] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Password] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PasswordSalt] [char] (6) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_User_PasswordSalt] DEFAULT ('ABCDEF'),
[PasswordExpiryDate] [smalldatetime] NULL,
[IsPasswordNonExpiring] [bit] NOT NULL CONSTRAINT [DF_User_IsPasswordNonExpiring] DEFAULT ((0)),
[IsEmployee] [bit] NOT NULL CONSTRAINT [DF_User_IsEmployee] DEFAULT ((0)),
[IsEngineer] [bit] NOT NULL CONSTRAINT [DF_User_IsEngineer] DEFAULT ((0)),
[CompanyName] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[PrimaryBrandId] [int] NOT NULL CONSTRAINT [DF_User_PrimaryBrandId] DEFAULT ((1)),
[CountryId] [int] NULL,
[PhoneNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[MobileNumber] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RegisterDate] [smalldatetime] NOT NULL CONSTRAINT [DF_User_CreateDate] DEFAULT (getdate()),
[LastLoginDate] [smalldatetime] NULL,
[AccountExpiryDate] [smalldatetime] NULL,
[IsAccountNonExpiring] [bit] NOT NULL CONSTRAINT [DF_User_IsAccountNonExpiring] DEFAULT ((0)),
[IsSuspended] [bit] NOT NULL CONSTRAINT [DF_User_IsSuspended] DEFAULT ((0)),
[UserRoleId] [int] NOT NULL,
[UserStatusId] [int] NOT NULL,
[UseWorkflow] [bit] NOT NULL CONSTRAINT [DF_User_UseWorkflow] DEFAULT ((0)),
[WorkflowId] [int] NULL,
[Notes] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsAllowedExternalAccess] [bit] NOT NULL CONSTRAINT [DF_User_IsAllowedExternalAccess] DEFAULT ((1)),
[ApproverUserId] [int] NULL,
[CatalogueAssetIdList] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_User_IsDeleted] DEFAULT ((0)),
[Guid] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_User_Guid] DEFAULT (''),
[EnableFilePathIngestion] [bit] NOT NULL CONSTRAINT [DF_User_EnableFilePathIngestion] DEFAULT ((0)),
[SessionAPIToken] [varchar] (38) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[LastAPIOperation] [datetime] NULL,
[UserAPIToken] [varchar] (32) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF__User__UserAPITok__750E476F] DEFAULT ([dbo].[GenerateUserAPIToken]('default@user.com'))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_User] on [dbo].[User]'
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED  ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Asset]'
GO
CREATE TABLE [dbo].[Asset]
(
[AssetId] [int] NOT NULL IDENTITY(1, 1),
[AssetTypeId] [int] NOT NULL,
[UploadDate] [smalldatetime] NOT NULL,
[Filename] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FileSize] [bigint] NOT NULL,
[FileHash] [varchar] (200) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_FileHash] DEFAULT (''),
[TemplateAssetId] [int] NULL,
[WorkflowId] [int] NULL,
[Title] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_Title] DEFAULT (''),
[ProjectCode] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Originator] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Description] [varchar] (5000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Keywords] [varchar] (5000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_Keywords] DEFAULT (''),
[ContactEmail] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[ProductionDay] [int] NULL,
[ProductionMonth] [int] NULL,
[ProductionYear] [int] NULL,
[BrandId] [int] NOT NULL CONSTRAINT [DF_Asset_PrimaryBrandId] DEFAULT ((1)),
[CopyrightOwner] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[UsageRestrictions] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[RestrictAttachedFiles] [bit] NOT NULL CONSTRAINT [DF_Asset_RestrictAttachedFiles] DEFAULT ((0)),
[WatermarkPreview] [bit] NOT NULL CONSTRAINT [DF_Asset_WatermarkPreview] DEFAULT ((0)),
[InternalUsers_DownloadApprovalRequired] [bit] NOT NULL CONSTRAINT [DF_Asset_InternalUsers_DownloadApprovalRequired] DEFAULT ((0)),
[InternalUsers_HideFromUsers] [bit] NOT NULL CONSTRAINT [DF_Asset_InternalUsers_HideFromUsers] DEFAULT ((0)),
[ExternalUsers_DownloadApprovalRequired] [bit] NOT NULL CONSTRAINT [DF_Asset_ExternalUsers_DownloadApprovalRequired] DEFAULT ((0)),
[ExternalUsers_HideFromUsers] [bit] NOT NULL CONSTRAINT [DF_Asset_ExternalUsers_HideFromUsers] DEFAULT ((0)),
[AssetFilePathId] [int] NOT NULL,
[PublishDate] [smalldatetime] NOT NULL,
[ExpiryDate] [smalldatetime] NOT NULL,
[IsProcessed] [bit] NOT NULL,
[AssetPublishStatusId] [int] NOT NULL CONSTRAINT [DF_Asset_AssetPublishStatusUd] DEFAULT ((1)),
[UploadedByUserId] [int] NOT NULL,
[CreateDate] [smalldatetime] NOT NULL CONSTRAINT [DF_Asset_CreateDate] DEFAULT (getdate()),
[LastUpdate] [smalldatetime] NOT NULL CONSTRAINT [DF_Asset_LastUpdate] DEFAULT (getdate()),
[DownloadCount] [int] NOT NULL CONSTRAINT [DF_Asset_DownloadCount] DEFAULT ((0)),
[PopularityRank] [int] NOT NULL CONSTRAINT [DF_Asset_PopularityRank] DEFAULT ((0)),
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Asset_IsDeleted] DEFAULT ((0)),
[AssetCategories] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_AssetCategories] DEFAULT (''),
[AssetMetadataVals] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_AssetMetadata] DEFAULT (''),
[MetadataXml] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Height] [int] NULL,
[Width] [int] NULL,
[Duration] [int] NULL,
[MetadataSearchVals] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Asset_MetadataSearchVals] DEFAULT (''),
[Plugin] [uniqueidentifier] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Asset] on [dbo].[Asset]'
GO
ALTER TABLE [dbo].[Asset] ADD CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED  ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AuditAssetHistory]'
GO
CREATE VIEW dbo.v_AuditAssetHistory
AS
SELECT     AAH.AuditAssetHistoryId, AAH.SessionId, AAH.IpAddress, AAH.AuditAssetActionId, AAA.Description, AAH.UserId, U.PrimaryBrandId AS UserPrimaryBrandId, 
                  A.BrandId AS AssetBrandId, AAH.AssetId, AAH.Notes, AAH.Date, U.FirstName + ' ' + U.LastName AS UserName, U.Email AS UserEmail, A.Title
FROM      dbo.AuditAssetAction AS AAA INNER JOIN
                  dbo.AuditAssetHistory AS AAH ON AAA.AuditAssetActionId = AAH.AuditAssetActionId INNER JOIN
                  dbo.[User] AS U ON AAH.UserId = U.UserId INNER JOIN
                  dbo.Asset AS A ON AAH.AssetId = A.AssetId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditAssetSearchResult]'
GO
CREATE TABLE [dbo].[AuditAssetSearchResult]
(
[AuditAssetSearchResultId] [int] NOT NULL IDENTITY(1, 1),
[AuditAssetSearchId] [int] NOT NULL,
[AssetId] [int] NOT NULL,
[Date] [smalldatetime] NOT NULL CONSTRAINT [DF_AuditAssetSearchResult_Date] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditAssetSearchResult] on [dbo].[AuditAssetSearchResult]'
GO
ALTER TABLE [dbo].[AuditAssetSearchResult] ADD CONSTRAINT [PK_AuditAssetSearchResult] PRIMARY KEY CLUSTERED  ([AuditAssetSearchResultId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetPopularityRank]'
GO
CREATE VIEW dbo.v_AssetPopularityRank
AS
SELECT     AssetId, UploadDate, DownloadCount, PopularityRank, DENSE_RANK() OVER (ORDER BY DownloadCount DESC, UploadDate DESC) 
AS 'ActualPopularityRank'
FROM         dbo.Asset A





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Brand]'
GO
CREATE TABLE [dbo].[Brand]
(
[BrandId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ShortName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_ShortName] DEFAULT (''),
[ApplicationName] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_ApplicationName] DEFAULT (''),
[OrganisationName] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_OrganisationName] DEFAULT (''),
[IsMasterBrand] [bit] NOT NULL CONSTRAINT [DF_Brand_IsMasterBrand] DEFAULT ((0)),
[WebsiteUrl] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_WebsiteUrl] DEFAULT (''),
[EmailFrom] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_EmailFrom] DEFAULT (''),
[IsBrandSelectionAllowed] [bit] NOT NULL CONSTRAINT [DF_Brand_IsBrandSelectionAllowed] DEFAULT ((1)),
[DisablePoweredByLogo] [bit] NOT NULL CONSTRAINT [DF_Brand_DisablePoweredByLogo] DEFAULT ((0)),
[LoginPageUpperCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_LoginPageUpperCopy] DEFAULT (''),
[LoginPageLowerCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_LoginPageLowerCopy] DEFAULT (''),
[DefaultUsageRestrictionsCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_DefaultUsageRestrictionsCopy] DEFAULT (''),
[MyAccountCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_MyAccountCopy] DEFAULT (''),
[AdminCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_AdminCopy] DEFAULT (''),
[TermsConditionsCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_TermsConditionsCopy] DEFAULT (''),
[PrivacyPolicyCopy] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_PrivacyPolicyCopy] DEFAULT (''),
[HideFilterSearch] [bit] NOT NULL CONSTRAINT [DF_Brand_HideFilterSearch] DEFAULT ((0)),
[HideCategorySearch] [bit] NOT NULL CONSTRAINT [DF_Brand_HideCategorySearch] DEFAULT ((0)),
[DirectDownloadEnabled] [bit] NOT NULL CONSTRAINT [DF_Brand_DirectDownloadEnabled] DEFAULT ((0)),
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Brand_IsDeleted] DEFAULT ((0)),
[FilterMarkup] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Brand_FilterMarkup] DEFAULT ('')
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Brand] on [dbo].[Brand]'
GO
ALTER TABLE [dbo].[Brand] ADD CONSTRAINT [PK_Brand] PRIMARY KEY CLUSTERED  ([BrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetType]'
GO
CREATE TABLE [dbo].[AssetType]
(
[AssetTypeId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (30) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsVisible] [bit] NOT NULL CONSTRAINT [DF_AssetType_IsVisible] DEFAULT ((1)),
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_AssetType_IsDeleted] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetType] on [dbo].[AssetType]'
GO
ALTER TABLE [dbo].[AssetType] ADD CONSTRAINT [PK_AssetType] PRIMARY KEY CLUSTERED  ([AssetTypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Homepage]'
GO
CREATE TABLE [dbo].[Homepage]
(
[HomepageId] [int] NOT NULL IDENTITY(1, 1),
[BrandId] [int] NOT NULL CONSTRAINT [DF_Homepage_BrandId] DEFAULT ((1)),
[IntroText] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_IntroText] DEFAULT (''),
[Url1] [varchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_Url1] DEFAULT (''),
[Url2] [varchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_Url2] DEFAULT (''),
[Url3] [varchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_Url3] DEFAULT (''),
[Url4] [varchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_Url4] DEFAULT (''),
[BumperPageHtml] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_BumperPageHtml] DEFAULT (''),
[BumperPageSkip] [bit] NOT NULL CONSTRAINT [DF_Homepage_BumperPageSkip] DEFAULT ((0)),
[CustomHtml] [nvarchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Homepage_CustomHtml] DEFAULT (''),
[HomepageTypeId] [int] NOT NULL CONSTRAINT [DF_Homepage_HomepageTypeId] DEFAULT ((1)),
[IsPublished] [bit] NOT NULL CONSTRAINT [DF_Homepage_IsPublished] DEFAULT ((1)),
[LastModifiedByUserId] [int] NOT NULL,
[LastModifiedDate] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Homepage] on [dbo].[Homepage]'
GO
ALTER TABLE [dbo].[Homepage] ADD CONSTRAINT [PK_Homepage] PRIMARY KEY CLUSTERED  ([HomepageId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_BrandId] on [dbo].[Homepage]'
GO
CREATE NONCLUSTERED INDEX [IX_BrandId] ON [dbo].[Homepage] ([BrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Homepage]'
GO
CREATE VIEW dbo.v_Homepage
AS
SELECT     H.HomepageId, H.BrandId, H.IntroText, H.Url1, H.Url2, H.Url3, H.Url4, H.BumperPageHtml, H.BumperPageSkip, H.CustomHtml, H.HomepageTypeId, H.IsPublished, 
                  H.LastModifiedByUserId, H.LastModifiedDate, B.Name AS BrandName
FROM      dbo.Brand AS B INNER JOIN
                  dbo.Homepage AS H ON B.BrandId = H.BrandId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetFile]'
GO
CREATE TABLE [dbo].[AssetFile]
(
[AssetFileId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[FileContent] [image] NOT NULL,
[Filename] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AssetFile_FileName] DEFAULT (''),
[FileExtension] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AssetFileTypeId] [int] NOT NULL CONSTRAINT [DF_AssetFile_AssetFileTypeId] DEFAULT ((1)),
[LastUpdate] [smalldatetime] NOT NULL CONSTRAINT [DF_AssetFile_LastUpdate] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetFileId] on [dbo].[AssetFile]'
GO
ALTER TABLE [dbo].[AssetFile] ADD CONSTRAINT [PK_AssetFileId] PRIMARY KEY CLUSTERED  ([AssetFileId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AssetId] on [dbo].[AssetFile]'
GO
CREATE NONCLUSTERED INDEX [IX_AssetId] ON [dbo].[AssetFile] ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_AddUpdateAssetFile]'
GO
CREATE PROCEDURE [dbo].[usp_AddUpdateAssetFile]
    @AssetId INT,
    @FileContent IMAGE,
    @FileName VARCHAR(300),
    @FileExtension VARCHAR(5),
    @AssetFileTypeId INT,
    @ForceAdd BIT = 0
AS 
    BEGIN
        SET NOCOUNT ON ;
		
        -- Initialize the AssetFileId to be updated to NULL
        DECLARE @assetFileId INT
        SELECT @assetFileId = NULL
        
        -- Only set the AssetFileId if we don't want to force the file to be added
        IF (@ForceAdd = 0)
        BEGIN
	        SELECT  @assetFileId = AssetFileId
		    FROM    AssetFile
			WHERE   ( AssetId = @assetId AND AssetFileTypeId = @AssetFileTypeId )
        END
		
        IF ( @assetFileId IS NULL ) 
            BEGIN
                INSERT  INTO AssetFile
                        (
                          [AssetId],
                          [FileContent],
                          [FileName],
                          [FileExtension],
                          [AssetFileTypeId],
                          [LastUpdate]
                        )
                VALUES  (
                          @AssetId,
                          @FileContent,
                          @FileName,
                          @FileExtension,
                          @AssetFileTypeId,
                          GETDATE()
                        )
            END
        ELSE 
            BEGIN
                UPDATE  AssetFile
                SET     [FileContent] = @FileContent,
						[FileName] = @FileName,
                        [FileExtension] = @FileExtension,
                        [AssetFileTypeId] = @AssetFileTypeId,
                        [LastUpdate] = GETDATE()
                WHERE   ( [AssetFileId] = @assetFileId )
            END
    END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetAssetReportSummary]'
GO
CREATE PROCEDURE [dbo].[usp_GetAssetReportSummary] 
	@brandId	int				= NULL,
	@startDate	smalldatetime	= NULL,
	@endDate	smalldatetime	= NULL
AS
BEGIN
	SET NOCOUNT ON;

	-----------------------------------------------------------------------------
	-- Initialise dates to default values
	-----------------------------------------------------------------------------
	SELECT @startDate = ISNULL(@startDate, '01 January 1900');
	SELECT @endDate = ISNULL(@endDate, '06 June 2079');

	-----------------------------------------------------------------------------
	-- Get AuditAssetAction ID's
	-----------------------------------------------------------------------------
	DECLARE @uploadsAuditAssetAction INT
	SELECT @uploadsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Uploaded Asset')

	DECLARE @downloadsAuditAssetAction INT
	SELECT @downloadsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Downloaded Asset File')

	DECLARE @orderedAuditAssetAction INT
	SELECT @orderedAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Ordered')

	DECLARE @viewsAuditAssetAction INT
	SELECT @viewsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Viewed Asset Detail')

	DECLARE @searcHitAssetActionId INT
	SELECT @searcHitAssetActionId = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Returned In Search')

	DECLARE @addedToCartAuditAssetAction INT
	SELECT @addedToCartAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Added To Cart')

	DECLARE @addedToLightboxAuditAssetAction INT
	SELECT @addedToLightboxAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Added To Lightbox')

	-----------------------------------------------------------------------------
	-- Declare variables for return values
	-----------------------------------------------------------------------------

	DECLARE @uploads INT
	DECLARE @totalUploads INT
	DECLARE @downloads INT
	DECLARE @totalDownloads INT
	DECLARE @orderRequests INT
	DECLARE @totalOrderRequests INT
	DECLARE @views INT
	DECLARE @totalViews INT
	DECLARE @searchHits INT
	DECLARE @totalSearchHits INT
	DECLARE @cart INT
	DECLARE @totalCart INT
	DECLARE @lightbox INT
	DECLARE @totalLightbox INT

	-----------------------------------------------------------------------------
	-- Get asset id's matching our critera into a temp table for easy comparison later
	-----------------------------------------------------------------------------
	DECLARE @assetIdList TABLE(AssetId INT)
	INSERT INTO @assetIdList
		SELECT
			AssetId
		FROM
			Asset A
		WHERE
			A.BrandId = ISNULL(@brandId, A.BrandId)

	-----------------------------------------------------------------------------
	-- Populate variables
	-----------------------------------------------------------------------------

	SELECT @uploads				= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @uploadsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalUploads		= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @uploadsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	SELECT @downloads			= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @downloadsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalDownloads		= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @downloadsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	SELECT @orderRequests		= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @orderedAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalOrderRequests	= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @orderedAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	SELECT @views				= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @viewsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalViews			= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @viewsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	SELECT @searchHits			= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @searcHitAssetActionId) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalSearchHits		= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @searcHitAssetActionId) AND (Date BETWEEN @startDate AND @endDate)

	SELECT @cart				= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToCartAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalCart			= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToCartAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)
	
	SELECT @lightbox			= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToLightboxAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate) AND AssetId IN (SELECT AssetId FROM @assetIdList)
	SELECT @totalLightbox		= COUNT(DISTINCT AssetId) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToLightboxAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	-----------------------------------------------------------------------------
	-- Select and return values
	-----------------------------------------------------------------------------
	SELECT	@uploads			AS Uploads,
			@totalUploads		AS TotalUploads,
			@downloads			AS Downloads,
			@totalDownloads		AS TotalDownloads,
			@orderRequests		AS OrderRequests,
			@totalOrderRequests	AS TotalOrderRequests,
			@views				AS [Views],
			@totalViews			AS TotalViews,
			@searchHits			AS SearchHits,
			@totalSearchHits	AS TotalSearchHits,
			@cart				AS AddedToCart,
			@totalCart			AS TotalAddedToCart,
			@lightbox			AS AddedToLightbox,
			@totalLightbox		AS TotalAddedToLightbox
END





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_LightboxAsset]'
GO
CREATE VIEW dbo.v_LightboxAsset
AS
SELECT     LA.LightboxAssetId, LA.LightboxId, LA.AssetId, LA.Notes, LA.CreateDate, LA.OrderNumber
FROM      dbo.LightboxAsset AS LA INNER JOIN
                  dbo.Asset AS A ON LA.AssetId = A.AssetId
WHERE    (A.IsDeleted = 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetTypeFileExtension]'
GO
CREATE TABLE [dbo].[AssetTypeFileExtension]
(
[AssetTypeFileExtensionId] [int] NOT NULL IDENTITY(1, 1),
[Extension] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AssetTypeId] [int] NOT NULL,
[IconImage] [image] NULL,
[IconFilename] [varchar] (300) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AssetTypeFileExtension_IconFilename] DEFAULT (''),
[IsVisible] [bit] NOT NULL CONSTRAINT [DF_AssetTypeFileExtension_IsVisible] DEFAULT ((0)),
[Plugin] [uniqueidentifier] NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetTypeFileExtensionId] on [dbo].[AssetTypeFileExtension]'
GO
ALTER TABLE [dbo].[AssetTypeFileExtension] ADD CONSTRAINT [PK_AssetTypeFileExtensionId] PRIMARY KEY CLUSTERED  ([AssetTypeFileExtensionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AssetTypeFileExtension] on [dbo].[AssetTypeFileExtension]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AssetTypeFileExtension] ON [dbo].[AssetTypeFileExtension] ([Extension])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetTypeFileExtension]'
GO


CREATE VIEW [dbo].[v_AssetTypeFileExtension]
AS
SELECT     ATFE.AssetTypeFileExtensionId, ATFE.Extension, ATFE.Name, ATFE.AssetTypeId, ATFE.IconImage, ATFE.IconFilename, ATFE.IsVisible, AT.Name AS AssetTypeName, ATFE.Plugin
FROM      dbo.AssetType AS AT RIGHT OUTER JOIN
                  dbo.AssetTypeFileExtension AS ATFE ON AT.AssetTypeId = ATFE.AssetTypeId

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Cart]'
GO
CREATE VIEW dbo.v_Cart
AS
SELECT     TOP (100) PERCENT C.CartId, C.UserId, C.AssetId, C.RequiredByDate, C.Notes, C.DateAdded
FROM      dbo.Cart AS C INNER JOIN
                  dbo.Asset AS A ON C.AssetId = A.AssetId
WHERE    (A.IsDeleted = 0)
ORDER BY C.UserId, A.AssetTypeId DESC
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetWorkflowUser]'
GO
CREATE TABLE [dbo].[AssetWorkflowUser]
(
[AssetWorkflowUserId] [int] NOT NULL IDENTITY(1, 1),
[AssetWorkflowId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[Position] [int] NOT NULL,
[Comments] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AssetWorkflowUserStatusId] [int] NOT NULL,
[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_AssetWorkflow_CreateDate] DEFAULT (getdate()),
[LastUpdate] [datetime] NOT NULL CONSTRAINT [DF_Table_1_LastModifiedDate] DEFAULT (getdate()),
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_AssetWorkflow_Deleted] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetWorkflow] on [dbo].[AssetWorkflowUser]'
GO
ALTER TABLE [dbo].[AssetWorkflowUser] ADD CONSTRAINT [PK_AssetWorkflow] PRIMARY KEY CLUSTERED  ([AssetWorkflowUserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetWorkflowCommenter]'
GO
CREATE VIEW dbo.v_AssetWorkflowCommenter
AS
SELECT     AWFC.AssetWorkflowCommenterId, AWFC.UserId, AWFU.UserId AS InvitingUserId, AWFU.AssetWorkflowId, AWFC.InvitingAssetWorkflowUserId, 
                      AWFC.InvitingUserMessage, AWFC.Comments, AWFC.CreateDate, AWFC.LastUpdate
FROM         dbo.AssetWorkflowCommenter AS AWFC INNER JOIN
                      dbo.AssetWorkflowUser AS AWFU ON AWFC.InvitingAssetWorkflowUserId = AWFU.AssetWorkflowUserId





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Category]'
GO
CREATE TABLE [dbo].[Category]
(
[CategoryId] [int] NOT NULL IDENTITY(1, 1),
[ParentCategoryId] [int] NULL,
[BrandId] [int] NOT NULL CONSTRAINT [DF_Category_BrandId] DEFAULT ((1)),
[Name] [varchar] (150) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExternalRef] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Category_ExternalRef] DEFAULT (''),
[Message] [varchar] (max) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[Synonyms] [varchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
[OwnerUserId] [int] NOT NULL,
[CategoryOrder] [int] NOT NULL,
[FullAssetCount] [int] NOT NULL CONSTRAINT [DF_Category_FullAssetCount] DEFAULT ((0)),
[AvailableAssetCount] [int] NOT NULL CONSTRAINT [DF_Category_AvailableAssetCount] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Category] on [dbo].[Category]'
GO
ALTER TABLE [dbo].[Category] ADD CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED  ([CategoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Brand]'
GO

CREATE VIEW [dbo].[v_Brand]
AS
SELECT     BrandId, Name, ShortName, ApplicationName, OrganisationName, IsMasterBrand, WebsiteUrl, EmailFrom, IsBrandSelectionAllowed, LoginPageUpperCopy, LoginPageLowerCopy, 
                  DisablePoweredByLogo, DefaultUsageRestrictionsCopy, MyAccountCopy, AdminCopy, TermsConditionsCopy, PrivacyPolicyCopy, HideFilterSearch, HideCategorySearch, 
                  DirectDownloadEnabled, IsDeleted, FilterMarkup
FROM      dbo.Brand AS B
WHERE    (IsDeleted = 0)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AuditUserHistory]'
GO
CREATE TABLE [dbo].[AuditUserHistory]
(
[AuditUserHistoryId] [int] NOT NULL IDENTITY(1, 1),
[SessionId] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IpAddress] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[AuditUserActionId] [int] NOT NULL CONSTRAINT [DF_AuditUserHistory_AuditUserActionId] DEFAULT ((1)),
[UserId] [int] NOT NULL,
[Notes] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Date] [smalldatetime] NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AuditLogEntry] on [dbo].[AuditUserHistory]'
GO
ALTER TABLE [dbo].[AuditUserHistory] ADD CONSTRAINT [PK_AuditLogEntry] PRIMARY KEY CLUSTERED  ([AuditUserHistoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetBadLoginInfo]'
GO
CREATE PROCEDURE dbo.usp_GetBadLoginInfo 
	@UserId INT,
	@BadLoginDateCutOff DATETIME
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @maxAuditUserHistoryId INT
	DECLARE @badLoginCount INT
	DECLARE @mostRecentBadLogin DATETIME
	
	SELECT @mostRecentBadLogin = NULL
	
	SELECT @maxAuditUserHistoryId = MAX([AuditUserHistoryId]) FROM [AuditUserHistory] WHERE ([AuditUserActionId] IN (32, 33)) AND (LOWER([Notes]) LIKE 'login successful%') AND (UserId = @UserId)
	SELECT @maxAuditUserHistoryId = ISNULL(@maxAuditUserHistoryId, 0)
	
	SELECT @badLoginCount = COUNT(AuditUserHistoryId) FROM [AuditUserHistory] WHERE ([AuditUserActionId] IN (32, 33)) AND (LOWER([Notes]) LIKE 'login failed%') AND (UserId = @UserId) AND ([Date] > @BadLoginDateCutOff) AND ([AuditUserHistoryId] > @maxAuditUserHistoryId)

	IF (@badLoginCount > 0)
		SELECT @mostRecentBadLogin = MAX([Date]) FROM [AuditUserHistory] WHERE ([AuditUserActionId] IN (32, 33)) AND (LOWER([Notes]) LIKE 'login failed%') AND (UserId = @UserId) AND ([AuditUserHistoryId] > @maxAuditUserHistoryId)
	
	SELECT @badLoginCount AS 'BadLoginCount', @mostRecentBadLogin AS 'LastBadLoginDate'
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetLastDownloadUsers]'
GO
CREATE PROCEDURE [dbo].[usp_GetLastDownloadUsers]
	@assetId INT,
	@maxRecords INT = 10
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @downloadsAuditAssetAction INT
	SELECT @downloadsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Downloaded Asset File')

	SELECT TOP(@maxRecords) ROW_NUMBER() OVER (ORDER BY [Date] DESC) AS 'Rank', U.Email, AAH.Date FROM AuditAssetHistory AAH INNER JOIN [User] U ON AAH.UserId = U.UserId WHERE (AAH.AuditAssetActionId = @downloadsAuditAssetAction) AND (AssetId = @assetId) ORDER BY [Date] DESC

END





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_OrderItem]'
GO
CREATE VIEW dbo.v_OrderItem
AS
SELECT     OI.OrderItemId, OI.OrderId, OI.AssetId, O.UserId, OI.Notes, OI.RequiredByDate, OI.OrderItemStatusId, OI.OrderItemStatusDate, A.UploadedByUserId AS AssetUploadUserId, 
                  A.BrandId AS AssetBrandId, OI.OrderItemStatusUserId, OI.AssignedToUserId, OI.CreateDate
FROM      dbo.Asset AS A INNER JOIN
                  dbo.OrderItem AS OI ON A.AssetId = OI.AssetId INNER JOIN
                  dbo.[Order] AS O ON OI.OrderId = O.OrderId
WHERE    (A.IsDeleted = 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_User]'
GO


CREATE VIEW [dbo].[v_User] AS
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Order]'
GO
CREATE VIEW dbo.v_Order
AS
SELECT     O.OrderId, O.UserId, U.FullName AS UserName, U.Email AS UserEmail, U.PrimaryBrandId AS UserPrimaryBrandId, U.BrandName AS UserPrimaryBrandName, O.OrderDate, 
                  O.CompletionDate,
                      (SELECT     COUNT(OrderItemId) AS Expr1
                       FROM       dbo.OrderItem AS OI
                       WHERE    (OrderId = O.OrderId)) AS AssetCount, CAST(CASE WHEN CompletionDate IS NULL THEN 1 ELSE 0 END AS BIT) AS IsPending,
                      (SELECT     MIN(RequiredByDate) AS Expr1
                       FROM       dbo.OrderItem AS OI
                       WHERE    (OrderId = O.OrderId)) AS DeadlineDate
FROM      dbo.[Order] AS O INNER JOIN
                  dbo.v_User AS U ON O.UserId = U.UserId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetAssetStats]'
GO
CREATE PROCEDURE [dbo].[usp_GetAssetStats] 
	@assetId int,
	@startDate smalldatetime = NULL,
	@endDate smalldatetime = NULL
AS
BEGIN
	SET NOCOUNT ON;

	SELECT @startDate = ISNULL(@startDate, '01 January 1900')
	SELECT @endDate = ISNULL(@endDate, '06 June 2079')

	DECLARE @downloadsAuditAssetAction INT
	SELECT @downloadsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Downloaded Asset File')

	DECLARE @viewsAuditAssetAction INT
	SELECT @viewsAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Viewed Asset Detail')

	DECLARE @addedToCartAuditAssetAction INT
	SELECT @addedToCartAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Added To Cart')

	DECLARE @addedToLightboxAuditAssetAction INT
	SELECT @addedToLightboxAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Added To Lightbox')

	DECLARE @totalDownloads INT
	SELECT @totalDownloads = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @downloadsAuditAssetAction) AND (Date BETWEEN @startDate AND @endDate)

	DECLARE @assetDownloads INT
	SELECT @assetDownloads = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @downloadsAuditAssetAction) AND (AssetId = @assetId) AND (Date BETWEEN @startDate AND @endDate)

	DECLARE @totalOrderRequests INT
	SELECT @totalOrderRequests = COUNT(*) FROM OrderItem WHERE (CreateDate BETWEEN @startDate AND @endDate)

	DECLARE @assetOrderRequests INT
	SELECT @assetOrderRequests = COUNT(*) FROM OrderItem WHERE (AssetId = @assetId) AND (CreateDate BETWEEN @startDate AND @endDate)

	DECLARE @totalViews INT
	SELECT @totalViews = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @viewsAuditAssetAction) AND Date BETWEEN @startDate AND @endDate

	DECLARE @assetViews INT
	SELECT @assetViews = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @viewsAuditAssetAction) AND (AssetId = @assetId) AND (Date BETWEEN @startDate AND @endDate)

	DECLARE @totalSearchHits INT
	SELECT @totalSearchHits = COUNT(*) FROM AuditAssetSearchResult WHERE (Date BETWEEN @startDate AND @endDate)

	DECLARE @assetSearchHits INT
	SELECT @assetSearchHits = COUNT(*) FROM AuditAssetSearchResult WHERE (AssetId = @assetId) AND (Date BETWEEN @startDate AND @endDate)

	DECLARE @totalAddedToCart INT
	SELECT @totalAddedToCart = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToCartAuditAssetAction) AND Date BETWEEN @startDate AND @endDate

	DECLARE @assetAddedToCart INT
	SELECT @assetAddedToCart = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToCartAuditAssetAction) AND (AssetId = @assetId) AND (Date BETWEEN @startDate AND @endDate)

	DECLARE @totalAddedToLightbox INT
	SELECT @totalAddedToLightbox = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToLightboxAuditAssetAction) AND Date BETWEEN @startDate AND @endDate

	DECLARE @assetAddedToLightbox INT
	SELECT @assetAddedToLightbox = COUNT(*) FROM AuditAssetHistory WHERE (AuditAssetActionId = @addedToLightboxAuditAssetAction) AND (AssetId = @assetId) AND (Date BETWEEN @startDate AND @endDate)

	SELECT	@assetId				AS 'AssetId',
			@startDate				AS 'StartDate',
			@endDate				AS 'EndDate',
			@totalDownloads			AS 'TotalDownloads',
			@assetDownloads			AS 'AssetDownloads',
			@totalOrderRequests		AS 'TotalOrderRequests',
			@assetOrderRequests		AS 'AssetOrderRequests',
			@totalViews				AS 'TotalViews',
			@assetViews				AS 'AssetViews',
			@totalSearchHits		AS 'TotalSearchHits',
			@assetSearchHits		AS 'AssetSearchHits',
			@totalAddedToCart		AS 'TotalAddedToCart',
			@assetAddedToCart		AS 'AssetAddedToCart',
			@totalAddedToLightbox	AS 'TotalAddedToLightbox',
			@assetAddedToLightbox	AS 'AssetAddedToLightbox'

END





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Workflow]'
GO
CREATE TABLE [dbo].[Workflow]
(
[WorkflowId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[BrandId] [int] NOT NULL CONSTRAINT [DF_Workflow_BrandId] DEFAULT ((1)),
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Workflow_IsDeleted] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Workflow] on [dbo].[Workflow]'
GO
ALTER TABLE [dbo].[Workflow] ADD CONSTRAINT [PK_Workflow] PRIMARY KEY CLUSTERED  ([WorkflowId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetOrderHistory]'
GO


CREATE VIEW [dbo].[v_AssetOrderHistory]
AS
SELECT oi.OrderItemId As AssetOrderHistoryId, oi.AssetId, 
	oh.OrderId, oh.OrderDate, oh.DeadlineDate, oh.UserEmail, oh.UserId, oh.UserName,
	oi.Notes,
	oi.OrderItemStatusDate,	oi.OrderItemStatusId
FROM dbo.[OrderItem] as oi
LEFT JOIN dbo.[v_Order] as oh on oh.[OrderId] = oi.[OrderId]


GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetCategory]'
GO
CREATE VIEW dbo.v_AssetCategory
AS
SELECT     [AS].AssetCategoryId, [AS].AssetId, [AS].CategoryId, [AS].IsPrimary, C.Name, C.ParentCategoryId, C.Synonyms, C.CategoryOrder, 
                  C.Name + ' ' + C.Synonyms AS NameAndSynonyms
FROM      dbo.AssetCategory AS [AS] INNER JOIN
                  dbo.Category AS C ON [AS].CategoryId = C.CategoryId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AuditUserHistory]'
GO
CREATE VIEW dbo.v_AuditUserHistory
AS
SELECT     AUH.AuditUserHistoryId, AUH.SessionId, AUH.AuditUserActionId, AUA.Description, AUH.UserId, U.FirstName + ' ' + U.LastName AS UserName, U.Email AS UserEmail, 
                  AUH.IpAddress, AUH.Notes, AUH.Date, U.PrimaryBrandId
FROM      dbo.AuditUserAction AS AUA INNER JOIN
                  dbo.AuditUserHistory AS AUH ON AUA.AuditUserActionId = AUH.AuditUserActionId INNER JOIN
                  dbo.[User] AS U ON AUH.UserId = U.UserId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[OrderItemComment]'
GO
CREATE TABLE [dbo].[OrderItemComment]
(
[OrderItemCommentId] [int] NOT NULL IDENTITY(1, 1),
[OrderItemId] [int] NOT NULL,
[UserId] [int] NOT NULL,
[CommentText] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[CommentDate] [smalldatetime] NOT NULL CONSTRAINT [DF_OrderItemComment_CommentDate] DEFAULT (getdate())
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_OrderItemComment] on [dbo].[OrderItemComment]'
GO
ALTER TABLE [dbo].[OrderItemComment] ADD CONSTRAINT [PK_OrderItemComment] PRIMARY KEY CLUSTERED  ([OrderItemCommentId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[Metadata]'
GO
CREATE TABLE [dbo].[Metadata]
(
[MetadataId] [int] NOT NULL IDENTITY(1, 1),
[BrandId] [int] NULL CONSTRAINT [DF_Metadata_BrandId] DEFAULT ((1)),
[ParentMetadataId] [int] NULL,
[Name] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[ExternalRef] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Metadata_ExternalRef] DEFAULT (''),
[Synonyms] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_Metadata_Synonyms] DEFAULT (''),
[GroupNumber] [int] NOT NULL,
[IsDeleted] [bit] NOT NULL CONSTRAINT [DF_Metadata_IsDeleted] DEFAULT ((0)),
[MetadataOrder] [int] NOT NULL CONSTRAINT [DF_Metadata_MetadataOrder] DEFAULT ((1))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_Metadata] on [dbo].[Metadata]'
GO
ALTER TABLE [dbo].[Metadata] ADD CONSTRAINT [PK_Metadata] PRIMARY KEY CLUSTERED  ([MetadataId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetMetadata]'
GO
CREATE TABLE [dbo].[AssetMetadata]
(
[AssetMetadataId] [int] NOT NULL IDENTITY(1, 1),
[AssetId] [int] NOT NULL,
[MetadataId] [int] NOT NULL CONSTRAINT [DF_AssetMetadata_Metadata1_Id] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetMetadata] on [dbo].[AssetMetadata]'
GO
ALTER TABLE [dbo].[AssetMetadata] ADD CONSTRAINT [PK_AssetMetadata] PRIMARY KEY CLUSTERED  ([AssetMetadataId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AssetMetadata_Asset] on [dbo].[AssetMetadata]'
GO
CREATE NONCLUSTERED INDEX [IX_AssetMetadata_Asset] ON [dbo].[AssetMetadata] ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetType]'
GO
CREATE VIEW dbo.v_AssetType
AS
SELECT     AssetTypeId, Name, IsVisible, IsDeleted
FROM      dbo.AssetType AS AT
WHERE    (IsDeleted = 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Metadata]'
GO

CREATE VIEW [dbo].[v_Metadata]
AS
SELECT     MetadataId, BrandId, ParentMetadataId, Name, ExternalRef, Synonyms, GroupNumber, IsDeleted, MetadataOrder
FROM      dbo.Metadata AS M
WHERE    (IsDeleted = 0)

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[AssetImageSize]'
GO
CREATE TABLE [dbo].[AssetImageSize]
(
[AssetImageSizeId] [int] NOT NULL IDENTITY(1, 1),
[Description] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[Height] [int] NOT NULL,
[Width] [int] NOT NULL,
[DotsPerInch] [int] NOT NULL CONSTRAINT [DF_AssetImageSize_DotsPerInch] DEFAULT ((0)),
[FileSuffix] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_AssetImageSize_FileSuffix] DEFAULT ('-')
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_AssetImageSize] on [dbo].[AssetImageSize]'
GO
ALTER TABLE [dbo].[AssetImageSize] ADD CONSTRAINT [PK_AssetImageSize] PRIMARY KEY CLUSTERED  ([AssetImageSizeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_AssetFileSuffix] on [dbo].[AssetImageSize]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AssetFileSuffix] ON [dbo].[AssetImageSize] ([FileSuffix])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[BrandMetadataSetting]'
GO
CREATE TABLE [dbo].[BrandMetadataSetting]
(
[BrandMetadataSettingId] [int] NOT NULL IDENTITY(1, 1),
[BrandId] [int] NOT NULL,
[FieldId] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[FieldName] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
[IsRequired] [bit] NOT NULL CONSTRAINT [DF_MetadataSetting_IsRequired] DEFAULT ((1)),
[OnAssetForm] [bit] NOT NULL CONSTRAINT [DF_MetadataSetting_OnAssetForm] DEFAULT ((1)),
[OnAssetDetail] [bit] NOT NULL CONSTRAINT [DF_MetadataSetting_OnAssetDetail] DEFAULT ((1)),
[AdditionalCopy] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_MetadataSetting_AdditionalCopy] DEFAULT (''),
[ToolTip] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL CONSTRAINT [DF_MetadataSetting_ToolTip] DEFAULT (''),
[UiControlType] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_UIControlType] DEFAULT ((0)),
[IsCustom] [bit] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_IsCustom] DEFAULT ((0)),
[CatalogueOrderNum] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_CatalogueOrderNum] DEFAULT ((0)),
[AssetDetailOrderNum] [int] NOT NULL CONSTRAINT [DF_BrandMetadataSetting_AssetDetailOrderNum] DEFAULT ((0))
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_MetadataSetting] on [dbo].[BrandMetadataSetting]'
GO
ALTER TABLE [dbo].[BrandMetadataSetting] ADD CONSTRAINT [PK_MetadataSetting] PRIMARY KEY CLUSTERED  ([BrandMetadataSettingId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IX_BrandMetadataSetting] on [dbo].[BrandMetadataSetting]'
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_BrandMetadataSetting] ON [dbo].[BrandMetadataSetting] ([BrandId], [FieldName])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[BrandMetadataSelectableSetting]'
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_BrandMetadataSelectableSetting] on [dbo].[BrandMetadataSelectableSetting]'
GO
ALTER TABLE [dbo].[BrandMetadataSelectableSetting] ADD CONSTRAINT [PK_BrandMetadataSelectableSetting] PRIMARY KEY CLUSTERED  ([BrandMetadataSelectableSettingId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_WorkflowUser]'
GO

--dbo.v_WorkflowUser

CREATE VIEW [dbo].[v_WorkflowUser]
AS
SELECT     dbo.WorkflowUser.WorkflowUserId, dbo.WorkflowUser.WorkflowId, dbo.WorkflowUser.UserId, dbo.[User].FirstName + ' ' + dbo.[User].LastName AS UserFullName, 
                      dbo.WorkflowUser.Position, dbo.WorkflowUser.DateAdded
FROM         dbo.[User] RIGHT OUTER JOIN
                      dbo.WorkflowUser ON dbo.[User].UserId = dbo.WorkflowUser.UserId

GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AuditAssetSearch]'
GO
CREATE VIEW dbo.v_AuditAssetSearch
AS
SELECT     AAS.AuditAssetSearchId, AAS.SessionId, AAS.IpAddress, AAS.UserId, AAS.Date, AASK.SearchKeyword, AASR.AssetId, U.FirstName + ' ' + U.LastName AS UserName
FROM      dbo.AuditAssetSearch AS AAS INNER JOIN
                  dbo.AuditAssetSearchKeyword AS AASK ON AAS.AuditAssetSearchId = AASK.AuditAssetSearchId INNER JOIN
                  dbo.AuditAssetSearchResult AS AASR ON AAS.AuditAssetSearchId = AASR.AuditAssetSearchId INNER JOIN
                  dbo.[User] AS U ON AAS.UserId = U.UserId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetTopSearchTerms]'
GO
CREATE PROCEDURE [dbo].[usp_GetTopSearchTerms] 
	@assetId INT,
	@maxRecords INT = 10
AS
BEGIN
	SET NOCOUNT ON;

	WITH
		SearchTerms
	AS
	(
		SELECT		SearchKeyword,
					COUNT(SearchKeyword) AS 'Count'
		FROM		v_AuditAssetSearch
		WHERE		(AssetId = @assetId)
		GROUP BY	SearchKeyword
	)

	SELECT TOP(@maxRecords) ROW_NUMBER() OVER (ORDER BY [Count] DESC) AS 'Rank', SearchKeyword, [Count] FROM SearchTerms ORDER BY [Count] DESC
END





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GetAssetCategories]'
GO
CREATE FUNCTION [dbo].[GetAssetCategories](@assetId INT)
RETURNS NVARCHAR(MAX)
AS 
BEGIN
	
	-- Initialize our category list string
	DECLARE @returnVal NVARCHAR(MAX)
	
	-- Get the category names and its synonyms for all categories for this asset
	SELECT @returnVal = (SELECT CAST([NameAndSynonyms] AS NVARCHAR(MAX)) + ' ' AS [text()] FROM v_AssetCategory AL WHERE AL.AssetID = @assetId FOR XML PATH(''));

	-- Strip out the semicolons in the synonyms
	SELECT @returnVal = REPLACE(@returnVal, ';', ' ');
	
	-- Ensure we have a value
	IF (@returnVal IS NULL)
		SELECT @returnVal = ' ';
	
	-- Strip spaces and return
	RETURN LTRIM(RTRIM(@returnVal))
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Asset]'
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating index [IDX_AssetId] on [dbo].[v_Asset]'
GO
CREATE UNIQUE CLUSTERED INDEX [IDX_AssetId] ON [dbo].[v_Asset] ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Category]'
GO
CREATE VIEW dbo.v_Category
AS
SELECT     C.CategoryId, C.ParentCategoryId, C.BrandId, C.Name, C.ExternalRef, C.Message, C.Synonyms, C.CategoryOrder, C.OwnerUserId, U.Email AS OwnerEmail, 
                  U.FirstName + ' ' + U.LastName AS OwnerName, C.FullAssetCount, C.AvailableAssetCount
FROM      dbo.Category AS C INNER JOIN
                  dbo.[User] AS U ON C.OwnerUserId = U.UserId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetLink]'
GO
CREATE VIEW dbo.v_AssetLink
AS
SELECT     AL.AssetLinkId, AL.AssetId, AL.LinkedAssetId, A.Title AS LinkedAssetTitle
FROM      dbo.AssetLink AS AL INNER JOIN
                  dbo.v_Asset AS A ON AL.LinkedAssetId = A.AssetId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_Workflow]'
GO
/*Views 
dbo.v_Workflow*/
CREATE VIEW dbo.v_Workflow
AS
SELECT     W.WorkflowId, W.Name, W.BrandId, B.Name AS BrandName, W.IsDeleted
FROM      dbo.Workflow AS W INNER JOIN
                  dbo.Brand AS B ON W.BrandId = B.BrandId
WHERE    (W.IsDeleted = 0)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_OrderItemComment]'
GO
CREATE VIEW dbo.v_OrderItemComment
AS
SELECT     OIC.OrderItemCommentId, OIC.OrderItemId, OIC.UserId, U.FullName AS UserFullName, OIC.CommentText, OIC.CommentDate
FROM         dbo.OrderItemComment AS OIC INNER JOIN
                      dbo.v_User AS U ON OIC.UserId = U.UserId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[usp_GetAssetPopularityList]'
GO
CREATE PROCEDURE [dbo].[usp_GetAssetPopularityList] 
	@top			int				= 10,
	@assetTypeId	int				= NULL,
	@brandId		int				= NULL,
	@startDate		smalldatetime	= NULL,
	@endDate		smalldatetime	= NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @downloadAuditAssetAction INT
	SELECT @downloadAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Downloaded Asset File')

	-----------------------------------------------------------------------------
	-- Initialise dates to default values
	-----------------------------------------------------------------------------
	SELECT @startDate = ISNULL(@startDate, '01 January 1900');
	SELECT @endDate = ISNULL(@endDate, '06 June 2079');

	-----------------------------------------------------------------------------
	-- First get all assets matching criteria and their download count
	-----------------------------------------------------------------------------
	WITH AssetsMatchingCriteriaWithDownloadCount
	AS
	(
		SELECT
			AssetId,
			AT.Name AS AssetTypeName,
			UploadedByUserName,
			Title,
			UploadDate,
			(SELECT COUNT(AAH.AuditAssetHistoryId) FROM AuditAssetHistory AAH WHERE (AAH.AuditAssetActionId=@downloadAuditAssetAction) AND (AAH.AssetId = A.AssetId) AND (Date BETWEEN @startDate AND @endDate)) AS 'DownloadCountInTimeFrame'
		FROM
			v_Asset A
			INNER JOIN AssetType AT ON AT.AssetTypeId = A.AssetTypeId
		WHERE
			A.AssetTypeId = ISNULL(@assetTypeId, A.AssetTypeId)
			AND
			A.BrandId = ISNULL(@brandId, A.BrandId)
	),

	-----------------------------------------------------------------------------
	-- Now get the popularity rank too
	-----------------------------------------------------------------------------
	AssetsMatchingCriteriaWithPopularityRank
	AS
	(
		SELECT
			DENSE_RANK() OVER (ORDER BY DownloadCountInTimeFrame DESC, UploadDate DESC) AS 'PopularityRankInTimeFrame',
			*
		FROM
			AssetsMatchingCriteriaWithDownloadCount
	)

	-----------------------------------------------------------------------------
	-- Get the returned result set, only returning the number of rows requested
	-----------------------------------------------------------------------------
	SELECT TOP(@top) * FROM AssetsMatchingCriteriaWithPopularityRank

END





GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[v_AssetMetadata]'
GO
CREATE VIEW dbo.v_AssetMetadata
AS
SELECT     AMD.*, M.BrandId, M.GroupNumber, M.Name
FROM      dbo.AssetMetadata AS AMD INNER JOIN
                  dbo.Metadata AS M ON AMD.MetadataId = M.MetadataId
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[GetProductionDate]'
GO





CREATE FUNCTION [dbo].[GetProductionDate]
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
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[IpAddress]'
GO
CREATE TABLE [dbo].[IpAddress]
(
[IpAddressId] [int] NOT NULL IDENTITY(1, 1),
[IpAddressValue] [varchar] (15) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_IpAddress] on [dbo].[IpAddress]'
GO
ALTER TABLE [dbo].[IpAddress] ADD CONSTRAINT [PK_IpAddress] PRIMARY KEY CLUSTERED  ([IpAddressId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating [dbo].[OrderItemStatus]'
GO
CREATE TABLE [dbo].[OrderItemStatus]
(
[OrderItemStatusId] [int] NOT NULL IDENTITY(1, 1),
[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
)
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating primary key [PK_OrderItemStatus] on [dbo].[OrderItemStatus]'
GO
ALTER TABLE [dbo].[OrderItemStatus] ADD CONSTRAINT [PK_OrderItemStatus] PRIMARY KEY CLUSTERED  ([OrderItemStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[Company]'
GO
ALTER TABLE [dbo].[Company] ADD CONSTRAINT [IX_CompanyDomain] UNIQUE NONCLUSTERED  ([Domain])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding constraints to [dbo].[User]'
GO
ALTER TABLE [dbo].[User] ADD CONSTRAINT [IX_UserEmail] UNIQUE NONCLUSTERED  ([Email])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Asset]'
GO
ALTER TABLE [dbo].[Asset] WITH NOCHECK ADD
CONSTRAINT [FK_Asset_AssetFilePath] FOREIGN KEY ([AssetFilePathId]) REFERENCES [dbo].[AssetFilePath] ([AssetFilePathId]),
CONSTRAINT [FK_Asset_AssetPublishStatus] FOREIGN KEY ([AssetPublishStatusId]) REFERENCES [dbo].[AssetPublishStatus] ([AssetPublishStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetWorkflowUser]'
GO
ALTER TABLE [dbo].[AssetWorkflowUser] WITH NOCHECK ADD
CONSTRAINT [FK_AssetWorkflowUser_AssetWorkflowUserStatus] FOREIGN KEY ([AssetWorkflowUserStatusId]) REFERENCES [dbo].[AssetWorkflowUserStatus] ([AssetWorkflowUserStatusId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditAssetHistory]'
GO
ALTER TABLE [dbo].[AuditAssetHistory] WITH NOCHECK ADD
CONSTRAINT [FK_AuditAssetHistory_AuditAssetAction] FOREIGN KEY ([AuditAssetActionId]) REFERENCES [dbo].[AuditAssetAction] ([AuditAssetActionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditUserHistory]'
GO
ALTER TABLE [dbo].[AuditUserHistory] WITH NOCHECK ADD
CONSTRAINT [FK_AuditUserHistory_AuditUserAction] FOREIGN KEY ([AuditUserActionId]) REFERENCES [dbo].[AuditUserAction] ([AuditUserActionId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetCategory]'
GO
ALTER TABLE [dbo].[AssetCategory] ADD
CONSTRAINT [FK_AssetCategory_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]),
CONSTRAINT [FK_AssetCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetFile]'
GO
ALTER TABLE [dbo].[AssetFile] ADD
CONSTRAINT [FK_AssetFile_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]),
CONSTRAINT [FK_AssetFile_AssetFileType] FOREIGN KEY ([AssetFileTypeId]) REFERENCES [dbo].[AssetFileType] ([AssetFileTypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetMetadata]'
GO
ALTER TABLE [dbo].[AssetMetadata] ADD
CONSTRAINT [FK_AssetMetadata_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]),
CONSTRAINT [FK_AssetMetadata_AssetMetadata1] FOREIGN KEY ([MetadataId]) REFERENCES [dbo].[Metadata] ([MetadataId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetMetadataTextArea]'
GO
ALTER TABLE [dbo].[AssetMetadataTextArea] ADD
CONSTRAINT [FK_AssetMetadataTextArea_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetMetadataTextField]'
GO
ALTER TABLE [dbo].[AssetMetadataTextField] ADD
CONSTRAINT [FK_AssetMetadataTextField_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetProcess]'
GO
ALTER TABLE [dbo].[AssetProcess] ADD
CONSTRAINT [FK_AssetProcess_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]),
CONSTRAINT [FK_AssetProcess_User] FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetWorkflow]'
GO
ALTER TABLE [dbo].[AssetWorkflow] ADD
CONSTRAINT [FK_AssetWorkflow_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]),
CONSTRAINT [FK_AssetWorkflow_User] FOREIGN KEY ([SubmittedByUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Cart]'
GO
ALTER TABLE [dbo].[Cart] ADD
CONSTRAINT [FK_Cart_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]) ON DELETE CASCADE,
CONSTRAINT [FK_Cart_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[LightboxAsset]'
GO
ALTER TABLE [dbo].[LightboxAsset] ADD
CONSTRAINT [FK_LightboxAsset_Asset] FOREIGN KEY ([AssetId]) REFERENCES [dbo].[Asset] ([AssetId]) ON DELETE CASCADE,
CONSTRAINT [FK_LightboxAsset_Lightbox] FOREIGN KEY ([LightboxId]) REFERENCES [dbo].[Lightbox] ([LightboxId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Asset]'
GO
ALTER TABLE [dbo].[Asset] ADD
CONSTRAINT [FK_Asset_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [dbo].[AssetType] ([AssetTypeId]),
CONSTRAINT [FK_Asset_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflow] ([WorkflowId]),
CONSTRAINT [FK_Asset_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId]),
CONSTRAINT [FK_Asset_User] FOREIGN KEY ([UploadedByUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetTypeFileExtension]'
GO
ALTER TABLE [dbo].[AssetTypeFileExtension] ADD
CONSTRAINT [FK_AssetTypeFileExtension_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [dbo].[AssetType] ([AssetTypeId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetWorkflowUser]'
GO
ALTER TABLE [dbo].[AssetWorkflowUser] ADD
CONSTRAINT [FK_AssetWorkflowUser_AssetWorkflow] FOREIGN KEY ([AssetWorkflowId]) REFERENCES [dbo].[AssetWorkflow] ([AssetWorkflowId]) ON DELETE CASCADE,
CONSTRAINT [FK_AssetWorkflowUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AssetWorkflowCommenter]'
GO
ALTER TABLE [dbo].[AssetWorkflowCommenter] ADD
CONSTRAINT [FK_AssetWorkflowCommenter_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
CONSTRAINT [FK_AssetWorkflowCommenter_AssetWorkflowUser] FOREIGN KEY ([InvitingAssetWorkflowUserId]) REFERENCES [dbo].[AssetWorkflowUser] ([AssetWorkflowUserId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditAssetHistory]'
GO
ALTER TABLE [dbo].[AuditAssetHistory] ADD
CONSTRAINT [FK_AuditAssetHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditAssetSearchKeyword]'
GO
ALTER TABLE [dbo].[AuditAssetSearchKeyword] ADD
CONSTRAINT [FK_AuditAssetSearchKeyword_AuditAssetSearch] FOREIGN KEY ([AuditAssetSearchId]) REFERENCES [dbo].[AuditAssetSearch] ([AuditAssetSearchId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditAssetSearchResult]'
GO
ALTER TABLE [dbo].[AuditAssetSearchResult] ADD
CONSTRAINT [FK_AuditAssetSearchResult_AuditAssetSearch] FOREIGN KEY ([AuditAssetSearchId]) REFERENCES [dbo].[AuditAssetSearch] ([AuditAssetSearchId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditAssetSearch]'
GO
ALTER TABLE [dbo].[AuditAssetSearch] ADD
CONSTRAINT [FK_AuditAssetSearch_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[AuditUserHistory]'
GO
ALTER TABLE [dbo].[AuditUserHistory] ADD
CONSTRAINT [FK_AuditUserHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Category]'
GO
ALTER TABLE [dbo].[Category] ADD
CONSTRAINT [FK_Category_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId]),
CONSTRAINT [FK_Category_ParentCategory] FOREIGN KEY ([ParentCategoryId]) REFERENCES [dbo].[Category] ([CategoryId]),
CONSTRAINT [FK_Category_User] FOREIGN KEY ([OwnerUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[CompanyBrand]'
GO
ALTER TABLE [dbo].[CompanyBrand] ADD
CONSTRAINT [FK_CompanyBrand_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId]),
CONSTRAINT [FK_CompanyBrand_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[LightboxBrand]'
GO
ALTER TABLE [dbo].[LightboxBrand] ADD
CONSTRAINT [FK_LightboxBrand_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId]) ON DELETE CASCADE,
CONSTRAINT [FK_LightboxBrand_Lightbox] FOREIGN KEY ([LightboxId]) REFERENCES [dbo].[Lightbox] ([LightboxId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Metadata]'
GO
ALTER TABLE [dbo].[Metadata] ADD
CONSTRAINT [FK_Metadata_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[User]'
GO
ALTER TABLE [dbo].[User] ADD
CONSTRAINT [FK_User_Brand] FOREIGN KEY ([PrimaryBrandId]) REFERENCES [dbo].[Brand] ([BrandId]),
CONSTRAINT [FK_User_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId]),
CONSTRAINT [FK_User_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([UserRoleId]),
CONSTRAINT [FK_User_UserStatus] FOREIGN KEY ([UserStatusId]) REFERENCES [dbo].[UserStatus] ([UserStatusId]),
CONSTRAINT [FK_User_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflow] ([WorkflowId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[UserBrand]'
GO
ALTER TABLE [dbo].[UserBrand] ADD
CONSTRAINT [FK_UserBrand_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId]),
CONSTRAINT [FK_UserBrand_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Workflow]'
GO
ALTER TABLE [dbo].[Workflow] ADD
CONSTRAINT [FK_Workflow_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[BrandMetadataSelectableSetting]'
GO
ALTER TABLE [dbo].[BrandMetadataSelectableSetting] ADD
CONSTRAINT [FK_BrandMetadataSelectableSetting_BrandMetadataSetting] FOREIGN KEY ([BrandMetadataSettingId]) REFERENCES [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[HomepageCategory]'
GO
ALTER TABLE [dbo].[HomepageCategory] ADD
CONSTRAINT [FK_HomepageCategory_Category] FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[Category] ([CategoryId]),
CONSTRAINT [FK_HomepageCategory_Homepage] FOREIGN KEY ([HomepageId]) REFERENCES [dbo].[Homepage] ([HomepageId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Company]'
GO
ALTER TABLE [dbo].[Company] ADD
CONSTRAINT [FK_Company_User] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Homepage]'
GO
ALTER TABLE [dbo].[Homepage] ADD
CONSTRAINT [FK_Homepage_HomepageType] FOREIGN KEY ([HomepageTypeId]) REFERENCES [dbo].[HomepageType] ([HomepageTypeId]),
CONSTRAINT [FK_Homepage_User] FOREIGN KEY ([LastModifiedByUserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[LightboxLinked]'
GO
ALTER TABLE [dbo].[LightboxLinked] ADD
CONSTRAINT [FK_LightboxLinked_Lightbox] FOREIGN KEY ([LightboxId]) REFERENCES [dbo].[Lightbox] ([LightboxId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[Lightbox]'
GO
ALTER TABLE [dbo].[Lightbox] ADD
CONSTRAINT [FK_Lightbox_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[OrderItemComment]'
GO
ALTER TABLE [dbo].[OrderItemComment] ADD
CONSTRAINT [FK_OrderItemComment_OrderItem] FOREIGN KEY ([OrderItemId]) REFERENCES [dbo].[OrderItem] ([OrderItemId]),
CONSTRAINT [FK_OrderItemComment_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[UserPasswordHistory]'
GO
ALTER TABLE [dbo].[UserPasswordHistory] ADD
CONSTRAINT [FK_UserPasswordHistory_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]) ON DELETE CASCADE
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Adding foreign keys to [dbo].[WorkflowUser]'
GO
ALTER TABLE [dbo].[WorkflowUser] ADD
CONSTRAINT [FK_WorkflowUser_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
CONSTRAINT [FK_WorkflowUser_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflow] ([WorkflowId])
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[UpdateAssetCategoryListOnDelete] on [dbo].[AssetCategory]'
GO
CREATE TRIGGER [dbo].[UpdateAssetCategoryListOnDelete] ON [dbo].[AssetCategory]  AFTER DELETE
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE A SET A.AssetCategories = dbo.GetAssetCategories(A.AssetId) FROM Asset A JOIN DELETED J ON A.AssetId = J.AssetId
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[UpdateAssetCategoryListOnInsert] on [dbo].[AssetCategory]'
GO
CREATE TRIGGER [dbo].[UpdateAssetCategoryListOnInsert] ON [dbo].[AssetCategory]  AFTER INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	UPDATE A SET A.AssetCategories = dbo.GetAssetCategories(A.AssetId) FROM Asset A JOIN INSERTED J ON A.AssetId = J.AssetId
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
PRINT N'Creating trigger [dbo].[UpdateAssetDownloadCount] on [dbo].[AuditAssetHistory]'
GO
CREATE TRIGGER [dbo].[UpdateAssetDownloadCount] ON [dbo].[AuditAssetHistory]  AFTER  INSERT
AS 
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @downloadAuditAssetAction INT
	SELECT @downloadAuditAssetAction = AuditAssetActionId FROM AuditAssetAction WHERE ([Description] = 'Downloaded Asset File')
	
	UPDATE A SET A.DownloadCount = A.DownloadCount+1  FROM Asset A JOIN INSERTED I ON A.AssetId = I.AssetId WHERE (I.AuditAssetActionId = @downloadAuditAssetAction)
END
GO
IF @@ERROR<>0 AND @@TRANCOUNT>0 ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT=0 BEGIN INSERT INTO #tmpErrors (Error) SELECT 1 BEGIN TRANSACTION END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT 'The database update succeeded'
COMMIT TRANSACTION
END
ELSE PRINT 'The database update failed'
GO
DROP TABLE #tmpErrors
GO
PRINT N'Creating full text catalogs'
GO
CREATE FULLTEXT CATALOG [FocusOPENFullTextCatalog]
ON FILEGROUP [PRIMARY]
--IN PATH 'C:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\FTData'
WITH ACCENT_SENSITIVITY = ON
AS DEFAULT
AUTHORIZATION [dbo]
GO
PRINT N'Adding full text indexing to tables'
GO
CREATE FULLTEXT INDEX ON [dbo].[AssetFile] KEY INDEX [PK_AssetFileId] ON [FocusOPENFullTextCatalog]
GO
CREATE FULLTEXT INDEX ON [dbo].[v_Asset] KEY INDEX [IDX_AssetId] ON [FocusOPENFullTextCatalog] WITH CHANGE_TRACKING OFF
GO
PRINT N'Adding full text indexing to columns'
GO
ALTER FULLTEXT INDEX ON [dbo].[AssetFile] ADD (FileContent TYPE COLUMN FileExtension LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[AssetFile] ADD (Filename LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[AssetFile] ADD (FileExtension LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (AssetTypeName LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (Filename LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (FileHash LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (Title LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (ProjectCode LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (Originator LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (Description LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (Keywords LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (CopyrightOwner LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (UploadedByUserName LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (AssetCategories LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (AssetMetadataVals LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (MetadataSearchVals LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (BrandName LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ADD (SearchableData LANGUAGE 1033)
GO
ALTER FULLTEXT INDEX ON [dbo].[AssetFile] ENABLE
GO
ALTER FULLTEXT INDEX ON [dbo].[v_Asset] ENABLE
GO
---------------------------------------------------------------------------------
-- FocusOPEN Asset View Creation Script
---------------------------------------------------------------------------------

IF EXISTS(SELECT * FROM sysobjects where name = 'v_Asset')
BEGIN
	DROP VIEW dbo.v_Asset
	PRINT 'Old view dropped successfully'
END

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

PRINT 'View created successfully'

CREATE UNIQUE CLUSTERED INDEX IDX_AssetId ON v_Asset(AssetId)

GO

PRINT 'Index created successfully'

EXEC sp_fulltext_table @tabname	= 'dbo.v_Asset', @action = 'create', @ftcat	= 'FocusOPENFullTextCatalog', @keyname = 'IDX_AssetId'
PRINT 'Full-Text Search enabled successfully'

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
PRINT 'Added indexed fields'

EXEC sp_fulltext_table 'dbo.v_Asset', 'start_full'
PRINT 'Done a full re-index of v_Asset'

EXEC sp_fulltext_table 'dbo.v_Asset', 'start_change_tracking' 
PRINT 'Enabled change tracking'

EXEC sp_fulltext_table 'dbo.v_Asset', 'start_background_updateindex' 
PRINT 'Enabled automatic background change tracking'