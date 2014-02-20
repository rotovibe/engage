-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 6/14/2011
-- Description:	Creates a user for a given contract
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_CreateUser]
	@ContractNumber varchar(50),
	@ContractUserName varchar(50),
	@ContractFirstName varchar(50),
	@ContractLastName varchar(50),
	@ContractEmail varchar(500)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @PhytelRoleId uniqueidentifier
	Declare @AdminRoleId uniqueidentifier
	Declare @NewUserID uniqueidentifier
	Declare @AppID uniqueidentifier
	Declare @NewContractID int

	Set @NewUserID = NEWID()
	Select @AppID = ApplicationId From [Application] Where ApplicationName = 'C3'
		
	If Exists(Select 1 From [Contract] Where Number = @ContractNumber)
	BEGIN
		Select @NewContractID = ContractId From [Contract] Where Number = @ContractNumber

		Select @AdminRoleId = RoleId From Role Where ContractId = @NewContractID And RoleName = 'Administrator'
		
		INSERT INTO [User]
				   ([ApplicationId]
				   ,[UserId]
				   ,[UserName]
				   ,[LoweredUserName]
				   ,[UserTypeId]
				   ,[IsAnonymous]
				   ,[NewUser]
				   ,[DisplayName]
				   ,[FirstName]
				   ,[MiddleName]
				   ,[LastName]
				   ,[Phone]
				   ,[Ext]
				   ,[LastActivityDate]
				   ,[SessionTimeout]
				   ,[StatusTypeId])
			 VALUES
				   (@AppID
				   ,@NewUserID
				   ,@ContractUserName
				   ,LOWER(@ContractUserName)
				   ,2
				   ,0
				   ,1
				   ,@ContractFirstName + ' ' + @ContractLastName
				   ,@ContractFirstName
				   ,Null
				   ,@ContractLastName
				   ,'0000000000'
				   ,Null
				   ,getDate()
				   ,480
				   ,1)

		INSERT INTO [Membership]
				   ([ApplicationId]
				   ,[UserId]
				   ,[Password]
				   ,[PasswordFormat]
				   ,[PasswordSalt]
				   ,[MobilePIN]
				   ,[Email]
				   ,[LoweredEmail]
				   ,[PasswordQuestion]
				   ,[PasswordAnswer]
				   ,[IsApproved]
				   ,[IsLockedOut]
				   ,[CreateDate]
				   ,[LastLoginDate]
				   ,[LastPasswordChangedDate]
				   ,[LastLockoutDate]
				   ,[FailedPasswordAttemptCount]
				   ,[FailedPasswordAttemptWindowStart]
				   ,[FailedPasswordAnswerAttemptCount]
				   ,[FailedPasswordAnswerAttemptWindowStart]
				   ,[Comment]
				   ,[PasswordExpiration]
				   ,[Status]
				   ,[AdministratorUserId])
			 VALUES
				   (@AppID
				   ,@NewUserID
				   ,'jxnd8PAUhmc0F5HtfBHmyPt8eNQ343wEuNGryVuwGSv6QzMo8Ezu0UrTvnE3N/8y' --Phytel2!
				   ,2
				   ,'/lHrs4tHzAlU8DHxpRBTrg=='
				   ,Null
				   ,@ContractEmail
				   ,LOWER(@ContractEmail)
				   ,Null
				   ,Null
				   ,1
				   ,0
				   ,getDate()
				   ,getDate()
				   ,getDate()
				   ,getDate()
				   ,0
				   ,getDate()
				   ,0
				   ,getDate()
				   ,Null
				   ,Null
				   ,'Active'
				   ,Null)


		Insert Into usercontract(userid, contractid, defaultcontract) Values (@NewUserID, @NewContractID, 1)
		
		Insert Into UserRole(UserId, RoleId) Values (@NewUserID, @AdminRoleId)
	END
END
