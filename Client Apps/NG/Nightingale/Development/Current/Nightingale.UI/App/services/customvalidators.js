/**
*	custom validators for breeze entities
*	@module customvalidators
*/
define(['services/formatter'],
    function (formatter) {

        var validators = {};

        validators.zipValidator = breeze.Validator.makeRegExpValidator(
            "zipVal",
            /^\d{5}([\-]\d{4})?$/,
            "The %displayName% '%value%' is not a valid U.S. zip code.");
			
		validators.ssnValidator = breeze.Validator.makeRegExpValidator(
			"SSN",
			/^\d{3}-?\d{2}-?\d{4}$/,
			"'%value%' is not a valid %displayName%");

		
		validators.dateValidator = function(context){
			
			return new breeze.Validator(
				"dateValidator",
				dateValidatorFn,
				{
					minDate: context.minDate,
					maxDate: context.maxDate,
					messageTemplate: "%msg%",					
				}
			);	
		};
		
		//the value can be a partial date string when date fields are keyboard friendly.
		// moment nor breeze date validators wont cut it right.
		function dateValidatorFn( value, context ){			
			if (value == null || value == "") return true;	//valid
			if( isNaN(new Date(value).valueOf()) ){				
				context.msg = "'"+ value + "' is not a valid " + context.property.displayName;
				return false;
			}	
			var theMoment = moment(value, ["MM-DD-YYYY","MM/DD/YYYY", "M/D/YYYY"], true);
			if( !theMoment.isValid() ){				
				var formattedValue = formatter.date.optimizeDate( value );				
				if( !moment(formattedValue, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ){
					context.msg = "'"+ value + "' is not a valid " + context.property.displayName;					
					return false;
				}
			}
			if( context.minDate ){
				var minDate = context.minDate;
				var errorMessage = context.property.displayName + ' must be on or after ' + moment(minDate).format("MM/DD/YYYY");
				if( minDate === 'now' || minDate === 'today'){
					errorMessage = context.property.displayName + ' must be today or in the past';
					minDate = moment();										
				}									
				if( !moment(minDate).isValid() ) return true;
				if( theMoment.isBefore(moment(minDate).format("MM/DD/YYYY"), 'days') ){
					context.msg = errorMessage;
					return false;
				}				
			}				
			if( context.maxDate ){
				var maxDate = context.maxDate;
				var errorMessage = context.property.displayName + ' must be on or before ' + moment(maxDate).format("MM/DD/YYYY");
				if( maxDate === 'now' || maxDate === 'today'){
					errorMessage = context.property.displayName + ' must be today or in the past';
					maxDate = moment();									
				}				
				if( !moment(maxDate).isValid() ) return true;
				if( theMoment.isAfter(moment(maxDate).format("MM/DD/YYYY"), 'days') ){
					context.msg = errorMessage;
					return false;
				}				
			}			
			return true;	//valid
		}
		
        // Check if all of the characters are zeroes
        function notAllZeroesFn(value, context) {
            // '== null' matches null and empty string
            if (value == null) return true; 
            // Strip out all the dashes '-'
            var strippedStr = value.replace("-", "");
            // Check if the value is all zeroes
            return parseInt(strippedStr) === 0;
        };

        validators.isNotAllZeroes = new breeze.Validator(
            "notAllZeroes",              // validator name
            notAllZeroesFn,    // validation function
            {                           // validator context
                messageTemplate: "'%displayName%' must not be all zeroes."
            });

        validators = {
            validators: validators
        };

        return validators;

});