CREATE VIEW [dbo].[vw_aspnet_Applications]
  AS SELECT [dbo].[Application].[ApplicationName], [dbo].[Application].[LoweredApplicationName], [dbo].[Application].[ApplicationId], [dbo].[Application].[Description]
  FROM [dbo].[Application]
