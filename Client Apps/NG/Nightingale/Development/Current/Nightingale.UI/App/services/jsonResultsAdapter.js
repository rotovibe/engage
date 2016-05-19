
/**
*			jsonResultsAdapter parses api result data into entities
*			engage api returned results are ususally enclosed in a wrapper object.
*			the requested result object array is within. for breeze to read it and materialize entities, 
*			we need the data.results to contain the array. => every get endpoint type should be added to the extractResults.
*	@module	jsonResultsAdapter
*	@class jsonResultsAdapter			
*/
define([], new breeze.JsonResultsAdapter({

    name: "Nightingale",

    extractResults: function (data) {
        var results = data.results;
        if (!results) throw new Error("Unable to resolve 'results' property");
        return results && (results.Problems || results.PatientProblem || results.PatientProblems || results.Patient || results.Cohorts || results.Patients || results.Program || results.Programs || results.PlanElems || results.Languages || results.States || results.Reason || results.CommModes || results.TimesOfDays || results.TimeZones || results.CommTypes || results.Contact || results.Contacts || results.LookUps || results.LookUpDetails || results.Goals || results.Goal || results.Note || results.Notes || results.PatientNote || results.Utilization || results.Library || results.CareMembers || results.CareTeam || results.Observations || results.Observation ||results.PatientObservations || results.PatientObservation || results.Action || results.Actions || results.Attributes || results.Objectives || results.ToDo || results.ToDos || results.PatientDetails || results.Intervention || results.Interventions || results.Task || results.Tasks || results.Systems || results.PatientSystems || results.Allergy || results.Allergies || results.PatientAllergy || results.PatientAllergies || results.PatientMedSupp || results.PatientMedSupps || results.PatientMedFrequencies || results.ContactTypeLookUps);
    },

    visitNode: function (node, mappingContext, nodeContext) {
        if (!node) { return false; }
        if (node.PatientProblemID) {
            return { entityType: "PatientProblem" };
        }
        if (node.ConditionID && node.DisplayName) {
            return { entityType: "Condition" };
        }
        if (node.Id && node.DOB || node.Id && node.Flagged && node.Priority) {
            return { entityType: "Patient" };
        }
        if (node.ProgramId && node.Actions) {
            var thisDateCompleted = node.DateCompleted
            // Grab a piece of the date,
            if (!thisDateCompleted) { return { entityType: "Module" } }
            var dateString = thisDateCompleted.substr(0, 6);
            // to check for the word 'Date', if is found,
            if (dateString.indexOf('Date') !== -1) {
                // Get only the milliseconds
                var thisSomethingThing = thisDateCompleted.substr(6, 13);
                // Create a new holder for a date
                var thisNewDate = new Date();
                // Set the time using milliseconds
                thisNewDate.setTime(thisSomethingThing);
                // Set the value equal to the ISO string
                node.DateCompleted = thisNewDate.toISOString();
            }
            return { entityType: "Module" };
        }
        if (node.PatientId && node.ContractProgramId) {
            return { entityType: "Program" };
        }
        if (node.Enrollment && node.PlanElementId) {
            return { entityType: "Attributes" };
        }
        if (node.ModuleId && node.Steps) {
            return { entityType: "Action" };
        }
        if (node.StepTypeId) {
            return { entityType: "Step" };
        }
        if (node.StepId) {
            return { entityType: "Response" };
        }
        if (node.ElementId) {
            return { entityType: "SpawnElement" };
        }
        if (node.CommModes && node.Id) {
            node.CommModeIds = [];
            $.each(node.CommModes, function (index, item) {
                node.CommModeIds.push({ Id: item });
            });
            return { entityType: "CommunicationType" }
        }
		if(node.ContactSubTypes) {
			var subTypes = []
			$.each(node.ContactSubTypes, function(index, sub){
				var theseIds = [];
				if( sub.SubSpecialtyIds ){
					$.each(sub.SubSpecialtyIds, function (index, item) {
						// if the item is not null
						if (item) {
							theseIds.push({ Id: item });
						}
					});
				}
				sub.SubSpecialtyIds = theseIds; 
				//TBD - may need to rewrite the node.ContactSubTypes (1,2)
				//1. subTypes.push(sub);	
			});
			//2. node.ContactSubTypes = subTypes;
		}
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'Note') {
            // If you were mapping to a note, we need to map the program ids
            if (node.ProgramIds) {
                var theseProgramIds = [];
                $.each(node.ProgramIds, function (index, item) {
                    // if the item is not null
                    if (item) {
                        theseProgramIds.push({ Id: item });
                    }
                });
                node.ProgramIds = theseProgramIds;
            }
        }
        if (node.TimeZoneId) {
            // All of the contact code arrays are coming back in different formats
            // So we have to map them to some whay
            node.PreferredTimesOfDayIds = [];
            node.PreferredDaysOfWeekIds = [];
            if (node.TimesOfDaysId) {
                $.each(node.TimesOfDaysId, function (index, item) {
                    node.PreferredTimesOfDayIds.push({ Id: item });
                });
            }
            if (node.WeekDays) {
                $.each(node.WeekDays, function (index, item) {
                    node.PreferredDaysOfWeekIds.push({ Id: item });
                });
            }
            return { entityType: "ContactCard" }
        }
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'ToDo') {
            if (node.ProgramIds) {
                var theseProgramIds = [];
                $.each(node.ProgramIds, function (index, item) {
                    // if the item is not null
                    if (item) {
                        theseProgramIds.push({ Id: item });
                    }
                });
                node.ProgramIds = theseProgramIds;
            }
            if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'patientDetails') {
                    return { entityType: "ToDoPatient" };
                }
            }
            return { entityType: "ToDo" };
        }
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'Intervention'|| (nodeContext.propertyName && nodeContext.propertyName === 'interventions')) {
            if (node.BarrierIds) {
                var theseIds = [];
                $.each(node.BarrierIds, function (index, item) {
                    // if the item is not null
                    if (item) {
                        theseIds.push({ Id: item });
                    }
                });
                node.BarrierIds = theseIds;
            }
            if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'patientDetails') {
                    return { entityType: "ToDoPatient" };
                }
            }
            return { entityType: "Intervention" };
        }
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'Task'|| (nodeContext.propertyName && nodeContext.propertyName === 'tasks')) {
            if (node.BarrierIds) {
                var theseIds = [];
                $.each(node.BarrierIds, function (index, item) {
                    // if the item is not null
                    if (item) {
                        theseIds.push({ Id: item });
                    }
                });
                node.BarrierIds = theseIds;
            }
            if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'patientDetails') {
                    return { entityType: "ToDoPatient" };
                }
            }
            if (node.CustomAttributes) {
                $.each(node.CustomAttributes, function (index, item) {
                    var theseValues = [];
                    if (item && item.Values) {
                        $.each(item.Values, function (index, item) {
                            // if the item is not null
                            if (item) {
                                theseValues.push({ Value: item });
                            }
                        });
                        item.Values = theseValues;
                    }
                });
                return { entityType: "Task" };
            }
        }
		
		if ((mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'CareTeam') 
			|| (nodeContext.propertyName && nodeContext.propertyName === 'careTeam') 
			|| (nodeContext.propertyName && nodeContext.propertyName === 'members') ) {
					
			//help breeze identify the entities in the associated relations CareTeam_CareMembers (both ways)
			//( this is also done in goals for the sub entities Barriers and Interventions )
			if (nodeContext.nodeType === 'root' || (nodeContext.propertyName && nodeContext.propertyName === 'careTeam')) {
				if( node.Members && node.Id ){
					$.each(node.Members, function(index, member) {
						member.CareTeamId = node.Id;	//populate the team id
					});
				}
				return { entityType: "CareTeam" };
			}
			if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'members') {
					//node.CareTeamId is now set to point to the team id
					return { entityType: "CareMember" };
                }
                //fix mapping to resolve navigation entities we need under caremember
				if (nodeContext.navigationProperty.name === 'contact') {					
					return { entityType: "ContactCard" };
				}
				if (nodeContext.navigationProperty.name === 'roleType') {					
					return { entityType: "ContactTypeLookup" };
				}
			}
		}				
		
        if ((mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'Goal') || (nodeContext.propertyName && nodeContext.propertyName === 'goals')) {
            if (nodeContext.nodeType === 'root' || (nodeContext.propertyName && nodeContext.propertyName === 'goals')) {
                if (node.FocusAreaIds) {
                    var theseIds = [];
                    $.each(node.FocusAreaIds, function (index, item) {
                        // if the item is not null
                        if (item) {
                            theseIds.push({ Id: item });
                        }
                    });
                    node.FocusAreaIds = theseIds;
                }
                if (node.ProgramIds) {
                    var theseProgramIds = [];
                    $.each(node.ProgramIds, function (index, item) {
                        // if the item is not null
                        if (item) {
                            theseProgramIds.push({ Id: item });
                        }
                    });
                    node.ProgramIds = theseProgramIds;
                }
                if (node.CustomAttributes) {
                    $.each(node.CustomAttributes, function (index, item) {
                        var theseValues = [];
                        if (item.Values) {
                            $.each(item.Values, function (index, item) {
                                // if the item is not null
                                if (item) {
                                    theseValues.push({ Value: item });
                                }
                            });
                            item.Values = theseValues;
                        }
                    });
                }
                return { entityType: "Goal" };
            }
            if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'tasks') {
                    if (node.BarrierIds) {
                        var theseIds = [];
                        $.each(node.BarrierIds, function (index, item) {
                            // if the item is not null
                            if (item) {
                                theseIds.push({ Id: item });
                            }
                        });
                        node.BarrierIds = theseIds;
                    }
                    if (node.CustomAttributes) {
                        $.each(node.CustomAttributes, function (index, item) {
                            var theseValues = [];
                            $.each(item.Values, function (index, item) {
                                // if the item is not null
                                if (item) {
                                    theseValues.push({ Value: item });
                                }
                            });
                            item.Values = theseValues;
                        });
                    }
                    return { entityType: "Task" };
                }
                else if (nodeContext.navigationProperty.name === 'barriers') {
                    return { entityType: "Barrier" };
                }
                else if (nodeContext.navigationProperty.name === 'interventions') {
                    if (node.BarrierIds) {
                        var theseIds = [];
                        $.each(node.BarrierIds, function (index, item) {
                            // if the item is not null
                            if (item) {
                                theseIds.push({ Id: item });
                            }
                        });
                        node.BarrierIds = theseIds;
                    }
                    return { entityType: "Intervention" };
                }
            }
        }
        if (node.Id && node.Name && node.Categories) {
            return { entityType: "ObjectiveLookup" };
        }
        // if (node.SystemId && node.value) {
            // return { entityType: "PatientSystem" };
        // }
        if (node.ObservationId && node.TypeId) {
            return { entityType: "PatientObservation" };
        }
        if (node.Title && node.StatusId && node.AssignedToId) {
            if (node.ProgramIds) {
                var theseProgramIds = [];
                $.each(node.ProgramIds, function (index, item) {
                    // if the item is not null
                    if (item) {
                        theseProgramIds.push({ Id: item });
                    }
                });
                node.ProgramIds = theseProgramIds;
            }
            return { entityType: "ToDo" };
        }
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'ObservationState') {
            node.AllowedTypeIds = [];
            $.each(node.TypeIds, function (index, item) {
                node.AllowedTypeIds.push({ Id: item });
            });
            // return { entityType: "CommunicationType" }
        }
        if (node.AllergyId && node.AllergyTypeIds) {
            var tempAllergyTypeIds = [];
            var tempReactionIds = [];
            $.each(node.AllergyTypeIds, function (index, item) {
                tempAllergyTypeIds.push({ Id: item });
            });
            node.AllergyTypeIds = tempAllergyTypeIds;
            $.each(node.ReactionIds, function (index, item) {
                tempReactionIds.push({ Id: item });
            });
            node.ReactionIds = tempReactionIds;
            return { entityType: "PatientAllergy" }
        }
        if (mappingContext.query.fromEntityType && mappingContext.query.fromEntityType.shortName === 'PatientMedication') {
            var tempNDCs = [];
            var tempPharmClasses = [];
            $.each(node.NDCs, function (index, item) {
                tempNDCs.push({ Id: item });
            });
            $.each(node.PharmClasses, function (index, item) {
                tempPharmClasses.push({ Id: item });
            });
            node.NDCs = tempNDCs;
            node.PharmClasses = tempPharmClasses;
            if (nodeContext.nodeType === 'root' || (nodeContext.propertyName && nodeContext.propertyName === 'patientMedSupps')) {
                return { entityType: "PatientMedication" };
            }
        }
    }
}));