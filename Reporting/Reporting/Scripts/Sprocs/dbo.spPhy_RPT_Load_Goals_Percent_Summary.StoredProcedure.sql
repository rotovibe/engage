SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_Load_Goals_Percent_Summary] 
	@Absstartdate date,
	@AbsendDate date,
	@start	date
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE FROM RPT_Goals_Percent_Summary;
	 
	DECLARE @DateRanges table (TheDate varchar(50))
	 
	while @Absstartdate < @AbsendDate begin
	 
	insert into @DateRanges
	select CONVERT(varchar, dateadd(d, -1, dateadd(m, 1, @AbsStartDate)))
	select @Absstartdate = DATEADD(M, 1, @Absstartdate)
	end

	INSERT INTO RPT_Goals_Percent_Summary
	select 
		TheDate,
		dbo.fn_Get_Met_Goal_Percentage(@start, TheDate) as [Just_Met], 
		dbo.fn_Just_Goals_Percentage(@start, TheDate) as [Just_Goals], 
		dbo.fn_Interventions_Tasks_Percentage(@start, TheDate) as [Interventions_Tasks], 
		(dbo.fn_Get_Patients_Without_Goals_Count( CONVERT(DATETIME, TheDate, 101)) / CAST((Select COUNT(DISTINCT PatientId) FROM dbo.RPT_Patient) AS DECIMAL) * 100 ) as [No_Goals]	 
	from 
	@DateRanges
END
GO
