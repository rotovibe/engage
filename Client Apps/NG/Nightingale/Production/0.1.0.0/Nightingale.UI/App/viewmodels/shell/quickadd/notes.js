define(['config.models', 'config.services', 'services/datacontext', 'services/session', 'services/navigation', 'viewmodels/patients/history/index'],
    function (modelConfig, servicesConfig, datacontext, session, navigation, historyIndex) {

        var ctor = function () {
            var self = this;
            // Note that is used when creating a new note
            self.newNote = ko.observable();
            self.isSaving = ko.observable();
        };

        ctor.prototype.contractProgramsEndPoint = ko.computed(function () {
            //var self = this;
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'programs/active', 'ContractProgram');
        });

        ctor.prototype.createNewNote = function () {
            var self = this;
            self.newNote(datacontext.createEntity('Note', { id: self.thisNoteId(), patientId: self.selectedPatient().id() }));
        };

        ctor.prototype.getContractProgramsByCategory = function () {
            var self = this;
            if (self.contractProgramsEndPoint()) {
                datacontext.getEntityList(self.contractPrograms, self.contractProgramsEndPoint().EntityType, self.contractProgramsEndPoint().ResourcePath, null, null, true, { ContractNumber: session.currentUser().contracts()[0].number() }, 'name');
            }
        };
        ctor.prototype.alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };
        ctor.prototype.alphabeticalDateSort = function (l, r) { return (l.createdOn() == r.createdOn()) ? (l.createdOn() < r.createdOn() ? 1 : -1) : (l.createdOn() < r.createdOn() ? 1 : -1) };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = settings.data.selectedPatient;
            self.thisNoteId = ko.computed(function () {
                return self.selectedPatient().notes().length * -1;
            });
            self.isShowing = self.settings.data.isShowing;
            self.cancel = function () {
                self.newNote().entityAspect.rejectChanges();
                self.createNewNote();
                self.isShowing(false);
            };
            self.availablePrograms = ko.computed(function () {
                var computedPrograms = [];
                if (self.selectedPatient()) {
                    var thesePrograms = self.selectedPatient().programs.slice(0).sort(self.alphabeticalNameSort);
                    ko.utils.arrayForEach(thesePrograms, function (program) {
                        if (program.elementState() !== 6 && program.elementState() !== 1 && program.elementState() !== 5) {
                            computedPrograms.push(program);
                        }
                    });
                }
                return computedPrograms;
            });
            self.startDate = ko.observable(new moment());
            self.save = function () {
                if (self.newNote()) {
                    self.newNote().patientId(self.selectedPatient().id());
                    self.isSaving(true);
                    // Find a care manager that matches this care manager
                    // var matchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (cm) {
                    //     return cm.id() === session.currentUser().userId();
                    // });
                    // if (matchedCareManager) {
                    //     '';
                    // }
                    // This should no longer be the UserID
                    self.newNote().createdById(session.currentUser().userId());
                    self.newNote().createdOn(new Date());
                    datacontext.saveNote(self.newNote()).then(noteSaved);
                    function noteSaved() {
                        self.isShowing(false);
                        self.newNote(null);
                        self.isSaving(false);
                        self.createNewNote();
                    }
                }
            };
            self.selectedPatient.subscribe(function () {
                self.cancel();
                historyIndex.activeNote(null);
            });
            self.viewDetails = function (sender) {
                self.isShowing(false);
                // Get the history subroute
                var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
                    return page.title === 'history';
                });
                navigation.setSubRoute(thisSubRoute);
                historyIndex.activeNote(sender);
            }
            self.gotoHistory = function () {
                // Get the history subroute
                self.isShowing(false);
                var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
                    return page.title === 'history';
                });
                navigation.setSubRoute(thisSubRoute);
            }
            self.canSave = ko.computed(function () {
                return self.newNote() && !self.isSaving() && self.newNote().text();
            });
            self.createNewNote();
            self.lastThreeNotes = ko.computed(function () {
                var theseNotes = self.selectedPatient().notes().sort(self.alphabeticalDateSort);
                var lastNotes = [];
                ko.utils.arrayForEach(theseNotes, function (note) {
                    if (lastNotes.length < 3 && !note.entityAspect.entityState.isAdded()) {
                        lastNotes.push(note);
                    }
                });
                return lastNotes;
            });
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });