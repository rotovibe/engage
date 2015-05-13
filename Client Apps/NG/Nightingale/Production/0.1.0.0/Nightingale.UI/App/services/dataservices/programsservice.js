﻿define(['services/session', 'config.services', 'services/entityfinder'],
    function (session, servicesConfig, entityFinder) {
                
        // Create an object to act as the datacontext
        var datacontext;

        // Create an object to use to reveal functions from this module
        var programsService = {
            saveActionPost: saveActionPost,
            cancelChangesForNonComputedPath: cancelChangesForNonComputedPath,
            setCompletedSteps: setCompletedSteps
        };
        return programsService;

        function cancelChangesForNonComputedPath(action) {
            var thisComputedPath = action.computedPath();
            // Go through all steps in the action,
            ko.utils.arrayForEach(action.steps(), function (step) {
                // And if the step is not found in the computed path,
                if (thisComputedPath.indexOf(step) === -1) {
                    // Cancel changes to the step,
                    step.entityAspect.rejectChanges();
                    // And cancel changes to each response
                    ko.utils.arrayForEach(step.responses(), function (response) {
                        response.entityAspect.rejectChanges();
                    });
                }
            });
        }

        function setCompletedSteps(action) {
            var thisComputedPath = action.computedPath();
            // Go through all steps in the action,
            ko.utils.arrayForEach(action.steps(), function (step) {
                // And if the step is found in the computed path,
                if (thisComputedPath.indexOf(step) !== -1) {
                    // Cancel changes to the step,
                    step.completed(true);
                }
            });
        }

        // POST to the server, check the results for entities
        function saveActionPost(manager, serializedAction, programId, patientId) {

            // If there is no manager, we can't query using breeze
            if (!manager) { throw new Error("[manager] cannot be a null parameter"); }

            // Check if the datacontext is available, if so require it
            checkDataContext();

            // Which method should be called in the path
            var postMethod = serializedAction.Completed ? 'Process' : 'Save';

            // Create an end point to use
            var endPoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'patient/' + patientId + '/Program/Module/Action/' + postMethod, 'Program');

            // If there is as action
            if (serializedAction) {

                // Create a payload from the JS object
                var payload = {};

                payload.Action = serializedAction;
                payload.ProgramId = programId;
                payload = JSON.stringify(payload);
                
                // Query to post the results
                var query = breeze.EntityQuery
                    .from(endPoint.ResourcePath)
                    .withParameters({
                        $method: 'POST',
                        $encoding: 'JSON',
                        $data: payload
                    });

                return manager.executeQuery(query).then(saveSucceeded).fail(saveFailed);
            }

            function saveSucceeded(data) {
                console.log(data);
                entityFinder.searchForProblems(data.httpResponse.data);

                return true;
            }
        }

        function saveFailed() {
            checkDataContext();
            console.log('Error - ', error);            
            var thisAlert = datacontext.createEntity('Alert', { result: 0, reason: 'Save failed!' });
            thisAlert.entityAspect.acceptChanges();
            datacontext.enums.alerts.push(thisAlert);
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }

    });