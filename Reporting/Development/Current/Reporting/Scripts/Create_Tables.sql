IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_AllergyTypeLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_AllergyTypeLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_AllergySourceLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_AllergySourceLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_SeverityLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_SeverityLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ReactionLookUp]') AND type in (N'U'))				
DROP TABLE [dbo].[RPT_ReactionLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_MedSupTypeLookUp]') AND type in (N'U'))				
DROP TABLE [dbo].[RPT_MedSupTypeLookUp]
GO 

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_FreqHowOftenLookUp]') AND type in (N'U'))				
DROP TABLE [dbo].[RPT_FreqHowOftenLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_FreqWhenLookUp]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_FreqWhenLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientAllergyReaction]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_PatientAllergyReaction]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Allergy]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_Allergy]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_AllergyType]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_AllergyType]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSupp]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_PatientMedSupp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSuppNDC]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_PatientMedSuppNDC]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientMedSuppPhClass]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_PatientMedSuppPhClass]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientAllergy]') AND type in (N'U'))	
DROP TABLE [dbo].[RPT_PatientAllergy]
GO

--ALTER TABLE [dbo].[RPT_PatientIntervention] ADD [ClosedDate] [datetime] NULL, TemplateId VARCHAR(50) ;
--GO

--ALTER TABLE [dbo].[RPT_PatientTask] ADD [ClosedDate] [datetime] NULL, TemplateId VARCHAR(50) ;
--GO

ALTER TABLE [dbo].[RPT_PatientGoal] ADD [ClosedDate] [datetime] NULL, TemplateId VARCHAR(50) ;
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_BSHSI_HW2_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info]
GO

CREATE TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [datetime] NULL,
	[Market] [varchar](max) NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_AllergyTypeLookUp](
	[AllergyTypeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[CodeSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
 CONSTRAINT [PK_RPT_AllergyTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[AllergyTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_AllergySourceLookUp](
	[AllergySourceId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[Active] [varchar](50) NULL,
	[Default] [varchar](50) NULL,
 CONSTRAINT [PK_RPT_AllergySourceLookUp] PRIMARY KEY CLUSTERED 
(
	[AllergySourceId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_SeverityLookUp](
	[SeverityId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[CodeSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
 CONSTRAINT [PK_RPT_SeverityLookUp] PRIMARY KEY CLUSTERED 
(
	[SeverityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_ReactionLookUp](
	[ReactionId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL,
	[CodeSystem] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
 CONSTRAINT [PK_RPT_ReactionLookUp] PRIMARY KEY CLUSTERED 
(
	[ReactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_MedSupTypeLookUp](
	[MedSupId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL
 CONSTRAINT [PK_RPT_MedSupTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[MedSupId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_FreqHowOftenLookUp](
	[FreqId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL
 CONSTRAINT [PK_RPT_FreqHowOftenLookUp] PRIMARY KEY CLUSTERED 
(
	[FreqId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_FreqWhenLookUp](
	[FreqWhenId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](100) NOT NULL,
	[lookUpType] [varchar](200) NULL,
	[Name] [varchar](300) NULL
 CONSTRAINT [PK_RPT_FreqWhenLookUp] PRIMARY KEY CLUSTERED 
(
	[FreqWhenId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_PatientAllergy](
	[PatientAllergyId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[AllergyId] [int] NOT NULL,
	[MongoAllergyId] [varchar](50) NOT NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NOT NULL,
	[SeverityId] [int] NULL,
	[MongoSeverityId] [varchar](50) NULL,
	[StatusId] [varchar](100) NOT NULL,
	[SourceId] [varchar](50) NOT NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Notes] [varchar](5000) NULL,
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
	[ExtraElements] [varchar](5000) NULL,
 CONSTRAINT [PK_PatientAllergy_1] PRIMARY KEY CLUSTERED 
(
	[PatientAllergyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_PatientAllergyReaction](
	[AllergyReactionId] [int] IDENTITY(1,1) NOT NULL,
	[PatientAllergyId] [int] NOT NULL,
	[MongoPatientAllergyId] [varchar](50) NULL,
	[MongoReactionId] [varchar](50) NOT NULL,
	[ReactionId] [int] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_RPT_PatientAllergyReaction] PRIMARY KEY CLUSTERED 
(
	[AllergyReactionId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_Allergy](
	[AllergyId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[Name] [varchar](100) NULL,
	[CodingSystem] [varchar](50) NOT NULL,
	[CodingSystemCode] [varchar](50) NOT NULL,
	[Version] [float] NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[TTLDate] [datetime] NULL,
	[Delete] [varchar](50) NULL,
	[ExtraElements] [varchar](5000) NULL,
 CONSTRAINT [PK_RPT_Allergy] PRIMARY KEY CLUSTERED 
(
	[AllergyId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO	
CREATE TABLE [dbo].[RPT_AllergyType](
	[AllergyTypeId] [int] IDENTITY(1,1) NOT NULL,
	[AllergyId] [int] NULL,	
	[MongoAllergyId] [varchar](50) NOT NULL,
	[MongoTypeId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[UpdatedById] [int] NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedById] [int] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL,
 CONSTRAINT [PK_RPT_AllergyType] PRIMARY KEY CLUSTERED 
(
	[AllergyTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_PatientMedSupp](
	[PatientMedSuppId]		[int] IDENTITY(1,1) NOT NULL,
	[MongoId]				[varchar](50) NOT NULL,
	[PatientId]				[int] NOT NULL,
	[MongoPatientId]		[varchar](50) NOT NULL,
	[MongoFamilyId]			[varchar](50) NOT NULL,
	[Name]					[varchar] (200) NOT NULL,
	[Category]				[varchar] (200) NOT NULL,
	[MongoTypeId]			[varchar] (50) NOT NULL,
	[TypeId]				int NOT NULL,
	[Status]				[varchar] (200) NOT NULL,
	[Dosage]				[varchar] (500) NULL,
	[Strength]				[varchar] (200) NULL,
	[Route]					[varchar] (200) NULL,
	[Form]					[varchar] (200) NULL,
	[FreqQuantity]			[varchar] (200) NULL,
	[MongoFreqHowOftenId]	[varchar] (50) NULL,
	[FreqHowOftenId]		INT NULL,
	[MongoFreqWhenId]		[varchar] (50) NULL,
	[FreqWhenId]			INT NULL,
	[MongoSourceId]			[varchar](50) NOT NULL,
	[SourceId]				INT NULL,	
	[StartDate]				[datetime] NULL,
	[EndDate]				[datetime] NULL,
	[Reason]				[varchar](5000) NULL,
	[Notes]					[varchar](5000) NULL,	
	[PrescribedBy]			[varchar](500) NULL,	
	[SystemName]			[varchar](50) NULL,
	[MongoUpdatedBy]		[varchar](50) NULL,
	[UpdatedBy]				[int] NULL,
	[LastUpdatedOn]			[datetime] NULL,
	[MongoRecordCreatedBy]	[varchar](50) NULL,
	[RecordCreatedBy]		[int] NULL,
	[RecordCreatedOn]		[datetime] NULL,
	[Version]				[float] NULL,
	[TTLDate]				[DATETIME] NULL,
	[Delete]				[VARCHAR](50) NULL
 CONSTRAINT [PK_PatientMedSupp] PRIMARY KEY CLUSTERED 
(
	[PatientMedSuppId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_PatientMedSuppNDC](
	[NDCId] [int] IDENTITY(1,1) NOT NULL,
	[PatientMedSuppId] [int] NOT NULL,
	[MongoPatientMedSuppId] [varchar](50) NOT NULL,
	[NDC] [varchar] (200) NOT NULL
 CONSTRAINT [PK_PatientMedSuppNDC] PRIMARY KEY CLUSTERED 
(
	[NDCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE TABLE [dbo].[RPT_PatientMedSuppPhClass](
	[PhCId] [int] IDENTITY(1,1) NOT NULL,
	[PatientMedSuppId] [int] NOT NULL,
	[MongoPatientMedSuppId] [varchar](50) NOT NULL,
	[PharmClass] [varchar] (2000) NOT NULL
 CONSTRAINT [PK_RPT_PatientMedSuppPhClass] PRIMARY KEY CLUSTERED 
(
	[PhCId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]
GO