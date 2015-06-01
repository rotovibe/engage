﻿/**
 * medications.panel module manages medication list and saving a medication from the data dialog
 *
 * @module medications.panel
 * @class medications.panel
 */

define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'viewmodels/patients/medications/index', 'durandal/app'],
    function (session, datacontext, servicesConfig, shell, modelConfig, medicationsIndex, app) {

        var ctor = function () {
    		var self = this;
    		self.descendingDateSort = function (l, r) { return (l.dueDate() == r.dueDate()) ? (l.dueDate() < r.dueDate() ? 1 : -1) : (l.dueDate() < r.dueDate() ? 1 : -1) };
	        self.medicationSort = function (l, r) {
	            // Primary sort property
	            var p1 = l.status() ? l.status().id() : '';
	            var p2 = r.status() ? r.status().id() : '';

	            // Secondary sort property
	            var o1 = l.name();
	            var o2 = r.name();
	            
	            if (p1 != p2) {
	                if (p1 < p2) return 1;
	                if (p1 > p2) return -1;
	                return 0;
	            }
	            if (o1 < o2) return 1;
	            if (o1 > o2) return -1;
	            return 0;
	        };
            self.modalShowing = ko.observable(false);
            self.modalEntity = ko.observable(new ModalEntity(self.modalShowing));
    	}

        // All the available columns
    	var allColumns = [
            new Column('expand', '', 'span1 ellipsis filters', '', true),
            new Column('priority', 'Priority', 'span2 ellipsis', 'priority.id'),
    		new Column('status', 'Status', 'span2 ellipsis', 'statusId'),
            new Column('priority-small', 'Priority', 'span1 ellipsis', 'priority.id'),
            new Column('status-small', '', 'span1 ellipsis', 'statusId'),
    		new Column('patient', 'Individual','span2 ellipsis', 'patientDetails.lastName'),
    		new Column('type', 'Type','span2', 'type.name', true),
            new Column('type-small', 'Type','span1', 'type.name', true),
    		new Column('name', 'Medication','span6 ellipsis', 'name'),
            new Column('name-small', 'Medication','span2 ellipsis', 'name'),
            new Column('strength', 'Strength','span3 ellipsis', 'strength'),
            new Column('strength-small', 'Strength','span2 ellipsis', 'strength'),
            new Column('reason', 'Reason','span4 ellipsis', 'reason.name'),
            new Column('reason-small', 'Reason','span3 ellipsis', 'reason.name'),
    		new Column('duedate', 'Due Date','span2 ellipsis', 'dueDate'),
    		new Column('assignedto', 'Assigned To','span2 ellipsis', 'assignedTo.preferredName'),
            new Column('startdate', 'Date','span2 ellipsis', 'startDate'),
            new Column('startdate-small', 'Date','span1 ellipsis', 'startDate'),
            new Column('updatedon', 'Date','span2 ellipsis', 'updatedOn'),
            new Column('updatedon-small', 'Date', 'span1 ellipsis', 'updatedOn')
    	];
        
        var patientEndPoint = ko.computed(function () {
            if(!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient', 'Patient');
        });
    	
        ctor.prototype.activate = function (data) {
    		var self = this;
    		self.medications = data.medications;
            self.selectedSortColumn = data.selectedSortColumn;
            self.toggleSort = data.toggleSort;
            self.canSort = data.canSort ? data.canSort : false;

            /**
            *   saveOverride saves a patient medication from edit mode (update only)
            *   using child module: medication.edit
            *   
            * @method saveOverride
            */
            self.saveOverride = function () {
                var medication = self.modalEntity().medication();
                datacontext.saveMedication(self.modalEntity().medication(), 'Update').then(saveCompleted);

                function saveCompleted() {
                    self.modalEntity().medication().isEditing(false);
                    self.modalEntity().medication().isNew(false);
                    self.modalEntity().medication().isUserCreated(false);
                    self.modalEntity().medication().entityAspect.acceptChanges();
                }
            };
                        
            self.cancelOverride = function () {
                self.modalEntity().medication().isEditing(false);
                datacontext.cancelEntityChanges(self.modalEntity().medication());
            };
            self.modal = new modelConfig.modal('Edit Medication', self.modalEntity, 'viewmodels/patients/sections/medication.edit', self.modalShowing, self.saveOverride, self.cancelOverride);
    		// A list of columns to display
    		self.columns = ko.computed(function () {
    			var tempcols = [];
    			var thesecols = data.columns();
    			ko.utils.arrayForEach(thesecols, function (col) {
    				var matchingCol = findColumnByName(col);
    				if (matchingCol) {
    					tempcols.push(matchingCol);
    				}
    			});
                // If no columns are found, show a default set of columns
                if (!tempcols) {
                    tempcols = ['status','patient','category','title','duedate'];
                }
    			return tempcols;
    		});
    		self.editMedication = function (medication) {
                medication.isEditing(true);
                // Set the medication
                self.modalEntity().medication(medication);
                // Set the current modal
                shell.currentModal(self.modal);
                // Show it
                self.modalShowing(true);
    		}
    		self.computedMedications = ko.computed(function () {
    			ko.utils.arrayForEach(self.medications(), function (td) {
                    // If there is no edit function
    				if (!td.edit) {
    					td.edit = function () {
    						self.editMedication(td);
    					}
    				}
                    // If there is no isExpanded property
                    if (!td.isExpanded) {
                        td.isExpanded = ko.observable(false);
                    } 
    			});
                return self.medications();
    		});
    	}

    	function findColumnByName(name) {
    		var match = ko.utils.arrayFirst(allColumns, function (allcol) {
    			return allcol.name.toLowerCase() === name.toLowerCase();
    		});
    		if (!match) {
    			console.log('bad column name used - ', name);
    		}
    		return match;
    	}

    	function Column(name, displayname, cssclass, sortprop, disablesort) {
    		var self = this;
    		self.name = name;
    		self.displayName = displayname;
            // If the name contains small,
            if (name.substr(name.length - 5, 5) === 'small') {
                self.path = 'views/templates/medication.' + name.substr(0, name.length - 6) + '.html';
            } else {
                self.path = 'views/templates/medication.' + name + '.html';                
            }
    		self.cssClass = cssclass;
            self.sortProperty = sortprop;
            self.canSort = disablesort ? false : true;
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.medication = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var medicationok = false;
                    if (self.medication()) {
                        var medicationtitle = !!self.medication().name();
                        var medicationstatus = !!self.medication().status();
                        medicationok = medicationtitle && medicationstatus;
                    }
                    return medicationok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: medicationsIndex.selectedPatient, medication: self.medication, canSave: self.canSave, showing: modalShowing  };
        }

	});