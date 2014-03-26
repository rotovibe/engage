-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/12/2011
-- Description:	Writes audit log for receiving
--				and error in the application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_LogAuditError]
	@UserId uniqueidentifier,
	@ImpersonatingUserId uniqueidentifier,
	@DateTimeStamp datetime,
	@SourcePage varchar(50),
	@SourceIp varchar(15),
	@Browser varchar(50),
	@SessionId varchar(100),
	@ContractId int,
	@AuditType varchar(50),
	@ErrorId varchar(20),
	@ErrorText varchar(max),
	@Source varchar(50),
	@StackTrace varchar(max)  
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	INSERT INTO [dbo].[AuditError]([AuditTypeId], [UserId], [ImpersonatingUserId], 
		[DateTimeStamp], [SourcePage], [SourceIp], [Browser], [SessionId], [ContractId], 
		[ErrorId], [ErrorText], [Source], [StackTrace])
	SELECT at.AuditTypeId
		, @UserId
		, CASE @ImpersonatingUserId WHEN '00000000-0000-0000-0000-000000000000' THEN NULL ELSE @ImpersonatingUserId END AS ImpersonatingUserId
		, @DateTimeStamp
		, @SourcePage
		, @SourceIp
		, @Browser
		, @SessionId
		, CASE @ContractId WHEN 0 THEN NULL ELSE @ContractId END AS ContractId
		, @ErrorId
		, @ErrorText
		, @Source
		, @StackTrace
	FROM AuditType at
	WHERE at.Name = @AuditType
				
END
