define([], function () {

    var ctor = function () {
        var self = this;
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.data = self.settings.data.data;
        // Actions list
        self.actions = self.data;
        self.computedActions = ko.computed(function () {
            var theseActions = self.actions();
            ko.utils.arrayForEach(theseActions, function (action) {
                // If the patient doesn't already have a 'selected' property
                if (action.selected === undefined) {
                    // Give it one
                    action.selected = ko.observable(false);
                }
            });
            return theseActions;
        }).extend({ throttle: 50 });
        self.allSelected = ko.computed({
            read: function () {
                var allAreSelected = true;
                ko.utils.arrayForEach(self.computedActions(), function (thisAction) {
                    var isselected = thisAction.selected();
                    if (!isselected) {
                        allAreSelected = false;
                    }
                });
                return allAreSelected;
            },
            write: function (newValue) {
                if (newValue) {
                    ko.utils.arrayForEach(self.computedActions(), function (action) {
                        action.selected(true);
                    });
                } else {
                    ko.utils.arrayForEach(self.computedActions(), function (action) {
                        action.selected(false);
                    });
                }
            }
        }).extend({ throttle: 50 });
        self.allSelected(true);
    };

    ctor.prototype.closeReport = function () {
        var self = this;
        self.settings.data(null);
    };

    ctor.prototype.sendCleanCSV = function () {
        var self = this;
        var a = document.createElement('a');
        var thisString = '';
        var colDelim = '","';
        var rowDelim = '"\r\n"';
        var thisString = 'Individual Name' + colDelim + 'Action Name' + colDelim + 'Status' + colDelim + 'Start Date' + colDelim + 'Module Name' + colDelim + 'Program Name' + rowDelim;
        ko.utils.arrayForEach(self.computedActions(), function (action) {
            if (action.selected()) {
                thisString = thisString + action.module().program().patient().fullName() + colDelim + action.name() + colDelim + action.elementStateModel().name() + colDelim + action.attrStartDate() + colDelim + action.module().name() + colDelim + action.module().program().name() + rowDelim;
            //thisString = thisString + patient.fullName() + colDelim + patient.gender() + colDelim + patient.dOB() + colDelim + patient.goals().length + colDelim + patient.programs().length + rowDelim;
            }
        });
        blob = new Blob([thisString], { type: 'text/csv' }); //new way
        var csvUrl = URL.createObjectURL(blob);

        $(a).attr({
            'download': 'exported_table.csv',
            'href': csvUrl
        });
        a.click();
    };

    return ctor;
});