define(['services/formatter'],
    function (formatter) {

        var validators = {};
		
        validators.zipValidator = breeze.Validator.makeRegExpValidator(
            "zipVal",
            /^\d{5}([\-]\d{4})?$/,
            "The %displayName% '%value%' is not a valid U.S. zip code.");
		
		validators.phoneValidator = breeze.Validator.makeRegExpValidator(
			"phoneValidator",
			/^d{3}-?\d{3}-?\d{4}$/,
			"The %displayName% '%value%' is not a valid U.S. pnone number.");
			
		validators.ssnValidator = breeze.Validator.makeRegExpValidator(
			"SSN",
			/^\d{3}-?\d{2}-?\d{4}$/,
			"The %displayName% '%value%' is not a valid SSN.");

		validators.dobValidator = new breeze.Validator(
				"dobValidator",
				dobValidatorFn,
				{
					messageTemplate: "'%displayName%' %msg%"
				}
		);
					
		//the value can be a partial date string when date fields are keyboard friendly.
		// moment nor breeze date validators wont cut it right.
		function dobValidatorFn( value, context ){			
			if (value == null || value == "") return true;	//valid
			if( isNaN(new Date(value).valueOf()) ){
				context.msg = "is not valid";
				return false;
			}
			if( !moment(value, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ){
				context.msg = "is not valid";
				return false;
			} 			
			var oldestDOB = moment().subtract(200, 'y').minutes(0).hours(0).seconds(0).milliseconds(-1);
			if( !moment(value).isBefore(moment()) ){
				context.msg = "is not valid (must be in the past)";
				return false;
			} 
			if ( !moment(value).isAfter( oldestDOB ) ){
				context.msg = "is not valid (over 200 years ago)";
				return false;
			}
			return true;	//valid
		}
		
		// validators.observationValidator = new breeze.validator(
			// 'observationValidator',
			// observationValidatorFn,
			// { 
				// messageTemplate: "%msg%"
			// }
		// );
		
		// function observationValidatorFn( entity, context ){
			// var startDate = entity.getProperty('startDate');
			// var values = entity.getProperty('values');
		// }
		
		validators.dateValidator = function(context){
			
			return new breeze.Validator(
				"dateValidator",
				dateValidatorFn,
				{
					minDate: context.minDate,
					maxDate: context.maxDate,
					messageTemplate: "'%displayName%' %msg%",					
				}
			);	
		};
		
		//the value can be a partial date string when date fields are keyboard friendly.
		// moment nor breeze date validators wont cut it right.
		function dateValidatorFn( value, context ){
			if (value == null || value == "") return true;	//valid
			if( isNaN(new Date(value).valueOf()) ){
				context.msg = "is not valid";
				return false;
			}
			if( !moment(value, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ){
				context.msg = "is not valid";
				return false;
			}
			if( context.minDate ){
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
					context.msg = "can not be before " + minStr;
					return false;
				}				
			}				
			if( context.maxDate ){
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
					context.msg = "can not be after " + maxStr;
					return false;
				}				
			}			
			return true;	//valid
		}
		
		validators.greaterThanValidator = function(context){ 
			var msg = "'%displayName%' must be greater than '%other%'";
			if(context.isDate){
				msg = "'%displayName%' must be after '%other%'";
			}
			return new breeze.Validator(
				"greaterThanValidator",
				greaterThanValidatorFn,
				{
					messageTemplate: msg,
					other: context.other,
					isDate: context.isDate
				}
			);
		
			function greaterThanValidatorFn( value, context ){
				if( !moment(value, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ) return true;
				if( context.other == null ) return true;
				if( context.isDate ){
					var other = context.other;
					if( other === 'now' || other === 'today'){
						other = moment();					
					}
					if( !moment(other).isValid() ) return true;
					return moment(value).isAfter(moment(other), 'days');
				}			
				return ( value > context.other ) ? true : false;
			}
		}
		
		validators.lessThanValidator = function(context){
			var msg = "'%displayName%' must be less than '%other%'";
			if(context.isDate){
				msg = "'%displayName%' must be before '%other%'";
			}
			return new breeze.Validator(
				"lessThanValidator",
				lessThanValidatorFn,
				{
					messageTemplate: msg,
					other: context.other,
					isDate: context.isDate
				}
			);
		
			function lessThanValidatorFn( value, context ){
				if( !moment(value, ["MM-DD-YYYY","MM/DD/YYYY"], true).isValid() ) return true;
				if( context.other == null ) return true;
				if( context.isDate ){
					var other = context.other;
					if( other === 'now' || other === 'today'){
						other = moment();				
					}
					if( !moment(other).isValid() ) return true;
					return moment(value).isAfter(moment(other), 'days');	
				}
				return ( value < context.other ) ? true : false;
			}
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