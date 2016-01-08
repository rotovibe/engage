
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_TaskStatistics')
	DROP TABLE [dbo].[RPT_Flat_TaskStatistics]
GO
