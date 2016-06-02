----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_CareTeamFrequency')
	DROP TABLE [dbo].[RPT_CareTeamFrequency]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_ContactTypeLookUp')
	DROP TABLE [dbo].RPT_ContactTypeLookUp
GO