CREATE PROCEDURE [dbo].[spPhy_GetContractOrgFunctions](@ContractId int)
AS
BEGIN
	SET NOCOUNT ON
	SELECT o.OrgFunctionId, o.OrgFunctionName, o.[Description], o.DeleteFlag
	FROM OrgFunction o
	JOIN ContractOrgFunction co ON o.OrgFunctionId = co.OrgFunctionId
	WHERE co.ContractId = @ContractId
	AND o.DeleteFlag = 0
	AND co.DeleteFlag = 0
END
