SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_PHQ2_Total_Point_Score]
AS
SELECT     
	TOP (100) PERCENT pt.PatientId, 
	CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
			THEN
				pr.[Text]
		WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
			THEN
				pr.[Text]
		ELSE
			NULL
	END   AS [Text], 
	CASE WHEN pr.[delete] = 'False' AND pa.[State] = 'Completed'
			THEN
				pr.[Value]
		WHEN pr.[delete] = 'True' AND pa.[State] = 'InProgress'
			THEN
				pr.[Value]
		ELSE
			NULL
	END   AS [Value],  
	pr.Selected, 
	pp.SourceId AS ProgramSourceId,
	pp.Name, 
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
((ps.SourceId = '540694b2ac80d330cb000010') OR (ps.SourceId = '52f54de1c34786660a0000de')) 
AND ((pa.SourceId = '5406b0dcac80d330cb00016d') OR (pa.SourceId = '531f300ac347861e7700001b'))
AND (pp.[Delete] = 'False') AND (pa.Archived = 'False')
AND pa.[State] IN ('InProgress', 'Completed')
GO
