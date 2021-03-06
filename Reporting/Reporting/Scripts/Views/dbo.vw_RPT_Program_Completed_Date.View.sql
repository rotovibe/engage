SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Program_Completed_Date]
AS
SELECT		pt.PatientId, 
			pr.[Text], 
			(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						CASE WHEN ISDATE(pr.Value) = 1 THEN CAST(pr.Value AS DATE) ELSE NULL END 
				ELSE
					NULL
			END) 						
			AS [Value],
			pr.Selected,pp.Name, 
			pp.SourceId AS ProgramSourceId, 
			pp.[Delete], 
			pa.Archived, 
			pa.[State],
			pa.RecordCreatedOn
FROM        
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
			dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
			dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
			dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
			dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
			dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
			((ps.SourceId = '53f57309ac80d31200000017') OR (ps.SourceId = '532c411bf8efe368860003b9') )
			AND ((pa.SourceId = '53f57383ac80d31203000033') OR (pa.SourceId = '532c48d7c347865db80000a1')) 
			AND (pp.[Delete] = 'False') AND (pa.Archived = 'False')
			AND pr.Value != ''
			AND pa.[State] IN ('InProgress', 'Completed')
GO
