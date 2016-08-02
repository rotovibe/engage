IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BarrierStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BarrierStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_BarrierStatistics]
As
Begin
	Truncate Table [RPT_Flat_BarrierStatistics]

	Insert Into [RPT_Flat_BarrierStatistics]
	(
		MongoBarrierId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,[Status]
		,Name
		,Details
		,Category
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
	)

	Select	   
		pb.MongoId as MongoBarrierId
		,pg.MongoPatientId
		,pb.MongoPatientGoalId as MongoGoalId
		,pb.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pb.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pb.[Status]
		,pb.Name
		,pb.Details
		,bcl.Name as Category
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
	From RPT_PatientBarrier pb with (nolock)
		LEFT OUTER JOIN RPT_BarrierCategoryLookUp as bcl with (nolock) on pb.MongoCategoryLookUpId = bcl.MongoId
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pb.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pb.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pb.MongoUpdatedBy = u3.MongoId	
	Where pb.[Delete] = 'False' and pb.TTLDate IS NULL 	



End

GO