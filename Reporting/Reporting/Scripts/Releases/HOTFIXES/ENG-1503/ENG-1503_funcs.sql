ALTER FUNCTION [dbo].[fn_RPT_GetPractice_Engage] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50)	
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	--Commonwealth Primary Care
	--Dominion Medical Associates
	--Lee Davis Medical Associates
	--Other
	--Virginia Diabetes and Endocrinology
	--Virginia Family Physicians
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@patientid, @patientprogramid, @ProgramSourceId, @ActionSourceId, @StepSourceId) );
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	--select @Practice;
			
	--SET @Result = 
	--	CASE					
	--		WHEN @Practice = 'Other' THEN -- get this from the other open textbox
	--			(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,'545cee7a890e9458aa000003', '544eff8dac80d37bc000027e') )
				
	--		ELSE
	--			@Practice			
	--	END;
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Practice);
			
	RETURN
END
GO

CREATE FUNCTION [dbo].[fn_RPT_Engage_V3_GetPCP] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	--------------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 47;
	--SET @patientprogramid = 111;
	--SET @ProgramSourceId = '54b69910ac80d33c2c000032';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422deccac80d3356d000002';
	----------------------------------------

	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	--Commonwealth Primary Care
	--Dominion Medical Associates
	--Lee Davis Medical Associates
	--Other
	--Virginia Diabetes and Endocrinology
	--Virginia Family Physicians
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '544efd6fac80d37bc000027b') );
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	
	-------
	--select @Practice;
	----select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '545cee7a890e9458aa000003', '545ce6f8890e9458a9000002')
	-------
			
	SET @Result = 
		CASE
			WHEN @Practice = 'Commonwealth Primary Care' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '545ce698890e9458aa000001') )

			WHEN @Practice = 'Dominion Medical Associates' THEN		
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '545ce6c7890e9458aa000002') )
		
			WHEN @Practice = 'Lee Davis Medical Associates' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '54598bcdac80d36bd1000001') )
			
			WHEN @Practice = 'Virginia Diabetes and Endocrinology' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '545ce65a890e9458a9000001') )
			                 
			WHEN @Practice = 'Virginia Family Physicians' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '545ce6f8890e9458a9000002') )
					
			WHEN @Practice = 'Other' THEN -- get this from the other open textbox
				NULL --@Practice --(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,'544eff8dac80d37bc000027e', '545cee7a890e9458aa000003') )

			WHEN @Practice IS NULL THEN -- this is really CPC
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_Engage_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '55a6a6c8ac80d308d2000255', '545ce698890e9458aa000001') )
		END;
	
	---------
	--select @Result;
	---------
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Result);
			
	RETURN
END

GO


