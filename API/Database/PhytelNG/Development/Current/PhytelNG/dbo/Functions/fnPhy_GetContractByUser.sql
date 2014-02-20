-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fnPhy_GetContractByUser] 
(
	-- Add the parameters for the function here
	@UserId UniqueIdentifier
)
RETURNS VARCHAR(1000)
AS
BEGIN
	DECLARE @contracts varchar(1000)
	
	SELECT 
		@contracts = COALESCE(@contracts + '', '', '''') + Name 
	FROM 
		UserContract uc 
		INNER JOIN [Contract] c
		ON uc.ContractId = c.ContractId 
	WHERE uc.UserId = @UserId	

	-- Return the result of the function
	RETURN @contracts

END
