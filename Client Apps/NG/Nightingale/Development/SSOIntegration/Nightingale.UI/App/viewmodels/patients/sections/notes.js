define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

        var ctor = function () {
            var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.group = ko.observable(self.settings.group);
            self.activeNote = self.settings.activeNote;
            self.setActiveNote = function (sender) {
                self.activeNote(sender);
            };            
            // Should the program be 'open' in the list?
            self.isOpen = ko.observable(true);
        };
        

        ctor.prototype.attached = function () {
        };

        return ctor;
    });