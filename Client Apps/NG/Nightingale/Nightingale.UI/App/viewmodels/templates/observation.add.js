define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
        	var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.observation = self.settings.observation;
            self.addNew = self.settings.addNew;
            self.canAdd = ko.computed(function () {
                if (self.observation && self.observation.computedValue() && self.observation.startDate()) {
                    return (!!self.observation.computedValue().value() && moment(self.observation.startDate()).isValid());
                }
            });
        };

        ctor.prototype.attached = function () {

        };
        
        return ctor;
    });