define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.activeDataType = self.settings.activeDataType;
            self.showing = ko.computed(function () { return !!self.activeDataType() });
            self.selectedPatient = self.settings.selectedPatient;
            self.showDropdown = self.settings.showDropdown || false;
            self.showActions = self.settings.hasOwnProperty('showActions') ? self.settings.showActions : self.showing;
            self.saveDataEntry = self.settings.saveDataEntry || function () { return false; };
            self.cancelDataEntry = self.settings.cancelDataEntry || function () { return false; };
            self.dataTypes = ko.computed(datacontext.enums.observationTypes);
            self.computedActiveDataType = ko.computed({
                read: function () {
                    return self.activeDataType();
                },
                write: function (newValue) {
                    self.activeDataType(newValue);
                }
            });
        };

        ctor.prototype.attached = function () {

        };

        return ctor;
    });