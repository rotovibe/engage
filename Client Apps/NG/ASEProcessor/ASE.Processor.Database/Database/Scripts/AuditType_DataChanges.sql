-- all products
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ExportPatientData')
	INSERT INTO AuditType (Name) VALUES ('ExportPatientData')
GO
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ExportSummaryData')
	INSERT INTO AuditType (Name) VALUES ('ExportSummaryData')
GO
-- campaign
IF NOT EXISTS(SELECT 1 FROM dbo.AuditType WHERE [Name] = 'ViewCampaignConfigurationData')
	Insert Into dbo.AuditType ([Name])
	VALUES ('ViewCampaignConfigurationData')
GO
-- patient management
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'AddCoordinateNote')
	INSERT INTO AuditType (Name) VALUES ('AddCoordinateNote')
GO
-- pqrs
IF NOT EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSPVSelectNew')
	INSERT INTO AuditType(Name)
	VALUES('ViewPatientsPQRSPVSelectNew')
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSPVLoadExisting')
	INSERT INTO AuditType(Name)
	VALUES('ViewPatientsPQRSPVLoadExisting')
GO
IF NOT EXISTS(SELECT TOP 1 1 FROM AuditType WHERE Name = 'ViewPatientsPQRSSubmissionSurvey')
	INSERT INTO AuditType(Name)
	VALUES('ViewPatientsPQRSSubmissionSurvey')
GO
-- transition
IF EXISTS(SELECT 1 FROM AuditType WHERE Name = 'ViewPatientFollowupDetail')
	DELETE AuditType WHERE [Name] = 'ViewPatientFollowupDetail'
GO
UPDATE AuditType SET Name = 'ViewPatientFollowupList' WHERE AuditTypeId = 36 /*FollowupList*/
GO
IF NOT EXISTS(SELECT 1 FROM AuditType WHERE Name = 'AddPatientFollowupDetailNote')
	INSERT INTO AuditType (Name) VALUES ('AddPatientFollowupDetailNote')
GO

