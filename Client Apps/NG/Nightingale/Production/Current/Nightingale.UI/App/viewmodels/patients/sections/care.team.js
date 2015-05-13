define(['models/base', 'services/datacontext', 'services/session', 'viewmodels/shell/shell'],
    function (modelConfig, datacontext, session, shell) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {

        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            // Get the selected patient that was passed in
            self.selectedPatient = settings.selectedPatient;
            // Get a list of all of the care team
            self.careMembers = self.selectedPatient.careMembers;
            // The view state of the section (open or not)
            self.isOpen = ko.observable(true);
            // Create a list of primary care team members to display in the widget
            self.primaryCareTeam = ko.computed(function () {
                // Create an empty array to fill with problems
                var thisCareTeam = [];
                // Sort the team
                var searchCareTeam = self.careMembers().sort(alphabeticalSort);
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // If they are a member of the primary care team,
                    if (careMember.primary()) {
                        // Add them to the team
                        thisCareTeam.push(careMember);
                    }
                });
                // Return the team
                return thisCareTeam;
            });
            // Create a list of secondary care team members
            self.secondaryCareTeam = ko.computed(function () {
                // Create an empty array to fill with problems
                var thisCareTeam = [];
                // Sort them
                var searchCareTeam = self.careMembers().sort(alphabeticalSort);
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // If they are not part of the primary care team,
                    if (!careMember.primary()) {
                        // Make them part of the secondary team
                        thisCareTeam.push(careMember);
                    }
                });
                return thisCareTeam;
            });
            self.saveType = ko.observable();
            self.saveOverride = function () { 
                // Get the current primary care manager
                var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                    return cmType.name() === 'Care Manager';
                });
                if (careMemberType) {
                    var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                        return caremanager.id() === session.currentUser().userId();
                    });
                    // Grab the first primary listed care member
                    var thisCareMember = ko.utils.arrayFirst(self.selectedPatient.careMembers(), function (ctMember) {
                        return ctMember.primary();
                    });
                    datacontext.saveCareMember(thisCareMember, self.saveType());
                }
            };
            self.cancelOverride = function () {
                datacontext.cancelEntityChanges(self.selectedPatient);
                ko.utils.arrayForEach(self.selectedPatient.careMembers(), function (cm) {
                    datacontext.cancelEntityChanges(cm);
                });
            };
            self.editModalShowing = ko.observable(false);
            self.modalEntity = ko.observable(new ModalEntity(self.selectedPatient, self.saveType));
            self.modal = new modelConfig.modal('Edit Care Team', self.modalEntity, 'viewmodels/templates/care.team.edit', self.editModalShowing, self.saveOverride, self.cancelOverride);
        };

        function ModalEntity(patient, savetype) {
            var self = this;
            self.selectedPatient = ko.observable(patient);
            self.saveType = savetype;
            // Create a computed property to subscribe to all of
            // the patients' observations and make sure they are
            // valid
            self.canSaveObservable = ko.observable(false);
            self.canSave = ko.computed({
                read: function () {
                    return self.canSaveObservable();
                },
                write: function (newValue) {
                    self.canSaveObservable(newValue);
                }
            });
            // Object containing parameters to pass to the modal
            self.activationData = { selectedPatient: self.selectedPatient, canSave: self.canSave, saveType: self.saveType };
        }

        ctor.prototype.canReassignToMe = function () {
            var self = this;
            if (self.primaryCareTeam().length > 0) {
                // var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                //     return caremanager.id() === session.currentUser().userId();
                // });
                return (self.primaryCareTeam().length > 0 && (self.primaryCareTeam()[0].contactId() !== session.currentUser().userId()));
            }
            return false;
        }

        ctor.prototype.assignToMe = function () {
            var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });
                var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient.id(), preferredName: thisMatchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: session.currentUser().userId() });
                datacontext.saveCareMember(thisCareMember, 'Insert');
            }
        };
        
        ctor.prototype.reassignToMe = function () {
            var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });
                // Grab the first primary listed care member
                var thisCareMember = ko.utils.arrayFirst(self.selectedPatient.careMembers(), function (ctMember) {
                    return ctMember.primary();
                });
                thisCareMember.preferredName(thisMatchedCareManager.preferredName());
                thisCareMember.gender('n');
                thisCareMember.contactId(thisMatchedCareManager.id());
                datacontext.saveCareMember(thisCareMember, 'Update');
            }
        };

        ctor.prototype.saveCareTeam = function (caremanagerid) {
            var careManagerId = caremanagerid ? caremanagerid : session.currentUser().userId();
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === careManagerId;
                });
                var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient.id(), preferredName: thisMatchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: careManagerId });
                datacontext.saveCareMember(thisCareMember, 'Insert');
            }
        }

        ctor.prototype.editCareTeam = function () {
            var self = this;
            self.editModalShowing(true);
            shell.currentModal(self.modal);
        }

        ctor.prototype.attached = function () {
        };

        return ctor;
    });