-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/26/2011
-- Description:	Gets the security questions for  
--				the application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetSecurityQuestion]
AS

BEGIN

	SET NOCOUNT ON
	
	SELECT SecurityQuestionId, Question
	FROM SecurityQuestion with(nolock)
	ORDER BY SecurityQuestionId
	
END
