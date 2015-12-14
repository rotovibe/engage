define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'models/base'],
    function (router, navigation, servicesConfig, session, datacontext, patientsIndex, shell, modelConfig) {

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var descendingDateSort = function (l, r) { return (l.createdOn() == r.createdOn()) ? (l.createdOn() < r.createdOn() ? 1 : -1) : (l.createdOn() < r.createdOn() ? 1 : -1) };

        var notes = ko.computed(function () {
            var theseNotes = [];
            if (selectedPatient()) {
                theseNotes = selectedPatient().notes();
            }
            return theseNotes;
        }).extend({ throttle: 1 });

        var isOpenToken = {}, notesToken = {};

        var notesToken = notes.subscribe(function () {
            getNotes();
        });

        var groups = ko.observableArray();
		var originalProgramIds = ko.observableArray([]);
        var activeNote = ko.observable();
		var activeNoteLoader = ko.computed(function(){
			var note = activeNote();
			if( note && note.type() && note.type().name().toLowerCase() === 'utilization' ){
				//load this note if the type is utilization
				datacontext.getNote( note.id(), note.patientId(), note.type().name() );
			}
		});
        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

        var openColumn = ko.observable();

        var initialized = false;

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.isOpen = ko.observable(data.open);
            self.column = column;
            self.isFullScreen = ko.observable(false);
			self.showEditButton = ko.computed( function(){
				var item = data.contentItem;	//reevaluate when the content entity changes
				var hasContentItem = data.contentItem? data.contentItem() : false;
				return data.showEdit? data.showEdit() : false;
			});
			self.showDeleteButton = ko.computed( function() {
				return data.showDelete? data.showDelete(): false;
			});
        }

        function column(name, open, widgets) {
            var self = this;
            self.name = ko.observable(name);
            self.isOpen = ko.observable(open).extend({ notify: 'always' });
            isOpenToken = self.isOpen.subscribe(function () {
                computedOpenColumn(self);
            });
            self.widgets = ko.observableArray();
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }
		function isShowEditButton(){			
			return activeNote && activeNote() && activeNote().isEditable();			
		}
        var columns = ko.observableArray([
            new column('historyList', false, [{ name: 'History', path: 'patients/widgets/history.list.html', open: true }]),
            new column('details', false, [{ name: 'Details', path: 'patients/widgets/history.detail.html', open: true, showEdit: isShowEditButton, showDelete: isShowEditButton, contentItem: activeNote }])
        ]);

        var computedOpenColumn = ko.computed({
            read: function () {
                return openColumn();
            },
            write: function (value) {
                // If this column is being set to closed
                if (!value.isOpen()) {
                    // Check if this is the open column and it's also the first column
                    if (value === openColumn() && value === columns()[0]) {
                        // Set the open column to be the second column
                        openColumn(columns()[1]);
                    }
                        // Or else check if this is the open column
                    else if (value === openColumn()) {
                        // and Set the open column to be the first column
                        openColumn(columns()[0]);
                    }
                }
                    // If it's being set to open, just set this column to be the open column
                else {
                    openColumn(value);
                }
            }
        });

		var noteModalShowing = ko.observable(true);

        var vm = {
            activate: activate,
            selectedPatient: selectedPatient,
            columns: columns,
            computedOpenColumn: computedOpenColumn,
            activeNote: activeNote,
            setActiveNote: setActiveNote,
			editClickFunc: editNote,
			deleteClickFunc: deleteNote,
            setOpenColumn: setOpenColumn,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            groups: groups,
            notes: notes,
            attached: attached,
            deactivate: deactivate,
            compositionComplete: compositionComplete,
            title: 'index'
        };

        return vm;

        function getNotes() {
            if (selectedPatient()) {
                var theseNotes = notes().slice(0).sort(descendingDateSort);
                theseNotes = theseNotes.slice(0, 100);
                groupNotes(theseNotes);
            }
        }

        function Group(name) {
            var self = this;
            self.Name = ko.observable(name);
            self.Notes = ko.observableArray();
        }

        function groupNotes(notes) {
            groups([]);
            ko.utils.arrayForEach(notes, function (note) {
                var thisLocalDate = note.localDate();
                if (moment(thisLocalDate).isValid()) {
                    var thisGroup = ko.utils.arrayFirst(groups(), function (group) {
                        return group.Name() === thisLocalDate;
                    });
                    if (!thisGroup) {
                        thisGroup = new Group(thisLocalDate);
                        groups.push(thisGroup);
                    }
                    thisGroup.Notes.push(note);
                }
            });
        }

        function activate() {
            openColumn(columns()[0]);
            getNotes();
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
        }

        function deactivate() {
            isOpenToken.dispose();
            notesToken.dispose();
        }

        function compositionComplete() {
            // After all of the composition is complete,
            // If there is an active note
            if (activeNote) {
                // Fire it's value to trigger resizing
                activeNote.valueHasMutated();
            }
        }

        function setActiveNote(sender) {
            activeNote(sender);
        }
		//edit note:
	    function ModalEntity(note) {
			var self = this;
			self.note = note;
			// Object containing parameters to pass to the modal
			self.activationData = { note: self.note };
			self.canSave = ko.computed(function () {
				return self.note.isValid();
			});
		}
		function editNote(sender){
			var modalEntity = ko.observable(new ModalEntity(activeNote()));
			var saveOverride = function () {
				var thisNote = modalEntity().note;
				// If there is new content,
				if (thisNote && thisNote.newContent()) {
					thisNote.checkAppend();
				}
			  datacontext.saveNote(thisNote).then( function(){
				  originalProgramIds.removeAll();
			  });
			};
			var cancelOverride = function () {
				var note = activeNote();
				note.entityAspect.rejectChanges();
				//revert to original program ids:
				note.programIds.removeAll();
				var progIds = note.programIds();
				if( originalProgramIds().length > 0){
					ko.utils.arrayPushAll(progIds, originalProgramIds());
					originalProgramIds.removeAll();
				}
				note.newContent('');
				//clear the entityAspect.entityState back to Unchanged state - to hide this correction (original program id's):
				note.entityAspect.setUnchanged();
			};
			var msg = 'Edit ' + activeNote().type().name() + ' Note';
			var modalSettings = {
				title: msg,
				showSelectedPatientInTitle: true,
				entity: modalEntity, 
				templatePath: 'viewmodels/patients/notes/index', 
				showing: noteModalShowing, 
				saveOverride: saveOverride, 
				cancelOverride: cancelOverride, 
				deleteOverride: null, 
				classOverride: null
			}
			var modal = new modelConfig.modal(modalSettings);

			//keep the original program ids
			originalProgramIds.removeAll();
			var progIds = originalProgramIds();
			ko.utils.arrayPushAll(progIds, activeNote().programIds());

			noteModalShowing(true);
			shell.currentModal(modal);
		}

		function deleteNote() {		
			var note = activeNote();
			var thistype = note.type().name();
			var result = confirm('You are about to delete a ' + thistype + ' note.  Press OK to continue, or cancel to return without deleting.');
			// If they press OK,
			if (result === true) {
				activeNote(null);
				datacontext.deleteNote( note );
			}
			else {
				return false;
			}
		}
		
        function setOpenColumn(sender) {
            openColumn(sender);
        }

        function minimizeThisColumn(sender) {
            sender.column.isOpen(false);
        }

        function maximizeThisColumn(sender) {
            sender.column.isOpen(true);
        }

        function toggleFullScreen(sender) {
            sender.isFullScreen(!sender.isFullScreen());
        }
    });