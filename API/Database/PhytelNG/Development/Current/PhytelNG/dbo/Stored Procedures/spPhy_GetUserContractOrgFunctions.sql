CREATE PROCEDURE [dbo].[spPhy_GetUserContractOrgFunctions](@UserId uniqueidentifier, @ContractId int)
AS
BEGIN
	SET NOCOUNT ON
	SELECT o.OrgFunctionId, o.OrgFunctionName, o.[Description], o.DeleteFlag
	FROM OrgFunction o
	JOIN ContractOrgFunction co ON o.OrgFunctionId = co.OrgFunctionId
	JOIN UserContractOrgFunction uco ON co.ContractOrgFunctionId = uco.ContractOrgFunctionId
	WHERE uco.UserId = @UserId 
	AND co.ContractId = @ContractId
	AND co.DeleteFlag = 0
	AND o.DeleteFlag = 0	
END
