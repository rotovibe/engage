SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_DoYouHaveaPCP]
AS
SELECT		pt.PatientId, 
			pp.PatientProgramId, 
			pp.[Name] AS [Program], 
			pm.PatientProgramModuleId, 
			pm.[Name] AS [Module], 
			pa.ActionId, 
			pa.[Name] AS [Action], 
			ps.StepId, 
			ps.[SourceId] as [Step Source], 
			ps.Question, 
			CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Text]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Text]
				ELSE
					NULL
			END AS [Text],  
			pr.[Value], 
			pr.[Selected], 
			pa.[state], 
			pa.[SourceId]  as [Action Source],
			pa.[Archived], 
			pa.ArchivedDate, 
			pa.MongoArchiveOriginId
FROM        
			dbo.RPT_Patient AS pt with (nolock)  INNER JOIN
            dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
            dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
            dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
            dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId INNER JOIN
            dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
			((ps.SourceId = '52f4fb32c34786626c000029') OR (ps.SourceId = '53f6ca09ac80d31390000001') )
			AND ((pa.SourceId = '52f8c601c34786763500007f') OR (pa.SourceId = '53f6cb33ac80d3138d00000a') )
			AND pr.Selected = 'True'
			AND pa.archived = 'False'
			AND pa.[State] IN ('InProgress', 'Completed')
GO
