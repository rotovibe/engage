﻿----------------------------------------------------------------------------------------------------------------------------------
--ENG-936
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_TruncateTables]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE RPT_CareMember
	DELETE RPT_CareMemberTypeLookUp
	DELETE RPT_ContactEmail
	DELETE RPT_ContactPhone
	DELETE RPT_ContactAddress
	DELETE RPT_ContactRecentList
	DELETE RPT_ContactMode
	DELETE RPT_ContactLanguage
	DELETE RPT_ContactWeekDay
	DELETE RPT_ContactTimeOfDay
	-- todos
	DELETE RPT_ToDoProgram
	DELETE RPT_ToDo
	-- patient programs
	DELETE RPT_SpawnElements
	DELETE RPT_SpawnElementTypeCode
	DELETE RPT_PatientProgramAttribute
	DELETE RPT_PatientProgramResponse
	DELETE RPT_PatientProgramStep
	DELETE RPT_PatientProgramAction	
	DELETE RPT_PatientProgramModule
	DELETE RPT_PatientProgram	
	DELETE RPT_PatientNoteProgram
	DELETE RPT_PatientNote
	DELETE RPT_PatientProblem
	DELETE RPT_ObjectiveCategory
	DELETE RPT_ObjectiveLookUp	
	DELETE RPT_PatientObservation	
	DELETE RPT_Observation
	DELETE RPT_PatientTaskBarrier
	DELETE RPT_PatientTask
	-- patient allergies
	DELETE RPT_Allergy
	DELETE RPT_AllergyType
	DELETE RPT_PatientAllergy
	DELETE RPT_PatientAllergyReaction
	-- patient medsupps
	DELETE RPT_PatientMedSuppPhClass
	DELETE RPT_MedPharmClass
	DELETE RPT_PatientMedSuppNDC
	DELETE RPT_PatientMedSupp	
	DELETE RPT_Medication
	DELETE RPT_MedicationMap
	DELETE RPT_PatientMedFrequency
	DELETE RPT_CustomPatientMedFrequency
	
	-- patient goal
	DELETE RPT_PatientGoalProgram
	DELETE RPT_PatientGoalFocusArea
	DELETE RPT_GoalAttributeOption	
	DELETE RPT_PatientGoalAttributeValue
	DELETE RPT_PatientGoalAttribute
	DELETE RPT_GoalAttribute
	DELETE RPT_PatientInterventionBarrier
	DELETE RPT_PatientIntervention	
	DELETE RPT_PatientBarrier	
	DELETE RPT_PatientGoal
	DELETE RPT_PatientUser	
	DELETE RPT_Contact
	DELETE RPT_System
	DELETE RPT_PatientSystem
	DELETE RPT_Patient
	DELETE RPT_CommTypeCommMode
	DELETE RPT_ToDoCategoryLookUp	
	DELETE RPTMongoCategoryLookUp
	DELETE RPT_SourceLookUp
	DELETE RPT_BarrierCategoryLookUp
	DELETE RPT_InterventionCategoryLookUp
	DELETE RPTMongoTimeZoneLookUp
	DELETE RPT_ProblemLookUp
	DELETE RPT_TimesOfDayLookUp
	DELETE RPT_CommTypeLookUp
	DELETE RPT_CommModeLookUp
	DELETE RPT_StateLookUp
	DELETE RPT_LanguageLookUp
	DELETE RPT_FocusAreaLookUp
	DELETE RPT_CodingSystemLookUp
	DELETE RPT_ObservationTypeLookUp
	DELETE RPT_AllergyTypeLookUp
	DELETE RPT_AllergySourceLookUp
	DELETE RPT_SeverityLookUp
	DELETE RPT_ReactionLookUp
	DELETE RPT_MedSupTypeLookUp
	DELETE RPT_FreqHowOftenLookUp
	DELETE RPT_FreqWhenLookUp
	DELETE RPT_NoteTypeLookUp
	DELETE RPT_UserRecentList
	DELETE [RPT_User]
	
	DELETE RPT_PatientUtilization
	DELETE RPT_PatientUtilizationProgram

	--Delete the lookup columns in the end due to it's dependencies on base tables.

	-- Notes Lookups
	DELETE RPT_NoteMethodLookUp
	DELETE RPT_NoteOutcomeLookUp
	DELETE RPT_NoteSourceLookUp
	DELETE RPT_NoteWhoLookUp
	DELETE RPT_MaritalStatusLookUp
	DELETE RPT_StatusReasonLookUp
	-- Utilization lookups
	DELETE RPT_VisitTypeLookUp
	DELETE RPT_UtilizationLocationLookUp
	DELETE RPT_DispositionLookUp
	DELETE RPT_UtilizationSourceLookUp

	-- Resetting the Identity columns.
	
	DBCC CHECKIDENT ('RPT_CareMember', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_CareMemberTypeLookUp', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_ContactLanguage', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactWeekDay', RESEED, 0)  
	DBCC CHECKIDENT ('RPT_ContactTimeOfDay', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactRecentList', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_ContactMode', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactPhone', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactAddress', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactEmail', RESEED, 0)
	
-- reseed program tables
	DBCC CHECKIDENT ('RPT_PatientProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SpawnElements', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramModule', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAction', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramStep', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramResponse', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAttribute', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_PatientNoteProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientNote', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProblem', RESEED, 0)

	-- allergies
	DBCC CHECKIDENT ('RPT_AllergyType', RESEED, 0)	
	DBCC CHECKIDENT ('RPT_Allergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedicationMap', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Medication', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CustomPatientMedFrequency', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedPharmClass', RESEED,0)
	
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteTypeLookUp', RESEED, 0)

	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTask', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientGoalProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalFocusArea', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttributeOption', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttribute', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientInterventionBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientIntervention', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoal', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUser', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Contact', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_System', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientSystem', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_Patient', RESEED, 0) 
	
	DBCC CHECKIDENT ('RPT_CommTypeCommMode', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_BarrierCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_InterventionCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoTimeZoneLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ProblemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_TimesOfDayLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommModeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StateLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_LanguageLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FocusAreaLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CodingSystemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObservationTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_UserRecentList', RESEED, 0)
	DBCC CHECKIDENT ('RPT_User', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_ToDoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDoProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDo', RESEED, 0)
	
	-- patient allergies
	DBCC CHECKIDENT ('RPT_PatientAllergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientAllergyReaction', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientUtilization', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUtilizationProgram', RESEED, 0)

	-- lookups
	DBCC CHECKIDENT ('RPT_NoteMethodLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteOutcomeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteSourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_NoteWhoLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MaritalStatusLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StatusReasonLookUp', RESEED, 0)
	-- utilization lookups
	DBCC CHECKIDENT ('RPT_VisitTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_UtilizationLocationLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_DispositionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_UtilizationSourceLookUp', RESEED, 0)

END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientTaskAttribute]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientTaskAttribute]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientTaskAttributeValue]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientTaskAttributeValue]
GO


----------------------------------------------------------------------------------------------------------------------------------
--ENG-1354
----------------------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Transition]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Transition]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Transition]
AS
BEGIN
	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '566f8316ac80d3662b00036f';
	
	Truncate Table [RPT_Flat_Transition_Info]

	INSERT INTO [RPT_Flat_Transition_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		Enrollment_Status,
		Enrolled_Date,
		Did_Not_Enroll_Date,
		Did_Not_Enroll_Reason,
		Disenroll_Date,
		Disenroll_Reason,
		Program_Completed_Date,
		Call_Within_48Hours_PostDischarge,
		Discharge_Type,
		Total_LACE_Score,
		High_Risk_For_Readmission,
		Program_Status
	)

	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId as MongoPatientId
		,ppt.PatientProgramId
		,ppt.MongoId as MongoPatientProgramId
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd419ac80d329bf000064')) as [Enrollment_Status]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd477ac80d329bf000065') ) as [Enrolled_Date]     	     	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd48eac80d32a86000081') ) as [Did_Not_Enroll_Date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd49fac80d329bf000066')) as [Did_Not_Enroll_Reason]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55177225ac80d37c8000038c', '54fdd51eac80d32a86000082')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55177225ac80d37c8000038c', '54fdd530ac80d329bf000069') ) as [Disenroll_Reason] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55177225ac80d37c8000038c', '54fdd649ac80d329bf00006b')) as [Program_Completed_Date]     	       	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d2a8ac80d308d000031e', '55a576f5ac80d308d00001e7')) as [Call_Within_48Hours_PostDischarge]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d2a8ac80d308d000031e', '55a7bce0ac80d308d20002dc')) as [Discharge_Type]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d828ac80d308d0000373', '55a57377ac80d308d2000126')) as [Total_LACE_Score]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55a7d828ac80d308d0000373', '55a575ffac80d308d00001e6')) as [High_Risk_For_Readmission]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55f0d598ac80d37770000156', '55f0d4fbac80d377730001e6')) as [Program_Status]
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.MongoId = ppt.MongoPatientId	
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'

END
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1559
----------------------------------------------------------------------------------------------------------------------------------
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoal]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientGoal]
	@MongoId varchar(50),
	@MongoPatientId varchar(50),
	@Name varchar(500),
	@Description varchar(50),
	@StartDate datetime,
	@EndDate datetime,
	@Source varchar(50),
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(500),
	@Type varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(5000),
	@TemplateId varchar(50),
	@Details varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @PatientId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @MongoPatientId

		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end
	
	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientGoal Where MongoId = @MongoId)
	Begin
		Update RPT_PatientGoal
		Set Name = @Name,
			StartDate = @StartDate,
			EndDate = @EndDate,
			[Source] = @Source,
			[Status] = @Status,
			[Description] = @Description,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			[Type] = @Type,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			[Delete] = @Delete,
			TTLDate = @TimeToLive,
			PatientId = @PatientId,
			MongoPatientId = @MongoPatientId,
			ExtraElements = @ExtraElements,
			TemplateId = @TemplateId,
			Details = @Details
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert into RPT_PatientGoal(
			Name, 
			StartDate, 
			EndDate, 
			[Source], 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue, 
			[Type], 
			UpdatedBy,
			MongoUpdatedBy, 
			LastUpdatedOn, 
			RecordCreatedBy, 
			MongoRecordCreatedBy,
			RecordCreatedOn, 
			[Version], 
			[Delete], 
			TTLDate,
			 MongoId, 
			 PatientId,
			 MongoPatientId,
			 [Description],
			 ExtraElements,
			 TemplateId,
			 Details
			 ) 
		 values (
			 @Name, 
			 @StartDate, 
			 @EndDate, 
			 @Source, 
			 @Status, 
			 @StatusDate,
			 @TargetDate, 
			 @TargetValue, 
			 @Type, 
			 @UpdatedBy,
			 @MongoUpdatedBy, 
			 @LastUpdatedOn, 
			 @RecordCreatedBy,
			 @MongoRecordCreatedBy, 
			 @RecordCreatedOn, 
			 @Version, 
			 @Delete, 
			 @TimeToLive, 
			 @MongoId, 
			 @PatientId,
			 @MongoPatientId,
			 @Description,
			 @ExtraElements,
			 @TemplateId,
			 @Details
		 )
	End
END

GO

DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientBarrier]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientBarrier] 
	@MongoID varchar(50),
	@PatientGoalMongoId varchar(50),
	@MongoCategoryLookUpId varchar(50),
	@Delete varchar(50),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TimeToLive datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@StartDate datetime,
	@Name varchar(500),
	@ExtraElements varchar(MAX),
	@Details varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@RecordCreatedById INT,
			@UpdatedById INT,
			@CategoryLookUpId INT
			
	Select @PatientGoalId = PatientGoalId from RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId from [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId from [RPT_User] Where MongoId = @UpdatedBy
	
	If @MongoCategoryLookUpId != ' '
	Select @CategoryLookUpId = BarrierCategoryId From RPT_BarrierCategoryLookUp Where MongoId = @MongoCategoryLookUpId
	
	If Exists(Select Top 1 1 From RPT_PatientBarrier Where MongoId = @MongoID)
	Begin
		Update RPT_PatientBarrier
		Set [Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @PatientGoalMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TTLDate = @TimeToLive,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			StartDate = @StartDate,
			Name = @Name,
			CategoryLookUpId = @CategoryLookUpId,
			MongoCategoryLookUpId = @MongoCategoryLookUpId,
			ExtraElements = @ExtraElements,
			Details = @Details
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientBarrier(CategoryLookUpId, MongoCategoryLookUpId, ExtraElements, StartDate, Name, [Delete], LastUpdatedOn, PatientGoalId, MongoPatientGoalId, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Status], StatusDate, TTLDate, MongoUpdatedBy, UpdatedById, [Version], MongoId, Details) values (@CategoryLookUpId, @MongoCategoryLookUpId, @ExtraElements, @StartDate, @Name, @Delete, @LastUpdatedOn, @PatientGoalId, @PatientGoalMongoId, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Status, @StatusDate, @TimeToLive, @UpdatedBy, @UpdatedById, @Version, @MongoID, @Details)
	End
END

GO

DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientIntervention]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientIntervention] 
	@MongoID varchar(50),
	@PatientGoalMongoId varchar(50),
	@MongoCategoryLookUpId varchar(50),
	@AssignedTo varchar(50),
	@Delete varchar(50),
	@Description varchar(5000),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TimeToLive datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@Name varchar(100),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50),
	@DueDate datetime,
	@Details varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@CategoryLookUpId INT,
			@RecordCreatedById INT,
			@UpdatedById INT,
			@UserId INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	
	Select @CategoryLookUpId = InterventionCategoryId From RPT_InterventionCategoryLookUp Where MongoId = @MongoCategoryLookUpId
	Select @UserId = UserId From [RPT_User] Where MongoId = @AssignedTo
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientIntervention Where MongoId = @MongoID)
	Begin
		Update RPT_PatientIntervention
		Set AssignedToUserId = @UserId,
			MongoContactUserId = @AssignedTo,
			CategoryLookUpId = @CategoryLookUpId,
			MongoCategoryLookUpId = @MongoCategoryLookUpId,
			[Delete] = @Delete,
			[Description] = @Description,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @PatientGoalMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TTLDate = @TimeToLive,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			Name = @Name,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId,
			DueDate = @DueDate,
			Details = @Details
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientIntervention(
		Name, 
		ExtraElements, 
		AssignedToUserId, 
		MongoContactUserId, 
		CategoryLookUpId, 
		MongoCategoryLookUpId, 
		[Delete], 
		[Description], 
		LastUpdatedOn, 
		PatientGoalId, 
		MongoPatientGoalId, 
		MongoRecordCreatedBy, 
		RecordCreatedById, 
		RecordCreatedOn, 
		StartDate, 
		[Status], 
		StatusDate, 
		TTLDate, 
		MongoUpdatedBy, 
		UpdatedById, 
		[Version], 
		MongoId, 
		[ClosedDate], 
		[TemplateId],
		DueDate,
		[Details]) 
		values 
		(@Name, 
		@ExtraElements, 
		@UserId, 
		@AssignedTo, 
		@CategoryLookUpId, 
		@MongoCategoryLookUpId, 
		@Delete, 
		@Description, 
		@LastUpdatedOn, 
		@PatientGoalId, 
		@PatientGoalMongoId, 
		@RecordCreatedBy, 
		@RecordCreatedById, 
		@RecordCreatedOn, 
		@StartDate, 
		@Status, 
		@StatusDate, 
		@TimeToLive, 
		@UpdatedBy, 
		@UpdatedById, 
		@Version, 
		@MongoID,
		@ClosedDate,
		@TemplateId,
		@DueDate,
		@Details)
	End
END

GO

DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientTask]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientTask] 
	@MongoId varchar(50),
	@MongoPatientGoalId varchar(50),
	@Name varchar(100),
	@Description varchar(5000),
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@TimeToLive datetime,
	@Delete varchar(50),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50),
	@Details varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@UpdatedBy		INT,
			@RecordCreatedBy INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @MongoPatientGoalId
	
		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end	
	
	If Exists(Select Top 1 1 From RPT_PatientTask Where MongoId = @MongoId)
	Begin
		Update RPT_PatientTask
		Set Name = @Name,
			Description = @Description,
			[Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @MongoPatientGoalId,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			TTLDate = @TimeToLive,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			[Version] = @Version,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId,
			Details = @Details
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert Into 
			RPT_PatientTask
			(
			Name, 
			[Description], 
			[Delete], 
			LastUpdatedOn, 
			PatientGoalId, 
			MongoPatientGoalId, 
			RecordCreatedBy,
			MongoRecordCreatedBy, 
			RecordCreatedOn, 
			StartDate, 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue,
			TTLDate, 
			UpdatedBy, 
			MongoUpdatedBy, 
			[Version], 
			ExtraElements,
			MongoId,
			ClosedDate,
			TemplateId,
			Details
			) 
			values 
			(
			@Name, 
			@Description, 
			@Delete, 
			@LastUpdatedOn, 
			@PatientGoalId, 
			@MongoPatientGoalId, 
			@RecordCreatedBy,
			@MongoRecordCreatedBy, 
			@RecordCreatedOn, 
			@StartDate, 
			@Status, 
			@StatusDate, 
			@TargetDate,
			@TargetValue, 
			@TimeToLive, 
			@UpdatedBy, 
			@MongoUpdatedBy, 
			@Version, 
			@ExtraElements,
			@MongoId,
			@ClosedDate,
			@TemplateId,
			@Details
			)
	End
END

GO

DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientGoalMetrics]
AS
BEGIN
	DELETE [RPT_PatientGoalMetrics]
	INSERT INTO [RPT_PatientGoalMetrics]
	(
		 MongoPatientId
		,PatientGoalId
		,MongoId
		,PatientGoalName
		,PatientGoalDesc
		,PatientGoalStartDate
		,PatientGoalEndDate
		,PatientGoalSource
		,PatientGoalStatus
		,PatientGoalStatusDate
		,PatientGoalTargetDate
		,PatientGoalTargetValue
		,PatientGoalType
		,PatientGoalLastUpdatedOn
		,PatientGoalCreatedOn
		,PatientGoalClosedDate
		,PatientGoalTemplateId
		,PatientGoalFocusArea
		,PatientGoalProgramName
		,PatientGoalProgramState
		,PatientGoalProgramAssignedDate
		,PatientGoalProgramStartDate
		,PatientGoalProgramEndDate
		,PatientGoalAttributeName
		,PatientGoalAttributeValue
		,PatientBarrierId
		,PatientBarrierName
		,PatientBarrierCategory
		,PatientBarrierStatus
		,PatientBarrierStatusDate
		,PatientBarrierLastUpdated
		,PatientBarrierCreatedOn
		,PatientTaskId
		,PatientTaskDescription
		,PatientTaskStartDate
		,PatientTaskStatus
		,PatientTaskStatusDate
		,PatientTaskTargetDate
		,PatientTaskTargetValue
		,PatientTaskLastUpdated
		,PatientTaskCreatedOn
		,PatientTaskClosedDate
		,PatientTaskTemplateId
		,PatientTaskBarrierName
		,PatientInterventionId
		,PatientInterventionCategoryName
		,PatientInterventionDesc
		,PatientInterventionStartDate
		,PatientInterventionStatus
		,PatientInterventionStatusDate
		,PatientInterventionLastUpdated
		,PatientInterventionCreatedOn
		,PatientInterventionClosedDate
		,PatientInterventionTemplateId
		,PatientInterventionAssignedTo
		,PatientInterventionBarrierName
		,PatientInterventionDueDate
		,PatientGoalDetails
		,PatientBarrierDetails
		,PatientInterventionDetails 	
		,PatientTaskDetails
	) 
SELECT DISTINCT 	
		   pg.MongoPatientId
		  ,pg.PatientGoalId
		  ,pg.MongoId
		  ,pg.Name as PatientGoalName
		  ,pg.Description as PatientGoalDesc
		  ,pg.StartDate as PatientGoalStartDate
		  ,pg.EndDate as PatientGoalEndDate
		  ,pgl.Name as PatientGoalSource
		  ,pg.[Status]  as PatientGoalSource
		  ,pg.StatusDate as PatientGoalStatusDate
		  ,pg.TargetDate as PatientGoalTargetDate
		  ,pg.TargetValue  as PatientGoalTargetValue
		  ,pg.[Type] as PatientGoalType
		  ,pg.LastUpdatedOn as PatientGoalLastUpdatedOn
		  ,pg.RecordCreatedOn as PatientGoalCreatedOn
		  ,pg.ClosedDate  as PatientGoalClosedDate
		  ,pg.TemplateId as PatientGoalTemplateId
		  ,pgfa.Name as PatientGoalFocusArea
		  ,pp1.Name as PatientGoalProgramName
		  ,pp1.[State] as PatientGoalProgramState
		  ,pp1.AssignedOn  as PatientGoalProgramAssignedDate
		  ,pp1.AttributeStartDate  as PatientGoalProgramStartDate
		  ,pp1.AttributeEndDate  as PatientGoalProgramEndDate
		  ,ga.Name as PatientGoalAttributeName
		  ,gao.Value as PatientGoalAttributeValue
		  ,pb.PatientBarrierId
		  ,pb.Name as PatientBarrierName
		  ,bcl.Name as PatientBarrierCategory
		  ,pb.[Status]  as PatientBarrierStatus
		  ,pb.StatusDate as PatientBarrierStatusDate
		  ,pb.LastUpdatedOn as PatientBarrierLastUpdated
		  ,pb.RecordCreatedOn as PatientBarrierCreatedOn
		  ,pt.PatientTaskId
		  ,pt.Description as PatientTaskDescription
		  ,pt.StartDate as PatientTaskStartDate
		  ,pt.[Status] as PatientTaskStatus
		  ,pt.StatusDate as PatientTaskStatusDate
		  ,pt.TargetDate as PatientTaskTargetDate
		  ,pt.TargetValue as PatientTaskTargetValue
		  ,pt.LastUpdatedOn as PatientTaskLastUpdated
		  ,pt.RecordCreatedOn as PatientTaskCreatedOn
		  ,pt.ClosedDate as PatientTaskClosedDate
		  ,pt.TemplateId as PatientTaskTemplateId
		  ,pb1.Name as PatientTaskBarrierName
		  ,pi.PatientInterventionId
		  ,icl.Name as PatientInterventionCategoryName
		  ,pi.Description as PatientInterventionDesc
		  ,pi.StartDate as PatientInterventionStartDate
		  ,pi.[Status] as PatientInterventionStatus
		  ,pi.StatusDate as PatientInterventionStatusDate
		  ,pi.LastUpdatedOn as PatientInterventionLastUpdated
		  ,pi.RecordCreatedOn as PatientInterventionCreatedOn
		  ,pi.ClosedDate as PatientInterventionClosedDate
		  ,pi.TemplateId as PatientInterventionTemplateId
		  ,u1.PreferredName as PatientInterventionAssignedTo
		  ,pb2.Name as PatientInterventionBarrierName 	
		  ,pi.DueDate as PatientInterventionDueDate 
		  ,pg.Details as PatientGoalDetails
		  ,pb.Details as PatientBarrierDetails
		  ,pi.Details as PatientInterventionDetails
		  ,pt.Details as PatientTaskDetails
	FROM
		  RPT_PatientGoal as pg with (nolock) 	
		  left join RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		  left join RPT_PatientGoalFocusArea as pgf with (nolock) on pg.MongoId = pgf.MongoPatientGoalId
		  left join RPT_FocusAreaLookUp as pgfa with (nolock) on pgf.MongoFocusAreaId = pgfa.MongoId
		  left join RPT_PatientGoalProgram as pgp with (nolock) on pg.MongoId = pgp.MongoPatientGoalId
		  left join RPT_PatientProgram as pp1 with (nolock) on pgp.MongoId = pp1.MongoId
		  left join RPT_PatientGoalAttribute as pga with (nolock) on pg.MongoId = pga.MongoPatientGoalId
		  left join RPT_GoalAttribute as ga with (nolock) on pga.MongoGoalAttributeId = ga.MongoId
		  left join RPT_PatientGoalAttributeValue as pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
		  left join RPT_GoalAttributeOption as gao with (nolock) on pgav.Value = gao.[Key] and ga.MongoId = gao.MongoGoalAttributeId
		  left join RPT_PatientBarrier as pb with (nolock) on pg.MongoId = pb.MongoPatientGoalId  and pb.[Delete] = 'False' and pb.TTLDate IS NULL 
		  left join RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.MongoCategoryLookUpId = bcl.MongoId
		  left join RPT_PatientTask as pt with (nolock) on pg.MongoId = pt.MongoPatientGoalId and pt.[Delete] = 'False' and pt.TTLDate IS NULL 
		  left join RPT_PatientTaskBarrier as ptb with (nolock) on pt.MongoId = ptb.MongoPatientTaskId
		  left join RPT_PatientBarrier as pb1 with (nolock) on ptb.MongoPatientBarrierId = pb1.MongoId
		  left join RPT_PatientIntervention as pi with (nolock) on pg.MongoId = pi.MongoPatientGoalId and pi.[Delete] = 'False' and pi.TTLDate IS NULL 
		  left join RPT_InterventionCategoryLookUp as icl with (nolock) on pi.MongoCategoryLookUpId = icl.MongoId
		  left join RPT_User as u1 with (nolock)on pi.MongoContactUserId = u1.MongoId
		  left join RPT_PatientInterventionBarrier as pib with (nolock) on pi.MongoId = pib.MongoPatientInterventionId
		  left join RPT_PatientBarrier as pb2 with (nolock) on pib.MongoPatientBarrierId = pb2.MongoId
	WHERE
		pg.[Delete] = 'False' and pg.TTLDate IS NULL 
END
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1560
----------------------------------------------------------------------------------------------------------------------------------
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientNote]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientNote] 
	@MongoID			varchar(50),
	@PatientMongoId		varchar(50),
	@Delete				varchar(50),
	@LastUpdatedOn		datetime,
	@RecordCreatedBy	varchar(50),
	@RecordCreatedOn	datetime,
	@Text				varchar(MAX),
	@UpdatedBy			varchar(50),
	@Version			float,
	@TTLDate			datetime,
	@ExtraElements		varchar(MAX),
	@MongoMethodId			varchar(50),	
	@Type				varchar(50),
	@MongoOutcomeId			varchar(50),
	@MongoWhoId				varchar(50)	,	
	@MongoSourceId			varchar(50),		
	@ContactedOn		datetime,
	@ValidatedIntentity	varchar(50),
	@DataSource varchar (50),
	@Duration int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId			INT,
			@RecordCreatedById	INT,
			@UpdatedById		INT,
			@MethodId			INT,
			@OutcomeId			INT,
			@WhoId				INT,	
			@SourceId			INT			
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy !=  ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy

	if @MongoMethodId != ' '	
		Select @MethodId = NoteMethodId From [RPT_NoteMethodLookUp] Where MongoId = @MongoMethodId
		
	if @MongoOutcomeId != ' '	
		Select @OutcomeId = NoteOutcomeId From [RPT_NoteOutcomeLookUp] Where MongoId = @MongoOutcomeId

	if @MongoWhoId != ' '	
		Select @WhoId = NoteWhoId From [RPT_NoteWhoLookUp] Where MongoId = @MongoWhoId

	if @MongoSourceId != ' '	
		Select @SourceId = NoteSourceId From [RPT_NoteSourceLookUp] Where MongoId = @MongoSourceId

	
	If Exists(Select Top 1 1 From RPT_PatientNote Where MongoId = @MongoID)
	Begin
		Update RPT_PatientNote
		Set [Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Text] = @Text,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			TTLDate = @TTLDate,
			ExtraElements = @ExtraElements,
			MongoMethodId			= @MongoMethodId,
			MethodId			= @MethodId,	
			[Type]				= @Type,
			MongoOutcomeId			= @MongoOutcomeId,
			OutcomeId			= @OutcomeId,	
			MongoWhoId				= @MongoWhoId,	
			WhoId				= @WhoId,		
			MongoSourceId			= @MongoSourceId,
			SourceId			= @SourceId,
			ContactedOn			= @ContactedOn,
			ValidatedIntentity	= @ValidatedIntentity,
			DataSource = @DataSource,
			Duration = @Duration
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientNote(
				ExtraElements, 
				TTLDate, 
				[Delete], 
				LastUpdatedOn, 
				PatientId, 
				MongoPatientId, 
				MongoRecordCreatedBy, 
				RecordCreatedById, 
				RecordCreatedOn, 
				[Text], 
				MongoUpdatedBy, 
				UpdatedById, 
				[Version], 
				MongoId,
				MongoMethodId,
				MethodId,
				[Type],
				MongoOutcomeId,
				OutcomeId,
				MongoWhoId,
				WhoId,
				MongoSourceId,
				SourceId,
				ContactedOn,	
				ValidatedIntentity,
				DataSource,
				Duration			
			) values (
				@ExtraElements, 
				@TTLDate, 
				@Delete, 
				@LastUpdatedOn, 
				@PatientId, 
				@PatientMongoId, 
				@RecordCreatedBy, 
				@RecordCreatedById, 
				@RecordCreatedOn, 
				@Text, 
				@UpdatedBy, 
				@UpdatedById, 
				@Version, 
				@MongoID,
				@MongoMethodId,
				@MethodId,	
				@Type,
				@MongoOutcomeId,
				@OutcomeId,	
				@MongoWhoId,	
				@WhoId,		
				@MongoSourceId,
				@SourceId,		
				@ContactedOn,
				@ValidatedIntentity,
				@DataSource,
				@Duration
			)
	End
END
GO

DROP PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_PatientNotes_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_PatientNotes_Dim]
	INSERT INTO [RPT_PatientNotes_Dim]
	(
	[PatientNoteId],
	[MongoPatientId],
	[Type],
	[Method],
	[Who],
	[Source],
	[Outcome],
	[ContactedOn],
	[ProgramName],
	[ValidatedIntentity],
	[Text],
	[RecordCreatedOn],
	[Record_Created_By],
	[PATIENTID],
	[FIRSTNAME],
	[MIDDLENAME],
	[LASTNAME],
	[DATEOFBIRTH],
	[AGE],
	[GENDER],
	[PRIORITY],
	[SYSTEMID],
	[ASSIGNED_PCM],
	[ASSIGNEDTO],
	[State],
	[StartDate],
	[EndDate],
	[AssignedOn],
	[RecordUpdatedOn],
	[RecordUpdatedBy],
	[DataSource],
	[DurationInt]		
	)
	SELECT
		pn.PatientNoteId,
		PT.MongoId,
		(SELECT DISTINCT Name FROM RPT_NoteTypeLookUp WHERE MongoId = pn.[Type]) as [Type],
		(SELECT DISTINCT Name FROM RPT_NoteMethodLookUp nm WHERE nm.MongoId = pn.MongoMethodId) as [Method],
		(SELECT DISTINCT Name FROM RPT_NoteWhoLookUp nw WHERE nw.MongoId = pn.MongoWhoId) as [Who],
		(SELECT DISTINCT Name FROM RPT_NoteSourceLookUp ns WHERE ns.MongoId= pn.MongoSourceId) as [Source],
		(SELECT DISTINCT Name FROM RPT_NoteOutcomeLookUp noc WHERE noc.MongoId = pn.MongoOutcomeId) as [Outcome],
		pn.ContactedOn,
		pp.Name as [ProgramName],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.PreferredName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DATEOFBIRTH],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		(SELECT TOP 1	
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.MongoId = PT.MongoId 	) AS [ASSIGNED_PCM]
		, (SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo]
		, pp.[State]
		, pp.AttributeStartDate as [StartDate]
		, pp.[AttributeEndDate]  as [EndDate]
		, pp.[AssignedOn]
		, pn.LastUpdatedOn
		, (SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
		, pn.DataSource
		, pn.Duration
	FROM 
		RPT_PatientNote pn with (nolock)
		left outer join RPT_PatientNoteProgram pnp with (nolock) on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT with (nolock) ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS with (nolock) ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP with (nolock) ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u with (nolock) ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Delete] = 'False'	
END
GO

DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_TouchPoint_Dim]
	INSERT INTO [RPT_TouchPoint_Dim]
	(
		[MongoPatientId],
		[PatientNoteId],
		[Method],
		[Who],
		[Source],
		[Outcome],
		[ContactedOn],
		[ProgramName],
		[ValidatedIntentity],
		[Text],
		[RecordCreatedOn],
		[Record_Created_By],
		[PATIENTID],
		[FIRSTNAME],
		[MIDDLENAME],
		[LASTNAME],
		[DATEOFBIRTH],
		[AGE],
		[GENDER],
		[PRIORITY],
		[SYSTEMID],
		[ASSIGNED_PCM],
		[ASSIGNEDTO],
		[State],
		[StartDate],
		[EndDate],
		[AssignedOn],
		[RecordUpdatedOn],
		[RecordUpdatedBy],
		[DataSource],
		[DurationInt]	
	)
	SELECT
		PT.MongoId,
		pn.PatientNoteId,
		(SELECT DISTINCT Name FROM RPT_NoteMethodLookUp nm WHERE nm.MongoId = pn.MongoMethodId) as [Method],
		(SELECT DISTINCT Name FROM RPT_NoteWhoLookUp nw WHERE nw.MongoId = pn.MongoWhoId) as [Who],
		(SELECT DISTINCT Name FROM RPT_NoteSourceLookUp ns WHERE ns.MongoId= pn.MongoSourceId) as [Source],
		(SELECT DISTINCT Name FROM RPT_NoteOutcomeLookUp noc WHERE noc.MongoId = pn.MongoOutcomeId) as [Outcome],
		pn.ContactedOn,
		pp.Name as [ProgramName],
		pn.ValidatedIntentity,
		pn.Text,
		pn.RecordCreatedOn,
		u.PreferredName as [Record_Created_By],
		PT.PATIENTID,
		PT.FIRSTNAME,
		PT.MiddleName,
		PT.LASTNAME,
		(CASE WHEN PT.DateOfBirth = '' THEN NULL ELSE PT.DATEOFBIRTH END) AS [DATEOFBIRTH],
		CASE WHEN PT.DATEOFBIRTH != '' AND ISDATE(PT.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,PT.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age,	
		PT.GENDER,
		PT.PRIORITY,
		PS.SYSTEMID,
		(SELECT TOP 1	
			U.PREFERREDNAME 	  
		  FROM
			RPT_PATIENT AS P,
			RPT_USER AS U,
			RPT_CAREMEMBER AS C 	 		 	  
		  WHERE
			P.MONGOID = C.MONGOPATIENTID
			AND C.MONGOUSERID = U.MONGOID
			AND P.MongoId = PT.MongoId 	) AS [ASSIGNED_PCM]
		, (SELECT TOP 1  		
			U.PREFERREDNAME 	  
		  FROM
			RPT_USER AS U
		  WHERE
			u.MongoId = pp.MongoAssignedToId) AS [AssignedTo]
		, pp.[State]
		, pp.AttributeStartDate as [StartDate]
		, pp.[AttributeEndDate]  as [EndDate]
		, pp.[AssignedOn]
		, pn.LastUpdatedOn
		, (SELECT PreferredName FROM RPT_User WHERE MongoId = pn.MongoUpdatedBy)
		, pn.DataSource
		, pn.Duration
	FROM 
		RPT_PatientNote pn
		left outer join RPT_PatientNoteProgram pnp on pn.MongoId = pnp.MongoPatientNoteId
		INNER JOIN RPT_PATIENT PT ON pn.MongoPatientId = pt.MongoId
		LEFT OUTER JOIN RPT_PATIENTSYSTEM PS ON PT.MongoPatientSystemId = PS.MongoId
		LEFT OUTER JOIN RPT_PATIENTPROGRAM PP ON PP.MongoId = pnp.MongoId
		INNER JOIN RPT_User u ON pn.MongoRecordCreatedBy = u.MongoId
	WHERE
		pn.[Type] = '54909997d43323251c0a1dfe'
		and pn.[Delete] = 'False'
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveNoteDurationLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveNoteDurationLookUp]
GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1589
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_GoalStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_GoalStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_GoalStatistics]
As
Begin
	Truncate Table [RPT_Flat_GoalStatistics]

	Create Table #GoalAttribute (
		MongoPatientGoalId VARCHAR(50),
		GoalAttributeName  VARCHAR(100),
		GoalAttributeValue VARCHAR(50)
	)

	Insert Into #GoalAttribute(
		MongoPatientGoalId, 
		GoalAttributeName, 
		GoalAttributeValue
		)
		Select pga.MongoPatientGoalId, ga.Name, gao.Value
			From RPT_PatientGoalAttribute pga with (nolock)
				LEFT OUTER JOIN RPT_GoalAttribute ga with (nolock) on pga.MongoGoalAttributeId = ga.MongoId
				LEFT OUTER JOIN RPT_PatientGoalAttributeValue pgav with (nolock) on pga.PatientGoalAttributeId = pgav.PatientGoalAttributeId
				LEFT OUTER JOIN RPT_GoalAttributeOption gao with (nolock) on pgav.Value = gao.[Key] and ga.MongoId = gao.MongoGoalAttributeId

	Insert Into [RPT_Flat_GoalStatistics]
	(
		MongoGoalId, 
		MongoPatientId,
		Confidence, 
		Importance, 
		StageofChange, 
		CreatedOn, 
		CreatedBy, 
		UpdatedOn, 
		UpdatedBy, 
		Name, 
		Details, 
		TemplateId, 
		[Source],
		TargetDate, 
		TargetValue, 
		[Status], 
		StartDate, 
		EndDate,
		FocusAreas,
		[Type],
		PrimaryCareManagerMongoId,
		PrimaryCareManagerPreferredName
	)
	Select 
		pg.MongoId as MongoGoalId
		,pg.MongoPatientId
		,gasub1.GoalAttributeValue as Confidence
		,gasub2.GoalAttributeValue as Importance
		,gasub3.GoalAttributeValue as 'StageofChange'
		,pg.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pg.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pg.Name
		,pg.Details
		,pg.TemplateId 
		,pgl.Name as Source
		,pg.TargetDate
		,pg.TargetValue
		,pg.[Status]
		,pg.StartDate
		,pg.EndDate
		, STUFF(
				(Select
					Distinct '| ' + pgfa.Name
				From RPT_PatientGoalFocusArea pgf with (nolock)
					LEFT OUTER JOIN RPT_FocusAreaLookUp pgfa with (nolock) on pgf.MongoFocusAreaId = pgfa.MongoId
				Where pgf.MongoPatientGoalId = pg.MongoId
				Order By '| ' + pgfa.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as FocusAreas
		,pg.[Type] as [Type]
		,u1.MongoId AS 'PrimaryCareManagerMongoId'
		,u1.PreferredName AS 'PrimaryCareManagerPreferredName'
	From RPT_PatientGoal as pg with (nolock) 
		LEFT OUTER JOIN RPT_SourceLookUp as pgl with (nolock) on pg.Source = pgl.MongoId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Confidence'
				) gasub1 ON pg.MongoId = gasub1.MongoPatientGoalId
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Importance'
				) gasub2 ON pg.MongoId = gasub2.MongoPatientGoalId				
		LEFT OUTER JOIN (
				Select MongoPatientGoalId, GoalAttributeValue
				From #GoalAttribute
				Where GoalAttributeName = 'Stage of Change'
				) gasub3 ON pg.MongoId = gasub3.MongoPatientGoalId
		LEFT OUTER JOIN [RPT_CareMember] cm WITH(NOLOCK) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u1 WITH(NOLOCK) on cm.MongoUserId = u1.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pg.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pg.MongoUpdatedBy = u3.MongoId							
	Where pg.[Delete] = 'False' and pg.TTLDate IS NULL

	Drop Table #GoalAttribute

End

GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1590
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_InterventionStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_InterventionStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_InterventionStatistics]
As
Begin
	Truncate Table [RPT_Flat_InterventionStatistics]

	Insert Into [RPT_Flat_InterventionStatistics]
	(
		MongoInterventionId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,DueDate
		,[Description]
		,Details
		,CategoryName
		,TemplateId
		,AssignedTo
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,InterventionBarriers
	)
	Select	   
		pi.MongoId as MongoInterventionId
		,pg.MongoPatientId
		,pi.MongoPatientGoalId as MongoGoalId
		,pi.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pi.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pi.ClosedDate
		,pi.[Status]
		,pi.StartDate
		,pi.DueDate
		,pi.Description
		,pi.Details
		,icl.Name as CategoryName
		,pi.TemplateId		
		,u1.PreferredName as AssignedTo
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientInterventionBarrier pib with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on pib.MongoPatientBarrierId = pb.MongoId
				Where pib.MongoPatientInterventionId = pi.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as InterventionBarriers
	From RPT_PatientIntervention pi with (nolock)
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pi.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN RPT_InterventionCategoryLookUp icl with (nolock) on pi.MongoCategoryLookUpId = icl.MongoId
		LEFT OUTER JOIN RPT_User u1 with (nolock) on pi.MongoContactUserId = u1.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pi.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pi.MongoUpdatedBy = u3.MongoId	
	Where pi.[Delete] = 'False' and pi.TTLDate IS NULL 							

End

GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1591
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TaskStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TaskStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_TaskStatistics]
As
Begin
	Truncate Table [RPT_Flat_TaskStatistics]

	Insert Into [RPT_Flat_TaskStatistics]
	(
		MongoTaskId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,TargetDate
		,TargetValue
		,[Description]
		,Details
		,TemplateId
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,TaskBarriers
	)
	Select	   
		pt.MongoId as MongoTaskId
		,pg.MongoPatientId
		,pt.MongoPatientGoalId as MongoGoalId
		,pt.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pt.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pt.ClosedDate
		,pt.[Status]
		,pt.StartDate
		,pt.TargetDate
		,pt.TargetValue
		,pt.[Description]
		,pt.Details
		,pt.TemplateId
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientTaskBarrier ptb with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on ptb.MongoPatientBarrierId = pb.MongoId
				Where ptb.MongoPatientTaskId = pt.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as TaskBarriers
	From RPT_PatientTask pt with (nolock)
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pt.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pt.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pt.MongoUpdatedBy = u3.MongoId	
	Where pt.[Delete] = 'False' and pt.TTLDate IS NULL 	
						

End

GO
----------------------------------------------------------------------------------------------------------------------------------
--ENG-1592
----------------------------------------------------------------------------------------------------------------------------------
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BarrierStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BarrierStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_BarrierStatistics]
As
Begin
	Truncate Table [RPT_Flat_BarrierStatistics]

	Insert Into [RPT_Flat_BarrierStatistics]
	(
		MongoBarrierId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,[Status]
		,Name
		,Details
		,Category
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
	)

	Select	   
		pb.MongoId as MongoBarrierId
		,pg.MongoPatientId
		,pb.MongoPatientGoalId as MongoGoalId
		,pb.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pb.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pb.[Status]
		,pb.Name
		,pb.Details
		,bcl.Name as Category
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
	From RPT_PatientBarrier pb with (nolock)
		LEFT OUTER JOIN RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.MongoCategoryLookUpId = bcl.MongoId
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pb.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pb.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pb.MongoUpdatedBy = u3.MongoId	
	Where pb.[Delete] = 'False' and pb.TTLDate IS NULL 	



End

GO

----------------------------------------------------------------------------------------------------------------------------------
--ENG-1663
----------------------------------------------------------------------------------------------------------------------------------

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Engage]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- [spPhy_RPT_Flat_Engage]
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Engage]
AS
BEGIN
	Truncate Table [RPT_Engage_Enrollment_Info]

	DECLARE @ProgramSourceId varchar(50);
	SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7';

	DECLARE @CoMorbidDisease TABLE
	(
	  Id int,
	  Name	varchar(50),
	  StepSourceId varchar(50)
	)

	Insert Into @CoMorbidDisease(Id, Name, StepSourceId) Values(1, 'HTN', '55ad0692ac80d308d20004b6')
	Insert Into @CoMorbidDisease(Id, Name, StepSourceId) Values(2, 'Heart Failure', '55ad04b8ac80d308d0000604')
	Insert Into @CoMorbidDisease(Id, Name, StepSourceId) Values(3, 'COPD', '55ad3cd4ac80d308d20004d5')
	Insert Into @CoMorbidDisease(Id, Name, StepSourceId) Values(4, 'Diabetes', '55ad3e0aac80d308d20004d8')
	Insert Into @CoMorbidDisease(Id, Name, StepSourceId) Values(5, 'Asthma', '55ad42bcac80d308d00006a2')
	
	INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease					
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '5465e772bdd4dfb6d80004f7'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545cee7a890e9458aa000003', '544efd6fac80d37bc000027b') )as [Practice]		
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255fa0890e942ba2000001')) as [Enrollment] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '53f572caac80d31203000020', '53f571e0ac80d3120300001b')) as [Re_enrollment_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'5450ff07ac80d37bc00002f6', '542561b4890e942ba1000003')) as [Disenroll_Date] 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542561ec890e942ba1000004') ) as [Disenroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425600e890e942ba2000003') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '5450ff07ac80d37bc00002f6', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '5450ff07ac80d37bc00002f6', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad0692ac80d308d20004b6')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad04b8ac80d308d0000604')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3cd4ac80d308d20004d5')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3e0aac80d308d20004d8')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad42bcac80d308d00006a2')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', cm.StepSourceId)
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'


---------------------------- version 2 ----------------------------------------

SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 
INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease					
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '54b69910ac80d33c2c000032'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '543d38bbac80d33fda00002a') )as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'545cee7a890e9458aa000003', '544efd6fac80d37bc000027b') )as [Practice] 
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId,'545c0805ac80d36bd4000089', '54255fa0890e942ba2000001')) as [Enrollment]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '54a3625d890e948042000052', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date] --*  	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'545c0805ac80d36bd4000089', '542561b4890e942ba1000003')) as [Disenroll_Date] --*
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '545c0805ac80d36bd4000089', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545c0805ac80d36bd4000089', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date] --(step, action)
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] --	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad0692ac80d308d20004b6')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad04b8ac80d308d0000604')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3cd4ac80d308d20004d5')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3e0aac80d308d20004d8')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad42bcac80d308d00006a2')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', cm.StepSourceId)
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'
	
	
---------------------------- version 3 ----------------------------------------
SET @ProgramSourceId = '55ca3880ac80d35b8e00053e';

INSERT INTO [RPT_Engage_Enrollment_Info]
	(
		PatientId,
		MongoPatientId,
		PatientProgramId,
		MongoPatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Exclusion_Criteria,
		Practice,
		PCP,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Disenroll_Date,
		Disenroll_Reason,
		Disenroll_Reason_Other,
		did_not_enroll_date,
		did_not_enroll_reason,
		did_not_enroll_reason_other,
		acuity_date,
		acuity_score,
		HTN,
		Heart_Failure,
		COPD,
		Diabetes,
		Asthma,
		Comorbid_Disease		
	)
--DECLARE @ProgramSourceId varchar(50);
--SET @ProgramSourceId = '55ca3880ac80d35b8e00053e'; 	
	SELECT DISTINCT 	
		pt.PatientId
		,pt.MongoId
		,ppt.PatientProgramId
		,ppt.MongoId
		,pt.[Priority]
		, pt.firstName
		, ps.SystemId
		, pt.LastName
		, pt.MiddleName
		, pt.Suffix
		, pt.Gender
		, pt.DateOfBirth
		,pt.LSSN
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
		,(SELECT  		
			u.PreferredName 	  
		  FROM
			RPT_Patient as p,
			RPT_User as u,
			RPT_CareMember as c 	 		 	  
		  WHERE
			p.MongoId = c.MongoPatientId
			AND c.MongoUserId = u.MongoId
			AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  
		,( COALESCE( (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '543d38bbac80d33fda00002a')), 
			(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '550afd42ac80d36b310005a3')))
			)as [Exclusion_Criteria] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetPractice_Engage(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '544efd6fac80d37bc000027b' ) )as [Practice]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_Engage_V3_GetPCP(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId) )as [PCP]
		,(SELECT TOP 1
			u.PreferredName as [fullname]
		  FROM
			rpt_patient as p with (nolock)
			INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
			INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
		  WHERE
			p.PatientId = pt.PatientId
			AND pp.SourceId = @ProgramSourceId
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,@ProgramSourceId,'55bfa837ac80d31c4c0001ce', '54255fa0890e942ba2000001')) as [Enrollment]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425620b890e942ba2000005')) as [Program_Completed_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '54a3625d890e948042000052', '54942ba8ac80d33c29000019')) as [Re_enrollment_Date] --*  	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54255ff8890e942ba2000002') ) as [Enrolled_Date] --*
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54255ff8890e942ba2000002')) as [Enrollment_Action_Completion_Date] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55bfa837ac80d31c4c0001ce', '542561b4890e942ba1000003')) as [Disenroll_Date] --*
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55bfa837ac80d31c4c0001ce', '542561ec890e942ba1000004') ) as [Disenroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '54264df1890e942ba2000006') )as [Disenroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425600e890e942ba2000003') ) as [did_not_enroll_date] --*
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,@ProgramSourceId, '55bfa837ac80d31c4c0001ce', '542560c3890e942ba2000004')) as [Did_Not_Enroll_Reason] --*
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetValue(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '55bfa837ac80d31c4c0001ce', '5425611d890e942ba1000001') )as [Did_Not_Enroll_Reason_Other] --*
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5446a365ac80d37bc20001de') ) as [acuity_date] --(step, action)
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_ZeroVal(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId, '545bfc3bac80d36bd10000a7', '5453e3bbac80d37bc0000f03') )as [acuity_score] --	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad0692ac80d308d20004b6')) as [HTN]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad04b8ac80d308d0000604')) as [Heart_Failure]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3cd4ac80d308d20004d5')) as [COPD]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad3e0aac80d308d20004d8')) as [Diabetes]
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END 
			from fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', '55ad42bcac80d308d00006a2')) as [Asthma]
		,STUFF(
				(select ', ' + CASE WHEN Value = 'Yes' THEN cm.Name ELSE NULL END 
					From @CoMorbidDisease cm
						OUTER APPLY fn_RPT_GetText_SingleSelect(pt.PatientId, ppt.PatientProgramId, @ProgramSourceId,'55c248aaac80d31c4a000709', cm.StepSourceId)
					For Xml Path, Type
				).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as CoMorbid_Disease
	FROM
		RPT_Patient as pt with (nolock) 	
		INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
		INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
		INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
		INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
	WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = @ProgramSourceId
		AND ppt.[Delete] = 'False'
				
END

GO


