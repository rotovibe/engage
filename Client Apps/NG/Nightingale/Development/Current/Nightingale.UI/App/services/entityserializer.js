define([],
	function () {

		// Function to capitalize the first letter of a string
		function capitalize(s) {
				return s[0].toUpperCase() + s.slice(1);
		}

		// Serialize an action to save it
		function serializeAction(action, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


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
						.select('id, archived, archiveOriginId, name, moduleId, sourceId, order, completed, enabled, status, deleteFlag, elementState, dateCompleted, next, previous, assignDate, assignById, assignToId, attrStartDate, attrEndDate, completedBy, stateUpdatedOn, description');
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
										.select('id, text, required, value, stepId, nominal, nextStepId, order');
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
		thisAction.DeleteFlag = unwrappedAction.deleteFlag;
				thisAction.ElementState = unwrappedAction.elementState;
				thisAction.StateUpdatedOn = unwrappedAction.stateUpdatedOn;
				thisAction.DateCompleted = unwrappedAction.dateCompleted;
				thisAction.Next = unwrappedAction.next;
				thisAction.Previous = unwrappedAction.previous;
				thisAction.AssignDate = unwrappedAction.assignDate;
				thisAction.AssignById = unwrappedAction.assignById;
				thisAction.AssignToId = unwrappedAction.assignToId;
				thisAction.Archived = unwrappedAction.archived;
				thisAction.ArchiveOriginId = unwrappedAction.archiveOriginId;

				thisAction.Description = unwrappedAction.description;
				thisAction.AttrStartDate = unwrappedAction.attrStartDate;
				thisAction.AttrEndDate = unwrappedAction.attrEndDate;
				thisAction.DateCompleted = unwrappedAction.dateCompleted;

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
								newResponse.NextStepId = response.nextStepId;
								newResponse.Order = response.order;

								newStep.Responses.push(newResponse);
						});
						var incrementalTime = new Date().getTime() - startTime;

						thisAction.Steps.push(newStep);
				});

				var totalTime = new Date().getTime() - startTime;


				return thisAction;
		}

		// Serialize a contact card to save it
		function serializeContactCard(contactCard, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


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
				thisContactCard.ContactSubTypes = [];

				// Get the values of the properties of the action
				//var unwrappedContactCard = ko.toJS(contactCard);

				var contactCardQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', contactCard.id())
						.toType('ContactCard')
						.select('id, patientId, timeZoneId, isPatient, userId, isUser, firstName, middleName, lastName, preferredName,'
							+ ' gender, contactTypeId, externalRecordId, dataSource, statusId, deceasedId, prefix, suffix, createdOn, updatedOn,'
							+ ' createdById, updatedById, externalId');
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
				thisContactCard.IsPatient		 = unwrappedContactCard.isPatient;
				thisContactCard.UserId           = unwrappedContactCard.userId;
				thisContactCard.IsUser           = unwrappedContactCard.isUser;
				thisContactCard.FirstName        = unwrappedContactCard.firstName;
				thisContactCard.MiddleName       = unwrappedContactCard.middleName;
				thisContactCard.LastName         = unwrappedContactCard.lastName;
				thisContactCard.PreferredName    = unwrappedContactCard.preferredName;
				thisContactCard.Gender           = unwrappedContactCard.gender;
				thisContactCard.ContactTypeId    = unwrappedContactCard.contactTypeId;
				thisContactCard.ExternalRecordId = unwrappedContactCard.externalRecordId;
				thisContactCard.DataSource       = unwrappedContactCard.dataSource;
				thisContactCard.StatusId         = unwrappedContactCard.statusId;
				thisContactCard.DeceasedId       = unwrappedContactCard.deceasedId;
				thisContactCard.Prefix           = unwrappedContactCard.prefix;
				thisContactCard.Suffix           = unwrappedContactCard.suffix;
				thisContactCard.CreatedOn        = unwrappedContactCard.createdOn;
				thisContactCard.UpdatedOn        = unwrappedContactCard.updatedOn;
				thisContactCard.CreatedById      = unwrappedContactCard.createdById;
				thisContactCard.UpdatedById      = unwrappedContactCard.updatedById;
				thisContactCard.ExternalId       = unwrappedContactCard.externalId;

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
						newPhone.Number = phone.number.peek().replace(/-/g, '');	//remove separators
						newPhone.OptOut = phone.optOut.peek();
						newPhone.PhonePreferred = phone.phonePreferred.peek();
						newPhone.TextPreferred = phone.textPreferred.peek();
						newPhone.TypeId = phone.typeId.peek();
						newPhone.DataSource = phone.dataSource.peek();
						thisContactCard.Phones.push(newPhone);
				});

				ko.utils.arrayForEach(contactCard.languages.peek(), function (language) {
						var newLanguage = {};
						newLanguage.LookUpLanguageId = language.lookUpLanguageId.peek();
						newLanguage.Preferred = language.preferred.peek();
						thisContactCard.Languages.push(newLanguage);
				});

				ko.utils.arrayForEach(contactCard.contactSubTypes.peek(), function (sub) {
					var subType = {};
					subType.Id = sub.id() ? sub.id() : null;
					subType.SubTypeId = sub.subTypeId();
					subType.SpecialtyId = sub.specialtyId() ? sub.specialtyId() : null;
					subType.SubSpecialtyIds = [];
					ko.utils.arrayForEach(sub.subSpecialtyIds.peek(), function (sid) {
						subType.SubSpecialtyIds.push(sid.id.peek());
					});
					thisContactCard.ContactSubTypes.push(subType);
				});

				var totalTime = new Date().getTime() - startTime;


				return thisContactCard;
		}

		// Serialize a goal to save it
		function serializeGoal(goal, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


				// Create an object to hold the unwrapped JSON
				var thisGoal = {};

				// Give this object a collection of Stuff
				// thisGoal.Tasks = [];
				// thisGoal.Barriers = [];
				// thisGoal.Interventions = [];
				thisGoal.FocusAreaIds = [];
				thisGoal.ProgramIds = [];
				thisGoal.CustomAttributes = [];

				var goalQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', goal.id())
						.toType('Goal')
						.select('id, name, patientId, sourceId, typeId, statusId, startDate, endDate, targetValue, targetDate, details');
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
				thisGoal.Details = unwrappedGoal.details;
				// ko.utils.arrayForEach(goal.tasks.peek(), function (fulltask) {
				//     // Go get a projection query of this task
				//     var taskQuery = breeze.EntityQuery
				//         .from('fakePath')
				//         .where('id', '==', fulltask.id())
				//         .toType('Task')
				//         .select('id, description, statusId, targetValue, startDate, targetDate, patientGoalId');
				//     var results = manager.executeQueryLocally(taskQuery);
				//     var task = results[0];

				//     var newTask = {};
				//     newTask.BarrierIds = [];
				//     newTask.CustomAttributes = [];
				//     newTask.Id = task.id;
				//     newTask.Description = task.description;
				//     newTask.StatusId = task.statusId;
				//     newTask.TargetValue = task.targetValue;
				//     newTask.StartDate = task.startDate;
				//     newTask.TargetDate = task.targetDate;
				//     newTask.PatientGoalId = task.patientGoalId;
				//     ko.utils.arrayForEach(fulltask.barrierIds.peek(), function (barId) {
				//         newTask.BarrierIds.push(barId.id.peek());
				//     });
				//     ko.utils.arrayForEach(fulltask.customAttributes.peek(), function (customAttribute) {
				//         var newCustomAttribute = { Id: customAttribute.id.peek(), Name: customAttribute.name.peek(), ControlType: customAttribute.controlType.peek(), Order: customAttribute.order.peek() };
				//         newCustomAttribute.Options = [];
				//         newCustomAttribute.Values = [];
				//         ko.utils.arrayForEach(customAttribute.options.peek(), function (option) {
				//             newCustomAttribute.Options.push({Value: option.value.peek(), Display: option.display.peek() });
				//         });
				//         ko.utils.arrayForEach(customAttribute.values.peek(), function (value) {
				//             newCustomAttribute.Values.push(value.value.peek());
				//         });
				//         newTask.CustomAttributes.push(newCustomAttribute);
				//     });
				//     thisGoal.Tasks.push(newTask);
				// });

				// ko.utils.arrayForEach(goal.barriers.peek(), function (fullbarrier) {
				//     // Go get a projection query of this task
				//     var barrierQuery = breeze.EntityQuery
				//         .from('fakePath')
				//         .where('id', '==', fullbarrier.id())
				//         .toType('Barrier')
				//         .select('id, name, patientGoalId, statusId, categoryId');
				//     var results = manager.executeQueryLocally(barrierQuery);
				//     var barrier = results[0];

				//     var newBarrier = {};
				//     newBarrier.Id = barrier.id;
				//     newBarrier.Name = barrier.name;
				//     newBarrier.PatientGoalId = barrier.patientGoalId;
				//     newBarrier.StatusId = barrier.statusId;
				//     newBarrier.CategoryId = barrier.categoryId;
				//     thisGoal.Barriers.push(newBarrier);
				// });

				// ko.utils.arrayForEach(goal.interventions.peek(), function (fullintervention) {
				//     // Go get a projection query of this task
				//     var interventionQuery = breeze.EntityQuery
				//         .from('fakePath')
				//         .where('id', '==', fullintervention.id())
				//         .toType('Intervention')
				//         .select('id, categoryId, assignedToId, description, statusId, startDate, patientGoalId');
				//     var results = manager.executeQueryLocally(interventionQuery);
				//     var intervention = results[0];

				//     var newIntervention = {};
				//     newIntervention.BarrierIds = [];
				//     newIntervention.CustomAttributes = [];
				//     newIntervention.Id = intervention.id;
				//     newIntervention.CategoryId = intervention.categoryId;
				//     newIntervention.AssignedToId = intervention.assignedToId;
				//     newIntervention.Description = intervention.description;
				//     newIntervention.StatusId = intervention.statusId;
				//     newIntervention.StartDate = intervention.startDate;
				//     newIntervention.PatientGoalId = intervention.patientGoalId;
				//     ko.utils.arrayForEach(fullintervention.barrierIds.peek(), function (barId) {
				//         newIntervention.BarrierIds.push(barId.id.peek());
				//     });
				//     thisGoal.Interventions.push(newIntervention);
				// });

				var totalTime = new Date().getTime() - startTime;

				return thisGoal;
		}

		// Serialize an iontervention to save it
		function serializeIntervention(intervention, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


				// Go get a projection query of this task
				var interventionQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', intervention.id())
						.toType('Intervention')
						.select('id, categoryId, assignedToId, description, statusId, startDate, dueDate, patientGoalId, patientId, closedDate, deleteFlag, details');
				var results = manager.executeQueryLocally(interventionQuery);
				var thisIntervention = results[0];

				var newIntervention = {};
				newIntervention.BarrierIds = [];
				newIntervention.CustomAttributes = [];
				newIntervention.Id = thisIntervention.id;
				newIntervention.CategoryId = thisIntervention.categoryId;
				newIntervention.AssignedToId = thisIntervention.assignedToId;
				newIntervention.Description = thisIntervention.description;
				newIntervention.StatusId = thisIntervention.statusId;
				newIntervention.StartDate = thisIntervention.startDate;
				newIntervention.DueDate = thisIntervention.dueDate;
				newIntervention.ClosedDate = thisIntervention.closedDate;
				newIntervention.PatientGoalId = thisIntervention.patientGoalId;
				newIntervention.DeleteFlag = thisIntervention.deleteFlag;
				newIntervention.Details = thisIntervention.details;
				// newIntervention.PatientId = thisIntervention.patientId;
				ko.utils.arrayForEach(intervention.barrierIds.peek(), function (barId) {
						newIntervention.BarrierIds.push(barId.id.peek());
				});

				var totalTime = new Date().getTime() - startTime;

				return newIntervention;
		}

		// Serialize a task to save it
		function serializeTask(task, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


				// Go get a projection query of this task
				var taskQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', task.id())
						.toType('Task')
						//.select('id, categoryId, assignedToId, description, statusId, startDate, patientGoalId, patientId');
						.select('id, description, statusId, targetValue, startDate, targetDate, patientGoalId, patientId, closedDate, statusDate, deleteFlag, details');
				var results = manager.executeQueryLocally(taskQuery);
				var thisTask = results[0];

				var newTask = {};
				newTask.BarrierIds = [];
				newTask.CustomAttributes = [];
				newTask.Id = thisTask.id;
				newTask.Description = thisTask.description;
				newTask.StatusId = thisTask.statusId;
				newTask.TargetValue = thisTask.targetValue;
				newTask.StartDate = thisTask.startDate;
				newTask.TargetDate = thisTask.targetDate;
				newTask.ClosedDate = thisTask.closedDate;
				newTask.StatusDate = thisTask.statusDate;
				newTask.PatientGoalId = thisTask.patientGoalId;
				//newTask.PatientId = thisTask.patientId;
				newTask.DeleteFlag = thisTask.deleteFlag;
				newTask.Details = thisTask.details;
				ko.utils.arrayForEach(task.barrierIds.peek(), function (barId) {
						newTask.BarrierIds.push(barId.id.peek());
				});
				ko.utils.arrayForEach(task.customAttributes.peek(), function (customAttribute) {
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

				var totalTime = new Date().getTime() - startTime;

				return newTask;
		}

		// Serialize a barrier to save it
		function serializeBarrier(barrier, manager) {

				// Go get a projection query of this task
				var barrierQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', barrier.id())
						.toType('Barrier')
						.select('id, name, patientGoalId, statusId, categoryId, deleteFlag, details');
				var results = manager.executeQueryLocally(barrierQuery);
				var thisBarrier = results[0];

				var newBarrier = {};
				newBarrier.Id = thisBarrier.id;
				newBarrier.Name = thisBarrier.name;
				newBarrier.PatientGoalId = thisBarrier.patientGoalId;
				newBarrier.StatusId = thisBarrier.statusId;
				newBarrier.CategoryId = thisBarrier.categoryId;
				newBarrier.DeleteFlag = thisBarrier.deleteFlag;
				newBarrier.Details = thisBarrier.details;
				return newBarrier;
		}

		// Serialize an action to save it
		function serializeObservation(observation, manager) {
				// When the serialization started
				var startTime = new Date().getTime();

				// Create an object to hold the unwrapped JSON
				var thisObservation = {};
				thisObservation.Values = [];

				// Create a query to
				// Get the unwrapped values of the properties of the observation
				var observationQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', observation.id())
						.toType('PatientObservation')
						.select('id, name, startDate, endDate, units, patientId, typeId, displayId, stateId, deleteFlag, standard, observationId, groupId, source');
				var results = manager.executeQueryLocally(observationQuery);
				var unwrappedObservation = results[0];

				// Copy actions properties
				thisObservation.Id = unwrappedObservation.id;
				thisObservation.Name = unwrappedObservation.name;
		var startMoment = moment(unwrappedObservation.startDate);
				thisObservation.StartDate = startMoment.isValid()? startMoment.toISOString() : null;
		var endMoment = moment(unwrappedObservation.endDate);
				thisObservation.EndDate = endMoment.isValid()? endMoment.toISOString() : null;
				thisObservation.GroupId = unwrappedObservation.groupId;
				thisObservation.Standard = unwrappedObservation.standard;
				thisObservation.ObservationId = unwrappedObservation.observationId;
				thisObservation.Source = unwrappedObservation.source;
				thisObservation.Units = unwrappedObservation.units;
				thisObservation.PatientId = unwrappedObservation.patientId;
				thisObservation.TypeId = unwrappedObservation.typeId;
				thisObservation.DisplayId = unwrappedObservation.displayId;
				thisObservation.StateId = unwrappedObservation.stateId;
				thisObservation.DeleteFlag = unwrappedObservation.deleteFlag;

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
						.select('id, text, patientId, createdOn, createdById, typeId, methodId, outcomeId, whoId, sourceId, duration, contactedOn, validatedIdentity, admitDate, dischargeDate, dataSource, admitted, visitTypeId, otherType, utilizationSourceId, dispositionId, otherDisposition, locationId, otherLocation, updatedById, updatedOn');
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
				thisNote.updatedOn = unwrappedNote.updatedOn;
				thisNote.updatedById = unwrappedNote.updatedById;
				thisNote.TypeId = unwrappedNote.typeId;
				thisNote.MethodId = unwrappedNote.methodId;
				thisNote.OutcomeId = unwrappedNote.outcomeId;
				thisNote.WhoId = unwrappedNote.whoId;
				thisNote.SourceId = unwrappedNote.sourceId;
				thisNote.Duration = unwrappedNote.duration;
				thisNote.ContactedOn = unwrappedNote.contactedOn;
				thisNote.ValidatedIdentity = unwrappedNote.validatedIdentity;
				//utilization:
				thisNote.admitDate =           unwrappedNote.admitDate;
				thisNote.dischargeDate =       unwrappedNote.dischargeDate;
				thisNote.dataSource =        unwrappedNote.dataSource;
				thisNote.admitted =            unwrappedNote.admitted;
				thisNote.visitTypeId =         unwrappedNote.visitTypeId;
				thisNote.otherType =           unwrappedNote.otherType;
				thisNote.utilizationSourceId = unwrappedNote.utilizationSourceId;
				thisNote.dispositionId =       unwrappedNote.dispositionId;
				thisNote.otherDisposition =    unwrappedNote.otherDisposition;
				thisNote.locationId =          unwrappedNote.locationId;
				thisNote.otherLocation =       unwrappedNote.otherLocation;

				var totalTime = new Date().getTime() - startTime;

				return thisNote;
		}

		// Serialize a todo to save it
		function serializeToDo(todo, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


				// Create an object to hold the unwrapped JSON
				var thisToDo = {};

				// Give this object a collection of Stuff
				thisToDo.ProgramIds = [];

				var todoQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', todo.id())
						.toType('ToDo')
						.select('id, title, description, patientId, createdById, assignedToId, statusId, categoryId, priorityId, dueDate, startTime, duration, createdOn, updatedOn, deleteFlag, closedDate');
				var results = manager.executeQueryLocally(todoQuery);
				var unwrappedToDo = results[0];

				// Add the program ids
				ko.utils.arrayForEach(todo.programIds.peek(), function (pid) {
						thisToDo.ProgramIds.push(pid.id.peek());
				});

				thisToDo.Id = unwrappedToDo.id;
				thisToDo.Title = unwrappedToDo.title;
				thisToDo.Description = unwrappedToDo.description;
				thisToDo.PatientId = unwrappedToDo.patientId;
				thisToDo.CreatedById = unwrappedToDo.createdById;
				thisToDo.AssignedToId = unwrappedToDo.assignedToId;
				thisToDo.StatusId = unwrappedToDo.statusId;
				thisToDo.CategoryId = unwrappedToDo.categoryId;
				thisToDo.PriorityId = unwrappedToDo.priorityId;
				thisToDo.DueDate = unwrappedToDo.dueDate;
				thisToDo.StartTime = unwrappedToDo.startTime;
				thisToDo.Duration = unwrappedToDo.duration;
				thisToDo.UpdatedOn = unwrappedToDo.updatedOn;
				thisToDo.CreatedOn = unwrappedToDo.createdOn;
				thisToDo.ClosedDate = unwrappedToDo.closedDate;
				thisToDo.DeleteFlag = unwrappedToDo.deleteFlag;

				var totalTime = new Date().getTime() - startTime;

				return thisToDo;
		}

		function serializeCareTeam(careTeam, manager){
			// Create an object to hold the unwrapped JSON
			var thisCareTeam = {};
			var careTeamQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', careTeam.id())
						.toType('CareTeam')
						.select('id, contactId');
			var results = manager.executeQueryLocally(careTeamQuery);
			var unwrappedCareTeam = results[0];

			thisCareTeam.Id = unwrappedCareTeam.id < 1 ? null : unwrappedCareTeam.id;			
			thisCareTeam.ContactId = unwrappedCareTeam.contactId;
			thisCareTeam.members = [];
			ko.utils.arrayForEach( careTeam.members(), function( member ){
				var thisCareMember = serializeCareTeamMember( member, manager );
				thisCareTeam.members.push( thisCareMember )
			});
			return thisCareTeam;
		}
		
		function serializeCareTeamMember( careMember, manager ){				
				var thisCareMember = {};
				var careMemberQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', careMember.id())
						.toType('CareMember')
						.select('id, contactId, careTeamId, roleId, customRoleName, startDate, endDate, core, notes, frequencyId, distance,'
								+ 'distanceUnit, externalRecordId, dataSource, statusId, updatedOn, createdOn, updatedById, createdById');

				var results = manager.executeQueryLocally(careMemberQuery);
				var unwrappedCareMember = results[0];
				
				thisCareMember.Id 				= unwrappedCareMember.id < 1 ? null : unwrappedCareMember.id;
				thisCareMember.ContactId		= unwrappedCareMember.contactId;		
				if( unwrappedCareMember.roleId == -1 ){	//Other Role - customRoleName
					thisCareMember.RoleId = null;					
					thisCareMember.CustomRoleName   = unwrappedCareMember.customRoleName;
				}
				else{
					thisCareMember.RoleId = unwrappedCareMember.roleId;
					thisCareMember.CustomRoleName   = null;
				}				          				
				thisCareMember.StartDate        = unwrappedCareMember.startDate; 
				thisCareMember.EndDate          = unwrappedCareMember.endDate;     
				thisCareMember.Core             = unwrappedCareMember.core;        
				thisCareMember.Notes            = unwrappedCareMember.notes;
				thisCareMember.FrequencyId      = unwrappedCareMember.frequencyId;
				thisCareMember.Distance         = unwrappedCareMember.distance;
				thisCareMember.DistanceUnit     = unwrappedCareMember.distanceUnit;
				thisCareMember.ExternalRecordId = unwrappedCareMember.externalRecordId;
				thisCareMember.DataSource       = unwrappedCareMember.dataSource;
				thisCareMember.StatusId         = unwrappedCareMember.statusId;
				thisCareMember.UpdatedOn        = unwrappedCareMember.updatedOn;
				thisCareMember.CreatedOn        = unwrappedCareMember.createdOn;
				thisCareMember.UpdatedById      = unwrappedCareMember.updatedById;
				thisCareMember.CreatedById      = unwrappedCareMember.createdById;
				thisCareMember.CareTeamId 		= unwrappedCareMember.careTeamId ? unwrappedCareMember.careTeamId : null;
			
				return thisCareMember;
		}
		
		//this will be deprecated:
		// Serialize a care member to save it
		function serializeCareMember(careMember, manager) {
				// When the serialization started
				var startTime = new Date().getTime();


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

				return thisCareMember;
		}

		// Serialize a care member to save it
		function serializeIndividual(patient, manager) {
			// When the serialization started
			var startTime = new Date().getTime();


			// Create an object to hold the unwrapped JSON
			var thisIndividual = {};

			// Get the values of the properties of the action
			// var unwrappedCareMember = ko.toJS(careMember);

			var patientQuery = breeze.EntityQuery
					.from('fakePath')
					.where('id', '==', patient.id())
					.toType('Patient')
					.select('id, priority, gender, firstName, lastName, preferredName, suffix, dOB, middleName, background, clinicalBackground, fullSSN, dataSource, statusId, reasonId, statusDataSource, deceasedId, maritalStatusId, protected');
			var results = manager.executeQueryLocally(patientQuery);
			var unwrappedIndividual = results[0];

			thisIndividual.Id = unwrappedIndividual.id;
			thisIndividual.Priority = unwrappedIndividual.priority;
			thisIndividual.Gender = unwrappedIndividual.gender;
			thisIndividual.FirstName = unwrappedIndividual.firstName;
			thisIndividual.LastName = unwrappedIndividual.lastName;
			thisIndividual.PreferredName = unwrappedIndividual.preferredName;
			thisIndividual.Suffix = unwrappedIndividual.suffix;
			thisIndividual.DOB = unwrappedIndividual.dOB;
			thisIndividual.MiddleName = unwrappedIndividual.middleName;
			thisIndividual.Background = unwrappedIndividual.background;
			thisIndividual.ClinicalBackground = unwrappedIndividual.clinicalBackground;
			thisIndividual.DataSource = unwrappedIndividual.dataSource;
			thisIndividual.StatusId = unwrappedIndividual.statusId;
			thisIndividual.ReasonId = unwrappedIndividual.reasonId;
			thisIndividual.StatusDataSource = unwrappedIndividual.statusDataSource;
			thisIndividual.DeceasedId = unwrappedIndividual.deceasedId;
			thisIndividual.MaritalStatusId = unwrappedIndividual.maritalStatusId;
			thisIndividual.Protected = unwrappedIndividual.protected;

			if (unwrappedIndividual.fullSSN) {
			// Add it as a parameter
					thisIndividual.FullSSN = unwrappedIndividual.fullSSN;
			}
			if (patient.isDuplicate() && patient.forcedSave) {
					thisIndividual.InsertDuplicate = true;
			}

			// thisIndividual.PatientId = unwrappedIndividual.patientId;
			// thisIndividual.ContactId = unwrappedIndividual.contactId;
			// thisIndividual.TypeId = unwrappedIndividual.typeId;
			// thisIndividual.Primary = unwrappedIndividual.primary;
			// thisIndividual.PreferredName = unwrappedIndividual.preferredName;

			var totalTime = new Date().getTime() - startTime;

			return thisIndividual;
		}

		// Serialize a patient system to save it
		function serializePatientSystem(patientSystem, manager) {
			// When the serialization started
			var startTime = new Date().getTime();

			// Create an object to hold the unwrapped JSON
			var thisPatSys = {};

			var patientSystemQuery = breeze.EntityQuery
					.from('fakePath')
					.where('id', '==', patientSystem.id())
					.toType('PatientSystem')
					.select('id, patientId, systemId, value, dataSource, statusId, primary, createdById, createdOn, updatedById, updatedOn');
			var results = manager.executeQueryLocally(patientSystemQuery);
			var unwrappedPatSys = results[0];

			if( unwrappedPatSys.id && !isNaN(unwrappedPatSys.id) && unwrappedPatSys.id < 0){
				thisPatSys.Id = null;
			}
			else{
				thisPatSys.Id = unwrappedPatSys.id;
			}
			thisPatSys.PatientId = unwrappedPatSys.patientId;
			thisPatSys.SystemId = unwrappedPatSys.systemId;
			thisPatSys.value = unwrappedPatSys.value.trim();
			thisPatSys.DataSource = unwrappedPatSys.dataSource;
			thisPatSys.statusId = unwrappedPatSys.statusId;
			thisPatSys.primary = unwrappedPatSys.primary;
			thisPatSys.createdById = unwrappedPatSys.createdById;
			thisPatSys.createdOn = unwrappedPatSys.createdOn;
			thisPatSys.updatedById = unwrappedPatSys.updatedById;
			thisPatSys.updatedOn = unwrappedPatSys.updatedOn;

			var totalTime = new Date().getTime() - startTime;

			return thisPatSys;
		}

		// Serialize an action to save it
		function serializePatientAllergy(allergy, manager) {
			// When the serialization started
			var startTime = new Date().getTime();

			// Create an object to hold the unwrapped JSON
			var thisAllergy = {};

			// Create a query to
			// Get the unwrapped values of the properties of the allergy
			var allergyQuery = breeze.EntityQuery
					.from('fakePath')
					.where('id', '==', allergy.id())
					.toType('PatientAllergy')
					.select('id, allergyName, startDate, endDate, patientId, statusId, deleteFlag, severityId, allergyId, sourceId, notes, systemName, code, codingSystemId');
			var results = manager.executeQueryLocally(allergyQuery);
			var unwrappedAllergy = results[0];

			// Copy actions properties
			thisAllergy.Id = unwrappedAllergy.id;
			thisAllergy.AllergyName = unwrappedAllergy.allergyName;
			var startMoment = moment(unwrappedAllergy.startDate);
			thisAllergy.StartDate = startMoment.isValid()? startMoment.toISOString() : null;
			var endMoment = moment(unwrappedAllergy.endDate);
			thisAllergy.EndDate = endMoment.isValid()? endMoment.toISOString() : null;
			thisAllergy.PatientId = unwrappedAllergy.patientId;
			thisAllergy.AllergyId = unwrappedAllergy.allergyId;
			thisAllergy.StatusId = unwrappedAllergy.statusId;
			thisAllergy.SourceId = unwrappedAllergy.sourceId;
			thisAllergy.DeleteFlag = unwrappedAllergy.deleteFlag;
			thisAllergy.SeverityId = unwrappedAllergy.severityId;
            thisAllergy.Notes = unwrappedAllergy.notes;
            thisAllergy.SystemName = unwrappedAllergy.systemName;
            thisAllergy.Code = unwrappedAllergy.code;
            thisAllergy.CodingSystem = unwrappedAllergy.codingSystem;

			// If it is a brand new allergy set an isNewAllergy property
			if (allergy.isUserCreated()) {
					thisAllergy.IsNewAllergy = true;
			}

			thisAllergy.AllergyTypeIds = [];
			ko.utils.arrayForEach(allergy.allergyTypeIds.peek(), function (value) {
					thisAllergy.AllergyTypeIds.push(value.id.peek());
			});

			thisAllergy.ReactionIds = [];
			ko.utils.arrayForEach(allergy.reactionIds.peek(), function (value) {
					thisAllergy.ReactionIds.push(value.id.peek());
			});

			var totalTime = new Date().getTime() - startTime;

			return thisAllergy;
		}

		// Serialize an action to save it
		function serializePatientMedication(medication, manager) {
				// When the serialization started
				var startTime = new Date().getTime();

				// Create an object to hold the unwrapped JSON
				var thisMedication = {};

				// Create a query to
				// Get the unwrapped values of the properties of the medication
				var medicationQuery = breeze.EntityQuery
						.from('fakePath')
						.where('id', '==', medication.id())
						.toType('PatientMedication')
						.select('id, name, startDate, endDate, patientId, statusId, deleteFlag, sourceId, notes, systemName, dosage, strength, route, form, freqQuantity, freqHowOftenId, frequencyId, freqWhenId, customFrequency, categoryId, prescribedBy, typeId, sigCode, reason, familyId, isCreateNewMedication, originalDataSource, duration, durationUnitId, otherDuration, reviewId, refusalReasonId, otherRefusalReason, orderedBy, orderedDate, prescribedDate, rxNumber, rxDate, pharmacy, originalDataSource');
				var results = manager.executeQueryLocally(medicationQuery);
				var unwrappedObservation = results[0];

				// Copy actions properties
				thisMedication.Id = unwrappedObservation.id;
				thisMedication.Name = unwrappedObservation.name;
		var startMoment = moment(unwrappedObservation.startDate);
				thisMedication.StartDate = startMoment.isValid()? startMoment.toISOString() : null;
		var endMoment = moment(unwrappedObservation.endDate);
				thisMedication.EndDate = endMoment.isValid()? endMoment.toISOString() : null;
				thisMedication.PatientId = unwrappedObservation.patientId;
				thisMedication.StatusId = unwrappedObservation.statusId;
				thisMedication.SourceId = unwrappedObservation.sourceId;
				thisMedication.DeleteFlag = unwrappedObservation.deleteFlag;
				thisMedication.Notes = unwrappedObservation.notes;
				thisMedication.SystemName = unwrappedObservation.systemName;
				thisMedication.Dosage = unwrappedObservation.dosage;
				thisMedication.Strength = unwrappedObservation.strength;
				thisMedication.Route = unwrappedObservation.route;
				thisMedication.Form = unwrappedObservation.form;
				thisMedication.FreqQuantity = unwrappedObservation.freqQuantity;
				thisMedication.FreqHowOftenId = unwrappedObservation.freqHowOftenId;
		thisMedication.FrequencyId = unwrappedObservation.frequencyId;
				thisMedication.FreqWhenId = unwrappedObservation.freqWhenId;
				thisMedication.CategoryId = unwrappedObservation.categoryId;
				thisMedication.PrescribedBy = unwrappedObservation.prescribedBy;
				thisMedication.TypeId = unwrappedObservation.typeId;
				thisMedication.SigCode = unwrappedObservation.sigCode;
				thisMedication.Reason = unwrappedObservation.reason;
				thisMedication.FamilyId = unwrappedObservation.familyId; //a new medicationMap record id
				thisMedication.RecalculateNDC = medication.recalculateNDC();
                thisMedication.OriginalDataSource = unwrappedObservation.originalDataSource;
                thisMedication.Duration = parseInt(unwrappedObservation.duration);
                thisMedication.DurationUnitId = unwrappedObservation.durationUnitId;
                thisMedication.OtherDuration = unwrappedObservation.otherDuration;
                thisMedication.ReviewId = unwrappedObservation.reviewId;
                thisMedication.RefusalReasonId = unwrappedObservation.refusalReasonId;
                thisMedication.OtherRefusalReason = unwrappedObservation.otherRefusalReason;
                thisMedication.OrderedBy = unwrappedObservation.orderedBy;
                thisMedication.OrderedDate = unwrappedObservation.orderedDate;
                thisMedication.PrescribedDate = unwrappedObservation.prescribedDate;
                thisMedication.RxNumber = unwrappedObservation.rxNumber;
                thisMedication.RxDate = unwrappedObservation.rxDate;
                thisMedication.Pharmacy = unwrappedObservation.pharmacy;

				thisMedication.NDCs = [];
				ko.utils.arrayForEach(medication.nDCs.peek(), function (value) {
						thisMedication.NDCs.push(value.id.peek());
				});

				thisMedication.PharmClasses = [];
				ko.utils.arrayForEach(medication.pharmClasses.peek(), function (value) {
						thisMedication.PharmClasses.push(value.id.peek());
				});

				var totalTime = new Date().getTime() - startTime;

				return thisMedication;
		}

		var entitySerializer = {
				serializeAction: serializeAction,
				serializeContactCard: serializeContactCard,
				serializeGoal: serializeGoal,
				serializeIntervention: serializeIntervention,
				serializeTask: serializeTask,
				serializeBarrier: serializeBarrier,
				serializeObservation: serializeObservation,
				serializeNote: serializeNote,
				serializeToDo: serializeToDo,
				serializeCareMember: serializeCareMember,	
				serializeCareTeam: serializeCareTeam,
				serializeCareTeamMember: serializeCareTeamMember,
				serializeIndividual: serializeIndividual,
				serializePatientSystem: serializePatientSystem,
				serializePatientAllergy: serializePatientAllergy,
				serializePatientMedication: serializePatientMedication
		}

		return entitySerializer;

});