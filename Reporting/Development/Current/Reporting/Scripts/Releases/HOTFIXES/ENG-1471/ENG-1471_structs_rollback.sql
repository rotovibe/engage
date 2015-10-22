DROP TABLE RPT_System;
GO

/* RPT_PatientSystem */
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'Value' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem] DROP COLUMN [Value];
	END
GO
	
IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'DataSource' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem]  DROP COLUMN [DataSource];
	END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'Status' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem]  DROP COLUMN [Status];
	END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'Primary' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem]  DROP COLUMN [Primary];
	END
GO

IF EXISTS(SELECT * FROM sys.columns WHERE Name = N'SysId' AND Object_ID = Object_ID(N'RPT_PatientSystem'))
	BEGIN
		ALTER TABLE [dbo].[RPT_PatientSystem]  DROP COLUMN [SysId];
	END
GO
	
/* RPT_PatientInformation */
ALTER TABLE 
	[dbo].[RPT_PatientInformation] 
DROP COLUMN  
	[PrimaryId],
	[PrimaryIdSystem],
	[EngageId]