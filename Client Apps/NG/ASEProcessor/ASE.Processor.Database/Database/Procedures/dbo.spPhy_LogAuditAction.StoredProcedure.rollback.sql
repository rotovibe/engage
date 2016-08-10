IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_LogAuditAction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_LogAuditAction]
GO


-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/12/2011
-- Description:	Writes audit log for taking an
--				action in the application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_LogAuditAction]
	@UserId uniqueidentifier,
	@ImpersonatingUserId uniqueidentifier,
	@DateTimeStamp datetime,
	@SourcePage varchar(50),
	@SourceIp varchar(15),
	@Browser varchar(50),
	@SessionId varchar(100),
	@ContractId int,
	@AuditType varchar(50),
	@EditedUserId uniqueidentifier,
	@EnteredUserName varchar(50),
	@SearchText varchar(max),
	@LandingPage varchar(50),
	@TOSVersion int,
	@NotificationTotal int,
	@DownloadedReport varchar(100),
	@Patients varchar(max)
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	INSERT INTO [dbo].[AuditAction]([UserId], [ImpersonatingUserId], [DateTimeStamp], [SourcePage], [SourceIp],
		[Browser], [SessionId], [ContractId], [AuditTypeId], [EditedUserId], [EnteredUserName], [SearchText],
		[LandingPage], [TOSVersion], [NotificationTotal], [DownloadedReport])
	SELECT @UserId
		, CASE @ImpersonatingUserId WHEN '00000000-0000-0000-0000-000000000000' THEN NULL ELSE @ImpersonatingUserId END AS ImpersonatingUserId
		, @DateTimeStamp
		, @SourcePage
		, @SourceIp
		, @Browser
		, @SessionId
		, CASE @ContractId WHEN 0 THEN NULL ELSE @ContractId END AS ContractId
		, at.AuditTypeId
		, CASE @EditedUserId WHEN '00000000-0000-0000-0000-000000000000' THEN NULL ELSE @EditedUserId END AS EditedUserId
		, @EnteredUserName
		, @SearchText
		, @LandingPage
		, CASE @TOSVersion WHEN 0 THEN NULL ELSE CAST(@TOSVersion AS int) END AS TOSVersion
		, CASE @NotificationTotal WHEN 0 THEN NULL ELSE @NotificationTotal END AS NotificationTotal
		, @DownloadedReport
	FROM AuditType at
	WHERE at.Name = @AuditType
	
	DECLARE @AuditActionId int
	SET @AuditActionId = @@IDENTITY
	
	IF (@Patients IS NOT NULL)
	BEGIN
		DECLARE @PatientTable TABLE(PatientId varchar(max))
		DECLARE @PatientId varchar(max)
		
		INSERT INTO @PatientTable(PatientId)
		SELECT value
		FROM fn_Split(@Patients, '|')	
		
		DECLARE curPatients CURSOR FOR 
		SELECT DISTINCT PatientId 
		FROM @PatientTable
		
		OPEN curPatients
		FETCH NEXT FROM curPatients INTO @PatientId
		
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO [dbo].[AuditActionPatient](AuditActionId, PatientId)
			SELECT @AuditActionId, @PatientId
			
			FETCH NEXT FROM curPatients INTO @PatientId
		END
		
		CLOSE curPatients
		DEALLOCATE curPatients
		
	END
		
END

GO