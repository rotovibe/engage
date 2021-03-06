SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_DisenrolledReason]
AS
SELECT     TOP (100) PERCENT pt.PatientId,
						CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
								THEN
									pr.[Text]
							WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
								THEN
									pr.[Text]
							ELSE
								NULL
						END   AS [Text],  
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
((ps.SourceId= '53f56f10ac80d31200000001') OR (ps.SourceId= '532c407fc347865db8000003') )
AND ((pa.SourceId = '53f57115ac80d31203000014') OR (pa.SourceId = '532c46b3c347865db8000092'))
AND (pp.[Delete] = 'False') AND (pa.Archived = 'False')
AND pr.Selected = 'True'
AND pa.State IN ('InProgress', 'Completed')
GO
