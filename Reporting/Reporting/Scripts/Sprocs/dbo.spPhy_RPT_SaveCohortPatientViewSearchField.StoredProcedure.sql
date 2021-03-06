SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveCohortPatientViewSearchField] 
	@SearchFieldId INT,
	@CohortPatientViewId INT,
	@FieldName varchar(50),
	@Value varchar(MAX),
	@Active varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT
	
	If Exists(Select Top 1 1 From RPT_CohortPatientViewSearchField Where SearchFieldId = @SearchFieldId)
	Begin
		Update RPT_CohortPatientViewSearchField
		Set Active = @Active,
			CohortPatientViewId = @CohortPatientViewId,
			FieldName = @FieldName,
			Value = @Value
		Where SearchFieldId = @SearchFieldId
		
		Select @ReturnID = SearchFieldId From RPT_CohortPatientViewSearchField Where SearchFieldId = @SearchFieldId
	End
	Else
	Begin
		Insert Into RPT_CohortPatientViewSearchField(Active, CohortPatientViewId, FieldName, SearchFieldId, Value) values (@Active, @CohortPatientViewId, @FieldName, @SearchFieldId, @Value)
		Select @ReturnID = @@IDENTITY
	End
	
	Select @ReturnID
END
GO
