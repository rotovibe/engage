-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 6/2/2011
-- Description:	Saves roles for a user
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SaveUserRoles]
	@UserId uniqueidentifier,
	@RoleIds varchar(max)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    --First remove all roles for this user
    Delete From UserRole Where UserId = @UserId
    
    Insert Into UserRole(UserId, RoleId)
    Select @UserId, CAST(value as uniqueidentifier) From fn_Split(@RoleIds, '|')
    
END
