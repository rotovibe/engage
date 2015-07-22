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
            self.deleteNote = function (sender) {
                var thistype = sender.typeId() === '2' ? 'touchpoint' : 'note';
                var result = confirm('You are about to delete a ' + thistype + '.  Press OK to continue, or cancel to return without deleting.');
                // If they press OK,
                if (result === true) {
                    if (self.activeNote() === sender) {
                        self.activeNote(null);
                    }
                    datacontext.deleteNote(sender);
                }
                else {
                    return false;
                }
            }
            // Should the program be 'open' in the list?
            self.isOpen = ko.observable(true);
        };
        

        ctor.prototype.attached = function () {
        };

        return ctor;
    });