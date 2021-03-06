SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_PatientMedSuppNDC]
	@MongoPatientMedSuppId	[varchar](50),
	@NDC					[varchar] (200)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientMedSuppId INT
	
	Select @PatientMedSuppId = PatientMedSuppId from RPT_PatientMedSupp Where MongoId = @MongoPatientMedSuppId
		
	If Exists(Select Top 1 1 From RPT_PatientMedSuppNDC Where PatientMedSuppId = @PatientMedSuppId AND NDC = @NDC)
		Begin
			Update RPT_PatientMedSuppNDC
			Set 
				PatientMedSuppId		= @PatientMedSuppId,		
				MongoPatientMedSuppId	= @MongoPatientMedSuppId,
				NDC					 	= @NDC						
			Where 
				MongoPatientMedSuppId = @MongoPatientMedSuppId
		End
	Else
		Begin
			Insert Into RPT_PatientMedSuppNDC
			(
				PatientMedSuppId,
				MongoPatientMedSuppId,
				NDC
			) 
			values 
			(
				@PatientMedSuppId,		
				@MongoPatientMedSuppId,
				@NDC						
			)
		End
END
GO
