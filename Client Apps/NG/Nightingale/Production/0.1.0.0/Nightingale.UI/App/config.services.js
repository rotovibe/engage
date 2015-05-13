// Configuration for modules
// and end point creation

define([],
    function () {

        // Create a route
        var route, apiAppKey;

        // If it is supposed to hit the model environment it will not have localhost nor :59922 in the href
        if (window.location.href.indexOf('localhost') === -1 && window.location.href.indexOf('dev') === -1 && window.location.href.indexOf('login.phytel') === -1) {
            route = '//azurephytel.cloudapp.net:59900';
            apiAppKey = '12345';
        }
        // Else if it is login.phytel.com,
        else if (window.location.href.indexOf('login.phytel') !== -1) {
            route = '//api.phytel.com';
            apiAppKey = '5273f88f69879b2f2c2fbeb3';
        }
        // Else hit the dev environment
        else {
            route = '//azurephyteldev.cloudapp.net:59900';
            apiAppKey = '12345';
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
            apiAppKey: apiAppKey
        };
        return datacontext;

    });