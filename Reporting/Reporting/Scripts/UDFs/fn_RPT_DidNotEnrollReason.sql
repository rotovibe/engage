CREATE FUNCTION [dbo].[fn_RPT_DidNotEnrollReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @DidNotEnrollTable TABLE( Reason VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' AND pr13.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
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
				AND pa13.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)								
				AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.patientprogramid = @patientprogramid
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
						AND pa14.ArchivedDate in (SELECT DISTINCT TOP 1 PERCENT  ActionArchivedDate FROM RPT_ProgramResponse_Flat WHERE PatientId = @patientid AND PatientProgramId = @patientprogramid and ActionSourceId = @ActionSourceId ORDER BY ActionArchivedDate DESC)																
						AND pt14.PatientId = @patientid
						AND pa14.[State] IN ('Completed')
						AND pp14.patientprogramid = @patientprogramid
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END

GO


