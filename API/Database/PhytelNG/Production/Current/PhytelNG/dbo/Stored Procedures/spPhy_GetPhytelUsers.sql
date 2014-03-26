CREATE PROCEDURE [dbo].[spPhy_GetPhytelUsers]
@FirstName varchar(50), 
@LastName varchar(50), 
@Status varchar(10), 
@Sort varchar(20)
AS

DECLARE @Users TABLE (UserId uniqueidentifier, UserName nvarchar(256), DisplayName varchar(300), CreateDate datetime, Status varchar(10), StatusTypeId int, Sort varchar(200), UserTypeId int, LastLoginDate datetime, Email varchar(250), IsNewUser int)

INSERT INTO @Users (UserId, UserName, DisplayName, CreateDate, Status, StatusTypeId, Sort, UserTypeId, LastLoginDate, Email, IsNewUser)
SELECT u.UserId, u.UserName, u.FirstName + ' ' + u.LastName as DisplayName, m.CreateDate, 
		CASE	WHEN m.IsLockedOut = 1 THEN 'Locked'
				ELSE m.Status
		END, 
		u.StatusTypeId,
		CASE	WHEN @Sort = 'Name' THEN u.FirstName
				WHEN @Sort = 'Status' THEN m.Status
				WHEN @Sort = 'Create Date' THEN CAST(m.CreateDate as varchar)
				ELSE u.DisplayName
		END,
		u.UserTypeId
		, m.LastLoginDate
            , m.Email
            , u.NewUser
	FROM [User] u with(nolock)
		INNER JOIN Membership m with(nolock) ON u.Userid = m.UserID
		INNER JOIN StatusType s with(nolock) ON s.StatusTypeId = u.StatusTypeId
	WHERE u.FirstName like COALESCE(@FirstName, u.FirstName) + '%'
		AND u.LastName like COALESCE(@LastName, u.LastName) + '%'
		AND u.UserTypeId = 1
		AND m.[Status] = COALESCE(@Status, m.[Status])
		
	ORDER BY CASE	WHEN @Sort = 'Name' THEN u.FirstName
					WHEN @Sort = 'Status' THEN m.Status
					WHEN @Sort = 'Create Date' THEN CAST(m.CreateDate as varchar)
					ELSE u.DisplayName
			END

SELECT DISTINCT UserId, UserName, DisplayName, CreateDate, Status, StatusTypeId, Sort, UserTypeId, LastLoginDate, Email, IsNewUser
FROM @Users
ORDER BY Sort
