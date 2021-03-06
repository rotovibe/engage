SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Referral_Provided_EAP]
AS
SELECT		TOP (100) PERCENT pt.PatientId, 
			(CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
					THEN
						pr.[Text]
				WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
					THEN
						pr.[Text]
				ELSE
					NULL
			END) AS [Text], 
			pr.Value, 
			pr.Selected, 
			pp.Name,
			pp.SourceId AS [ProgramSourceId], 
			pp.[Delete], 
			pa.Archived, 
			pa.RecordCreatedOn
FROM        
			dbo.RPT_Patient AS pt with (nolock) INNER JOIN
			dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
			dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
			dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
			dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
			dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
			((ps.SourceId = '5406954dac80d330c8000013') OR (ps.SourceId = '531f2db3c347861e77000009') )
			AND ((pa.SourceId = '5406b0dcac80d330cb00016d') OR (pa.SourceId = '531f300ac347861e7700001b')) 
			AND (pp.[Delete] = 'False') AND (pa.Archived = 'False') AND (pr.Value = 'true') AND pr.Text = 'EAP'
			AND pa.[State] IN ('InProgress', 'Completed')
GO
