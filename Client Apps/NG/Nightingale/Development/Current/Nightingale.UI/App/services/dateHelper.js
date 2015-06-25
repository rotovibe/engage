
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
		*	@param noOptimize {Boolean} optional parameter, if true it will evalueate the date string as it is. 
		*	if false/undefined then it will optimize formatting of the date, then check if the optimized date string is valid. 
		*	@example given date value="6/2/2015" with noOptimize = true => not valid.
		*	@example given date value="6/2/2015" with noOptimize = false => optimize it to "06/02/2015" and then validate => valid.
		*/
		dateHelper.isValidDate = function (value, noOptimize){
			if (value === null || value === "" || value === undefined) return false;	
			if ( Object.prototype.toString.call(value) === '[object Date]' ){
				//value is a Date object
				if( isNaN(value.valueOf()) ){				
					return false;	
				}
				return true;	//valid
			}				
			else{
				//string
				if( noOptimize && value.search(/^\d{2}\/\d{2}\/\d{4}/) !== 0 ){
					//incomplete / not formatted date string
					return false;
				}
				if( !moment(value, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ){				
					//trying to parse value as a short date string failed
					if( moment(value).isValid() && moment(value)._f === "YYYY-MM-DDTHH:mm:ss.SSSSZ" ){
						return true; //ISO 8601
					}
					if( !noOptimize ){
						//try to optimize the date string:
						value = formatter.date.optimizeDate( value );
						value = formatter.date.optimizeYear( value );
					}
					if( !moment(value, ["MM/DD/YYYY"], true).isValid() ){
						return false;	//failed
					} 					
				}				
			}	
			return true;
		};
		
		/**
		*	validate a date and return error object with Message, or null (null=valid)
		*	@method isInvalidDate
		*	@param value {String} or {date} 
		*	@param context optional: a validation context object with minDate / maxDate
		*	@param context.mindate {String} or {Date} or one of the following: 'now' , 'today'
		* 	@example testing a date with isInvalidDate with a max date value of today: 
		*
		*		var startDateError = dateHelper.isInvalidDate( medication.startDate(), { maxDate: 'today'} );	
		*		if( startDateError != null ){
		*			medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date ' + startDateError.Message});
		*			hasErrors = true;
		*		}
		*/
		dateHelper.isInvalidDate = function(value, context){
			if (value == null || value == "") return null;	//valid	
			if( isNaN(new Date(value).valueOf()) ){
				return {Message: 'is not valid'};
			}
			if( !moment(value, ["MM/DD/YYYY", "MM-DD-YYYY"], true).isValid() ){
				//short format failed
				var theMoment = moment(value, ["YYYY-MM-DDTHH:mm:ss.SSSSZ"], true);	//iso 8601
				if( !theMoment.isValid() ){
					//iso 8601 failed
					var formattedValue = formatter.date.optimizeDate( value );
					formattedValue = formatter.date.optimizeYear( value );
					if( !moment(formattedValue, ["MM/DD/YYYY"], true).isValid() ){
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
				if( moment(value).isBefore(moment(minDate), 'days') ){
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
				if( moment(value).isAfter(moment(maxDate), 'days') ){
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
			// console.log('datepicker setDateValue starts: src='+ momentSrc.toISOString() + ' dest=' +momentDest.toISOString());
			momentDest.date( momentSrc.date() );
			momentDest.month( momentSrc.month() );
			momentDest.year( momentSrc.year() );
			// console.log('datepicker setDateValue returns: ' + momentDest.toISOString());
			return momentDest;
		};
		
		return dateHelper;
});