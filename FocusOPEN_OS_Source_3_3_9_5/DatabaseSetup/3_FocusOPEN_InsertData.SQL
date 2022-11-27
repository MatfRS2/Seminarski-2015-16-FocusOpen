SET NUMERIC_ROUNDABORT OFF
GO
SET ANSI_PADDING, ANSI_WARNINGS, CONCAT_NULL_YIELDS_NULL, ARITHABORT, QUOTED_IDENTIFIER, ANSI_NULLS, NOCOUNT ON
GO
SET DATEFORMAT YMD
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
-- Pointer used for text / image updates. This might not be needed, but is declared here just in case
DECLARE @pv binary(16)
BEGIN TRANSACTION

-- Drop constraints from [dbo].[User]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Brand]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Country]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_UserRole]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_UserStatus]
ALTER TABLE [dbo].[User] DROP CONSTRAINT [FK_User_Workflow]

-- Drop constraints from [dbo].[Metadata]
ALTER TABLE [dbo].[Metadata] DROP CONSTRAINT [FK_Metadata_Brand]

-- Drop constraints from [dbo].[Homepage]
ALTER TABLE [dbo].[Homepage] DROP CONSTRAINT [FK_Homepage_HomepageType]
ALTER TABLE [dbo].[Homepage] DROP CONSTRAINT [FK_Homepage_User]

-- Drop constraints from [dbo].[CompanyBrand]
ALTER TABLE [dbo].[CompanyBrand] DROP CONSTRAINT [FK_CompanyBrand_Brand]
ALTER TABLE [dbo].[CompanyBrand] DROP CONSTRAINT [FK_CompanyBrand_Company]

-- Drop constraints from [dbo].[Company]
ALTER TABLE [dbo].[Company] DROP CONSTRAINT [FK_Company_User]

-- Drop constraints from [dbo].[AssetTypeFileExtension]
ALTER TABLE [dbo].[AssetTypeFileExtension] DROP CONSTRAINT [FK_AssetTypeFileExtension_AssetType]

-- Add row to [dbo].[AssetFilePath]
SET IDENTITY_INSERT [dbo].[AssetFilePath] ON
INSERT INTO [dbo].[AssetFilePath] ([AssetFilePathId], [Path], [IsDefault]) VALUES (1, 'C:\FocusOPEN\Data\', 1)
SET IDENTITY_INSERT [dbo].[AssetFilePath] OFF

-- Add rows to [dbo].[AssetFileType]
INSERT INTO [dbo].[AssetFileType] ([AssetFileTypeId], [Name]) VALUES (1, 'Asset File')
INSERT INTO [dbo].[AssetFileType] ([AssetFileTypeId], [Name]) VALUES (2, 'Asset File Zipped')
INSERT INTO [dbo].[AssetFileType] ([AssetFileTypeId], [Name]) VALUES (3, 'Asset Preview')
INSERT INTO [dbo].[AssetFileType] ([AssetFileTypeId], [Name]) VALUES (4, 'Asset Thumbnail')
INSERT INTO [dbo].[AssetFileType] ([AssetFileTypeId], [Name]) VALUES (5, 'Attached File')
-- Operation applied to 5 rows out of 5

-- Add rows to [dbo].[AssetPublishStatus]
SET IDENTITY_INSERT [dbo].[AssetPublishStatus] ON
INSERT INTO [dbo].[AssetPublishStatus] ([AssetPublishStatusId], [Description]) VALUES (1, 'Not Approved')
INSERT INTO [dbo].[AssetPublishStatus] ([AssetPublishStatusId], [Description]) VALUES (2, 'Pending Approval')
INSERT INTO [dbo].[AssetPublishStatus] ([AssetPublishStatusId], [Description]) VALUES (3, 'Approved')
INSERT INTO [dbo].[AssetPublishStatus] ([AssetPublishStatusId], [Description]) VALUES (4, 'Not Published')
INSERT INTO [dbo].[AssetPublishStatus] ([AssetPublishStatusId], [Description]) VALUES (5, 'Published')
SET IDENTITY_INSERT [dbo].[AssetPublishStatus] OFF
-- Operation applied to 5 rows out of 5

-- Add rows to [dbo].[AssetWorkflowUserStatus]
SET IDENTITY_INSERT [dbo].[AssetWorkflowUserStatus] ON
INSERT INTO [dbo].[AssetWorkflowUserStatus] ([AssetWorkflowUserStatusId], [Name]) VALUES (1, 'Waiting')
INSERT INTO [dbo].[AssetWorkflowUserStatus] ([AssetWorkflowUserStatusId], [Name]) VALUES (2, 'Pending')
INSERT INTO [dbo].[AssetWorkflowUserStatus] ([AssetWorkflowUserStatusId], [Name]) VALUES (3, 'Approved')
INSERT INTO [dbo].[AssetWorkflowUserStatus] ([AssetWorkflowUserStatusId], [Name]) VALUES (4, 'Rejected')
SET IDENTITY_INSERT [dbo].[AssetWorkflowUserStatus] OFF
-- Operation applied to 4 rows out of 4

-- Add rows to [dbo].[AuditAssetAction]
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (1, 'Added To Cart')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (2, 'Added To Lightbox')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (3, 'Approved For Download')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (4, 'Catalogued Asset')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (5, 'Delete Asset')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (6, 'Downloaded Asset File')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (7, 'Downloaded Attached File')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (8, 'Ordered')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (9, 'Rejected For Download')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (10, 'Removed From Cart')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (11, 'Removed From Lightbox')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (12, 'Replaced Asset File')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (13, 'Returned In Search')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (14, 'Saved Asset')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (15, 'Uploaded Asset')
INSERT INTO [dbo].[AuditAssetAction] ([AuditAssetActionId], [Description]) VALUES (16, 'Viewed Asset Detail')
-- Operation applied to 16 rows out of 16

-- Add rows to [dbo].[AuditUserAction]
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (1, 'Account Update')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (2, 'Add Asset Type')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (3, 'Add Brand')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (4, 'Add Category')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (5, 'Add Company')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (6, 'Add File Extension')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (7, 'Add Filepath')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (8, 'Add Ip Address')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (9, 'Add Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (10, 'Add Metadata')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (11, 'Add To Cart')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (12, 'Add To Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (13, 'Add User')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (14, 'Authorise Order')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (15, 'Authorise User')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (16, 'Catalogue Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (17, 'Change Default Filepath')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (18, 'Change Password')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (19, 'Confirm Email Address')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (20, 'Delete Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (21, 'Delete Asset Type')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (22, 'Delete Brand')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (23, 'Delete Category')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (24, 'Delete Company')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (25, 'Delete File Extension')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (26, 'Delete Ip Address')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (27, 'Delete Keyword')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (28, 'Delete Metadata')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (29, 'Delete User')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (30, 'Download Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (31, 'Download Attached File')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (32, 'Duplicate File Extension')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (33, 'Edit Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (34, 'Empty Cart')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (35, 'Logout')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (36, 'Modify Asset Type')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (37, 'Modify Brand')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (38, 'Modify Category')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (39, 'Modify Company')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (40, 'Modify File Extension')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (41, 'Modify Ip Address')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (42, 'Modify Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (43, 'Modify Metadata')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (44, 'Modify User')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (45, 'Order Assets')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (46, 'Password Reminder')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (47, 'Preview Homepage')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (48, 'Publish Homepage')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (49, 'Reactivate Account')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (50, 'Register')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (51, 'Registered By Admin')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (52, 'Remove From Cart')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (53, 'Remove From Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (54, 'Remove Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (55, 'Replaced Asset File')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (56, 'Search')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (57, 'Send Lightbox')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (58, 'System Error')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (59, 'Upload Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (60, 'User Login')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (61, 'Viewed Asset Detail')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (62, 'Download Contact Sheet Asset')
INSERT INTO [dbo].[AuditUserAction] ([AuditUserActionId], [Description]) VALUES (63, 'API Call')
-- Operation applied to 63 rows out of 63

-- Add rows to [dbo].[Country]
SET IDENTITY_INSERT [dbo].[Country] ON
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (1, 'AD', 'Andorra', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (2, 'AE', 'United Arab Emirates', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (3, 'AF', 'Afghanistan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (4, 'AG', 'Antigua and Barbuda', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (5, 'AI', 'Anguilla', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (6, 'AL', 'Albania', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (7, 'AM', 'Armenia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (8, 'AN', 'Netherlands Antilles', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (9, 'AO', 'Angola', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (10, 'AQ', 'Antarctica', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (11, 'AR', 'Argentina', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (12, 'AS', 'American Samoa', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (13, 'AT', 'Austria', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (14, 'AU', 'Australia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (15, 'AW', 'Aruba', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (16, 'AZ', 'Azerbaijan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (17, 'BA', 'Bosnia and Herzegovina', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (18, 'BB', 'Barbados', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (19, 'BD', 'Bangladesh', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (20, 'BE', 'Belgium', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (21, 'BF', 'Burkina Faso', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (22, 'BG', 'Bulgaria', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (23, 'BH', 'Bahrain', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (24, 'BI', 'Burundi', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (25, 'BJ', 'Benin', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (26, 'BM', 'Bermuda', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (27, 'BN', 'Brunei Darussalam', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (28, 'BO', 'Bolivia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (29, 'BR', 'Brazil', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (30, 'BS', 'Bahamas', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (31, 'BT', 'Bhutan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (32, 'BV', 'Bouvet Island', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (33, 'BW', 'Botswana', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (34, 'BY', 'Belarus', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (35, 'BZ', 'Belize', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (36, 'CA', 'Canada', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (37, 'CC', 'Cocos (Keeling) Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (38, 'CD', 'Congo, the Democratic Republic of the', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (39, 'CF', 'Central African Republic', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (40, 'CG', 'Congo', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (41, 'CH', 'Switzerland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (42, 'CK', 'Cook Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (43, 'CL', 'Chile', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (44, 'CM', 'Cameroon', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (45, 'CN', 'China', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (46, 'CO', 'Colombia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (47, 'CR', 'Costa Rica', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (48, 'CS', 'Serbia and Montenegro', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (49, 'CU', 'Cuba', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (50, 'CV', 'Cape Verde', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (51, 'CX', 'Christmas Island', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (52, 'CY', 'Cyprus', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (53, 'CZ', 'Czech Republic', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (54, 'DE', 'Germany', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (55, 'DJ', 'Djibouti', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (56, 'DK', 'Denmark', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (57, 'DM', 'Dominica', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (58, 'DO', 'Dominican Republic', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (59, 'DZ', 'Algeria', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (60, 'EC', 'Ecuador', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (61, 'EE', 'Estonia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (62, 'EG', 'Egypt', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (63, 'EH', 'Western Sahara', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (64, 'ER', 'Eritrea', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (65, 'ES', 'Spain', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (66, 'ET', 'Ethiopia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (67, 'FI', 'Finland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (68, 'FJ', 'Fiji', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (69, 'FK', 'Falkland Islands (Malvinas)', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (70, 'FM', 'Micronesia, Federated States of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (71, 'FO', 'Faroe Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (72, 'FR', 'France', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (73, 'GA', 'Gabon', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (74, 'GB', 'Great Britain', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (75, 'GD', 'Grenada', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (76, 'GE', 'Georgia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (77, 'GF', 'French Guiana', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (78, 'GG', 'Guernsey', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (79, 'GH', 'Ghana', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (80, 'GI', 'Gibraltar', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (81, 'GL', 'Greenland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (82, 'GM', 'Gambia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (83, 'GN', 'Guinea', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (84, 'GP', 'Guadeloupe', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (85, 'GQ', 'Equatorial Guinea', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (86, 'GR', 'Greece', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (87, 'GS', 'South Georgia and the South Sandwich Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (88, 'GT', 'Guatemala', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (89, 'GU', 'Guam', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (90, 'GW', 'Guinea-Bissau', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (91, 'GY', 'Guyana', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (92, 'HK', 'Hong Kong', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (93, 'HM', 'Heard Island and Mcdonald Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (94, 'HN', 'Honduras', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (95, 'HR', 'Croatia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (96, 'HT', 'Haiti', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (97, 'HU', 'Hungary', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (98, 'ID', 'Indonesia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (99, 'IE', 'Ireland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (100, 'IL', 'Israel', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (101, 'IN', 'India', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (102, 'IO', 'British Indian Ocean Territory', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (103, 'IQ', 'Iraq', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (104, 'IR', 'Iran, Islamic Republic of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (105, 'IS', 'Iceland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (106, 'IT', 'Italy', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (107, 'JM', 'Jamaica', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (108, 'JO', 'Jordan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (109, 'JP', 'Japan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (110, 'KE', 'Kenya', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (111, 'KG', 'Kyrgyzstan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (112, 'KH', 'Cambodia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (113, 'KI', 'Kiribati', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (114, 'KM', 'Comoros', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (115, 'KN', 'Saint Kitts and Nevis', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (116, 'KR', 'Korea, Republic of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (117, 'KW', 'Kuwait', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (118, 'KY', 'Cayman Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (119, 'KZ', 'Kazakhstan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (120, 'LB', 'Lebanon', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (121, 'LC', 'Saint Lucia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (122, 'LI', 'Liechtenstein', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (123, 'LK', 'Sri Lanka', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (124, 'LR', 'Liberia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (125, 'LS', 'Lesotho', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (126, 'LT', 'Lithuania', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (127, 'LU', 'Luxembourg', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (128, 'LV', 'Latvia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (129, 'LY', 'Libyan Arab Jamahiriya', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (130, 'MA', 'Morocco', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (131, 'MC', 'Monaco', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (132, 'MD', 'Moldova, Republic of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (133, 'MG', 'Madagascar', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (134, 'MH', 'Marshall Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (135, 'MK', 'Macedonia, the Former Yugoslav Republic of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (136, 'ML', 'Mali', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (137, 'MM', 'Myanmar', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (138, 'MN', 'Mongolia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (139, 'MO', 'Macao', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (140, 'MP', 'Northern Mariana Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (141, 'MQ', 'Martinique', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (142, 'MR', 'Mauritania', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (143, 'MS', 'Montserrat', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (144, 'MT', 'Malta', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (145, 'MU', 'Mauritius', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (146, 'MV', 'Maldives', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (147, 'MW', 'Malawi', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (148, 'MX', 'Mexico', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (149, 'MY', 'Malaysia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (150, 'MZ', 'Mozambique', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (151, 'NA', 'Namibia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (152, 'NC', 'New Caledonia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (153, 'NE', 'Niger', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (154, 'NF', 'Norfolk Island', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (155, 'NG', 'Nigeria', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (156, 'NI', 'Nicaragua', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (157, 'NL', 'Netherlands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (158, 'NO', 'Norway', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (159, 'NP', 'Nepal', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (160, 'NR', 'Nauru', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (161, 'NU', 'Niue', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (162, 'NZ', 'New Zealand', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (163, 'OM', 'Oman', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (164, 'PA', 'Panama', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (165, 'PE', 'Peru', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (166, 'PF', 'French Polynesia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (167, 'PG', 'Papua New Guinea', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (168, 'PH', 'Philippines', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (169, 'PK', 'Pakistan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (170, 'PL', 'Poland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (171, 'PM', 'Saint Pierre and Miquelon', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (172, 'PN', 'Pitcairn', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (173, 'PR', 'Puerto Rico', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (174, 'PS', 'Palestinian Territory, Occupied', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (175, 'PT', 'Portugal', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (176, 'PW', 'Palau', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (177, 'PY', 'Paraguay', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (178, 'QA', 'Qatar', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (179, 'RE', 'Reunion', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (180, 'RO', 'Romania', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (181, 'RU', 'Russian Federation', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (182, 'RW', 'Rwanda', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (183, 'SA', 'Saudi Arabia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (184, 'SB', 'Solomon Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (185, 'SC', 'Seychelles', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (186, 'SD', 'Sudan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (187, 'SE', 'Sweden', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (188, 'SG', 'Singapore', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (189, 'SH', 'Saint Helena', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (190, 'SI', 'Slovenia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (191, 'SJ', 'Svalbard and Jan Mayen', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (192, 'SK', 'Slovakia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (193, 'SL', 'Sierra Leone', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (194, 'SM', 'San Marino', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (195, 'SN', 'Senegal', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (196, 'SO', 'Somalia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (197, 'SR', 'Suriname', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (198, 'ST', 'Sao Tome and Principe', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (199, 'SV', 'El Salvador', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (200, 'SY', 'Syrian Arab Republic', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (201, 'SZ', 'Swaziland', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (202, 'TC', 'Turks and Caicos Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (203, 'TD', 'Chad', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (204, 'TF', 'French Southern Territories', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (205, 'TG', 'Togo', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (206, 'TH', 'Thailand', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (207, 'TJ', 'Tajikistan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (208, 'TK', 'Tokelau', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (209, 'TL', 'Timor-Leste', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (210, 'TM', 'Turkmenistan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (211, 'TN', 'Tunisia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (212, 'TO', 'Tonga', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (213, 'TR', 'Turkey', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (214, 'TT', 'Trinidad and Tobago', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (215, 'TV', 'Tuvalu', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (216, 'TW', 'Taiwan, Province of China', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (217, 'TZ', 'Tanzania, United Republic of', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (218, 'UA', 'Ukraine', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (219, 'UG', 'Uganda', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (220, 'UK', 'United Kingdom', 1)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (221, 'UM', 'United States Minor Outlying Islands', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (222, 'US', 'United States', 2)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (223, 'UY', 'Uruguay', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (224, 'UZ', 'Uzbekistan', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (225, 'VA', 'Holy See (Vatican City State)', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (226, 'VC', 'Saint Vincent and the Grenadines', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (227, 'VE', 'Venezuela', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (228, 'VG', 'Virgin Islands, British', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (229, 'VI', 'Virgin Islands, U.s.', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (230, 'VN', 'Viet Nam', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (231, 'VU', 'Vanuatu', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (232, 'WF', 'Wallis and Futuna', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (233, 'WS', 'Samoa', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (234, 'YE', 'Yemen', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (235, 'YT', 'Mayotte', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (236, 'ZA', 'South Africa', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (237, 'ZM', 'Zambia', 1000)
INSERT INTO [dbo].[Country] ([CountryId], [Code], [Name], [Rank]) VALUES (238, 'ZW', 'Zimbabwe', 1000)
SET IDENTITY_INSERT [dbo].[Country] OFF
-- Operation applied to 238 rows out of 238

-- Add rows to [dbo].[IpAddress]
SET IDENTITY_INSERT [dbo].[IpAddress] ON
INSERT INTO [dbo].[IpAddress] ([IpAddressId], [IpAddressValue]) VALUES (1, '127.0.0.1')
INSERT INTO [dbo].[IpAddress] ([IpAddressId], [IpAddressValue]) VALUES (2, '::1')
SET IDENTITY_INSERT [dbo].[IpAddress] OFF
-- Operation applied to 2 rows out of 2

-- Add rows to [dbo].[OrderItemStatus]
SET IDENTITY_INSERT [dbo].[OrderItemStatus] ON
INSERT INTO [dbo].[OrderItemStatus] ([OrderItemStatusId], [Name]) VALUES (1, 'Preapproved')
INSERT INTO [dbo].[OrderItemStatus] ([OrderItemStatusId], [Name]) VALUES (2, 'Awaiting Approval')
INSERT INTO [dbo].[OrderItemStatus] ([OrderItemStatusId], [Name]) VALUES (3, 'Approved')
INSERT INTO [dbo].[OrderItemStatus] ([OrderItemStatusId], [Name]) VALUES (4, 'Rejected')
SET IDENTITY_INSERT [dbo].[OrderItemStatus] OFF
-- Operation applied to 4 rows out of 4

-- Add rows to [dbo].[UserRole]
INSERT INTO [dbo].[UserRole] ([UserRoleId], [Name]) VALUES (1, 'Normal')
INSERT INTO [dbo].[UserRole] ([UserRoleId], [Name]) VALUES (2, 'Upload User')
INSERT INTO [dbo].[UserRole] ([UserRoleId], [Name]) VALUES (3, 'Brand Administrator')
INSERT INTO [dbo].[UserRole] ([UserRoleId], [Name]) VALUES (4, 'Super Administrator')
-- Operation applied to 4 rows out of 4

-- Add rows to [dbo].[UserStatus]
SET IDENTITY_INSERT [dbo].[UserStatus] ON
INSERT INTO [dbo].[UserStatus] ([UserStatusId], [Name]) VALUES (1, 'PendingAdminApproval')
INSERT INTO [dbo].[UserStatus] ([UserStatusId], [Name]) VALUES (2, 'PendingEmailConfirmation')
INSERT INTO [dbo].[UserStatus] ([UserStatusId], [Name]) VALUES (3, 'Approved')
INSERT INTO [dbo].[UserStatus] ([UserStatusId], [Name]) VALUES (4, 'Rejected')
SET IDENTITY_INSERT [dbo].[UserStatus] OFF
-- Operation applied to 4 rows out of 4

-- Add rows to [dbo].[AssetImageSize]
SET IDENTITY_INSERT [dbo].[AssetImageSize] ON
INSERT INTO [dbo].[AssetImageSize] ([AssetImageSizeId], [Description], [Height], [Width], [DotsPerInch], [FileSuffix]) VALUES (1, '72 dpi (screen & web)', 1024, 768, 72, '72dpi_screen&print')
INSERT INTO [dbo].[AssetImageSize] ([AssetImageSizeId], [Description], [Height], [Width], [DotsPerInch], [FileSuffix]) VALUES (2, '300 dpi (print)', 1500, 1000, 300, '300dpi_print')
INSERT INTO [dbo].[AssetImageSize] ([AssetImageSizeId], [Description], [Height], [Width], [DotsPerInch], [FileSuffix]) VALUES (3, '600 dpi (print)', 2500, 1250, 600, '600dpi_print')
SET IDENTITY_INSERT [dbo].[AssetImageSize] OFF
-- Operation applied to 3 rows out of 3

-- Add rows to [dbo].[AssetType]
SET IDENTITY_INSERT [dbo].[AssetType] ON
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (1, 'Generic', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (2, 'Image', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (3, 'Video', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (4, 'Animation', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (5, 'Print', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (6, 'Document', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (7, 'Presentation', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (8, 'Audio', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (9, '3D', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (10, 'Font', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (11, 'Spreadsheet', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (12, 'InDesign', 1, 0)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (13, 'MindMap', 1, 1)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (14, 'Test', 1, 1)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (15, 'MindMap', 1, 1)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (16, 'MindMap', 1, 1)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (17, 'MindMap', 1, 1)
INSERT INTO [dbo].[AssetType] ([AssetTypeId], [Name], [IsVisible], [IsDeleted]) VALUES (18, 'MindMap', 1, 0)
SET IDENTITY_INSERT [dbo].[AssetType] OFF
-- Operation applied to 18 rows out of 18

-- Add rows to [dbo].[AssetTypeFileExtension]
SET IDENTITY_INSERT [dbo].[AssetTypeFileExtension] ON
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (1, 'jpg', 'jpg', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (2, 'jpeg', 'jpeg', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (3, 'tif', 'tif', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (4, 'tiff', 'tiff', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (5, 'eps', 'eps', 2, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (6, 'gif', 'gif', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (7, 'png', 'png', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (8, 'psd', 'psd', 2, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (9, 'mpg', 'mpg', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (10, 'mpeg', 'mpeg', 3, NULL, '', 1, '69971c70-6415-46bf-aa76-dbadda5b4f06')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (11, 'avi', 'avi', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (12, 'mov', 'mov', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (13, 'wmv', 'wmv', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (14, 'vob', 'vob', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (15, 'flv', 'flv', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (16, 'mp4', 'mp4', 3, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (17, 'm4v', 'm4v', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (18, 'h264', 'h264', 3, NULL, '', 1, '81fb4b97-5b58-441a-89a3-df0708862dc7')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (19, 'swf', 'swf', 4, NULL, '', 1, '69971c70-6415-46bf-aa76-dbadda5b4f06')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (20, 'pdf', 'pdf', 5, NULL, '', 1, '198818c2-6012-486c-a5bd-e40a67dcfe71')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (21, 'doc', 'doc', 6, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (22, 'docx', 'docx', 6, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (23, 'rtf', 'rtf', 6, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (24, 'ppt', 'ppt', 7, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (25, 'pptx', 'pptx', 7, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (26, 'mp3', 'mp3', 8, NULL, '', 1, '6101ce70-f2ea-4a8b-ae6e-900f8795f248')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (27, 'wav', 'wav', 8, NULL, '', 1, '6101ce70-f2ea-4a8b-ae6e-900f8795f248')
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (28, 'max', 'max', 9, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (29, 'ttf', 'ttf', 10, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (30, 'ps', 'ps', 10, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (31, 'tft', 'tft', 10, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (32, 'xls', 'xls', 11, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (33, 'xlsx', 'xlsx', 11, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (34, 'csv', 'csv', 11, NULL, '', 1, NULL)
INSERT INTO [dbo].[AssetTypeFileExtension] ([AssetTypeFileExtensionId], [Extension], [Name], [AssetTypeId], [IconImage], [IconFilename], [IsVisible], [Plugin]) VALUES (35, 'indd', 'indd', 12, NULL, '', 1, NULL)
SET IDENTITY_INSERT [dbo].[AssetTypeFileExtension] OFF
-- Operation applied to 35 rows out of 35

-- Add row to [dbo].[Brand]
SET IDENTITY_INSERT [dbo].[Brand] ON
INSERT INTO [dbo].[Brand] ([BrandId], [Name], [ShortName], [ApplicationName], [OrganisationName], [IsMasterBrand], [WebsiteUrl], [EmailFrom], [IsBrandSelectionAllowed], [DisablePoweredByLogo], [LoginPageUpperCopy], [LoginPageLowerCopy], [DefaultUsageRestrictionsCopy], [MyAccountCopy], [AdminCopy], [TermsConditionsCopy], [PrivacyPolicyCopy], [HideFilterSearch], [HideCategorySearch], [DirectDownloadEnabled], [IsDeleted], [FilterMarkup]) VALUES (1, 'FocusOPEN', 'FocusOPEN', 'FocusOPEN Digital Asset Manager', 'Daydream', 1, 'http://localhost/FocusOPEN', 'do-not-reply@focusopen.net', 1, 0, 'Enter your email and password below. If you have forgotten your password, enter your email address below and press ''forgot password?'' to have it emailed to you.', 'This facility is not available to the general public. It is for the sole use of employees and approved suppliers. Attempting unauthorised access to this facility is forbidden and may result in action being taken against you.', N'Default Usage Restrictions', N'Welcome to FocusOPEN, our resource for storing, sharing and managing images and video. For help using this website please refer to the online help or email your system administrator.', N'The FocusOPEN Digital Asset Manager Administration Area allows users to manage the system and media assets. Use the menu above to access those features available to you. For further help, please refer to the online help or email your system administrator.', N'Terms and conditions', N'Privacy Policy', 0, 0, 1, 0, N'')
SET IDENTITY_INSERT [dbo].[Brand] OFF

-- Add rows to [dbo].[BrandMetadataSetting]
SET IDENTITY_INSERT [dbo].[BrandMetadataSetting] ON
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (1, 1, 'DateUploaded', N'Date Uploaded', 0, 1, 1, N'', N'The date the asset was uploaded', 0, 0, 1, 1)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (2, 1, 'Filename', N'Filename!!', 0, 1, 1, N'', N'The name of the file', 0, 0, 2, 2)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (3, 1, 'FileHash', N'File Hash', 0, 1, 0, N'', N'', 0, 0, 3, 3)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (4, 1, 'FileSize', N'File Size', 0, 1, 1, N'', N'', 0, 0, 4, 4)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (5, 1, 'AssetType', N'Asset Type', 1, 1, 1, N'', N'The type of the asset', 0, 0, 5, 5)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (6, 1, 'DateProduced', N'Date Produced', 1, 1, 1, N'', N'', 0, 0, 6, 6)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (7, 1, 'Originator', N'Originator', 0, 1, 1, N'', N'', 0, 0, 7, 7)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (8, 1, 'Title', N'Title', 1, 1, 1, N'', N'', 0, 0, 8, 8)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (9, 1, 'ProjectCode', N'Project Code', 1, 1, 1, N'', N'', 0, 0, 9, 9)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (10, 1, 'Brand', N'Brand', 1, 1, 1, N'', N'', 0, 0, 10, 10)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (11, 1, 'Metadata1', N'Asset Purpose', 1, 1, 1, N'', N'', 3, 1, 11, 11)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (12, 1, 'Metadata2', N'Target Group', 1, 1, 1, N'', N'', 3, 1, 12, 12)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (13, 1, 'Metadata3', N'Campaign', 1, 1, 1, N'', N'', 3, 1, 13, 13)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (14, 1, 'Metadata4', N'Service', 1, 1, 1, N'', N'', 3, 1, 14, 14)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (15, 1, 'Metadata5', N'Industry', 1, 1, 1, N'', N'', 3, 1, 15, 15)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (16, 1, 'Metadata6', N'Sector', 1, 1, 1, N'', N'', 3, 1, 16, 16)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (17, 1, 'Metadata7', N'Zone', 1, 1, 1, N'', N'', 3, 1, 17, 17)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (18, 1, 'Metadata8', N'Skills', 1, 1, 1, N'', N'', 3, 1, 18, 18)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (19, 1, 'Metadata9', N'Metadata1_9', 0, 1, 1, N'', N'', 3, 1, 19, 19)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (20, 1, 'Metadata10', N'Metadata1_10', 0, 0, 0, N'', N'', 3, 1, 20, 20)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (21, 1, 'Metadata11', N'Metadata1_11', 0, 0, 0, N'', N'', 3, 1, 21, 21)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (22, 1, 'Metadata12', N'Metadata1_12', 0, 0, 0, N'', N'', 3, 1, 22, 22)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (23, 1, 'Categories', N'Categories', 1, 1, 1, N'', N'', 0, 0, 23, 23)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (24, 1, 'PrimaryCategory', N'Primary Category', 1, 1, 1, N'', N'', 0, 0, 24, 24)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (25, 1, 'ShortDescription', N'Short Description', 1, 1, 1, N'', N'', 0, 0, 25, 25)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (26, 1, 'AdditionalKeywords', N'Additional Keywords', 0, 1, 0, N'', N'', 0, 0, 26, 26)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (27, 1, 'CopyrightOwner', N'Copyright Owner', 1, 1, 1, N'', N'', 0, 0, 27, 27)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (28, 1, 'UsageRestrictions', N'Usage Restrictions', 1, 1, 1, N'', N'', 0, 0, 28, 28)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (29, 1, 'AttachedFiles', N'Attached Files', 0, 1, 1, N'', N'', 0, 0, 29, 29)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (30, 1, 'DownloadRestrictions', N'Download Restrictions', 0, 1, 1, N'', N'', 0, 0, 30, 30)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (31, 1, 'ContactEmail', N'Contact Email', 1, 1, 1, N'', N'', 0, 0, 31, 31)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (32, 1, 'PublicationDate', N'Publication Date', 1, 1, 0, N'', N'', 0, 0, 32, 32)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (33, 1, 'ExpiryDate', N'Expiry Date', 1, 1, 0, N'', N'', 0, 0, 33, 33)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (34, 1, 'LinkedAssets', N'Linked Assets', 0, 1, 1, N'', N'', 0, 0, 34, 34)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (35, 1, 'FeedbackForm', N'Feedback Form', 0, 1, 1, N'', N'', 0, 0, 35, 35)
INSERT INTO [dbo].[BrandMetadataSetting] ([BrandMetadataSettingId], [BrandId], [FieldId], [FieldName], [IsRequired], [OnAssetForm], [OnAssetDetail], [AdditionalCopy], [ToolTip], [UiControlType], [IsCustom], [CatalogueOrderNum], [AssetDetailOrderNum]) VALUES (36, 1, 'ShowFileMetadata', N'Show File Metadata', 0, 1, 0, N'', N'', 0, 0, 36, 36)
SET IDENTITY_INSERT [dbo].[BrandMetadataSetting] OFF
-- Operation applied to 36 rows out of 36

-- Add row to [dbo].[Company]
SET IDENTITY_INSERT [dbo].[Company] ON
INSERT INTO [dbo].[Company] ([CompanyId], [Name], [Brands], [Domain], [IsInternal], [CreatedByUserId], [CreateDate]) VALUES (0, 'External Company', N'', '', 0, 1, '1900-01-01 00:00:00.000')
SET IDENTITY_INSERT [dbo].[Company] OFF

-- Add row to [dbo].[Homepage]
SET IDENTITY_INSERT [dbo].[Homepage] ON
INSERT INTO [dbo].[Homepage] ([HomepageId], [BrandId], [IntroText], [Url1], [Url2], [Url3], [Url4], [BumperPageHtml], [BumperPageSkip], [CustomHtml], [HomepageTypeId], [IsPublished], [LastModifiedByUserId], [LastModifiedDate]) VALUES (1, 1, '<p align="left"><strong>This space available for introductory copy </strong></p><p align="left">Messages for users and <a href="http://www.digitalassetmanager.com" title="This is a hyperlink">hyperlinks</a> can be included in both this copy and the images to the right.</p>', '', '', '', '', N'', 0, N'', 1, 1, 1, '1900-01-01 00:00:00.000')
SET IDENTITY_INSERT [dbo].[Homepage] OFF

-- Add rows to [dbo].[HomepageType]
INSERT INTO [dbo].[HomepageType] ([HomepageTypeId], [Description], [ShortName]) VALUES (1, 'Standard', 'Standard')
INSERT INTO [dbo].[HomepageType] ([HomepageTypeId], [Description], [ShortName]) VALUES (2, 'Custom HTML - No Navigation', 'CustomHTML_NoNav')
INSERT INTO [dbo].[HomepageType] ([HomepageTypeId], [Description], [ShortName]) VALUES (3, 'Custom HTML - With Navigation', 'CustomHTML_WithNav')
-- Operation applied to 3 rows out of 3

-- Add row to [dbo].[Metadata]
SET IDENTITY_INSERT [dbo].[Metadata] ON
INSERT INTO [dbo].[Metadata] ([MetadataId], [BrandId], [ParentMetadataId], [Name], [ExternalRef], [Synonyms], [GroupNumber], [IsDeleted], [MetadataOrder]) VALUES (0, NULL, NULL, N'Not Specified', '', '', 0, 0, 1)
SET IDENTITY_INSERT [dbo].[Metadata] OFF

-- Add rows to [dbo].[Plugins]
SET IDENTITY_INSERT [dbo].[Plugins] ON
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (1, '198818c2-6012-486c-a5bd-e40a67dcfe71', 'Images', 'Image Preview.xml', 'Image Preview', -1814607180, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (2, '6101ce70-f2ea-4a8b-ae6e-900f8795f248', 'MP3', 'MP3 Preview.xml', 'MP3 Preview', 509845421, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (3, '69971c70-6415-46bf-aa76-dbadda5b4f06', 'Flash', 'Flash Preview.xml', 'Flash Preview', 761263218, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (4, '81fb4b97-5b58-441a-89a3-df0708862dc7', 'FlowPlayer', 'FlowPlayer Preview.xml', 'Flowplayer Preview', -2004403416, NULL, NULL)
INSERT INTO [dbo].[Plugins] ([PluginId], [RegistrationKey], [RelativePath], [Filename], [Name], [Checksum], [PluginType], [IsDefault]) VALUES (5, '3445ec3c-3c66-4f05-8ddb-193cfd8078cf', 'FlowPlayer_Streaming', 'FlowPlayer_Streaming.xml', 'Flowplayer Streaming Preview', -559134806, NULL, NULL)
SET IDENTITY_INSERT [dbo].[Plugins] OFF
-- Operation applied to 5 rows out of 5

-- Add row to [dbo].[User]
SET IDENTITY_INSERT [dbo].[User] ON
INSERT INTO [dbo].[User] ([UserId], [FirstName], [LastName], [Email], [Password], [PasswordSalt], [PasswordExpiryDate], [IsPasswordNonExpiring], [IsEmployee], [IsEngineer], [CompanyName], [PrimaryBrandId], [CountryId], [PhoneNumber], [MobileNumber], [RegisterDate], [LastLoginDate], [AccountExpiryDate], [IsAccountNonExpiring], [IsSuspended], [UserRoleId], [UserStatusId], [UseWorkflow], [WorkflowId], [Notes], [IsAllowedExternalAccess], [ApproverUserId], [CatalogueAssetIdList], [IsDeleted], [Guid], [EnableFilePathIngestion], [SessionAPIToken], [LastAPIOperation], [UserAPIToken]) VALUES (1, 'Super', 'Admin', 'admin@example.com', '681aa98c0a64b90192c41d883f18bccd', 'ABCDEF', '1900-01-01 00:00:00.000', 0, 1, 0, 'Example', 1, 222, '0', '0', '1900-01-01 00:00:00.000', NULL, NULL, 0, 0, 4, 3, 0, NULL, '', 0, NULL, '', 0, '', 0, '', NULL, 'd76b2e8ce6deb8a38c24f607253777b1')
SET IDENTITY_INSERT [dbo].[User] OFF

-- Add constraints to [dbo].[User]
ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_Brand] FOREIGN KEY ([PrimaryBrandId]) REFERENCES [dbo].[Brand] ([BrandId])
ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_Country] FOREIGN KEY ([CountryId]) REFERENCES [dbo].[Country] ([CountryId])
ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_UserRole] FOREIGN KEY ([UserRoleId]) REFERENCES [dbo].[UserRole] ([UserRoleId])
ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_UserStatus] FOREIGN KEY ([UserStatusId]) REFERENCES [dbo].[UserStatus] ([UserStatusId])
ALTER TABLE [dbo].[User] ADD CONSTRAINT [FK_User_Workflow] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflow] ([WorkflowId])

-- Add constraints to [dbo].[Metadata]
ALTER TABLE [dbo].[Metadata] ADD CONSTRAINT [FK_Metadata_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId])

-- Add constraints to [dbo].[Homepage]
ALTER TABLE [dbo].[Homepage] ADD CONSTRAINT [FK_Homepage_HomepageType] FOREIGN KEY ([HomepageTypeId]) REFERENCES [dbo].[HomepageType] ([HomepageTypeId])
ALTER TABLE [dbo].[Homepage] ADD CONSTRAINT [FK_Homepage_User] FOREIGN KEY ([LastModifiedByUserId]) REFERENCES [dbo].[User] ([UserId])

-- Add constraints to [dbo].[CompanyBrand]
ALTER TABLE [dbo].[CompanyBrand] ADD CONSTRAINT [FK_CompanyBrand_Brand] FOREIGN KEY ([BrandId]) REFERENCES [dbo].[Brand] ([BrandId])
ALTER TABLE [dbo].[CompanyBrand] ADD CONSTRAINT [FK_CompanyBrand_Company] FOREIGN KEY ([CompanyId]) REFERENCES [dbo].[Company] ([CompanyId]) ON DELETE CASCADE

-- Add constraints to [dbo].[Company]
ALTER TABLE [dbo].[Company] ADD CONSTRAINT [FK_Company_User] FOREIGN KEY ([CreatedByUserId]) REFERENCES [dbo].[User] ([UserId])

-- Add constraints to [dbo].[AssetTypeFileExtension]
ALTER TABLE [dbo].[AssetTypeFileExtension] ADD CONSTRAINT [FK_AssetTypeFileExtension_AssetType] FOREIGN KEY ([AssetTypeId]) REFERENCES [dbo].[AssetType] ([AssetTypeId])
COMMIT TRANSACTION
GO
