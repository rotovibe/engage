IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_Latest_PatientObservations]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_Latest_PatientObservations]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_Latest_PatientObservations]
AS
BEGIN
	TRUNCATE TABLE [RPT_Flat_Latest_PatientObservations]

	INSERT INTO [RPT_Flat_Latest_PatientObservations]
	(
		[MongoPatientId],
		[ObservationType],
		[Code],
		[CommonName],
		[Description],
		[State],
		[StartDate],
		[EndDate],
		[NumericValue],
		[NonNumericValue],
		[PrimaryCareManagerMongoId],
		[PrimaryCareManagerFirstName],
		[PrimaryCareManagerLastName],
		[PrimaryCareManagerPreferredName]
	)
	SELECT 
		obs.MongoPatientId,
		obs.ObservationType,
		obs.Code,
		obs.CommonName,
		obs.[Description],
		obs.[State],
		obs.StartDate,
		obs.EndDate,
		obs.NumericValue,
		obs.NonNumericValue,
		u.MongoId AS 'PrimaryCareManagerMongoId',
		u.FirstName AS 'PrimaryCareManagerFirstName',
		u.LastName AS 'PrimaryCareManagerLastName',
		u.PreferredName AS 'PrimaryCareManagerPreferredName'
	FROM (SELECT ROW_NUMBER() OVER (PARTITION BY MongoPatientId, ObservationType, [Code], [Description]  ORDER BY StartDate DESC, LastUpdatedOn DESC) AS RowNumber,
			MongoPatientId,
			ObservationType,
			Code,
			[Description],
			CommonName,
			[State],
			StartDate,
			EndDate,
			NumericValue,
			NonNumericValue
		FROM RPT_Patient_ClinicalData pcd WITH(NOLOCK)
		) obs
	INNER JOIN	RPT_Patient p WITH(NOLOCK)
		ON obs.MongoPatientId = p.MongoId
	LEFT OUTER JOIN [RPT_CareMember] cm WITH(NOLOCK)
		ON obs.MongoPatientId = cm.MongoPatientId
	LEFT OUTER JOIN [dbo].[RPT_User] u WITH(NOLOCK)
		ON cm.MongoUserId = u.MongoId
	WHERE obs.RowNumber = 1
	and p.[Delete] = 'False' and p.TTLDate IS NULL

END

GO