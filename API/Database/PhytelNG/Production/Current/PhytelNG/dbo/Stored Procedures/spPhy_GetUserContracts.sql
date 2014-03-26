-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/13/2011
-- Description:	Returns all of the ContractIds
--				for the given UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetUserContracts]
	@UserId UNIQUEIDENTIFIER
AS

BEGIN

	SET NOCOUNT ON
	
	SELECT uc.ContractId
		, c.Name
		, c.Number
		, uc.DefaultContract
		, srvs.IpAddress as [Server]
		, cd.DatabaseName as [Database]
		, cd.UserName
		, cd.[Password]
		, c.ContractID as PhytelContractId
		, 0 as DefaultContract
	FROM UserContract uc with(nolock)
		INNER JOIN Contract c with(nolock) on c.ContractId = uc.ContractId
		INNER JOIN ContractDatabase cd with(nolock) on c.ContractId = cd.ContractId
		INNER JOIN [Servers] srvs with(nolock) on cd.ServerId = srvs.ServerId
	WHERE cd.DatabaseType = 'SQL' And cd.SystemType = 'Contract'
	AND uc.UserId = @UserId

END
