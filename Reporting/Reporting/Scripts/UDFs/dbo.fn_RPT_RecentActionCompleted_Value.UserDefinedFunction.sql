SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_RecentActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @RecentActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @RecentActionCompletedTable
		SELECT
			CASE WHEN p.[DateCompleted] IS NOT NULL THEN
					p.[DateCompleted]--CONVERT(VARCHAR(2000),p.[DateCompleted],120)		
			ELSE 						
				NULL
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
			p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid			
	RETURN
END
GO
