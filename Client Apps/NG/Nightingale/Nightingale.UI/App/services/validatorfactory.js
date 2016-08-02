define([], function () {

    var validators = {
        fixNamesAndRegisterValidators: fixNamesAndRegisterValidators
    };

    // TODO : Needs unit tests
    function fixNamesAndRegisterValidators(metadataStore, entityTypeName, propList) {
        var thisEntityType = metadataStore.getEntityType(entityTypeName);
        ko.utils.arrayForEach(propList, function (entityProp) {
            var thisProp = thisEntityType.getProperty(entityProp.name);
            thisProp.displayName = entityProp.displayName;
            ko.utils.arrayForEach(entityProp.validatorsList, function (validator) {
                thisProp.validators.push(validator);
            });
        });
    }

    return validators;

});