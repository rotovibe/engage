/****** Object:  StoredProcedure [dbo].[spPhy_RPT_ProgramResponse_Flat]    Script Date: 05/04/2015 12:02:34 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_ProgramResponse_Flat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_ProgramResponse_Flat]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
AS
BEGIN
	TRUNCATE TABLE RPT_ProgramResponse_Flat
	INSERT INTO RPT_ProgramResponse_Flat
		([PatientId]			,
		[PatientProgramId]	,
		[ProgramSourceId]	,
		[ActionSourceId]	,
		[ActionCompleted]	,
		[ActionArchived]	,
		[ActionArchivedDate],	
		[StepSourceId]		,
		[Text]				,
		[Value]				,
		[Selected]			,
		[Delete],
		[StepCompleted]		,
		[DateCompleted]		,
		[ActionName]	,
		[Question])
	SELECT
		ptx.PatientId as			[PatientId]			,
		ppx.PatientProgramId as		[PatientProgramId]	,
		ppx.SourceId as				[ProgramSourceId]	,
		pax.SourceId as				[ActionSourceId]	,
		pax.Completed as            [ActionCompleted]	,
		pax.Archived as				[ActionArchived]	,
		pax.ArchivedDate as			[ActionArchivedDate],	
		psx.SourceId as				[StepSourceId]		,
		prx.[Text] as				[Text]				,
		prx.[Value] as				[Value]				,
		prx.Selected as				[Selected]			,
		prx.[Delete] as				[Delete]			,
		psx.Completed as			[StepCompleted]		,
		pax.DateCompleted as		[DateCompleted]		,
		pax.Name as					[ActionName],
		psx.Question as				[Question]
	FROM
		RPT_Patient AS ptx with (nolock) INNER JOIN 
		RPT_PatientProgram AS ppx with (nolock)  ON ptx.PatientId = ppx.PatientId
		INNER JOIN RPT_PatientProgramModule AS pmx with (nolock) ON ppx.PatientProgramId = pmx.PatientProgramId  				
		INNER JOIN RPT_PatientProgramAction AS pax with (nolock)  ON pmx.PatientProgramModuleId = pax.PatientProgramModuleId  				
		INNER JOIN RPT_PatientProgramStep AS psx with (nolock) ON pax.ActionId = psx.ActionId  				
		LEFT OUTER JOIN RPT_PatientProgramResponse AS prx with (nolock) ON psx.StepId = prx.StepId
END
' 
END
GO
