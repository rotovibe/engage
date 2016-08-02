/***************** TO DO METRICS *******************/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Dim_ToDo]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_Dim_ToDo]
GO

CREATE TABLE [dbo].[RPT_Dim_ToDo](
	[DimToDoId] [int] IDENTITY(1,1) NOT NULL,
	[MongoPatientId] [varchar](50) NULL,
	[MongoToDoId] [varchar](50) NULL,
	[Source] [varchar](200) NULL,
	[Category] [varchar](100) NULL,
	[Status] [varchar](100) NULL,
	[AssignedTo] [varchar](1000) NULL,
	[DueDate] datetime NULL,
	[Priority] varchar(200) NULL,
	[Related Program] varchar(2000) NULL,
	[Title] varchar(900) NULL,
	[Description] varchar(900) NULL,
	[RecordCreatedOn] datetime NULL,
	[RecordCreatedBy] varchar(200) NULL	,
	[UpdatedBy] varchar(2000) NULL,
	[LastUpdatedOn]	datetime NULL
) ON [PRIMARY]

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_ToDo_Dim]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 05/15/2015 16:33:17 ******/
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_Dim_ToDo]
	INSERT INTO [RPT_Dim_ToDo]
	(
		[MongoPatientId],
		[MongoToDoId],
		[Source],
		[Category],
		[Related Program],
		[Status],
		[AssignedTo],		
		[Title],
		[Description],
		[DueDate],
		[Priority],		
		[RecordCreatedOn],
		[RecordCreatedBy],
		[LastUpdatedOn],
		[UpdatedBy]
	)
	SELECT
		(CASE WHEN td.MongoPatientId = '' THEN  NULL ELSE td.MongoPatientId END) as [MongoPatientId],
		td.MongoId as [MongoToDoId],
		(CASE WHEN td.MongoSourceId = '000000000000000000000000' THEN  NULL ELSE td.MongoSourceId END) as [Source],
		(select Name from RPT_ToDoCategoryLookUp where MongoId = td.MongoCategory) as [Category],
		(select Name from RPT_PatientProgram where MongoId = tdp.MongoProgramId) as [Related Program],
		td.[Status],
		(select PreferredName  from RPT_User where MongoId = td.MongoAssignedToId) as [AssignedTo],
		td.[Title],
		(CASE WHEN td.[Description] = '' THEN  NULL ELSE td.[Description] END) as [Description],
		td.DueDate,
		td.[Priority],
		td.RecordCreatedOn,
		(select PreferredName  from RPT_User where MongoId = td.MongoRecordCreatedBy) as [RecordCreatedBy],
		td.LastUpdatedOn,
		(select PreferredName from RPT_User where MongoId = td.MongoUpdatedBy) as [UpdatedBy]
	FROM 
		RPT_ToDo as td
		LEFT JOIN RPT_ToDoProgram tdp ON td.MongoId = tdp.MongoToDoId
	WHERE
		td.[DeleteFlag] = 'False'	
		AND td.TTLDate IS NULL
END
GO

/*** RPT_SprocName ***/
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_ToDo_Dim', 'false');