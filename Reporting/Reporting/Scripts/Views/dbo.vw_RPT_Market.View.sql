SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Market]
AS
SELECT		TOP (100) PERCENT pt.PatientId, 
			CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Text]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Text]
				ELSE
					NULL
			END   AS [Text],  
			pr.[Value], 
			pr.[Selected], 
			pp.[Name],
			pp.SourceId AS ProgramSourceId, 
			pp.[Delete], 
			pa.ActionId, 
			pa.[State],
			pa.Archived, 
			pa.RecordCreatedOn, 
			pa.ArchivedDate
FROM         
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
            dbo.RPT_PatientProgram AS pp with (nolock)  ON pt.PatientId = pp.PatientId INNER JOIN
            dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
            dbo.RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
            dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
            dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
((ps.SourceId = '53f4fb39ac80d30e00000067')  OR (ps.SourceId = '532c3e76c347865db8000001') )
AND ((pa.SourceId = '53f4fd75ac80d30e00000083') OR (pa.SourceId = '532c45bff8efe36886000446') )
AND (pp.[Delete] = 'False') AND (pa.Archived = 'False') AND pa.ArchivedDate IS NULL AND pr.Selected = 'True'
AND pa.State IN ('InProgress', 'Completed')
GO
