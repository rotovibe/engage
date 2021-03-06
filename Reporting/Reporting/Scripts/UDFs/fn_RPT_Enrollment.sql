/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Enrollment]    Script Date: 05/04/2015 12:02:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Enrollment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Enrollment]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Enrollment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_Enrollment] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @EnrollmentTable TABLE( Enrollment VARCHAR(2000))
AS
BEGIN
		INSERT INTO @EnrollmentTable
		select 
			ppa.Enrollment
		from 
			RPT_PatientProgram p,
			RPT_PatientProgramAttribute ppa,
			RPT_PatientProgramModule m,
			RPT_PatientProgramAction a,
			RPT_Patientprogramstep s,
			RPT_PatientProgramResponse r
		where 
			p.MongoId = ppa.MongoPlanElementId
			and p.MongoId = m.MongoProgramId
			and m.MongoId = a.MongoModuleId
			and a.mongoId = s.MongoActionId
			and s.MongoId = r.MongoStepId
			and p.PatientProgramId = @patientprogramid
			and p.SourceId = @ProgramSourceId
			and a.SourceId = @ActionSourceId
			and s.SourceId = @StepSourceId
	RETURN
END
' 
END
GO
