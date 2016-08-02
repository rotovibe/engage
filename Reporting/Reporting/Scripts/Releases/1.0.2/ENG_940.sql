/****** Object:  Table [dbo].[RPT_NoteDurationLookUp]    Script Date: 06/23/2015 21:46:45 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_NoteTypeLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_NoteTypeLookUp]
GO

CREATE TABLE [dbo].[RPT_NoteTypeLookUp](
	[NoteTypeId] [int] IDENTITY(1,1) NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
	[LookUpType] [varchar](200) NULL,
	[Name] [varchar](200) NULL,
 CONSTRAINT [PK_NoteTypeLookUp] PRIMARY KEY CLUSTERED 
(
	[NoteTypeId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON, FILLFACTOR = 90) ON [PRIMARY]
) ON [PRIMARY]

GO

select * from RPT_NoteTypeLookUp
select * from RPT_ProcessAudit order by Start desc

CREATE NONCLUSTERED INDEX IX_RPT_NoteTypeLookUp_Compound 
    ON [RPT_NoteTypeLookUp] 
    (
		[MongoId]
		,[Name]
    ) ON [PRIMARY]; 
GO

/****** Object:  Table [dbo].[RPT_TouchPoint_Dim]    Script Date: 06/23/2015 22:57:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_PatientNotes_Dim]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_PatientNotes_Dim]
GO

CREATE TABLE [dbo].[RPT_PatientNotes_Dim](
	[DimId] [int] IDENTITY(1,1) NOT NULL,
	[PatientNoteId] [int] NULL,
	[MongoPatientId] [varchar](50) NULL,	
	[Type] [varchar](50) NULL,
	[Method] [varchar](100) NULL,
	[Who] [varchar](100) NULL,
	[Source] [varchar](100) NULL,
	[Outcome] [varchar](100) NULL,
	[ContactedOn] [datetime] NULL,
	[ProgramName] [varchar](200) NULL,
	[Duration] [varchar](100) NULL,
	[ValidatedIntentity] [varchar](100) NULL,
	[Text] [varchar](5000) NULL,
	[RecordCreatedOn] [datetime] NULL,
	[Record_Created_By] [varchar](200) NULL,
	[PATIENTID] [int] NULL,
	[FIRSTNAME] [varchar](200) NULL,
	[MIDDLENAME] [varchar](200) NULL,
	[LASTNAME] [varchar](200) NULL
) ON [PRIMARY]
SET ANSI_PADDING ON
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [DATEOFBIRTH] [varchar](50) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [AGE] [int] NULL
SET ANSI_PADDING OFF
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [GENDER] [varchar](100) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [PRIORITY] [varchar](100) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [SYSTEMID] [varchar](100) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [ASSIGNED_PCM] [varchar](500) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [ASSIGNEDTO] [varchar](500) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [State] [varchar](100) NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [StartDate] [datetime] NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [EndDate] [datetime] NULL
ALTER TABLE [dbo].[RPT_PatientNotes_Dim] ADD [AssignedOn] [datetime] NULL

GO

/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Flat_TouchPoint_Dim]    Script Date: 06/23/2015 16:35:48 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_PatientNotes_Dim]') AND type in (N'P', N'PC'))
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
	[Duration],
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
	[AssignedOn]		
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
		(SELECT DISTINCT Name FROM RPT_NoteDurationLookUp nd WHERE nd.MongoId = pn.MongoDurationId) as [Duration],
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

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_PatientNotes_Dim';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_PatientNotes_Dim', 'false');
GO

