ALTER FUNCTION [dbo].[fn_RPT_PCPOther] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 582;
	--SET @patientprogramid = 38;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422df97ac80d3356d000004';
	------------------------------------

		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	------
	--select @CompletedCount;
	--select @SavedCount;
	--select @ActionNotComplete;
	------
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 [Value] FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where [Value] != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 NULL as [Value] FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @ResultTable WHERE Value IS NULL;
			INSERT INTO @ResultTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = 'True' THEN
					CASE WHEN pr7.[Delete] = 'False' THEN
						pr7.[Value]
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
				ps7.SourceId= @StepSourceId
				AND pa7.SourceId = @ActionSourceId
				AND pp7.SourceId = @ProgramSourceId
				AND pp7.[Delete] = 'False'
				AND pr7.Selected = 'False'
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
							ps8.SourceId = @StepSourceId
							AND pa8.SourceId = @ActionSourceId
							AND pp8.SourceId = @ProgramSourceId
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


ALTER FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
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

ALTER FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			'0' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			--AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO

/*** fn_RPT_GetValue ***/
ALTER FUNCTION [dbo].[fn_RPT_GetValue] 
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
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL AS [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) WHERE [Value] != '0'
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
						pr13.[Value]
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
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
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