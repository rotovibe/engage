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
                }
                // , {
                //     title: 'practicehome',
                //     subroute: 'viewmodels/home/population/index',
                //     content: 'Practice Home'
                // }
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
                }
                // , {
                //     title: 'practicehome',
                //     subroute: 'viewmodels/home/population/index',
                //     content: 'Practice Home'
                // }
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
        title: 'patients',
        nav: true,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_patients.png',
            content: 'Patients',
            pages: [
                {
                //    title: 'overview',
                //    subroute: 'viewmodels/patients/overview/index',
                //    content: 'Overview'
                //}, {
                    title: 'goals',
                    subroute: 'viewmodels/patients/goals/index',
                    content: 'Goals'
                }, {
                    title: 'careplan',
                    subroute: 'viewmodels/patients/careplan/index',
                    content: 'Plan'
                }, {
                    title: 'history',
                    subroute: 'viewmodels/patients/history/index',
                    content: 'History'
                }, {
                    title: 'data',
                    subroute: 'viewmodels/patients/data/index',
                    content: 'Data'
                }
            ]
        }
    }, {
        route: 'designer(/:id)',
        moduleId: 'exclmodules/designer/index',
        title: 'designer',
        nav: false,
        settings: {
            imageSource: '/NightingaleUI/Content/images/nav_programs.png',
            content: 'Program Designer',
            pages: [
                {
                    title: 'thinker',
                    subroute: 'exclmodules/designer/home/index',
                    content: 'Thinker'
                }
            ]
        }
    }];

    var routeConfig = {
        routes: routes
    };

    return routeConfig;
});