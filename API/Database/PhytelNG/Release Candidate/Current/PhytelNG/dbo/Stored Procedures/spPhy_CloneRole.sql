CREATE PROCEDURE [dbo].[spPhy_CloneRole]
	-- Add the parameters for the stored procedure here
	@PhytelRoleId uniqueidentifier,
	@SpecificPermissionIds varchar(max),
	@ContractId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --first create the new role
    DECLARE @TranStarted	bit
    DECLARE @ErrorCode		int
    DECLARE @PhytelRoleName	varchar(200)
    DECLARE @NewRoleId		uniqueidentifier
    
    SET @ErrorCode = 0
    SET @TranStarted = 0
	Select @PhytelRoleName = RoleName From dbo.[Role] with(nolock) Where RoleId = @PhytelRoleId AND DeleteFlag = 0 
	
	If Not Exists(Select 1 From dbo.[Role] Where ContractId = @ContractId And RoleName = @PhytelRoleName)
	BEGIN
		IF( @@TRANCOUNT = 0 )
		BEGIN
			BEGIN TRANSACTION
			SET @TranStarted = 1
		END
		ELSE
			SET @TranStarted = 0

		INSERT INTO dbo.[Role]
					(ApplicationId, RoleName, LoweredRoleName, Description, UserTypeId, ContractId, Level)
			Select ApplicationId, RoleName, LoweredRoleName, 'Cloned Role From Phytel Base Role ''' + @PhytelRoleName + '''', UserTypeId, @ContractId, [Level]
				From dbo.[Role] Where RoleId = @PhytelRoleId AND DeleteFlag = 0
		
		Select @NewRoleId = RoleId From dbo.[Role] with(nolock) Where RoleName = @PhytelRoleName And ContractId = @ContractId
		
		IF( @@ERROR <> 0 )
		BEGIN
			SET @ErrorCode = -1
			GOTO Cleanup
		END

		--Now clone the permissions for the new role
		If(RTRIM(LTRIM(@SpecificPermissionIds)) <> '')
			BEGIN
				Insert Into dbo.[RolePermission](RoleId, PermissionId)
				Select @NewRoleId, value from dbo.[fn_Split](RTRIM(LTRIM(@SpecificPermissionIds)), '|')
			END
		Else
			BEGIN
				Insert Into dbo.[RolePermission](RoleId, PermissionId)
				Select @NewRoleId, PermissionId from dbo.[RolePermission] with(nolock) Where RoleId = @PhytelRoleId
			END
		
		IF( @@ERROR <> 0 )
		BEGIN
			SET @ErrorCode = -1
			GOTO Cleanup
		END

		IF( @TranStarted = 1 )
		BEGIN
			SET @TranStarted = 0
			COMMIT TRANSACTION
		END
	END
	
    RETURN(0)

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
        ROLLBACK TRANSACTION
    END

    RETURN @ErrorCode

END
