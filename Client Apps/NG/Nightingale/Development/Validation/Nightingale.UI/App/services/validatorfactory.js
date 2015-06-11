define([], function () {

    var validators = {
        fixNamesAndRegisterValidators: fixNamesAndRegisterValidators
    };

    // TODO : Needs unit tests
    function fixNamesAndRegisterValidators(metadataStore, entityTypeName, propList, entityValidators) {
        var thisEntityType = metadataStore.getEntityType(entityTypeName);
		if( propList ){
			ko.utils.arrayForEach(propList, function (entityProp) {
				var thisProp = thisEntityType.getProperty(entityProp.name);
				thisProp.displayName = entityProp.displayName;
				ko.utils.arrayForEach(entityProp.validatorsList, function (validator) {
					thisProp.validators.push(validator);
				});
			});
		}
		if( entityValidators ){
			ko.utils.arrayForEach( entityValidators, function( ev ) {
				thisEntityType.validators.push( ev );
			});
		}
		
    }

    return validators;

});