//TODO: Inject dependencies
define(['plugins/router', 'services/navigation', 'services/session', 'config.services', 'services/datacontext'],
    function (router, navigation, session, servicesConfig, datacontext) {
        // Internal properties and functions

        var cohortPatientSelectPanelShowing = ko.observable(false);
        var cohorts = ko.observableArray();

        ko.extenders.calculateShowingPatients = function (target, value) {
            target.subscribe(function () {
                //console.log('Cohorts have checked or changed - ', cohorts());
            });
            return target;
        };

        var computedCohorts = ko.computed(function () {
            var theseCohorts = [];
            ko.utils.arrayForEach(cohorts(), function (cohort) {
                cohort.isChecked = ko.observable().extend({ calculateShowingPatients: true });
                theseCohorts.push(cohort);
            });
            return theseCohorts;
        });

        var cohortEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('10', session.currentUser().contracts()[0].number(), 'cohorts', 'Cohort');
        });

        // Reveal the bindable properties and functions
        var vm = {
            cohortPatientSelectPanelShowing: cohortPatientSelectPanelShowing,
            cohorts: cohorts,
            activate: activate,
            navigation: navigation,
            computedCohorts: computedCohorts,
            title: 'Home'
        };

        return vm;
        
        function activate() {
            getCohorts();
        }

        function getCohorts() {
            datacontext.getEntityList(cohorts, cohortEndPoint().EntityType, cohortEndPoint().ResourcePath, null, null, false);
        }
    });