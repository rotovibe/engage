﻿// Register all of the models in the entity manager (initialize function) and provide other non-entity models

define(['services/validatorfactory', 'services/customvalidators'],
	function (validatorFactory, customValidators) {

	    var datacontext;
	    var DT = breeze.DataType;
	    var Validator = breeze.Validator;

	    // Create a common user model
		function property(name, value) {
		    var self = this;
		    self.propertyName = ko.observable(name);
		    self.propertyValue = ko.observable(value);
		}
        
        // Parameter for something
		function Parameter(prop, val, op) {
		    var self = this;
		    self.Property = prop;
		    self.Operator = op;
		    self.Value = val;
		}

	    // Create a common modal model
		function modal(title, entity, templatePath, showing, saveOverride, cancelOverride, deleteOverride) {
		    var self = this;
		    self.Title = ko.observable(title);
		    self.Entity = entity;
		    self.TemplatePath = ko.observable(templatePath);
		    self.Showing = showing ? showing : false;
            // Method on the modal to save the currently mapped properties
		    self.saveChanges = function () {
                // If a save override was passed in,
		        if (saveOverride) {
                    // Use that to save.
		            saveOverride();
		            self.Showing(false);
		        } else {
                    // If not, use the entities default ave
		            // Check if a datacontext exists
		            checkDataContext();
		            var thisEntity = ko.unwrap(self.Entity);
		            thisEntity.saveChanges();
		            // Close the modal when the save is complete
		            self.Showing(false);
		        }
		    };
		    self.cancelChanges = function () {
		        // If a cancel override function was passed in,
		        if (cancelOverride) {
		            // Use it
		            cancelOverride();
		            self.Showing(false);
		        } else {
                    // If not, use the entities default
		            var thisEntity = ko.unwrap(self.Entity);
		            thisEntity.cancelChanges();
		            self.Showing(false);
		        }
		    };
		    // Placeholder delete method
		    self.delete = function () {
		        // If a cancel override function was passed in,
		        if (deleteOverride) {
		            // Use it
		            deleteOverride();
		            self.Showing(false);
		        } else {
		            self.Showing(false);
		        }
		    };
		    self.deleteText = ko.observable('Delete');
		    // Controls whether the modal shows save or delete buttons
		    self.canDelete = ko.observable(false);
		}
        
        // Section for the quick add menu in the shell module
		function quickAddSection(type, title, iconPath, path) {
		    var self = this;
		    self.Type = ko.observable(type);
		    self.Title = ko.observable(title);
		    self.IconPath = ko.observable(iconPath);
		    self.Path = ko.observable(path);
		}

        // Gender model to use
		function Gender(id, gender, desc) {
		    var self = this;
		    self.Id = id;
		    self.Gender = gender;
		    self.Description = desc;
		    self.SmallImgSrc = '/NightingaleUI/Content/images/patient_' + desc.toLowerCase() + '_small.png';
		    self.LargeImgSrc = '/NightingaleUI/Content/images/patient_' + desc.toLowerCase() + '_large.png';
		}

	    // Day of week model to use
		function Day(id, code, name) {
		    var self = this;
		    self.Id = id;
		    self.Code = code;
		    self.Name = name;
		}

        // Place holder until I get the types done
		function SomeType(id, name) {
		    var self = this;
		    self.id = ko.observable(id);
		    self.name = ko.observable(name);
		}

	    // Validation error message to populate entity's collection
		function validationError(propName, message) {
		    var self = this;
		    self.PropName = propName;
		    self.Message = message;
		}

		// Expose the model module to the requiring modules
		var model = {
		    property: property,
		    Parameter: Parameter,
		    modal: modal,
		    quickAddSection: quickAddSection,
		    Gender: Gender,
		    Day: Day,
		    SomeType: SomeType,
		    validationError: validationError,
		    initialize: initialize
		};
		return model;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

			// Patient information
		    metadataStore.addEntityType({
				shortName: "Patient",
				namespace: "Nightingale",
				dataProperties: {
				    id: { dataType: "String", isPartOfKey: true },
                    firstName: { dataType: "String" },
                    lastName: { dataType: "String" },
                    middleName: { dataType: "String" },
                    suffix: { dataType: "String" },
                    preferredName: { dataType: "String" },
                    displaySystemId: { dataType: "String" },
                    displaySystemName: { dataType: "String" },
                    displayLabel: { dataType: "String" },
                    providerId: { dataType: "Int64" },
                    dOB: { dataType: "String" },
                    gender: { dataType: "String" },
                    marital: { dataType: "String" },
                    background: { dataType: "String" },
                    protectedFlag: { dataType: "Boolean" },
                    lastFourSSN: { dataType: "String" },
                    fullSSN: { dataType: "String" },
                    priority: { dataType: "String" },
                    flagged: { dataType: "Boolean" },
                    twitterHandle: { dataType: "String" }
				},
				navigationProperties: {
					provider: {
						entityTypeName: "Provider", isScalar: true,
						associationName: "Provider_Patients", foreignKeyNames: ["providerId"]
					},
					priorityModel: {
					    entityTypeName: "Priority", isScalar: true,
					    associationName: "Priority_Patients", foreignKeyNames: ["priority"]
					},
					problems: {
					    entityTypeName: "PatientProblem", isScalar: false,
					    associationName: "PatientProblem_Patient"
					},
					programs: {
					    entityTypeName: "Program", isScalar: false,
					    associationName: "Patient_Programs"
					},
					goals: {
					    entityTypeName: "Goal", isScalar: false,
					    associationName: "Patient_Goals"
					},
					notes: {
					    entityTypeName: "Note", isScalar: false,
					    associationName: "Patient_Notes"
					},
					todos: {
					    entityTypeName: "ToDo", isScalar: false,
					    associationName: "Patient_Todos"
					},
					allergies: {
					    entityTypeName: "PatientAllergy", isScalar: false,
					    associationName: "Patient_Allergies"
					},
					medications: {
					    entityTypeName: "PatientMedication", isScalar: false,
					    associationName: "Patient_Medications"
					},					
					careMembers: {
					    entityTypeName: "CareMember", isScalar: false,
					    associationName: "Patient_CareMembers"
					},
					contactCard: {
					    entityTypeName: "ContactCard", isScalar: true,
					    associationName: "Patient_ContactCard"
					},
					observations: {
					    entityTypeName: "PatientObservation", isScalar: false,
					    associationName: "Patient_Observations"
					},
					patientSystems: {
					    entityTypeName: "PatientSystem", isScalar: false,
					    associationName: "Patient_PatientSystems"
					}
				}
			});

		    var patientPropertyList = [
                {
		            // Short name of property
		            name: 'firstName',
		            // Desired display name of property
		            displayName: 'First Name',
		            // Collection of validators
		            validatorsList: [
                        Validator.required(),
                        Validator.maxLength({ maxLength: 20 })
		            ]
                },
                {
                    // Short name of property
                    name: 'lastName',
                    // Desired display name of property
                    displayName: 'Last Name',
                    // Collection of validators
                    validatorsList: [
                        Validator.required(),
                        Validator.maxLength({ maxLength: 20 })
                    ]
                },
				{
					name: 'fullSSN',
					displayName: 'SSN',
					validatorsList: [
						//Validator.required(),
						customValidators.validators.ssnValidator
					]
				},
				{
					name: 'dOB',
					displayName: 'DOB',
					validatorsList: [
						Validator.required(),
						customValidators.validators.dateValidator({minDate: moment().subtract(200, 'year').format('MM/DD/YYYY'), maxDate: 'today'})	//customValidators.validators.dobValidator
					]
				}
				
                // Example of using a custom regex for validation
                //,
                //{
                //    name: 'suffix',
                //    displayName: 'Suffix',
                //    validatorsList: [
                //        customValidators.validators.zipValidator
                //    ]
                //}
		    ];

		    validatorFactory.fixNamesAndRegisterValidators(metadataStore, 'Patient', patientPropertyList);

		    // Junction object for Patient <--> Problem
			metadataStore.addEntityType({
			    shortName: "PatientProblem",
			    namespace: "Nightingale",
			    dataProperties: {
			        iD: { dataType: "String", isPartOfKey: true },
			        patientID: { dataType: "String" },
			        problemID: { dataType: "String" },
			        level: { dataType: "String" }
			    },
			    navigationProperties: {
			        patient: {
			            entityTypeName: "Patient", isScalar: true,
			            associationName: "PatientProblem_Patient", foreignKeyNames: ["patientID"]
			        },
			        problem: {
			            entityTypeName: "Problem", isScalar: true,
			            associationName: "PatientProblem_Problem", foreignKeyNames: ["problemID"]
			        }
			    }
			});

		    // Problem for a Patient
			metadataStore.addEntityType({
			    shortName: "Problem",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        name: { dataType: "String" }
			    },
			    navigationProperties: {
			        patientProblems: {
			            entityTypeName: "PatientProblem", isScalar: false,
			            associationName: "PatientProblem_Problem"
			        }
			    }
			});

			// Provider
			metadataStore.addEntityType({
				shortName: "Provider",
				namespace: "Nightingale",
				dataProperties: {
					id: { dataType: "Int64", isPartOfKey: true },
					name: { dataType: "String" }
				},
				navigationProperties: {
					patients: {
						entityTypeName: "Patient", isScalar: false,
						associationName: "Provider_Patients"
					}
				}
			});

		    // Cohort
			metadataStore.addEntityType({
			    shortName: "Cohort",
			    namespace: "Nightingale",
			    dataProperties: {
			        iD: { dataType: "String", isPartOfKey: true },
			        name: { dataType: "String" },
			        sName: { dataType: "String" },
                    description: { dataType: "String" }
			    }
			});

		    // General identifier complex type (for creating collections of ids, such as ContactCard.CommunicationModeIds
			metadataStore.addEntityType({
			    shortName: "Identifier",
			    namespace: "Nightingale",
			    isComplexType: true,
			    dataProperties: {
			        id: { dataType: "String" }
			    }
			});

		    // Allergy Search
			metadataStore.addEntityType({
			    shortName: "AllergySearch",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        name: { dataType: "String" },
			        displayName: { dataType: "String" }
			    }
			});

		    // Medication Search
			metadataStore.addEntityType({
			    shortName: "MedicationSearch",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        name: { dataType: "String" },
			        displayName: { dataType: "String" }
			    }
			});


		    // Care Member complex type
			metadataStore.addEntityType({
			    shortName: "CareMember",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        gender: { dataType: "String" },
			        preferredName: { dataType: "String" },
			        patientId: { dataType: "String" },
			        contactId: { dataType: "String" },
			        typeId: { dataType: "String" },
			        primary: { dataType: "Boolean" }
			    },
			    navigationProperties: {
			        patient: {
			            entityTypeName: "Patient", isScalar: true,
			            associationName: "Patient_CareMembers", foreignKeyNames: ["patientId"]
			        },
			        careManager: {
			            entityTypeName: "CareManager", isScalar: true,
			            associationName: "CareManager_CareMembers", foreignKeyNames: ["contactId"]
			        },
			        type: {
			            entityTypeName: "CareMemberType", isScalar: true,
			            associationName: "CareMember_Type", foreignKeyNames: ["typeId"]
			        },
			    }
			});

		    // Alert
			metadataStore.addEntityType({
			    shortName: "Alert",
			    namespace: "Nightingale",
                autoGeneratedKeyType: breeze.AutoGeneratedKeyType.Identity,
			    dataProperties: {
			        id: { dataType: "Int64", isPartOfKey: true },
			        result: { dataType: "Int64" },
			        reason: { dataType: "String" }
			    }
			});
            
		    // Patient System
			metadataStore.addEntityType({
			    shortName: "PatientSystem",
			    namespace: "Nightingale",
			    dataProperties: {
			        id: { dataType: "String", isPartOfKey: true },
			        patientId: { dataType: "String" },
			        systemId: { dataType: "String" },
			        systemName: { dataType: "String" },
			        displayLabel: { dataType: "String" },
			        deleteFlag: { dataType: "Boolean" },
			    },
			    navigationProperties: {
					patient: {
					    entityTypeName: "Patient", isScalar: true,
					    associationName: "Patient_PatientSystems", foreignKeyNames: ["patientId"]
					}
				}
			});

			//  Register constructor functions for patient
			metadataStore.registerEntityTypeCtor(
				'Patient', null, patientInitializer);
            metadataStore.registerEntityTypeCtor(
				'PatientProblem', null, patientProblemInitializer);
            metadataStore.registerEntityTypeCtor(
				'Alert', null, alertInitializer);
            metadataStore.registerEntityTypeCtor(
				'CareMember', null, careMemberInitializer);

            function patientInitializer(patient) {
                // Check if a datacontext exists
                checkDataContext();
                patient.isNew = ko.observable(false);
                patient.isDuplicate = ko.observable(false);
                patient.forcedSave = false;
                patient.age = ko.computed(function () {
                    // Get the DOB and add 23 hours to it, so it is always considered close to midnight as possible for all time zones
                    if (!patient.dOB()) { return ''; }
                    var dob = moment(patient.dOB()).add('hours', 23);
                    // Get the current moment in time to compare for age
                    var rightnow = moment(new Date()).add('days', 1);
                    // If the DOB is null, is after right now, or is not a valid date, return null
                    if (!dob || dob > rightnow || !dob.isValid()) {
                        return '';
                    }
                    // If not then calculate and return the age
                    return rightnow.diff(dob, 'years');
                });
                patient.fullFirstName = ko.computed(function () {
                    // Get the 3 possible values to be included in the full first name
                    var fn = patient.firstName();
                    var pn = patient.preferredName();
                    var mi = patient.middleName() ? patient.middleName().substr(0, 1) + '.' : '';
                    // If there is a preferred name, return that
                    //      if not then check for a middle name that is 2 chars in length and return first name + middle initial
                    //           if not then return first name only
                    var fullfn = pn ? pn : (mi.length === 2 ? fn + ' ' + mi : fn);
                    return fullfn;
                });
                patient.fullLastName = ko.computed(function () {
                    // Get the values we will need
                    var ln = patient.lastName();
                    var sfx = patient.suffix();
                    // Get the full last name.  If there is a suffix, add it to the last name
                    //      If there is not a suffix, use last name only
                    var fullln = sfx ? ln + ' ' + sfx : ln;
                    return fullln;
                });
                patient.fullName = ko.computed(function () {
                    // Get the patients full first name
                    var fn = patient.fullFirstName();
                    // Get the patients full last name
                    var ln = patient.fullLastName();
                    // If there is no first name, just show the last name
                    return fn ? fn + ' ' + ln : ln;
                });
                patient.formattedSSN = ko.computed(function () {
                	var thisString = '';
                	if (patient.lastFourSSN()) {
                		thisString = "XXX-XX-" + patient.lastFourSSN();
                	}
                	return thisString;
                });
                patient.genderModel = ko.computed({
                    read: function () {
                    	checkDataContext();
                        var thisGender;
                        var gender = patient.gender() ? patient.gender().toLowerCase() : '';
                        if (gender === 'm' || gender === 'male') {
                            patient.gender('M');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'm' === item.Id;
                            });
                        }
                        else if (gender === 'f' || gender === 'female') {
                            patient.gender('F');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'f' === item.Id;
                            });
                        }
                        else {
                            patient.gender('N');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'n' === item.Id;
                            });
                        }
                        return thisGender;
                    },
                    write: function (newValue) {
                        patient.gender(ko.unwrap(newValue).Id.toUpperCase());
                    }
			    });
				patient.forceSave = function () {
					patient.forcedSave = true;
					patient.saveChanges();
				}
                // Method to save changes to the patient
			    patient.saveChanges = function () {
    				checkDataContext();
			        // // Get the patients id
			        // var patientId = patient.id();
			        var thisPatient = patient;
			        // var params = [
           //                              new Parameter('Id', patientId),
           //                              new Parameter('Priority', thisPatient.priority()),
           //                              new Parameter('Gender', thisPatient.gender()),
           //                              new Parameter('FirstName', thisPatient.firstName()),
           //                              new Parameter('LastName', thisPatient.lastName()),
           //                              new Parameter('PreferredName', thisPatient.preferredName()),
           //                              new Parameter('Suffix', thisPatient.suffix()),
           //                              new Parameter('DOB', thisPatient.dOB()),
           //                              new Parameter('MiddleName', thisPatient.middleName()),
           //                              new Parameter('Background', thisPatient.background())
			        // ];
			        // If they are trying to override the ssn,
			        // if (thisPatient.fullSSN()) {
			        // 	// Add it as a parameter
           //              params.push(new Parameter('FullSSN', thisPatient.fullSSN()));
			        // }
			        // Go save the entity, pass in which parameters should be different
			        datacontext.saveIndividual(thisPatient);
			    }
                // Method on the modal to cancel changes to the patient
			    patient.cancelChanges = function () {
			        checkDataContext();
			        datacontext.cancelEntityChanges(patient);
			    }
			    patient.isNew.subscribe(function (newValue) {
			    	// if (newValue) {
			    	// 	patient.isValid(false);
			    	// }
			    });
			    patient.isValid = ko.observable(true);
			    patient.canSave = ko.computed(function () {
			    	return patient.isValid() && !patient.isDuplicate();
			    });
			    patient.validationErrors = ko.observableArray([]);
			    patient.entityAspect.validationErrorsChanged.subscribe(function (validationChangeArgs) {
			        var hasErrors = patient.entityAspect.hasValidationErrors;
			        var errorsList = [];
			        if (hasErrors) {
			            patient.isValid(false);
			            var errors = patient.entityAspect.getValidationErrors();
			            ko.utils.arrayForEach(errors, function (error) {
			                errorsList.push(new validationError(error.propertyName, error.errorMessage));
			            });
			            patient.validationErrors(errorsList);
			        } else {
			            patient.validationErrors([]);
			            patient.isValid(true);
			        }
			    });
			    patient.validationErrorsArray = ko.computed(function () {
			        var thisArray = [];
			        ko.utils.arrayForEach(patient.validationErrors(), function (error) {
			            thisArray.push(error.PropName);
			        });
			        return thisArray;
			    });
			    if (patient.firstName() === null) {
			    	patient.firstName('.');
			    	setTimeout(function () {
				    	patient.firstName('');
				    }, 1);
			    }
			    if (patient.lastName() === null) {
			    	patient.lastName('.');
			    	setTimeout(function () {
				    	patient.lastName('');
				    }, 1);
			    }
            }

            function careMemberInitializer(careTeam) {
                careTeam.genderModel = ko.computed({
                    read: function () {
                    	checkDataContext();
                        var thisGender;
                        var gender = careTeam.gender() ? careTeam.gender().toLowerCase() : '';
                        if (gender === 'm' || gender === 'male') {
                            careTeam.gender('M');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'm' === item.Id;
                            });
                        }
                        else if (gender === 'f' || gender === 'female') {
                            careTeam.gender('F');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'f' === item.Id;
                            });
                        }
                        else {
                            careTeam.gender('N');
                            thisGender = ko.utils.arrayFirst(datacontext.enums.genders(), function (item) {
                                return 'n' === item.Id;
                            });
                        }
                        return thisGender;
                    },
                    write: function (newValue) {
                        careTeam.gender(ko.unwrap(newValue).Id.toUpperCase());
                    }
                });
            }

			function patientProblemInitializer(problem) {
			    if (!problem.level || !problem.level()) {
			        problem.level('1');
			    }
			}

			function alertInitializer(alert) {
			    alert.type = ko.computed(function () {
			        if (alert.result() || alert.result() === 0) {
                        return alert.result() === 0 ? 'error' : 'warning';
			        }
			        return '';
			    });
			    alert.alertPath = ko.computed(function () {
			        if (alert.type()) {
			            return 'shell/alerttypes/' + alert.type() + '.html';
			        }
			        return '';
			    });
			    alert.showing = ko.observable(true);
			    var thisToken = alert.showing.subscribe(function () {
			    	checkDataContext();
			    	datacontext.alerts.remove(alert);
			    	thisToken.dispose();
			    });
			}
		}

		function checkDataContext() {
		    if (!datacontext) {
		        datacontext = require('services/datacontext');
		    }
		}
	});
