define(['services/formatter'],
    function (formatter) {

        var validators = {};

        validators.zipValidator = breeze.Validator.makeRegExpValidator(
            "zipVal",
            /^\d{5}([\-]\d{4})?$/,
            "The %displayName% '%value%' is not a valid U.S. zip code.");
		
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