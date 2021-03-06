SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Acuity_Level]
AS
SELECT     pt.PatientId, 
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
		   pr.Selected, 
		   pp.SourceId AS [ProgramSourceId],
		   pp.[Name], 
		   pp.[Delete],
		   pa.[RecordCreatedOn]
FROM         dbo.RPT_Patient AS pt with (nolock) INNER JOIN
                      dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
                      dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
                      dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
                      dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
                      dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
((ps.sourceid = '53f6cc4cac80d3138d000012') OR (ps.sourceid = '52f50dc3c34786662e000002') )
AND ((pa.sourceid = '53f6ce5bac80d3138d000022') OR (pa.sourceid = '532c9833a38116ac18000371')) 
AND (pp.[Delete] = 'False') AND (pa.Archived = 'False') AND 
                      (pr.Selected = 'True')
GO
