define(['config.models', 'services/datacontext', 'services/session'],
    function (modelConfig, datacontext, session) {

        var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

        var ctor = function () {

        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            // Get the selected patient that was passed in
            self.selectedPatient = settings.selectedPatient;
            // Get a list of all of the care team
            self.careMembers = self.selectedPatient.careMembers;
            // The view state of the section (open or not)
            self.isOpen = ko.observable(true);
            // Create a list of primary care team members to display in the widget
            self.primaryCareTeam = ko.computed(function () {
                // Create an empty array to fill with problems
                var thisCareTeam = [];
                // Sort the team
                var searchCareTeam = self.careMembers().sort(alphabeticalSort);
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // If they are a member of the primary care team,
                    if (careMember.primary()) {
                        // Add them to the team
                        thisCareTeam.push(careMember);
                    }
                });
                // Return the team
                return thisCareTeam;
            });
            // Create a list of secondary care team members
            self.secondaryCareTeam = ko.computed(function () {
                // Create an empty array to fill with problems
                var thisCareTeam = [];
                // Sort them
                var searchCareTeam = self.careMembers().sort(alphabeticalSort);
                // Create a filtered list of care teams,
                ko.utils.arrayForEach(searchCareTeam, function (careMember) {
                    // If they are not part of the primary care team,
                    if (!careMember.primary()) {
                        // Make them part of the secondary team
                        thisCareTeam.push(careMember);
                    }
                });
                return thisCareTeam;
            });
        };

        ctor.prototype.canReassignToMe = function () {
            var self = this;
            if (self.primaryCareTeam().length > 0) {
                // var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                //     return caremanager.id() === session.currentUser().userId();
                // });
                return (self.primaryCareTeam().length > 0 && (self.primaryCareTeam()[0].contactId() !== session.currentUser().userId()));
            }
            return false;
        }

        ctor.prototype.assignToMe = function () {
            var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });
                var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient.id(), preferredName: thisMatchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: session.currentUser().userId() });
                datacontext.saveCareMember(thisCareMember, 'Insert');
            }
        };
        
        ctor.prototype.reassignToMe = function () {
            var self = this;
            // Get the care manager type
            var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                return cmType.name() === 'Care Manager';
            });
            if (careMemberType) {
                var thisMatchedCareManager = ko.utils.arrayFirst(datacontext.enums.careManagers(), function (caremanager) {
                    return caremanager.id() === session.currentUser().userId();
                });
                // Grab the first primary listed care member
                var thisCareMember = ko.utils.arrayFirst(self.selectedPatient.careMembers(), function (ctMember) {
                    return ctMember.primary();
                });
                thisCareMember.preferredName(thisMatchedCareManager.preferredName());
                thisCareMember.gender('n');
                thisCareMember.contactId(thisMatchedCareManager.id());
                datacontext.saveCareMember(thisCareMember, 'Update');
            }
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });