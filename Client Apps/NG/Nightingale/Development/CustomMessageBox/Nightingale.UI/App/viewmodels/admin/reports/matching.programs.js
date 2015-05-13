define([], function () {

    var ctor = function () {
        var self = this;
    };

    var dateSort = function (l, r) { return (l.duration() == r.duration()) ? (l.duration() > r.duration() ? 1 : -1) : (l.duration() > r.duration() ? 1 : -1) };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.data = self.settings.data.data;
        // Programs list
        self.programs = self.data.sort(dateSort);
        self.computedPrograms = ko.computed(function () {
            var thesePrograms = self.programs;
            ko.utils.arrayForEach(thesePrograms, function (prog) {
                // If the patient doesn't already have a 'selected' property
                if (prog.selected === undefined) {
                    // Give it one
                    prog.selected = ko.observable(false);
                }
            });
            return thesePrograms;
        }).extend({ throttle: 50 });
        self.allSelected = ko.computed({
            read: function () {
                var allAreSelected = true;
                ko.utils.arrayForEach(self.computedPrograms(), function (thisProg) {
                    var isselected = thisProg.selected();
                    if (!isselected) {
                        allAreSelected = false;
                    }
                });
                return allAreSelected;
            },
            write: function (newValue) {
                if (newValue) {
                    ko.utils.arrayForEach(self.computedPrograms(), function (prog) {
                        prog.selected(true);
                    });
                } else {
                    ko.utils.arrayForEach(self.computedPrograms(), function (prog) {
                        prog.selected(false);
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
        var table_div = document.getElementById('matching-programs');
        var table_html = table_div.outerHTML.replace(/ /g, '%20');
        a.href = data_type + ', ' + table_html;
        //setting the file name
        a.download = 'exported_table.xls';
        a.click();
    };

    ctor.prototype.sendCleanCSV = function () {
        var self = this;
        var a = document.createElement('a');
        var thisString = '';
        var colDelim = '","';
        var rowDelim = '"\r\n"';
        var thisString = 'Individual Name' + colDelim + 'Program Name' + colDelim + 'Start Date' + colDelim + 'Duration' + rowDelim;
        ko.utils.arrayForEach(self.computedPrograms(), function (program) {
            if (program.selected()) {
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