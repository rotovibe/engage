-- =============================================
-- Author:		CHill
-- Create date: 4/13/2011
-- Description:	gets user password attributes
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetUser] @UserName nvarchar(256)
AS
BEGIN
SET NOCOUNT ON;

Declare @CurrentTOSID int
Declare @AcceptedTOS bit
Declare @LastTOSVersion int

SELECT	@LastTOSVersion = COALESCE(Max(tos.Version), 0)
FROM	TermsOfService tos with(nolock)
	INNER JOIN UserTermsOfService uts with(nolock) on tos.TermsOfServiceID = uts.TermsOfServiceID
	INNER JOIN [User] u with(nolock) on uts.UserId = u.UserId
WHERE u.UserName = @UserName

SELECT @CurrentTOSID = x.TermsOfServiceID
FROM (	SELECT TermsOfServiceID, ROW_NUMBER() OVER (ORDER BY [Version] DESC) AS Seq
		FROM TermsOfService with(nolock)) x
WHERE x.Seq = 1

If Exists(
	Select 1 From UserTermsOfService uts with(nolock)
		Inner Join [User] u with(nolock) on uts.UserId = u.UserId
	Where u.UserName = @UserName
	And uts.TermsOfServiceID = @CurrentTOSID)

	Set @AcceptedTOS = 1
Else
	Set @AcceptedTOS = 0

SELECT 
	@UserName as UserName
	, m.PasswordExpiration
	, m.PasswordQuestion
	, m.PasswordAnswer
	, m.AdministratorUserId	
	, @AcceptedTOS As AcceptedLatestTOS
	, @LastTOSVersion as LastTOSVersion
	, u.NewUser
	, u.DisplayName
	, u.UserId
	, u.FirstName
	, u.MiddleName
	, u.LastName
	, u.Phone
	, u.Ext
	, CASE WHEN u.SessionTimeout > 0 THEN u.SessionTimeout ELSE 480 END as SessionTimeout
	, u.UserTypeId
	, m.FailedPasswordAttemptCount
	, m.FailedPasswordAnswerAttemptCount
	, s.Name as [Status]
	, u.StatusTypeId
	, m.AdministratorUserId
	, u2.FirstName + ' ' + u2.LastName as AdminUserName
FROM Membership m with(nolock)
	INNER JOIN [User] u with(nolock) ON m.UserId = u.UserId
	INNER JOIN StatusType s with(nolock) ON s.StatusTypeId = u.StatusTypeId
	LEFT OUTER JOIN [User] u2 with (nolock) ON u2.UserId = m.AdministratorUserId
	
WHERE u.UserName = @UserName

END
