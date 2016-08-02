define(['services/datacontext'],
    function (datacontext) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.action = self.settings.action;
            self.canSave = self.settings.canSave;
            self.showing = self.settings.showing;
            self.careManagers = datacontext.enums.careManagers;
            // Set the displayed value to the current assign to
            self.assignToDisplay = ko.observable((self.action() && self.action().assignTo()) ? self.action().assignTo().preferredName() : '');
            self.assignToDisplay.subscribe(function (newValue) {
                // else, Find the care manager that matches
                var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                    return cm.preferredName() === newValue;
                });
                // If a match is found,
                if (matchedCareManager) {
                    self.action().assignTo(matchedCareManager);
                } else {
                    console.log('No match found');
                }
            });
            self.checkForMatch = function () {
                return self.assignToDisplay() === (self.action().assignTo() ? self.action().assignTo().preferredName() : '');                
            }
            self.removeAssignment = function () {
                self.action().assignTo(null);
                self.assignToDisplay('');
            }
            self.validMatch = ko.computed(function () {
                var result = false;
                if (self.action()) {
                    // Check if it matches a valid value
                    result = self.checkForMatch();
                    // If there is an invalid value,
                    if (!result) {
                        // if the assign to id has been changed,
                        if (self.action() && self.action().assignToId()) {
                            // Reset the value
                            self.assignToDisplay((self.action() && self.action().assignTo()) ? self.action().assignTo().preferredName() : '');
                            self.assignToDisplay.valueHasMutated();
                            // And check again
                            result = self.checkForMatch();
                        } else {
                            // Else clear the value
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
                            if (item && item.firstLastOrPreferredName) {
                                return !!~item.firstLastOrPreferredName().toLowerCase().indexOf(filterVal);
                            }
                            return false;
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