SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_MetGoal]
	@startDate DateTime,
	@endDate DateTime
AS
BEGIN
	SET NOCOUNT ON;
	
-- select percentage of met goals from total goals	
select
		CAST(CAST(COUNT(DISTINCT PatientGoalId) AS DECIMAL) / CAST((Select COUNT(DISTINCT goals) FROM vw_GoalsMap) AS DECIMAL)  * 100 as DECIMAL(12,0)) as [Just_Met]
	from 
		RPT_PatientGoal
	where 
		Status = 'met'
		AND PatientId IS NOT NULL
		AND LastUpdatedOn BETWEEN @startDate AND @endDate
		AND RecordCreatedOn BETWEEN @startDate AND @endDate
END
GO
