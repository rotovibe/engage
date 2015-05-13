define(['plugins/router', 'services/navigation', 'config.services', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/shell/shell', 'config.models'],
    function (router, navigation, servicesConfig, session, datacontext, patientsIndex, shell, modelConfig) {

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var activeDataType = ko.observable();
        var dataTypes = ko.computed(datacontext.enums.observationTypes);

        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };

        var openColumn = ko.observable();
        var widgets = ko.observableArray();

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
            new column('dataType', false, [{ name: 'Data Type', path: 'patients/patientswidgets/datatypewidget.html', open: true }]),
            new column('dataEntry', false, [{ name: 'Details', path: 'patients/patientswidgets/dataentrywidget.html', open: true }])
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
            openColumn: openColumn,
            columns: columns,
            widgets: widgets,
            computedOpenColumn: computedOpenColumn,
            selectedPatient: selectedPatient,
            alphabeticalOrderSort: alphabeticalOrderSort,
            minimizeThisColumn: minimizeThisColumn,
            maximizeThisColumn: maximizeThisColumn,
            toggleFullScreen: toggleFullScreen,
            activeDataType: activeDataType,
            setActiveDataType: setActiveDataType,
            dataTypes: dataTypes,
            saveDataEntry: saveDataEntry,
            cancelDataEntry: cancelDataEntry,
            title: 'index'
        };

        return vm;

        function activate() {
            // Set a local instance of selectedPatient equal to the injected patient
            selectedPatient.subscribe(function (newValue) {
                datacontext.checkIfAllAdditionalObservationsAreLoadedYet(selectedPatient().id());
                activeDataType(null);
            });
            openColumn(columns()[0]);
        }

        function attached() {
            if (!initialized) {
                if (selectedPatient()) {
                    datacontext.checkIfAllAdditionalObservationsAreLoadedYet(selectedPatient().id());
                }
                initialized = true;
            }
        }

        function saveDataEntry() {
            // Go save the current patients' observations
            datacontext.saveObservations(selectedPatient().id()).then(saveCompleted);
            function saveCompleted() {
                // Clear out the standard observations
                var thisArray = selectedPatient().observations().slice(0);
                // Blow the entity out of the cache
                while (thisArray.length > 0) {
                    var observation = thisArray[0];
                    observation.entityAspect.setDeleted();
                    observation.entityAspect.acceptChanges();
                    thisArray.splice(0, 1);
                };
                // HACK : Trigger notify subscribers to go get more
                activeDataType.valueHasMutated();
            }
        }

        function cancelDataEntry() {
            var thisArray = selectedPatient().observations().slice(0);
            var destroyThese = [];
            //ko.utils.arrayForEach(thisArray, function (observation) {
            //    // HACK : 
            //    // Set the date to clear to allow the UI to clear the value 
            //    // in the date picker control
            //    observation.startDate('clear');
            //});
            // Go through each observation,
            ko.utils.arrayForEach(thisArray, function (observation) {
                // And cancel changes
                //observation.entityAspect.rejectChanges();
                // If the observation is not standard,
                //if (!observation.standard()) {
                    // Delete it from the manager
                    destroyThese.push(observation);
                //}
            });
            while (destroyThese.length > 0) {
                var observation = destroyThese[0];
                observation.entityAspect.setDeleted();
                observation.entityAspect.acceptChanges();
                destroyThese.splice(0, 1);
            };
            // Force the observations lists' to recalculate
            var thisActiveDataType = activeDataType();
            activeDataType(null);
            activeDataType(thisActiveDataType);
        }

        function setActiveDataType(sender) {
            activeDataType(sender);
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