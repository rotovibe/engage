-- =============================================
-- Author:		Tom Gindrup
-- Create date: 03/12/2013
-- Description:	Sets User Contract Property
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetUserContractProperty]
	@UserId uniqueidentifier,
	@ContractId INT,
	@key varchar(50),
	@value varchar(max)
AS

BEGIN

	IF EXISTS (SELECT TOP 1 1 FROM [UserContractProperty] WHERE UserId = @UserId AND ContractId = @ContractId AND [Key] = @key)
	BEGIN
	UPDATE 
		[UserContractProperty]
	SET 
		Value = @value
	WHERE 
		UserId = @UserId
		AND ContractId = @ContractId
		AND [Key] = @key
	END
	ELSE
	BEGIN
	INSERT INTO [UserContractProperty] ([UserId], [ContractId], [Key], [Value])
	SELECT @UserId, @ContractId, @key, @value
	END
END
