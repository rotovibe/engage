IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_MaritalStatusLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_MaritalStatusLookUp]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_StatusReasonLookUp]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_StatusReasonLookUp]
GO

-- Rollback patient demographics properties in RPT_Patient table.
ALTER TABLE RPT_Patient
DROP COLUMN [DataSource],
COLUMN [MongoMaritalStatusId],
COLUMN [Protected],
COLUMN [Deceased],
COLUMN [Status],
COLUMN [MongoReasonId],
COLUMN [StatusDataSource]
GO

-- Rollback patient demographics properties in RPT_PatientInformation table.
ALTER TABLE RPT_PatientInformation
DROP COLUMN [DataSource],
COLUMN [MaritalStatus],
COLUMN [Protected],
COLUMN [Deceased],
COLUMN [Status],
COLUMN [Reason],
COLUMN [StatusDataSource],
COLUMN [Minor]
GO
