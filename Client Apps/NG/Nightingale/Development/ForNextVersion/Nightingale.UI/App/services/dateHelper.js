
/**
*	utility helper date related funcs
*	@module dateHelper
*/
define([ 'services/formatter'],
	function(formatter){
		
		var dateHelper = function(){};
		/**
		*	validate a date that can be a partial date string. this is important for typeable date inputs.
		*	@method isValidDate		
		*/
		dateHelper.isValidDate = function (value){
			return !this.isInvalidDate( value, null, true );
		};
		
		/**
		*	validate a date and return error object with Message, or null (null=valid)
		*	@method isInvalidDate
		*	@param value {String} or {date} 
		*	@param context optional: a validation context object with minDate / maxDate
		*	@param context.mindate {String} or {Date} or one of the following: 'now' , 'today'
		*	@emptyIsInvalid {Boolean} if true then empty date values (null / "" / undefined) are not valid.
		* 	@example testing a date with isInvalidDate with a max date value of today: 
		*
		*		var startDateError = dateHelper.isInvalidDate( medication.startDate(), { maxDate: 'today'} );	
		*		if( startDateError != null ){
		*			medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date ' + startDateError.Message});
		*			hasErrors = true;
		*		}
		*/
		
		dateHelper.isInvalidDate = function(value, context, emptyIsInvalid){
			if ( value == null || value == "" || value === undefined ){
				if( emptyIsInvalid ){
					return {Message: 'is not valid'};
				}
				else{
					return null;	//valid	
				}
			} 	
			if( isNaN(new Date(value).valueOf()) ){
				return {Message: 'is not valid'};
			}
			var theMoment = moment(value, ["MM-DD-YYYY","MM/DD/YYYY","M/D/YYYY"], true);
			if( !theMoment.isValid() || value.search(/^\d{1,2}\/\d{1,2}\/\d{4}/) === -1 ){
				//short format failed
				theMoment = moment(value, ["YYYY-MM-DDTHH:mm:ss.SSSSZ", "YYYY-MM-DDTHH:mm:ssZ"], true);	//iso 8601
				if( !theMoment.isValid() ){
					//iso 8601 failed
					var formattedValue = formatter.date.optimizeDate( value );
					if( !moment(formattedValue, ["MM/DD/YYYY"], true).isValid() || value.search(/^\d{1,2}\/\d{1,2}\/\d{4}/) === -1 ){
						return {Message: 'is not valid'};
					}
				}									
			}	
			if( context && context.minDate ){
				var minDate = context.minDate;
				var minStr;
				if( minDate === 'now' || minDate === 'today'){
					minStr = minDate;
					minDate = moment();										
				}					
				else{
					minStr = moment(minDate).format("MM/DD/YYYY");
				}
				if( !moment(minDate).isValid() ) return true;
				if( theMoment.isBefore(moment(minDate), 'days') ){
					return {Message: 'can not be before ' + minStr};					
				}				
			}				
			if( context && context.maxDate ){
				var maxDate = context.maxDate;
				var maxStr;
				if( maxDate === 'now' || maxDate === 'today'){
					maxStr = maxDate;
					maxDate = moment();									
				}
				else{
					maxStr = moment(maxDate).format("MM/DD/YYYY");
				}
				if( !moment(maxDate).isValid() ) return true;
				if( theMoment.isAfter(moment(maxDate), 'days') ){
					return {Message: 'can not be after ' + maxStr};					
				}				
			}
			return null;	//valid
		};
		
		dateHelper.isSameDate = function (moment1, moment2){
			//compare date parts only:			
			return moment(moment1.format('MM/DD/YYYY')).isSame(moment2.format('MM/DD/YYYY'))
		};
		
		dateHelper.setDateValue = function ( momentSrc, momentDest ){			
			momentDest.date( momentSrc.date() );
			momentDest.month( momentSrc.month() );
			momentDest.year( momentSrc.year() );			
			return momentDest;
		};
		
		dateHelper.setTimeValue = function( hour, minute, momentDest ){
			momentDest.hour( hour );
			momentDest.minute( minute );
			return momentDest;
		};
		return dateHelper;
});