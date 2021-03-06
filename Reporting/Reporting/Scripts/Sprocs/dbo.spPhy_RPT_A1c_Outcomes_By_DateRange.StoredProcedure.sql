SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [spPhy_RPT_A1c_Outcomes_By_DateRange]
	@startDate DateTime,
	@endDate DateTime
AS
BEGIN
	SET NOCOUNT ON;
	
		DECLARE 
			@sample decimal(5,2),
			@totalIndividuals decimal(5,2),
			@sample2 decimal(5,2),
			@totalIndividuals2 decimal(5,2),
			@sample3 decimal(5,2),
			@totalIndividuals3 decimal(5,2),
			@tested decimal(5,2)
			--@startDate datetime,
			--@endDate datetime

		--SET @startDate = Convert(datetime, '2014-04-01');
		--SET	@endDate = Convert(datetime, '2014-04-30');
		select @totalIndividuals = COUNT(distinct PatientId) from RPT_PatientObservation where ObservationId = 1
		
		SELECT @sample =  
			COUNT(*) 
		from 
			RPT_PatientObservation 
		where 
			observationId = 1 
			and PatientId is not null 
			and NumericValue is not null
			and NumericValue <= 7.0
			AND StartDate >= @startDate
			AND EndDate <= @endDate

		-- a1c 7.0 - 9.0
		SELECT @sample2 =  
			COUNT(*) 
		from 
			RPT_PatientObservation 
		where 
			observationId = 1 
			and PatientId is not null 
			and NumericValue is not null
			and NumericValue >= 7.0
			and NumericValue <= 9.0
			AND StartDate >= @startDate
			AND EndDate <= @endDate

		-- a1c > 9.0
		SELECT @sample3 =  
			COUNT(*) 
		from 
			RPT_PatientObservation 
		where 
			observationId = 1 
			and PatientId is not null 
			and NumericValue is not null
			and NumericValue >= 9.0
			AND StartDate >= @startDate
			AND EndDate <= @endDate

		-- tested
		SELECT @tested =  
			COUNT(*) 
		from 
			RPT_PatientObservation 
		where 
			observationId = 1 
			and PatientId is not null 
			and NumericValue is not null
			AND StartDate >= @startDate
			AND EndDate <= @endDate

		SELECT
		@startDate as [Start Date],
		@endDate as [End Date],
		(select @sample * 100 / @totalIndividuals) as [A1c < 7.0%],
		(select @sample2 * 100 / @totalIndividuals) as [A1c 7.0 to 9.0%],
		(select @sample3 * 100 / @totalIndividuals) as [A1c > 9.0%],
		(select @tested * 100/ @totalIndividuals) as [Individuals Tested]

END
GO
