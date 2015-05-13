define(['plugins/router', 'services/navigation', 'services/session', 'services/bindings', 'services/datacontext', 'config.models', 'config.routes'],
    function (router, navigation, session, bindings, datacontext, modelConfig, routeConfig) {
        // Internal properties and functions
       
        var showSideBar = ko.observable(false);
        var showQuickAdd = ko.observable(false);
        var loadingMessages = ko.computed(datacontext.loadingMessages);
        var compLoadingMessages = ko.computed(function () {
            var messages = loadingMessages();
            var theseMessages = [];
            ko.utils.arrayForEach(messages, function (message) {
                if (theseMessages.length < 3) {
                    theseMessages.push(message);
                }
            });
            return theseMessages;
        });
        var userMenuOpen = ko.observable(false);
        var loadingMessagesShowing = ko.computed(datacontext.loadingMessagesShowing);
        var currentModal = ko.observable();
        var alerts = ko.computed(datacontext.enums.alerts);
        var priorities = ko.computed(datacontext.enums.priorities);
        var genders = ko.computed(datacontext.enums.genders);
        var states = ko.computed(datacontext.enums.states);
        var phoneTypes = ko.computed(datacontext.enums.phoneTypes);
        var emailTypes = ko.computed(datacontext.enums.emailTypes); 
        var daysOfWeek = ko.computed(datacontext.enums.daysOfWeek);
        var timesOfDay = ko.computed(datacontext.enums.timesOfDay);
        var languages = ko.computed(datacontext.enums.languages);
        var timeZones = ko.computed(datacontext.enums.timeZones);
        var addressTypes = ko.computed(datacontext.enums.addressTypes);
        var communicationModes = ko.computed(datacontext.enums.communicationModes);
        var token;

        // Reveal the bindable properties and functions
        var shell = {
            showSideBar: showSideBar,
            showQuickAdd: showQuickAdd,
            compLoadingMessages: compLoadingMessages,
            loadingMessagesShowing: loadingMessagesShowing,
            currentModal: currentModal,
            toggleNavVisibility: toggleNavVisibility,
            toggleQuickAddShowing: toggleQuickAddShowing,
            activate: activate,
            goBack: goBack,
            attached: attached,
            navigation: navigation,
            session: session,
            title: 'Shell',
            alerts: alerts,
            priorities: priorities,
            genders: genders,
            stateList: states,
            daysOfWeek: daysOfWeek,
            timesOfDay: timesOfDay,
            languages: languages,
            timeZones: timeZones,
            phoneTypes: phoneTypes,
            emailTypes: emailTypes,
            addressTypes: addressTypes,
            communicationModes: communicationModes,
            setRoute: setRoute,
            userMenuOpen: userMenuOpen
        };

        return shell;

        function attached() {
        }

        function setRoute(sender) {
            showSideBar(false);
            var result = navigation.setRoute(sender);

            if (result) {
                console.log('The route has changed');
            }
            else {
                console.log('The route has NOT changed');
            }
        }

        function deactivate() {
            token.dispose();
        }

        function activate() {

            // Load all of the routes
            router.map(routeConfig.routes).buildNavigationModel();
            
            // Check if the route to go to is ut (authentication)
            var hasATokenId = (window.location.href.indexOf("#ut") > -1);
            if (!hasATokenId) {
                var foundUser = session.getUserFromSession();
                if (foundUser) {
                    boot();
                }
            }
            token = navigation.currentRoute.subscribe(function () {
                showSideBar(false);
                if (navigation.currentRoute() && navigation.currentRoute().config.settings) {
                    if (navigation.currentRoute().config.settings.pages.length !== 0) {
                        navigation.currentSubRoute(navigation.currentRoute().config.settings.pages[0]);
                    }
                }
            });
            router.activate();
            // If we got here, something went wrong so reload
        }

        function boot() {
            return datacontext.primeData().then(function () {
                // Navigate to the patients page first
                //return router.navigate('patients');
            });
        }
        
        function toggleNavVisibility() {
            showSideBar(!showSideBar());
        }

        function toggleQuickAddShowing() {
            showQuickAdd(!showQuickAdd());
        }        

        function closeModal(sender) {
            sender.Showing(false);
        }

        function goBack(complete) {
            router.navigateBack();
        }

        function genderType(letter, description) {
            var self = this;
            var letter = ko.observable(letter);
            var description = ko.observable(description);
        }
    });