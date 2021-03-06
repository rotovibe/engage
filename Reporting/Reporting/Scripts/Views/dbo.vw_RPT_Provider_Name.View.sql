SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [vw_RPT_Provider_Name]
AS
SELECT    pt.PatientId, pp.sourceid as [programsourceid], pp.Name, pr.Text, pr.Value,pa.Archived, pa.ArchivedDate, pa.MongoArchiveOriginId
FROM      dbo.RPT_Patient AS pt INNER JOIN
          dbo.RPT_PatientProgram AS pp ON pt.PatientId = pp.PatientId INNER JOIN
          dbo.RPT_PatientProgramModule AS pm ON pp.PatientProgramId = pm.PatientProgramId INNER JOIN
          dbo.RPT_PatientProgramAction AS pa ON pm.PatientProgramModuleId = pa.PatientProgramModuleId INNER JOIN
          dbo.RPT_PatientProgramStep AS ps ON pa.ActionId = ps.ActionId LEFT OUTER JOIN
          dbo.RPT_PatientProgramResponse AS pr ON ps.StepId = pr.StepId
WHERE     (pp.[Delete] = 'False') AND pa.archived = 'False'
		  AND ps.sourceid = '52f4fff2c34786626c00002a'
		  AND pr.value != ''
		  AND pr.value is not null
		  AND ps.completed = 'True' 
		  AND pr.[delete] = 'False'
GO
