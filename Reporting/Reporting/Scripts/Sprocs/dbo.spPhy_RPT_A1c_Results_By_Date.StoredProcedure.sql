SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_A1c_Results_By_Date]
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @sDate datetime;
	DECLARE @eDate datetime;

	SELECT @sDate = MIN(StartDate) from InHealth001_Report.dbo.RPT_PatientObservation;
	SELECT @eDate = MAX(StartDate) from InHealth001_Report.dbo.RPT_PatientObservation;

	DECLARE @DateRanges table (id int, startDate datetime, endDate datetime)
	 
	DECLARE @id int = 1;
	WHILE @sDate < @eDate 
	BEGIN
		insert into @DateRanges
			SELECT @id, DATEADD(mm, DATEDIFF(mm, 0, @sDate), 0), DATEADD(ms, -3, DATEADD(mm, DATEDIFF(m, 0, @sDate) + 1, 0))
			select @sDate = DATEADD(M, 1, @sDate)
			SET @id = @id + 1;
	END
	--select * from @DateRanges;

	DECLARE @rowCount int, @I int;
	select @rowCount = COUNT(*) FROM @DateRanges

	DECLARE @Result table (startDate datetime, endDate datetime, [a1c<7] decimal(5,2),[a1c7-9] decimal(5,2), [a1c>9] decimal(5,2), [Total Tested] int);

	SET @I = 1;	
	WHILE(@I < @rowCount)
	BEGIN
		DECLARE @_sDate datetime, @_eDate datetime;
		SELECT @_sDate = '2014-04-01', @_eDate = endDate FROM @DateRanges where id = @I ORDER BY id ASC;
		INSERT INTO @Result 
		EXEC [dbo].[spPhy_RPT_A1c_Outcomes_By_DateRange] @startDate = @_sDate, @endDate = @_eDate;
		SET @I = @I + 1;
	END

	SELECT * FROM @Result;
		
		--EXEC [dbo].[spPhy_RPT_A1c_Outcomes_By_DateRange] @startDate = @sDate, @endDate = @eDate

END
GO
