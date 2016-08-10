DELETE 
	AuditActionPatient 
WHERE AuditActionID in (
			SELECT 
				AuditActionID 
			FROM 
				AuditAction 
			WHERE AuditTypeID IN (SELECT 
									AuditTypeID 
								  FROM 
									AuditType 
								  WHERE Name IN ('ExportPatientData', 
												 'ExportSummaryData', 
												 'ViewCampaignConfigurationData', 
												 'ExportInsightComparisonPatientDetailsData', 
												 'AddCoordinateNote', 
												 'ViewPatientsPQRSPVSelectNew',
												 'ViewPatientsPQRSPVLoadExisting',
												 'ViewPatientsPQRSSubmissionSurvey',
												 'ViewPatientFollowupDetail',
												 'AddPatientFollowupDetailNote')))
GO											 
DELETE 
	AuditAction 
WHERE AuditActionID in (
			SELECT 
				AuditActionID 
			FROM 
				AuditAction 
			WHERE AuditTypeID IN (SELECT 
									AuditTypeID 
								  FROM 
									AuditType 
								  WHERE Name IN ('ExportPatientData', 
												 'ExportSummaryData', 
												 'ViewCampaignConfigurationData', 
												 'ExportInsightComparisonPatientDetailsData', 
												 'AddCoordinateNote', 
												 'ViewPatientsPQRSPVSelectNew',
												 'ViewPatientsPQRSPVLoadExisting',
												 'ViewPatientsPQRSSubmissionSurvey',
												 'ViewPatientFollowupDetail',
												 'AddPatientFollowupDetailNote')))										
GO
-- all products
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ExportPatientData')
	DELETE AuditType WHERE Name = 'ExportPatientData'
GO
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ExportSummaryData')
	DELETE AuditType WHERE Name = 'ExportSummaryData'
GO
-- campaign
IF EXISTS(SELECT 1 FROM dbo.AuditType WHERE [Name] = 'ViewCampaignConfigurationData')
	Delete from dbo.AuditType Where [Name] = 'ViewCampaignConfigurationData'
GO
-- insight (this was removed during development)
IF EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ExportInsightComparisonPatientDetailsData')
	DELETE FROM AuditType WHERE Name = 'ExportInsightComparisonPatientDetailsData'
GO
-- patient management
IF EXISTS(SELECT 1 FROM AuditType WHERE Name = 'AddCoordinateNote')
	DELETE FROM AuditType WHERE Name = 'AddCoordinateNote'
GO
-- pqrs
IF EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSPVSelectNew')
	DELETE FROM AuditType
	WHERE Name = 'ViewPatientsPQRSPVSelectNew'
GO
IF EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSPVLoadExisting')
	DELETE FROM AuditType
	WHERE Name = 'ViewPatientsPQRSPVLoadExisting'
GO
IF EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSSubmissionSurvey')
	DELETE FROM AuditType
	WHERE Name = 'ViewPatientsPQRSSubmissionSurvey'
GO
-- transition
IF EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ViewPatientFollowupDetail')
	DELETE FROM AuditType WHERE Name = 'ViewPatientFollowupDetail'
GO
UPDATE AuditType SET Name = 'FollowupList' WHERE AuditTypeId = 36 
GO
IF EXISTS(SELECT 1 FROM AuditType WHERE Name = 'AddPatientFollowupDetailNote')
	DELETE FROM AuditType WHERE Name = 'AddPatientFollowupDetailNote'
GO