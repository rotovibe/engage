IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Did_Not_Enroll_Reason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Did_Not_Enroll_Reason]
GO

CREATE FUNCTION [dbo].[fn_RPT_Did_Not_Enroll_Reason] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS VARCHAR(2000) 
AS
BEGIN
	DECLARE @DidNotEnrollReason varchar(2000)

	set @DidNotEnrollReason = 
		( 		
			COALESCE(
				(SELECT 
					( CASE WHEN pr12.[delete] = 'False' AND pa12.[State] = 'Completed' 
							THEN pr12.[Text] ELSE NULL END) AS [didnotenrollreasonText]
					FROM
					RPT_Patient AS pt12 with (nolock) INNER JOIN
					RPT_PatientProgram AS pp12 with (nolock) ON pt12.PatientId = pp12.PatientId INNER JOIN
					RPT_PatientProgramModule AS pm12 with (nolock) ON pp12.PatientProgramId = pm12.PatientProgramId INNER JOIN
					RPT_PatientProgramAction AS pa12 with (nolock) ON pm12.PatientProgramModuleId = pa12.PatientProgramModuleId INNER JOIN
					RPT_PatientProgramStep AS ps12 with (nolock) ON pa12.ActionId = ps12.ActionId LEFT OUTER JOIN
					RPT_PatientProgramResponse AS pr12 with (nolock) ON ps12.StepId = pr12.StepId
					WHERE
					pp12.sourceid = '541943a6bdd4dfa5d90002da'
					AND ((ps12.SourceId = '53f4f885ac80d30e00000065') OR (ps12.SourceId = '532c3fc2f8efe368860003b5') )
					AND ((pa12.SourceId = '53f4fd75ac80d30e00000083') OR (pa12.SourceId = '532c45bff8efe36886000446') )
					AND (pp12.[Delete] = 'False')
					AND (pa12.Archived = 'False')
					AND (pr12.Selected = 'True')
					AND pa12.ArchivedDate IS NULL
					AND pt12.patientid = @patientid
					AND pa12.[State] IN ('Completed')
					AND pp12.patientprogramid = @patientprogramid
					),
					(SELECT TOP 1
					(CASE WHEN pr13.[delete] = 'False' AND pa13.[State] = 'Completed'
						THEN
							pr13.[Text]
						ELSE
							NULL
						END) AS [didnotenrollTextarch]
					FROM
					RPT_Patient AS pt13 with (nolock) INNER JOIN
					RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN
					RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
					RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
					RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
					RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
					WHERE
					pp13.sourceid = '541943a6bdd4dfa5d90002da'
					AND ((ps13.SourceId = '53f4f885ac80d30e00000065') OR (ps13.SourceId = '532c3fc2f8efe368860003b5') )
					AND ((pa13.SourceId = '53f4fd75ac80d30e00000083') OR (pa13.SourceId = '532c45bff8efe36886000446') )
					AND (pp13.[Delete] = 'False')
					AND (pa13.Archived = 'True')
					AND (pr13.Selected = 'True')
					AND pa13.ArchivedDate IS NOT NULL
					AND pa13.[State] IN ('Completed')
					AND pt13.PatientId = @patientid
					AND pp13.patientprogramid = @patientprogramid
					AND pa13.ActionId IN (
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
							pp14.sourceid = '541943a6bdd4dfa5d90002da'
							AND ((ps14.SourceId = '53f4f885ac80d30e00000065') OR (ps14.SourceId = '532c3fc2f8efe368860003b5') )
							AND ((pa14.SourceId = '53f4fd75ac80d30e00000083') OR (pa14.SourceId = '532c45bff8efe36886000446') )
							AND (pp14.[Delete] = 'False')
							AND (pr14.Selected = 'True')
							AND pt14.PatientId = pt13.patientId
							AND pa14.[State] IN ('Completed')
							AND pp14.patientprogramid = @patientprogramid
							)
					ORDER BY pa13.ArchivedDate DESC
					) 	
					)
		)

	RETURN @DidNotEnrollReason
	
END

GO


