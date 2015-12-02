IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientClinicalData]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
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
        ,(CASE WHEN po.Display = '0' THEN  NULL ELSE po.Display END) as Display
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
-- Update the NumericValue column based on ObservationType and NonNumericValue.
UPDATE RPT_Patient_ClinicalData
SET NumericValue = NULL
WHERE (ObservationType = 'Problems') OR (ObservationType != 'Problems' AND NonNumericValue IS NOT NULL)
GO