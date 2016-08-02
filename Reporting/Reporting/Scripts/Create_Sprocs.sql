IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveAllergyTypeLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveAllergyTypeLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveAllergySourceLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveAllergySourceLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveSeverityLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveSeverityLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveReactionLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveReactionLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveMedSupTypeLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveMedSupTypeLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveFreqHowOftenLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveFreqHowOftenLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveFreqWhenLookUp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveFreqWhenLookUp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveAllergy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveAllergy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SaveAllergyType]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SaveAllergyType]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientAllergy]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientAllergy]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientAllergyReaction]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientAllergyReaction]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_SavePatientMedSupp]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_PatientMedSuppNDC]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_PatientMedSuppNDC]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_PatientMedSuppPhClass]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_PatientMedSuppPhClass]
GO

/****** UserDefinedFunction [dbo].[fn_RPT_Market] ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Market]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Market]
GO

CREATE FUNCTION [dbo].[fn_RPT_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000), Completed VARCHAR(50))
AS
BEGIN
		DECLARE @count int;
		DECLARE @Completed varchar(50);
		
		SET @Completed = 
			(
			SELECT top 1 
				pax.Completed
			FROM
				RPT_Patient AS ptx with (nolock) INNER JOIN 
				RPT_PatientProgram AS ppx with (nolock)  ON ptx.PatientId = ppx.PatientId
				INNER JOIN RPT_PatientProgramModule AS pmx with (nolock) ON ppx.PatientProgramId = pmx.PatientProgramId  				
				INNER JOIN RPT_PatientProgramAction AS pax with (nolock)  ON pmx.PatientProgramModuleId = pax.PatientProgramModuleId  				
				INNER JOIN RPT_PatientProgramStep AS psx with (nolock) ON pax.ActionId = psx.ActionId  				
				LEFT OUTER JOIN RPT_PatientProgramResponse AS prx with (nolock) ON psx.StepId = prx.StepId 		
			WHERE 				
				ppx.SourceId = '541943a6bdd4dfa5d90002da' 				
				AND ((psx.SourceId = '53f4fb39ac80d30e00000067')  OR (psx.SourceId = '532c3e76c347865db8000001') ) 				
				AND ((pax.SourceId = '53f4fd75ac80d30e00000083') OR (pax.SourceId = '532c45bff8efe36886000446') ) 				
				AND (ppx.[Delete] = 'False') 				
				AND (pax.Archived = 'False') 				
				AND prx.Selected = 'True' 				
				AND ptx.PatientId = @patientid 				
				AND ppx.patientprogramid = @patientprogramid
			)
		
		
		IF @Completed = 'True' OR @Completed IS NULL
			BEGIN
				INSERT INTO @MarketTable
					SELECT top 1 
						CASE WHEN prx.[Delete] = 'False' THEN
							prx.[Text]
						ELSE
							NULL
						END   
						, pax.Completed
					FROM
						RPT_Patient AS ptx with (nolock) INNER JOIN 
						RPT_PatientProgram AS ppx with (nolock)  ON ptx.PatientId = ppx.PatientId
						INNER JOIN RPT_PatientProgramModule AS pmx with (nolock) ON ppx.PatientProgramId = pmx.PatientProgramId  				
						INNER JOIN RPT_PatientProgramAction AS pax with (nolock)  ON pmx.PatientProgramModuleId = pax.PatientProgramModuleId  				
						INNER JOIN RPT_PatientProgramStep AS psx with (nolock) ON pax.ActionId = psx.ActionId  				
						LEFT OUTER JOIN RPT_PatientProgramResponse AS prx with (nolock) ON psx.StepId = prx.StepId 		
					WHERE 				
						ppx.SourceId = '541943a6bdd4dfa5d90002da' 				
						AND ((psx.SourceId = '53f4fb39ac80d30e00000067')  OR (psx.SourceId = '532c3e76c347865db8000001') ) 				
						AND ((pax.SourceId = '53f4fd75ac80d30e00000083') OR (pax.SourceId = '532c45bff8efe36886000446') ) 				
						AND (ppx.[Delete] = 'False') 				
						AND (pax.Archived = 'False') 				
						AND prx.Selected = 'True' 				
						AND ptx.PatientId = @patientid 				
						AND ppx.patientprogramid = @patientprogramid
			END
	
	
	--select and delete need to true, completed must true stay in current action
	--achive false, selected true, delete false - do not go to archive.
	
	
	set @count = (select COUNT(*) from @MarketTable);
	--set @Completed = (select Completed from @MarketTable);		

	IF @Completed = 'False' OR @count = 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
				SELECT TOP 1 				
						--CASE WHEN prxx.[Delete] = 'False' THEN
							prxx.[Text]
						--ELSE
							--NULL
						--END 
						, paxx.Completed   
				 FROM
					RPT_Patient AS ptxx with (nolock)
					INNER JOIN RPT_PatientProgram AS ppxx with (nolock)  ON ptxx.PatientId = ppxx.PatientId  				
					INNER JOIN RPT_PatientProgramModule AS pmxx with (nolock) ON ppxx.PatientProgramId = pmxx.PatientProgramId  				
					INNER JOIN RPT_PatientProgramAction AS paxx with (nolock)  ON pmxx.PatientProgramModuleId = paxx.PatientProgramModuleId  				
					INNER JOIN RPT_PatientProgramStep AS psxx with (nolock) ON paxx.ActionId = psxx.ActionId  				
					LEFT OUTER JOIN RPT_PatientProgramResponse AS prxx with (nolock) ON psxx.StepId = prxx.StepId 			 
				 WHERE  				
					ppxx.SourceId = '541943a6bdd4dfa5d90002da' 				
					AND ((psxx.SourceId = '53f4fb39ac80d30e00000067')  OR (psxx.SourceId = '532c3e76c347865db8000001') ) 				
					AND ((paxx.SourceId = '53f4fd75ac80d30e00000083') OR (paxx.SourceId = '532c45bff8efe36886000446') ) 				
					AND (ppxx.[Delete] = 'False') 				
					AND prxx.Selected = 'True' 				
					AND ptxx.PatientId = @patientid	
					AND prxx.[Delete] = 'False'			
					AND ppxx.patientprogramid = @patientprogramid
					AND paxx.Archived = 'True'
					AND paxx.ActionId IN ( 
										 SELECT DISTINCT
											paxxx.ActionId
										 FROM
											RPT_Patient AS ptxxx with (nolock)
											INNER JOIN RPT_PatientProgram AS ppxxx with (nolock)  ON ptxxx.PatientId = ppxxx.PatientId  						
											INNER JOIN RPT_PatientProgramModule AS pmxxx with (nolock) ON ppxxx.PatientProgramId = pmxxx.PatientProgramId  						
											INNER JOIN RPT_PatientProgramAction AS paxxx with (nolock)  ON pmxxx.PatientProgramModuleId = paxxx.PatientProgramModuleId  						
											INNER JOIN RPT_PatientProgramStep AS psxxx with (nolock) ON paxxx.ActionId = psxxx.ActionId  						
											LEFT OUTER JOIN RPT_PatientProgramResponse AS prxxx with (nolock) ON psxxx.StepId = prxxx.StepId 					
										 WHERE 						
											ppxxx.SourceId = '541943a6bdd4dfa5d90002da' 						
											AND ((psxxx.SourceId = '53f4fb39ac80d30e00000067')  OR (psxxx.SourceId = '532c3e76c347865db8000001') ) 						
											AND ((paxxx.SourceId = '53f4fd75ac80d30e00000083') OR (paxxx.SourceId = '532c45bff8efe36886000446') ) 						
											AND (ppxxx.[Delete] = 'False') 						
											AND prxxx.Selected = 'True' 						
											AND ptxxx.PatientId = @patientid 						
											AND ppxxx.patientprogramid = @patientprogramid
										 ) 
				ORDER BY paxx.ArchivedDate DESC
		END
	RETURN
END

GO

/****** StoredProcedure [dbo].[spPhy_RPT_Flat_BSHSI_HW2]   ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
AS
BEGIN
	DELETE [RPT_BSHSI_HW2_Enrollment_Info]
	INSERT INTO [RPT_BSHSI_HW2_Enrollment_Info] 
	SELECT DISTINCT 	
			pt.PatientId 	
			,ppt.PatientProgramId
			,pt.[Priority]
			, pt.firstName  	 	
			, ps.SystemId  	 	
			, pt.LastName  	 	
			, pt.MiddleName  	 	
			, pt.Suffix  	 	
			, pt.Gender  	 	
			, pt.DateOfBirth
			,pt.LSSN 		
			,(SELECT  		
				u.PreferredName 	  
			  FROM
				RPT_Patient as p,
				RPT_User as u,
				RPT_CareMember as c 	 		 	  
			  WHERE
				p.MongoId = c.MongoPatientId
				AND c.MongoUserId = u.MongoId
				AND p.PatientId = pt.PatientId 	) AS [Assigned_PCM]  	 	
			,(SELECT TOP 1
				u.PreferredName as [fullname]
			  FROM
				rpt_patient as p with (nolock)
				INNER JOIN rpt_patientprogram as pp with (nolock)  ON p.MongoId = pp.MongoPatientId
				INNER JOIN rpt_user as u with (nolock) ON pp.MongoAssignedToId = u.MongoId
			  WHERE
				p.PatientId = pt.PatientId
				AND pp.Name = 'BSHSI - Healthy Weight v2'
				AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
			,(  SELECT TOP 1 	
					eppa.Enrollment 	
				FROM
					RPT_Patient as ept with (nolock) 	
					INNER JOIN RPT_PatientProgram as eppt with (nolock) ON ept.PatientId = eppt.PatientId  	
					INNER JOIN RPT_PatientProgramModule AS epm with (nolock) ON eppt.PatientProgramId = epm.PatientProgramId  	
					INNER JOIN RPT_PatientProgramAction AS epa with (nolock)  ON epm.PatientProgramModuleId = epa.PatientProgramModuleId        	
					INNER JOIN RPT_PatientProgramAttribute as eppa with (nolock) ON eppt.MongoId = eppa.MongoPlanElementId  
					LEFT JOIN RPT_PatientSystem as eps with (nolock) ON ept.PatientId = eps.PatientId   	 
				WHERE
					ept.[Delete] = 'False'
					AND eppt.SourceId = '541943a6bdd4dfa5d90002da'
					AND epa.[State] IN ('Completed')
					AND ept.PatientId = pt.PatientId
					AND (epa.Archived = 'False')
					AND (epa.[Delete] = 'False' OR epa.[Delete] IS NULL) 
			 ) as [Enrollment]
			,ppa.GraduatedFlag 	
			,ppt.AttributeStartDate as [StartDate] 	
			,ppt.AttributeEndDate as [EndDate] 	
			,ppt.AssignedOn as [Assigned_Date] 	
			,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
			,ppt.[State] as [State] 
			,ppa.Eligibility	
			,(SELECT TOP 1 [Value] 		FROM vw_RPT_Program_Completed_Date WITH (NOLOCK) where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight v2' ORDER BY RecordCreatedOn DESC ) as [Program_Completed_Date]        	
			,(SELECT TOP 1 [Value]  FROM vw_RPT_ReEnrollment_Date  WITH (NOLOCK) where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight v2' ORDER BY RecordCreatedOn DESC ) as [Re_enrollment_Date]        	
			,(SELECT TOP 1 [Value]  FROM vw_RPT_Enrolled_Date  WITH (NOLOCK) where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight v2' ORDER BY RecordCreatedOn DESC ) as [Enrolled_Date]        	
			,(SELECT TOP 1 [Value]  FROM vw_RPT_Pending_Enrollment  WITH (NOLOCK) where PatientId = pt.PatientId AND name = 'BSHSI - Healthy Weight v2' ORDER BY RecordCreatedOn DESC ) as [Pending_Enrolled_Date]        	
			,(SELECT pa.DateCompleted FROM RPT_Patient AS pts with (nolock) INNER JOIN RPT_PatientProgram AS ptp with (nolock) ON pts.PatientId = ptp.PatientId INNER JOIN      RPT_PatientProgramModule AS pm with (nolock) ON ptp.PatientProgramId = pm.PatientProgramId                 INNER JOIN  RPT_PatientProgramAction AS pa with (nolock) ON pm.PatientProgramModuleId = pa.PatientProgramModuleId                INNER JOIN      RPT_PatientProgramStep AS ps with (nolock) ON pa.ActionId = ps.ActionId                   LEFT OUTER JOIN   RPT_PatientProgramResponse AS pr with (nolock) ON ps.StepId = pr.StepId         WHERE ((ps.SourceId = '532c3e1ef8efe368860003b3') OR (ps.SourceId = '53f4f920ac80d30e00000066'))              AND ((pa.SourceId = '532c45bff8efe36886000446') OR (pa.SourceId = '53f4fd75ac80d30e00000083') )                AND (ptp.[Delete] = 'False')             AND (pa.Archived = 'False')               AND pa.State = 'Completed'                AND pts.PatientId = pt.PatientId                AND ptp.PatientProgramId = ppt.PatientProgramId) as [Enrollment_Action_Completion_Date]              	
			,( select Market FROM dbo.fn_RPT_Market(pt.PatientId,ppt.PatientProgramId) ) as [Market]
			,( 		
					SELECT					
						CASE WHEN pr3.[Selected] = 'True' AND pa3.[Archived] = 'False' THEN
							CASE WHEN pr3.[Delete] = 'False'
								THEN
									CASE WHEN ISDATE(pr3.Value) = 1 THEN 
										CAST(pr3.Value AS DATE) 
									ELSE 
										NULL 
									END
							ELSE
								NULL
							END		
						ELSE
							(SELECT   TOP 1 				
								CASE WHEN pa4.Archived = 'True' AND pr4.Selected = 'True' THEN --pr4.[delete] = 'False' AND pa4.[State] = 'Completed' THEN
									CASE WHEN pr4.[Delete] = 'False' THEN
										CASE WHEN ISDATE(pr4.Value) = 1 THEN 
											CAST(pr4.Value AS DATE) 
										ELSE 
											NULL 
										END
									ELSE
										NULL
									END									
								ELSE
									NULL
								END AS [DisEnrollValueArch]
							  FROM RPT_Patient AS pt4 with (nolock) INNER JOIN RPT_PatientProgram AS pp4 with (nolock) ON pt4.PatientId = pp4.PatientId INNER JOIN 				RPT_PatientProgramModule AS pm4 with (nolock) ON pp4.PatientProgramId = pm4.PatientProgramId INNER JOIN 				RPT_PatientProgramAction AS pa4 with (nolock) ON pm4.PatientProgramModuleId = pa4.PatientProgramModuleId INNER JOIN 				RPT_PatientProgramStep AS ps4 with (nolock) ON pa4.ActionId = ps4.ActionId LEFT OUTER JOIN 				RPT_PatientProgramResponse AS pr4 with (nolock) ON ps4.StepId = pr4.StepId 			WHERE      			((ps4.SourceId = '53f56eb7ac80d31203000001') OR (ps4.SourceId = '532c4061f8efe368860003b6') ) 			AND ((pa4.SourceId = '53f57115ac80d31203000014') OR (pa4.SourceId = '532c46b3c347865db8000092')) 			AND pp4.Name = 'BSHSI - Healthy Weight v2' 			AND (pp4.[Delete] = 'False') 			AND pr4.Value != '' 			AND pa4.[State] IN ('Completed') 			AND pa4.Archived = 'True' 			AND pt4.patientid = pt.patientid 			AND pp4.patientprogramid = ppt.patientprogramid 			AND pa4.ActionId IN ( 				SELECT		 					DISTINCT pa5.ActionId 				FROM          					RPT_Patient AS pt5 with (nolock) INNER JOIN 					RPT_PatientProgram AS pp5 with (nolock)  ON pt5.PatientId = pp5.PatientId INNER JOIN 					RPT_PatientProgramModule AS pm5 with (nolock) ON pp5.PatientProgramId = pm5.PatientProgramId INNER JOIN 					RPT_PatientProgramAction AS pa5 with (nolock)  ON pm5.PatientProgramModuleId = pa5.PatientProgramModuleId INNER JOIN 					RPT_PatientProgramStep AS ps5 with (nolock) ON pa5.ActionId = ps5.ActionId LEFT OUTER JOIN 					RPT_PatientProgramResponse AS pr5 with (nolock) ON ps5.StepId = pr5.StepId 				WHERE   					((ps5.SourceId = '53f56eb7ac80d31203000001') OR (ps5.SourceId = '532c4061f8efe368860003b6') ) 					AND ((pa5.SourceId = '53f57115ac80d31203000014') OR (pa5.SourceId = '532c46b3c347865db8000092')) 					AND pp5.Name = 'BSHSI - Healthy Weight v2' 					AND (pp5.[Delete] = 'False') 					AND pr5.Value != '' 					AND pa5.[State] IN ('Completed') 					AND pt5.PatientId = pt4.PatientId 					AND pp5.patientprogramid = ppt.patientprogramid 			) 			ORDER BY pa4.ArchivedDate DESC)
						END AS [DisEnrollValue]
					  FROM
						RPT_Patient AS pt3 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp3 with (nolock) ON pt3.PatientId = pp3.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm3 with (nolock) ON pp3.PatientProgramId = pm3.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa3 with (nolock) ON pm3.PatientProgramModuleId = pa3.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps3 with (nolock) ON pa3.ActionId = ps3.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr3 with (nolock) ON ps3.StepId = pr3.StepId
					  WHERE
						((ps3.SourceId = '53f56eb7ac80d31203000001') OR (ps3.SourceId = '532c4061f8efe368860003b6') )
						AND ((pa3.SourceId = '53f57115ac80d31203000014') OR (pa3.SourceId = '532c46b3c347865db8000092'))
						AND pp3.Name = 'BSHSI - Healthy Weight v2'
						AND (pp3.[Delete] = 'False') 
						--AND (pa3.Archived = 'False')
						AND pr3.Value != ''
						AND pt3.patientid = pt.patientid
						AND pa3.[State] IN ( 'Completed')
						AND pp3.patientprogramid = ppt.patientprogramid
					) as [Disenroll_Date]	 	
			,( 	SELECT TOP 1
					CASE WHEN pr6.[Selected] = 'True' AND pa6.[Archived] = 'False' THEN -- pa6.Archived = 'False' AND pr6.Selected = 'True' THEN
						CASE WHEN pr6.[Delete] = 'False' THEN
							pr6.[Text]
						ELSE
							NULL
						END			
					ELSE 						
						(SELECT TOP 1 				
							CASE WHEN  pa7.Archived = 'True' AND pr7.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
								CASE WHEN pr7.[Delete] = 'False' THEN
									pr7.[Text]
								ELSE
									NULL
								END
							ELSE
								NULL
							END  AS [DisenrollReasonText]	
						 FROM RPT_Patient AS pt7 with (nolock) INNER JOIN 				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN 				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN 				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN 				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN 				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId 			WHERE      				((ps7.SourceId= '53f56f10ac80d31200000001') OR (ps7.SourceId= '532c407fc347865db8000003') ) 				AND ((pa7.SourceId = '53f57115ac80d31203000014') OR (pa7.SourceId = '532c46b3c347865db8000092')) 				AND pp7.Name = 'BSHSI - Healthy Weight v2' 				AND (pp7.[Delete] = 'False')  				AND pr7.Selected = 'True' 				AND pa7.[State] IN ('Completed') 				AND pa7.Archived = 'True' 				AND pt7.patientid = pt.patientid 				AND pp7.patientprogramid = ppt.patientprogramid 				AND pa7.ActionId IN ( 					SELECT		 						DISTINCT pa8.ActionId 					FROM          						RPT_Patient AS pt8 with (nolock) INNER JOIN 						RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN 						RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN 						RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN 						RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN 						RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId 					WHERE   						((ps8.SourceId = '53f56eb7ac80d31203000001') OR (ps8.SourceId = '532c4061f8efe368860003b6') ) 						AND ((pa8.SourceId = '53f57115ac80d31203000014') OR (pa8.SourceId = '532c46b3c347865db8000092')) 						AND pp8.Name = 'BSHSI - Healthy Weight v2' 						AND (pp8.[Delete] = 'False') 						AND pa8.[State] IN ('Completed')	 						AND pt8.PatientId = pt7.PatientId 						AND pp8.patientprogramid = ppt.patientprogramid 				) 			ORDER BY pa7.ArchivedDate DESC)
			 		END   AS [ReasonText]
			 	FROM 
			 		RPT_Patient AS pt6 with (nolock) INNER JOIN
			 		RPT_PatientProgram AS pp6 with (nolock) ON pt6.PatientId = pp6.PatientId INNER JOIN 				
			 		RPT_PatientProgramModule AS pm6 with (nolock) ON pp6.PatientProgramId = pm6.PatientProgramId INNER JOIN 				
			 		RPT_PatientProgramAction AS pa6 with (nolock) ON pm6.PatientProgramModuleId = pa6.PatientProgramModuleId INNER JOIN 				
			 		RPT_PatientProgramStep AS ps6 with (nolock) ON pa6.ActionId = ps6.ActionId LEFT OUTER JOIN 				
			 		RPT_PatientProgramResponse AS pr6 with (nolock) ON ps6.StepId = pr6.StepId 			
			 	WHERE      				
			 		((ps6.SourceId= '53f56f10ac80d31200000001') OR (ps6.SourceId= '532c407fc347865db8000003') ) 				
			 		AND ((pa6.SourceId = '53f57115ac80d31203000014') OR (pa6.SourceId = '532c46b3c347865db8000092')) 				
			 		AND pp6.Name = 'BSHSI - Healthy Weight v2' 				
			 		AND (pp6.[Delete] = 'False') 
			 		--AND (pa6.Archived = 'False') 				
			 		AND pr6.Selected = 'True' 				
			 		AND pa6.[State] IN ('Completed') 				
			 		--AND pa6.Archived = 'False' 				
			 		AND pt6.patientid = pt.patientid 				
			 		AND pp6.patientprogramid = ppt.patientprogramid
			 ) as [Disenroll_Reason]	
			,( 		
				SELECT					
					CASE WHEN pr9.[Selected] = 'True' AND pa9.[Archived] = 'False' THEN
						CASE WHEN pr9.[Delete] = 'False' THEN
							CASE WHEN ISDATE(pr9.Value) = 1 THEN 
								CAST(pr9.Value AS DATE) 
							ELSE 
								NULL 
							END
						ELSE
							NULL
						END						
					ELSE
						(SELECT	TOP 1 
							 CASE WHEN pa10.Archived = 'True' AND pr10.Selected = 'True' THEN --pr10.[delete] = 'False' AND pa10.[State] = 'Completed' THEN 
								CASE WHEN pr10.[Delete] = 'False' THEN
									CASE WHEN ISDATE(pr10.Value) = 1 THEN 
										CAST(pr10.Value AS DATE) 
									ELSE 
										NULL 
									END
								ELSE
									NULL
								END								
							 ELSE
								NULL								
							 END AS [didnotenrollValuearch]
						 FROM RPT_Patient AS pt10 WITH (NOLOCK) INNER JOIN RPT_PatientProgram AS pp10 WITH (NOLOCK) ON pt10.PatientId = pp10.PatientId INNER JOIN 				RPT_PatientProgramModule AS pm10 WITH (NOLOCK) ON pp10.PatientProgramId = pm10.PatientProgramId INNER JOIN 				RPT_PatientProgramAction AS pa10 WITH (NOLOCK) ON pm10.PatientProgramModuleId = pa10.PatientProgramModuleId INNER JOIN 				RPT_PatientProgramStep AS ps10 WITH (NOLOCK) ON pa10.ActionId = ps10.ActionId LEFT OUTER JOIN 				RPT_PatientProgramResponse AS pr10 WITH (NOLOCK) ON ps10.StepId = pr10.StepId 			WHERE  				pp10.SourceId = '541943a6bdd4dfa5d90002da'     				AND ps10.Question = 'Date did not enroll:'  				AND pa10.Name = 'Enrollment'  				AND (pp10.[Delete] = 'False')  				AND (pa10.Archived = 'TRUE')  				AND pa10.ArchivedDate IS NOT NULL 				AND pr10.Value != ''  				AND pt10.patientid = pt.patientid	 				AND pa10.[State] IN ('Completed')	 				AND pp10.patientprogramid = ppt.patientprogramid 				AND pa10.ActionId IN ( 					SELECT		 						DISTINCT pa11.ActionId 					FROM          						RPT_Patient AS pt11 with (nolock) INNER JOIN 						RPT_PatientProgram AS pp11 with (nolock)  ON pt11.PatientId = pp11.PatientId INNER JOIN 						RPT_PatientProgramModule AS pm11 with (nolock) ON pp11.PatientProgramId = pm11.PatientProgramId INNER JOIN 						RPT_PatientProgramAction AS pa11 with (nolock)  ON pm11.PatientProgramModuleId = pa11.PatientProgramModuleId INNER JOIN 						RPT_PatientProgramStep AS ps11 with (nolock) ON pa11.ActionId = ps11.ActionId LEFT OUTER JOIN 						RPT_PatientProgramResponse AS pr11 with (nolock) ON ps11.StepId = pr11.StepId 					WHERE   						pp11.SourceId = '541943a6bdd4dfa5d90002da'    			 						AND ps11.Question = 'Date did not enroll:'  						AND pa11.Name = 'Enrollment'  						AND pp11.Name = 'BSHSI - Healthy Weight v2' 						AND (pp11.[Delete] = 'False') 						AND pa11.[State] IN ('Completed')	 						AND pt11.PatientId = pt10.PatientId 						AND pp11.patientprogramid = ppt.patientprogramid 				)	 			ORDER BY pa10.ArchivedDate DESC)
					END AS [didnotenrollValue]
				FROM
						RPT_Patient AS pt9 WITH (NOLOCK) INNER JOIN
						RPT_PatientProgram AS pp9 WITH (NOLOCK) ON pt9.PatientId = pp9.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm9 WITH (NOLOCK) ON pp9.PatientProgramId = pm9.PatientProgramId INNER JOIN 					
						RPT_PatientProgramAction AS pa9 WITH (NOLOCK) ON pm9.PatientProgramModuleId = pa9.PatientProgramModuleId INNER JOIN 					
						RPT_PatientProgramStep AS ps9 WITH (NOLOCK) ON pa9.ActionId = ps9.ActionId LEFT OUTER JOIN 					
						RPT_PatientProgramResponse AS pr9 WITH (NOLOCK) ON ps9.StepId = pr9.StepId 				
					WHERE      					
						pp9.Name = 'BSHSI - Healthy Weight v2' 					
						AND ps9.Question = 'Date did not enroll:'  					
						AND pa9.Name = 'Enrollment'  					
						AND (pp9.[Delete] = 'False')  					
						--AND (pa9.Archived = 'False')  					
						AND pa9.ArchivedDate IS NULL 					
						AND pr9.Value != ''  					
						AND pa9.[State] IN ('Completed') 					
						AND pt9.patientId = pt.patientid 					
						AND pp9.patientprogramid = ppt.patientprogramid 						
				    ) as [did_not_enroll_date]
			,(
				(SELECT 						
					CASE WHEN pr12.[Selected] = 'True' AND pa12.[Archived] = 'False' THEN -- pa6.Archived = 'False' AND pr6.Selected = 'True' THEN
						CASE WHEN pr12.[Delete] = 'False' THEN
							pr12.[Text]
						ELSE
							NULL
						END							
					 ELSE
						(SELECT TOP 1
							CASE WHEN  pa13.Archived = 'True' AND pr13.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
								CASE WHEN pr13.[Delete] = 'False' THEN
									pr13.[Text]
								ELSE
									NULL
								END
							ELSE
								NULL
							END AS [didnotenrollTextarch]							 
						 FROM RPT_Patient AS pt13 with (nolock) INNER JOIN RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN 					RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN 					RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN 					RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId 				WHERE   					pp13.sourceid = '541943a6bdd4dfa5d90002da'    					AND ((ps13.SourceId = '53f4f885ac80d30e00000065') OR (ps13.SourceId = '532c3fc2f8efe368860003b5') ) 					AND ((pa13.SourceId = '53f4fd75ac80d30e00000083') OR (pa13.SourceId = '532c45bff8efe36886000446') ) 					AND (pp13.[Delete] = 'False')  					AND (pa13.Archived = 'True')  					AND (pr13.Selected = 'True')  					AND pa13.ArchivedDate IS NOT NULL 					AND pa13.[State] IN ('Completed') 					AND pt13.PatientId = pt.patientId 					AND pp13.patientprogramid = ppt.patientprogramid 					AND pa13.ActionId IN ( 							SELECT  								DISTINCT pa14.ActionId 							FROM          								RPT_Patient AS pt14 with (nolock) INNER JOIN 								RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN 								RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN 								RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN 								RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN 								RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId 							WHERE   								pp14.sourceid = '541943a6bdd4dfa5d90002da'    								AND ((ps14.SourceId = '53f4f885ac80d30e00000065') OR (ps14.SourceId = '532c3fc2f8efe368860003b5') ) 								AND ((pa14.SourceId = '53f4fd75ac80d30e00000083') OR (pa14.SourceId = '532c45bff8efe36886000446') ) 								AND (pp14.[Delete] = 'False')  								AND (pr14.Selected = 'True')  								AND pt14.PatientId = pt13.patientId 								AND pa14.[State] IN ('Completed') 								AND pp14.patientprogramid = ppt.patientprogramid	 					)	 				ORDER BY pa13.ArchivedDate DESC) 	
					 END AS [didnotenrollreasonText]
				 FROM
						RPT_Patient AS pt12 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp12 with (nolock) ON pt12.PatientId = pp12.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm12 with (nolock) ON pp12.PatientProgramId = pm12.PatientProgramId INNER JOIN 					
						RPT_PatientProgramAction AS pa12 with (nolock) ON pm12.PatientProgramModuleId = pa12.PatientProgramModuleId INNER JOIN 					
						RPT_PatientProgramStep AS ps12 with (nolock) ON pa12.ActionId = ps12.ActionId LEFT OUTER JOIN 					
						RPT_PatientProgramResponse AS pr12 with (nolock) ON ps12.StepId = pr12.StepId 				
					WHERE   					
						pp12.sourceid = '541943a6bdd4dfa5d90002da'    					
						AND ((ps12.SourceId = '53f4f885ac80d30e00000065') OR (ps12.SourceId = '532c3fc2f8efe368860003b5') ) 					
						AND ((pa12.SourceId = '53f4fd75ac80d30e00000083') OR (pa12.SourceId = '532c45bff8efe36886000446') ) 					
						AND (pp12.[Delete] = 'False')  					
						--AND (pa12.Archived = 'False')  					
						AND (pr12.Selected = 'True')  					
						AND pa12.ArchivedDate IS NULL 					
						AND pt12.patientid = pt.patientid 					
						AND pa12.[State] IN ('Completed') 					
						AND pp12.patientprogramid = ppt.patientprogramid)
			 ) as [did_not_enroll_reason]
		FROM
			RPT_Patient as pt with (nolock) 	
			INNER JOIN RPT_PatientProgram as ppt with (nolock) ON pt.PatientId = ppt.PatientId  	
			INNER JOIN RPT_PatientProgramModule AS pm with (nolock) ON ppt.PatientProgramId = pm.PatientProgramId  	
			INNER JOIN RPT_PatientProgramAction AS pa with (nolock)  ON pm.PatientProgramModuleId = pa.PatientProgramModuleId        	
			INNER JOIN RPT_PatientProgramAttribute as ppa with (nolock) ON ppt.MongoId = ppa.MongoPlanElementId  
			LEFT JOIN RPT_PatientSystem as ps with (nolock) ON pt.PatientId = ps.PatientId   	 
		WHERE
		pt.[Delete] = 'False' 	
		AND ppt.SourceId = '541943a6bdd4dfa5d90002da'
END



GO
 

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveAllergyTypeLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@CodeSystem varchar(100),
	@Code varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_AllergyTypeLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_AllergyTypeLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			CodeSystem = @CodeSystem,
			Code = @Code
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_AllergyTypeLookUp
			(LookUpType, 
			Name, 
			CodeSystem, 
			Code, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@CodeSystem, 
			@Code, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveAllergySourceLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@Active varchar(50),
	@Default varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_AllergySourceLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_AllergySourceLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			[Active] = @Active,
			[Default] = @Default
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_AllergySourceLookUp
			(LookUpType, 
			Name, 
			[Active], 
			[Default], 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@Active, 
			@Default, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveSeverityLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@CodeSystem varchar(100),
	@Code varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_SeverityLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_SeverityLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			CodeSystem = @CodeSystem,
			Code = @Code
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_SeverityLookUp
			(LookUpType, 
			Name, 
			CodeSystem, 
			Code, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@CodeSystem, 
			@Code, 
			@MongoID)
	End
END

GO 

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveReactionLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300),
	@CodeSystem varchar(100),
	@Code varchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_ReactionLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_ReactionLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name,
			CodeSystem = @CodeSystem,
			Code = @Code
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_ReactionLookUp
			(LookUpType, 
			Name, 
			CodeSystem, 
			Code, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@CodeSystem, 
			@Code, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveMedSupTypeLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_MedSupTypeLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_MedSupTypeLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_MedSupTypeLookUp
			(LookUpType, 
			Name, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveFreqHowOftenLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_FreqHowOftenLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_FreqHowOftenLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_FreqHowOftenLookUp
			(LookUpType, 
			Name, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveFreqWhenLookUp] 
	@MongoID varchar(50),
	@LookUpType varchar(200),
	@Name varchar(300)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	If Exists(Select Top 1 1 From RPT_FreqWhenLookUp Where MongoId = @MongoID)
	Begin
		Update RPT_FreqWhenLookUp
		Set 
			LookUpType = @LookUpType,
			Name = @Name
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_FreqWhenLookUp
			(LookUpType, 
			Name, 
			MongoId) 
		values 
			(@LookUpType, 
			@Name, 
			@MongoID)
	End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveAllergy] 
	@MongoId [varchar](50),
	@Name [varchar](50),
	@CodingSystem [varchar](50),
	@CodingSystemCode [varchar](50),
	@Version [float],	
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@TTLDate [datetime],
	@Delete [varchar](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@RecordCreatedById INT,
			@UpdatedById INT
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_Allergy Where MongoId = @MongoID)
		Begin
			Update RPT_Allergy
			Set 
				Name					= @Name,					
				CodingSystem			= @CodingSystem,			
				CodingSystemCode		= @CodingSystemCode,		
				[Version]				= @Version,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedById				= @UpdatedById,
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,	
				RecordCreatedById       = @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,		
				TTLDate					= @TTLDate,				
				[Delete]				= @Delete					
			Where MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_Allergy
			(
				MongoId,
				Name,
				CodingSystem,			
				CodingSystemCode,		
				[Version],				
				MongoUpdatedBy,			
				UpdatedById,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedById,       
				RecordCreatedOn,			
				TTLDate,					
				[Delete]								
			) 
			values 
			(
				@MongoId,
				@Name,				
				@CodingSystem,		
				@CodingSystemCode,	
				@Version,				
				@MongoUpdatedBy,		
				@UpdatedById,
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,		
				@TTLDate,				
				@Delete								
			)
		End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SaveAllergyType]
	@MongoAllergyId			[varchar](50),
	@MongoTypeId			[varchar](50),	
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version			 	[float]
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@RecordCreatedById INT,
			@UpdatedById INT,
			@AllergyId INT
	
	if @MongoAllergyId != ' '
		Select @AllergyId = AllergyId From [RPT_Allergy] Where MongoId = @MongoAllergyId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	--If Exists(Select Top 1 1 From RPT_AllergyType Where MongoAllergyId = @MongoAllergyId)
	--	Begin
	--		Update RPT_AllergyType
	--		Set 
	--			AllergyId				= @AllergyId,
	--			MongoAllergyId			= @MongoAllergyId,
	--			MongoTypeId				= @MongoTypeId,
	--			MongoUpdatedBy			= @MongoUpdatedBy,
	--			UpdatedById				= @UpdatedById,
	--			LastUpdatedOn			= @LastUpdatedOn,		
	--			MongoRecordCreatedBy	= @MongoRecordCreatedBy,
	--			RecordCreatedById		= @RecordCreatedById,
	--			RecordCreatedOn			= @RecordCreatedOn,	
	--			[Version]				= @Version
	--		Where MongoAllergyId = @MongoAllergyId
	--	End
	--Else
	--	Begin
			Insert Into RPT_AllergyType
			(
				AllergyId,
				MongoAllergyId,					
				MongoTypeId,		
				MongoUpdatedBy,		
				LastUpdatedOn,		
				MongoRecordCreatedBy,
				RecordCreatedOn,	
				[Version]			 												
			) 
			values 
			(
				@AllergyId,
				@MongoAllergyId,					
				@MongoTypeId,		
				@MongoUpdatedBy,		
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedOn,	
				@Version			
			)
		--End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientAllergy] 
	@MongoId [varchar](50),
	@MongoAllergyId [varchar](50),
	@MongoPatientId [varchar](50),
	@MongoSeverityId [varchar](50),
	@StatusId [varchar](100),
	@SourceId [varchar](50),
	@StartDate [datetime],
	@EndDate [datetime],
	@Notes [varchar](5000),
	@SystemName [varchar](50),
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@Version [float],
	@TTLDate [datetime],
	@Delete [varchar](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@AllergyId INT,
			@SeverityId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	Select @AllergyId = AllergyId From RPT_Allergy Where MongoId = @MongoAllergyId
	
	Select @SeverityId = SeverityId from RPT_SeverityLookUp Where MongoId = @MongoSeverityId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientAllergy Where MongoId = @MongoID)
		Begin
			Update RPT_PatientAllergy
			Set 
				MongoId					= @MongoId,				
				AllergyId				= @AllergyId,
				MongoAllergyId			= @MongoAllergyId,			
				PatientId				= @PatientId,
				MongoPatientId			= @MongoPatientId,			
				SeverityId				= @SeverityId,
				MongoSeverityId			= @MongoSeverityId,		
				StatusId				= @StatusId,				
				SourceId				= @SourceId,				
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Notes					= @Notes,					
				SystemName				= @SystemName,
				UpdatedBy				= @UpdatedById,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				LastUpdatedOn			= @LastUpdatedOn,			
				RecordCreatedBy			= @RecordCreatedById,
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,	
				RecordCreatedOn			= @RecordCreatedOn,		
				[Version]				= @Version,				
				TTLDate					= @TTLDate,				
				[Delete]				= @Delete					
			Where MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientAllergy
			(
				MongoId,				
				AllergyId,				
				MongoAllergyId,			
				PatientId,			
				MongoPatientId,			
				SeverityId,				
				MongoSeverityId,			
				StatusId,				
				SourceId,				
				StartDate,				
				EndDate,					
				Notes,					
				SystemName,				
				UpdatedBy,				
				MongoUpdatedBy,			
				LastUpdatedOn,			
				RecordCreatedBy,		
				MongoRecordCreatedBy,	
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]
			) 
			values 
			(
				@MongoId,				
				@AllergyId,
				@MongoAllergyId,		
				@PatientId,
				@MongoPatientId,		
				@SeverityId,
				@MongoSeverityId,		
				@StatusId,			
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Notes,				
				@SystemName,
				@UpdatedById,			
				@MongoUpdatedBy,		
				@LastUpdatedOn,		
				@RecordCreatedById,
				@MongoRecordCreatedBy,
				@RecordCreatedOn,		
				@Version,				
				@TTLDate,				
				@Delete
			)
		End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientAllergyReaction]
	@MongoPatientAllergyId [varchar](50),
	@MongoReactionId [varchar](50),	
	@MongoUpdatedBy [varchar](50),
	@LastUpdatedOn [datetime],
	@MongoRecordCreatedBy [varchar](50),
	@RecordCreatedOn [datetime],
	@Version [float]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare 
			@PatientAllergyId INT,
			@ReactionId INT,
			@RecordCreatedById INT,
			@UpdatedById INT
	
	Select @PatientAllergyId = PatientAllergyId From [RPT_PatientAllergy] Where MongoId = @MongoPatientAllergyId
	
	Select @ReactionId = ReactionId From [RPT_ReactionLookUp] Where MongoId = @MongoReactionId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	--If Exists(Select Top 1 1 From RPT_PatientAllergyReaction Where ReactionId = @ReactionId)
	--	Begin
	--		Update RPT_PatientAllergyReaction
	--		Set 
	--			PatientAllergyId		= @PatientAllergyId,
 --				MongoPatientAllergyId	= @MongoPatientAllergyId,
 --				MongoReactionId			= @MongoReactionId,	
 --				ReactionId				= @ReactionId,	
 --				UpdatedById				= @UpdatedById,
 --				MongoUpdatedBy			= @MongoUpdatedBy,
 --				LastUpdatedOn			= @LastUpdatedOn,
 --				RecordCreatedById		= @RecordCreatedById,
 --				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
 --				RecordCreatedOn			= @RecordCreatedOn,
 --				[Version]				= @Version
	--		Where MongoPatientAllergyId = @MongoPatientAllergyId
	--	End
	--Else
	--	Begin
			Insert Into RPT_PatientAllergyReaction
			(
				PatientAllergyId,
				MongoPatientAllergyId,	
				MongoReactionId,			
				ReactionId,				
				UpdatedById,			
				MongoUpdatedBy,			
				LastUpdatedOn,			
				RecordCreatedById,		
				MongoRecordCreatedBy,	
				RecordCreatedOn,
				[Version]								
			) 
			values 
			(
				@PatientAllergyId,
				@MongoPatientAllergyId,
				@MongoReactionId,	
				@ReactionId,	
				@UpdatedById,
				@MongoUpdatedBy,
				@LastUpdatedOn,
				@RecordCreatedById,
				@MongoRecordCreatedBy,
				@RecordCreatedOn,
				@Version
			)
		--End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_SavePatientMedSupp] 
	@MongoId				[varchar](50),
	@MongoPatientId			[varchar](50),
	@MongoFamilyId			[VARCHAR](50),
	@Name					[varchar] (200),
	@Category				[varchar] (200),
	@MongoTypeId			[varchar] (50),
	@Status					[varchar] (200),
	@Dosage					[varchar] (500),
	@Strength				[varchar] (200),
	@Route					[varchar] (200),
	@Form					[varchar] (200),
	@FreqQuantity			[varchar] (200),
	@MongoFreqHowOftenId	[varchar] (50),
	@MongoFreqWhenId		[varchar] (50),
	@MongoSourceId			[varchar](50),	
	@StartDate				[datetime],
	@EndDate				[datetime],
	@Reason					[varchar](5000),
	@Notes					[varchar](5000),	
	@PrescribedBy			[varchar](500),	
	@SystemName				[varchar](50),
	@MongoUpdatedBy			[varchar](50),
	@LastUpdatedOn			[datetime],
	@MongoRecordCreatedBy	[varchar](50),
	@RecordCreatedOn		[datetime],
	@Version				[float],
	@TTLDate				[DATETIME],
	@Delete					[VARCHAR](50)
AS	
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientId INT,
			@FreqHowOftenId INT,
			@FreqWhenId INT,			
			@RecordCreatedById INT,
			@UpdatedById INT,
			@MedSuppTypeId INT,
			@SourceId INT
	
	select @FreqHowOftenId = FreqId from RPT_FreqHowOftenLookUp where MongoId = @MongoFreqHowOftenId
	select @FreqWhenId = FreqWhenId from RPT_FreqWhenLookUp where MongoId = @MongoFreqWhenId
	select @MedSuppTypeId = mst.MedSupId  from RPT_MedSupTypeLookUp as mst where MongoId = @MongoTypeId
	select @SourceId = AllergySourceId from RPT_AllergySourceLookUp where MongoId = @MongoSourceId
	
	Select @PatientId = PatientId from RPT_Patient Where MongoId = @MongoPatientId
	
	If @MongoRecordCreatedBy != ' '
		Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @MongoRecordCreatedBy
	
	If @MongoUpdatedBy != ' '
		Select @UpdatedById = UserId From [RPT_User] Where MongoId = @MongoUpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientMedSupp Where MongoId = @MongoID)
		Begin
			Update RPT_PatientMedSupp
			Set 
				MongoId					= @MongoId,				
				MongoPatientId			= @MongoPatientId,
				MongoFamilyId			= @MongoFamilyId,
				PatientId				= @PatientId,			
				Name					= @Name,					
				Category				= @Category,				
				MongoTypeId				= @MongoTypeId,
				TypeId					= @MedSuppTypeId,			
				[Status]				= @Status,					
				Dosage					= @Dosage,					
				Strength				= @Strength,				
				[Route]					= @Route,					
				Form					= @Form,					
				FreqQuantity			= @FreqQuantity,			
				MongoFreqHowOftenId		= @MongoFreqHowOftenId,	
				FreqHowOftenId			= @FreqHowOftenId,	
				MongoFreqWhenId			= @MongoFreqWhenId,	
				FreqWhenId				= @FreqWhenId,		
				MongoSourceId			= @MongoSourceId,			
				SourceId				= @SourceId,			
				StartDate				= @StartDate,				
				EndDate					= @EndDate,				
				Reason					= @Reason,					
				Notes					= @Notes,					
				PrescribedBy			= @PrescribedBy,			
				SystemName				= @SystemName,				
				MongoUpdatedBy			= @MongoUpdatedBy,			
				UpdatedBy				= @UpdatedById,				
				LastUpdatedOn			= @LastUpdatedOn,			
				MongoRecordCreatedBy	= @MongoRecordCreatedBy,
				RecordCreatedBy			= @RecordCreatedById,
				RecordCreatedOn			= @RecordCreatedOn,
				[Version]				= @Version,
				TTLDate					= @TTLDate,
				[Delete]				= @Delete
			Where 
				MongoId = @MongoID
		End
	Else
		Begin
			Insert Into RPT_PatientMedSupp
			(
				MongoId,
				MongoPatientId,
				MongoFamilyId,
				PatientId,
				Name,
				Category,
				MongoTypeId,
				TypeId,
				[Status],
				Dosage,
				Strength,
				[Route],
				Form,
				FreqQuantity,			
				MongoFreqHowOftenId,		
				FreqHowOftenId,			
				MongoFreqWhenId,			
				FreqWhenId,				
				MongoSourceId,			
				SourceId,				
				StartDate,				
				EndDate,					
				Reason,					
				Notes,					
				PrescribedBy,			
				SystemName,				
				MongoUpdatedBy,			
				UpdatedBy,				
				LastUpdatedOn,			
				MongoRecordCreatedBy,	
				RecordCreatedBy,			
				RecordCreatedOn,			
				[Version],				
				TTLDate,					
				[Delete]						
			) 
			values 
			(
				@MongoId,				
				@MongoPatientId,
				@MongoFamilyId,
				@PatientId,			
				@Name,				
				@Category,			
				@MongoTypeId,
				@MedSuppTypeId,		
				@Status,				
				@Dosage,				
				@Strength,			
				@Route,				
				@Form,				
				@FreqQuantity,		
				@MongoFreqHowOftenId,	
				@FreqHowOftenId,	
				@MongoFreqWhenId,	
				@FreqWhenId,		
				@MongoSourceId,		
				@SourceId,			
				@StartDate,			
				@EndDate,				
				@Reason,				
				@Notes,				
				@PrescribedBy,		
				@SystemName,			
				@MongoUpdatedBy,		
				@UpdatedById,			
				@LastUpdatedOn,		
				@MongoRecordCreatedBy,
				@RecordCreatedById,
				@RecordCreatedOn,
				@Version,
				@TTLDate,
				@Delete
			)
		End
END

GO

CREATE PROCEDURE [dbo].[spPhy_RPT_PatientMedSuppNDC]
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


CREATE PROCEDURE [dbo].[spPhy_RPT_PatientMedSuppPhClass]
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

ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientIntervention] 
	@MongoID varchar(50),
	@PatientGoalMongoId varchar(50),
	@MongoCategoryLookUpId varchar(50),
	@AssignedTo varchar(50),
	@Delete varchar(50),
	@Description varchar(5000),
	@LastUpdatedOn datetime,
	@RecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TimeToLive datetime,
	@UpdatedBy varchar(50),
	@Version float,
	@Name varchar(100),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@CategoryLookUpId INT,
			@RecordCreatedById INT,
			@UpdatedById INT,
			@UserId INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @PatientGoalMongoId
	
	Select @CategoryLookUpId = InterventionCategoryId From RPT_InterventionCategoryLookUp Where MongoId = @MongoCategoryLookUpId
	Select @UserId = UserId From [RPT_User] Where MongoId = @AssignedTo
	
	If @RecordCreatedBy != ' '
	Select @RecordCreatedById = UserId From [RPT_User] Where MongoId = @RecordCreatedBy
	
	If @UpdatedBy != ' '
	Select @UpdatedById = UserId From [RPT_User] Where MongoId = @UpdatedBy
	
	If Exists(Select Top 1 1 From RPT_PatientIntervention Where MongoId = @MongoID)
	Begin
		Update RPT_PatientIntervention
		Set AssignedToUserId = @UserId,
			MongoContactUserId = @AssignedTo,
			CategoryLookUpId = @CategoryLookUpId,
			MongoCategoryLookUpId = @MongoCategoryLookUpId,
			[Delete] = @Delete,
			[Description] = @Description,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @PatientGoalMongoId,
			MongoRecordCreatedBy = @RecordCreatedBy,
			RecordCreatedById = @RecordCreatedById,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TTLDate = @TimeToLive,
			MongoUpdatedBy = @UpdatedBy,
			UpdatedById = @UpdatedById,
			[Version] = @Version,
			Name = @Name,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId
		Where MongoId = @MongoID
		
	End
	Else
	Begin
		Insert Into RPT_PatientIntervention(
		Name, 
		ExtraElements, 
		AssignedToUserId, 
		MongoContactUserId, 
		CategoryLookUpId, 
		MongoCategoryLookUpId, 
		[Delete], 
		[Description], 
		LastUpdatedOn, 
		PatientGoalId, 
		MongoPatientGoalId, 
		MongoRecordCreatedBy, 
		RecordCreatedById, 
		RecordCreatedOn, 
		StartDate, 
		[Status], 
		StatusDate, 
		TTLDate, 
		MongoUpdatedBy, 
		UpdatedById, 
		[Version], 
		MongoId, 
		[ClosedDate], 
		[TemplateId]) 
		values 
		(@Name, 
		@ExtraElements, 
		@UserId, 
		@AssignedTo, 
		@CategoryLookUpId, 
		@MongoCategoryLookUpId, 
		@Delete, 
		@Description, 
		@LastUpdatedOn, 
		@PatientGoalId, 
		@PatientGoalMongoId, 
		@RecordCreatedBy, 
		@RecordCreatedById, 
		@RecordCreatedOn, 
		@StartDate, 
		@Status, 
		@StatusDate, 
		@TimeToLive, 
		@UpdatedBy, 
		@UpdatedById, 
		@Version, 
		@MongoID,
		@ClosedDate,
		@TemplateId)
	End
END

GO

ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientGoal]
	@MongoId varchar(50),
	@MongoPatientId varchar(50),
	@Name varchar(500),
	@Description varchar(50),
	@StartDate datetime,
	@EndDate datetime,
	@Source varchar(50),
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(500),
	@Type varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@Delete varchar(50),
	@TimeToLive datetime,
	@ExtraElements varchar(5000),
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @PatientId INT,
			@UpdatedBy INT,
			@RecordCreatedBy INT
	
	Select @PatientId = PatientId From RPT_Patient Where MongoId = @MongoPatientId

		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end
	
	-- Insert statements for procedure here
	If Exists(Select Top 1 1 From RPT_PatientGoal Where MongoId = @MongoId)
	Begin
		Update RPT_PatientGoal
		Set Name = @Name,
			StartDate = @StartDate,
			EndDate = @EndDate,
			[Source] = @Source,
			[Status] = @Status,
			[Description] = @Description,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			[Type] = @Type,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			LastUpdatedOn = @LastUpdatedOn,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			[Version] = @Version,
			[Delete] = @Delete,
			TTLDate = @TimeToLive,
			PatientId = @PatientId,
			MongoPatientId = @MongoPatientId,
			ExtraElements = @ExtraElements,
			TemplateId = @TemplateId
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert into RPT_PatientGoal(
			Name, 
			StartDate, 
			EndDate, 
			[Source], 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue, 
			[Type], 
			UpdatedBy,
			MongoUpdatedBy, 
			LastUpdatedOn, 
			RecordCreatedBy, 
			MongoRecordCreatedBy,
			RecordCreatedOn, 
			[Version], 
			[Delete], 
			TTLDate,
			 MongoId, 
			 PatientId,
			 MongoPatientId,
			 [Description],
			 ExtraElements,
			 TemplateId
			 ) 
		 values (
			 @Name, 
			 @StartDate, 
			 @EndDate, 
			 @Source, 
			 @Status, 
			 @StatusDate,
			 @TargetDate, 
			 @TargetValue, 
			 @Type, 
			 @UpdatedBy,
			 @MongoUpdatedBy, 
			 @LastUpdatedOn, 
			 @RecordCreatedBy,
			 @MongoRecordCreatedBy, 
			 @RecordCreatedOn, 
			 @Version, 
			 @Delete, 
			 @TimeToLive, 
			 @MongoId, 
			 @PatientId,
			 @MongoPatientId,
			 @Description,
			 @ExtraElements,
			 @TemplateId
		 )
	End
END

GO

ALTER PROCEDURE [dbo].[spPhy_RPT_SavePatientTask] 
	@MongoId varchar(50),
	@MongoPatientGoalId varchar(50),
	@Name varchar(100),
	@Description varchar(5000),
	@StartDate datetime,
	@Status varchar(50),
	@StatusDate datetime,
	@TargetDate datetime,
	@TargetValue varchar(50),
	@MongoUpdatedBy varchar(50),
	@LastUpdatedOn datetime,
	@MongoRecordCreatedBy varchar(50),
	@RecordCreatedOn datetime,
	@Version float,
	@TimeToLive datetime,
	@Delete varchar(50),
	@ExtraElements varchar(5000),
	@ClosedDate datetime,
	@TemplateId varchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	Declare @PatientGoalId INT,
			@UpdatedBy		INT,
			@RecordCreatedBy INT
	
	Select @PatientGoalId = PatientGoalId From RPT_PatientGoal Where MongoId = @MongoPatientGoalId
	
		-- find record created by Id
	if @MongoRecordCreatedBy != ''
		begin
			set @RecordCreatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoRecordCreatedBy);
		end

	-- find record created by Id
	if @MongoUpdatedBy != ''
		begin
			set @UpdatedBy = (select dbo.[RPT_User].UserId from dbo.[RPT_User] where MongoId = @MongoUpdatedBy);
		end	
	
	If Exists(Select Top 1 1 From RPT_PatientTask Where MongoId = @MongoId)
	Begin
		Update RPT_PatientTask
		Set Name = @Name,
			Description = @Description,
			[Delete] = @Delete,
			LastUpdatedOn = @LastUpdatedOn,
			PatientGoalId = @PatientGoalId,
			MongoPatientGoalId = @MongoPatientGoalId,
			RecordCreatedBy = @RecordCreatedBy,
			MongoRecordCreatedBy = @MongoRecordCreatedBy,
			RecordCreatedOn = @RecordCreatedOn,
			StartDate = @StartDate,
			[Status] = @Status,
			StatusDate = @StatusDate,
			TargetDate = @TargetDate,
			TargetValue = @TargetValue,
			TTLDate = @TimeToLive,
			UpdatedBy = @UpdatedBy,
			MongoUpdatedBy = @MongoUpdatedBy,
			[Version] = @Version,
			ExtraElements = @ExtraElements,
			ClosedDate = @ClosedDate,
			TemplateId = @TemplateId
		Where MongoId = @MongoId
		
	End
	Else
	Begin
		Insert Into 
			RPT_PatientTask
			(
			Name, 
			[Description], 
			[Delete], 
			LastUpdatedOn, 
			PatientGoalId, 
			MongoPatientGoalId, 
			RecordCreatedBy,
			MongoRecordCreatedBy, 
			RecordCreatedOn, 
			StartDate, 
			[Status], 
			StatusDate, 
			TargetDate, 
			TargetValue,
			TTLDate, 
			UpdatedBy, 
			MongoUpdatedBy, 
			[Version], 
			ExtraElements,
			MongoId,
			ClosedDate,
			TemplateId
			) 
			values 
			(
			@Name, 
			@Description, 
			@Delete, 
			@LastUpdatedOn, 
			@PatientGoalId, 
			@MongoPatientGoalId, 
			@RecordCreatedBy,
			@MongoRecordCreatedBy, 
			@RecordCreatedOn, 
			@StartDate, 
			@Status, 
			@StatusDate, 
			@TargetDate,
			@TargetValue, 
			@TimeToLive, 
			@UpdatedBy, 
			@MongoUpdatedBy, 
			@Version, 
			@ExtraElements,
			@MongoId,
			@ClosedDate,
			@TemplateId
			)
	End
END
GO 

ALTER PROCEDURE [dbo].[spPhy_RPT_TruncateTables]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE RPT_CareMember
	DELETE RPT_CareMemberTypeLookUp
	DELETE RPT_ContactEmail
	DELETE RPT_ContactPhone
	DELETE RPT_ContactAddress
	DELETE RPT_ContactRecentList
	DELETE RPT_ContactMode
	DELETE RPT_ContactLanguage
	DELETE RPT_ContactWeekDay
	DELETE RPT_ContactTimeOfDay
	-- todos
	DELETE RPT_ToDoProgram
	DELETE RPT_ToDo
	-- patient programs
	DELETE RPT_SpawnElements
	DELETE RPT_SpawnElementTypeCode
	DELETE RPT_PatientProgramAttribute
	DELETE RPT_PatientProgramResponse
	DELETE RPT_PatientProgramStep
	DELETE RPT_PatientProgramAction	
	DELETE RPT_PatientProgramModule
	DELETE RPT_PatientProgram	
	DELETE RPT_PatientNoteProgram
	DELETE RPT_PatientNote
	DELETE RPT_PatientProblem
	DELETE RPT_ObjectiveCategory
	DELETE RPT_ObjectiveLookUp	
	DELETE RPT_PatientObservation	
	DELETE RPT_Observation
	DELETE RPT_PatientTaskAttributeValue
	DELETE RPT_PatientTaskAttribute
	DELETE RPT_PatientTaskBarrier
	DELETE RPT_PatientTask
	-- patient allergies
	DELETE RPT_Allergy
	DELETE RPT_AllergyType
	DELETE RPT_PatientAllergy
	DELETE RPT_PatientAllergyReaction
	-- patient medsupps
	DELETE RPT_PatientMedSuppPhClass
	DELETE RPT_PatientMedSuppNDC
	DELETE RPT_PatientMedSupp	
	-- patient goal
	DELETE RPT_PatientGoalProgram
	DELETE RPT_PatientGoalFocusArea
	DELETE RPT_GoalAttributeOption	
	DELETE RPT_PatientGoalAttributeValue
	DELETE RPT_PatientGoalAttribute
	DELETE RPT_GoalAttribute
	DELETE RPT_PatientInterventionBarrier
	DELETE RPT_PatientIntervention	
	DELETE RPT_PatientBarrier	
	DELETE RPT_PatientGoal
	DELETE RPT_PatientUser	
	DELETE RPT_Contact
	DELETE RPT_PatientSystem
	DELETE RPT_Patient
	DELETE RPT_CommTypeCommMode
	DELETE RPT_ToDoCategoryLookUp	
	DELETE RPTMongoCategoryLookUp
	DELETE RPT_SourceLookUp
	DELETE RPT_BarrierCategoryLookUp
	DELETE RPT_InterventionCategoryLookUp
	DELETE RPTMongoTimeZoneLookUp
	DELETE RPT_ProblemLookUp
	DELETE RPT_TimesOfDayLookUp
	DELETE RPT_CommTypeLookUp
	DELETE RPT_CommModeLookUp
	DELETE RPT_StateLookUp
	DELETE RPT_LanguageLookUp
	DELETE RPT_FocusAreaLookUp
	DELETE RPT_CodingSystemLookUp
	DELETE RPT_ObservationTypeLookUp
	DELETE RPT_AllergyTypeLookUp
	DELETE RPT_AllergySourceLookUp
	DELETE RPT_SeverityLookUp
	DELETE RPT_ReactionLookUp
	DELETE RPT_MedSupTypeLookUp
	DELETE RPT_FreqHowOftenLookUp
	DELETE RPT_FreqWhenLookUp
	DELETE RPT_UserRecentList
	DELETE [RPT_User]
	
	--DELETE CohortPatientView	
	--DELETE CohortPatientViewSearchField
	
	DBCC CHECKIDENT ('RPT_CareMember', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_CareMemberTypeLookUp', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_ContactLanguage', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactWeekDay', RESEED, 0)  
	DBCC CHECKIDENT ('RPT_ContactTimeOfDay', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactRecentList', RESEED, 0) 
	DBCC CHECKIDENT ('RPT_ContactMode', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactPhone', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactAddress', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ContactEmail', RESEED, 0)
	
-- reseed program tables
	DBCC CHECKIDENT ('RPT_PatientProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SpawnElements', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramModule', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAction', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramStep', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramResponse', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProgramAttribute', RESEED, 0)	
	
	DBCC CHECKIDENT ('RPT_PatientNoteProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientNote', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientProblem', RESEED, 0)

	-- allergies
	DBCC CHECKIDENT ('RPT_AllergyType', RESEED, 0)	
	DBCC CHECKIDENT ('RPT_Allergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppPhClass', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSuppNDC', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientMedSupp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergyTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_AllergySourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SeverityLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ReactionLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_MedSupTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqHowOftenLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FreqWhenLookUp', RESEED, 0)

	DBCC CHECKIDENT ('RPT_ObjectiveCategory', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObjectiveLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientObservation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Observation', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTaskBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientTask', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientGoalProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalFocusArea', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttributeOption', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttributeValue', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoalAttribute', RESEED, 0)
	DBCC CHECKIDENT ('RPT_GoalAttribute', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientInterventionBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientIntervention', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientBarrier', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientGoal', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientUser', RESEED, 0)
	DBCC CHECKIDENT ('RPT_Contact', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_PatientSystem', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_Patient', RESEED, 0) 
	
	DBCC CHECKIDENT ('RPT_CommTypeCommMode', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_SourceLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_BarrierCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_InterventionCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPTMongoTimeZoneLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ProblemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_TimesOfDayLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommTypeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CommModeLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_StateLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_LanguageLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_FocusAreaLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_CodingSystemLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ObservationTypeLookUp', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_UserRecentList', RESEED, 0)
	DBCC CHECKIDENT ('RPT_User', RESEED, 0)
	
	DBCC CHECKIDENT ('RPT_ToDoCategoryLookUp', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDoProgram', RESEED, 0)
	DBCC CHECKIDENT ('RPT_ToDo', RESEED, 0)
	
	-- patient allergies
	DBCC CHECKIDENT ('RPT_PatientAllergy', RESEED, 0)
	DBCC CHECKIDENT ('RPT_PatientAllergyReaction', RESEED, 0)
	
	--DBCC CHECKIDENT ('RPT_CohortPatientView', RESEED, 0)
	--DBCC CHECKIDENT ('RPT_CohortPatientViewSearchField', RESEED, 0)
END
GO

/****** Object:  Table [dbo].[RPT_SpawnElementTypeCode]    Script Date: 11/25/2014 23:42:26 ******/
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (1, N'Program')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (2, N'Module')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (3, N'Action')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (4, N'Step')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (10, N'Eligibility')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (11, N'IneligibleReason')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (12, N'ProgramState')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (13, N'ProgramStartdate')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (14, N'ProgramEnddate')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (15, N'EnrollmentStatus')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (16, N'OptOut')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (19, N'Graduated')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (20, N'Locked')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (101, N'Problem')
INSERT [dbo].[RPT_SpawnElementTypeCode] ([TypeId], [Code]) VALUES (111, N'ToDo')

GO