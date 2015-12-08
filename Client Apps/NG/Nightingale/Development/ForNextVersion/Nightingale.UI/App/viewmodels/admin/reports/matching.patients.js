define(['viewmodels/admin/reports/index'], function (reportsIndex) {

    var ctor = function () {
        var self = this;
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.data = self.settings.data.data;
        // Programs list
        self.patients = self.data;
        self.computedPatients = ko.computed(function () {
            var thesePatients = self.patients();
            ko.utils.arrayForEach(thesePatients, function (pat) {
                // If the patient doesn't already have a 'selected' property
                if (pat.selected === undefined) {
                    // Give it one
                    pat.selected = ko.observable(false);
                }
            });
            return thesePatients;
        }).extend({ throttle: 50 });
        self.goalsLoaded = ko.computed(function () {
            return reportsIndex.goalsList().length > 0;
        });
        self.programsLoaded = ko.computed(function () {
            return reportsIndex.programsList().length > 0;
        });
        self.allSelected = ko.computed({
            read: function () {
                var allAreSelected = true;
                ko.utils.arrayForEach(self.computedPatients(), function (thisPat) {
                    var isselected = thisPat.selected();
                    if (!isselected) {
                        allAreSelected = false;
                    }
                });
                return allAreSelected;
            },
            write: function (newValue) {
                if (newValue) {
                    ko.utils.arrayForEach(self.computedPatients(), function (pat) {
                        pat.selected(true);
                    });
                } else {
                    ko.utils.arrayForEach(self.computedPatients(), function (pat) {
                        pat.selected(false);
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

    ctor.prototype.sendToExcel = function () {
        var self = this;
        var a = document.createElement('a');
        var data_type = 'data:application/vnd.ms-excel';
        var table_div = document.getElementById('matching-patients');
        var table_html = table_div.outerHTML.replace(/ /g, '%20');
        a.href = data_type + ', ' + table_html;
        //setting the file name
        a.download = 'exported_table.xls';
        a.click();
    };

    ctor.prototype.sendCleanCSV = function () {
        var self = this;
        var a = document.createElement('a');
        var colDelim = '","';
        var rowDelim = '"\r\n"';
        var thisString = 'Individual Name' + colDelim + 'Gender' + colDelim + 'DOB' + colDelim + '# of Goal' + colDelim + '# of Programs' + rowDelim;
        ko.utils.arrayForEach(self.computedPatients(), function (patient) {
            if (patient.selected()) {
                thisString = thisString + patient.fullName() + colDelim + patient.gender() + colDelim + patient.dOB() + colDelim + patient.goals().length + colDelim + patient.programs().length + rowDelim;
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