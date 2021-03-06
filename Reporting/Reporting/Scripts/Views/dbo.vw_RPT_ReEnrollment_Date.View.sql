SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_ReEnrollment_Date]
AS
SELECT     pt.PatientId,
                      pr.Text, 
						(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
								THEN
									CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
							WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
								THEN
									CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
							ELSE
								NULL
						END) AS [Value],
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
FROM         
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
            dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
            dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
            dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
            dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
            dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE  
			((ps.SourceId = '532c40d0f8efe368860003b7') OR (ps.SourceId = '53f571e0ac80d3120300001b'))
			AND ((pa.SourceId = '532c4804f8efe368860004e2') OR (pa.SourceId = '53f572caac80d31203000020'))
			AND (pp.[Delete] = 'False') 
			AND (pa.Archived = 'False')
			AND pa.[State] IN ('InProgress', 'Completed')
GO
