
-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 6/14/2011
-- Description:	Creates the Initial User for a given contract
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_CreateInitialAdmin]
	@ContractNumber varchar(50),
	@ContractAdminUserName varchar(50),
	@ContractAdminFirstName varchar(50),
	@ContractAdminLastName varchar(50),
	@ContractAdminEmail varchar(500),
	@ContractDBUserId varchar(50),
	@ContractDBPwd varchar(500)
	
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

	--First check to see if there is already a user for this contract.  If so, get out!!
	If Not Exists(Select 1 From UserContract uc Inner Join [Contract] c on uc.ContractId = c.ContractId Where c.Number = @ContractNumber)
	BEGIN
		Set @NewUserID = NEWID()
		Select @AppID = ApplicationId From [Application] Where ApplicationName = 'C3'
		
		If Exists(Select 1 From [Contract] Where Number = @ContractNumber)
		BEGIN
			Select @NewContractID = ContractId From [Contract] Where Number = @ContractNumber
			
			Declare curRoles Cursor For 
			Select RoleId From [Role] Where ContractId is null And RoleName <> 'Phytel'

			OPEN curRoles
			FETCH NEXT FROM curRoles INTO @PhytelRoleId
			WHILE @@FETCH_STATUS=0
			BEGIN
				exec spPhy_CloneRole @PhytelRoleId, '', @NewContractID

				FETCH NEXT FROM curRoles INTO @PhytelRoleId
			END
			CLOSE curRoles
			DEALLOCATE curRoles

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
					   ,@ContractAdminUserName
					   ,LOWER(@ContractAdminUserName)
					   ,2
					   ,0
					   ,1
					   ,@ContractAdminFirstName + ' ' + @ContractAdminLastName
					   ,@ContractAdminFirstName
					   ,Null
					   ,@ContractAdminLastName
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
					   ,@ContractAdminEmail
					   ,LOWER(@ContractAdminEmail)
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
END
