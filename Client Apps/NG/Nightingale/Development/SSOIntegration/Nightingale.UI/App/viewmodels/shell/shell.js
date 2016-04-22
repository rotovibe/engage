define(['plugins/router', 'services/navigation', 'services/session', 'services/bindings', 'services/local.collections', 'services/datacontext', 'models/base', 'config.routes', 'config.services', 'services/logger'],
	function (router, navigation, session, bindings, localCollections, datacontext, modelConfig, routeConfig, configServices, logger) {

    var alphabeticalNameSort = function (l, r) { return (l.name() == r.name()) ? (l.name() > r.name() ? 1 : -1) : (l.name() > r.name() ? 1 : -1) };

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
			showSideBar: showSideBar,
			showQuickAdd: showQuickAdd,
			compLoadingMessages: compLoadingMessages,
			loadingMessagesShowing: loadingMessagesShowing,
			currentModal: currentModal,
			toggleNavVisibility: toggleNavVisibility,
			toggleQuickAddShowing: toggleQuickAddShowing,
			activate: activate,
			goBack: goBack,
			navigation: navigation,
			session: session,
			title: 'Shell',

			alerts: ko.computed(datacontext.alerts),
			priorities: ko.computed(datacontext.enums.priorities),
			genders: ko.computed(datacontext.enums.genders),
			maritalStatuses: ko.computed(datacontext.enums.maritalStatuses),
			deceasedStatuses: ko.computed(function () { return datacontext.enums.deceasedStatuses().sort(alphabeticalNameSort) }),
			protectedStatuses: ko.computed(function () { return [{Id: false, Name: 'No'}, {Id: true, Name: 'Yes'}] }),
			stateList: ko.computed(datacontext.enums.states),
			phoneTypes: ko.computed(datacontext.enums.phoneTypes),
			emailTypes: ko.computed(datacontext.enums.emailTypes),
			daysOfWeek: ko.computed(datacontext.enums.daysOfWeek),
			timesOfDay: ko.computed(datacontext.enums.timesOfDay),
			patientStatuses: ko.computed(datacontext.enums.patientStatuses),
			patientStatusReasons: ko.computed(datacontext.enums.patientStatusReasons),
			languages: ko.computed(datacontext.enums.languages),
			timeZones: ko.computed(datacontext.enums.timeZones),
			addressTypes: ko.computed(datacontext.enums.addressTypes),
			communicationModes: ko.computed(datacontext.enums.communicationModes),

			setRoute: setRoute,
			userMenuOpen: userMenuOpen,
			initializeLogger: initializeLogger
		};

		return shell;

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