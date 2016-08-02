define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
            var self = this;
            self.alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };
            self.alphabeticalOrderSort = function (l, r) { return (l.order() == r.order()) ? (l.order() > r.order() ? 1 : -1) : (l.order() > r.order() ? 1 : -1) };
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.intervention = ko.unwrap(self.settings.entity);
            // Decides whether we can change status or not
            self.isNew = (self.intervention && self.intervention.goal()) ? self.intervention.goal().isNew : function () { return false; };
			self.existingDetailsOpen = ko.observable(false);
			self.toggleOpen = function () {
				self.existingDetailsOpen(!self.existingDetailsOpen());
			};
            self.careManagers = ko.computed(function () {
                var theseCareManagers =  datacontext.enums.careManagers()
                return theseCareManagers.sort(self.alphabeticalSort);
            });
            self.computedBarriers = ko.computed(self.intervention.goal().barriers);
            self.interventionStatuses = datacontext.enums.interventionStatuses;
            self.interventionCategories = datacontext.enums.interventionCategories;
            self.careManagers = datacontext.enums.careManagers;
            self.canSave = self.settings.canSave ? self.settings.canSave : true;
            self.showing = self.settings.showing ? self.settings.showing : true;
            // Set the displayed value to the current assign to
            self.assignedToDisplay = ko.observable((self.intervention && self.intervention.assignedTo()) ? self.intervention.assignedTo().preferredName() : '');
            self.assignedToDisplay.subscribe(function (newValue) {
                // else, Find the care manager that matches
                var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                    return cm.preferredName() === newValue;
                });
                // If a match is found,
                if (matchedCareManager) {
                    self.intervention.assignedTo(matchedCareManager);
                } else {
                }
            });
            self.checkForMatch = function () {
                return self.assignedToDisplay() === (self.intervention.assignedTo() ? self.intervention.assignedTo().preferredName() : '');
            };
            self.removeAssignment = function () {
                self.intervention.assignedTo(null);
                self.assignedToDisplay('');
            };
            self.validMatch = ko.computed(function () {
                var result = false;
                if (self.intervention) {
                    // Check if it matches a valid value
                    result = self.checkForMatch();
                    // If there is an invalid value,
                    if (!result) {
                        // if the assign to id has been changed,
                        if (self.intervention && self.intervention.assignedToId()) {
                            // Reset the value
                            self.assignedToDisplay((self.intervention && self.intervention.assignedTo()) ? self.intervention.assignedTo().preferredName() : '');
                            self.assignedToDisplay.valueHasMutated();
                            // And check again
                            result = self.checkForMatch();
                        } else {
                            // Else clear the value
                            self.removeAssignment();
                        }
                    }
                }
                // Enable or disable the can save state
                self.canSave = result;
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
                        var theseCareManagers = self.careManagers().sort(self.alphabeticalSort);
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
			/**
			*	computed. to allow forcing the datetimepicker control to set the due date as invalid.
			*	this is needed when the date is valid but range is wrong.
			*	@method setInvalidDueDate
			*/
			self.setInvalidDueDate = ko.computed( function(){
				return ( self.intervention && self.intervention.validationErrorsArray().indexOf('dueDate') !== -1);
			});
			/**
			*	computed. to allow forcing the datetimepicker control to set the start date as invalid.
			*	this is needed when the date is valid but range is wrong.
			*	@method setInvalidStartDate
			*/
			self.setInvalidStartDate = ko.computed( function(){
				return ( self.intervention && self.intervention.validationErrorsArray().indexOf('StartDate') !== -1);
			});

        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });