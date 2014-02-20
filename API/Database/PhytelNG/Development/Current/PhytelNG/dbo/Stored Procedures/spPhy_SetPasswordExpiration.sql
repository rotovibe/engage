-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/29/2011
-- Description:	Sets PasswordExpiration
--				for the given UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_SetPasswordExpiration]
	@UserId uniqueidentifier,
	@Expiration varchar(50)	= ''
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	DECLARE @ExpirationPeriod INT
	DECLARE @ExpirationDate DATETIME
	
	SELECT @ExpirationPeriod = CAST([Value] AS int) FROM ApplicationSetting with(nolock) WHERE [Key] = 'PWD_EXP_TIME_PERIOD'

	IF (@Expiration = '')
	BEGIN
		SET @ExpirationDate = ( CAST(CONVERT(varchar(50), GETDATE(), 101) AS datetime) + @ExpirationPeriod)
	END
	ELSE
	BEGIN
		SET @ExpirationDate = CAST(@Expiration AS datetime)
	END

	UPDATE Membership
	SET PasswordExpiration = @ExpirationDate
	WHERE UserId = @UserId
	
	SELECT @ExpirationDate as ExpirationDate
END
