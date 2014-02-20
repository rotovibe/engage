CREATE VIEW [dbo].[vw_aspnet_Users]
  AS SELECT [dbo].[User].[ApplicationId], [dbo].[User].[UserId], [dbo].[User].[UserName], [dbo].[User].[LoweredUserName], [dbo].[User].[MobileAlias], [dbo].[User].[IsAnonymous], [dbo].[User].[LastActivityDate]
  FROM [dbo].[User]
