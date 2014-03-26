-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/31/2011
-- Description:	Performs multiple cleanup tasks
--				for C3
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_PhytelMaintenance]
AS

BEGIN

	SET NOCOUNT ON
	
	DECLARE @ArchiveAuditPeriod int
	DECLARE @ResetLockoutPeriod int
	DECLARE @UserInactivePeriod int
	DECLARE @InActiveStatusTypeId int
	DECLARE @ActiveStatusTypeId int
	
	SELECT @ArchiveAuditPeriod = [Value] FROM ApplicationSetting WHERE [Key] = 'ARCHIVE_AUDIT_PERIOD'
	SELECT @ResetLockoutPeriod = [Value] FROM ApplicationSetting WHERE [Key] = 'RESET_LOCKOUT'
	SELECT @UserInactivePeriod = [Value] FROM ApplicationSetting WHERE [Key] = 'DAYS_TO_INACTIVE'
	
	SELECT @InActiveStatusTypeId = StatusTypeId From StatusType Where [Name] = 'Inactive'
	SELECT @ActiveStatusTypeId = StatusTypeId From StatusType Where [Name] = 'Active'
	
	Update m
	Set m.[Status] = 'Inactive'
	From [Membership] m
		Inner Join [User] u on m.UserId = u.UserId
	Where COALESCE(m.LastLoginDate, GETDATE()) <= DATEADD(dd, -@UserInactivePeriod, CONVERT(VARCHAR(10), GETDATE(), 101))
	And u.StatusTypeId = @ActiveStatusTypeId

	Update u
	Set u.StatusTypeId = @InActiveStatusTypeId
	From [User] u
	Inner Join [Membership] m on u.UserId = m.UserId
	Where COALESCE(m.LastLoginDate, GETDATE()) <= DATEADD(dd, -@UserInactivePeriod, CONVERT(VARCHAR(10), GETDATE(), 101))
	And u.StatusTypeId = @ActiveStatusTypeId

	UPDATE [Membership]
	SET IsLockedOut = 0
	WHERE COALESCE(DATEADD(minute, DATEDIFF(minute,GETUTCDATE(),GETDATE()), LastLockoutDate), GETDATE()) <= DATEADD(mi, -@ResetLockoutPeriod, GETDATE())
		AND (IsLockedOut = 1)
	
	DELETE
	FROM AuditView
	WHERE COALESCE(DateTimeStamp, GETDATE()) < DATEADD(dd, -@ArchiveAuditPeriod, CONVERT(VARCHAR(10), GETDATE(), 101))

	DELETE
	FROM AuditError
	WHERE COALESCE(DateTimeStamp, GETDATE()) < DATEADD(dd, -@ArchiveAuditPeriod, CONVERT(VARCHAR(10), GETDATE(), 101))

	DELETE
	FROM AuditAction
	WHERE COALESCE(DateTimeStamp, GETDATE()) < DATEADD(dd, -@ArchiveAuditPeriod, CONVERT(VARCHAR(10), GETDATE(), 101))

	DELETE
	FROM APIUserToken
	WHERE COALESCE(CreatedOn, GETDATE()) < DATEADD(dd, -1, CONVERT(VARCHAR(10), GETDATE(), 101))
		
END
