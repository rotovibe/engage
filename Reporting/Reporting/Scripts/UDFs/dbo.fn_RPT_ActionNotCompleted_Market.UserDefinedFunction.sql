SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_ActionNotCompleted_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			'0' as [Market]
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = '541943a6bdd4dfa5d90002da' 				
			AND ((p.StepSourceId = '53f4fb39ac80d30e00000067')  OR (p.StepSourceId = '532c3e76c347865db8000001') ) 				
			AND ((p.ActionSourceId = '53f4fd75ac80d30e00000083') OR (p.ActionSourceId = '532c45bff8efe36886000446') ) 							
			AND (p.ActionArchived = 'False') 				
			AND (p.ActionCompleted = 'False')
			AND (p.StepCompleted = 'False')
			AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
