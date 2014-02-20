CREATE VIEW [dbo].[vw_aspnet_WebPartState_Paths]
  AS SELECT [dbo].[Path].[ApplicationId], [dbo].[Path].[PathId], [dbo].[Path].[Path], [dbo].[Path].[LoweredPath]
  FROM [dbo].[Path]
