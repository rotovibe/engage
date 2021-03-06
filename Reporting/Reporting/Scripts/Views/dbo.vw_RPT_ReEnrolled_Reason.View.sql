SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_ReEnrolled_Reason]
AS
SELECT      pt.PatientId, 
			pr.[Text], 
			(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Value]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Value]
				ELSE
					NULL
			END) AS [Value],  
			pr.[Selected], 
			pp.[Name],
			pp.SourceId AS [ProgramSourceId], 
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
			((ps.SourceId = '53f5720bac80d3120300001c') OR (ps.SourceId = '532c40f3f8efe368860003b8'))
			AND ((pa.SourceId = '53f572caac80d31203000020') OR (pa.SourceId = '532c4804f8efe368860004e2')) 
			AND (pp.[Delete] = 'False') AND (pa.Archived = 'False') 
			AND pa.[State] IN ('InProgress', 'Completed')
GO
