SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Risk_Level]
AS
SELECT		pt.PatientId,
			(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Text]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Text]
				ELSE
					NULL
			END) AS [Text],  
			(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Value]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Value]
				ELSE
					NULL
			END) AS [Value], 
			pr.Selected, 
			pp.SourceId AS [ProgramSourceId], 
			pp.[Delete],
			pp.Name, 
			pa.Archived, 
			pa.[state],
			pa.RecordCreatedOn
FROM        
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
			dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
			dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
			dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
			dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
			dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
			((ps.SourceId = '53f6cc01ac80d3139000000d') OR (ps.SourceId = '52f50d60c34786662e000001') )
			AND ((pa.SourceId = '53f6ce5bac80d3138d000022') OR (pa.SourceId = '532c9833a38116ac18000371') )
			AND (pp.[Delete] = 'False') 
			AND (pa.Archived = 'False')
			AND pa.[State] IN ('InProgress', 'Completed')
GO
