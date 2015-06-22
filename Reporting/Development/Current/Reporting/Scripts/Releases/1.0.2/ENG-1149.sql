IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_Patient_ClinicalData]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_Patient_ClinicalData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING OFF
GO

CREATE TABLE [dbo].[RPT_Patient_ClinicalData](
	[MongoPatientId] [varchar](50) NOT NULL,
	[PatientObservationId] [int] NOT NULL,
	[MongoId] [varchar](50) NOT NULL,
    [MongoObservationId] [varchar](50) NOT NULL,
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

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
AS
BEGIN
	DELETE [RPT_Patient_ClinicalData]
	INSERT INTO [RPT_Patient_ClinicalData]
	(
		 MongoPatientId
		,PatientObservationId
		,MongoId
	    ,MongoObservationId
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
		 po.MongoPatientId
	    ,po.PatientObservationId
	    ,po.MongoId
		,po.MongoObservationId
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
		RPT_PatientObservation as po with (nolock) 	
		LEFT JOIN RPT_User as u1 with (nolock) on po.MongoUpdatedBy = u1.MongoId
		LEFT JOIN RPT_User as u2 with (nolock) on po.MongoRecordCreatedBy = u2.MongoId
		LEFT JOIN RPT_Observation as o with (nolock) on po.MongoObservationId = o.MongoId
		LEFT JOIN RPT_ObservationTypeLookUp as otl with (nolock) on o.MongoObservationLookUpId = otl.MongoId
		LEFT JOIN RPT_CodingSystemLookUp as csl with (nolock) on o.CodingSystemId = csl.MongoId
	WHERE
		po.[Delete] = 'False' and po.TTLDate IS NULL
END



GO


