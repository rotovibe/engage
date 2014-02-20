--exec spPhy_GetUsers 6, NULL, '', NULL, NULL

CREATE PROCEDURE [dbo].[spPhy_GetUsers]
@ContractID INT,
@FirstName varchar(50) = NULL, 
@LastName varchar(50) = NULL, 
@Status varchar(10) = NULL, 
@Roles varchar(MAX) = NULL

AS

DECLARE @RolesXML as XML
DECLARE @Users TABLE (UserId uniqueidentifier, UserName nvarchar(256), DisplayName varchar(300), CreateDate datetime, Status varchar(50), IsLockedOut bit, StatusTypeId int, UserTypeId int, LastLoginDate datetime, Email varchar(250), IsNewUser int )

SET @RolesXML = @Roles

--Insert Roles
SELECT G.Item.query('.').value('.','uniqueidentifier') RoleId
INTO #RolesTbl
FROM @RolesXML.nodes('/roles/role') as G(Item)

--Insert Users that Meet Criteria
INSERT INTO @Users (UserId, UserName, DisplayName, CreateDate, Status, IsLockedOut, StatusTypeId, UserTypeId, LastLoginDate, Email, IsNewUser )
SELECT 
            u.UserId
            , u.UserName
            , u.FirstName + ' ' + u.LastName as DisplayName
            , m.CreateDate
            , m.[Status]
            , m.IsLockedOut
            , u.StatusTypeId
            , u.UserTypeId
            , m.LastLoginDate
            , m.Email
            , u.NewUser
      FROM [User] u with(nolock)
            INNER JOIN Membership m with(nolock) ON u.Userid = m.UserID
            INNER JOIN UserRole ur with(nolock) ON u.UserId = ur.UserId
            INNER JOIN UserContract uc with(nolock) ON u.UserId = uc.UserId
            INNER JOIN StatusType s with(nolock) ON s.StatusTypeId = u.StatusTypeId
      WHERE u.FirstName like COALESCE(@FirstName, u.FirstName) + '%'
            AND u.LastName like COALESCE(@LastName, u.LastName) + '%'
            AND uc.ContractId = @ContractID
            AND ((@Roles IS NULL) OR (ur.RoleId IN (SELECT DISTINCT(RoleId) FROM #RolesTbl)))

IF (@Status = 'Locked')
BEGIN
      SELECT DISTINCT UserId, UserName, DisplayName, CreateDate, 
            CASE WHEN IsLockedOut = 1 THEN [Status] + ' (Locked)' ELSE [Status] END as [Status], StatusTypeId, UserTypeId, LastLoginDate, Email, IsNewUser
      FROM @Users
      WHERE ([Status] = COALESCE(@Status, [Status]) OR IsLockedOut = 1)
END
ELSE
BEGIN
      SELECT DISTINCT UserId, UserName, DisplayName, CreateDate, 
            CASE WHEN IsLockedOut = 1 THEN [Status] + ' (Locked)' ELSE [Status] END as [Status], StatusTypeId, UserTypeId, LastLoginDate, Email, IsNewUser
      FROM @Users
      WHERE [Status] = COALESCE(@Status, [Status])
END

DROP TABLE #rolestbl
