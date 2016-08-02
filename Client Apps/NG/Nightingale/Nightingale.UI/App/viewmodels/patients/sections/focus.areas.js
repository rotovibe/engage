define(['models/base', 'services/datacontext', 'services/session', 'viewmodels/shell/shell', 'viewmodels/patients/data/index'],
    function (modelConfig, datacontext, session, shell, dataIndex) {

        var ctor = function () {
        };

        ctor.prototype.alphabeticalSort = function (l, r) { return (l.name().toLowerCase() == r.name().toLowerCase()) ? (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) : (l.name().toLowerCase() > r.name().toLowerCase() ? 1 : -1) };
        
        function ModalEntity(patient) {
            var self = this;
            // TODO: Need to remove the modal from needing this property inherently
            self.activeDataType = ko.observable();
            self.selectedPatient = ko.observable(patient);
            self.showDropdown = true;
            self.showActions = false;
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: self.selectedPatient, activeDataType: self.activeDataType, showDropdown: self.showDropdown, showActions: self.showActions };
            // Create a computed property to subscribe to all of
            // the patients' observations and make sure they are
            // valid
            self.canSave = ko.computed(function () {
                var truthy = true;
                var issaving = datacontext.observationsSaving();
                var problemObservations = self.selectedPatient().observations();
                if (problemObservations.length === 0) { truthy = false; }
                ko.utils.arrayForEach(problemObservations, function (obs) {
                    // If any of the observations that are problems are not valid,
                    if (obs.type() && obs.type().name().toLowerCase() === 'problems' && !obs.isValid()) {
                        // Set the bool to false
                        truthy = false;
                    }
                });
                // Also make sure we aren't already saving or initializing observations
                if (datacontext.observationsSaving()) { truthy = false; }
                return truthy;
            });
        }

        ctor.prototype.activate = function (settings) {
            var self = this;
            //self.problems = settings.problems;
            self.selectedPatient = settings.selectedPatient;
            self.patientsObservations = self.selectedPatient.observations;
            // Compute a list of the patients observations that are problems to display
            self.problems = ko.computed(function () {
                // Set a local variable to hold observations
                var theseObservations = self.patientsObservations();
                // Filter the array by - 
                var filteredObservations = ko.utils.arrayFilter(theseObservations, function (item) {
                    // False by default
                    var truthy = false;
                    // If the observation has a type,
                    if (item.type()) {
                        // Check if it is a problem, don't want to hardcode but asked to do so
                        if (item.type().name().toLowerCase() === 'problems') {
                            // Check if the observation has a display and state
                            // and if it is active and display primary or secondary (greater than 0)
                            // and also make sure it isn't flagged for deletion : )
                            truthy = (item.displayId() && item.state() && item.displayId() > 0 && item.state().name().toLowerCase() === 'active' && !item.deleteFlag()) ? true : false;
                        }
                    }
                    return truthy;
                });
                return filteredObservations;
            }).extend({ throttle: 15 });
            self.isExpanded = ko.observable(false);
            // Create a list of primary problems to display in the widget
            self.primaryProblems = ko.computed(function () {
                // Create an empty array to fill with problems
                var theseProblems = [];
                var searchProblems = self.problems().sort(self.alphabeticalSort);
                // If not expanded limit to five
                var limitToFive = (!self.isExpanded());
                // Create a filtered list of problems,
                ko.utils.arrayForEach(searchProblems, function (problem) {
                    // If the observations display === 1 (primary),
                    if (problem.display().id() === '1') {
                        // If the length is less than 5 and it is not limited to 5 results,
                        if (theseProblems.length < 5 || !limitToFive) {
                            // Add it to the list of primary problems
                            theseProblems.push(problem);
                        }
                    }
                });
                return theseProblems;
            }).extend({ throttle: 15 });
            // Create a list of secondary problems to display in the widget
            self.secondaryProblems = ko.computed(function () {
                // Create an empty array to fill with problems
                var theseProblems = [];
                var searchProblems = self.problems().sort(self.alphabeticalSort);
                // If not expanded limit to five
                var limitToFive = (!self.isExpanded());
                // Create a filtered list of problems,
                ko.utils.arrayForEach(searchProblems, function (problem) {
                    // Where a problem level exists.  Need to change this to choose primary only later
                    if (problem.display().id() === '2') {
                        if (theseProblems.length < 5 || !limitToFive) {
                            theseProblems.push(problem);
                        }
                    }
                });
                return theseProblems;
            }).extend({ throttle: 15 });

            self.focusProblemModalShowing = ko.observable(false);
            self.saveProblemObservations = dataIndex.saveDataEntry;
            // TODO: Fix this cancel also
            self.cancelProblemObservations = function () {
                // Make a list of the observation of type problem,
                var problemObservations = ko.utils.arrayFilter(self.selectedPatient.observations(), function (item) {
                    // If the observation has a type that is problem, add it to the list
                    return (item.type() && item.type().name().toLowerCase() === 'problems');
                });
                var destroyThese = [];
                // Go through the problem observations,
                ko.utils.arrayForEach(problemObservations, function (obs) {
                    // If the observation is new,
                    if (obs.isNew()) {
                        // Set it as deleted
                        obs.deleteFlag(true);
                        obs.entityAspect.acceptChanges();
                        // Delete it from the manager
                        destroyThese.push(obs);
                    } else {
                        // If not, reject it's changes
                        obs.entityAspect.rejectChanges();
                    }
                });
                while (destroyThese.length > 0) {
                    var observation = destroyThese[0];
                    observation.entityAspect.setDeleted();
                    observation.entityAspect.acceptChanges();
                    destroyThese.splice(0, 1);
                };

            }
            self.modalEntity = ko.observable(new ModalEntity(self.selectedPatient));
			
            self.isOpen = ko.observable(true);
            self.isFullScreen = ko.observable(false);
            self.toggleEditing = function () {
				var modalSettings = {
					title: 'Focus Problems',
					showSelectedPatientInTitle: true,
					entity: self.modalEntity, 
					templatePath: 'viewmodels/templates/focusproblems', 
					showing: self.focusProblemModalShowing, 
					saveOverride: self.saveProblemObservations, 
					cancelOverride: self.cancelProblemObservations,
					classOverride: 'modal-lg'
				}
				self.modal = new modelConfig.modal(modalSettings);
                shell.currentModal(self.modal);
                self.focusProblemModalShowing(true);
                self.isOpen(true);
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });