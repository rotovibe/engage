SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Pending_Enrollment]
AS
SELECT		pt.PatientId, 
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
			pr.[Selected], 
			pp.SourceId AS [ProgramSourceId], 
			pp.[Delete], 
			pa.[state], 
			pa.Archived, 
			pp.[Name],
			pa.RecordCreatedOn, 
			pa.ArchivedDate, 
			pa.MongoArchiveOriginId
FROM        
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
            dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
            dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
            dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
            dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
            dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
			((ps.SourceId = '532c3de1f8efe368860003b2') or (ps.SourceId = '53f4f7ecac80d30e02000072'))
			AND ((pa.SourceId = '532c45bff8efe36886000446')  or (pa.SourceId = '53f4fd75ac80d30e00000083'))
			AND (pp.[Delete] = 'False') 
			AND (pa.Archived = 'False')
			AND (pr.Value != '')
			AND (pa.ArchivedDate IS NULL)
			AND pa.State IN ('InProgress', 'Completed')
GO
