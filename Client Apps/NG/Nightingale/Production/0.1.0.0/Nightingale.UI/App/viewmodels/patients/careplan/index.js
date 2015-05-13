define(['plugins/router', 'services/navigation', 'config.services', 'config.models', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/patients/data/index', 'viewmodels/shell/shell'],
    function (router, navigation, servicesConfig, modelConfig, session, datacontext, patientsIndex, dataIndex, shell) {

        var programsSortTwo = function (l, r) {

            // Primary sort property
            var p1 = l.elementStateModel().order();
            var p2 = r.elementStateModel().order();

            // Secondary sort property
            var o1 = l.name().toLowerCase();
            var o2 = r.name().toLowerCase();
            
            if (p1 != p2) {
                if (p1 < p2) return -1;
                if (p1 > p2) return 1;
                return 0;
            }
            if (o1 < o2) return -1;
            if (o1 > o2) return 1;
            return 0;
        };

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var selectedPatientsPrograms = ko.computed(function () {
            var thesePrograms = [];
            if (selectedPatient()) {
                thesePrograms = selectedPatient().programs();
                thesePrograms.sort(programsSortTwo);
            }
            return thesePrograms;
        });

        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

        var programsSort = function (l, r) {
            return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1)
        };

        function ModalEntity() {
            var self = this;
            self.activeDataType = ko.observable();
            self.selectedPatient = selectedPatient;
            self.showDropdown = true;
            self.showActions = false;
            self.canSave = ko.observable(true);
        }
        var modalShowing = ko.observable(false);
        var modalEntity = ko.observable(new ModalEntity());
        function saveOverride () {
            modalEntity().activeDataType(null);
            dataIndex.saveDataEntry();
        };
        function cancelOverride () {
            modalEntity().activeDataType(null);
            dataIndex.cancelDataEntry();
        };
        var modal = new modelConfig.modal('Data Entry', modalEntity, 'viewmodels/templates/clinical.dataentry', modalShowing, saveOverride, cancelOverride);
        function toggleModalShowing() {
            shell.currentModal(modal);
            modalShowing(!modalShowing());
        }

        var openColumn = ko.observable();
        var widgets = ko.observableArray();

        var activeAction = ko.observable();
        var initialized = false;

        var programEndPoint = ko.computed(function() {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'program', 'Program');
        });

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
            self.isOpen.subscribe(function() {
                computedOpenColumn(self);
            });
            self.widgets = ko.observableArray();
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }

        var columns = ko.observableArray([
            new column('carePlan', false, [{ name: 'Plan', path: 'patients/patientswidgets/careplanwidget.html', open: true }]),
            new column('actionDetails', false, [{ name: 'Details', path: 'patients/patientswidgets/actionwidget.html', open: true}])
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
            attached: attached,
            activeAction: activeAction,
            openColumn: openColumn,
            columns: columns,
            widgets: widgets,
            computedOpenColumn: computedOpenColumn,
            selectedPatient: selectedPatient,
            selectedPatientsPrograms: selectedPatientsPrograms,
            programsSortTwo: programsSortTwo,
            alphabeticalOrderSort: alphabeticalOrderSort,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            modalShowing: modalShowing,
            toggleModalShowing: toggleModalShowing,
            title: 'index'
        };

        return vm;
        
        function activate() {
            // Set a local instance of selectedPatient equal to the injected patient
            selectedPatient.subscribe(function (newValue) {
                // Need to go get programs for the selected patient whenever selected patient changes
                //getProgramsForSelectedPatient();

                // Clear out the active showing action column
                activeAction(null);
            });
            openColumn(columns()[0]);
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
        }
        
        function getProgramsForSelectedPatient() {
            datacontext.getEntityList(null, programEndPoint().EntityType, programEndPoint().ResourcePath, null, null, false);
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