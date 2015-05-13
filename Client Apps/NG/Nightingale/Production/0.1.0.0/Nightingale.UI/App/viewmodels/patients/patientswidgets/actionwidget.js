define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeAction = self.settings.data.column.activeAction;
            self.column = self.settings.data.column;
            self.content = self.settings.data.content;
            self.isOpen = ko.observable(false);
            self.isFullScreen = ko.observable(false);
            self.toggleWidgetOpen = function () {
                self.isOpen(!self.isOpen());
            };
            self.toggleFullScreen = function () {
                self.isFullScreen(!self.isFullScreen());
            };
            self.toggleColumn = function () {
                self.column.isOpen(!self.column.isOpen());
            };
        };

        ctor.prototype.attached = function () {

        };

        ctor.prototype.completeAction = function (action) {
            datacontext.saveAction(action);
        };

        return ctor;
    });