define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'config.models'],
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
        });

        notes.subscribe(function () {
            getNotes();
        });

        var groups = ko.observableArray();

        var activeNote = ko.observable();

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
        }

        function column(name, open, widgets) {
            var self = this;
            self.name = ko.observable(name);
            self.isOpen = ko.observable(open).extend({ notify: 'always' });
            self.isOpen.subscribe(function () {
                computedOpenColumn(self);
            });
            self.widgets = ko.observableArray();
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }

        var columns = ko.observableArray([
            new column('historyList', false, [{ name: 'History', path: 'patients/patientswidgets/historylistwidget.html', open: true }]),
            new column('details', false, [{ name: 'Details', path: 'patients/patientswidgets/historydetailwidget.html', open: true }])
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

        var vm = {
            activate: activate,
            selectedPatient: selectedPatient,
            columns: columns,
            computedOpenColumn: computedOpenColumn,
            activeNote: activeNote,
            setActiveNote: setActiveNote,
            setOpenColumn: setOpenColumn,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            groups: groups,
            notes: notes,
            attached: attached,
            compositionComplete: compositionComplete,
            title: 'index'
        };

        return vm;
        
        function getNotes() {
            if (selectedPatient()) {
                //datacontext.getEntityList(notes, null, null, 'patientId', selectedPatient().id(), false);
                var theseNotes = notes().slice(0).sort(descendingDateSort);
                theseNotes = theseNotes.slice(0, 25);
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