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

	var structuraltypes = ko.observableArray([]);
	var selectedMainType = ko.observable();
	var newQueryReport = ko.observable();
    var metadataStore = datacontext.manager.metadataStore;

    selectedMainType.subscribe(function (newValue) {
    	console.log('Doign this ', newValue);
    	newQueryReport(add(newValue.shortName));
    	console.log(newQueryReport());
    });

    var queryBuilder = {
    	structuraltypes: structuraltypes,
    	selectedMainType: selectedMainType,
    	newQueryReport: newQueryReport,
        activate: activate
    };
    return queryBuilder;

    function activate() {
    	showMetaData();
    	//newQueryReport(reportContext.report('Test', 'Test description', []));
    }


    function add(type) {
    	console.log(type);
        var thisType = metadataStore.getEntityType(type);
        console.log(thisType);
    	var theseAvailableColumns = thisType.dataProperties;
    	// Get a list of the singular nav props
    	var theseAvailableNavProps = ko.utils.arrayFilter(thisType.navigationProperties, function (navprop) {
    		return navprop.isScalar;
    	});
    	// Get a list of the collection nav properties
    	var theseAvailableChildEntities = ko.utils.arrayFilter(thisType.navigationProperties, function (navprop) {
    		return !navprop.isScalar;
    	});
    	var thisEntity = new entity(thisType.shortName);
    	thisEntity.availableColumns(theseAvailableColumns);
    	thisEntity.availableNavProps(theseAvailableNavProps);
    	thisEntity.availableChildEntities(theseAvailableChildEntities);
    	return thisEntity;
    }

 	function showMetaData() {
        var md = $.parseJSON(metadataStore.exportMetadata());
        console.log(md);
        var odType = metadataStore.getEntityType("Patient");
        console.log(odType);
        structuraltypes(md.structuralTypes);
        console.log(structuraltypes());
    }

});