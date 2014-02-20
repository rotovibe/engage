CREATE FUNCTION [dbo].[fnPhy_GetUserRolesByContract]
(
@UserId uniqueidentifier,
@ContractId int
)
RETURNS @Roles TABLE
(
	RoleId uniqueidentifier,
	RoleName varchar(500),
	[Level] int
)
AS
BEGIN
	
	Declare @MinLevel int --The lower the # the higher the permission level
	
	Insert Into @Roles(RoleId, RoleName, Level)
	SELECT r.RoleId, r.RoleName, r.Level
	From [Role] r with(nolock)
		Inner Join UserRole ur with(nolock) ON r.RoleId = ur.RoleId
	Where r.ContractId = @ContractId
	And ur.UserId = @UserId
	AND DeleteFlag = 0

	Select @MinLevel = Min(Level) From @Roles
	
	--Now go get all other roles that are a higher level than what this user already has assigned
	Insert Into @Roles(RoleId, RoleName, Level)
	SELECT r.RoleId, r.RoleName, r.Level
	From [Role] r with(nolock)
	Where r.ContractId = @ContractId
	And r.Level > @MinLevel
	And r.RoleId Not In (Select RoleId From @Roles)
	AND DeleteFlag = 0
	
	RETURN 
END
