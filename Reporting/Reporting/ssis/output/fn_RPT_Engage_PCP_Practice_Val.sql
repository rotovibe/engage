/****** Object:  UserDefinedFunction [dbo].[fn_RPT_Engage_PCP_Practice_Val]    Script Date: 05/04/2015 12:02:36 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Engage_PCP_Practice_Val]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
BEGIN
execute dbo.sp_executesql @statement = N'CREATE FUNCTION [dbo].[fn_RPT_Engage_PCP_Practice_Val] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),	
	@StepSourceId VARCHAR(50)
)
RETURNS @ResultTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	----SET @patientid = 546;
	----SET @patientprogramid = 283;
	--SET @patientid = 217;
	--SET @patientprogramid = 280;	
	--SET @ProgramSourceId = ''5465e772bdd4dfb6d80004f7'';
	--SET @ActionSourceId = ''545cee7a890e9458aa000003'';
	--SET @StepSourceId = ''545ce6f8890e9458a9000002'';
	--------------------------------------


	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	-------
	--select @CPCTemp;
	-------
	
	SET @Result =
		CASE
			WHEN @CPCTemp = ''other'' THEN
				''other''
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	---------
	--select @Result
	---------

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END
' 
END
GO
