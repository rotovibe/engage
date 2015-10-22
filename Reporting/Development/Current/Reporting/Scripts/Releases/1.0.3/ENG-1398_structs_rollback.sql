IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Program_Enrollment_Beacon')
	DROP TABLE [dbo].[RPT_Flat_Program_Enrollment_Beacon]
GO