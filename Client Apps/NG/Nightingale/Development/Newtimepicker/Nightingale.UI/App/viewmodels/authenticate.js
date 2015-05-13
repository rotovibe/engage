define(['services/session', 'config.routes', 'plugins/router', 'services/datacontext', 'viewmodels/shell/shell'],
    function (session, routeConfig, router, datacontext, shell) {

        var token;
        var tokenId;
        
        function activate(tokenIdFromUrl) {
            // Run tests to see if local storage and cookies are enabled
            session.runTests();
            var hasATokenId = (window.location.href.indexOf("#ut") > -1);
            if (hasATokenId) {
                // Replace the entry in browser history so the token isn't reused
                router.navigate('#', { replace: true, trigger: false });
                return getUserSessionByToken(tokenIdFromUrl);
            }
            return false;
        }

        var auth = {
            activate: activate
        }

        return auth;

        function boot() {
            return datacontext.primeData().then(function () {
                shell.initializeLogger();
                // Navigate to the patients page first
                return router.navigate('home');
            });
        }

        function getUserSessionByToken(tokenId) {
            // Use the data context to go get a user by the token in the query string
            datacontext.getUserByUserToken(session.currentUser, tokenId, true);
            token = session.currentUser.subscribe(function () {
                if (session.currentUser()) {
                    session.setLocalStorageAndCookie();
                    token.dispose();
                    return boot();
                }
            });
            // If no user is found, redirect to login page
            return false;
        }
    });