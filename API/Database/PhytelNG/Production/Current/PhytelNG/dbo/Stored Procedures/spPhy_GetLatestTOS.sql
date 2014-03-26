-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 6/2/2011
-- Description:	Retrieves latest version of the terms of service
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetLatestTOS]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	Top 1 TermsOfServiceID, [Text], [Version]
	From	TermsOfService with(nolock)
	Order By [Version] Desc
END
