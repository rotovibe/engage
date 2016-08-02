define(['services/datacontext'],
    function (datacontext) {

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
                new tab('Details', 'viewmodels/patients/tabs/module.details', self.activeTab)
            ]);
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeModule = self.settings.activeModule;
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

        return ctor;
    });