define(['models/base'],
    function ( modelConfig) {

        var datacontext;
        var localStorageEnabled = false, cookiesEnabled = false;
        
        var currentUser = ko.observable();

        var session = {
            setLocalStorageAndCookie: setLocalStorageAndCookie,
            deleteCookie: deleteCookie,
            currentUser: currentUser,
            logOff: logOff,
            runTests: runTests,
            getUserFromSession: getUserFromSession
        };
        return session;

        function logOff() {
            var resource = '/1.0/' + currentUser().contracts()[0].number() + '/logout';
            checkDataContext();
            datacontext.logOutUserByToken(resource).then(finishLoggingOff);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

        function finishLoggingOff() {
            clearSession();
            window.location.href = "/Logout.aspx";
        }

        function clearSession() {
            clearLocalStorage();
            deleteCookie('ngSession');
            return true;
        }

        function setLocalStorageAndCookie() {
            // Set user to localStorage is possible
            if (localStorageEnabled) {
                var thisUser = ko.toJS(currentUser);
                thisUser = JSON.stringify(thisUser, ['userId', 'aPIToken', 'firstName', 'lastName', 'userName', 'aPIToken', 'contracts', 'id', 'name', 'number']);
                localStorage.setItem('ngSession', thisUser);
            }

            // Set a cookie for user if possible
            if (cookiesEnabled) {
                setCookie('ngSession', thisUser);
            }

            return true;
        }

        function clearLocalStorage() {
            localStorage.clear();
        }

        function setCookie(c_name, value) {
            var c_value = escape(value);
            document.cookie = c_name + "=" + c_value;
        }

        function getCookie(c_name) {
            var c_value = document.cookie;
            var c_start = c_value.indexOf(" " + c_name + "=");
            if (c_start == -1) {
                c_start = c_value.indexOf(c_name + "=");
            }
            if (c_start == -1) {
                c_value = null;
            }
            else {
                c_start = c_value.indexOf("=", c_start) + 1;
                var c_end = c_value.indexOf(";", c_start);
                if (c_end == -1) {
                    c_end = c_value.length;
                }
                c_value = unescape(c_value.substring(c_start, c_end));
            }
            return c_value;
        }

        function deleteCookie(c_name) {
            var name = c_name ? c_name : 'ngSession';
            document.cookie = name + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
        }

        function runTests() {

            // Test if local storage is enabled
            function lsTest() {
                var test = 'test';
                try {
                    localStorage.setItem(test, test);
                    localStorage.removeItem(test);
                    return true;
                } catch (e) {
                    return false;
                }
            }

            // Test if local storage is enabled
            function are_cookies_enabled() {
                var cookieEnabled = (navigator.cookieEnabled) ? true : false;

                if (typeof navigator.cookieEnabled == "undefined" && !cookieEnabled) {
                    document.cookie = "testcookie";
                    cookieEnabled = (document.cookie.indexOf("testcookie") != -1) ? true : false;
                }
                return (cookieEnabled);
            }

            localStorageEnabled = lsTest();
            cookiesEnabled = are_cookies_enabled();
            return { ls: localStorageEnabled, cookies: cookiesEnabled };
        }

        function getUserFromSession() {
            checkDataContext();
            runTests();
            if (localStorageEnabled) {
                // Check for a session in localStorage
                var retrievedItem = localStorage.getItem('ngSession');
                var user = JSON.parse(retrievedItem);
                // If an item is retrieved, try to get latest user by userId
                if (user && user.aPIToken) {
                    currentUser(datacontext.createUserFromSessionUser(user));
                    return currentUser();
                }
            }
            else if (cookiesEnabled) {
                var user = getCookie('ngSession');
                if (user && user.aPIToken) {
                    currentUser(datacontext.createUserFromSessionUser(user));
                    return currentUser();
                }
            }
        }
    });