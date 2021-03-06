SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_PatientMedSuppPhClass]
	@MongoPatientMedSuppId		[varchar](50),
	@PharmClass					[varchar] (2000)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientMedSuppId INT
	
	Select @PatientMedSuppId = PatientMedSuppId from RPT_PatientMedSupp Where MongoId = @MongoPatientMedSuppId
		
	If Exists(Select Top 1 1 From RPT_PatientMedSuppPhClass Where PatientMedSuppId = @PatientMedSuppId AND PharmClass = @PharmClass)
		Begin
			Update RPT_PatientMedSuppPhClass
			Set 
				PatientMedSuppId		= @PatientMedSuppId,		
				MongoPatientMedSuppId	= @MongoPatientMedSuppId,
				PharmClass				= @PharmClass
			Where 
				MongoPatientMedSuppId = @MongoPatientMedSuppId
		End
	Else
		Begin
			Insert Into RPT_PatientMedSuppPhClass
			(
				PatientMedSuppId,
				MongoPatientMedSuppId,
				PharmClass
			) 
			values 
			(
				@PatientMedSuppId,		
				@MongoPatientMedSuppId,
				@PharmClass
			)
		End
END
GO
