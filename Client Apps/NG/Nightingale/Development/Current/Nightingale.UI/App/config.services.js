// Configuration for modules
// and end point creation

define([],
    function () {

        // Create a route
        var route, apiAppKey, gaId;

        // If it is supposed to hit the model environment it will not have localhost nor :59922 in the href
        if (window.location.href.indexOf('localhost') === -1 && window.location.href.indexOf('dev') === -1 && window.location.href.indexOf('login.phytel') === -1 && window.location.href.indexOf('ngmodel.phytel') === -1 && window.location.href.indexOf('staging') === -1 && window.location.href.indexOf('nightingalelogin') === -1) {
            route = '//azurephytel.cloudapp.net:59900';
            apiAppKey = '12345';
            gaId = '';
            if (window.location.href.indexOf('azurephytel.cloudapp.net') !== -1) {            
                gaId = 'UA-44886803-5';                     //google analytics Engage QA (New) account 
                console.log('config.services - ENV = QA ');
            }            
        }
            // Else if it is phytel model
        else if (window.location.href.indexOf('ngmodel.phytel') !== -1) {
            route = '//mdlapi.phytel.com';
            apiAppKey = '12345';
            gaId = 'UA-44886803-6';
		console.log('config.services - ENV = MODEL ');
        }
            // Else if it is login.phytel.com,
        else if (window.location.href.indexOf('login.phytel') !== -1) {
            route = '//api.phytel.com';
            apiAppKey = '5273f88f69879b2f2c2fbeb3';
            gaId = 'UA-44886803-4'; //google analytics production account engage PROD (New)
            console.log('config.services - ENV = PROD ');
        }
            // Else if it is the staging environment,
        else if (window.location.href.indexOf('staging') !== -1) {
            route = '//azurephytelstaging.cloudapp.net:59900';
            apiAppKey = '12345';
            gaId = '';
        }
            // Else if it is the nightingale login environment,
        else if (window.location.href.indexOf('nightingalelogin') !== -1) {
            route = '//nightingalelogin.cloudapp.net:59900';
            apiAppKey = '12345';
            gaId = '';
        }        
        else {
            // Else hit the dev environment
            route = '//azurephyteldev.cloudapp.net:59900';
            apiAppKey = '12345';
            gaId = 'UA-44886803-3';	//google analytics Engage Dev (New) account
            console.log('config.services - ENV = DEV ');
        }

        // Create the paths we will expose
        var remoteServiceName = route + '/Nightingale';
        var securityServiceName = route + '/Security';

        // Create a common end point model
        function createEndPoint(version, contractNumber, name, entityType, params) {
            var self = this;
            self.ResourcePath = version + '/' + contractNumber + '/' + name + '/';
            self.EntityType = entityType;
            self.Parameters = params;
        }

        var datacontext = {
            remoteServiceName: remoteServiceName,
            securityServiceName: securityServiceName,
            createEndPoint: createEndPoint,
            apiAppKey: apiAppKey,
            gaId: gaId
        };
        return datacontext;

    });