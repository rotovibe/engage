define(['services/session', 'services/dateHelper'],
    function (session, dateHelper) {

        var datacontext;

        var DT = breeze.DataType;

        var medicationModels = {
            initialize: initialize
        };
        return medicationModels;

        function initialize(metadataStore) {

            metadataStore.addEntityType({
                shortName: "PatientMedication",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    patientId: { dataType: "String" },
                    dosage: { dataType: "String" },
                    strength: { dataType: "String" },
                    route: { dataType: "String" },
                    form: { dataType: "String" },
                    deleteFlag: { dataType: "Boolean" },
                    startDate: { dataType: "DateTime" },
                    endDate: { dataType: "DateTime" },
                    createdOn: { dataType: "DateTime" },
                    updatedOn: { dataType: "DateTime" },
                    statusId: { dataType: "String" },
                    freqQuantity: { dataType: "String" },
                    freqHowOftenId: { dataType: "String" },
                    frequencyId:  { dataType: "String" },
                    freqWhenId: { dataType: "String" },
                    categoryId: { dataType: "String" },
                    sourceId: { dataType: "String" },
                    systemName: { dataType: "String" },
                    typeId: { dataType: "String" },
                    prescribedBy: { dataType: "String" },
                    sigCode: { dataType: "String" },
                    reason: { dataType: "String" },
                    isCreateNewMedication: { dataType: "Boolean" },
                    customFrequency:  { dataType: "String" },
                    isDuplicate: { dataType: "Boolean" },
                    familyId: { dataType: "String" },
                    notes: { dataType: "String" },
                    nDCs: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
                    pharmClasses: { complexTypeName: "Identifier:#Nightingale", isScalar: false },
                    originalDataSource: { dataType: "String" },
                    duration: { dataType: "String" },
                    durationUnitID: { dataType: "String" },
                    otherDuration: { dataType: "String" },
                    reviewID: { dataType: "String" },
                    refusalReasonID: { dataType: "String" },
                    otherRefusalReason: { dataType: "String" },
                    orderedBy: { dataType: "String" },
                    orderedDate: { dataType: "DateTime" },
                    prescribedDate: { dataType: "DateTime" },
                    rxNumber: { dataType: "String" },
                    rxDate: { dataType: "DateTime" },
                    pharmacy: { dataType: "String" },
                    originalDataSource:  { dataType: "String", defaultValue: 'Engage' },
                },
                navigationProperties: {
                    patient: {
                        entityTypeName: "Patient", isScalar: true,
                        associationName: "Patient_Medications", foreignKeyNames: ["patientId"]
                    },
                    status: {
                        entityTypeName: "MedicationStatus", isScalar: true,
                        associationName: "Medication_Status", foreignKeyNames: ["statusId"]
                    },
                    category: {
                        entityTypeName: "MedicationCategory", isScalar: true,
                        associationName: "Medication_Category", foreignKeyNames: ["categoryId"]
                    },
                    type: {
                        entityTypeName: "MedSuppType", isScalar: true,
                        associationName: "Medication_Type", foreignKeyNames: ["typeId"]
                    },
                    source: {
                        entityTypeName: "AllergySource", isScalar: true,
                        associationName: "Medication_Source", foreignKeyNames: ["sourceId"]
                    },
                    freqHowOften: {
                        entityTypeName: "FreqHowOften", isScalar: true,
                        associationName: "Medication_FreqHowOften", foreignKeyNames: ["freqHowOftenId"]
                    },
                    freqWhen: {
                        entityTypeName: "FreqWhen", isScalar: true,
                        associationName: "Medication_FreqWhen", foreignKeyNames: ["freqWhenId"]
                    },
                    frequency: {
                        entityTypeName: "PatientMedicationFrequency", isScalar: true,
                        associationName: "Medication_Frequency", foreignKeyNames: ["frequencyId"]
                    },
                    durationUnit: {
                        entityTypeName: "PatientMedicationFrequency", isScalar: true,
                        associationName: "Medication_DurationUnit", foreignKeyNames: ["durationUnitID"]
                    },
                    review: {
                        entityTypeName: "PatientMedicationFrequency", isScalar: true,
                        associationName: "Medication_MedicationReview", foreignKeyNames: ["reviewID"]
                    },
                    refusalReason: {
                        entityTypeName: "PatientMedicationFrequency", isScalar: true,
                        associationName: "Medication_RefusalReason", foreignKeyNames: ["refusalReasonID"]
                    }
                }
            });

            metadataStore.registerEntityTypeCtor(
                'PatientMedication', null, medicationInitializer);

            function medicationInitializer(medication) {
                medication.isNew = ko.observable(false);

                medication.isCreateNewMedication = ko.observable(false);

                medication.customFrequency = ko.observable();
                medication.isDuplicate = ko.observable(false);
                medication.familyId = ko.observable();
                medication.canSave = ko.observable(false);
                medication.isEditing = ko.observable(false);
                medication.recalculateNDC = ko.observable(false);
                medication.deleteFlag(false);
                medication.computedSigCode = ko.computed(function () {
                    var strDateRange = '';
                    if(medication.startDate() && medication.endDate()){
                        var startDate = moment(medication.startDate());
                        var startDate = startDate && startDate.isValid() ? startDate : null;
                        var endDate = moment(medication.endDate());
                        var endDate = endDate && endDate.isValid() ? endDate : null;
                        if(startDate && endDate){
                            var days = endDate.diff(startDate, 'days');
                            if (days){
                                strDateRange = 'for ' + days + (days==1 ? ' day' : ' days');
                            }
                        }
                    }
                    if(!medication.freqQuantity() && !medication.strength() && !medication.form() && !medication.route() &&  !medication.frequency() && !strDateRange){
                        return '-';
                    }

                    var strength = medication.strength() ? medication.strength().trim() : '';
                    var form = medication.form() ? medication.form().trim() : '';
                    var route = medication.route() ? medication.route().trim() : '';

                    var quantity = medication.freqQuantity() ? medication.freqQuantity().trim() : '';
                    quantity = quantity ? quantity: '';
                    var howOften = medication.frequency() ? medication.frequency().name().trim() : '';

                    if(!quantity && !strength && !form && !route && !howOften && !strDateRange){
                        return '-';
                    }
                    return quantity + ' ' + strength + ' ' + form + ' ' + route + ' ' + howOften + ' ' + strDateRange;
                });
                medication.computedPrescribedBy = ko.computed(function () {
                    var result = '';
                    var prescribedBy = medication.prescribedBy();
                    result = prescribedBy ? prescribedBy : '';
                    var prescribedDate = medication.prescribedDate();
                    if (prescribedDate) {
                        result = result ? (result + ' on ' + prescribedDate) : prescribedDate;
                    }
                    return result;
                });
                medication.computedOrderedBy = ko.computed(function () {
                    var result = '';
                    var orderedBy = medication.orderedBy();
                    result = orderedBy ? orderedBy : '';
                    var orderedDate = medication.orderedDate();
                    if (orderedDate) {
                        var date = moment(orderedDate);
                        var strDate = date.format('MM/DD/YYYY');
                        result = result ? (result + ' on ' + strDate) : strDate;
                    }
                    return result;
                });
                medication.computedRxInfo = ko.computed(function () {
                    var result = '';
                    var type = medication.type();
                    result = type ? type.name() : 'Unknown';
                    var rxNumber = medication.rxNumber();
                    result = rxNumber ? (result + ', Rx # ' + rxNumber) : result;
                    var rxDate = medication.rxDate();
                    if (rxDate) {
                        var date = moment(rxDate);
                        var strDate = date.format('MM/DD/YYYY');
                        result = result ? (result + ' on ' + strDate) : result;
                    }
                    return result;
                });

                medication.associatedProblems = ko.computed(function () {
                  var problemString = '';
                  if (medication && medication.patient()) {
                    var theseObservations = medication.patient().observations();
                    var filteredObservations = ko.utils.arrayFilter(theseObservations, function (item) {
                      var truthy = false;
                      if (item.type()) {
                        if (item.type().name().toLowerCase() === 'problems') {
                          truthy = (item.displayId() && item.state() && item.displayId() > 0 && item.state().name().toLowerCase() === 'active' && !item.deleteFlag()) ? true : false;
                        }
                      }
                      return truthy;
                    });
                    ko.utils.arrayForEach(filteredObservations, function (problem) {
                      problemString += problem.name() + ', ';
                    });
                    problemString = problemString.slice(0, -2);
                  }
                  return problemString;
                }).extend({ throttle: 75 });

                medication.setStatus = function(statusId, doneBannerMessage){
                    checkDataContext();
                    medication.statusId(statusId);
                    datacontext.saveMedication(medication).then(saveCompleted);

                    function saveCompleted() {
                        medication.isNew(false);
                        medication.entityAspect.acceptChanges();
                        datacontext.createAlert('warning', doneBannerMessage);
                    }
                }
                medication.inactivate = function () {
                    medication.setStatus(2, 'Medication has been deactivated!');
                }
                medication.activatePatientMedication = function(){
                    medication.setStatus(1, 'Medication has been activated!');
                }
                medication.deletePatientMedication = function(){
                    var message = 'You are about to delete: ' + medication.name() +' from this individual.  Press OK to continue, or cancel to return without deleting.';
                    var result = confirm(message);
                    if (result === true) {
                        checkDataContext();
                        datacontext.deletePatientMedication(medication).then(deleted);
                        function deleted () {
                            return true;
                        }
                    }
                    else {
                        return false;
                    }
                }
                medication.startDateErrors = ko.observableArray([]);
                medication.endDateErrors = ko.observableArray([]);
                medication.prescribedDateErrors = ko.observableArray([]);
                medication.orderedDateErrors = ko.observableArray([]);
                medication.rxDateErrors = ko.observableArray([]);
                medication.validationErrors = ko.observableArray([]);
                medication.isValid = ko.computed( function() {
                    var hasErrors = false;
                    var medicationErrors = [];
                    var startDate = medication.startDate();
                    var endDate = medication.endDate();
                    var startDateErrors = medication.startDateErrors();
                    var endDateErrors = medication.endDateErrors();
                    if( startDateErrors.length > 0 ){
                        ko.utils.arrayForEach( startDateErrors, function(error){
                            medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date ' + error.Message});
                            hasErrors = true;
                        });
                    }
                    if( endDate ){
                        if( endDateErrors.length > 0 ){
                            ko.utils.arrayForEach( endDateErrors, function(error){
                                medicationErrors.push({ PropName: 'endDate', Message: medication.name() + ' End Date ' + error.Message});
                                hasErrors = true;
                            });
                        }
                        if( startDate && !hasErrors ){
                            if( moment(startDate).isAfter( moment( endDate ) ) ){
                                medicationErrors.push({ PropName: 'endDate', Message: medication.name() + ' End Date must be on or after: ' + moment( startDate ).format("MM/DD/YYYY") });
                                medicationErrors.push({ PropName: 'startDate', Message: medication.name() + ' Start Date must be on or before: ' + moment( endDate ).format("MM/DD/YYYY") });
                                hasErrors = true;
                            }
                        }
                    }
                    medication.validationErrors(medicationErrors);
                    return medication.canSave() && !hasErrors;
                });

                medication.needToSave = function(){
                    var result = (medication.isNew() && medication.name() && medication.type() && medication.category() && medication.canSave());
                    result = result || ( medication.isEditing() && medication.entityAspect.entityState.isModified() );
                    return result;
                }
                medication.validationErrorsArray = ko.computed(function () {
                    var thisArray = [];
                    ko.utils.arrayForEach(medication.validationErrors(), function (error) {
                        thisArray.push(error.PropName);
                    });
                    return thisArray;
                });

            }

            metadataStore.addEntityType({
                shortName: "PatientMedicationFrequency",
                namespace: "Nightingale",
                dataProperties: {
                    id: { dataType: "String", isPartOfKey: true },
                    name: { dataType: "String" },
                    patientId: { dataType: "String", isNullable: true },
                    sortOrder:  { dataType: "Int64" }
                }
            });

            metadataStore.registerEntityTypeCtor(
                'PatientMedicationFrequency', null, medFrequencyInitializer);

            function medFrequencyInitializer(frequency) {
                if(frequency.patientId === undefined){
                    frequency.patientId = ko.observable(null);
                }
                if(frequency.sortOrder === undefined){
                    frequency.sortOrder = ko.observable(0);
                }
            }
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });