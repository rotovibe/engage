define([],
    function () {

        var ctor = function () {
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.settings.isOpen = ko.observable();
            self.settings.isFullScreen = ko.observable(false);
            self.toggleWidgetOpen = function () {
                self.settings.isOpen(!self.settings.isOpen());
            };
            self.toggleFullScreen = function () {
                self.settings.isFullScreen(!self.settings.isFullScreen());
            };
        };

        ctor.prototype.attached = function () {
        };

        return ctor;
    });