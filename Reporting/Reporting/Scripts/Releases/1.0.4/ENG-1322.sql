IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_MaritalStatusLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_MaritalStatusLookUp]
GO

CREATE TABLE [dbo].[RPT_MaritalStatusLookUp](
	[MaritalStatusId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_MaritalStatusLookUp] PRIMARY KEY CLUSTERED 
(
	[MaritalStatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_StatusReasonLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_StatusReasonLookUp]
GO

CREATE TABLE [dbo].[RPT_StatusReasonLookUp](
	[StatusReasonId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](max) NULL,
	[Name] [varchar](max) NULL,
	[Default] [varchar](50) NULL,
	[Active] [varchar](50) NULL,
 CONSTRAINT [PK_StatusReasonLookUp] PRIMARY KEY CLUSTERED 
(
	[StatusReasonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

-- Adding the patient demographic fields in RPT_ table.
ALTER TABLE RPT_Patient
ADD [DataSource] [varchar](50) NULL,
	[MongoMaritalStatusId] [varchar](50) NULL,
	[Protected] [varchar](50) NULL,
	[Deceased] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[MongoReasonId] [varchar](50) NULL,
	[StatusDataSource] [varchar](50) NULL
GO

-- Adding the patient demographic fields in RPT_PatientInformation table.
ALTER TABLE RPT_PatientInformation
ADD [DataSource] [varchar](50) NULL,
	[MaritalStatus] [varchar](50) NULL,
	[Protected] [varchar](50) NULL,
	[Deceased] [varchar](50) NULL,
	[Status] [varchar](50) NULL,
	[Reason] [varchar](50) NULL,
	[StatusDataSource] [varchar](50) NULL,
	[Minor] [varchar](50) NULL
GO