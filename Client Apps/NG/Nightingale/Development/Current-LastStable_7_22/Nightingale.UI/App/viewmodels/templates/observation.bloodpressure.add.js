define(['services/datacontext'],
    function (datacontext) {

        var ctor = function () {
        	var self = this;
        };
        
        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.observation = self.settings.observation;
            self.diastolicObservationValue = ko.computed(function () {
                var diastolic = ko.utils.arrayFirst(self.observation.values(), function (value) {
                    return value.text().indexOf('Diast') !== -1;
                });
                if (diastolic) {
                    return diastolic;
                }
                return null;
            });
            self.systolicObservationValue = ko.computed(function () {
                var systolic = ko.utils.arrayFirst(self.observation.values(), function (value) {
                    return value.text().indexOf('Sys') !== -1;
                });
                if (systolic) {
                    return systolic;
                }
                return null;
            });
            self.customBPString = ko.computed(function () {
                var thisString = '';
                if (self.diastolicObservationValue() && self.systolicObservationValue()) {
                    // Create a string from it
                    var diasvalue = self.diastolicObservationValue().previousValue().value();
                    var systvalue = self.systolicObservationValue().previousValue().value();
                    var value = (!!diasvalue && !!systvalue) ? systvalue + '/' + diasvalue : '';
                    var startdate = moment(self.diastolicObservationValue().previousValue().startDate()).format('MM/DD/YYYY');
                    var unit = self.diastolicObservationValue().previousValue().unit();
                    var source = self.diastolicObservationValue().previousValue().source();
                    thisString = value ? value + ' ' + unit : '';
                    thisString = moment(startdate).isValid() ? (thisString ? thisString + ' on ' + startdate : startdate) : thisString;
                    thisString = source ? thisString + ' (' + source + ')' : thisString;
                }
                return thisString;
            });
            self.canAdd = ko.computed(function () {
                if (!!self.observation && !!self.diastolicObservationValue() && !!self.systolicObservationValue()) {
                    return (!!self.diastolicObservationValue().value() && !!self.systolicObservationValue().value() && moment(self.observation.startDate()).isValid());
                }
                return false;
            });
            self.addNew = self.settings.addNew;
        };

        ctor.prototype.attached = function () {

        };
        
        return ctor;
    });