define(['services/datacontext', 'services/report.context'], function (datacontext, reportContext) {

	function entity(type, propname, columns, navprops, childentities) {
		var self = this;
		self.entityType = type;
		self.propName = propname;
		self.columns = ko.observableArray(columns);
		self.navProperties = ko.observableArray(navprops);
		self.childEntities = ko.observableArray(childentities);
		self.availableColumns = ko.observableArray([]);
		self.availableNavProps = ko.observableArray([]);
		self.availableChildEntities = ko.observableArray([]);
	}

    var metadataStore = datacontext.manager.metadataStore;

    var ctor = function () {

    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.entityType = settings.entityType;
        if (endsWith(self.entityType, 's')) {
            self.entityType = self.entityType.substr(0, self.entityType.length - 1);
        }
        if (endsWith(self.entityType, 'Model')) {
            self.entityType = self.entityType.substr(0, self.entityType.length - 5);
        }
        var thisType = metadataStore.getEntityType(self.entityType);
        var theseAvailableColumns = thisType.dataProperties;
        // Get a list of the singular nav props
        var theseAvailableNavProps = ko.utils.arrayFilter(thisType.navigationProperties, function (navprop) {
            return navprop.isScalar;
        });
        // Get a list of the collection nav properties
        var theseAvailableChildEntities = ko.utils.arrayFilter(thisType.navigationProperties, function (navprop) {
            return !navprop.isScalar;
        });
        self.entity = new entity(thisType.shortName);
        self.entity.availableColumns(theseAvailableColumns);
        self.entity.availableNavProps(theseAvailableNavProps);
        self.entity.availableChildEntities(theseAvailableChildEntities);
    }

    return ctor;

    function endsWith(str, suffix) {
        return str.indexOf(suffix, str.length - suffix.length) !== -1;
    }

});