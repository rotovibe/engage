define(['services/datacontext'], function (datacontext) {

	function clause (prop, comp, value) {
		var self = this;
		self.Property = prop;
		self.Comparator = comp;
		self.Value = value;		
	}

	var manager = datacontext.manager;

	var reportContext = {
		clause: clause,
		createDynamicQuery: createDynamicQuery,
		//runReport: runReport,
		runNewReport: runNewReport,
		report: report
	};
	return reportContext;

	function createDynamicQuery (entityType, clauses) {
        var query = breeze.EntityQuery.from(entityType)
            .toType(entityType);

        $.each(clauses, function (index, item) {
        	query = query.where(item.Property, item.Comparator, item.Value);
        });

        return manager.executeQueryLocally(query);
	}

	function report (name, description, columns) {
		var self = this;
		self.entities = ko.observableArray([]);
		self.name = name;
		self.description = description;
		self.columns = columns;
	}

	function runNewReport(reportpassedin) {
		var thisReport = reportpassedin;
		// Get the properties we need
		var theseEntities = getListLocally(thisReport.entityType, thisReport.columns);
		goThroughArray(theseEntities, thisReport);
		return theseEntities;
	}

	function goThroughChildren(thisObject, context) {
		ko.utils.arrayForEach(context.childEntities, function (thisChildEntityType) {
        	// If we should only get the first,
        	if (thisChildEntityType.onlyFirst) {
	        	// Name the new property singularly
	        	var thisPropName = thisChildEntityType.entityType;
        		// Go get only the first result
        		thisObject[thisPropName] = getListLocally(thisChildEntityType.entityType, thisChildEntityType.columns, makeParentPropName(context.entityType), thisObject.id)[0];
        		goThroughArray(thisObject[thisPropName], thisChildEntityType);
        	} else {
	        	// Name the new property plurally
	        	var thisPropName = thisChildEntityType.entityType + 's';
				// Get the data
	        	thisObject[thisPropName] = getListLocally(thisChildEntityType.entityType, thisChildEntityType.columns, makeParentPropName(context.entityType), thisObject.id);
	        	goThroughArray(thisObject[thisPropName], thisChildEntityType);
        	}
        });
	}

	function goThroughNavProperties(thisObject, context) {
		if (context && context.navProperties) {
			ko.utils.arrayForEach(context.navProperties, function (thisNavPropType) {
	        	// Name the new property singularly
	        	var thisPropName = thisNavPropType.entityType;
	        	var navPropName = thisNavPropType.propName ? thisNavPropType.propName : makeParentPropName(thisNavPropType.entityType);
	    		// Go get only the first result
	    		thisObject[thisPropName] = getNavPropLocally(thisNavPropType.entityType, thisNavPropType.columns, 'id', thisObject[navPropName]);
	    		goThroughArray(thisObject[thisPropName], thisNavPropType);
	        });
		}
	}

	function goThroughArray (thisObject, context) {

		if (Array.isArray(thisObject)) {
	    	// Go through each object
	        ko.utils.arrayForEach(thisObject, function (item) {
	        	// It's down to a single object, pass it back
	            //single(item);
	            // For each of it's children,
	            goThroughChildren(item, context);
	            goThroughNavProperties(item, context);
	        });
		} else {
			goThroughChildren(thisObject, context);
            goThroughNavProperties(thisObject, context);
		}
	}

	function getListLocally (entityType, columns, referencePropName, parentId) {

		var columnsString = '';
		ko.utils.arrayForEach(columns, function (column) {
			columnsString = columnsString + camelCase(column) + ',';
		});
		columnsString = columnsString.length > 0 ? columnsString.substr(0, columnsString.length - 1) : '';
        var query = breeze.EntityQuery.from(entityType)
        	.toType(entityType)
        	.select(columnsString);

		if (parentId) {
			query = query.where(referencePropName, '==', parentId);
		}

        return manager.executeQueryLocally(query);
	}

	function getNavPropLocally (entityType, columns, referencePropName, parentId) {

		var columnsString = '';
		ko.utils.arrayForEach(columns, function (column) {
			columnsString = columnsString + camelCase(column) + ',';
		});
		columnsString = columnsString.length > 0 ? columnsString.substr(0, columnsString.length - 1) : '';
        var query = breeze.EntityQuery.from(entityType)
        	.toType(entityType)
        	.select(columnsString);

		if (parentId) {
			query = query.where(referencePropName, '==', parentId);
		}

        return manager.executeQueryLocally(query)[0];
	}
	
	function makeParentPropName(thisString) {
		return thisString.toLowerCase() + 'Id';
	}

	function camelCase(string) {
	    return string.charAt(0).toLowerCase() + string.slice(1);
	}

});