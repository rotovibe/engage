define(['services/session', 'config.services', 'services/entityfinder'],
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
			getContacts: getContacts,
			getLocalContacts: getLocalContacts,
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

		//assign values into complex type collection arrays
		function copyArray(source, dest){
			dest.removeAll();
			var copy = dest();				
			if( source().length > 0){
				ko.utils.arrayPushAll(copy, source());					
			}
		}
		
        function cancelAllChangesToContactCard(contactCard) {
            checkDataContext();
            //Cancel all changes to all levels of a contact card			
			if( contactCard.entityAspect.entityState.isAddedModifiedOrDeleted() ){
				contactCard.entityAspect.rejectChanges();

				if( !contactCard.isNew() ){
					//revert to the original complex type collections as rejectChanges wont do complex type rollback:
					
					// contactCard.contactSubTypes.removeAll();
					// var subTypes = contactCard.contactSubTypes();				
					// if( contactCard.originalContactSubTypes().length > 0){
						// ko.utils.arrayPushAll(subTypes, contactCard.originalContactSubTypes());					
					// }
					copyArray( contactCard.originalContactSubTypes, contactCard.contactSubTypes );
					copyArray( contactCard.originalPreferredTimesOfDayIds, contactCard.preferredTimesOfDayIds );
					copyArray( contactCard.originalPreferredDaysOfWeekIds, contactCard.preferredDaysOfWeekIds );
					copyArray( contactCard.originalLanguages, contactCard.languages );
					copyArray( contactCard.originalModes, contactCard.modes );
					copyArray( contactCard.originalEmails, contactCard.emails );
					copyArray( contactCard.originalPhones, contactCard.phones );
					copyArray( contactCard.originalAddresses, contactCard.addresses );
					//clear the entityAspect.entityState back to Unchanged state - to hide the complex type correction (original values):
					contactCard.entityAspect.setUnchanged();
				}
			}
			
			//this is not needed anymore:
            // if( !contactCard.isNew() && contactCard.patientId() ){								
				// //contactCard.entityAspect.setDetached();				
				// var patientId = contactCard.patientId();            
			
				// // Go get a list of contact cards for the currently selected patient
				// datacontext.getEntityList(contactCard, contactCardEndPoint().EntityType, contactCardEndPoint().ResourcePath + patientId + '/Contact', null, null, true);			
			// }
        }

		function getLocalContacts( manager, contactEntity, params ){
			// Make sure the datacontext has been loaded
            checkDataContext();
            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

			if( !contactEntity ){
				contactEntity = 'ContactCard';
			}
            // Create a base query
            var query = breeze.EntityQuery.from(contactEntity)
                .toType(contactEntity);

			if( params ){	
				// Create a predicate array
				var preds = [];

				// For each of the params,
				ko.utils.arrayForEach(params, function (param) {
					// Create a predicate
					var thispred = breeze.Predicate.create(param.Property, param.Operator, param.Value);
					// Add the predicate to the array of predicates
					preds.push(thispred);
				});

				// If there are predicates in the array,
				if (preds) {
					// Create a list of them
					var pred = breeze.Predicate.and(preds);
					query = query.where(pred);
				}
			}			
            return manager.executeQueryLocally(query);
		}
		
		function getContacts(manager, observable, params, observableTotalCount, entityName) {

		    // If there is no manager, we can't query using breeze
		    if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

		    // Check if the datacontext is available, if so require it
		    checkDataContext();

		    var payload = {
		        ContactTypeIds: params.contactTypeIds,
		        ContactSubTypeIds: params.contactSubTypeIds,
		        ContactStatuses: params.contactStatuses,
		        FirstName: params.firstName,
		        LastName: params.lastName,
		        FilterType: params.filterType ? params.filterType : null,	//'ExactMatch' / 'StartsWith' 
		        Take: params.take,
		        Skip: params.skip
		    };

		    if (!entityName) {
		        entityName = 'ContactCard';
		    }

		    endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'SearchContacts', entityName);
		    payload = JSON.stringify(payload);

		    // Query to post the results
		    var query = breeze.EntityQuery
				.from(endPoint.ResourcePath)
				.withParameters({
				    $method: 'POST',
				    $encoding: 'JSON',
				    $data: payload
				}).toType(entityName);

		    function searchSucceeded(data) {
		        var s = data.results;
		        if (observableTotalCount) {
		            var count = data.httpResponse.data.TotalCount;
		            if (count != undefined) {
		                observableTotalCount(count);
		            }
		        }
		        if (observable) {
		            observable(s);
		        } else {
		            return s;
		        }
		    }
			
		    return manager.executeQuery(query).then(searchSucceeded).fail(saveFailed);
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