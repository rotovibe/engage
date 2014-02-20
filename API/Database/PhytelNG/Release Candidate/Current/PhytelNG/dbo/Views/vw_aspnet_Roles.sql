CREATE VIEW [dbo].[vw_aspnet_Roles]
  AS SELECT [dbo].[Role].[ApplicationId], [dbo].[Role].[RoleId], [dbo].[Role].[RoleName], [dbo].[Role].[LoweredRoleName], [dbo].[Role].[Description]
  FROM [dbo].[Role]
