-- =============================================
-- Author:		Suneetha neerukonda
-- Create date: 08/05/2011
-- Description:	Gets the Report Type 
-- =============================================

CREATE PROCEDURE [dbo].[spPhy_GetReportTypes]
	
AS
BEGIN

	SET NOCOUNT ON;
		
		SELECT ReportTypeID, ReportTypeName, [Description], CreatedBy, CreatedDate, UpdatedBy, UpdateDate 
		FROM dbo.ReportType WITH (NOLOCK)
		WHERE Deleteflag = 0
		ORDER BY ReportTypeName ASC

END
