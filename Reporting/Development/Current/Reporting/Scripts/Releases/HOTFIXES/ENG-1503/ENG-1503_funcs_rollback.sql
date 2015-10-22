ALTER FUNCTION [dbo].[fn_RPT_GetPractice_Engage] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50)
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
			from fn_RPT_GetText_SingleSelect(@patientid, @patientprogramid, @ProgramSourceId, '545cee7a890e9458aa000003', '544efd6fac80d37bc000027b') );
			
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
