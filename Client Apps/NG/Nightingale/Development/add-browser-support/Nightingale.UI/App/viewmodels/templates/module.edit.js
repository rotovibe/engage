﻿define(['services/datacontext'],
    function (datacontext) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.module = self.settings.module;
            self.canSave = self.settings.canSave;
            self.showing = self.settings.showing;
            self.careManagers = datacontext.enums.careManagers;
            // Set the displayed value to the current assign to
            self.assignToDisplay = ko.observable((self.module() && self.module().assignTo()) ? self.module().assignTo().preferredName() : '');
            self.assignToDisplay.subscribe(function (newValue) {
                // else, Find the care manager that matches
                var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                    return cm.preferredName() === newValue;
                });
                // If a match is found,
                if (matchedCareManager) {
                    self.module().assignTo(matchedCareManager);
                } else {
                    console.log('No match found');
                }
            });
            self.showing.subscribe(function (newValue) {
                // If the user closes the modal without saving,
                if (!newValue) {
                    // If the assigned to has been cleared out,
                    if (!self.module().assignToId()) {
                        // Clear the values so it clears the text box on next load
                        self.assignToDisplay('');
                    }
                }
            });
            self.checkForMatch = function () {
                return self.assignToDisplay() === (self.module().assignTo() ? self.module().assignTo().preferredName() : '');                
            }
            self.removeAssignment = function () {
                self.module().assignTo(null);
                self.assignToDisplay('');
            }
            self.validMatch = ko.computed(function () {
                var result = false;
                if (self.module()) {
                    // Check if it matches a valid value
                    result = self.checkForMatch();
                    // If there is an invalid value,
                    if (!result) {
                        // If the assign to id has been cleared,
                        if (self.module() && self.module().assignToId()) {
                            // Reset the value
                            self.assignToDisplay((self.module() && self.module().assignTo()) ? self.module().assignTo().preferredName() : '');
                            self.assignToDisplay.valueHasMutated();
                            // And check again
                            result = self.checkForMatch();
                        } else {
                            self.removeAssignment();
                        }
                    }
                }
                // Enable or disable the can save state
                self.canSave(result);
                return result;
            }).extend({ throttle: 25 });
            self.careManagersBloodhound = new Bloodhound({
                datumTokenizer: function (d) {
                    return Bloodhound.tokenizers.whitespace(d.name());
                },
                queryTokenizer: Bloodhound.tokenizers.whitespace,
                remote: {
                    url: '%QUERY',
                    transport: function (url, options, onSuccess, onError) {
                        var theseCareManagers = self.careManagers().sort(alphabeticalSort);
                        var deferred = $.Deferred();
                        deferred.done(function () { onSuccess(this); });

                        var filterVal = url.toLowerCase();
                        var result = theseCareManagers.filter(function (item) {
                            return !!~item.preferredName().toLowerCase().indexOf(filterVal);
                        });
                        deferred.resolveWith(result);
                        return deferred.promise();
                    }
                },
                limit: 25
            });
            self.careManagersBloodhound.initialize();
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });