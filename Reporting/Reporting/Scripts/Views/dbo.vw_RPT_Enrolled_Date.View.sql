SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Enrolled_Date]
AS
SELECT     pt.PatientId,
                      pr.Text, 
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
                      --pr.Value,
                      pr.Selected, 
                      pp.Name,
                      pp.SourceId AS [ProgramSourceId], 
                      pp.[Delete], 
                      pa.Archived,
                      pa.Name as [Action Name], 
                      pa.SourceId as [action source],
                      pa.[State], 
                      pa.RecordCreatedOn,
                      ps.SourceId as [step source],
                      ps.Question
FROM         dbo.RPT_Patient AS pt with (nolock)  INNER JOIN
                      dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
                      dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
                      dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
                      dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
                      dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE  
((ps.SourceId = '532c3e1ef8efe368860003b3') OR (ps.SourceId = '53f4f920ac80d30e00000066')) 
AND ((pa.SourceId = '532c45bff8efe36886000446') OR (pa.SourceId = '53f4fd75ac80d30e00000083') )
AND (pp.[Delete] = 'False') 
AND (pa.Archived = 'False')
AND pa.State IN ('InProgress', 'Completed')
GO
