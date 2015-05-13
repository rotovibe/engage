define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'models/base', 'viewmodels/patients/data/index'],
    function (router, navigation, servicesConfig, session, datacontext, patientsIndex, shell, modelConfig, dataIndex) {
        
        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var descendingDateSort = function (l, r) { return (l.createdOn() == r.createdOn()) ? (l.createdOn() < r.createdOn() ? 1 : -1) : (l.createdOn() < r.createdOn() ? 1 : -1) };

        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

        var openColumn = ko.observable();

        var initialized = false;

        var myMedications = ko.computed(function () {
            if (selectedPatient()) {
                return selectedPatient().medications();
            }
            return [];
        });
        var medicationSaving = ko.computed(datacontext.medicationSaving);
        var selectedSortColumn = ko.observable();
        var activeMedicationColumns = ko.computed(function () {
            return ['expand','type-small','name-small','strength-small','reason','status'];
        });
        var toggleMedicationSort = function (sender) {
            // If the current column is the one to sort by
            if (self.selectedSortColumn() && self.selectedSortColumn().indexOf(sender.sortProperty) !== -1) {
                // If it ends in desc
                if (self.selectedSortColumn() && self.selectedSortColumn().substr(self.selectedSortColumn().length - 4, 4) === 'desc' ) {
                    // Clear the sort column, as it should be undone
                    self.selectedSortColumn(null);
                } else {
                    // Else set it as the sort column
                    self.selectedSortColumn(sender.sortProperty + ' desc');
                }
            } else {
                // Else set it as the new sort column
                self.selectedSortColumn(sender.sortProperty);
            }
        };

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.isOpen = ko.observable(data.open);
            self.column = column;
            self.isFullScreen = ko.observable(false);
            self.filtersOpen = ko.observable(true);
            self.activationData = { widget: self, selectedPatient: selectedPatient, defaultSort: data.defaultSort };
            self.allowAdd = data.allowAdd;
            self.statusIds = data.statusIds;
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
            new column('medications', false, [{ name: 'Active Medications', path: 'viewmodels/patients/widgets/medications', open: true, statusIds: [1], allowAdd: true }, { name: 'Medication History', path: 'viewmodels/patients/widgets/medications', open: true, statusIds: [2], defaultSort: 'endDate desc, name' }]),
            new column('allergies', false, [{ name: 'Allergies', path: 'viewmodels/patients/widgets/allergies', open: true, statusIds: [1] }, { name: 'Review', path: 'viewmodels/patients/widgets/allergies', open: false, statusIds: [2,3,4] }])
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
            myMedications: myMedications,
            medicationSaving: medicationSaving,
            selectedSortColumn: selectedSortColumn,
            activeMedicationColumns: activeMedicationColumns,
            toggleMedicationSort: toggleMedicationSort,

            activate: activate,
            selectedPatient: selectedPatient,
            columns: columns,
            computedOpenColumn: computedOpenColumn,
            setOpenColumn: setOpenColumn,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            attached: attached,
            title: 'index',
            toggleWidgetOpen: toggleWidgetOpen,
            addMedication: addMedication
        };

        return vm;
        
        function Group(name) {
            var self = this;
            self.Name = ko.observable(name);
            self.Notes = ko.observableArray();
        }

        function addMedication () {
            dataIndex.addData();
        };

        function activate() {
            openColumn(columns()[0]);
            //getNotes();
        }

        function attached() {
            if (!initialized) {
                initialized = true;
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

        // Force refresh todos from the server
        function refreshTodoView() {
            // Get the patients' todos
            patientsIndex.getPatientsAllergies();
        }

        // Toggle whether the widget is open or not
        function toggleWidgetOpen(sender) {
            // Find how many widgets are open
            var openwidgets = ko.utils.arrayFilter(sender.column.widgets(), function (wid) {
                return wid.isOpen();
            });
            // If the widget is the only open widget
            if (openwidgets.length === 1 && openwidgets[0] === sender) {
                // Do nothing
            } else {
                sender.isOpen(!sender.isOpen());
            }
        }
    });