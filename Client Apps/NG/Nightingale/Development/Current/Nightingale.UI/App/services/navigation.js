define(['plugins/router', 'services/branding', 'services/logger'],
    function (router, branding, logger) {

        // The current route that shell has showing
        var currentRoute = ko.computed(function () {
            if (router.activeInstruction() && !router.isNavigating()) {
                return router.activeInstruction();
            }
            else { return false; }
        }).extend({ throttle: 1 });

        // The current sub route for the route that shell has showing
        var currentSubRoute = ko.observable();

        var indexOverride = ko.observable(0);

        var currentRouteWatcher = ko.computed(function () {
            var thisRoute = currentRoute();
            if (thisRoute && thisRoute.config && thisRoute.config.settings) {
                if (thisRoute.config.settings.pages.length !== 0) {
                    // Check if the sub route is already a page of the current route
                    //   so that we don't reset the page if so
                    if (thisRoute.config.settings.pages.indexOf(currentSubRoute()) === -1) {
                        setSubRoute(currentRoute().config.settings.pages[indexOverride()]);
                        //currentSubRoute(currentRoute().config.settings.pages[indexOverride()]);
                        indexOverride(0);
                    }
                }
            } else if (!currentSubRoute.peek() && currentRoute()) {
                setSubRoute(currentRoute().config.settings.pages[indexOverride()]);
                indexOverride(0);
            }
        }).extend({ throttle: 1 });

        // The currently loaded branding stuff (this is probably moving a style selector somehow
        var currentBrand = ko.computed(function () {
            return branding.currentBrand();
        });

        var navRoutes = ko.computed(function () {
            // Need to stuff this with the routes from the router
            return router.navigationModel();
        });

        var navigation = {
            currentBrand: currentBrand,
            currentRoute: currentRoute,
            currentSubRoute: currentSubRoute,
            setSubRoute: setSubRoute,
            navRoutes: navRoutes,
            setRoute: setRoute,
            indexOverride: indexOverride
        };

        return navigation;

        // Change the main route
        function setRoute(sender) {
            var thisRoute = sender.hash;
            // If the hash has an optional parameter,
            if (thisRoute.indexOf('(/:id)') !== -1) {
                thisRoute = thisRoute.replace('(/:id)', '');
            }
            if (thisRoute.indexOf('#') !== -1) {
                return router.navigate(thisRoute);
            }
            return router.navigate('#' + thisRoute);
        }

        // Change the 'sub' route
        function setSubRoute(sender) {
            // If the sent route is already the sub route, ignore it
            if (currentSubRoute() !== sender) {
                currentSubRoute(sender);
                logger.logNavigation(currentRoute().config.title + "/" + sender.title);
            }            
        }

    });