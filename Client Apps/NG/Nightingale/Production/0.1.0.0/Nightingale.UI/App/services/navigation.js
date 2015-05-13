define(['plugins/router', 'services/branding'],
    function (router, branding) {

        // The current route that shell has showing
        var currentRoute = ko.computed(function () {
            if (router.activeInstruction()) {
                return router.activeInstruction();
            }
            else { return true; }
        });

        // The current sub route for the route that shell has showing
        var currentSubRoute = ko.observable();

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
            setRoute: setRoute
        };

        return navigation;

        // Change the main route
        function setRoute(sender) {
            var thisRoute = sender.hash;
            // If the hash has an optional parameter,
            if (thisRoute.indexOf('(/:id)') !== -1) {
                thisRoute = thisRoute.replace('(/:id)', '');
            }
            return router.navigate('#' + thisRoute);
        }

        // Change the 'sub' route
        function setSubRoute(sender) {
            currentSubRoute(sender);
        }

    });