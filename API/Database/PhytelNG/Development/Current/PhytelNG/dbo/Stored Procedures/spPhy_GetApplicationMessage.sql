-- =============================================
-- Author:		Tony DiGiorgio
-- Create date: 04/25/2011
-- Description:	Returns all the application messages
-- =============================================
CREATE PROCEDURE [dbo].[spPhy_GetApplicationMessage]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Code, Title, [Text], Audit From ApplicationMessage with(nolock)
END
