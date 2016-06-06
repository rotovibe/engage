----------------------------------------------------------------------------------------------------------------------------------
--ENG-2010
----------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_CareTeamFrequency')
	DROP TABLE [dbo].[RPT_CareTeamFrequency]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_ContactTypeLookUp')
	DROP TABLE [dbo].[RPT_ContactTypeLookUp]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_CareTeam')
	DROP TABLE [dbo].[RPT_CareTeam]
GO

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_Flat_Assigned_PCM')
	DROP TABLE [dbo].[RPT_Flat_Assigned_PCM]
GO
