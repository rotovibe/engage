SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_ActionCompletedDate] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @ActionCompletedDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedDateTable
		SELECT
			CASE WHEN p.[DateCompleted] IS NOT NULL THEN
					p.[DateCompleted]--CONVERT(VARCHAR(2000),p.[DateCompleted],120)		
			ELSE 						
				NULL
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = '541943a6bdd4dfa5d90002da'
			AND p.StepSourceId = '53f4f920ac80d30e00000066'
			AND p.ActionSourceId = '53f4fd75ac80d30e00000083'
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO
