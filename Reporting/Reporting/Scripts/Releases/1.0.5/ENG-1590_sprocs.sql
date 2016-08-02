IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_InterventionStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_InterventionStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_InterventionStatistics]
As
Begin
	Truncate Table [RPT_Flat_InterventionStatistics]

	Insert Into [RPT_Flat_InterventionStatistics]
	(
		MongoInterventionId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,DueDate
		,[Description]
		,Details
		,CategoryName
		,TemplateId
		,AssignedTo
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,InterventionBarriers
	)
	Select	   
		pi.MongoId as MongoInterventionId
		,pg.MongoPatientId
		,pi.MongoPatientGoalId as MongoGoalId
		,pi.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pi.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pi.ClosedDate
		,pi.[Status]
		,pi.StartDate
		,pi.DueDate
		,pi.Description
		,pi.Details
		,icl.Name as CategoryName
		,pi.TemplateId		
		,u1.PreferredName as AssignedTo
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientInterventionBarrier pib with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on pib.MongoPatientBarrierId = pb.MongoId
				Where pib.MongoPatientInterventionId = pi.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as InterventionBarriers
	From RPT_PatientIntervention pi with (nolock)
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pi.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN RPT_InterventionCategoryLookUp icl with (nolock) on pi.MongoCategoryLookUpId = icl.MongoId
		LEFT OUTER JOIN RPT_User u1 with (nolock) on pi.MongoContactUserId = u1.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pi.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pi.MongoUpdatedBy = u3.MongoId	
	Where pi.[Delete] = 'False' and pi.TTLDate IS NULL 							

End

GO