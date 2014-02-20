-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/13/2011
-- Description:	Returns the last so many (count
--				from ApplicationSettings table)
--				passwords for the passed in
--				UserId
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetPasswordHistory]
	@UserId UNIQUEIDENTIFIER
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	DECLARE @Count INT
	
	SELECT @Count = CAST([Value] AS INT)
	FROM ApplicationSetting with(nolock)
	WHERE [Key] = 'PWD_HISTORY_COUNT'

	SELECT x.UserId, x.[Password], x.PasswordChangedDate
	FROM (	SELECT UserID, Password, PasswordChangedDate, 
			ROW_NUMBER() OVER (PARTITION BY UserId ORDER BY PasswordChangedDate DESC) AS Seq
			FROM PasswordHistory with(nolock)) x
	WHERE x.Seq <= @Count
		AND x.UserId = @UserId	
	
END
