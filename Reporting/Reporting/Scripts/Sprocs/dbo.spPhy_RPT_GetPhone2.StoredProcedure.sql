SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_GetPhone2]
	@contactid int
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @secondaryPhone varchar(50);
	DECLARE @rowcount int;
	
	SET @rowcount = (SELECT COUNT(*) from RPT_ContactPhone cp where cp.ContactId = @contactid) select @rowcount as [rowcount]
	
	IF @rowcount > 1
		BEGIN
			SET @secondaryPhone = (select cp.Number from RPT_ContactPhone cp where cp.PhoneId= (select max(ctp.PhoneId)-1 from RPT_ContactPhone ctp where ctp.ContactId = @contactid))
		END

	SELECT @secondaryPhone as [secondary]
END
GO
