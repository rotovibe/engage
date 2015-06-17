ALTER PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
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
	DELETE RPT_PatientTaskAttributeValue
	DELETE RPT_PatientTaskAttribute
	DELETE RPT_PatientTaskBarrier
	DELETE RPT_PatientTask
	-- patient allergies
	DELETE RPT_Allergy
	DELETE RPT_AllergyType
	DELETE RPT_PatientAllergy
	DELETE RPT_PatientAllergyReaction
	-- patient medsupps
	DELETE RPT_PatientMedSuppPhClass
	DELETE RPT_PatientMedSuppNDC
	DELETE RPT_PatientMedSupp	
	DELETE RPT_Medication
	Delete RPT_MedicationMap
	Delete RPT_PatientMedFrequency
	
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
	DELETE RPT_UserRecentList
	DELETE [RPT_User]
	
	--DELETE CohortPatientView	
	--DELETE CohortPatientViewSearchField
	
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
	
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)

	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttribute', RESEED, 0)
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
	
	--DBCC CHECKIDENT ('RPT_CohortPatientView', RESEED, 0)
	--DBCC CHECKIDENT ('RPT_CohortPatientViewSearchField', RESEED, 0)
END
GO

/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Medication')
	DROP TABLE [RPT_Medication];
GO

CREATE TABLE [dbo].[RPT_Medication](
[MedId] [int] IDENTITY(1,1) NOT NULL,
[MongoId] [varchar](50) NOT NULL,
[ProductId] [varchar](100) NULL,
[NDC] [varchar](900) NULL,
[FullName] [varchar](5000) NULL,
[ProprietaryName] [varchar](900) NULL,
[ProprietaryNameSuffix] [varchar](900) NULL,
[StartDate] [varchar](100) NULL,
[EndDate] [varchar](100) NULL,
[SubstanceName] [varchar](5000) NULL,
[Route] [varchar](5000) NULL,
[Form] [varchar](5000) NULL,
[FamilyId] [varchar](50) NULL,
[Unit] [varchar](5000) NULL,
[Strength] [varchar](5000) NULL,
[Version] [float] NULL,
[DeleteFlag] [varchar](50) NULL,
[TTLDate] [datetime] NULL,
[LastUpdatedOn] [datetime] NULL,
[MongoRecordCreatedBy] [varchar](50) NULL,
[RecordCreatedOn] [datetime] NULL,
[MongoUpdatedBy] [varchar](50) NULL,
	
 CONSTRAINT [PK_Medication] PRIMARY KEY CLUSTERED 
(
	[MedId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [RPT_Medication_MongoId] ON [dbo].[RPT_Medication] 
(
	[MongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

CREATE NONCLUSTERED INDEX [RPT_Medication_NDC] ON [dbo].[RPT_Medication] 
(
	[NDC] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO


/*** RPT_Medication ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedPharmClass')
	DROP TABLE [RPT_MedPharmClass];
GO

CREATE TABLE [dbo].[RPT_MedPharmClass](
[PhmCId] [int] IDENTITY(1,1) NOT NULL,
[MedMongoId] [varchar](50) NULL,
[PharmClass] [varchar](900) NULL,
	
 CONSTRAINT [PK_MedPharmClass] PRIMARY KEY CLUSTERED 
(
	[PhmCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE NONCLUSTERED INDEX [Med_Mongo_Id] ON [dbo].[RPT_MedPharmClass] 
(
	[MedMongoId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [PharmClass] ON [dbo].[RPT_MedPharmClass] 
(
	[PharmClass] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO

/*** RPT_MedicationMap ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_MedicationMap')
	DROP TABLE [RPT_MedicationMap];
GO

CREATE TABLE [dbo].[RPT_MedicationMap](
[MedMapId] [int] IDENTITY(1,1) NOT NULL,
[MongoId] [varchar](50) NOT NULL,
[FullName] [varchar](5000) NULL,
[SubstanceName] [varchar](5000) NULL,
[Route] [varchar](5000) NULL,
[Form] [varchar](5000) NULL,
[Strength] [varchar](5000) NULL,
[Version] [float] NULL,
[DeleteFlag] [varchar](50) NULL,
--[TTLDate] [datetime] NULL,
--[LastUpdatedOn] [datetime] NULL,
[MongoRecordCreatedBy] [varchar](50) NULL,
[RecordCreatedOn] [datetime] NULL,
[MongoUpdatedBy] [varchar](50) NULL,
	
 CONSTRAINT [PK_MedicationMap] PRIMARY KEY CLUSTERED 
(
	[MedMapId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

/**** RPT_PatientMedSupp ****/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientMedSupp]
GO

CREATE TABLE [dbo].[RPT_PatientMedSupp](
	[PatientMedSuppId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[MongoFrequencyId] [varchar](50) NULL,	
	[MongoFamilyId] [varchar](50) NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[Category] [varchar](200) NOT NULL,
	[MongoTypeId] [varchar](50) NOT NULL,
	[TypeId] [int] NULL,
	[Status] [varchar](200) NOT NULL,
	[Strength] [varchar](200) NULL,
	[Route] [varchar](200) NULL,
	[Form] [varchar](200) NULL,
	[FreqQuantity] [varchar](200) NULL,
	[MongoSourceId] [varchar](50) NOT NULL,
	[SourceId] [int] NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Reason] [varchar](5000) NULL,
	[Notes] [varchar](5000) NULL,
	[PrescribedBy] [varchar](500) NULL,
	[SystemName] [varchar](50) NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedBy] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedBy] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
 CONSTRAINT [PK_PatientMedSupp] PRIMARY KEY CLUSTERED 
(
	[PatientMedSuppId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientMedSupp]') AND type in (N'P', N'PC'))
	DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp] 
	@MongoId				[varchar](50),
	@MongoPatientId			[varchar](50),
	@MongoFrequencyId		[VARCHAR](50),	
	@MongoFamilyId			[VARCHAR](50),
	@Name					[varchar] (200),
	@Category				[varchar] (200),
	@MongoTypeId			[varchar] (50),
	@Status					[varchar] (200),
	@Strength				[varchar] (200),
	@Route					[varchar] (200),
	@Form					[varchar] (200),
	@FreqQuantity			[varchar] (200),
	@MongoSourceId			[varchar](50),	
	@StartDate				[datetime],
	@EndDate				[datetime],
	@Reason					[varchar](5000),
	@Notes					[varchar](5000),	
	@PrescribedBy			[varchar](500),	
	@SystemName				[varchar](50),
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version				[float],
	@TTLDate				[DATETIME],
	@Delete					[VARCHAR](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@FreqHowOftenId INT,
			@FreqWhenId INT,			
			@RecordCreatedById INT,
			@UpdatedById INT,
			@MedSuppTypeId INT,
			@SourceId INT
	
	select @MedSuppTypeId = mst.MedSupId  from RPT_MedSupTypeLookUp as mst where MongoId = @MongoTypeId
	select @SourceId = AllergySourceId from RPT_AllergySourceLookUp where MongoId = @MongoSourceId
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientMedSupp Where MongoId = @MongoID)
		Begin
			Update RPT_PatientMedSupp
			Set 
				MongoId					= @MongoId,				
				MongoPatientId			= @MongoPatientId,
				MongoFrequencyId		= @MongoFrequencyId,
				MongoFamilyId			= @MongoFamilyId,
				PatientId				= @PatientId,			
				Name					= @Name,					
				Category				= @Category,				
				MongoTypeId				= @MongoTypeId,
				TypeId					= @MedSuppTypeId,			
				[Status]				= @Status,					
				Strength				= @Strength,				
				[Route]					= @Route,					
				Form					= @Form,					
				FreqQuantity			= @FreqQuantity,			
				MongoSourceId			= @MongoSourceId,			
				SourceId				= @SourceId,			
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Reason					= @Reason,					
				Notes					= @Notes,					
				PrescribedBy			= @PrescribedBy,			
				SystemName				= @SystemName,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedBy				= @UpdatedById,				
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
				RecordCreatedBy			= @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,
				[Version]				= @Version,
				TTLDate					= @TTLDate,
				[Delete]				= @Delete
			Where 
				MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientMedSupp
			(
				MongoId,
				MongoPatientId,
				MongoFrequencyId,				
				MongoFamilyId,
				PatientId,
				Name,
				Category,
				MongoTypeId,
				TypeId,
				[Status],
				Strength,
				[Route],
				Form,
				FreqQuantity,					
				MongoSourceId,			
				SourceId,				
				StartDate,				
				EndDate,					
				Reason,					
				Notes,					
				PrescribedBy,			
				SystemName,				
				MongoUpdatedBy,			
				UpdatedBy,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedBy,			
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]						
			) 
			values 
			(
				@MongoId,				
				@MongoPatientId,
				@MongoFrequencyId,				
				@MongoFamilyId,
				@PatientId,			
				@Name,				
				@Category,			
				@MongoTypeId,
				@MedSuppTypeId,		
				@Status,					
				@Strength,			
				@Route,				
				@Form,				
				@FreqQuantity,		
				@MongoSourceId,		
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Reason,				
				@Notes,				
				@PrescribedBy,		
				@SystemName,			
				@MongoUpdatedBy,		
				@UpdatedById,			
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,
				@Version,
				@TTLDate,
				@Delete
			)
		End
END
GO

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedFrequency]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientMedFrequency]
GO

CREATE TABLE [dbo].[RPT_PatientMedFrequency](
	[PMFreqId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[LookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
)
GO

select * from RPT_PatientMedFrequency
select * from RPT_PatientMedSupp
select * from RPT_MedPharmClass