
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Transition_Info')
	DROP TABLE [dbo].[RPT_Flat_Transition_Info]
GO