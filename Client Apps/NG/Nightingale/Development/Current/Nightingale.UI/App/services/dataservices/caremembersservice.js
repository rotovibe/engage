define(['services/session', 'config.services'],
    function (session, servicesConfig) {
        
        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var careMemberService = {
			getCareTeam: getCareTeam,
			saveCareTeam: saveCareTeam,
			saveCareTeamMember: saveCareTeamMember,
            saveCareMemberOld: saveCareMemberOld,
            deleteNote: deleteNote
        };
        return careMemberService;

		function saveCareTeam( manager, serializedCareTeam ){						
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }
            checkDataContext();
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contacts/' + serializedCareTeam.ContactId + '/CareTeams' , 'CareTeam');
			var method = 'POST';			
            if (serializedCareTeam) {
                var payload = {};
                payload.CareTeam = serializedCareTeam;
                payload = JSON.stringify(payload);
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    }).toType('CareTeam');                                
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
		}
		
		function saveCareTeamMember( manager, serializedCareMember, teamId, patientContactId ){
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }
            checkDataContext();
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 
							'Contacts/' + patientContactId + '/CareTeams/' + teamId +  '/CareTeamMembers' , 'CareMember');			
			var method = 'PUT';
			if( !teamId ){
				throw new Error("saveCareTeamMember must have [teamId]");
			}
			if( serializedCareMember.Id < 0 ){
				method = 'POST';								
			}
			else{		
				//UpdateCareTeamMemberRequest
				endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 
							'Contacts/' + patientContactId + '/CareTeams/' + teamId +  '/CareTeamMembers/' + serializedCareMember.Id , 'CareMember');
			}

            if (serializedCareMember) {
                var payload = {};
                payload.CareTeamMember = serializedCareMember;
                payload = JSON.stringify(payload);
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: method,
                        $encoding: 'JSON',
                        $data: payload
                    })
					.toType('CareMember');
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
		}
		
		function getCareTeam( manager, observable, patientContactId ){
			// If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

		    //Contacts/{ContactId}/CareTeams
			endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Contacts/' + patientContactId + '/CareTeams', 'CareTeam');			
                
			// Query to post the results
			var query = breeze.EntityQuery
				.from(endPoint.ResourcePath)
				.withParameters({
					$method: 'GET',
					$encoding: 'JSON'					
				}).toType('CareTeam');

            return manager.executeQuery(query).then(getCareTeamSucceeded).fail(getCareTeamFailed);	
			
			function getCareTeamSucceeded(data) {                
				if( data.results && data.results.length > 0 ){
					var s = data.results[0];
					if (observable) {
						return observable(s);
					} else {
						return s;
					}
				}
            }
		}
		
        function getCareTeamFailed(error) {
            checkDataContext();
            console.log('Error - ', error);            
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Get Care Team failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.alerts.push(thisAlert);
        }
		
		//this will be deprecated:
        // POST to the server, check the results for entities
        function saveCareMemberOld(manager, serializedCareMember, saveType) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + serializedCareMember.PatientId + '/CareMember/' + saveType, 'CareMember');

            // If there is a contact card,
            if (serializedCareMember) {

                // Create a payload from the JS object
                var payload = {};

                payload.CareMember = serializedCareMember;
                payload = JSON.stringify(payload);

                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    });
                
                //return query.execute().then(saveSucceeded).fail(postFailed);
                return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);
            }

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }

        function deleteNote(manager, note) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Patient/' + note.patientId() + '/Note/' + note.id() + '/Delete', 'Note');

            // Create a payload from the JS object
            var payload = {};

            payload = JSON.stringify(payload);

            // Query to post the results
            var query = breeze.EntityQuery
                .from(endPoint.ResourcePath)
                .withParameters({
                    $method: 'POST',
                    $encoding: 'JSON',
                    $data: payload
                });

            return manager.executeQuery(query).then(saveSucceeded).fail(postFailed);

            function saveSucceeded(data) {
                return data.httpResponse.data;
            }
        }

        function postFailed(error) {
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