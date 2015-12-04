IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_VisitTypeLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_VisitTypeLookUp]
GO

CREATE TABLE [dbo].[RPT_VisitTypeLookUp](
	[VisitTypeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_VisitTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[VisitTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_UtilizationLocationLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_UtilizationLocationLookUp]
GO

CREATE TABLE [dbo].[RPT_UtilizationLocationLookUp](
	[UtilizationLocationId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_UtilizationLocationLookUp] PRIMARY KEY CLUSTERED 
(
	[UtilizationLocationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_DispositionLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_DispositionLookUp]
GO

CREATE TABLE [dbo].[RPT_DispositionLookUp](
	[DispositionId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_DispositionLookUp] PRIMARY KEY CLUSTERED 
(
	[DispositionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_UtilizationSourceLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_UtilizationSourceLookUp]
GO

CREATE TABLE [dbo].[RPT_UtilizationSourceLookUp](
	[UtilizationSourceId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_UtilizationSourceLookUp] PRIMARY KEY CLUSTERED 
(
	[UtilizationSourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF NOT EXISTS (SELECT * FROM [dbo].[RPT_SprocNames] WHERE SprocName = 'spPhy_RPT_Flat_PatientUtilization_Dim')
BEGIN
	INSERT INTO [dbo].[RPT_SprocNames]([SprocName],[Prerequire],[Description])
	VALUES ('spPhy_RPT_Flat_PatientUtilization_Dim', 0, null)	
END
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilization]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilization]
GO

CREATE TABLE [dbo].[RPT_PatientUtilization](
	 [PatientUtilizationId] [int] IDENTITY(1,1) NOT NULL,
	 [MongoId] [varchar](50) NOT NULL,
	 [MongoPatientId] [varchar](50) NOT NULL,
     [MongoNoteTypeId] [varchar](50) NULL,
	 [Reason] [varchar](max) NULL,
	 [MongoVisitTypeId] [varchar](50) NULL,
	 [OtherVisitType] [varchar](100) NULL,
	 [AdmitDate] [datetime] NULL,
	 [Admitted] [varchar](50) NULL,
	 [DischargeDate] [datetime] NULL,
	 [MongoLocationId] [varchar](50) NULL,
	 [OtherLocation] [varchar](100) NULL,
	 [MongoDispositionId] [varchar](50) NULL,
	 [OtherDisposition] [varchar](100) NULL,	
	 [MongoUtilizationSourceId] [varchar](50) NULL,
	 [DataSource] [varchar](50) NULL,
	 [MongoUpdatedBy] [varchar](50) NULL,
	 [LastUpdatedOn] [datetime] NULL,
	 [MongoRecordCreatedBy] [varchar](50) NULL,
	 [RecordCreatedOn] [datetime] NULL,
	 [Version] [float] NULL,
	 [Delete] [varchar](50) NULL
 CONSTRAINT [PK_PatientUtilization] PRIMARY KEY CLUSTERED 
(
	[PatientUtilizationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilizationProgram]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilizationProgram]
GO

CREATE TABLE [dbo].[RPT_PatientUtilizationProgram](
	[PatientUtilizationProgramId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[MongoPatientUtilizationId] [varchar](50) NOT NULL,
	[MongoUpdatedBy] [varchar](50) NULL,
	[LastUpdatedOn] [datetime] NULL,
	[MongoRecordCreatedBy] [varchar](50) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Version] [float] NULL
 CONSTRAINT [PK_PatientUtilizationProgram] PRIMARY KEY CLUSTERED 
(
	[PatientUtilizationProgramId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientUtilization_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientUtilization_Dim]
GO

CREATE TABLE [dbo].[RPT_PatientUtilization_Dim](
	[DimId] [int] IDENTITY(1,1) NOT NULL,
	[PatientUtilizationId] [int] NULL,
	[MongoPatientUtilizationId] [varchar](50) NULL,
	[NoteType] [varchar](100) NULL,
	[Reason] [varchar](max) NULL,
	[VisitType] [varchar](100) NULL,
	[OtherVisitType] [varchar](100) NULL,
	[AdmitDate] [datetime] NULL,
	[Admitted] [varchar](50) NULL,
	[DischargeDate] [datetime] NULL,
	[Length] [int] NULL,
	[Location] [varchar](100) NULL,
	[OtherLocation] [varchar](100) NULL,
	[Disposition] [varchar](100) NULL,
	[OtherDisposition] [varchar](100) NULL,	
	[UtilizationSource] [varchar](100) NULL,
	[DataSource] [varchar](50) NULL,
	[ProgramName] [varchar](200) NULL,
	[PatientId] [int] NULL,
	[MongoPatientId] [varchar](50) NULL,
	[FirstName] [varchar](200) NULL,
	[MiddleName] [varchar](200) NULL,
	[LastName] [varchar](200) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Age] [int] NULL,
	[Gender] [varchar](100) NULL,
	[Priority] [varchar](100) NULL,
	[SystemId] [varchar](100) NULL,
	[Assigned_PCM] [varchar](500) NULL,
	[AssignedTo] [varchar](500) NULL,
	[State] [varchar](100) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[AssignedOn] [datetime] NULL,
	[RecordCreatedOn] [datetime] NULL,
	[RecordCreated_By] [varchar](200) NULL,
	[RecordUpdatedOn] [datetime] NULL,
	[RecordUpdatedBy] [varchar](200) NULL
	 CONSTRAINT [PK_PatientUtilization_Dim] PRIMARY KEY CLUSTERED 
(
	[DimId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO







