﻿define(['plugins/router', 'services/navigation', 'services/session', 'services/bindings', 'services/local.collections', 'services/datacontext', 'models/base', 'config.routes', 'config.services', 'services/logger'],
    function (router, navigation, session, bindings, localCollections, datacontext, modelConfig, routeConfig, configServices, logger) {
       


        // Testing some new features, remove later
        var newFeaturesOpen = ko.observable(false);
        var featureMessages = ko.observableArray();
        var showingNewFeatures = ko.observable(false);
        // Check global vars for proper some engsettings
        if (window.engsettings) {
            var theseSettings = window.engsettings;
            if (theseSettings.featureMessages) {
                featureMessages(theseSettings.featureMessages);
            } else {
                featureMessages(theseSettings.featureMessages);
            }
            if (theseSettings.showNewFeatures && featureMessages() && featureMessages().length > 0) {
                showingNewFeatures(true);
            } else {
                showingNewFeatures(false);
            }
        }
        // Done

        // Internal properties and functions
       
        var showSideBar = ko.observable(false);
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
        var alerts = ko.computed(datacontext.alerts);
        var priorities = ko.computed(datacontext.enums.priorities);
        var genders = ko.computed(datacontext.enums.genders);
        var states = ko.computed(datacontext.enums.states);
        var phoneTypes = ko.computed(datacontext.enums.phoneTypes);
        var emailTypes = ko.computed(datacontext.enums.emailTypes); 
        var daysOfWeek = ko.computed(datacontext.enums.daysOfWeek);
        var timesOfDay = ko.computed(datacontext.enums.timesOfDay);
		var patientStatuses = ko.computed(datacontext.enums.patientStatuses);
		var patientStatusReasons = ko.computed(datacontext.enums.patientStatusReasons);			
        var languages = ko.computed(datacontext.enums.languages);
        var timeZones = ko.computed(datacontext.enums.timeZones);
        var addressTypes = ko.computed(datacontext.enums.addressTypes);
        var communicationModes = ko.computed(datacontext.enums.communicationModes);
        var token;

        // Show the quick add only when active route is patients
        var showQuickAdd = ko.computed(function () {
            if (navigation.currentRoute() !== false) {
                return navigation.currentRoute().config.title === 'Individual';
            }
            return false;
        });

        // Reveal the bindable properties and functions
        var shell = {
            // Test features
            newFeaturesOpen: newFeaturesOpen,
            featureMessages: featureMessages,
            showingNewFeatures: showingNewFeatures,
            // #end



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
			patientStatuses: patientStatuses,
			patientStatusReasons: patientStatusReasons,
            languages: languages,
            timeZones: timeZones,
            phoneTypes: phoneTypes,
            emailTypes: emailTypes,
            addressTypes: addressTypes,
            communicationModes: communicationModes,
            setRoute: setRoute,
            userMenuOpen: userMenuOpen,
            initializeLogger: initializeLogger
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
            token = navigation.currentRoute.subscribe(function () {
                showSideBar(false);
                // if (navigation.currentRoute() && navigation.currentRoute().config.settings) {
                //     if (navigation.currentRoute().config.settings.pages.length !== 0) {
                //         navigation.currentSubRoute(navigation.currentRoute().config.settings.pages[0]);
                //     }
                // }
            });
            if (!hasATokenId) {
                var foundUser = session.getUserFromSession();
                if (foundUser) {
                    return boot().then(function () {
                        initializeLogger();
                        //navigation.currentSubRoute(navigation.currentRoute().config.settings.pages[0]);
                        router.activate();
                        return true;
                    });
                }
            }
            // If no token id has been found, load authenticate by default
            router.activate();
            // If we got here, something went wrong so reload
        }

        function initializeLogger() {
            var gaId = configServices.gaId;
            var contractId = session.currentUser().contracts()[0].number();
            var userName = session.currentUser().userName();
            logger.initialize(gaId, contractId, userName)
        }

        function boot() {
            return datacontext.primeData();
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