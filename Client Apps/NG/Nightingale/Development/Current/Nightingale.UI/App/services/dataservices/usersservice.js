define(['models/base', 'config.services', 'services/session'],
    function (modelConfig, serviceConfig, session) {

        var datacontext;

        var getSessionUser = function (manager, resource, userObservable, tokenId, forceRemote) {
            
            // Create a payload to post
            var payload = { Token: tokenId, APIKey: serviceConfig.apiAppKey, Context: 'NG' }

            // If there is a token id passed in from the URL string, get user that way
            if (tokenId) {
                // Query to post the results
                var query = breeze.EntityQuery
                    .from(resource)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    })
                    .toType('User');

                return manager.executeQuery(query).then(saveSucceeded).fail(loginFailed);
            }

            function saveSucceeded(data) {
                var user = data.results[0];
                // Make sure the user has been materialized
                return userObservable(user);

            }
        }

        var deleteSessionToken = function (token, resource) {

            var fullUrl;

            fullUrl = serviceConfig.securityServiceName + resource;

            // If there is a token id passed in from the URL string, get user that way
            if (token) {
                return $.ajax({
                    url: fullUrl,
                    cache: false,
                    headers: {
                        token: token
                    },
                    dataType: 'json',
                    type: "POST",
                    data: { Context: 'NG' },
                    success: function (data) {
                        return true;
                    }
                });
            }
        }

        var getUserSettings = function (resource, userObservable, apiToken) {

            // Go out and get a user's settings settings to load into the application

            var fullUrl;

            // If no resource is passed in, create a path to user settings for the end point
            if (!resource) {
                resource = 'usersettings';
            }

            fullUrl = serviceConfig.remoteServiceName + resource;
            
            // Make an ajax call to go get the user - create the users' settings on success
            $.ajax({
                url: fullUrl,
                cache: false,
                headers: {
                    Token: apiToken
                },
                dataType: 'json',
                type: "GET",
                success: function (data) {
                    // Push all of the settings that are returned into a settings object
					userObservable().settings(data.Settings);
                    return true;
                }
            }).fail(loginFailed);
        }

        var createUserFromSessionUser = function (manager, userData) {
            // Create a user to use as the current user
            var parameters = {};
            
            parameters.firstName = userData.firstName || '';
            parameters.lastName = userData.lastName || '';
            parameters.aPIToken = userData.aPIToken || '';
            parameters.userId = userData.userId || '';
            parameters.userName = userData.userName || '';
            parameters.contracts = userData.contracts || [];

            // Create and save the user
            var thisUser = manager.createEntity('User', parameters);
            thisUser.entityAspect.acceptChanges();
            return thisUser;
        }

        var userservice = {
            getSessionUser: getSessionUser,
            deleteSessionToken: deleteSessionToken,
            getUserSettings: getUserSettings,
            createUserFromSessionUser: createUserFromSessionUser
        };

        return userservice;

        function loginFailed(error) {
            checkDataContext();
            console.log('Error - ', error);

            // Check if the error status code is a 401
            if (error.status && error.status === 401) {
                // Log the user out
                session.logOff();
            }

            var thisAlert = datacontext.createEntity('Alert', { result: 'error', reason: 'Log-in failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });