//TODO: Inject dependencies
define(['viewmodels/shell/shell', 'models/base', 'services/datacontext'],
    function (shell, modelConfig, datacontext) {

        var activeSecondColumn = ko.observable();

        var newPatient = ko.observable();
        var createModalShowing = ko.observable(false);
        // var isOpen = ko.observable(true);
        var isEditing = ko.observable(false);

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            activeSecondColumn: activeSecondColumn,
            deleteIndividual: deleteIndividual,
            createIndividual: createIndividual
        };

        return vm;
        
        function deleteIndividual () {
            activeSecondColumn('viewmodels/admin/widgets/delete.individuals.list');
        }
        function createIndividual () {
            datacontext.initializePatient(newPatient).then(indReturned);
            activeSecondColumn(null);

            function indReturned(data) {
                newPatient().isNew(true);
				newPatient().statusId('1'); //active										
				newPatient().statusDataSource('Engage');
				var modalSettings = {
					title: 'Create Individual',
					entity: newPatient, 
					templatePath: 'templates/patient.html', 
					showing: createModalShowing 
					//saveOverride, cancelOverride, deleteOverride, classOverride
				}
                var modal = new modelConfig.modal(modalSettings);
                shell.currentModal(modal);
                createModalShowing(true);
                var editingToken = createModalShowing.subscribe(function () {
                    isEditing(false);
                    editingToken.dispose();
                    // If it is a new patient that is a dupe and isn't force save,
                    var dupetoken = newPatient().isDuplicate.subscribe(function (newValue) {
                        if (newValue) {
                            createModalShowing(true);
                            var forcingToken = newPatient().isDuplicate.subscribe(function () {
                                isEditing(false);
                                forcingToken.dispose();
                                createModalShowing(false);
                                newPatient().entityAspect.acceptChanges();
                            });
                        }
                        dupetoken.dispose();
                    });
                });
            }
            // }
            // isEditing(!isEditing());
            // isOpen(true);
        };
        
        function activate() {
        }

    });