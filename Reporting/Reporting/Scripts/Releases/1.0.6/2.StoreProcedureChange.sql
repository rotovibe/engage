
/****** Object:  StoredProcedure [dbo].[spPhy_RPT_SavePatientObservation]    Script Date: 3/17/2016 10:55:37 AM ******/
/* ENG1397 Remove Type field */

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientObservation] 
	@MongoID varchar(50),
	@Delete varchar(50),
	@Display varchar(50),
	@LastUpdatedOn datetime,
	@EndDate datetime,
	@NumericValue float,
	@MongoObservationId varchar(50),
	@PatientMongoId varchar(50),
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Source varchar(50),
	@StartDate datetime,
	@State varchar(50),
	@TimeToLive datetime,
	@Units varchar(50),
	@UpdatedBy varchar(50),
	@Version float,
	@AdministeredBy varchar(50),
	--@Type varchar(50),
	@ExtraElements varchar(MAX),
	@NonNumericValue varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@ObservationId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @PatientMongoId
	
	Select @ObservationId = ObservationId From RPT_Observation Where MongoId = @MongoObservationId
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If (@NumericValue = -1)
	Select @NumericValue = NULL
	
	If Exists(Select Top 1 1 From RPT_PatientObservation Where MongoId = @MongoID)
	Begin
		Update RPT_PatientObservation
		Set [Delete] = @Delete,
			Display = @Display,
			EndDate = @EndDate,
			LastUpdatedOn = @LastUpdatedOn,
			NumericValue = @NumericValue,
			ObservationId = @ObservationId,
			MongoObservationId = @MongoObservationId,
			PatientId = @PatientId,
			MongoPatientId = @PatientMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			[Source] = @Source,
			StartDate = @StartDate,
			[State] = @State,
			TTLDate = @TimeToLive,
			Units = @Units,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			AdministeredBy = @AdministeredBy,
			ExtraElements = @ExtraElements,
			--Type = @Type,
			NonNumericValue = @NonNumericValue
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientObservation(AdministeredBy, ExtraElements, --Type, 
		NonNumericValue, [Delete], Display, EndDate, LastUpdatedOn, NumericValue, ObservationId, MongoObservationId, PatientId, MongoPatientId, MongoRecordCreatedBy, RecordCreatedById, RecordCreatedOn, [Source], StartDate, [State], TTLDate, Units, MongoUpdatedBy, UpdatedById, [Version], MongoId) values 
		(@AdministeredBy, @ExtraElements, --@Type, 
		@NonNumericValue, @Delete, @Display, @EndDate, @LastUpdatedOn, @NumericValue, @ObservationId, @MongoObservationId, @PatientId, @PatientMongoId, @RecordCreatedBy, @RecordCreatedById, @RecordCreatedOn, @Source, @StartDate, @State, @TimeToLive, @Units, @UpdatedBy, @UpdatedById, @Version, @MongoID)
	End
END


GO




/****** Object:  StoredProcedure [dbo].[spPhy_RPT_SavePatientClinicalData]    Script Date: 3/17/2016 11:01:35 AM ******/
/* ENG1397 Remove Type field */
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientClinicalData]
AS
BEGIN
	DELETE [RPT_Patient_ClinicalData]
	INSERT INTO [RPT_Patient_ClinicalData]
	(
		 MongoPatientId
		,PatientObservationId
		,MongoId
	    ,MongoObservationId
        ,ObservationType
		,Code
		,CodingSystem
        ,CommonName
        ,Description
        ,Display 
        ,StartDate
        ,EndDate
        ,NumericValue
        ,NonNumericValue
        ,Source
        ,[State]
      --  ,[Type]
        ,Units
        ,AdministeredBy
        ,UpdatedBy
        ,LastUpdatedOn 
        ,CreatedBy
        ,RecordCreatedOn
	) 
	SELECT DISTINCT 	
		 po.MongoPatientId
	    ,po.PatientObservationId
	    ,po.MongoId
		,po.MongoObservationId
        ,otl.Name as ObservationType
		,o.Code
		,csl.Name as CodingSystem
        ,o.CommonName
        ,o.Description
        ,(CASE WHEN po.Display = '0' THEN  NULL ELSE po.Display END) as Display
        ,po.StartDate
        ,po.EndDate
        ,po.NumericValue
        ,po.NonNumericValue
        ,po.Source
        ,po.[State]
        --,po.[Type]
        ,po.Units
        ,po.AdministeredBy
        ,u1.PreferredName as UpdatedBy
        ,po.LastUpdatedOn 
        ,u2.PreferredName as CreatedBy
        ,po.RecordCreatedOn
	FROM
		RPT_PatientObservation as po with (nolock) 	
		LEFT JOIN RPT_User as u1 with (nolock) on po.MongoUpdatedBy = u1.MongoId
		LEFT JOIN RPT_User as u2 with (nolock) on po.MongoRecordCreatedBy = u2.MongoId
		LEFT JOIN RPT_Observation as o with (nolock) on po.MongoObservationId = o.MongoId
		LEFT JOIN RPT_ObservationTypeLookUp as otl with (nolock) on o.MongoObservationLookUpId = otl.MongoId
		LEFT JOIN RPT_CodingSystemLookUp as csl with (nolock) on o.CodingSystemId = csl.MongoId
	WHERE
		po.[Delete] = 'False' and po.TTLDate IS NULL
END
-- Update the NumericValue column based on ObservationType and NonNumericValue.
UPDATE RPT_Patient_ClinicalData
SET NumericValue = NULL
WHERE (ObservationType = 'Problems') OR (ObservationType != 'Problems' AND NonNumericValue IS NOT NULL)


GO


--ENG1675
-- No sp changes
