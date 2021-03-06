DROP FUNCTION [dbo].[fn_RPT_PCP_Practice_Val]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_PCP_Practice_Val] 
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

	------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 135;
	--SET @patientprogramid = 269;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422deccac80d3356d000002';
	--------------------------------

	DECLARE @CPCTemp VARCHAR(200);
	DECLARE @Result VARCHAR(200);
	
	SET @CPCTemp = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
	SET @CPCTemp = LOWER(RTRIM(LTRIM(@CPCTemp)));
	
	SET @Result =
		CASE
			WHEN @CPCTemp = 'other' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df97ac80d3356d000004') )							
			ELSE
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, @ActionSourceId, @StepSourceId) )
		END;

	INSERT INTO @ResultTable (Value) VALUES (@Result);
			
	RETURN
END
GO
