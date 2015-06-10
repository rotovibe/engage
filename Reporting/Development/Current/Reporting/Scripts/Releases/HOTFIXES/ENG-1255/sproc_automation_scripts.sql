/*** sproc automation ****/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'RPT_SprocNames')
	DROP TABLE RPT_SprocNames;
GO

CREATE TABLE RPT_SprocNames
	(
	id int NOT NULL IDENTITY (1, 1),
	SprocName varchar(100) NOT NULL,
	Prerequire bit NOT NULL,
	Description varchar(2000) NULL
	)  ON [PRIMARY]
GO

INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_ProgramResponse_Flat', 'true');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_PatientInformation', 'true');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_BSHSI_HW2', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_CareBridge', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Engage', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_Observations_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Flat_TouchPoint_Dim', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_SavePatientInfo', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight', 'false');
INSERT INTO RPT_SprocNames (SprocName, Prerequire) VALUES ('spPhy_RPT_Program_Details_By_Individual_Healthy_Weight2', 'false');
GO

/***** spPhy_RPT_Execute_Sproc ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Execute_Sproc')
	DROP PROCEDURE [spPhy_RPT_Execute_Sproc];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Execute_Sproc]
(	
	@Sproc VARCHAR(2000)
)
AS
BEGIN                    
	DECLARE @StartTime Datetime;
	DECLARE @Sql VARCHAR(2000);
	
	SET @StartTime = GETDATE();
	
	SET @Sql = N'EXECUTE ['+ @Sproc +'];'
	EXEC(@Sql);
	
	INSERT RPT_ProcessAudit ([Statement], [Start], [End], [Contract], [Time]) VALUES (@Sproc, @StartTime, GETDATE(), '', LEFT(CONVERT(VARCHAR(10), GETDATE() - @StartTime, 108), 10));
END
GO

/***** [spPhy_RPT_Load_Controller] ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Load_Controller')
	DROP PROCEDURE [dbo].[spPhy_RPT_Load_Controller];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Load_Controller]
AS
BEGIN                    

	/***** Cursor to run through prereq sproc list  ******/
	DECLARE @Cursor CURSOR;
	DECLARE @SprocName VARCHAR(200);
	BEGIN
		SET @Cursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'true'      

		OPEN @Cursor 
		FETCH NEXT FROM @Cursor 
		INTO @SprocName

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @SprocName
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @SprocName;
		  FETCH NEXT FROM @Cursor 
		  INTO @SprocName 
		END; 

		CLOSE @Cursor ;
		DEALLOCATE @Cursor;
	END;	
	
	/***** Cursor to run through sproc list  ******/
	DECLARE @sCursor CURSOR;
	DECLARE @Sproc VARCHAR(200);
	BEGIN
		SET @sCursor = CURSOR FOR
		select SprocName from RPT_SprocNames where Prerequire = 'false'      

		OPEN @sCursor 
		FETCH NEXT FROM @sCursor 
		INTO @Sproc

		WHILE @@FETCH_STATUS = 0
		BEGIN
			Print @Sproc
			EXEC [spPhy_RPT_Execute_Sproc] @Sproc = @Sproc;
			
		  FETCH NEXT FROM @sCursor 
		  INTO @Sproc 
		END; 

		CLOSE @sCursor ;
		DEALLOCATE @sCursor;
	END;	


END
GO


/****** Object:  StoredProcedure [dbo].[spPhy_RPT_Initialize_Flat_Tables]    Script Date: 05/15/2015 13:34:55 ******/
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'spPhy_RPT_Initialize_Flat_Tables')
	DROP PROCEDURE [spPhy_RPT_Initialize_Flat_Tables];
GO

CREATE PROCEDURE [dbo].[spPhy_RPT_Initialize_Flat_Tables]
AS
BEGIN
	EXEC [spPhy_RPT_Load_Controller];
END
GO