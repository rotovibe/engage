/* jsonResultsAdapter: parses data into entities */
define([], new breeze.JsonResultsAdapter({

    name: "Nightingale",

    extractResults: function (data) {
        var results = data.results;
        if (!results) throw new Error("Unable to resolve 'results' property");
        return results && (results.Problems || results.PatientProblem || results.PatientProblems || results.Patient || results.Cohorts || results.Patients || results.Program || results.Programs || results.PlanElems || results.Languages || results.States || results.CommModes || results.TimesOfDays || results.TimeZones || results.CommTypes || results.Contact || results.Contacts || results.LookUps || results.Goals || results.Goal || results.Task || results.Note || results.Notes || results.Library || results.CareMembers || results.Observations || results.Observation);
    },

    visitNode: function (node, mappingContext, nodeContext) {
        if (node.PatientProblemID) {
            return { entityType: "PatientProblem" };
        }
        if (node.ConditionID && node.DisplayName) {
            return { entityType: "Condition" };
        }
        if (node.Id && node.DOB) {
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
        if (mappingContext.query.entityType && mappingContext.query.entityType.shortName === 'Note') {
            // If you were mapping to a note, we need to map the program ids
            if (node.ProgramIds) {
                var theseProgramIds = [];
                $.each(node.ProgramIds, function (index, item) {
                    theseProgramIds.push({ Id: item });
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
        if (mappingContext.query.entityType && mappingContext.query.entityType.shortName === 'Goal') {
            if (nodeContext.nodeType === 'root') {
                if (node.FocusAreaIds) {
                    var theseIds = [];
                    $.each(node.FocusAreaIds, function (index, item) {
                        theseIds.push({ Id: item });
                    });
                    node.FocusAreaIds = theseIds;
                }
                if (node.ProgramIds) {
                    var theseProgramIds = [];
                    $.each(node.ProgramIds, function (index, item) {
                        theseProgramIds.push({ Id: item });
                    });
                    node.ProgramIds = theseProgramIds;
                }
                if (node.CustomAttributes) {
                    $.each(node.CustomAttributes, function (index, item) {
                        var theseValues = [];
                        if (item.Values) {
                            $.each(item.Values, function (index, item) {
                                theseValues.push({ Value: item });
                            });
                            item.Values = theseValues;
                        }
                    });
                }
            }
            if (nodeContext.navigationProperty) {
                if (nodeContext.navigationProperty.name === 'tasks') {
                    if (node.BarrierIds) {
                        var theseIds = [];
                        $.each(node.BarrierIds, function (index, item) {
                            theseIds.push({ Id: item });
                        });
                        node.BarrierIds = theseIds;
                    }
                    if (node.CustomAttributes) {
                        $.each(node.CustomAttributes, function (index, item) {
                            var theseValues = [];
                            $.each(item.Values, function (index, item) {
                                theseValues.push({ Value: item });
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
                            theseIds.push({ Id: item });
                        });
                        node.BarrierIds = theseIds;
                    }
                    return { entityType: "Intervention" };
                }
            }
        }
    }
}));