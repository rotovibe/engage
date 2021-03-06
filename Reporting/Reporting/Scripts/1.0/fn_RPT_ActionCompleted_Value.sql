DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Value]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = 'False' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]		
			ELSE 						
				'0'
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
