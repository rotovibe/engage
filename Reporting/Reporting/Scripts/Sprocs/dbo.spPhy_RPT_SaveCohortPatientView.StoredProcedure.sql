SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_SaveCohortPatientView] 
	@MongoID	varchar(50),
	@PatientId INT,
	@LastName	varchar(100),
	@Version varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @ReturnID	INT
	
	If Exists(Select Top 1 1 From RPT_CohortPatientView Where MongoId = @MongoID)
	Begin
		Update RPT_CohortPatientView
		Set PatientId = @PatientId,
			LastName = @LastName,
			[Version] = @Version
		Where MongoId = @MongoID
		
		Select @ReturnID = CohortPatientViewId From RPT_CohortPatientView Where MongoId = @MongoID
	End
	Else
	Begin
		Insert Into RPT_CohortPatientView(PatientId, LastName, [Version], MongoId) values (@PatientId, @LastName, @Version, @MongoID)
		Select @ReturnID = @@IDENTITY
	End
	
	Select @ReturnID
END
GO
