define(['plugins/router', 'services/navigation', 'config.services', 'models/base', 'services/session', 'services/datacontext', 'viewmodels/patients/index', 'viewmodels/patients/data/index', 'viewmodels/shell/shell'],
    function (router, navigation, servicesConfig, modelConfig, session, datacontext, patientsIndex, dataIndex, shell) {
        var subscriptionTokens = [];

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

        var selectedPatientToken;

        var selectedPatient = ko.computed(function () {
            return patientsIndex.selectedPatient();
        });

        var selectedPatientsPrograms = ko.computed(function () {
            var thesePrograms = [];
            if (selectedPatient()) {
                thesePrograms = selectedPatient().programs();
                ko.utils.arrayForEach(thesePrograms, function (program) {
                    // If this is the first time we see this program,
                    if (!program.isOpen) {
                        // Give it an isOpen property
                        program.isOpen = ko.observable(false);
                        // If the program has already fetched modules,
                        if (program.modules().length !== 0) {
                            // Open the program
                            program.isOpen(true);
                        }
                    }
                });
            }
            return thesePrograms;
        });

        var alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
        var endDateSort = function (a,b) { var x = Date.parse(a.attrEndDate()); var y = Date.parse(b.attrEndDate()); if (x == y) { return 0; } if (isNaN(x) || x > y) { return -1; } if (isNaN(y) || x < y) { return 1; }}

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
			self.activationData = { selectedPatient: self.selectedPatient, activeDataType: self.activeDataType, showDropdown: self.showDropdown, showActions: self.showActions };
        }
        var modalShowing = ko.observable(false);
        var modalEntity = ko.observable(new ModalEntity());
        function saveOverride () {
            modalEntity().activeDataType(null);
            dataIndex.saveAllData();
        };
        function cancelOverride () {
            modalEntity().activeDataType(null);
            dataIndex.cancelDataEntry();
        };

		var modalSettings = {
			title: 'Data Entry',
			showSelectedPatientInTitle: true,
			entity: modalEntity, 
			templatePath: 'viewmodels/templates/clinical.dataentry', 
			showing: modalShowing, 
			saveOverride: saveOverride, 
			cancelOverride: cancelOverride, 
			deleteOverride: null, 
			classOverride: 'modal-lg'
		}
        var modal = new modelConfig.modal(modalSettings);
        function toggleModalShowing() {			
            shell.currentModal(modal);
            modalShowing(!modalShowing());
        }

        var openColumn = ko.observable();
        var widgets = ko.observableArray();

        var activeAction = ko.observable();
        var activeModule = ko.observable();
        var activeProgram = ko.observable();

        var activeWidgetWatcher = ko.computed(function () {
            var activeaction = activeAction();
            var activeprogram = activeProgram();
            var activemodule = activeModule();
            if (activeaction) {
                var thisAction = activeAction();
                if (thisAction && thisAction.steps().length === 0) {
					thisAction.isLoading(true);
                    getStepsForAction(thisAction).then( function(data){
						if( thisAction.steps().length > 0 ){
							thisAction.isLoading(false);
						}
					});
                }                    
            } else {
            }
        }).extend({ throttle: 25 });

        var initialized = false;

        var programEndPoint = ko.computed(function() {
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'program', 'Program');
        });

        var actionEndPoint = ko.computed(function() {
            if (activeAction() && selectedPatient()) {
                var thisAction = activeAction();
                return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + selectedPatient().id() + '/Program/' + thisAction.module().program().id() + '/Module/' + thisAction.module().id() + '/Action/' + thisAction.id(), 'Action');
            }
        });

        var genericActionEndPoint = ko.computed(function() {
            if (selectedPatient()) {
                return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient');
            }
        });

        function widget(data, column) {
            var self = this;
            self.name = ko.observable(data.name);
            self.path = ko.observable(data.path);
            self.isOpen = ko.observable(data.open);
            self.column = column;
            self.isFullScreen = ko.observable(false);
            self.elementStateIds = data.elementStateIds;
            self.sortOverride = data.sortOverride;
        }

        function column(name, open, widgets) {
            var self = this;
            self.name = ko.observable(name);
            self.isOpen = ko.observable(open).extend({ notify: 'always' });
            var ioToken = self.isOpen.subscribe(function() {
                computedOpenColumn(self);
            });
            subscriptionTokens.push(ioToken);
            self.widgets = ko.observableArray();
            $.each(widgets, function (index, item) {
                self.widgets.push(new widget(item, self))
            });
        }

        var planColumn = ko.observable(new column('carePlan', false, [{ name: 'Active Programs', path: 'patients/widgets/careplan.html', open: true, elementStateIds: [2,3,4], sortOverride: programsSortTwo }, { name: 'Program History', path: 'patients/widgets/careplan.html', open: false, elementStateIds: [1,5,6], sortOverride: endDateSort  }]));
        var detailsColumn = ko.observable(new column('actionDetails', false, [{ name: 'ActionDetails', path: 'viewmodels/patients/widgets/action.details', open: true }, { name: 'ModuleDetails', path: 'viewmodels/patients/widgets/module.details', open: true }, { name: 'ProgramDetails', path: 'viewmodels/patients/widgets/program.details', open: true }]));

        var computedOpenColumn = ko.computed({
            read: function () {
                return openColumn();
            },
            write: function (value) {
                // If this column is being set to closed
                if (!value.isOpen()) {
                    // Check if this is the open column and it's also the first column
                    if (value === openColumn() && value.name() === 'carePlan') {
                        // Set the open column to be the second column
                        openColumn(detailsColumn());
                    }
                        // Or else check if this is the open column
                    else if (value === openColumn()) {
                        // and Set the open column to be the first column
                        openColumn(planColumn());
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
            activeModule: activeModule,
            activeProgram: activeProgram,
            openColumn: openColumn,
            planColumn: planColumn,
            detailsColumn: detailsColumn,
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
            getStepsForAction: getStepsForAction,
            title: 'index',
            toggleWidgetOpen: toggleWidgetOpen
        };

        return vm;

        function activate() {
            // If we aren't already subscribed to this patient,
            if (!selectedPatientToken) {
            // Set a local instance of selectedPatient equal to the injected patient
                selectedPatientToken = selectedPatient.subscribe(function (newValue) {
                    // Need to go get programs for the selected patient whenever selected patient changes
                    //getProgramsForSelectedPatient();

                    // Clear out the active showing action or program column
                    activeAction(null);
                    activeModule(null);
                    activeProgram(null);
                });
                subscriptionTokens.push(selectedPatientToken);
            }
            openColumn(planColumn());
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
        }

        function detached() {
          isComposed(false);
          // selectedPatient.dispose();
          // selectedPatientsPrograms.dispose();
          // activeWidgetWatcher.dispose();
          // programEndPoint.dispose();
          // actionEndPoint.dispose();
          // genericActionEndPoint.dispose();
          // computedOpenColumn.dispose();
          // ko.utils.arrayForEach(subscriptionTokens, function (token) {
          //     token.dispose();
          // });
        }

        
        function getProgramsForSelectedPatient() {
            datacontext.getEntityList(null, programEndPoint().EntityType, programEndPoint().ResourcePath, null, null, false);
        }

        function getStepsForAction(action) {
            return datacontext.getEntityList(null, actionEndPoint().EntityType, genericActionEndPoint().ResourcePath + '/' + selectedPatient().id() + '/Program/' + action.module().program().id() + '/Module/' + action.module().id() + '/Action/' + action.id(), null, null, true);
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