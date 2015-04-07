SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [fn_RPT_CareBridge_GetPCP] 
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
	
	SET @Practice = (select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422dd36ac80d3356d000001') );
			
	SET @Practice = RTRIM(LTRIM(@Practice));
			
	SET @Result = 
		CASE
			WHEN @Practice = 'CPC' THEN
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
				 from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422de0fac80d3356f000001') )

			WHEN @Practice = 'DMA' THEN		
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
					from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422de52ac80d3356f000002') )
		
			WHEN @Practice = 'LDM' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
					from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422deccac80d3356d000002') )
			
			WHEN @Practice = 'VDE' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
					from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df12ac80d3356d000003') )
			
			WHEN @Practice = 'VFP' THEN					
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
					from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df50ac80d3356f000003') )
					
			WHEN @Practice = 'Other' THEN							
				(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
					from fn_RPT_GetText(@PatientId, @PatientProgramId, @ProgramSourceId, '5453cf73bdd4dfc95100001e', '5422df97ac80d3356d000004') )
		END;
	
	INSERT INTO @GetDateDateTable (Value) VALUES (@Result);
			
	RETURN
END
GO
