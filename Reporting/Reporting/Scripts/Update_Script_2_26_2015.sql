IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_Flat_BSHSI_HW2]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[spPhy_RPT_ProgramResponse_Flat]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Did_Not_Enroll_Reason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Did_Not_Enroll_Reason]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DidNotEnrollReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_DidNotEnrollReason]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_DisEnrollmentReason]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_DisEnrollmentReason]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Enrollment]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Enrollment]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetDate]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetRecentActionCompletedDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetRecentActionCompletedDate]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_Market]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_Market]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPhone1]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetPhone1]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_GetPhone2]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_GetPhone2]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_RecentActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_RecentActionCompleted_Value]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Market]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Market]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Text]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompleted_Value]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionCompletedDate]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionCompletedDate]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Market]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Market]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Text]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionNotCompleted_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Market]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Market]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Text]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Text]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[fn_RPT_ActionSaved_Value]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[fn_RPT_ActionSaved_Value]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_BSHSI_HW2_Enrollment_Info]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RPT_ProgramResponse_Flat]') AND type in (N'U'))
DROP TABLE [dbo].[RPT_ProgramResponse_Flat]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RPT_ProgramResponse_Flat](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[ProgramSourceId] [varchar](50) NULL,
	[ActionSourceId] [varchar](50) NULL,
	[ActionArchived] [varchar](50) NULL,
	[ActionArchivedDate] [datetime] NULL,
	[StepSourceId] [varchar](50) NULL,
	[Text] [varchar](max) NULL,
	[Value] [varchar](max) NULL,
	[Selected] [varchar](50) NULL,
	[Delete] [varchar](50) NULL,
	[ActionCompleted] [varchar](50) NULL,
	[StepCompleted] [varchar](50) NULL,
	[DateCompleted] [datetime] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[RPT_BSHSI_HW2_Enrollment_Info](
	[PatientId] [int] NOT NULL,
	[PatientProgramId] [int] NOT NULL,
	[Priority] [varchar](50) NULL,
	[firstName] [varchar](100) NULL,
	[SystemId] [varchar](50) NULL,
	[LastName] [varchar](100) NULL,
	[MiddleName] [varchar](100) NULL,
	[Suffix] [varchar](50) NULL,
	[Gender] [varchar](50) NULL,
	[DateOfBirth] [varchar](50) NULL,
	[LSSN] [int] NULL,
	[Assigned_PCM] [varchar](100) NULL,
	[Program_CM] [varchar](100) NULL,
	[Enrollment] [varchar](50) NULL,
	[GraduatedFlag] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[Assigned_Date] [datetime] NULL,
	[Last_State_Update_Date] [datetime] NULL,
	[State] [varchar](50) NULL,
	[Eligibility] [varchar](50) NULL,
	[Program_Completed_Date] [date] NULL,
	[Re_enrollment_Date] [date] NULL,
	[Enrolled_Date] [date] NULL,
	[Pending_Enrolled_Date] [date] NULL,
	[Enrollment_Action_Completion_Date] [date] NULL,
	[Market] [varchar](max) NULL,
	[Disenroll_Date] [date] NULL,
	[Disenroll_Reason] [varchar](max) NULL,
	[did_not_enroll_date] [date] NULL,
	[did_not_enroll_reason] [varchar](max) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			CASE WHEN p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 THEN
				p.[Text]
			ELSE
				'0'
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			CASE WHEN p.[Selected] = 'True' THEN
				p.[Text]
			ELSE
				'0'
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND	p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionSaved_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			CASE WHEN p.[Selected] = 'True' THEN
				p.[Text]
			ELSE
				'0'
			END
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = '541943a6bdd4dfa5d90002da' 				
			AND ((p.StepSourceId = '53f4fb39ac80d30e00000067')  OR (p.StepSourceId = '532c3e76c347865db8000001') ) 				
			AND ((p.ActionSourceId = '53f4fd75ac80d30e00000083') OR (p.ActionSourceId = '532c45bff8efe36886000446') ) 					
			AND (p.ActionArchived = 'False') 				
			AND p.ActionCompleted = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @ValueTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ValueTable
		SELECT 
			'0' as [Value]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			--AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			'0' as [Market]
		FROM 
			RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = @ProgramSourceId 				
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'False'
			AND p.StepCompleted = 'False'
			AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionNotCompleted_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			'0' as [Market]
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = '541943a6bdd4dfa5d90002da' 				
			AND ((p.StepSourceId = '53f4fb39ac80d30e00000067')  OR (p.StepSourceId = '532c3e76c347865db8000001') ) 				
			AND ((p.ActionSourceId = '53f4fd75ac80d30e00000083') OR (p.ActionSourceId = '532c45bff8efe36886000446') ) 							
			AND (p.ActionArchived = 'False') 				
			AND (p.ActionCompleted = 'False')
			AND (p.StepCompleted = 'False')
			AND p.[Selected] = 'False'
			AND p.[Delete] = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionCompletedDate] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @ActionCompletedDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedDateTable
		SELECT
			CASE WHEN p.[DateCompleted] IS NOT NULL THEN
					p.[DateCompleted]--CONVERT(VARCHAR(2000),p.[DateCompleted],120)		
			ELSE 						
				NULL
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = '541943a6bdd4dfa5d90002da'
			AND p.StepSourceId = '53f4f920ac80d30e00000066'
			AND p.ActionSourceId = '53f4fd75ac80d30e00000083'
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Delete] = 'False' AND (p.[Value] IS NOT NULL OR LEN(p.[Value]) > 0 )THEN
					p.[Value]		
			ELSE 						
				'0'
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Text] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @ActionCompletedTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @ActionCompletedTable
		SELECT
			CASE WHEN p.[Selected] = 'True' AND p.[Delete] = 'False' THEN
					p.[Text]		
			ELSE 						
				'0'
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
				p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_ActionCompleted_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		INSERT INTO @MarketTable
		SELECT 
			CASE WHEN p.[Selected] = 'True' AND p.[Delete] = 'False' THEN
				p.[Text]
			ELSE
				'0'
			END 
		FROM RPT_ProgramResponse_Flat as p
		WHERE
			 p.ProgramSourceId = '541943a6bdd4dfa5d90002da' 				
			AND ((p.StepSourceId = '53f4fb39ac80d30e00000067')  OR (p.StepSourceId = '532c3e76c347865db8000001') ) 				
			AND ((p.ActionSourceId = '53f4fd75ac80d30e00000083') OR (p.ActionSourceId = '532c45bff8efe36886000446') ) 							
			AND (p.ActionArchived = 'False') 				
			AND (p.ActionCompleted = 'True')
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid		
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_RecentActionCompleted_Value] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @RecentActionCompletedTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		INSERT INTO @RecentActionCompletedTable
		SELECT
			CASE WHEN p.[DateCompleted] IS NOT NULL THEN
					p.[DateCompleted]--CONVERT(VARCHAR(2000),p.[DateCompleted],120)		
			ELSE 						
				NULL
			END
		FROM	
			RPT_ProgramResponse_Flat as p
		WHERE
			p.ProgramSourceId = @ProgramSourceId
			AND p.StepSourceId = @StepSourceId
			AND p.ActionSourceId = @ActionSourceId
			AND p.ActionArchived = 'False'
			AND p.ActionCompleted = 'True'
			AND p.PatientId = @patientid 				
			AND p.patientprogramid = @patientprogramid			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_GetPhone2] 
(	
	@contactid INT
)
RETURNS VARCHAR(50) 
AS
BEGIN
	DECLARE @secondaryPhone varchar(50);
	DECLARE @rowcount int;
	
	SET @rowcount = (SELECT COUNT(*) from RPT_ContactPhone cp where cp.ContactId = @contactid)
	
	IF @rowcount > 1
		BEGIN
			DECLARE @TempTable TABLE(PhoneId int, Number varchar(50), contactId int, preferred varchar(50))
			INSERT INTO @TempTable (PhoneId, Number, contactId, preferred) 
				SELECT cp.PhoneId, cp.Number, cp.ContactId, cp.PhonePreferred from RPT_ContactPhone cp where cp.ContactId = @contactid AND cp.OptOut != 'True';
					
			SET @secondaryPhone = (SELECT TOP 1 t.Number from ( SELECT TOP 2 PhoneId, Number, contactId FROM @TempTable ORDER BY PhoneId DESC) as t)
		END
	return @secondaryPhone;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_GetPhone1] 
(	
	@contactid INT
)
RETURNS VARCHAR(50) 
AS
BEGIN
	DECLARE @PreferredPhone varchar(50)

--	DECLARE @contactid int;
--	SET @contactid = 14;
--	SELECT cp.Number FROM RPT_ContactPhone cp WHERE cp.ContactId = @contactid --ORDER BY cp.PhonePreferred 
	set @PreferredPhone = (SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp WHERE cp.ContactId = @contactid AND cp.OptOut != 'True') -- AND cp.PhonePreferred = 'True'
--	select @PreferredPhone

	--IF @PreferredPhone IS NULL
	--	BEGIN
	--		SET @PreferredPhone = 
	--		(	SELECT TOP 1 cp.Number FROM RPT_ContactPhone cp 
	--			WHERE 
	--				cp.ContactId = @contactid
	--				AND cp.PhonePreferred = 'False'
	--				AND cp.OptOut = 'False'
	--			ORDER BY cp.PhoneId DESC)
	--	END

	RETURN @PreferredPhone
	
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_Market] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionCompleted_Market(@patientid, @patientprogramid));
		SET @SavedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionSaved_Market(@patientid, @patientprogramid));
		SET @ActionNotComplete = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionNotCompleted_Market(@patientid, @patientprogramid));
	
	--1) get completed market value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Market] FROM 	dbo.fn_RPT_ActionCompleted_Market(@patientid, @patientprogramid) where Market != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected market
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Market] FROM 	dbo.fn_RPT_ActionSaved_Market(@patientid, @patientprogramid) WHERE [Market] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
				SELECT TOP 1 				
						--CASE WHEN prxx.[Delete] = 'False' THEN
							prxx.[Text]
						--ELSE
							--NULL
						--END  
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
	
		Final:
			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_GetRecentActionCompletedDate] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_RecentActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_RecentActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where Value != '0'
			GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @CompletedCount = 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' OR pr13.[Delete] = 'True' THEN
						CASE WHEN pa13.[DateCompleted] IS NOT NULL AND LEN(pa13.[DateCompleted]) > 0 THEN
								pa13.[DateCompleted]
						ELSE 						
							NULL
						END					
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				--AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						--AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_GetDate] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @GetDateDateTable TABLE( Value VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @SavedCount = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Value]) FROM 	dbo.fn_RPT_ActionNotCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 [Value] FROM 	
				dbo.fn_RPT_ActionCompleted_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where Value != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1 NULL as [Value] FROM 	
				dbo.fn_RPT_ActionSaved_Value(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) WHERE [Value] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @GetDateDateTable WHERE Value IS NULL;
			INSERT INTO @GetDateDateTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' THEN
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Value]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = @ProgramSourceId
				AND ps13.SourceId = @StepSourceId
				AND pa13.SourceId = @ActionSourceId
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				--AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.PatientProgramId = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = @ProgramSourceId
						AND ps14.SourceId = @StepSourceId
						AND pa14.SourceId = @ActionSourceId
						AND (pp14.[Delete] = 'False')
						--AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pp14.PatientProgramId = @patientprogramid
						AND pa14.[State] IN ('Completed')
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_Enrollment] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @EnrollmentTable TABLE( Enrollment VARCHAR(2000))
AS
BEGIN
		INSERT INTO @EnrollmentTable
		select 
			ppa.Enrollment
		from 
			RPT_PatientProgram p,
			RPT_PatientProgramAttribute ppa,
			RPT_PatientProgramModule m,
			RPT_PatientProgramAction a,
			RPT_Patientprogramstep s,
			RPT_PatientProgramResponse r
		where 
			p.MongoId = ppa.MongoPlanElementId
			and p.MongoId = m.MongoProgramId
			and m.MongoId = a.MongoModuleId
			and a.mongoId = s.MongoActionId
			and s.MongoId = r.MongoStepId
			and p.PatientProgramId = @patientprogramid
			and p.SourceId = @ProgramSourceId
			and a.SourceId = @ActionSourceId
			and s.SourceId = @StepSourceId
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_DisEnrollmentReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @MarketTable TABLE( Market VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @SavedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 [Market] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where Market != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 NULL as [Market] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) WHERE [Market] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @MarketTable WHERE Market IS NULL;
			INSERT INTO @MarketTable
			SELECT TOP 1 				
				CASE WHEN  pa7.Archived = 'True' AND pr7.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
					CASE WHEN pr7.[Delete] = 'False' THEN
						pr7.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END  AS [DisenrollReasonText]	
			FROM 
				RPT_Patient AS pt7 with (nolock) INNER JOIN
				RPT_PatientProgram AS pp7 with (nolock) ON pt7.PatientId = pp7.PatientId INNER JOIN
				RPT_PatientProgramModule AS pm7 with (nolock) ON pp7.PatientProgramId = pm7.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa7 with (nolock) ON pm7.PatientProgramModuleId = pa7.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps7 with (nolock) ON pa7.ActionId = ps7.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr7 with (nolock) ON ps7.StepId = pr7.StepId
			WHERE
				((ps7.SourceId= '53f56f10ac80d31200000001') OR (ps7.SourceId= '532c407fc347865db8000003') )
				AND ((pa7.SourceId = '53f57115ac80d31203000014') OR (pa7.SourceId = '532c46b3c347865db8000092'))
				AND pp7.Name = 'BSHSI - Healthy Weight v2'
				AND (pp7.[Delete] = 'False')
				AND pr7.Selected = 'True'
				AND pa7.[State] IN ('Completed')
				AND pa7.Archived = 'True'
				AND pt7.patientid = @patientid
				AND pp7.patientprogramid = @patientprogramid
				AND pa7.ActionId IN 
					(
						SELECT DISTINCT 
							pa8.ActionId
						FROM
							RPT_Patient AS pt8 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp8 with (nolock)  ON pt8.PatientId = pp8.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm8 with (nolock) ON pp8.PatientProgramId = pm8.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa8 with (nolock)  ON pm8.PatientProgramModuleId = pa8.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps8 with (nolock) ON pa8.ActionId = ps8.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr8 with (nolock) ON ps8.StepId = pr8.StepId
						WHERE
							((ps8.SourceId = '53f56eb7ac80d31203000001') OR (ps8.SourceId = '532c4061f8efe368860003b6') )
							AND ((pa8.SourceId = '53f57115ac80d31203000014') OR (pa8.SourceId = '532c46b3c347865db8000092'))
							AND pp8.Name = 'BSHSI - Healthy Weight v2'
							AND (pp8.[Delete] = 'False')
							AND pa8.[State] IN ('Completed')
							AND pt8.PatientId = @patientid
							AND pp8.patientprogramid = @patientprogramid
					)
			ORDER BY pa7.ArchivedDate DESC
				
		END
	
		Final:
			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_DidNotEnrollReason] 
(	
	@patientid INT,
	@patientprogramid INT,
	@ProgramSourceId VARCHAR(50),
	@StepSourceId VARCHAR(50),
	@ActionSourceId VARCHAR(50)
)
RETURNS @DidNotEnrollTable TABLE( Reason VARCHAR(2000))
AS
BEGIN
		DECLARE @CompletedCount int;
		DECLARE @SavedCount int;
		DECLARE @ActionNotComplete int;
	
		SET @CompletedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @SavedCount = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
		SET @ActionNotComplete = (SELECT COUNT([Market]) FROM 	dbo.fn_RPT_ActionNotCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId));
	
	--1) get completed value
	IF @CompletedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 [Market] FROM 	dbo.fn_RPT_ActionCompleted_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) where Market != '0'
			GOTO Final
		END
	
	
	--2) saved actions will not show selected
	IF @CompletedCount = 0 AND @SavedCount > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1 NULL as [Market] FROM 	dbo.fn_RPT_ActionSaved_Text(@patientid, @patientprogramid, @ProgramSourceId, @StepSourceId, @ActionSourceId) WHERE [Market] != '0'
			--GOTO Final
		END
	

	--3) if @Current Action not complete find archive value
	IF @ActionNotComplete > 0
		BEGIN
			DELETE @DidNotEnrollTable WHERE Reason IS NULL;
			INSERT INTO @DidNotEnrollTable
			SELECT TOP 1
				CASE WHEN  pa13.Archived = 'True' AND pr13.Selected = 'True' THEN --pr.[delete] = 'False' AND pa.[State] = 'Completed'
					CASE WHEN pr13.[Delete] = 'False' THEN
						pr13.[Text]
					ELSE
						NULL
					END
				ELSE
					NULL
				END AS [didnotenrollTextarch]
			FROM 
				RPT_Patient AS pt13 with (nolock) INNER JOIN 
				RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN 					
				RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
				RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
				RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
				RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
			WHERE
				pp13.sourceid = '541943a6bdd4dfa5d90002da'
				AND ((ps13.SourceId = '53f4f885ac80d30e00000065') OR (ps13.SourceId = '532c3fc2f8efe368860003b5') )
				AND ((pa13.SourceId = '53f4fd75ac80d30e00000083') OR (pa13.SourceId = '532c45bff8efe36886000446') )
				AND (pp13.[Delete] = 'False')
				AND (pa13.Archived = 'True')
				AND (pr13.Selected = 'True')
				AND pa13.ArchivedDate IS NOT NULL
				AND pa13.[State] IN ('Completed')
				AND pt13.PatientId = @patientid
				AND pp13.patientprogramid = @patientprogramid
				AND pa13.ActionId IN 
				(
					SELECT DISTINCT 
						pa14.ActionId
					FROM
						RPT_Patient AS pt14 with (nolock) INNER JOIN
						RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
						RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
						RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
						RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
						RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
					WHERE
						pp14.sourceid = '541943a6bdd4dfa5d90002da'
						AND ((ps14.SourceId = '53f4f885ac80d30e00000065') OR (ps14.SourceId = '532c3fc2f8efe368860003b5') )
						AND ((pa14.SourceId = '53f4fd75ac80d30e00000083') OR (pa14.SourceId = '532c45bff8efe36886000446') )
						AND (pp14.[Delete] = 'False')
						AND (pr14.Selected = 'True')
						AND pt14.PatientId = @patientid
						AND pa14.[State] IN ('Completed')
						AND pp14.patientprogramid = @patientprogramid
				)	 				
			ORDER BY pa13.ArchivedDate DESC 	
				
		END
	
		Final:
			
	RETURN
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[fn_RPT_Did_Not_Enroll_Reason] 
(	
	@patientid INT,
	@patientprogramid INT
)
RETURNS VARCHAR(2000) 
AS
BEGIN
	DECLARE @DidNotEnrollReason varchar(2000)

	set @DidNotEnrollReason = 
		( 		
			COALESCE(
				(SELECT 
					( CASE WHEN pr12.[delete] = 'False' AND pa12.[State] = 'Completed' 
							THEN pr12.[Text] ELSE NULL END) AS [didnotenrollreasonText]
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
					AND (pa12.Archived = 'False')
					AND (pr12.Selected = 'True')
					AND pa12.ArchivedDate IS NULL
					AND pt12.patientid = @patientid
					AND pa12.[State] IN ('Completed')
					AND pp12.patientprogramid = @patientprogramid
					),
					(SELECT TOP 1
					(CASE WHEN pr13.[delete] = 'False' AND pa13.[State] = 'Completed'
						THEN
							pr13.[Text]
						ELSE
							NULL
						END) AS [didnotenrollTextarch]
					FROM
					RPT_Patient AS pt13 with (nolock) INNER JOIN
					RPT_PatientProgram AS pp13 with (nolock) ON pt13.PatientId = pp13.PatientId INNER JOIN
					RPT_PatientProgramModule AS pm13 with (nolock) ON pp13.PatientProgramId = pm13.PatientProgramId INNER JOIN
					RPT_PatientProgramAction AS pa13 with (nolock) ON pm13.PatientProgramModuleId = pa13.PatientProgramModuleId INNER JOIN
					RPT_PatientProgramStep AS ps13 with (nolock) ON pa13.ActionId = ps13.ActionId LEFT OUTER JOIN
					RPT_PatientProgramResponse AS pr13 with (nolock) ON ps13.StepId = pr13.StepId
					WHERE
					pp13.sourceid = '541943a6bdd4dfa5d90002da'
					AND ((ps13.SourceId = '53f4f885ac80d30e00000065') OR (ps13.SourceId = '532c3fc2f8efe368860003b5') )
					AND ((pa13.SourceId = '53f4fd75ac80d30e00000083') OR (pa13.SourceId = '532c45bff8efe36886000446') )
					AND (pp13.[Delete] = 'False')
					AND (pa13.Archived = 'True')
					AND (pr13.Selected = 'True')
					AND pa13.ArchivedDate IS NOT NULL
					AND pa13.[State] IN ('Completed')
					AND pt13.PatientId = @patientid
					AND pp13.patientprogramid = @patientprogramid
					AND pa13.ActionId IN (
						SELECT DISTINCT 
							pa14.ActionId
						FROM
							RPT_Patient AS pt14 with (nolock) INNER JOIN
							RPT_PatientProgram AS pp14 with (nolock) ON pt14.PatientId = pp14.PatientId INNER JOIN
							RPT_PatientProgramModule AS pm14 with (nolock) ON pp14.PatientProgramId = pm14.PatientProgramId INNER JOIN
							RPT_PatientProgramAction AS pa14 with (nolock) ON pm14.PatientProgramModuleId = pa14.PatientProgramModuleId INNER JOIN
							RPT_PatientProgramStep AS ps14 with (nolock) ON pa14.ActionId = ps14.ActionId LEFT OUTER JOIN
							RPT_PatientProgramResponse AS pr14 with (nolock) ON ps14.StepId = pr14.StepId
						WHERE
							pp14.sourceid = '541943a6bdd4dfa5d90002da'
							AND ((ps14.SourceId = '53f4f885ac80d30e00000065') OR (ps14.SourceId = '532c3fc2f8efe368860003b5') )
							AND ((pa14.SourceId = '53f4fd75ac80d30e00000083') OR (pa14.SourceId = '532c45bff8efe36886000446') )
							AND (pp14.[Delete] = 'False')
							AND (pr14.Selected = 'True')
							AND pt14.PatientId = pt13.patientId
							AND pa14.[State] IN ('Completed')
							AND pp14.patientprogramid = @patientprogramid
							)
					ORDER BY pa13.ArchivedDate DESC
					) 	
					)
		)

	RETURN @DidNotEnrollReason
	
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPhy_RPT_ProgramResponse_Flat]
AS
BEGIN
	TRUNCATE TABLE RPT_ProgramResponse_Flat
	INSERT INTO RPT_ProgramResponse_Flat
		([PatientId]			,
		[PatientProgramId]	,
		[ProgramSourceId]	,
		[ActionSourceId]	,
		[ActionCompleted]	,
		[ActionArchived]	,
		[ActionArchivedDate],	
		[StepSourceId]		,
		[Text]				,
		[Value]				,
		[Selected]			,
		[Delete],
		[StepCompleted]		,
		[DateCompleted])
	SELECT
		ptx.PatientId as			[PatientId]			,
		ppx.PatientProgramId as		[PatientProgramId]	,
		ppx.SourceId as				[ProgramSourceId]	,
		pax.SourceId as				[ActionSourceId]	,
		pax.Completed as            [ActionCompleted]	,
		pax.Archived as				[ActionArchived]	,
		pax.ArchivedDate as			[ActionArchivedDate],	
		psx.SourceId as				[StepSourceId]		,
		prx.[Text] as				[Text]				,
		prx.[Value] as				[Value]				,
		prx.Selected as				[Selected]			,
		prx.[Delete] as				[Delete]			,
		psx.Completed as			[StepCompleted]		,
		pax.DateCompleted as		[DateCompleted]
	FROM
		RPT_Patient AS ptx with (nolock) INNER JOIN 
		RPT_PatientProgram AS ppx with (nolock)  ON ptx.PatientId = ppx.PatientId
		INNER JOIN RPT_PatientProgramModule AS pmx with (nolock) ON ppx.PatientProgramId = pmx.PatientProgramId  				
		INNER JOIN RPT_PatientProgramAction AS pax with (nolock)  ON pmx.PatientProgramModuleId = pax.PatientProgramModuleId  				
		INNER JOIN RPT_PatientProgramStep AS psx with (nolock) ON pax.ActionId = psx.ActionId  				
		LEFT OUTER JOIN RPT_PatientProgramResponse AS prx with (nolock) ON psx.StepId = prx.StepId
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[spPhy_RPT_Flat_BSHSI_HW2]
AS
BEGIN
	EXECUTE dbo.spPhy_RPT_ProgramResponse_Flat;
	
	DELETE [RPT_BSHSI_HW2_Enrollment_Info]
	INSERT INTO [RPT_BSHSI_HW2_Enrollment_Info]
	(
		PatientId,
		PatientProgramId,		
		[Priority],
		firstName,				
		SystemId,					
		LastName,					
		MiddleName,				
		Suffix,				
		Gender,
		DateOfBirth,
		LSSN,
		GraduatedFlag,
		StartDate,
		EndDate,				
		Assigned_Date,
		Last_State_Update_Date,
		[State],
		Eligibility,				
		Assigned_PCM,
		Program_CM,
		Enrollment,
		Program_Completed_Date,
		Re_enrollment_Date,
		Enrolled_Date,
		Pending_Enrolled_Date,
		Enrollment_Action_Completion_Date,
		Market,
		Disenroll_Date,
		Disenroll_Reason,
		did_not_enroll_date,
		did_not_enroll_reason
	) 
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
		,ppa.GraduatedFlag
		,ppt.AttributeStartDate as [StartDate] 	
		,ppt.AttributeEndDate as [EndDate] 	
		,ppt.AssignedOn as [Assigned_Date] 	
		,ppt.StateUpdatedOn as [Last_State_Update_Date] 	
		,ppt.[State] as [State] 
		,ppa.Eligibility			
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
			AND pp.SourceId = '541943a6bdd4dfa5d90002da'
			AND pp.PatientProgramId = ppt.PatientProgramId) as [Program_CM]		 	
		,(SELECT TOP 1 [Enrollment] FROM dbo.fn_RPT_Enrollment(pt.PatientId, ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4fb39ac80d30e00000067','53f4fd75ac80d30e00000083')) as [Enrollment]	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f57309ac80d31200000017', '53f57383ac80d31203000033')) as [Program_Completed_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f571e0ac80d3120300001b', '53f572caac80d31203000020')) as [Re_enrollment_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4f920ac80d30e00000066', '53f4fd75ac80d30e00000083') ) as [Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4f7ecac80d30e02000072', '53f4fd75ac80d30e00000083')) as [Pending_Enrolled_Date]        	
		,(select CASE WHEN LEN(Value) > 0 THEN [Value] ELSE NULL END
			from fn_RPT_GetRecentActionCompletedDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4f920ac80d30e00000066', '53f4fd75ac80d30e00000083')) as [Enrollment_Action_Completion_Date]
		,( select Market FROM dbo.fn_RPT_Market(pt.PatientId,ppt.PatientProgramId) ) as [Market]
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f56eb7ac80d31203000001', '53f57115ac80d31203000014')) as [Disenroll_Date]	 	
		,( select Market FROM dbo.fn_RPT_DisEnrollmentReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f56f10ac80d31200000001', '53f57115ac80d31203000014') ) as [Disenroll_Reason]	
		,( select CASE WHEN LEN(Value) > 0 THEN Value ELSE NULL END
			from fn_RPT_GetDate(pt.PatientId, ppt.PatientProgramId, '541943a6bdd4dfa5d90002da', '53f4fc71ac80d30e02000074', '53f4fd75ac80d30e00000083') ) as [did_not_enroll_date]
		,(select Reason FROM dbo.fn_RPT_DidNotEnrollReason(pt.PatientId,ppt.PatientProgramId,'541943a6bdd4dfa5d90002da', '53f4f885ac80d30e00000065', '53f4fd75ac80d30e00000083')) as [did_not_enroll_reason]
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
	AND ppt.[Delete] = 'False'
END
GO
