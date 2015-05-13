define(['config.models', 'services/datacontext', 'services/session'],
    function (models, datacontext, session) {

        var alphabeticalSort = function (l, r) { return (l.problem().name() == r.problem().name()) ? (l.problem().name() > r.problem().name() ? 1 : -1) : (l.problem().name() > r.problem().name() ? 1 : -1) };

        var ctor = function () {
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.problems = settings.problems;            
            self.isExpanded = ko.observable(false);
            // Create a list of primary problems to display in the widget
            self.primaryProblems = ko.computed(function () {
                // Create an empty array to fill with problems
                var theseProblems = [];
                var searchProblems = self.problems().sort(self.alphabeticalSort);
                // If not expanded limit to five
                var limitToFive = (!self.isExpanded());
                // Create a filtered list of problems,
                ko.utils.arrayForEach(searchProblems, function (problem) {
                    // Where a problem level exists.  Need to change this to choose primary only later
                    if (problem.level() === '1') {
                        if (theseProblems.length < 5 || !limitToFive) {
                            theseProblems.push(problem);
                        }
                    }
                });
                return theseProblems;
            });
            // Create a list of secondary problems to display in the widget
            self.secondaryProblems = ko.computed(function () {
                // Create an empty array to fill with problems
                var theseProblems = [];
                var searchProblems = self.problems().sort(self.alphabeticalSort);
                // If not expanded limit to five
                var limitToFive = (!self.isExpanded());
                // Create a filtered list of problems,
                ko.utils.arrayForEach(searchProblems, function (problem) {
                    // Where a problem level exists.  Need to change this to choose primary only later
                    if (problem.level() === '2') {
                        if (theseProblems.length < 5 || !limitToFive) {
                            theseProblems.push(problem);
                        }
                    }
                });
                return theseProblems;
            });
            self.isOpen = ko.observable(true);
            self.isFullScreen = ko.observable(false);
            self.isEditing = ko.observable(false);
            self.toggleEditing = function () {
                self.isEditing(!self.isEditing());
                self.isOpen(true);
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });