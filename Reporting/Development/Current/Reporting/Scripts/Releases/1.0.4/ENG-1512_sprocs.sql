IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveToDo]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveToDo]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveToDo] 
	@MongoId				varchar(50),
	@MongoSourceId			varchar(50),
	@MongoPatientId			varchar(50),
	@MongoAssignedToId		varchar(50),
	@Description		varchar(500),
	@Title				varchar(500),
	@DueDate			datetime,
	@ClosedDate			datetime,
	@Status				varchar(50),
	@MongoCategory			varchar(50),
	@Priority			varchar(50),
	@MongoUpdatedBy			varchar(50),
	@LastUpdatedOn		datetime,
	@MongoRecordCreatedBy	varchar(50),
	@RecordCreatedOn	datetime,
	@Version			float,
	@TTLDate			datetime,
	@DeleteFlag			varchar(50),
	@ExtraElements		varchar(MAX),
	@StartTime time,
	@Duration int
AS
BEGIN
	SET NOCOUNT ON;
	
	Declare @PatientId			int,
			@AssignedToId		int,
			@ToDoCategoryId		int,
			@RecordCreatedById	int,
			@UpdatedById		int
	
	Select @PatientId = PatientId  from RPT_Patient Where MongoId = @MongoPatientId
	
	If @MongoAssignedToId != ' '
		Select @AssignedToId = u.UserId From [RPT_User] u Where MongoId = @MongoAssignedToId

	If @MongoCategory != ' '
		Select @ToDoCategoryId = ToDoCategoryId From RPT_ToDoCategoryLookUp Where MongoId = @MongoCategory
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	
	If Exists(Select Top 1 1 From RPT_ToDo Where MongoId = @MongoId)
		Begin
			Update RPT_ToDo
			Set 
				MongoId =				@MongoId,				
				MongoSourceId =			@MongoSourceId,			
				MongoPatientId =		@MongoPatientId,		
				PatientId =			@PatientId,			
				MongoAssignedToId =		@MongoAssignedToId,		
				AssignedToId =		@AssignedToId,		
				[Description] =		@Description,			
				Title =				@Title,			
				DueDate =			@DueDate,				
				ClosedDate =		@ClosedDate,		
				[Status] =			@Status,				
				MongoCategory =			@MongoCategory,			
				ToDoCategoryId =	@ToDoCategoryId,	
				[Priority] =		@Priority,			
				MongoUpdatedBy =		@MongoUpdatedBy,		
				UpdatedById =		@UpdatedById,			
				LastUpdatedOn =		@LastUpdatedOn,		
				MongoRecordCreatedBy =	@MongoRecordCreatedBy,	
				RecordCreatedById =	@RecordCreatedById,
				RecordCreatedOn =	@RecordCreatedOn,
				[Version] =			@Version,		
				TTLDate =			@TTLDate,			
				DeleteFlag =		@DeleteFlag,
				ExtraElements =		@ExtraElements,    
				StartTime =		@StartTime,     
				Duration =		@Duration     
			
			Where MongoId = @MongoId
		End
	Else
		Begin
			Insert Into RPT_ToDo 
			(
				MongoId,				
				MongoSourceId,			
				MongoPatientId,			
				PatientId,			
				MongoAssignedToId,		
				AssignedToId,		
				[Description],			
				Title,			
				DueDate,				
				ClosedDate,			
				[Status],				
				MongoCategory,			
				ToDoCategoryId,		
				[Priority],			
				MongoUpdatedBy,			
				UpdatedById,			
				LastUpdatedOn,		
				MongoRecordCreatedBy,	
				RecordCreatedById,	
				RecordCreatedOn,
				[Version],		
				TTLDate,			
				DeleteFlag,
				ExtraElements,
				StartTime,
				Duration
			) 
			values 
			(
				@MongoId,				
				@MongoSourceId,			
				@MongoPatientId,			
				@PatientId,			
				@MongoAssignedToId,		
				@AssignedToId,		
				@Description,			
				@Title,			
				@DueDate,				
				@ClosedDate,			
				@Status,				
				@MongoCategory,			
				@ToDoCategoryId,		
				@Priority,			
				@MongoUpdatedBy,			
				@UpdatedById,			
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,	
				@RecordCreatedById,	
				@RecordCreatedOn,
				@Version,		
				@TTLDate,			
				@DeleteFlag,
				@ExtraElements,
				@StartTime,
				@Duration
			)
		End
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_ToDo_Dim]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_ToDo_Dim]
GO

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
		[UpdatedBy],
		[StartTime],
		[Duration]
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
		(select PreferredName from RPT_User where MongoId = td.MongoUpdatedBy) as [UpdatedBy],
		td.StartTime,
		td.Duration
	FROM 
		RPT_ToDo as td
		LEFT JOIN RPT_ToDoProgram tdp ON td.MongoId = tdp.MongoToDoId
	WHERE
		td.[DeleteFlag] = 'False'	
		AND td.TTLDate IS NULL
END

GO