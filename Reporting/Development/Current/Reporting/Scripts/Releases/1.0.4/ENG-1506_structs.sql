IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Latest_PatientObservations')
	DROP TABLE [dbo].[RPT_Flat_Latest_PatientObservations]
GO

CREATE TABLE [dbo].[RPT_Flat_Latest_PatientObservations](
	MongoPatientId [varchar](50) NULL,
	[ObservationType] [varchar](100) NULL,
	[Code] [varchar](100) NULL,
	[CommonName] [varchar](100) NULL,
	[Description] [varchar](max) NULL,
	[State] [varchar](50) NULL,
	[StartDate] [datetime2](7) NULL,
	[EndDate] [datetime2](7) NULL,
	[NumericValue] [float] NULL,
	[NonNumericValue] [varchar](50) NULL,
	PrimaryCareManagerMongoId [varchar](50) NULL,
	PrimaryCareManagerFirstName [varchar](100) NULL,
	PrimaryCareManagerLastName [varchar](100) NULL,
	PrimaryCareManagerPreferredName [varchar](100) NULL,
) ON [PRIMARY]
GO

DELETE FROM RPT_SprocNames WHERE SprocName = 'spPhy_RPT_Flat_Latest_PatientObservations';
GO
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Latest_PatientObservations', 'false');
GO