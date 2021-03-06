SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Referral_Other]
AS
SELECT	
		TOP (100) PERCENT pt.PatientId, 
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
		pr.Selected,
		pp.[Name], 
		pp.SourceId AS [ProgramSourceId], 
		pp.[Delete], 
		pa.Archived,
		pa.SourceId as [action source],
		ps.Question,
		ps.[SourceId] as [step source],
		pa.[State], 
		pa.RecordCreatedOn
FROM         
		dbo.RPT_Patient AS pt with (nolock) INNER JOIN
		dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
		dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
		dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
		dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId INNER JOIN
		dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
		((ps.SourceId = '52f54e8cc34786660a0000e0') OR (ps.SourceId = '5406956aac80d330c8000014')) 
		AND ( (pa.SourceId = '531f300ac347861e7700001b') OR (pa.SourceId = '5406b0dcac80d330cb00016d') )
		AND (pa.Archived = 'False')
		AND pa.[State] IN ('InProgress', 'Completed')
GO
