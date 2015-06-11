﻿define(['services/datacontext', 'viewmodels/patients/data/index'],
    function (datacontext, dataIndex) {

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeDataType = self.settings.activeDataType;
			self.canSave = self.settings.canSave;
            self.showing = ko.computed(function () { return !!self.activeDataType() });
            self.selectedPatient = self.settings.selectedPatient;
            self.selectedTemplate = ko.observable();
            self.saveDataEntry = self.settings.saveDataEntry || function () { return false; };
            self.cancelDataEntry = self.settings.cancelDataEntry || function () { return false; };
            self.dataTypes = ko.computed(function () {
                var theseObservationTypes = datacontext.enums.observationTypes().slice(0);
                if (theseObservationTypes.indexOf(dataIndex.allergiesType) === -1) {
                    theseObservationTypes.push(dataIndex.allergiesType);
                }
                if (theseObservationTypes.indexOf(dataIndex.medicationsType) === -1) {
                    theseObservationTypes.push(dataIndex.medicationsType);
                }
                return theseObservationTypes;
            });
            self.templates = ko.computed(function () {
                var theseTypes = self.dataTypes();
                var tempArray = [];
                ko.utils.arrayForEach(theseTypes, function (type) {
                    // If it isn't the problem type,
                    if (type.name().toLowerCase().indexOf('problems') === -1 && type.name() !== 'Allergies' && type.name() !== 'Medications') {
                        tempArray.push(new Template(type.name(), 'viewmodels/patients/sections/basic.observations', 'viewmodels/patients/sections/additional.observations'));
                    } else if (type.name().toLowerCase().indexOf('problems') !== -1) {
                        // Or if it is a problem type,
                        tempArray.push(new Template(type.name(), 'viewmodels/patients/sections/additional.problem.observations', 'viewmodels/patients/sections/problems.list'));
                    } else if (type.name().toLowerCase().indexOf('allergies') !== -1) {
                        // Or if it is an allergy type,
                        tempArray.push(new Template(type.name(), 'viewmodels/patients/sections/allergies.search', 'viewmodels/patients/sections/allergies.edit'));
                    } else {
                        // else it must be medications
                        tempArray.push(new Template(type.name(), 'viewmodels/patients/sections/medications.search', 'viewmodels/patients/sections/medication.edit'));
                    }
                });
                return tempArray;
            });
            self.computedActiveDataType = ko.computed({
                read: function () {
                    return self.activeDataType();
                },
                write: function (newValue) {
                    self.activeDataType(newValue);
                    // If the new value is not null,
                    if (newValue) {
                        // Find a match,
                        var matchingTemplate = ko.utils.arrayFirst(self.templates(), function (template) {
                            return template.typeName === newValue.name();
                        });
                        // And set it to the selected template
                        self.selectedTemplate(matchingTemplate);   
                    } else {
                        // Or else if it is null set selected template to null
                        self.selectedTemplate(newValue);
                    }
                }
            });
			
			/**
			*	computed. returns the templates (data entry screens) that have any validation errors. 
			*	returns one message for each errored screen, excluding the current screen 
			*	(since on the current screen the detailed validation errors will show).
			*	@method dataEntryValidationErrors
			*/
			self.dataEntryValidationErrors = ko.computed(function(){
				//list the tabs that have errors, except the current tab
				var theseTypes = self.dataTypes();
				var currentTemplate = self.selectedTemplate();
				var tempArray = [];
				var isValid = false;
				if( currentTemplate ){
					isValid =  true;
					ko.utils.arrayForEach( self.templates(), function( template ){
						if( !template.isValid() ){
							isValid = false;
							if( currentTemplate.typeName !== template.typeName ){
								tempArray.push( {name: template.typeName, Message: 'errors in ' + template.typeName} );
							}
						}
					});
				}				
				self.canSave(isValid);				
				return tempArray;
			});
        };

        ctor.prototype.attached = function () {

        };

        return ctor;

		/**
		*	represents a screen (by data type) in the data entry modal.
		*	@class Template
		*/
        function Template(name, pathone, pathtwo) {
            var self = this;
            self.typeName = name;
            self.firstSection = new Section(pathone);
            self.secondSection = new Section(pathtwo);			
			self.isValid = ko.computed( function(){
				return ( self.firstSection.isValid() && self.secondSection.isValid() );
			});
			self.templateValidationErrors = ko.computed( function(){
				return self.firstSection.validationErrors().concat( self.secondSection.validationErrors() );
			});
        }

        function Section(path) {
            var self = this;
            self.path = path;
			self.isValid = ko.observable(true);
			self.validationErrors = ko.observableArray();
        }

    });