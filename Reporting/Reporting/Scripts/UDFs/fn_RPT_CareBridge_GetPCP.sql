CREATE FUNCTION [dbo].[fn_RPT_CareBridge_GetPCP] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN

	----------- TESTING HARNESS --------
	--DECLARE @patientid INT;
	--DECLARE @patientprogramid INT;
	--DECLARE @ProgramSourceId VARCHAR(50);
	--DECLARE @ActionSourceId VARCHAR(50);	
	--DECLARE @StepSourceId VARCHAR(50);
	
	--SET @patientid = 45;
	--SET @patientprogramid = 304;
	--SET @ProgramSourceId = '5453f570bdd4dfcef5000330';
	--SET @ActionSourceId = '5453cf73bdd4dfc95100001e';
	--SET @StepSourceId = '5422deccac80d3356d000002';
	------------------------------------
	
	DECLARE @Practice VARCHAR(100);
	DECLARE @Result VARCHAR(200);
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText_SingleSelect(@PatientId, @PatientProgramId, '5453f570bdd4dfcef5000330', '5453cf73bdd4dfc95100001e', '5422dd36ac80d3356d000001') );
			
			
	SET @Practice = RTRIM(LTRIM(@Practice));
	-------
	--SELECT @Practice;
	-------
			
	SET @Result = 
		CASE
			WHEN @Practice = 'CPC' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422de0fac80d3356f000001') )

			WHEN @Practice = 'DMA' THEN		
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422de52ac80d3356f000002') )
		
			WHEN @Practice = 'LDM' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422deccac80d3356d000002') )
			
			WHEN @Practice = 'VDE' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df12ac80d3356d000003') )
			
			WHEN @Practice = 'VFP' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df50ac80d3356f000003') )
					
			WHEN @Practice = 'Other' THEN -- get this from the other open textbox
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCPOther(@PatientId, @PatientProgramId, @ProgramSourceId,'5453cf73bdd4dfc95100001e','5422df97ac80d3356d000004') )

			WHEN @Practice IS NULL THEN -- this is really CPC
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END from fn_RPT_PCP_Practice_Val(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422de0fac80d3356f000001') )
		END;
	
	--select @Result;
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Result);
			
	RETURN
END

GO


