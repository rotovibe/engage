define([], function () {

    // Function to capitalize the first letter of a string
    function capitalize(s) {
        return s[0].toUpperCase() + s.slice(1);
    }

    // Serialize an action to save it
    function serializeAction(action, manager) {
        // When the serialization started
        var startTime = new Date().getTime();

        console.log('Starting the serializer', startTime);
        
        // Create an object to hold the unwrapped JSON
        var thisAction = {};

        // Give this object a collection of Modules
        thisAction.Steps = [];

        // Give this action a collection of Spawn Elements
        thisAction.SpawnElement = [];

        var actionQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', action.id())
            .toType('Action')
            .select('id, name, moduleId, sourceId, order, completed, completedBy, enabled, status, elementState, dateCompleted, next, previous');
        var results = manager.executeQueryLocally(actionQuery);
        var unwrappedAction = results[0];

        unwrappedAction.steps = [];
        unwrappedAction.spawnElement = [];

        ko.utils.arrayForEach(action.spawnElement.peek(), function (spawnelement) {
            var unwrappedSpawnElement = { elementId: spawnelement.elementId.peek(), elementType: spawnelement.elementType.peek(), tag: spawnelement.tag.peek() };
            unwrappedAction.spawnElement.push(unwrappedSpawnElement);
        });

        // TODO: Foreach steps,
        ko.utils.arrayForEach(action.steps.peek(), function (step) {
            var stepQuery = breeze.EntityQuery
                .from('fakePath')
                .where('id', '==', step.id())
                .toType('Step')
                .select('id, header, description, title, text, question, notes, actionId, sourceId, stepTypeId, selectedResponseId, order, completed, enabled, status');
            var stepresults = manager.executeQueryLocally(stepQuery);
            var unwrappedStep = stepresults[0];
            unwrappedStep.responses = [];
            unwrappedStep.spawnElement = [];

            ko.utils.arrayForEach(step.spawnElement.peek(), function (spawnelement) {
                var unwrappedSpawnElement = { elementId: spawnelement.elementId.peek(), elementType: spawnelement.elementType.peek(), tag: spawnelement.tag.peek() };
                unwrappedStep.spawnElement.push(unwrappedSpawnElement);
            });

            ko.utils.arrayForEach(step.responses.peek(), function (response) {
                var responseQuery = breeze.EntityQuery
                    .from('fakePath')
                    .where('id', '==', response.id())
                    .toType('Response')
                    .select('id, text, required, value, stepId, nominal, nextStepId');
                var responseresponse = manager.executeQueryLocally(responseQuery);
                var unwrappedResponse = responseresponse[0];
                unwrappedResponse.spawnElement = [];

                ko.utils.arrayForEach(response.spawnElement.peek(), function (spawnelement) {
                    var unwrappedSpawnElement = { elementId: spawnelement.elementId.peek(), elementType: spawnelement.elementType.peek(), tag: spawnelement.tag.peek() };
                    unwrappedResponse.spawnElement.push(unwrappedSpawnElement);
                });

                unwrappedStep.responses.push(unwrappedResponse);
            });
            unwrappedAction.steps.push(unwrappedStep);
        });
        
        // Copy actions properties
        thisAction.Id = unwrappedAction.id;
        thisAction.Name = unwrappedAction.name;
        thisAction.ModuleId = unwrappedAction.moduleId;
        thisAction.SourceId = unwrappedAction.sourceId;
        thisAction.Order = unwrappedAction.order;
        thisAction.Completed = unwrappedAction.completed;
        thisAction.CompletedBy = unwrappedAction.completedBy;
        thisAction.Enabled = unwrappedAction.enabled;
        thisAction.Status = unwrappedAction.status;
        thisAction.ElementState = unwrappedAction.elementState;
        thisAction.DateCompleted = unwrappedAction.dateCompleted;
        thisAction.Next = unwrappedAction.next;
        thisAction.Previous = unwrappedAction.previous;

        // Copy any spawn element's that exist on this action
        ko.utils.arrayForEach(unwrappedAction.spawnElement, function (spawnElement) {
            thisAction.SpawnElement.push({ ElementId: spawnElement.elementId, ElementType: spawnElement.elementType, Tag: spawnElement.tag });
        });
        
        // Go through the steps,
        ko.utils.arrayForEach(unwrappedAction.steps, function (step) {

            var newStep = {};
            newStep.Responses = [];
            newStep.SpawnElement = [];

            ko.utils.arrayForEach(step.spawnElement, function (spawnElement) {
                newStep.SpawnElement.push({ ElementId: spawnElement.elementId, ElementType: spawnElement.elementType, Tag: spawnElement.tag });
            });

            // Copy steps properties
            newStep.Id = step.id;
            newStep.Header = step.header;
            newStep.Description = step.description;
            newStep.Title = step.title;
            newStep.Text = step.text;
            newStep.Question = step.question;
            newStep.Notes = step.notes;
            newStep.ActionId = step.actionId;
            newStep.SourceId = step.sourceId;
            newStep.StepTypeId = step.stepTypeId;
            newStep.SelectedResponseId = step.selectedResponseId;
            newStep.Order = step.order;
            newStep.Completed = step.completed;
            newStep.Enabled = step.enabled;
            newStep.Status = step.status;
            
            // Go through the steps,
            ko.utils.arrayForEach(step.responses, function (response) {

                var newResponse = {};

                newResponse.SpawnElement = [];

                ko.utils.arrayForEach(response.spawnElement, function (spawnElement) {
                    newResponse.SpawnElement.push({ ElementId: spawnElement.elementId, ElementType: spawnElement.elementType, Tag: spawnElement.tag })
                });
                
                // Copy responses properties
                newResponse.Id = response.id;
                newResponse.Text = response.text;
                newResponse.Required = response.required;
                newResponse.Value = response.value;
                newResponse.StepId = response.stepId;
                newResponse.Nominal = response.nominal;
                newResponse.nextStepId = response.nextStepId;

                newStep.Responses.push(newResponse);
            });        
            var incrementalTime = new Date().getTime() - startTime;

            thisAction.Steps.push(newStep);
        });

        var totalTime = new Date().getTime() - startTime;

        console.log('Done custom stringifying, it took this long to stringify (in milliseconds) - ', totalTime);

        return thisAction;
    }

    // Serialize a contact card to save it
    function serializeContactCard(contactCard, manager) {
        // When the serialization started
        var startTime = new Date().getTime();

        console.log('Starting the serializer', startTime);
        
        // Create an object to hold the unwrapped JSON
        var thisContactCard = {};

        // Give this object a collection of Stuff
        thisContactCard.Modes = [];
        thisContactCard.Addresses = [];
        thisContactCard.Phones = [];
        thisContactCard.Emails = [];
        thisContactCard.Languages = [];
        thisContactCard.TimesOfDaysId = [];
        thisContactCard.WeekDays = [];

        // Get the values of the properties of the action
        //var unwrappedContactCard = ko.toJS(contactCard);

        var contactCardQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', contactCard.id())
            .toType('ContactCard')
            .select('id, patientId, timeZoneId');
        var results = manager.executeQueryLocally(contactCardQuery);
        var unwrappedContactCard = results[0];

        unwrappedContactCard.steps = [];

        ko.utils.arrayForEach(contactCard.preferredTimesOfDayIds.peek(), function (tod) {
            thisContactCard.TimesOfDaysId.push(tod.id.peek());
        });

        ko.utils.arrayForEach(contactCard.preferredDaysOfWeekIds.peek(), function (dow) {
            thisDowId = parseInt(dow.id.peek());
            thisContactCard.WeekDays.push(thisDowId);
        });

        thisContactCard.PatientId = unwrappedContactCard.patientId;
        thisContactCard.Id = unwrappedContactCard.id;
        thisContactCard.TimeZoneId = unwrappedContactCard.timeZoneId;

        ko.utils.arrayForEach(contactCard.modes.peek(), function (mode) {
            var newMode = {};
            newMode.LookUpModeId = mode.lookUpModeId.peek();
            newMode.OptOut = mode.optOut.peek();
            newMode.Preferred = mode.preferred.peek();
            thisContactCard.Modes.push(newMode);
        });

        ko.utils.arrayForEach(contactCard.addresses.peek(), function (address) {
            var newAddress = {};
            newAddress.Id = address.id.peek();
            newAddress.OptOut = address.optOut.peek();
            newAddress.Preferred = address.preferred.peek();
            newAddress.TypeId = address.typeId.peek();
            newAddress.City = address.city.peek();
            newAddress.StateId = address.stateId.peek();
            newAddress.PostalCode = address.postalCode.peek();
            newAddress.Line1 = address.line1.peek();
            newAddress.Line2 = address.line2.peek();
            newAddress.Line3 = address.line3.peek();
            thisContactCard.Addresses.push(newAddress);
        });

        ko.utils.arrayForEach(contactCard.emails.peek(), function (email) {
            var newEmail = {};
            newEmail.Id = email.id.peek();
            newEmail.OptOut = email.optOut.peek();
            newEmail.Preferred = email.preferred.peek();
            newEmail.TypeId = email.typeId.peek();
            newEmail.Text = email.text.peek();
            thisContactCard.Emails.push(newEmail);
        });

        ko.utils.arrayForEach(contactCard.phones.peek(), function (phone) {
            var newPhone = {};
            newPhone.Id = phone.id.peek();
            newPhone.IsText = phone.isText.peek();
            newPhone.Number = phone.number.peek();
            newPhone.OptOut = phone.optOut.peek();
            newPhone.PhonePreferred = phone.phonePreferred.peek();
            newPhone.TextPreferred = phone.textPreferred.peek();
            newPhone.TypeId = phone.typeId.peek();
            thisContactCard.Phones.push(newPhone);
        });

        ko.utils.arrayForEach(contactCard.languages.peek(), function (language) {
            var newLanguage = {};
            newLanguage.LookUpLanguageId = language.lookUpLanguageId.peek();
            newLanguage.Preferred = language.preferred.peek();
            thisContactCard.Languages.push(newLanguage);
        });

        var totalTime = new Date().getTime() - startTime;

        console.log('Done custom stringifying, it took this long to stringify (in milliseconds) - ', totalTime);

        return thisContactCard;
    }

    // Serialize a goal to save it
    function serializeGoal(goal, manager) {
        // When the serialization started
        var startTime = new Date().getTime();

        console.log('Starting the serializer', startTime);

        // Create an object to hold the unwrapped JSON
        var thisGoal = {};

        // Give this object a collection of Stuff
        thisGoal.Tasks = [];
        thisGoal.Barriers = [];
        thisGoal.Interventions = [];
        thisGoal.FocusAreaIds = [];
        thisGoal.ProgramIds = [];
        thisGoal.CustomAttributes = [];

        var goalQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', goal.id())
            .toType('Goal')
            .select('id, name, patientId, sourceId, typeId, statusId, startDate, endDate, targetValue, targetDate');
        var results = manager.executeQueryLocally(goalQuery);
        var unwrappedGoal = results[0];

        // Copy any spawn element's that exist on this action
        ko.utils.arrayForEach(goal.focusAreaIds.peek(), function (fa) {
            thisGoal.FocusAreaIds.push(fa.id.peek());
        });

        ko.utils.arrayForEach(goal.programIds.peek(), function (pid) {
            thisGoal.ProgramIds.push(pid.id.peek());
        });

        ko.utils.arrayForEach(goal.customAttributes.peek(), function (customAttribute) {
            var newCustomAttribute = { Id: customAttribute.id.peek(), Name: customAttribute.name.peek(), ControlType: customAttribute.controlType.peek(), Order: customAttribute.order.peek() };
            newCustomAttribute.Options = [];
            newCustomAttribute.Values = [];
            ko.utils.arrayForEach(customAttribute.options.peek(), function (option) {
                newCustomAttribute.Options.push({ Value: option.value.peek(), Display: option.display.peek() });
            });
            ko.utils.arrayForEach(customAttribute.values.peek(), function (value) {
                newCustomAttribute.Values.push(value.value.peek());
            });
            thisGoal.CustomAttributes.push(newCustomAttribute);
        });

        thisGoal.Id = unwrappedGoal.id;
        thisGoal.Name = unwrappedGoal.name;
        thisGoal.PatientId = unwrappedGoal.patientId;
        thisGoal.SourceId = unwrappedGoal.sourceId;
        thisGoal.TypeId = unwrappedGoal.typeId;
        thisGoal.StatusId = unwrappedGoal.statusId;
        thisGoal.StartDate = unwrappedGoal.startDate;
        thisGoal.EndDate = unwrappedGoal.endDate;
        thisGoal.TargetValue = unwrappedGoal.targetValue;
        thisGoal.TargetDate = unwrappedGoal.targetDate;

        ko.utils.arrayForEach(goal.tasks.peek(), function (fulltask) {
            // Go get a projection query of this task
            var taskQuery = breeze.EntityQuery
                .from('fakePath')
                .where('id', '==', fulltask.id())
                .toType('Task')
                .select('id, description, statusId, targetValue, startDate, targetDate, patientGoalId');
            var results = manager.executeQueryLocally(taskQuery);
            var task = results[0];

            var newTask = {};
            newTask.BarrierIds = [];
            newTask.CustomAttributes = [];
            newTask.Id = task.id;
            newTask.Description = task.description;
            newTask.StatusId = task.statusId;
            newTask.TargetValue = task.targetValue;
            newTask.StartDate = task.startDate;
            newTask.TargetDate = task.targetDate;
            newTask.PatientGoalId = task.patientGoalId;
            ko.utils.arrayForEach(fulltask.barrierIds.peek(), function (barId) {
                newTask.BarrierIds.push(barId.id.peek());
            });
            ko.utils.arrayForEach(fulltask.customAttributes.peek(), function (customAttribute) {
                var newCustomAttribute = { Id: customAttribute.id.peek(), Name: customAttribute.name.peek(), ControlType: customAttribute.controlType.peek(), Order: customAttribute.order.peek() };
                newCustomAttribute.Options = [];
                newCustomAttribute.Values = [];
                ko.utils.arrayForEach(customAttribute.options.peek(), function (option) {
                    newCustomAttribute.Options.push({Value: option.value.peek(), Display: option.display.peek() });
                });
                ko.utils.arrayForEach(customAttribute.values.peek(), function (value) {
                    newCustomAttribute.Values.push(value.value.peek());
                });
                newTask.CustomAttributes.push(newCustomAttribute);
            });
            thisGoal.Tasks.push(newTask);
        });

        ko.utils.arrayForEach(goal.barriers.peek(), function (fullbarrier) {
            // Go get a projection query of this task
            var barrierQuery = breeze.EntityQuery
                .from('fakePath')
                .where('id', '==', fullbarrier.id())
                .toType('Barrier')
                .select('id, name, patientGoalId, statusId, categoryId');
            var results = manager.executeQueryLocally(barrierQuery);
            var barrier = results[0];

            var newBarrier = {};
            newBarrier.Id = barrier.id;
            newBarrier.Name = barrier.name;
            newBarrier.PatientGoalId = barrier.patientGoalId;
            newBarrier.StatusId = barrier.statusId;
            newBarrier.CategoryId = barrier.categoryId;
            thisGoal.Barriers.push(newBarrier);
        });

        ko.utils.arrayForEach(goal.interventions.peek(), function (fullintervention) {
            // Go get a projection query of this task
            var interventionQuery = breeze.EntityQuery
                .from('fakePath')
                .where('id', '==', fullintervention.id())
                .toType('Intervention')
                .select('id, categoryId, assignedToId, description, statusId, startDate, patientGoalId');
            var results = manager.executeQueryLocally(interventionQuery);
            var intervention = results[0];

            var newIntervention = {};
            newIntervention.BarrierIds = [];
            newIntervention.CustomAttributes = [];
            newIntervention.Id = intervention.id;
            newIntervention.CategoryId = intervention.categoryId;
            newIntervention.AssignedToId = intervention.assignedToId;
            newIntervention.Description = intervention.description;
            newIntervention.StatusId = intervention.statusId;
            newIntervention.StartDate = intervention.startDate;
            newIntervention.PatientGoalId = intervention.patientGoalId;
            ko.utils.arrayForEach(fullintervention.barrierIds.peek(), function (barId) {
                newIntervention.BarrierIds.push(barId.id.peek());
            });
            thisGoal.Interventions.push(newIntervention);
        });

        var totalTime = new Date().getTime() - startTime;
        console.log('Done custom stringifying, it took this long to stringify (in milliseconds) - ', totalTime);

        return thisGoal;
    }

    // Serialize an action to save it
    function serializeObservation(observation, manager) {
        // When the serialization started
        var startTime = new Date().getTime();
        
        // Create an object to hold the unwrapped JSON
        var thisObservation = {};
        thisObservation.Values = [];

        // Get the values of the properties of the action
        //var unwrappedObservation = ko.toJS(observation);

        var observationQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', observation.id())
            .toType('Observation')
            .select('id, name, startDate, units, patientId');
        var results = manager.executeQueryLocally(observationQuery);
        var unwrappedObservation = results[0];

        // Copy actions properties
        thisObservation.Id = unwrappedObservation.id;
        thisObservation.Name = unwrappedObservation.name;
        thisObservation.StartDate = unwrappedObservation.startDate;
        thisObservation.Units = unwrappedObservation.units;
        thisObservation.PatientId = unwrappedObservation.patientId;

        // Copy any spawn element's that exist on this action
        ko.utils.arrayForEach(observation.values.peek(), function (value) {
            thisObservation.Values.push({ Id: value.id.peek(), Text: value.text.peek(), Value: value.value.peek() });
        });

        var totalTime = new Date().getTime() - startTime;

        return thisObservation;
    }

    // Serialize a note to save it
    function serializeNote(note, manager) {
        // When the serialization started
        var startTime = new Date().getTime();

        console.log('Starting the serializer', startTime);

        // Create an object to hold the unwrapped JSON
        var thisNote = {};

        // Give this object a collection of Stuff
        thisNote.ProgramIds = [];

        // Get the values of the properties of the action
        //var unwrappedNote = ko.toJS(note);        

        var noteQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', note.id())
            .toType('Note')
            .select('id, text, patientId, createdOn, createdById');
        var results = manager.executeQueryLocally(noteQuery);
        var unwrappedNote = results[0];

        // Add the program ids
        ko.utils.arrayForEach(note.programIds.peek(), function (pid) {
            thisNote.ProgramIds.push(pid.id.peek());
        });
        
        thisNote.Id = unwrappedNote.id;
        thisNote.Text = unwrappedNote.text;
        thisNote.PatientId = unwrappedNote.patientId;
        thisNote.CreatedOn = unwrappedNote.createdOn;
        thisNote.CreatedById = unwrappedNote.createdById;
        
        var totalTime = new Date().getTime() - startTime;
        console.log('Done custom stringifying, it took this long to stringify (in milliseconds) - ', totalTime);

        return thisNote;
    }

    // Serialize a care member to save it
    function serializeCareMember(careMember, manager) {
        // When the serialization started
        var startTime = new Date().getTime();

        console.log('Starting the serializer', startTime);

        // Create an object to hold the unwrapped JSON
        var thisCareMember = {};

        // Get the values of the properties of the action
        // var unwrappedCareMember = ko.toJS(careMember);

        var careMemberQuery = breeze.EntityQuery
            .from('fakePath')
            .where('id', '==', careMember.id())
            .toType('CareMember')
            .select('id, patientId, contactId, typeId, primary, preferredName');
        var results = manager.executeQueryLocally(careMemberQuery);
        var unwrappedCareMember = results[0];

        thisCareMember.Id = unwrappedCareMember.id;
        // Don't send gender, as we don't actually know it yet
        //thisCareMember.Gender = unwrappedCareMember.gender;
        thisCareMember.PatientId = unwrappedCareMember.patientId;
        thisCareMember.ContactId = unwrappedCareMember.contactId;
        thisCareMember.TypeId = unwrappedCareMember.typeId;
        thisCareMember.Primary = unwrappedCareMember.primary;
        thisCareMember.PreferredName = unwrappedCareMember.preferredName;

        var totalTime = new Date().getTime() - startTime;
        console.log('Done custom stringifying, it took this long to stringify (in milliseconds) - ', totalTime);

        return thisCareMember;
    }

    var entitySerializer = {
        serializeAction: serializeAction,
        serializeContactCard: serializeContactCard,
        serializeGoal: serializeGoal,
        serializeObservation: serializeObservation,
        serializeNote: serializeNote,
        serializeCareMember: serializeCareMember
    }

    return entitySerializer;

});