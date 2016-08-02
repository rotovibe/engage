//Set the routes up in here

define([], function () {

    function Widget(title, widgetroute, content) {
        var self = this;
        self.title = title;
        self.widgetroute = widgetroute;
        self.content = content;
    }

    // The routes define the structure that will eventually be passed down by
    // the API to give routes to the client
    var routes = [{
        route: '',
        moduleId: 'viewmodels/home/index',
        title: 'home',
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_home.png',
            content: 'Home',
            pages: [
                {
                    title: 'myhome',
                    subroute: 'viewmodels/home/myhome/myhome',
                    content: 'My Home'
                }, {
                    title: 'todos',
                    subroute: 'viewmodels/home/todos/index',
                    content: 'To Do'
                }, {
					title: 'contacts',
					subroute: 'viewmodels/home/contacts/index',
					content: 'Contacts'
				}
            ]
        }
    }, {
        route: 'home',
        moduleId: 'viewmodels/home/index',
        title: 'home',
        nav: true,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_home.png',
            content: 'Home',
            pages: [
                {
                    title: 'myhome',
                    subroute: 'viewmodels/home/myhome/myhome',
                    content: 'My Home'
                }, {
                    title: 'todos',
                    subroute: 'viewmodels/home/todos/index',
                    content: 'To Do'
                }, {
					title: 'contacts',
					subroute: 'viewmodels/home/contacts/index',
					content: 'Contacts'
				}
            ]
        }
    }, {
        route: 'ut/(:tokenId)',
        moduleId: 'viewmodels/authenticate',
        title: 'authenticate',
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_population.png',
            content: 'Authenticating',
            pages: [{
                title: 'myhome',
                subroute: 'viewmodels/home/myhome/myhome',
                content: 'My Home'
            }]
        }
    }, {
        route: 'populations',
        moduleId: 'viewmodels/populations/index',
        title: 'populations',
        nav: false,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_population.png',
            content: 'Populations',
            pages: [
                {
                    title: 'careplan',
                    subroute: 'populations/careplan/index.html',
                    content: 'Plan'
                }, {
                    title: 'overview',
                    subroute: 'populations/overview/index.html',
                    content: 'Overview'
                }
            ]
        }
    }, {
        route: 'patients(/:id)',
        moduleId: 'viewmodels/patients/index',
        title: 'Individual',
        nav: true,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_patients.png',
            content: 'Individual',
            pages: [
                {
                   title: 'overview',
                   subroute: 'viewmodels/patients/overview/index',
                   content: 'Overview'
                }, {
                    title: 'goals',
                    subroute: 'viewmodels/patients/goals/index',
                    content: 'Goals'
                }, {
                    title: 'careplan',
                    subroute: 'viewmodels/patients/careplan/index',
                    content: 'Programs'
                }, {
                    title: 'history',
                    subroute: 'viewmodels/patients/history/index',
                    content: 'History'
				}, {
                    title: 'team',
                    subroute: 'viewmodels/patients/team/index',
                    content: 'Team'
                }, {
                    title: 'data',
                    subroute: 'viewmodels/patients/data/index',
                    content: 'Data'
                }, {
                    title: 'medications',
                    subroute: 'viewmodels/patients/medications/index',
                    content: 'Medications'
                }
            ]
        }
    }, {
        route: 'admin',
        moduleId: 'viewmodels/admin/index',
        title: 'admin',
        nav: true,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_admin.png',
            content: 'Admin',
            pages: [
                {
                    title: 'Concierge',
                    subroute: 'viewmodels/admin/concierge/index',
                    content: 'Concierge'
                }
            ]
        }
    }];

    var routeConfig = {
        routes: routes
    };

    return routeConfig;
});