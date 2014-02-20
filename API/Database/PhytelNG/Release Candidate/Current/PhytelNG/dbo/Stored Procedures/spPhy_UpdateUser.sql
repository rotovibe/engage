CREATE PROCEDURE [dbo].[spPhy_UpdateUser]
    @UserId					uniqueidentifier
    ,@FirstName				varchar(100)
    ,@MiddleName		    varchar(100)
    ,@LastName				varchar(100)
    ,@Phone					varchar(10)
    ,@Ext					varchar(5)
    ,@SessionTimeout		Smallint
    ,@PasswordExpiration	DateTime
    ,@AcceptedLatestTOS		bit
    ,@UserTypeId 			int
    ,@StatusTypeId			int
    ,@AdminUserId			uniqueidentifier
AS
BEGIN
   
	DECLARE @CurrentTOSID int
    DECLARE @TranStarted   bit
    DECLARE @CurrentStatus varchar(100)
    
    SET @TranStarted = 0

    IF( @@TRANCOUNT = 0 )
    BEGIN
	    BEGIN TRANSACTION
	    SET @TranStarted = 1
    END
    ELSE
	SET @TranStarted = 0
	
	Select @CurrentStatus = Name From StatusType Where StatusTypeId = @StatusTypeId
	
	UPDATE 
		[User]
	SET
		FirstName = @FirstName
		,MiddleName = @MiddleName
		,LastName = @LastName
		,Phone = @Phone
		,Ext = @Ext
		,SessionTimeout = @SessionTimeout
		,UserTypeId = @UserTypeId
		,StatusTypeId = @StatusTypeId
	WHERE
		UserId = @UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

    UPDATE dbo.Membership WITH (ROWLOCK)
    SET
         PasswordExpiration		= @PasswordExpiration,
         AdministratorUserId	= @AdminUserId,
         [Status]				= @CurrentStatus
    WHERE
       @UserId = UserId

    IF( @@ERROR <> 0 )
        GOTO Cleanup

	if(@AcceptedLatestTOS = 1)
	BEGIN
		SELECT @CurrentTOSID = x.TermsOfServiceID
			FROM (SELECT TermsOfServiceID, ROW_NUMBER() OVER (ORDER BY [Version] DESC) AS Seq
					FROM TermsOfService with(nolock)) x
			WHERE x.Seq = 1

		If Not Exists(Select 1 From UserTermsOfService Where UserId = @UserId And TermsOfServiceID = @CurrentTOSID)
		BEGIN
			Insert Into UserTermsOfService (UserId, TermsOfServiceId, AgreedOn) Values (@UserId, @CurrentTOSID, getDate())
		END
		
		IF( @@ERROR <> 0 )
			GOTO Cleanup
	END
	
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
