DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Text]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( [Value] VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			(CASE WHEN p.[Selected] = 'True' THEN
				p.[Text]
			ELSE
				'0'
			END) as [Value]
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
