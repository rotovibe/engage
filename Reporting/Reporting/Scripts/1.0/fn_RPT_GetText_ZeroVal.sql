DROP FUNCTION [dbo].[fn_RPT_GetText_ZeroVal]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_GetText_ZeroVal] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
	--------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 8;
	--SET @patientprogramid = 279;
	--SET @ProgramSourceId = '54b69910ac80d33c2c000032';
	--SET @ActionSourceId = '545bfc3bac80d36bd10000a7';
	--SET @StepSourceId = '5453e3bbac80d37bc0000f03';
	
	----------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	-------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	
	--SELECT TOP 1 [Value] FROM 	
	--	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) ORDER BY [Value] DESC
	-------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId)
			ORDER BY [Value] DESC
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId)
			ORDER BY [Value] DESC
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
												ActionArchivedDate 
											FROM RPT_ProgramResponse_Flat 
											WHERE 
												PatientId = @patientid AND 
												PatientProgramId = @patientprogramid AND 
												ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)				
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT TOP 1
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pa14.ArchivedDate in (	SELECT DISTINCT TOP 1 PERCENT  
														ActionArchivedDate 
													FROM RPT_ProgramResponse_Flat 
													WHERE 
														PatientId = @patientid AND 
														PatientProgramId = @patientprogramid AND 
														ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)			
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO
