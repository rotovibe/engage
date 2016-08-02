define(['durandal/composition'], function (composition) {

    var ctor = function () {
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.canClose = ko.observable(false);
        self.showing = ko.observable(false);
        self.options = self.settings.options;
        self.selectedValue = self.settings.value;
        self.text = self.settings.text;
        self.label = self.settings.label;
        self.idValue = self.settings.idValue;
        self.computedOptions = ko.computed(function () {
            var thisList = ko.unwrap(self.options);
            ko.utils.arrayForEach(thisList, function (item) {
                item.thisText = ko.computed(item[self.text]);
            });
            return thisList;
        });
        self.computedLabel = ko.computed(function () {
            if (self.selectedValue()) {
                if (self.idValue) {
                    var thisList = ko.unwrap(self.options);
                    var thisItem = ko.utils.arrayFirst(thisList, function (item) {
                        return (item[self.idValue]() === self.selectedValue());
                    });
                    if (!thisItem) {
                        return null;
                    }
                    return thisItem[self.text]();
                }
                return self.selectedValue()[self.text]();
            }
            return 'Choose...';
        });
        self.selectOption = function (sender) {
            // If there is an idValue to reference as the selected value,
            if (self.idValue) {
                // Set the value of that property to selectedValue
                self.selectedValue(sender[self.idValue]());
            }
            // Or else,
            else {
                // Set directly to the value of the sender
                self.selectedValue(sender);
            }
            self.showing(false);
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
});