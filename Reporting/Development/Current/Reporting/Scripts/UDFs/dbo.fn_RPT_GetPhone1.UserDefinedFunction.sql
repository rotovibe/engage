SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_GetPhone1] 
(	
	@contactid INT
)
RETURNS VARCHAR(50) 
AS
BEGIN
	DECLARE @PreferredPhone varchar(50)

--	DECLARE @contactid int;
--	SET @contactid = 14;
--	SELECT cp.Number FROM RPT_ContactPhone cp WHERE cp.ContactId = @contactid --ORDER BY cp.PhonePreferred 
	set @PreferredPhone = (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp WHERE cp.ContactId = @contactid AND cp.OptOut != 'True') -- AND cp.PhonePreferred = 'True'
--	select @PreferredPhone

	--IF @PreferredPhone IS NULL
	--	BEGIN
	--		SET @PreferredPhone = 
	--		(	SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp 
	--			WHERE 
	--				cp.ContactId = @contactid
	--				AND cp.PhonePreferred = 'False'
	--				AND cp.OptOut = 'False'
	--			ORDER BY cp.PhoneId DESC)
	--	END

	RETURN @PreferredPhone
	
END
GO
