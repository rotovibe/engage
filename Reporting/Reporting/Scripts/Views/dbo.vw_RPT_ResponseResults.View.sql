SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_ResponseResults]
AS
SELECT		pt.PatientId, 
			pp.PatientProgramId, 
			pp.Name AS [Program], 
			pm.PatientProgramModuleId, 
			pm.Name AS [Module], 
			pa.ActionId, 
			pa.Name AS [Action], 
			ps.StepId, 
			ps.SourceId, 
			ps.Question, 
			pr.[Text], 
			pr.Value, 
			pr.Selected, 
			pa.[status], 
			pa.Archived, 
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
			(pp.[Delete] = 'False') 
			AND pa.archived = 'False'
			AND Value is not null 
			AND value != ''
			AND pa.[State] IN ('InProgress', 'Completed')
GO
