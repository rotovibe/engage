SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Disenrolled_General_Comments]
AS
SELECT     TOP (100) PERCENT pt.PatientId, 
                      pr.[Text], 
						CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
								THEN
									pr.[Value]
							WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
								THEN
									pr.[Value]
							ELSE
								NULL
						END   AS [Value], 
                      pr.[Selected], 
                      pp.[SourceId] AS [ProgramSourceId],
                      pp.[Name], 
                      pp.[Delete], 
                      pa.[Archived], 
                      pa.[RecordCreatedOn],
                      pa.State as [Action_state],
                      ps.Completed as [step_completed],
                      pr.[delete] as [response_delete]
FROM         dbo.RPT_Patient AS pt with (nolock) INNER JOIN
                      dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
                      dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
                      dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
                      dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
                      dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
((ps.SourceId = '53f4f8a0ac80d30e02000073') OR (ps.SourceId = '530f844ef8efe307660001ab')) 
AND ((pa.SourceId = '53f57115ac80d31203000014') OR (pa.SourceId = '532c46b3c347865db8000092')) 
AND (pp.[Delete] = 'False') AND (pa.Archived = 'False')
AND pr.Value != ''
AND pa.State IN ('InProgress', 'Completed')
GO
