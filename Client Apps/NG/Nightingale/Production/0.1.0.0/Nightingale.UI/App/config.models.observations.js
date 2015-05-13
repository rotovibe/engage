// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session'],
	function (session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var observationModels = {
		    initialize: initialize
		};
		return observationModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Observation information
		    metadataStore.addEntityType({
		        shortName: "Observation",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            observationId: { dataType: "String" },
		            name: { dataType: "String" },
		            standard: { dataType: "Boolean" },
		            typeId: { dataType: "String" },
		            patientId: { dataType: "String" },
		            units: { dataType: "String" },
		            startDate: { dataType: "DateTime" },
		            groupId: { dataType: "String" },
		            values: { complexTypeName: "ObservationValue:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            type: {
		                entityTypeName: "ObservationType", isScalar: true,
		                associationName: "Observation_Type", foreignKeyNames: ["typeId"]
		            },
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Observations", foreignKeyNames: ["patientId"]
		            }
		        }
		    });

		    // Observations' value complex type
		    metadataStore.addEntityType({
		        shortName: "ObservationValue",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            id: { dataType: "String" },
		            text: { dataType: "String" },
		            value: { dataType: "String" },
		            previousValue: { complexTypeName: "ObservationPreviousValue:#Nightingale", isScalar: true }
		        }
		    });

		    // Observation values' previous value complex type
		    metadataStore.addEntityType({
		        shortName: "ObservationPreviousValue",
		        namespace: "Nightingale",
		        isComplexType: true,
		        dataProperties: {
		            value: { dataType: "String" },
		            startDate: { dataType: "DateTime" },
		            unit: { dataType: "String" },
		            source: { dataType: "String" }
		        }
		    });
            
		    // Additional Observation information
		    //  - This is a list of observations to use 
		    //  - for typeahead adding of observations
		    metadataStore.addEntityType({
		        shortName: "AdditionalObservation",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            name: { dataType: "String" },
		            typeId: { dataType: "String" }
		        },
		        navigationProperties: {
		            type: {
		                entityTypeName: "ObservationType", isScalar: true,
		                associationName: "Observation_Type", foreignKeyNames: ["typeId"]
		            }
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'AdditionalObservation', null, additionalObservationInitializer);

		    metadataStore.registerEntityTypeCtor(
				'Observation', null, observationInitializer);
		    
		    metadataStore.registerEntityTypeCtor(
				'ObservationValue', null, observationValueInitializer);

		    function observationInitializer(observation) {
		        observation.computedValue = ko.computed({
		            read: function () {
		                var thisObservationsValues = observation.values();
		                var returnValue = null;
		                if (thisObservationsValues.length === 1) {
		                    returnValue = thisObservationsValues[0];
		                }
		                else if (thisObservationsValues.length > 1) {
		                    returnValue = thisObservationsValues[0];
		                }
		                return returnValue;
		            },
		            write: function (newValue) {
		                // If attribute is a multiselect,
		                if (observation.controlType === 2) {
		                    // TODO: Do something
		                }
		                else {
		                    observation.values()[0].value(newValue);
		                }
		            }
		        });
		    }

		    function additionalObservationInitializer(observation) {
		        observation.value = ko.computed(observation.name);
		    }
		    
		    function observationValueInitializer(obsValue) {
		        obsValue.previousValueString = ko.computed(function () {
		            var thisString = '';
		            // If there is a last value,
		            if (obsValue.previousValue()) {
		                // Create a string from it
		                var value = obsValue.previousValue().value();
		                var startdate = moment(obsValue.previousValue().startDate()).format('MM/DD/YYYY');
		                var unit = obsValue.previousValue().unit();
		                var source = obsValue.previousValue().source();
                    	thisString = value ? value + ' ' + unit : '';                    	
                    	thisString = moment(startdate).isValid() ? (thisString ? thisString + ' on ' + startdate : startdate) : thisString;
		                thisString = source ? thisString + ' (' + source + ')' : thisString;
		            }
		            return thisString;
		        });
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});