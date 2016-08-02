/*** ENG-1146 ***/
ALTER PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	TRUNCATE TABLE RPT_CareMember
	TRUNCATE TABLE RPT_CareMemberTypeLookUp
	TRUNCATE TABLE RPT_ContactEmail
	TRUNCATE TABLE RPT_ContactPhone
	TRUNCATE TABLE RPT_ContactAddress
	TRUNCATE TABLE RPT_ContactRecentList
	TRUNCATE TABLE RPT_ContactMode
	TRUNCATE TABLE RPT_ContactLanguage
	TRUNCATE TABLE RPT_ContactWeekDay
	TRUNCATE TABLE RPT_ContactTimeOfDay
	-- todos
	TRUNCATE TABLE RPT_ToDoProgram
	TRUNCATE TABLE RPT_ToDo
	-- patient programs
	TRUNCATE TABLE RPT_SpawnElements
	TRUNCATE TABLE RPT_SpawnElementTypeCode
	TRUNCATE TABLE RPT_PatientProgramAttribute
	TRUNCATE TABLE RPT_PatientProgramResponse
	TRUNCATE TABLE RPT_PatientProgramStep
	TRUNCATE TABLE RPT_PatientProgramAction	
	TRUNCATE TABLE RPT_PatientProgramModule
	TRUNCATE TABLE RPT_PatientProgram	
	TRUNCATE TABLE RPT_PatientNoteProgram
	TRUNCATE TABLE RPT_PatientNote
	TRUNCATE TABLE RPT_PatientProblem
	TRUNCATE TABLE RPT_ObjectiveCategory
	TRUNCATE TABLE RPT_ObjectiveLookUp	
	TRUNCATE TABLE RPT_PatientObservation	
	TRUNCATE TABLE RPT_Observation
	TRUNCATE TABLE RPT_PatientTaskAttributeValue
	TRUNCATE TABLE RPT_PatientTaskAttribute
	TRUNCATE TABLE RPT_PatientTaskBarrier
	TRUNCATE TABLE RPT_PatientTask
	-- patient allergies
	TRUNCATE TABLE RPT_Allergy
	TRUNCATE TABLE RPT_AllergyType
	TRUNCATE TABLE RPT_PatientAllergy
	TRUNCATE TABLE RPT_PatientAllergyReaction
	-- patient medsupps
	TRUNCATE TABLE RPT_PatientMedSuppPhClass
	TRUNCATE TABLE RPT_MedPharmClass
	TRUNCATE TABLE RPT_PatientMedSuppNDC
	TRUNCATE TABLE RPT_PatientMedSupp	
	TRUNCATE TABLE RPT_Medication
	TRUNCATE TABLE RPT_MedicationMap
	TRUNCATE TABLE RPT_PatientMedFrequency
	TRUNCATE TABLE RPT_CustomPatientMedFrequency
	
	-- patient goal
	TRUNCATE TABLE RPT_PatientGoalProgram
	TRUNCATE TABLE RPT_PatientGoalFocusArea
	TRUNCATE TABLE RPT_GoalAttributeOption	
	TRUNCATE TABLE RPT_PatientGoalAttributeValue
	TRUNCATE TABLE RPT_PatientGoalAttribute
	TRUNCATE TABLE RPT_GoalAttribute
	TRUNCATE TABLE RPT_PatientInterventionBarrier
	TRUNCATE TABLE RPT_PatientIntervention	
	TRUNCATE TABLE RPT_PatientBarrier	
	TRUNCATE TABLE RPT_PatientGoal
	TRUNCATE TABLE RPT_PatientUser	
	TRUNCATE TABLE RPT_Contact
	TRUNCATE TABLE RPT_PatientSystem
	TRUNCATE TABLE RPT_Patient
	TRUNCATE TABLE RPT_CommTypeCommMode
	TRUNCATE TABLE RPT_ToDoCategoryLookUp	
	TRUNCATE TABLE RPTMongoCategoryLookUp
	TRUNCATE TABLE RPT_SourceLookUp
	TRUNCATE TABLE RPT_BarrierCategoryLookUp
	TRUNCATE TABLE RPT_InterventionCategoryLookUp
	TRUNCATE TABLE RPTMongoTimeZoneLookUp
	TRUNCATE TABLE RPT_ProblemLookUp
	TRUNCATE TABLE RPT_TimesOfDayLookUp
	TRUNCATE TABLE RPT_CommTypeLookUp
	TRUNCATE TABLE RPT_CommModeLookUp
	TRUNCATE TABLE RPT_StateLookUp
	TRUNCATE TABLE RPT_LanguageLookUp
	TRUNCATE TABLE RPT_FocusAreaLookUp
	TRUNCATE TABLE RPT_CodingSystemLookUp
	TRUNCATE TABLE RPT_ObservationTypeLookUp
	TRUNCATE TABLE RPT_AllergyTypeLookUp
	TRUNCATE TABLE RPT_AllergySourceLookUp
	TRUNCATE TABLE RPT_SeverityLookUp
	TRUNCATE TABLE RPT_ReactionLookUp
	TRUNCATE TABLE RPT_MedSupTypeLookUp
	TRUNCATE TABLE RPT_FreqHowOftenLookUp
	TRUNCATE TABLE RPT_FreqWhenLookUp
	TRUNCATE TABLE RPT_NoteTypeLookUp
	TRUNCATE TABLE RPT_UserRecentList
	TRUNCATE TABLE [RPT_User]
	
	--DELETE CohortPatientView	
	--DELETE CohortPatientViewSearchField
	
--	DBCC CHECKIDENT ('RPT_CareMember', RESEED, 0) 
--	DBCC CHECKIDENT ('RPT_CareMemberTypeLookUp', RESEED, 0)	
	
--	DBCC CHECKIDENT ('RPT_ContactLanguage', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ContactWeekDay', RESEED, 0)  
--	DBCC CHECKIDENT ('RPT_ContactTimeOfDay', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ContactRecentList', RESEED, 0) 
--	DBCC CHECKIDENT ('RPT_ContactMode', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ContactPhone', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ContactAddress', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ContactEmail', RESEED, 0)
	
---- reseed program tables
--	DBCC CHECKIDENT ('RPT_PatientProgram', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_SpawnElements', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProgramModule', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProgramAction', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProgramStep', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProgramResponse', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProgramAttribute', RESEED, 0)	
	
--	DBCC CHECKIDENT ('RPT_PatientNoteProgram', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientNote', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientProblem', RESEED, 0)

--	-- allergies
--	DBCC CHECKIDENT ('RPT_AllergyType', RESEED, 0)	
--	DBCC CHECKIDENT ('RPT_Allergy', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_MedicationMap', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_Medication', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientMedFrequency', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_CustomPatientMedFrequency', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_MedPharmClass', RESEED,0)
	
--	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_NoteTypeLookUp', RESEED, 0)

--	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientTaskAttributeValue', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientTaskAttribute', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientTaskBarrier', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientTask', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_PatientGoalProgram', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientGoalFocusArea', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_GoalAttributeOption', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientGoalAttributeValue', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientGoalAttribute', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_GoalAttribute', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_PatientInterventionBarrier', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientIntervention', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientBarrier', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientGoal', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientUser', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_Contact', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_PatientSystem', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_Patient', RESEED, 0) 
	
--	DBCC CHECKIDENT ('RPT_CommTypeCommMode', RESEED, 0)
--	DBCC CHECKIDENT ('RPTMongoCategoryLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_SourceLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_BarrierCategoryLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_InterventionCategoryLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPTMongoTimeZoneLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ProblemLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_TimesOfDayLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_CommTypeLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_CommModeLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_StateLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_LanguageLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_FocusAreaLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_CodingSystemLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ObservationTypeLookUp', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_UserRecentList', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_User', RESEED, 0)
	
--	DBCC CHECKIDENT ('RPT_ToDoCategoryLookUp', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ToDoProgram', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_ToDo', RESEED, 0)
	
--	-- patient allergies
--	DBCC CHECKIDENT ('RPT_PatientAllergy', RESEED, 0)
--	DBCC CHECKIDENT ('RPT_PatientAllergyReaction', RESEED, 0)
	
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
[NDC] [varchar](20) NULL,
[FullName] [varchar](300) NULL,
[ProprietaryName] [varchar](300) NULL,
[ProprietaryNameSuffix] [varchar](200) NULL,
[StartDate] [varchar](100) NULL,
[EndDate] [varchar](100) NULL,
[SubstanceName] [varchar](3000) NULL,
[Route] [varchar](200) NULL,
[Form] [varchar](100) NULL,
[FamilyId] [varchar](50) NULL,
[Unit] [varchar](5000) NULL,
[Strength] [varchar](900) NULL,
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
[PharmClass] [varchar](150) NULL,
	
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
[FullName] [varchar](300) NULL,
[SubstanceName] [varchar](3000) NULL,
[Route] [varchar](200) NULL,
[Form] [varchar](100) NULL,
[Strength] [varchar](2000) NULL,
[Version] [float] NULL,
[DeleteFlag] [varchar](50) NULL,
[Custom] [varchar](50) NULL,
[Verified] [varchar](50) NULL,
[TTLDate] [datetime] NULL,
[LastUpdatedOn] [datetime] NULL,
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

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_PatientMedSupp_Compound') 
	DROP INDEX IX_RPT_PatientMedSupp_Compound ON RPT_PatientMedSupp; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_PatientMedSupp_Compound 
	ON [RPT_PatientMedSupp] 
	(
		[MongoId]
		,[PatientId]
		,[MongoPatientId]
		,[MongoFrequencyId]
		,[MongoFamilyId]
		,[Name]
		--,[Category]
		--,[MongoTypeId]
		,[TypeId]
		--,[Status]
		--,[Strength]
		,[Route]
		,[Form]
		--,[FreqQuantity]
		--,[MongoSourceId]
		,[SourceId]
	) ON [PRIMARY]; 
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

/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_CustomPatientMedFrequency]') AND type in (N'U'))
	DROP TABLE [dbo].[RPT_CustomPatientMedFrequency]
GO

CREATE TABLE [dbo].[RPT_CustomPatientMedFrequency](
	[CPMFreqId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[Name] [varchar](300) NULL,
	[MongoPatientId] [VARCHAR](50) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [VARCHAR](50) NULL,
	[DeleteFlag] [VARCHAR](50) NULL,
	[TTLDate] [DATETIME] NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[MongoRecordCreatedBy] [VARCHAR](50) NULL,
	[RecordCreatedOn] [DATETIME] NULL
	
)
GO

/*** [add non clustered indexes to medication, map, and phcls] ***/
IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_Medication_FamilyId') 
	DROP INDEX IX_RPT_Medication_FamilyId ON RPT_Medication; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_Medication_FamilyId 
	ON RPT_Medication 
	(
		[FamilyId]
		,[ProductId]
		,[NDC]
		,[ProprietaryName]
		--,[ProprietaryNameSuffix]
		,[StartDate]
		,[EndDate]
		,[Version]
		,[DeleteFlag]
		,[TTLDate]
		,[LastUpdatedOn]
		,[MongoRecordCreatedBy]
		,[RecordCreatedOn]
		,[MongoUpdatedBy]		
		--,[Unit]		
	) ON [PRIMARY]; 
GO

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_MedicationMap_MongoId') 
	DROP INDEX IX_RPT_MedicationMap_MongoId ON RPT_MedicationMap; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_MedicationMap_MongoId 
	ON RPT_MedicationMap 
	(
		[MongoId]
	--,[FullName]
	--,[SubstanceName]
	,[Route]
	,[Form]
	--,[Strength]
	,[Version]
	,[DeleteFlag]
	,[Custom]
	,[Verified]
	,[TTLDate]
	,[LastUpdatedOn]
	,[MongoRecordCreatedBy]
	,[RecordCreatedOn]
	,[MongoUpdatedBy]		
	) ON [PRIMARY]; 
GO

IF EXISTS (SELECT name FROM sys.indexes
			WHERE name = N'IX_RPT_MedPharmClass_MedMongoId') 
	DROP INDEX IX_RPT_MedPharmClass_MedMongoId ON RPT_MedPharmClass; 
GO

CREATE NONCLUSTERED INDEX IX_RPT_MedPharmClass_MedMongoId
	ON RPT_MedPharmClass ([MedMongoId], [PharmClass]) ON [PRIMARY]; 
GO


/*** RPT_PatientMedFrequency ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_MedicationMap_Dim]') AND type in (N'U'))
	DROP TABLE [RPT_Flat_MedicationMap_Dim]
GO

CREATE TABLE [dbo].[RPT_Flat_MedicationMap_Dim](
	[MMId] [int] IDENTITY(1,1) NOT NULL,	
	[MongoId] [varchar](50) NOT NULL,
	[ProductId] [varchar](50) NULL,
	[NDC] [varchar](20) NULL,
	[ProprietaryName] [varchar](300) NULL,
	[ProprietaryNameSuffix] [varchar](200) NULL,
	[StartDate] [varchar](25) NULL,
	[EndDate] [varchar](25) NULL,
	[Unit] [varchar](2000) NULL,
	[FullName] [varchar](300) NULL,
	[SubstanceName] [varchar](3000) NULL,
	[Route] [varchar](200) NULL,
	[Form] [varchar](50) NULL,
	[Strength] [varchar](2000) NULL,
	[Custom] [varchar](10) NULL,
	[Verified] [varchar](10) NULL,
	[PharmClass] [varchar](100) NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [VARCHAR](50) NULL,
	[DeleteFlag] [VARCHAR](10) NULL,
	[TTLDate] [DATETIME] NULL,
	[LastUpdatedOn] [DATETIME] NULL,
	[MongoRecordCreatedBy] [VARCHAR](50) NULL,
	[RecordCreatedOn] [DATETIME] NULL,
	[Med_Version] [float] NULL,
	[Med_DeleteFlag] [VARCHAR](10) NULL,
	[Med_TTLDate] [VARCHAR](50) NULL,
	[Med_LastUpdatedOn]  [DATETIME] NULL,
	[Med_MongoRecordCreatedBy]  [VARCHAR](50) NULL,
	[Med_RecordCreatedOn]  [DATETIME] NULL,
	[Med_MongoUpdatedBy] [VARCHAR](50) NULL
)
GO

IF (OBJECT_ID('[spPhy_RPT_Flat_MedicationMap_Dim]') IS NOT NULL)
  DROP PROCEDURE spPhy_RPT_Flat_MedicationMap_Dim
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_MedicationMap_Dim]
AS
BEGIN
	TRUNCATE TABLE RPT_Flat_MedicationMap_Dim
	INSERT INTO RPT_Flat_MedicationMap_Dim
	(
		[MongoId]
		,[ProductId]
		,[NDC]
		,[ProprietaryName]
		,[ProprietaryNameSuffix]
		,[StartDate]
		,[EndDate]
		,[Unit]
		,[FullName]
		,[SubstanceName]
		,[Route]
		,[Form]
		,[Strength]
		,[Version]
		,[DeleteFlag]
		,[Custom]
		,[Verified]
		,[TTLDate]
		,[LastUpdatedOn]
		,[MongoRecordCreatedBy]
		,[RecordCreatedOn]
		,[MongoUpdatedBy]
		,[PharmClass] 
		,[Med_Version]
		,[Med_DeleteFlag]
		,[Med_TTLDate]
		,[Med_LastUpdatedOn]
		,[Med_MongoRecordCreatedBy]
		,[Med_RecordCreatedOn]
		,[Med_MongoUpdatedBy]
	)
	select
		mm.[MongoId]
		,m.[ProductId]
		,m.[NDC]
		,m.[ProprietaryName]
		,m.[ProprietaryNameSuffix]
		,m.[StartDate]
		,m.[EndDate]
		,m.[Unit]
		,mm.[FullName]
		,mm.[SubstanceName]
		,mm.[Route]
		,mm.[Form]
		,mm.[Strength]
		,mm.[Version]
		,mm.[DeleteFlag]
		,mm.[Custom]
		,mm.[Verified]
		,mm.[TTLDate]
		,mm.[LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = mm.[MongoRecordCreatedBy]) as [MongoRecordCreatedBy]
		,mm.[RecordCreatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = mm.[MongoUpdatedBy]) as [MongoUpdatedBy]
		,pc.[PharmClass] 
		,m.[Version] AS [Med_Version]
		,m.[DeleteFlag] AS [Med_DeleteFlag]
		,m.[TTLDate] AS [Med_TTLDate]
		,m.[LastUpdatedOn] AS [Med_LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = m.[MongoRecordCreatedBy]) as [Med_MongoRecordCreatedBy]
		,m.[RecordCreatedOn] AS [Med_RecordCreatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = m.[MongoUpdatedBy]) as [Med_MongoUpdatedBy]		
	from 
		RPT_MedicationMap as [mm] WITH (NOLOCK) 
		LEFT JOIN RPT_Medication as [m] WITH (NOLOCK) ON mm.MongoId = m.FamilyId
		LEFT OUTER JOIN RPT_MedPharmClass as [pc] WITH (NOLOCK) ON pc.MedMongoId = m.MongoId
	WHERE
		mm.DeleteFlag = 'False'
		AND mm.TTLDate IS NULL
END
GO

/*** [RPT_Flat_PatientMedSup_Dim] ***/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Flat_PatientMedSup_Dim]') AND type in (N'U'))
	DROP TABLE [RPT_Flat_PatientMedSup_Dim]
GO

CREATE TABLE [RPT_Flat_PatientMedSup_Dim](
 [PMSId] [int] IDENTITY(1,1) NOT NULL	 
,[Name] [VARCHAR](200) NULL
,[MongoId] [VARCHAR](50) NULL
,[MongoPatientId] [VARCHAR](50) NULL
,[Frequency] [VARCHAR](300) NULL
,[MongoFamilyId] [VARCHAR](50) NULL
,[Category] [VARCHAR](200) NULL
,[Type] [VARCHAR](300) NULL
,[Status] [VARCHAR](200) NULL
,[Strength] [VARCHAR](300) NULL
,[Route] [VARCHAR](300) NULL
,[Form] [VARCHAR](300) NULL
,[FreqQuantity] [VARCHAR](200) NULL
,[Source] [VARCHAR](max) NULL
,[StartDate] [VARCHAR](25) NULL
,[EndDate] [VARCHAR](25) NULL
,[Reason] [VARCHAR](5000) NULL
,[Notes] [VARCHAR](5000) NULL
,[PrescribedBy] [VARCHAR](900) NULL
,[SystemName] [VARCHAR](300) NULL
,[UpdatedBy] [VARCHAR](100) NULL
,[LastUpdatedOn] [DATETIME] NULL
,[RecordCreatedBy] [VARCHAR](100) NULL
,[RecordCreatedOn] [DATETIME] NULL
,[Version] [float] NULL
,[TTLDate] [VARCHAR](25) NULL
,[Delete] [VARCHAR](10) NULL
,[PharmClass] [VARCHAR](200) NULL
,[NDC] [VARCHAR](50) NULL
,[Custom] [VARCHAR](50) NULL
,[Verified] [VARCHAR](10) NULL
)
GO

IF (OBJECT_ID('[spPhy_RPT_Flat_PatientMedSup_Dim]') IS NOT NULL)
  DROP PROCEDURE [spPhy_RPT_Flat_PatientMedSup_Dim]
GO

CREATE PROCEDURE [spPhy_RPT_Flat_PatientMedSup_Dim]
AS
BEGIN
	TRUNCATE TABLE [RPT_Flat_PatientMedSup_Dim]
	INSERT INTO [RPT_Flat_PatientMedSup_Dim]
	(
		[Name]
		,[MongoId]
		,[MongoPatientId]
		,[Frequency]
		,[MongoFamilyId]
		,[Category]
		,[Type]
		,[Status]
		,[Strength]
		,[Route]
		,[Form]
		,[FreqQuantity]
		,[Source]
		,[StartDate]
		,[EndDate]
		,[Reason]
		,[Notes]
		,[PrescribedBy]
		,[SystemName]
		,[UpdatedBy]
		,[LastUpdatedOn]
		,[RecordCreatedBy] 
		,[RecordCreatedOn]
		,[Version]
		,[TTLDate]
		,[Delete]
		,[PharmClass]
		,[NDC]
		,[Custom]
		,[Verified]
	)
	select 
		pm.Name
		,pm.[MongoId]
		,pm.[MongoPatientId]
		, (COALESCE( (select TOP 1 Name from RPT_PatientMedFrequency where MongoId = pm.[MongoFrequencyId]), (select TOP 1  Name from RPT_CustomPatientMedFrequency  where MongoId = pm.[MongoFrequencyId]) )) as [Frequency]
		,pm.[MongoFamilyId]
		,pm.[Category]
		,(select Name from RPT_MedSupTypeLookUp where MongoId = pm.[MongoTypeId]) as [Type]
		,pm.[Status]
		,pm.[Strength]
		,pm.[Route]
		,pm.[Form]
		,pm.[FreqQuantity]
		,(select Name from RPT_AllergySourceLookUp where MongoId = pm.[MongoSourceId]) as [Source]
		,pm.[StartDate]
		,pm.[EndDate]
		,pm.[Reason]
		,pm.[Notes]
		,pm.[PrescribedBy]
		,pm.[SystemName]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = pm.[MongoUpdatedBy])as [UpdatedBy]
		,pm.[LastUpdatedOn]
		,(SELECT PreferredName FROM RPT_User WHERE MongoId = pm.[MongoRecordCreatedBy]) as [RecordCreatedBy] 
		,pm.[RecordCreatedOn]
		,pm.[Version]
		,pm.[TTLDate]
		,pm.[Delete]
		,pc.PharmClass
		,n.NDC
		,mm.Custom
		, mm.Verified
	from 
		RPT_PatientMedSupp pm
		left outer join RPT_PatientMedSuppNDC n on pm.MongoId = n.MongoPatientMedSuppId
		left outer join RPT_PatientMedSuppPhClass pc on pm.MongoId = pc.MongoPatientMedSuppId
		left outer join RPT_MedicationMap mm on (pm.Name = mm.FullName and pm.Route = mm.Route and pm.Form = mm.Form and pm.Strength = mm.Strength) 
		or (pm.Name = mm.FullName and pm.Route = mm.Route and pm.Form
		 = mm.Form) or (pm.Name = mm.FullName and pm.Route = mm.Route) or (pm.Name = mm.FullName)
	WHERE
		pm.TTLDate IS NULL
		AND pm.[Delete] = 'False'
END
GO	