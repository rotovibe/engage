IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_ClinicalData]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_ClinicalData]
GO

CREATE TABLE [dbo].[RPT_Patient_ClinicalData](
	[PatientId] [int] NOT NULL,
	[FirstName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[Age] [tinyint] NULL,
	[Gender] [varchar](50) NULL,
	[Priority] [varchar](50) NULL,
	[SystemId] [varchar](50) NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[PatientObservationId] [int] NULL,
	[ObservationType] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[CodingSystem] [varchar](100) NULL,
	[CommonName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[Display] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[NumericValue] [float] NULL,
	[NonNumericValue] [varchar](50) NULL,
	[Source] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Type] [varchar](50) NULL,
	[Units] [varchar](50) NULL,
	[AdministeredBy] [varchar](50) NULL,
	[UpdatedBy] [varchar](100) NULL,
	[LastUpdatedOn] [datetime2](7) NULL,
	[CreatedBy] [varchar](100) NULL,
	[RecordCreatedOn] [datetime2](7) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientClinicalData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
AS
BEGIN
	DELETE [RPT_Patient_ClinicalData]
	INSERT INTO [RPT_Patient_ClinicalData]
	(
		 PatientId
		,FirstName				
		,MiddleName				
		,LastName				
		,DateOfBirth
		,Age
		,Gender
		,[Priority]
		,SystemId
		,Assigned_PCM
		,PatientObservationId
        ,ObservationType
		,Code
		,CodingSystem
        ,CommonName
        ,Description
        ,Display 
        ,StartDate
        ,EndDate
        ,NumericValue
        ,NonNumericValue
        ,Source
        ,[State]
        ,[Type]
        ,Units
        ,AdministeredBy
        ,UpdatedBy
        ,LastUpdatedOn 
        ,CreatedBy
        ,RecordCreatedOn
	) 
	SELECT DISTINCT 	
		p.PatientId
		,p.FirstName
		,p.MiddleName
		,p.LastName
		,p.DateOfBirth
		,CASE WHEN p.DATEOFBIRTH != '' AND ISDATE(p.DATEOFBIRTH) = 1 THEN  CAST(DATEDIFF(DAY, CONVERT(DATETIME,p.DATEOFBIRTH), GETDATE()) / (365.23076923074) AS INT) END as Age
		,p.Gender
		,p.[Priority]
		,ps.SystemId
		,u.PreferredName as Assigned_PCM
	    ,po.PatientObservationId
        ,otl.Name as ObservationType
		,o.Code
		,csl.Name as CodingSystem
        ,o.CommonName
        ,o.Description
        ,po.Display 
        ,po.StartDate
        ,po.EndDate
        ,po.NumericValue
        ,po.NonNumericValue
        ,po.Source
        ,po.[State]
        ,po.[Type]
        ,po.Units
        ,po.AdministeredBy
        ,u1.PreferredName as UpdatedBy
        ,po.LastUpdatedOn 
        ,u2.PreferredName as CreatedBy
        ,po.RecordCreatedOn
	FROM
		RPT_Patient as p with (nolock) 	
		LEFT JOIN RPT_PatientSystem as ps with (nolock) ON p.MongoId = ps.MongoPatientId
		LEFT JOIN RPT_CareMember as c with (nolock) on p.PatientId = c.PatientId
		LEFT JOIN RPT_User as u with (nolock) on c.UserId = u.UserId 
		LEFT JOIN RPT_PatientObservation as po with (nolock) on p.PatientId = po.PatientId and po.[Delete] = 'False' and po.TTLDate IS NULL
		LEFT JOIN RPT_User as u1 with (nolock) on po.UpdatedById = u1.UserId
		LEFT JOIN RPT_User as u2 with (nolock) on po.RecordCreatedById = u2.UserId
		LEFT JOIN RPT_Observation as o with (nolock) on po.ObservationId = o.ObservationId
		LEFT JOIN RPT_ObservationTypeLookUp as otl with (nolock) on o.ObservationTypeId = otl.ObservationTypeId
		LEFT JOIN RPT_CodingSystemLookUp as csl with (nolock) on o.CodingSystemId = csl.MongoId
	WHERE
		p.[Delete] = 'False' and p.TTLDate IS NULL
END

GO



