-----------------------------------------------------------------------------------------------------------------------
-- Create Tables
-----------------------------------------------------------------------------------------------------------------------

CREATE TABLE [APSDebugLog] (
[APSDebugLogId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[Date] datetime  NOT NULL,
[Thread] varchar  NOT NULL,
[Level] varchar  NOT NULL,
[Logger] varchar  NOT NULL,
[Message] varchar  NOT NULL
);

CREATE TABLE [APSLog] (
[LogId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[JobId] INTEGER  NOT NULL,
[Message] VARCHAR(5000)  NOT NULL,
[Date] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE [APSQueuedJob] (
[QueuedJobId] INTEGER  PRIMARY KEY AUTOINCREMENT NOT NULL,
[AssetId] INTEGER  NOT NULL,
[InputPath] VARCHAR(1000)  NOT NULL,
[WatermarkPath] VARCHAR(1000)  NULL,
[PluginName] VARCHAR(200)  NOT NULL,
[CreateThumbnail] BOOLEAN  NOT NULL,
[CreatePreview] BOOLEAN  NOT NULL,
[OverrideHeight] INTEGER  NOT NULL,
[OverrideWidth] INTEGER  NOT NULL,
[DateAdded] TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
[DateProcessed] TIMESTAMP  NULL,
[QueuedJobStatusId] INTEGER  NOT NULL,
[Message] VARCHAR(5000)  NOT NULL,
[CallbackUrl] VARCHAR(1000)  NOT NULL,
[AdditionalData] VARCHAR(5000)  NOT NULL
);

CREATE TABLE [APSQueuedJobStatus] (
[QueuedJobStatusId] INTEGER  PRIMARY KEY NOT NULL,
[Name] VARCHAR(100)  NOT NULL
);

-----------------------------------------------------------------------------------------------------------------------
-- Create Views
-----------------------------------------------------------------------------------------------------------------------
CREATE VIEW [v_APSQueuedJob] AS 
	SELECT QJ.*, QJS.Name AS QueuedJobStatus
	FROM APSQueuedJob QJ INNER JOIN APSQueuedJobStatus QJS ON QJ.QueuedJobStatusId = QJS.QueuedJobStatusId;


-----------------------------------------------------------------------------------------------------------------------
-- Insert Data
-----------------------------------------------------------------------------------------------------------------------
INSERT INTO [APSQueuedJobStatus] ([QueuedJobStatusId], [Name]) VALUES (1, 'Not Started');
INSERT INTO [APSQueuedJobStatus] ([QueuedJobStatusId], [Name]) VALUES (2, 'Processing');
INSERT INTO [APSQueuedJobStatus] ([QueuedJobStatusId], [Name]) VALUES (3, 'Completed Successfully');
INSERT INTO [APSQueuedJobStatus] ([QueuedJobStatusId], [Name]) VALUES (4, 'Completed With Errors');