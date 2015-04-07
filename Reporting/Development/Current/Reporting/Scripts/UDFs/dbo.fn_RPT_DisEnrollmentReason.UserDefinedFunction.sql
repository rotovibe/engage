SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_DisEnrollmentReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where [Value] != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = 'True' AND pr7.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
					CASE WHEN pr7.[Delete] = 'False' THEN
						pr7.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				((ps7.SourceId= '53f56f10ac80d31200000001') OR (ps7.SourceId= '532c407fc347865db8000003') )
				AND ((pa7.SourceId = '53f57115ac80d31203000014') OR (pa7.SourceId = '532c46b3c347865db8000092'))
				AND pp7.Name = 'BSHSI - Healthy Weight v2'
				AND (pp7.[Delete] = 'False')
				AND pr7.Selected = 'True'
				AND pa7.[State] IN ('Completed')
				AND pa7.Archived = 'True'
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							((ps8.SourceId = '53f56eb7ac80d31203000001') OR (ps8.SourceId = '532c4061f8efe368860003b6') )
							AND ((pa8.SourceId = '53f57115ac80d31203000014') OR (pa8.SourceId = '532c46b3c347865db8000092'))
							AND pp8.Name = 'BSHSI - Healthy Weight v2'
							AND (pp8.[Delete] = 'False')
							AND pa8.[State] IN ('Completed')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
GO
