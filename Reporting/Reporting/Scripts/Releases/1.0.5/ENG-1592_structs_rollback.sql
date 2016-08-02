
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_BarrierStatistics')
	DROP TABLE [dbo].[RPT_Flat_BarrierStatistics]
GO
