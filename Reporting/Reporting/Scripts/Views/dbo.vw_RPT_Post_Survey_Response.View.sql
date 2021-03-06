SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Post_Survey_Response]
AS
SELECT		pt.PatientId, 
			pr.[Text], 
			CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
				ELSE
					NULL
			END 						
			AS [Value],
			pr.Selected, 
			pp.SourceId AS ProgramSourceId, 
			pp.[Delete], 
			pp.Name,
			ps.SourceId as [Step SourceId] ,
			pa.[state], 
			pa.SourceId as [Action SourceId],
			pa.Archived, 
			pa.RecordCreatedOn
FROM
	dbo.RPT_Patient AS pt with (nolock) INNER JOIN
    dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
    dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
    dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
    dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
    dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
	(ps.SourceId = '531a2ec7c347860424000023') 
	AND (pa.SourceId = '5329d20ba3811660150000ea') 
	AND (pp.[Delete] = 'False') 
	AND (pa.Archived = 'False')
	AND pa.[State] IN ('InProgress', 'Completed')
	AND (pr.Value != '')
GO
