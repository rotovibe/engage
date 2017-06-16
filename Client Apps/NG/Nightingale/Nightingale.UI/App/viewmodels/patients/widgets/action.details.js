define(['services/datacontext', 'viewmodels/patients/careplan/index'],
    function (datacontext, carePlanIndex) {

        function tab (name, path, activeTab) {
            var self = this;
            self.name = ko.observable(name);
            self.path = ko.observable(path);
            self.isActive = ko.computed(function () {
                var thisActiveTab = activeTab();
                return (thisActiveTab && thisActiveTab.name() === self.name());
            });
        };

        var ctor = function () {
            var self = this;
            self.activeTab = ko.observable();
            self.tabs = ko.observableArray([
                new tab('Steps', 'viewmodels/patients/tabs/action.steps', self.activeTab),
                new tab('Details', 'viewmodels/patients/tabs/action.details', self.activeTab)
            ]);
            self.actionActions = ko.observableArray([{ text: ko.observable('Repeat') }]);
            self.selectedAction = ko.observable();
            self.selectedHistoricalAction = ko.observable();
            self.programsSaving = ko.computed(datacontext.programsSaving);			
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeAction = self.settings.activeAction;
            // Change tab to default tab when action changes
            self.activeActionToken = self.activeAction.subscribe(function () {
                self.activeTab(self.tabs()[0]);
            });
			self.disableActionActions = ko.computed( function(){
				var isSaving = datacontext.programsSaving();
				var activeAction = self.activeAction();
				var isLoading = activeAction? activeAction.isLoading() : false;
				return isSaving || isLoading;
			});
            self.selectedAction.subscribe(function (newValue) {
                if (newValue && newValue.text() === 'Repeat') {
                    if (self.activeAction().completed()  && !datacontext.programsSaving()) {
                        datacontext.repeatAction(self.activeAction()).then(repeatCompleted);
                        self.selectedAction(null);
                    } else {
                        datacontext.createAlert(0, 'The action cannot be repeated');
                        self.selectedAction(null);
                    }
                }

                function repeatCompleted() {
                    var repeatedAction = datacontext.getRepeatedAction(self.activeAction());
                    self.activeAction(repeatedAction);
                }
            });
            self.availableHistoricalActions = ko.computed(function () {
                var tempArray = [];
                if (self.activeAction()) {
                    tempArray = self.activeAction().history();
                }
                return tempArray;
            });
            self.selectedHistoricalAction(self.availableHistoricalActions()[0]);
            self.availableHistoricalActionsComputed = ko.computed(function () {
                // Subscribe to availableHistoricalActions
                var theseactions = self.availableHistoricalActions();
                if (theseactions.length > 0) {
                    self.selectedHistoricalAction(theseactions[0]);                    
                }
                return false;
            }).extend({ throttle: 50 });
            self.selectedActionToken = self.selectedHistoricalAction.subscribe(function (newValue) {
                // Appears to be setting the value too quickly and the new available values haven't loaded, HACK
                setTimeout(function () {
                    if (newValue && newValue.steps().length === 0) {
                        carePlanIndex.getStepsForAction(newValue);
                    }
                }, 50);
            });
            self.isFullScreen = ko.observable(false);
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.setActiveTab = function (sender) {
                self.activeTab(sender);
            };
            self.activeTab(self.tabs()[0]);
        };

        ctor.prototype.attached = function () {

        };

        ctor.prototype.deactivate = function () {
            var self = this;
            if (self.selectedActionToken) {
                self.selectedActionToken.dispose();
            }
            if (self.activeActionToken) {
                self.activeActionToken.dispose();
            }
        }
		
		ctor.prototype.detached = function() {
			var self = this;
			self.availableHistoricalActionsComputed.dispose();
			self.availableHistoricalActions.dispose();
			self.disableActionActions.dispose();
			self.programsSaving.dispose();
			ko.utils.arrayForEach( self.tabs, function( tab ){
				tab.isActive.dispose();
			});
		}
        return ctor;
    });