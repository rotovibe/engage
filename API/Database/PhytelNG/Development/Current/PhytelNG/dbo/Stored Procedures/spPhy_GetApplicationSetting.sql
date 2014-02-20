-- =============================================
-- Author:		Josh Gattis
-- Create date: 04/25/2011
-- Description:	Gets the settings for the 
--				application
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetApplicationSetting]
AS

BEGIN

	SET NOCOUNT ON
	SET XACT_ABORT ON
	
	SELECT [Key], Value
	FROM ApplicationSetting with(nolock)
	
END
