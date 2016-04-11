﻿define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {

        var contactCardEndPoint = ko.computed(function () {
            var currentUser = session.currentUser();
            if (!currentUser) {
                return '';
            }
            return new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient', 'ContactCard');
        });

        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var contactService = {
            saveContactCard: saveContactCard,
            cancelAllChangesToContactCard: cancelAllChangesToContactCard
        };
        return contactService;

        // POST to the server, check the results for entities
        function saveContactCard(manager, serializedContactCard, isInsert) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();
            
            
            
            // If there is a contact card,
            if (serializedContactCard) {
				// Create an end point to use
				var endPoint;
				var method;
				if( isInsert ){
					//insert new
					method = 'POST';
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contacts', 'ContactCard');
				}
				else{
					//update
					method = 'PUT';
					endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contacts/' + serializedContactCard.Id, 'ContactCard');	
				}
				
                // Create a payload from the JS object
                var payload = {};

                payload.Contact = serializedContactCard;
                payload = JSON.stringify(payload);
                
                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: method,
                        $encoding: 'JSON',
                        $data: payload
                    });

                return manager.executeQuery(query).then(saveSucceeded).fail(saveFailed);
            }

            function saveSucceeded(data) {

                return data.httpResponse.data;
            }
        }

        function cancelAllChangesToContactCard(contactCard) {
            checkDataContext();
            // TODO : Cancel all changes to all levels of a contact card
			if( contactCard.entityAspect.entityState.isAddedModifiedOrDeleted() ){
				contactCard.entityAspect.rejectChanges();	
			}
			
            if( contactCard.id() > 0 ){
				contactCard.entityAspect.setDetached();
				if( contactCard.patient ){
					var patientId = contactCard.patient().id();            
				
					// Go get a list of contact cards for the currently selected patient
					datacontext.getEntityList(null, contactCardEndPoint().EntityType, contactCardEndPoint().ResourcePath + patientId + '/Contact', null, null, true);
				}
			}
        }

        function saveFailed(error) {
            checkDataContext();
            console.log('Error - ', error);            
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Save failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

    });