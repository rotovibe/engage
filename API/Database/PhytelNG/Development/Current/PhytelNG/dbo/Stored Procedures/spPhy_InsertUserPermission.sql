CREATE PROCEDURE [dbo].[spPhy_InsertUserPermission]
    @UserId					uniqueidentifier
    ,@PermissionId			int
AS
BEGIN
   
	DECLARE @CurrentTOSID int
    DECLARE @TranStarted   bit
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0
	
	INSERT INTO UserPermission(UserId, PermissionId)
    VALUES(@UserId, @PermissionId)

    IF( @TranStarted = 1 )
    BEGIN
		SET @TranStarted = 0
		COMMIT TRANSACTION
    END

    RETURN 0

Cleanup:

    IF( @TranStarted = 1 )
    BEGIN
        SET @TranStarted = 0
    	ROLLBACK TRANSACTION
    END

    RETURN -1
END
