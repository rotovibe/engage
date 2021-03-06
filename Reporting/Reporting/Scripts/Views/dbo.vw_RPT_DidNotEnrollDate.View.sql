SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_DidNotEnrollDate]
AS
SELECT		pt.PatientId,  	
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
                      pp.SourceId AS ProgramSourceId,
                      pp.Name, 
                      pp.[Delete],
					  pa.SourceId,                        
                      pa.Archived,
					  pa.ArchivedDate,                      
                      pa.Name as [action name],  
                      pa.RecordCreatedOn,
                      pa.[State] as [Action_state],
                      ps.[Completed] as [step_completed],
                      ps.SourceId as [step sourceid], 
                      pr.[delete] as [response_delete]
                      
FROM      
		  dbo.RPT_Patient AS pt with (nolock) INNER JOIN
          dbo.RPT_PatientProgram AS pp with (nolock) ON pt.PatientId = pp.PatientId INNER JOIN
          dbo.RPT_PatientProgramModule AS pm with (nolock) ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
          dbo.RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
          dbo.RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
          dbo.RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId
WHERE     
ps.Question = 'Date did not enroll:' 
AND pa.Name = 'Enrollment' 
AND (pp.[Delete] = 'False') 
AND (pa.Archived = 'False') 
AND pa.ArchivedDate IS NULL
AND pr.Value != '' 
AND pa.[State] IN ('InProgress', 'Completed')
GO
