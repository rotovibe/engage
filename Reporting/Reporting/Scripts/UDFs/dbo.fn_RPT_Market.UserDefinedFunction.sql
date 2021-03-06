SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionCompleted_Market(@patientid, @patientprogramid));
		SET @SavedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionSaved_Market(@patientid, @patientprogramid));
		SET @ActionNotComplete = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionNotCompleted_Market(@patientid, @patientprogramid));
	
	--1) get completed market value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Market] FROM 	dbo.fn_RPT_ActionCompleted_Market(@patientid, @patientprogramid) where Market != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected market
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Market] FROM 	dbo.fn_RPT_ActionSaved_Market(@patientid, @patientprogramid) WHERE [Market] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
				SELECT TOP 1 				
						--CASE WHEN prxx.[Delete] = 'False' THEN
							prxx.[Text]
						--ELSE
							--NULL
						--END  
				 FROM
					RPT_Patient AS ptxx with (nolock)
					INNER JOIN RPT_PatientProgram AS ppxx with (nolock)  ON ptxx.PatientId = ppxx.PatientId  				
					INNER JOIN RPT_PatientProgramModule AS pmxx with (nolock) ON ppxx.PatientProgramId = pmxx.PatientProgramId  				
					INNER JOIN RPT_PatientProgramAction AS paxx with (nolock)  ON pmxx.PatientProgramModuleId = paxx.PatientProgramModuleId  				
					INNER JOIN RPT_PatientProgramStep AS psxx with (nolock) ON paxx.ActionId = psxx.ActionId  				
					LEFT OUTER JOIN RPT_PatientProgramResponse AS prxx with (nolock) ON psxx.StepId = prxx.StepId 			 
				 WHERE  				
					ppxx.SourceId = '541943a6bdd4dfa5d90002da' 				
					AND ((psxx.SourceId = '53f4fb39ac80d30e00000067')  OR (psxx.SourceId = '532c3e76c347865db8000001') ) 				
					AND ((paxx.SourceId = '53f4fd75ac80d30e00000083') OR (paxx.SourceId = '532c45bff8efe36886000446') ) 				
					AND (ppxx.[Delete] = 'False') 				
					AND prxx.Selected = 'True' 				
					AND ptxx.PatientId = @patientid	
					AND prxx.[Delete] = 'False'			
					AND ppxx.patientprogramid = @patientprogramid
					AND paxx.Archived = 'True'
					AND paxx.ActionId IN ( 
										 SELECT DISTINCT
											paxxx.ActionId
										 FROM
											RPT_Patient AS ptxxx with (nolock)
											INNER JOIN RPT_PatientProgram AS ppxxx with (nolock)  ON ptxxx.PatientId = ppxxx.PatientId  						
											INNER JOIN RPT_PatientProgramModule AS pmxxx with (nolock) ON ppxxx.PatientProgramId = pmxxx.PatientProgramId  						
											INNER JOIN RPT_PatientProgramAction AS paxxx with (nolock)  ON pmxxx.PatientProgramModuleId = paxxx.PatientProgramModuleId  						
											INNER JOIN RPT_PatientProgramStep AS psxxx with (nolock) ON paxxx.ActionId = psxxx.ActionId  						
											LEFT OUTER JOIN RPT_PatientProgramResponse AS prxxx with (nolock) ON psxxx.StepId = prxxx.StepId 					
										 WHERE 						
											ppxxx.SourceId = '541943a6bdd4dfa5d90002da' 						
											AND ((psxxx.SourceId = '53f4fb39ac80d30e00000067')  OR (psxxx.SourceId = '532c3e76c347865db8000001') ) 						
											AND ((paxxx.SourceId = '53f4fd75ac80d30e00000083') OR (paxxx.SourceId = '532c45bff8efe36886000446') ) 						
											AND (ppxxx.[Delete] = 'False') 						
											AND prxxx.Selected = 'True' 						
											AND ptxxx.PatientId = @patientid 						
											AND ppxxx.patientprogramid = @patientprogramid
										 ) 
				ORDER BY paxx.ArchivedDate DESC
		END
	
		Final:
			
	RETURN
END
GO
