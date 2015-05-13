// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session'],
	function (session) {

	    var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var noteModels = {
		    initialize: initialize
		};
		return noteModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

		    // Goal information
		    metadataStore.addEntityType({
		        shortName: "Note",
		        namespace: "Nightingale",
		        dataProperties: {
		            id: { dataType: "String", isPartOfKey: true },
		            patientId: { dataType: "String" },
		            text: { dataType: "String" },
		            createdOn: { dataType: "DateTime" },
		            createdById: { dataType: "String" },
		            programIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false }
		        },
		        navigationProperties: {
		            patient: {
		                entityTypeName: "Patient", isScalar: true,
		                associationName: "Patient_Notes", foreignKeyNames: ["patientId"]
		            }
		        }
		    });

		    metadataStore.registerEntityTypeCtor(
				'Note', null, noteInitializer);

		    function noteInitializer(note) {
                note.isNew = ko.observable(false);
                note.programString = ko.computed(function () {
		            checkDataContext();
		            var thisString = '';
		            var theseProgramIds = note.programIds();
		            if (note.patient() && note.patient().programs()) {
		                var thesePrograms = note.patient().programs();
		                ko.utils.arrayForEach(theseProgramIds, function (program) {
		                    var thisProgram = ko.utils.arrayFirst(thesePrograms, function (programEnum) {
		                        return programEnum.id() === program.id();
		                    });
		                    if (thisProgram) {
		                        thisString += thisProgram.name() + ', ';
		                    }
		                });
		                // If the string is longer than zero,
		                if (thisString.length > 0) {
		                    // Remove the trailing comma and space
		                    thisString = thisString.substr(0, thisString.length - 2);
		                }
		            }
		            if (thisString.length === 0) {
		                thisString = 'None';
		            }
		            return thisString;
                });
                note.createdBy = ko.computed(function () {
                    checkDataContext();
                    var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                        return caremanager.id() === note.createdById();
                    });
                    return thisMatchedCareManager;
                });
                note.localDate = ko.computed(function () {
                    var thisDate = moment(note.createdOn()).format('MM/DD/YYYY');
                    return thisDate;
                });
		    }
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});