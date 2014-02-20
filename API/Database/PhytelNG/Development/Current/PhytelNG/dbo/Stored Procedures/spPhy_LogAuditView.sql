-- =============================================
-- Author:		Josh Gattis
-- Create date: 05/11/2011
-- Description:	Writes audit log for Viewing
--				pages in the application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_LogAuditView]
	@UserId uniqueidentifier,
	@ImpersonatingUserId uniqueidentifier,
	@DateTimeStamp datetime,
	@SourcePage varchar(50),
	@SourceIp varchar(15),
	@Browser varchar(50),
	@SessionId varchar(100),
	@ContractId int
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
		
	INSERT INTO [dbo].[AuditView]([UserId], [ImpersonatingUserId], [DateTimeStamp], [SourcePage], [SourceIp],
		[Browser], [SessionId], [ContractId])
	SELECT @UserId
		, CASE @ImpersonatingUserId WHEN '00000000-0000-0000-0000-000000000000' THEN NULL ELSE @ImpersonatingUserId END AS ImpersonatingUserId
		, @DateTimeStamp
		, @SourcePage
		, @SourceIp
		, @Browser
		, @SessionId
		, CASE @ContractId WHEN 0 THEN NULL ELSE @ContractId END AS ContractId
		
END
