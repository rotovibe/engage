DROP FUNCTION [dbo].[fn_RPT_Enrollment]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_Enrollment] 
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
GO
