define(['services/datacontext'],
  function (datacontext) {

      var alphabeticalSort = function (l, r) { return (l.preferredName() == r.preferredName()) ? (l.preferredName() > r.preferredName() ? 1 : -1) : (l.preferredName() > r.preferredName() ? 1 : -1) };

      var ctor = function () {
          var self = this;
      };

      ctor.prototype.careManagers = ko.computed(datacontext.enums.careManagers);
      // ctor.prototype.deleteFocusProblem = function (sender) {
      //     sender.entityAspect.rejectChanges();
      //     sender.deleteFlag(true);
      // };
      // ctor.prototype.observationStates = ko.computed(function () {
      //     // Find problem observation type from list
      //     var problemType = ko.utils.arrayFirst(datacontext.enums.observationTypes(), function (type) {
      //         return type.name() === 'Problems';
      //     });
      //     // Filter states down to only those who have allowed type id that contains the problem type id
      //     var theseStates = ko.utils.arrayFilter(datacontext.enums.observationStates(), function (state) {
      //         return state.allowedTypeIds().indexOf(problemType.id()) !== -1;
      //     });
      //     return theseStates;
      // });

      ctor.prototype.activate = function (settings) {
          var self = this;
          self.settings = settings;
          self.showing = ko.computed(function () { return true; });
          self.selectedPatient = ko.computed(self.settings.selectedPatient);
          self.canSave = self.settings.canSave;
          self.saveType = self.settings.saveType;
          self.primaryCareManager = ko.computed(function () {
            var matchedCareManager = ko.utils.arrayFirst(self.selectedPatient().careMembers(), function (cm) {
              return cm.primary();
            });
            return matchedCareManager;
          });
          self.newAssignedTo = ko.observable();
          self.careManagers = datacontext.enums.careManagers;
          // Set the displayed value to the current assign to
          self.assignToDisplay = ko.observable();
          self.assignToDisplay.subscribe(function (newValue) {
              // else, Find the care manager that matches
              var matchedCareManager = ko.utils.arrayFirst(self.careManagers(), function (cm) {
                  return cm.preferredName() === newValue;
              });
              // If a match is found,
              if (matchedCareManager) {
                // If we already have a primary,
                if (self.primaryCareManager()) {
                  // Overwrite it
                  self.primaryCareManager().preferredName(matchedCareManager.preferredName());
                  self.primaryCareManager().gender('n');
                  self.primaryCareManager().contactId(matchedCareManager.id());
                  self.saveType('Update');
                } else {
                  // Get the care manager type
                  var careMemberType = ko.utils.arrayFirst(datacontext.enums.careMemberTypes(), function (cmType) {
                      return cmType.name() === 'Care Manager';
                  });
                  // Else, create a new one
                  var thisCareMember = datacontext.createEntity('CareMember', { id: -1, patientId: self.selectedPatient().id(), preferredName: matchedCareManager.preferredName(), typeId: careMemberType.id(), gender: 'n', primary: true, contactId: matchedCareManager.id() });
                  self.saveType('Insert');
                }
                self.canSave(true);
                // self.action().assignTo(matchedCareManager);
              } else {
                console.log('No match found');
              }
          });
          self.checkForMatch = function () {
              return self.assignToDisplay() === (self.primaryCareManager() ? self.primaryCareManager().preferredName() : '');                
          }
          // self.removeAssignment = function () {
          //     self.primaryCareManager()(null);
          //     self.assignToDisplay('');
          // }
          self.validMatch = ko.computed(function () {
              var result = false;
              if (self.primaryCareManager()) {
                  // Check if it matches a valid value
                  result = self.checkForMatch();
                  // If there is an invalid value,
                  if (!result) {
                      // if the assign to id has been changed,
                      if (self.primaryCareManager() && self.primaryCareManager().id()) {
                          // Reset the value
                          self.assignToDisplay(self.primaryCareManager() ? self.primaryCareManager().preferredName() : '');
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
          // A list of the problem observations for this patient
          // self.computedProblemObservations = ko.computed(function () {
          //     var filteredObservations = [];
          //     var patientsObservations = self.selectedPatient().observations();
          //     // Filter the list only to observations that are of type 'Problem'
          //     filteredObservations = ko.utils.arrayFilter(patientsObservations, function (item) {
          //         // If the item has a type, return if it matches problem, else if there is no type return false
          //         return item.type() ? (item.type().name() === 'Problems' && !item.deleteFlag() && (item.entityAspect.entityState.isModified() || item.state().name().toLowerCase() === 'active')) : false;
          //     }).sort(self.newThenAlphabeticalSort);
          //     return filteredObservations;
          // }).extend({ throttle: 50 });
          // self.showActions = self.settings.hasOwnProperty('showActions') ? self.settings.showActions : self.showing;
          // self.saveFocusProblems = self.settings.saveFocusProblems || function () { return false; };
          // self.cancelFocusProblems = self.settings.cancelFocusProblems || function () { return false; };
      };

      ctor.prototype.attached = function () {

      };

      return ctor;
  });