/****** Object:  UserDefinedFunction [dbo].[fn_RPT_ActionCompleted_Text]    Script Date: 05/04/2015 12:02:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Text]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Text] 
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
			(CASE WHEN p.[Selected] = ''True'' AND p.[Delete] = ''False'' THEN
					p.[Text]		
			ELSE 						
				NULL --''0''
			END) as [Value]
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
