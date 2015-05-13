define(['durandal/composition'], function (composition) {

    var ctor = function () {
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.options = self.settings.options;
        self.selectedValue = self.settings.value;
        self.text = self.settings.text;
        self.label = self.settings.label;
        self.idValue = self.settings.idValue;
        self.disabled = self.settings.disabled;
        self.caption = self.settings.caption ? self.settings.caption : 'Choose one';
        self.computedOptions = ko.computed(function () {
            var thisList = ko.unwrap(self.options);
            ko.utils.arrayForEach(thisList, function (item) {
                // Create a property to dynamically set the showing property
                if (ko.isObservable(item[self.text])) {
                    item.thisText = ko.computed(item[self.text]);
                } else {
                    item.thisText = ko.computed(function () { return item[self.text]; });
                }
            });
            return thisList;
        });
    };

    ctor.prototype.attached = function () {
    };

    return ctor;
});