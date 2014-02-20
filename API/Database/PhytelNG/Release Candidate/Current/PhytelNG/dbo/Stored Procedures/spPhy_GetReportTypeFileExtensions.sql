-- =============================================
-- Author:		Joe Rupert
-- Create date: 08/05/2011
-- Description:	Gets all file extensions associated with each report type 
-- =============================================

CREATE PROCEDURE [dbo].[spPhy_GetReportTypeFileExtensions]
	-- Add the parameters for the stored procedure here
	@ReportTypeID int = 0
AS
BEGIN

SET NOCOUNT ON;
		
	SELECT [FileExtension]
	FROM [dbo].[ReportTypeFileExtension] 
	INNER JOIN [dbo].[ReportType]
	ON [dbo].[ReportType].[ReportTypeID] = [dbo].[ReportTypeFileExtension].[ReportTypeID]
	WHERE [dbo].[ReportType].[ReportTypeID] = @ReportTypeID

END
