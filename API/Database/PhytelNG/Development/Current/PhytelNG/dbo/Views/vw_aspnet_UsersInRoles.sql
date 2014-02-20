CREATE VIEW [dbo].[vw_aspnet_UsersInRoles]
  AS SELECT [dbo].[UserRole].[UserId], [dbo].[UserRole].[RoleId]
  FROM [dbo].[UserRole]
