SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_GetPreferredPhone]
	@contactid int
AS
BEGIN
	SET NOCOUNT ON;	
	DECLARE @PreferredPhone varchar(50)

	SELECT @PreferredPhone =
		cp.Number 
	FROM 
		RPT_ContactPhone cp 
	WHERE 
		cp.ContactId = @contactid
		AND cp.PhonePreferred = 'True'

	IF @PreferredPhone IS NULL
		BEGIN
			SET @PreferredPhone = 
			(	SELECT TOP 1 
					cp.Number 
				FROM 
					RPT_ContactPhone cp 
				WHERE 
					cp.ContactId = @contactid
					AND cp.PhonePreferred = 'False'
					AND cp.OptOut = 'False'
				ORDER BY cp.PhoneId DESC)
		END

	select @PreferredPhone
END
GO
