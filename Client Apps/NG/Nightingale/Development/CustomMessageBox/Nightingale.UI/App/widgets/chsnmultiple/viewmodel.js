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
        self.computedOptions = ko.computed(function () {
            var thisList = ko.unwrap(self.options);
            ko.utils.arrayForEach(thisList, function (item) {
                // Create a property to dynamically set the showing property
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
    };

    ctor.prototype.attached = function () {
    };

    return ctor;
});