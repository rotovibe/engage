IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Latest_PatientObservations')
	DROP TABLE [dbo].[RPT_Flat_Latest_PatientObservations]
GO


DELETE [dbo].[RPT_SprocNames]
WHERE SprocName = 'spPhy_RPT_Flat_Latest_PatientObservations'
GO