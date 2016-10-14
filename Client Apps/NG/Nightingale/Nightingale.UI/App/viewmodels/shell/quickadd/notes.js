define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'services/navigation', 'viewmodels/patients/index', 'viewmodels/patients/history/index'],
	function (modelConfig, servicesConfig, datacontext, session, navigation, patientIndex, historyIndex) {

		var ctor = function () {
			var self = this;
				// Note that is used when creating a new note
				self.newNote = ko.observable();
				// Touchpoint note
				self.newTouchPoint = ko.observable();
				self.newUtilization = ko.observable();

				// Are we currently saving
				self.newTouchPointToken = {};
				self.isSaving = ko.observable();
				// Available note types
				self.noteTypes = datacontext.enums.noteTypes;
				// Find the touchpoint type,
				var touchpointType = ko.utils.arrayFirst(self.noteTypes(), function (type) {
					return type.name().toLowerCase() === 'touchpoint';
				});
				self.selectedNoteType = ko.observable();
				if (touchpointType) {
					self.selectedNoteType(touchpointType);
				} else {
					// Which note type should be showing
					self.selectedNoteType(self.noteTypes()[1]);
				}
				// Active note type path to show
				self.activeNoteType = ko.computed(function () {
					// Value to return
					var returnValue = '';
					// Get the type name
					var typename = self.selectedNoteType().name().toLowerCase();
					switch( typename ){
						case 'touchpoint':{
						// Show the touchpoint template
						returnValue = 'shell/quickadd/touchpoint.note.html';
						break;
						}
						case 'utilization':{
							returnValue = 'shell/quickadd/utilization.note.html';
							break;
						}
						default:{
							// Else always show the general
							returnValue = 'shell/quickadd/general.note.html';
							break;
						}
					}
				return returnValue;
			});
			// Available properties of a note
			self.methods = datacontext.enums.noteMethods;
			self.whos = datacontext.enums.noteWhos;
			self.sources = datacontext.enums.noteSources;
			self.outcomes = datacontext.enums.noteOutcomes;
			//utilization note lookups:
			self.visitTypes = datacontext.enums.visitTypes;
			self.utilizationSources = datacontext.enums.utilizationSources;
			self.dispositions = datacontext.enums.dispositions;
			self.utilizationLocations = datacontext.enums.utilizationLocations;

			// TODO: REMOVE THESE ONCE IS DEFAULT IS SET
			self.defaultOutcome = ko.utils.arrayFirst(self.outcomes(), function (outcome) {
				return outcome.isDefault();
			});
			self.defaultMethod = ko.utils.arrayFirst(self.methods(), function (method) {
				return method.isDefault();
			});
			self.defaultWho = ko.utils.arrayFirst(self.whos(), function (who) {
				return who.isDefault();
			});
			self.defaultSource = ko.utils.arrayFirst(self.sources(), function (source) {
				return source.isDefault();
			});
			self.defaultVisitType = ko.utils.arrayFirst(self.visitTypes(), function (visitType) {
				return visitType.isDefault();
			});
			self.defaultUtilizationSource = ko.utils.arrayFirst(self.utilizationSources(), function (utilizationSource) {
				return utilizationSource.isDefault();
			});
			self.defaultDisposition = ko.utils.arrayFirst(self.dispositions(), function (disposition) {
				return disposition.isDefault();
			});
			self.defaultUtilizationLocation = ko.utils.arrayFirst(self.utilizationLocations(), function (location) {
				return location.isDefault();
			});

			// Content area toggling for a touchpoint
			self.tpContentOpen = ko.observable(true);
			self.tpDetailsOpen = ko.observable(false);
			self.tpLastNotesOpen = ko.observable(false);
			// Content area toggling for a note
			self.gnContentOpen = ko.observable(true);
			self.gnLastNotesOpen = ko.observable(true);
			// Content area toggling for a utilization
			self.utContentOpen = ko.observable(true);
			self.utDetailsOpen = ko.observable(true);
			self.utLastNotesOpen = ko.observable(false);

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
			// Find a general note type id
			var generalNoteType = ko.utils.arrayFirst(self.noteTypes(), function (type) {
				return type.name().toLowerCase() === 'general';
			});
			// Set the newNote obs to a new note entity
			self.newNote(datacontext.createEntity('Note',
			{
				id: self.thisNoteId(),
				patientId: self.selectedPatient().id(),
				typeId: generalNoteType.id(),
				dataSource: "Engage"
			}));
			self.newNote().watchDirty();
		};

		ctor.prototype.createNewUtilization = function () {
			var self = this;
			// Find a utilization note type id
			var utilizationNoteType = ko.utils.arrayFirst(self.noteTypes(), function (type) {
				return type.name().toLowerCase() === 'utilization';
			});
			self.newUtilization(datacontext.createEntity('Note',
				{ 	id: self.thisUtilizationId(),
					patientId: self.selectedPatient().id(),
					admitted: false,
					typeId: utilizationNoteType.id(),
					visitType: self.defaultVisitType,
					utilizationSource: self.defaultUtilizationSource,
					disposition: self.defaultDisposition,
					utilizationLocation: self.defaultUtilizationLocation,
					dataSource: "Engage"
				}));
			self.newUtilization().watchDirty();
		};

		ctor.prototype.createNewTouchPoint = function () {
			var self = this;
			// Find a touchpoint note type id
			var touchpointNoteType = ko.utils.arrayFirst(self.noteTypes(), function (type) {
				return type.name().toLowerCase() === 'touchpoint';
			});
			self.newTouchPoint(datacontext.createEntity('Note',
			{
				id: self.thisTouchPointId(),
				patientId: self.selectedPatient().id(),
				contactedOn: new moment().format(),
				outcome: self.defaultOutcome,
				method: self.defaultMethod,
				source: self.defaultSource,				
				who: self.defaultWho,
				typeId: touchpointNoteType.id(),
				validatedIdentity: false,
				dataSource: "Engage"
			}));
			// If new touch points' date changes
			self.newTouchPointToken = self.newTouchPoint().contactedOn.subscribe(function (newValue) {
				// If there is no new value,
				if (!newValue) {
					// Reset to now
					var thisnow = new moment().format();
					setTimeout(function () {
						self.newTouchPoint().contactedOn(thisnow);
					}, 100);
				}
			});
			self.newTouchPoint().watchDirty();
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
			// What id should we use for the note (must be unique)
			self.thisNoteId = ko.computed(function () {
				return self.selectedPatient().notes().length * -1;
			});
			// What id should we use for the touchpoint note (must be unique)
			self.thisTouchPointId = ko.computed(function () {
				return self.thisNoteId() - 100;
			});
			// utilization note id
			self.thisUtilizationId = ko.computed(function () {
				return self.thisTouchPointId() - 100;
			});
			self.isShowing = self.settings.data.isShowing;

			self.cancelTouchPoint = function () {
			    if (self.newTouchPoint()) {
			        self.newTouchPoint().entityAspect.rejectChanges();
			        if (self.newTouchPointToken) {
			            self.newTouchPointToken.dispose();
			        }
			        self.createNewTouchPoint();			        
			    }
			    self.closePopupIfNoMoreChanges();
			}

			self.cancelUtilization = function () {
			    if (self.newUtilization()) {
			        self.newUtilization().entityAspect.rejectChanges();
			        self.createNewUtilization();
			    }
			    self.closePopupIfNoMoreChanges();
			}

			self.cancelNote = function () {
			    if (self.newNote()) {
			        self.newNote().entityAspect.rejectChanges();
			        self.createNewNote();
			    }
			    self.closePopupIfNoMoreChanges();
			}

			self.cancel = function () {
                /*
			    var typename = self.selectedNoteType().name().toLowerCase();
				
				if (typename == 'touchpoint' && self.newTouchPoint()) {
					self.newTouchPoint().entityAspect.rejectChanges();
					// If there is a new touch point subscription,
					if (self.newTouchPointToken) {
						// Dispose of it
						self.newTouchPointToken.dispose();
					}
					self.createNewTouchPoint();
				}
				else if (typename == 'utilization' && self.newUtilization()) {
					self.newUtilization().entityAspect.rejectChanges();
					self.createNewUtilization();
				}
				else if (self.newNote()) {
				    self.newNote().entityAspect.rejectChanges();
				    self.createNewNote();
				}
                */
			    self.cancelNote();
			    self.cancelTouchPoint();
			    self.cancelUtilization();
			};

			self.closePopupIfNoMoreChanges = function(){
			    if (!self.newNote().isDirty() && !self.newTouchPoint().isDirty() && !self.newUtilization().isDirty()) {
			        self.isShowing(false);
			    }
			}

			self.availablePrograms = ko.computed(function () {
				var computedPrograms = [];
				if (self.selectedPatient()) {
					var thesePrograms = self.selectedPatient().programs.slice(0).sort(self.alphabeticalNameSort);
					ko.utils.arrayForEach(thesePrograms, function (program) {
					    if (program.elementState() !== 1) {
					        if (program.elementState() != 5 && program.elementState() != 6) {
					            computedPrograms.push(program);					            
					        }
					        //5 or 6 within last 30 days
					        else {
					            var today = moment(new Date());
					            var stateUpdatedDate = moment(program.stateUpdatedOn());
					            if (today.diff(stateUpdatedDate, 'days') <= 30) {					                
					                computedPrograms.push(program);
					            }
					        }
						}
					});
				}
				return computedPrograms;
			});
			self.startDate = ko.observable(new moment());
			self.saveNote = function () {
				if (self.newNote()) {
					// Set the properties of the note before saving
					self.newNote().patientId(self.selectedPatient().id());
					self.isSaving(true);
					self.newNote().createdById(session.currentUser().userId());
					self.newNote().createdOn(new Date());
					// Make sure the selected note type is set
					self.newNote().typeId(self.selectedNoteType().id());
					datacontext.saveNote(self.newNote()).then(noteSaved);
					
				}
			};
			function noteSaved() {
				self.isShowing(false);
				self.newNote(null);
				self.isSaving(false);
				self.createNewNote();
			}
			self.saveTouchPoint = function () {
				function saved() {
					self.isShowing(false);
					self.newTouchPoint(null);
					self.isSaving(false);
					self.createNewTouchPoint();
				}
				if (self.newTouchPoint()) {
					self.newTouchPoint().patientId(self.selectedPatient().id());
					self.isSaving(true);
					self.newTouchPoint().createdById(session.currentUser().userId());
					self.newTouchPoint().createdOn(new Date());
					datacontext.saveNote(self.newTouchPoint()).then(saved);

				}
			};
			self.saveUtilization = function(){
				function saved() {
					self.isShowing(false);
					self.newUtilization().clearDirty();
					self.newUtilization(null);
					self.isSaving(false);
					self.createNewUtilization();
				}
				if (self.newUtilization()) {
					self.newUtilization().patientId(self.selectedPatient().id());
					self.isSaving(true);
					datacontext.saveNote(self.newUtilization()).then(saved);
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
			};
			self.gotoHistory = function () {
				// Get the history subroute
				self.isShowing(false);
				var thisSubRoute = ko.utils.arrayFirst(navigation.currentRoute().config.settings.pages, function (page) {
					return page.title === 'history';
				});
				navigation.setSubRoute(thisSubRoute);
			};
			self.canSave = ko.computed(function () {
				return self.newNote() && !self.isSaving() && self.newNote().isValid();
			});
			self.canSaveTouchPoint = ko.computed(function () {
				//subscribe to the condition variables: (this fixes a firefox issue)
				var hasNewTouchPoint = self.newTouchPoint()? true : false;
				var isSaving = self.isSaving();
				if(self.newTouchPoint()){					
					var text = self.newTouchPoint().text();
					var contactedOn = self.newTouchPoint().contactedOn();
				}
				return self.newTouchPoint() && !self.isSaving() && self.newTouchPoint().text() && self.newTouchPoint().contactedOn();
			});
			self.canSaveUtilization = ko.computed(function () {
				//subscribe to the condition variables: (this fixes a firefox issue)				
				var hasNewUtilization = self.newUtilization()? true : false;
				var isSaving = self.isSaving();				
				return self.newUtilization() && !self.isSaving() && self.newUtilization().isValid() && self.newUtilization().visitType();
			});
			self.createNewNote();
			self.createNewTouchPoint();
			self.createNewUtilization();
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
			patientIndex.leaving.subscribe(function (newValue) {
				if (newValue) {
					self.cancel();
				} else {
				}
			});
		};

		ctor.prototype.attached = function () {
		};

		return ctor;
	});  