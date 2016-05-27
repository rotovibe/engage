define(['services/session', 'services/datacontext', 'config.services', 'viewmodels/shell/shell', 'models/base', 'viewmodels/patients/index', 'viewmodels/patients/medications/index'],
    function (session, datacontext, servicesConfig, shell, modelConfig, patientsIndex, medicationsIndex) {

        var ctor = function () {
    		var self = this;
    		self.descendingDateSort = function (l, r) { return (l.dueDate() == r.dueDate()) ? (l.dueDate() < r.dueDate() ? 1 : -1) : (l.dueDate() < r.dueDate() ? 1 : -1) };
	        self.allergySort = function (l, r) {
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
    		new Column('type', 'Type','span3', 'type.name', true),
            new Column('type-small', 'Type','span2', 'type.name', true),
    		new Column('name', 'Allergy','span6 ellipsis', 'allergyName'),
            new Column('name-small', 'Allergy','span3 ellipsis', 'allergyName'),
            new Column('severity', 'Severity','span4 ellipsis', 'severity.name'),
            new Column('severity-small', 'Severity','span3 ellipsis', 'severity.name'),
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

        // Endpoint to use for getting the current patient's programs
        var patientProgramEndPoint = ko.computed(function () {
            if(!session.currentUser()) {
                return false;
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'Program');
        });

        ctor.prototype.activate = function (data) {
    		var self = this;
    		self.allergies = data.allergies;
			self.selectedPatient = data.selectedPatient;
            self.selectedSortColumn = data.selectedSortColumn;
            self.toggleSort = data.toggleSort;
            self.canSort = data.canSort ? data.canSort : false;
            self.activeAllergy = medicationsIndex.activeAllergy;
            self.saveOverride = function () {
                // Edit Existing Allergy: Save it if its valid. if not - cancel any chages (silently !)
				if ( self.modalEntity().allergy().isValid() ){
					datacontext.saveAllergies([self.modalEntity().allergy()], 'Update').then(saveCompleted);
				} else{
					self.cancelOverride()
				}

                function saveCompleted() {
                    self.modalEntity().allergy().isNew(false);
                    self.modalEntity().allergy().isUserCreated(false);
                    self.modalEntity().allergy().entityAspect.acceptChanges();
                }
            };
            self.cancelOverride = function () {
                datacontext.cancelEntityChanges(self.modalEntity().allergy());
                patientsIndex.getPatientsAllergies();
            };

			var modalSettings = {
				title: 'Edit Allergy',
				showSelectedPatientInTitle: true,
				entity: self.modalEntity,
				templatePath: 'viewmodels/templates/allergy.edit',
				showing: self.modalShowing,
				saveOverride: self.saveOverride,
				cancelOverride: self.cancelOverride,
				deleteOverride: null,
				classOverride: 'modal-lg'
			}
            self.modal = new modelConfig.modal(modalSettings);
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
            self.setActiveAllergy = function (allergy) {
                medicationsIndex.setActiveAllergy(allergy);
            };
    		self.editAllergy = function (allergy) {
                // Set the allergy
                self.modalEntity().allergy(allergy);
                // Set the current modal
                shell.currentModal(self.modal);
                // Show it
                self.modalShowing(true);
    		}
    		self.computedAllergies = ko.computed(function () {
    			ko.utils.arrayForEach(self.allergies(), function (td) {
                    // If there is no edit function
    				if (!td.edit) {
    					td.edit = function () {
    						self.editAllergy(td);
    					}
    				}
                    if (!td.setActiveAllergy) {
                        td.setActiveAllergy = function () {
                            self.setActiveAllergy(td);
                        }
                    }
                    // If there is no isExpanded property
                    if (!td.isExpanded) {
                        td.isExpanded = ko.observable(false);
                    }
    			});
                return self.allergies();
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
                self.path = 'views/templates/allergy.' + name.substr(0, name.length - 6) + '.html';
            } else {
                self.path = 'views/templates/allergy.' + name + '.html';
            }
    		self.cssClass = cssclass;
            self.sortProperty = sortprop;
            self.canSort = disablesort ? false : true;
    	}

    	return ctor;

        function ModalEntity(modalShowing) {
            var self = this;
            self.allergy = ko.observable();
            self.canSaveObservable = ko.observable(true);
            self.canSave = ko.computed({
                read: function () {
                    var allergyok = false;
                    if (self.allergy()) {
                        var allergytitle = !!self.allergy().allergyName();
                        var allergystatus = !!self.allergy().status();
                        allergyok = allergytitle && allergystatus;
                    }
                    return allergyok && self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { allergy: self.allergy, canSave: self.canSave, showing: modalShowing  };
        }

	});