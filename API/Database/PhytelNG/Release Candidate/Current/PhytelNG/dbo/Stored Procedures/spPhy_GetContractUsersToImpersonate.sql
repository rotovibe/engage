-- =============================================
-- Author:		FRED FULCHER
-- Create date: 05/04/2011
-- Update Date: 12/13/2011
-- Description:	Returns users tied to contracts
--				that can be impersonated
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetContractUsersToImpersonate]
@ContractId INT
AS

BEGIN

	--DECLARE @ContractId INT
	
	--SET @ContractId = 447
	
	SELECT
		u.UserId
		,u.FirstName + ' ' + COALESCE(u.MiddleName, '') + ' ' + u.LastName as DisplayName
		, [C3].[dbo].[fnPhy_GetContractByUser](u.UserId) as ContractName
		,u.Username
		,st.Name as UserStatus
		,st.StatusTypeId
	FROM [User] u with(nolock)
		INNER JOIN UserContract uc with(nolock) ON uc.UserId = u.UserId
		INNER JOIN [Contract] c with(nolock) ON c.ContractId = uc.ContractId
		INNER JOIN UserType ut with(nolock) on u.UserTypeId = ut.UserTypeId	
		INNER JOIN StatusType st with(nolock) on st.StatusTypeId = u.StatusTypeId 
	WHERE uc.ContractId = @ContractId
	AND	ut.Name = 'ContractUser' --Contract Users Only
			
	SELECT
		ur.UserId
		,ur.RoleId
		,RoleName
	FROM 
		UserRole ur
	INNER JOIN
		[Role] r with(nolock)
		ON r.RoleId = ur.RoleId
	WHERE r.ContractId = @ContractId
	
	SELECT
		uc.UserId
		,uc.ContractId
		,c.Name
		,c.Number		
	FROM
		UserContract uc with(nolock)
	INNER JOIN
		[Contract] c
		ON c.ContractId = uc.ContractId
	WHERE c.ContractId = @ContractId		
	
END
