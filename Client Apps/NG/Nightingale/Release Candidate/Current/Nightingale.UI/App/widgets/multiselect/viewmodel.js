define(['durandal/composition'], function (composition) {

    var datacontext;

    var ctor = function () {
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.canClose = ko.observable(false);
        self.showing = ko.observable(false);
        self.options = self.settings.options;
        self.selectedValues = self.settings.values;
        self.isRequired = self.settings.isRequired;
        self.text = self.settings.text;
        self.label = self.settings.label;
        self.idValue = self.settings.idValue;
        self.stringValue = self.settings.stringValue;
        self.computedOptions = ko.computed(function () {
            var thisList = self.options();
            ko.utils.arrayForEach(thisList, function (item) {
                item.thisText = ko.computed(item[self.text]);
            });
            return thisList;
        });
        self.checkForIdValue = function (idvalue) {
            var thisMatch = ko.utils.arrayFirst(self.selectedValues(), function (selectedValue) {
                return selectedValue.id() === idvalue;
            });
            if (thisMatch) {
                return true;
            }
            return false;
        };
        self.computedLabel = ko.computed(function () {
            if (self.selectedValues()) {
                return self.selectedValues().length + ' selected.';
            }
            return 'Choose...';
        });
        self.selectOption = function (sender) {
            checkDataContext();
            // Use array first here to see if it is a duplicate
            var foundComplexType = ko.utils.arrayFirst(self.selectedValues(), function (selectedValue) {
                return selectedValue.id() === sender.id();
            });
            if (foundComplexType) {
                var thisIndex = self.selectedValues().indexOf(foundComplexType);
                if (thisIndex > -1) {
                    self.selectedValues.splice(thisIndex, 1);
                } else {
                    self.selectedValues.remove(foundComplexType);
                }
            }
            else {
                var theseParameters = { id: sender.id() };
                // Create one to use from the sender
                thisComplexType = datacontext.createComplexType('Identifier', theseParameters);
                self.selectedValues.push(thisComplexType);
            }
        };
        self.checkClosing = ko.computed(function () {
            if (self.canClose()) { self.showing(false); }
        }).extend({ throttle: 1000 });
        self.isDisabled = ko.computed(function () {
            var thisState = false;
            if (self.settings.disabled) {
                thisState = self.settings.disabled;
            }
            return thisState;
        });
        self.isInvalid = ko.computed(function () {
            return self.isRequired && self.selectedValues().length === 0;
        });
    };

    ctor.prototype.toggleDropdown = function () {
        var self = this;
        if (self.computedOptions().length !== 0 && !self.isDisabled()) {
            self.showing(!self.showing());
        }
    };

    ctor.prototype.startClosing = function () {
        var self = this;
        self.canClose(false);
        self.canClose(true);
    };

    ctor.prototype.stopClosing = function () {
        var self = this;
        self.canClose(false);
    };

    ctor.prototype.attached = function () {
    };

    return ctor;
    
    function checkDataContext() {
        if (!datacontext) {
            datacontext = require('services/datacontext');
        }
    }
});