/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Value]    Script Date: 05/04/2015 12:02:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Value]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = ''False'' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]		
			ELSE 						
				''0''
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = ''False''
			AND p.ActionCompleted = ''True''
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
' 
END
GO
