IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_TaskStatistics]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_TaskStatistics]
GO

Create Procedure [dbo].[spPhy_RPT_Flat_TaskStatistics]
As
Begin
	Truncate Table [RPT_Flat_TaskStatistics]

	Insert Into [RPT_Flat_TaskStatistics]
	(
		MongoTaskId
		,MongoPatientId
		,MongoGoalId
		,CreatedOn
		,CreatedBy
		,UpdatedOn
		,UpdatedBy
		,ClosedDate
		,[Status]
		,StartDate
		,TargetDate
		,TargetValue
		,[Description]
		,Details
		,TemplateId
		,PrimaryCareManagerMongoId
		,PrimaryCareManagerPreferredName
		,TaskBarriers
	)
	Select	   
		pt.MongoId as MongoTaskId
		,pg.MongoPatientId
		,pt.MongoPatientGoalId as MongoGoalId
		,pt.RecordCreatedOn	as CreatedOn
		,u2.PreferredName AS CreatedBy
		,pt.LastUpdatedOn as UpdatedOn
		,u3.PreferredName AS UpdatedBy
		,pt.ClosedDate
		,pt.[Status]
		,pt.StartDate
		,pt.TargetDate
		,pt.TargetValue
		,pt.[Description]
		,pt.Details
		,pt.TemplateId
		,u.MongoId AS 'PrimaryCareManagerMongoId'
		,u.PreferredName AS 'PrimaryCareManagerPreferredName'
		,STUFF(
				(Select
					Distinct '| ' + pb.Name
				From RPT_PatientTaskBarrier ptb with (nolock) 
					LEFT OUTER JOIN RPT_PatientBarrier pb with (nolock) on ptb.MongoPatientBarrierId = pb.MongoId
				Where ptb.MongoPatientTaskId = pt.MongoId
				Order By '| ' + pb.Name
				For Xml Path, Type).value('.', 'varchar(max)')
			, 1
			, 2
			, '') as TaskBarriers
	From RPT_PatientTask pt with (nolock)
		LEFT OUTER JOIN RPT_PatientGoal pg with (nolock) on pt.MongoPatientGoalId = pg.MongoId
		LEFT OUTER JOIN [RPT_CareMember] cm with (nolock) on pg.MongoPatientId = cm.MongoPatientId
		LEFT OUTER JOIN [dbo].[RPT_User] u with (nolock) on cm.MongoUserId = u.MongoId	
		LEFT OUTER JOIN [dbo].[RPT_User] u2 WITH(NOLOCK) on pt.MongoRecordCreatedBy = u2.MongoId							
		LEFT OUTER JOIN [dbo].[RPT_User] u3 WITH(NOLOCK) on pt.MongoUpdatedBy = u3.MongoId	
	Where pt.[Delete] = 'False' and pt.TTLDate IS NULL 	
						

End

GO