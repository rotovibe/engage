SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_GetPhone2] 
(	
	@contactid INT
)
RETURNS VARCHAR(50) 
AS
BEGIN
	DECLARE @secondaryPhone varchar(50);
	DECLARE @rowcount int;
	
	SET @rowcount = (SELECT COUNT(*) from RPT_ContactPhone cp where cp.ContactId = @contactid)
	
	IF @rowcount > 1
		BEGIN
			DECLARE @TempTable TABLE(PhoneId int, Number varchar(50), contactId int, preferred varchar(50))
			INSERT INTO @TempTable (PhoneId, Number, contactId, preferred) 
				SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp where cp.ContactId = @contactid AND cp.OptOut != 'True';
					
			SET @secondaryPhone = (SELECT TOP 1 t.Number from ( SELECT TOP 2 PhoneId, Number, contactId FROM @TempTable ORDER BY PhoneId DESC) as t)
		END
	return @secondaryPhone;
END
GO
