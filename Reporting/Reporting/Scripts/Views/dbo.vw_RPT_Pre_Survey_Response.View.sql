SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Pre_Survey_Response]
AS
SELECT		TOP (100) PERCENT 
			pt.PatientId,
			pr.[Text], 
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
			pr.Selected, 
			pp.[SourceId] AS [ProgramSourceId],
			pp.[Name], 
			pp.[Delete], 
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
			((ps.sourceid='531a2ec7c347860424000023') OR (ps.sourceid='53f4c273ac80d30e00000009') )
			AND ((pa.sourceid='531a304bc347860424000109') OR (pa.sourceid='53f4ea64ac80d30e00000021') )
			AND (pp.[Delete] = 'False') 
			AND (pa.Archived = 'False')
			AND pr.value != ''
			AND pa.[State] IN ('InProgress', 'Completed')
GO
