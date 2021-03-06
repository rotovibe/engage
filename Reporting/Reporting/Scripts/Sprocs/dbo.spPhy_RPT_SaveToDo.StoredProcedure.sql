SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveToDo] 
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
	@ExtraElements		varchar(MAX)
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
				ExtraElements =		@ExtraElements     
			
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
				ExtraElements
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
				@ExtraElements
			)
		End
END
GO
