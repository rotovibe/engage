// Register all of the user related models in the entity manager (initialize function) and provide other non-entity models
define(['services/session', 'services/dateHelper'],
	function (session, dateHelper) {

			var datacontext;

		var DT = breeze.DataType;

		// Expose the model module to the requiring modules
		var noteModels = {
				initialize: initialize
		};
		return noteModels;

		// Initialize the entity models in the entity manager
		function initialize(metadataStore) {

				// Note information
				metadataStore.addEntityType({
					shortName: "Note",
					namespace: "Nightingale",
					dataProperties: {
						id: { dataType: "String", isPartOfKey: true },
						patientId: { dataType: "String" },
						text: { dataType: "String" },
						createdOn: { dataType: "DateTime" },
						updatedOn: { dataType: "DateTime" },
						createdById: { dataType: "String" },
						updatedById: { dataType: "String" },
						typeId: { dataType: "String" },
						methodId: { dataType: "String" },
						outcomeId: { dataType: "String" },
						whoId: { dataType: "String" },
						sourceId: { dataType: "String" },
						durationId: { dataType: "String" },
						contactedOn: { dataType: "DateTime" },
						validatedIdentity: { dataType: "Boolean" },
						programIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
						//utilization props:
						admitDate: { dataType: "DateTime" },
						dischargeDate: { dataType: "DateTime" },
						systemSource: { dataType: "String" },
						admitted: { dataType: "Boolean" },
						visitTypeId: { dataType: "String" },
						otherType: { dataType: "String" },
						utilizationSourceId: { dataType: "String" },	//<-map from sourceId
						dispositionId: { dataType: "String" },
						otherDisposition: { dataType: "String" },
						locationId: { dataType: "String" },	//utilizationLocationId
						otherLocation: { dataType: "String" }
					},
					navigationProperties: {
						patient: {
							entityTypeName: "Patient", isScalar: true,
							associationName: "Patient_Notes", foreignKeyNames: ["patientId"]
						},
						type: {
							entityTypeName: "NoteType", isScalar: true,
							associationName: "Note_Type", foreignKeyNames: ["typeId"]
						},
						method: {
							entityTypeName: "NoteMethod", isScalar: true,
							associationName: "Note_Method", foreignKeyNames: ["methodId"]
						},
						outcome: {
							entityTypeName: "NoteOutcome", isScalar: true,
							associationName: "Note_Outcome", foreignKeyNames: ["outcomeId"]
						},
						who: {
							entityTypeName: "NoteWho", isScalar: true,
							associationName: "Note_Who", foreignKeyNames: ["whoId"]
						},
						source: {
							entityTypeName: "NoteSource", isScalar: true,
							associationName: "Note_Source", foreignKeyNames: ["sourceId"]
						},
						duration: {
							entityTypeName: "NoteDuration", isScalar: true,
							associationName: "Note_Duration", foreignKeyNames: ["durationId"]
						},
						//utilization lookups:
						visitType: {
							entityTypeName: "VisitType", isScalar: true,
							associationName: "Note_VisitType", foreignKeyNames: ["visitTypeId"]
						},
						utilizationSource: {
							entityTypeName: "UtilizationSource", isScalar: true,
							associationName: "Note_UtilizationSource", foreignKeyNames: ["utilizationSourceId"]
						},
						disposition: {
							entityTypeName: "Disposition", isScalar: true,
							associationName: "Note_Disposition", foreignKeyNames: ["dispositionId"]
						},
						utilizationLocation: {
							entityTypeName: "UtilizationLocation", isScalar: true,
							associationName: "Note_UtilizationLocation", foreignKeyNames: ["locationId"]
						},
					}
				});

				// ToDo information
				metadataStore.addEntityType({
						shortName: "ToDo",
						namespace: "Nightingale",
						dataProperties: {
								id: { dataType: "String", isPartOfKey: true },
								createdById: { dataType: "String" },
								assignedToId: { dataType: "String" },
								statusId: { dataType: "Int64" },
								categoryId: { dataType: "String" },
								priorityId: { dataType: "Int64" },
								dueDate: { dataType: "DateTime" },
								title: { dataType: "String" },
								description: { dataType: "String" },
								createdOn: { dataType: "DateTime" },
								updatedOn: { dataType: "DateTime" },
								closedDate: { dataType: "DateTime" },
								patientId: { dataType: "String" },
								deleteFlag: { dataType: "Boolean" },
								programIds: { complexTypeName: "Identifier:#Nightingale", isScalar: false }
						},
						navigationProperties: {
								createdBy: {
										entityTypeName: "CareManager", isScalar: true,
										associationName: "Todo_CreatedBy", foreignKeyNames: ["createdById"]
								},
								assignedTo: {
										entityTypeName: "CareManager", isScalar: true,
										associationName: "Todo_AssignedTo", foreignKeyNames: ["assignedToId"]
								},
								category: {
										entityTypeName: "ToDoCategory", isScalar: true,
										associationName: "Todo_TodoCategory", foreignKeyNames: ["categoryId"]
								},
								priority: {
										entityTypeName: "Priority", isScalar: true,
										associationName: "Todo_Priority", foreignKeyNames: ["priorityId"]
								},
								status: {
										entityTypeName: "GoalTaskStatus", isScalar: true,
										associationName: "Todo_Status", foreignKeyNames: ["statusId"]
								},
								patient: {
									entityTypeName: "Patient", isScalar: true,
									associationName: "Patient_Todos", foreignKeyNames: ["patientId"]
								},
								patientDetails: {
									entityTypeName: "ToDoPatient", isScalar: true,
									associationName: "ToDoPatient_Todos", foreignKeyNames: ["patientId"]
								}
						}
				});

				// Patient DTO
			metadataStore.addEntityType({
					shortName: "ToDoPatient",
					namespace: "Nightingale",
					dataProperties: {
							id: { dataType: "String", isPartOfKey: true },
							firstName: { dataType: "String" },
							lastName: { dataType: "String" },
							middleName: { dataType: "String" },
							suffix: { dataType: "String" },
							preferredName: { dataType: "String" }
					}
			});

				metadataStore.registerEntityTypeCtor(
				'Note', null, noteInitializer);
				metadataStore.registerEntityTypeCtor(
				'ToDo', null, todoInitializer);
				metadataStore.registerEntityTypeCtor(
				'ToDoPatient', null, toDoPatientInitializer);

				function toDoPatientInitializer (patient) {
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
				}

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
					note.updatedBy = ko.computed(function () {
						checkDataContext();
						var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
								return caremanager.id() === note.updatedById();
						});
						return thisMatchedCareManager;
					});
					note.localDate = ko.computed(function () {
						var thisDate = moment(note.createdOn()).format('MM/DD/YYYY');
						return thisDate;
					});
					note.displayText = ko.computed(function () {
						var thisText = '';
						if (note.text() && /\S/.test(note.text())) {
							thisText = note.text();
						} else {
							thisText = '-';
						}
						return thisText;
					});

					note.contactedOnErrors = ko.observableArray([]);	//datetimepicker validation errors
					note.admitDateErrors = ko.observableArray([]);
					note.dischargeDateErrors = ko.observableArray([]);
					note.validationErrors = ko.observableArray([]);
					note.isDirty = ko.observable(false);
					note.clearDirty = function () {
						note.isDirty(false);
						note.propToken.dispose();
					};
					note.watchDirty = function () {
						note.propToken = note.entityAspect.propertyChanged.subscribe(function (newValue) {
							note.isDirty(true);
						});
					};
					note.checkAppend = function () {
						// Append the new content
						var append = '\n' + moment().format('MM-DD-YYYY') + ' ';
						append += (' ' + session.currentUser().firstName() + ' ' + session.currentUser().lastName());
						append += (' - ' + note.newContent());
						note.text(note.text() ? note.text() + append : append);
						note.newContent('');
					};
					// New content when editing a note
					note.newContent = ko.observable();
					note.isValid = ko.computed( function(){
						var type = note.type();
						var typeName = type ? type.name().toLowerCase() : null;
						var hasErrors = false;
						var noteErrors = [];
						var contactedOn = note.contactedOn();
						var contactedOnErrors = note.contactedOnErrors();
						var text = note.text();
						//utilization:
						var admitDateErrors = note.admitDateErrors();
						var admitDate = note.admitDate();
						var dischargeDateErrors = note.dischargeDateErrors();
						var dischargeDate = note.dischargeDate();
						var visitType = note.visitType();
						var otherType = note.otherType();
						var disposition = note.disposition();
						var otherDisposition = note.otherDisposition();
						var utilizationLocation = note.utilizationLocation();
						var otherLocation = note.otherLocation();
						var hasChanges = note.isDirty();

						switch( typeName ){
							case 'touchpoint':
							{
								if( !text ){
									noteErrors.push({ PropName: 'text', Message: 'Content is required' });
									hasErrors = true;
								}
								if( contactedOn ){
									if( contactedOnErrors.length > 0 ){
										//datetimepicker validation errors:
										ko.utils.arrayForEach( contactedOnErrors, function(error){
											noteErrors.push({ PropName: 'contactedOn', Message: 'Date/Time of Contact ' + error.Message});
											hasErrors = true;
										});
									}
								}
								break;
							}
							case 'utilization':
							{
								if( admitDateErrors.length > 0 ){
									//datetimepicker validation errors
									ko.utils.arrayForEach( admitDateErrors, function(error){
										noteErrors.push({ PropName: 'admitDate', Message: 'Visit/Admit Date ' + error.Message});
										hasErrors = true;
									});
								}
								if( dischargeDateErrors.length > 0 ){
									//datetimepicker validation errors
									ko.utils.arrayForEach( dischargeDateErrors, function(error){
										noteErrors.push({ PropName: 'dischargeDate', Message: 'Discharge Date ' + error.Message});
										hasErrors = true;
									});
								}
								if( admitDateErrors.length == 0 && dischargeDateErrors.length == 0 && admitDate && dischargeDate ){
									//admitDate - dischargeDate range: both dates exist and valid:
									if( moment(admitDate).isAfter( moment( dischargeDate ) ) ){
										noteErrors.push({ PropName: 'admitDate', Message: ' Discharge Date must be on or after: ' + moment( admitDate ).format("MM/DD/YYYY") });
										noteErrors.push({ PropName: 'dischargeDate', Message: ' Visit/Admit Date must be on or before: ' + moment( dischargeDate ).format("MM/DD/YYYY") });
										hasErrors = true;
									}
								}
								if( !visitType && hasChanges ){
									noteErrors.push({ PropName: 'visitType', Message: 'Visit Type is required' });
									hasErrors = true;
								}
								// else if( visitType.name().toLowerCase() === 'other' && !otherType ){
									// noteErrors.push({ PropName: 'otherType', Message: 'Other Visit Type is required' });
									// hasErrors = true;
								// }
								// if( utilizationLocation && utilizationLocation.name().toLowerCase() === 'other' && !otherLocation ){
									// noteErrors.push({ PropName: 'otherLocation', Message: 'Other Location is required' });
									// hasErrors = true;
								// }
								// if( disposition && disposition.name().toLowerCase() === 'other' && !otherDisposition ){
									// noteErrors.push({ PropName: 'otherDisposition', Message: 'Other Disposition is required' });
									// hasErrors = true;
								// }
								break;
							}
							case null:
							case undefined:
							{
								noteErrors.push({ PropName: 'Type', Message: 'Type is required '});
								hasErrors = true;
							}
							default:
							{
								if( !text ){
									noteErrors.push({ PropName: 'text', Message: 'Content is required' });
									hasErrors = true;
								}
								break;
							}
						}
						note.validationErrors(noteErrors);
						return !hasErrors;
					});

					//utilization:
					note.showOtherVisitType	= ko.computed( function(){
						var visitType = note.visitType();
						var isOther = (!!visitType && visitType.name().toLowerCase() === 'other');
						if( !isOther ){
							note.otherType(null);
						}
						return isOther;
					});
					//utilization:
					note.showOtherLocation	= ko.computed( function(){
						var utilizationLocation = note.utilizationLocation();
						var isOther = (!!utilizationLocation && utilizationLocation.name().toLowerCase() === 'other');
						if( !isOther ){
							note.otherLocation(null);
						}
						return isOther;
					});
					//utilization:
					note.showOtherDisposition = ko.computed( function(){
						var disposition = note.disposition();
						var isOther = (!!disposition && disposition.name().toLowerCase() === 'other');
						if( !isOther ){
							note.otherDisposition(null);
						}
						return isOther;
					});

					/**
					*	for utilization note type: calculate the days from admitDate to discharge ( or until today )
					*	note: same day (admission = discharge) should calculate to 0
					*	@method getUtilizationLength
					*/
					note.getUtilizationLength = function(){
						var admitted = note.admitted();
						var admitDate = note.admitDate();
						var dischargeDate = note.dischargeDate();
						var admitDateErrors = note.admitDateErrors();
						var dischargeDateErrors = note.dischargeDateErrors();
						var result = null;
						if( moment(admitDate).isValid() && admitDateErrors.length === 0){
							if(!dischargeDate && admitted){
								//days from Visit/Admit Date until today
								result = moment().diff( admitDate, 'days' );
							}
							else if(dischargeDate && dischargeDateErrors.length === 0){
								//days from Visit/Admit Date until discharge Date
								result = moment(dischargeDate).diff( admitDate, 'days' );
							}
						}
						return result;
					};
					note.utilizationLengthStr = ko.computed(function(){
						var utilizationLength = note.getUtilizationLength();
						var result = '';
						if( utilizationLength === 1 ){
							result = String(utilizationLength) + ' day';
						} else if( utilizationLength === 0 || utilizationLength > 1 ){
							result = String(utilizationLength) + ' days';
						}
						return result;
					});
					// Descriptor in view for note util length
					note.utilizationLengthDesc = ko.computed(function(){
						var result = '';
						if (note.admitDate() && note.admitted() && !note.dischargeDate()) {
							result = 'Current Stay';
						} else {
							result = 'Total Stay';
						}
						return result;
					});
					note.validationErrorsArray = ko.computed( function(){
						var thisArray = [];
						ko.utils.arrayForEach( note.validationErrors(), function(error){
							thisArray.push( error.PropName );
						});
						return thisArray;
					});
					/**
					*	computed. for utilization to allow forcing the datetimepicker control to set the admit/visit date as invalid.
					*	this is needed when the date is valid but range is wrong.
					*	@method note.setInvalidStartDate
					*/
					note.setInvalidAdmitDate = ko.computed( function(){
						var validationErrorsArray = note.validationErrorsArray();
						return (validationErrorsArray && validationErrorsArray.indexOf('admitDate') !== -1);
					});

					/**
					*	computed. for utilization to allow forcing the datetimepicker control to set the discharge date as invalid.
					*	this is needed when the date is valid but range is wrong.
					*	@method note.setInvalidDischargeDate
					*/
					note.setInvalidDischargeDate = ko.computed( function(){
						var validationErrorsArray = note.validationErrorsArray();
						return (validationErrorsArray && validationErrorsArray.indexOf('dischargeDate') !== -1);
					});

				}

				function todoInitializer (todo) {
					todo.isNew = ko.observable(false);
								todo.programString = ko.computed(function () {
								checkDataContext();
								var thisString = '';
								var theseProgramIds = todo.programIds();
								if (todo.patientId() && todo.patient() && todo.patient().programs()) {
										var thesePrograms = todo.patient().programs();
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
				}
		}

		function checkDataContext() {
				if (!datacontext) {
						datacontext = require('services/datacontext');
				}
		}
	});