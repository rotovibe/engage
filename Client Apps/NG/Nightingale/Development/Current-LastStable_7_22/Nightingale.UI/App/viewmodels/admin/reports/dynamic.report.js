define(['viewmodels/admin/reports/index'], function (reportsIndex) {

    var ctor = function () {
        var self = this;
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.data = self.settings.data;
        self.report = self.data.data();

        // Programs list
        //self.patients = self.data;

        self.computedEntities = ko.computed(function () {
            var theseEntities = self.report.entities();
            ko.utils.arrayForEach(theseEntities, function (pat) {
                // If the patient doesn't already have a 'selected' property
                if (pat.selected === undefined) {
                    // Give it one
                    pat.selected = ko.observable(false);
                }
            });
            return theseEntities;
        }).extend({ throttle: 50 });

        self.allSelected = ko.computed({
            read: function () {
                var allAreSelected = true;
                ko.utils.arrayForEach(self.computedEntities(), function (thisPat) {
                    var isselected = thisPat.selected();
                    if (!isselected) {
                        allAreSelected = false;
                    }
                });
                return allAreSelected;
            },
            write: function (newValue) {
                if (newValue) {
                    ko.utils.arrayForEach(self.computedEntities(), function (pat) {
                        pat.selected(true);
                    });
                } else {
                    ko.utils.arrayForEach(self.computedEntities(), function (pat) {
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

    ctor.prototype.sendCleanCSV = function () {
        var self = this;
        var a = document.createElement('a');
        var colDelim = '","';
        var rowDelim = '"\r\n"';
        var thisString = '';
        thisString = thisString + self.report.name + rowDelim;
        thisString = thisString + self.report.description + rowDelim;
        thisString = thisString + moment().format('LLLL') + rowDelim;
        ko.utils.arrayForEach(self.report.columns, function (column) {
            thisString = thisString + column.displayName + colDelim;
        });
        thisString = thisString + rowDelim;
        ko.utils.arrayForEach(self.computedEntities(), function (entity) {
            if (entity.selected()) {
                ko.utils.arrayForEach(self.report.columns, function (column) {
                    if (column.name.indexOf('.') === -1 && column.name.indexOf('|') === -1) {
                        // It must not be delimited
                        thisString = thisString + entity[column.name] + colDelim;
                    } else if (column.name.indexOf('|') === -1) {
                        // It must be period delimited
                        thisString = thisString + (entity[column.name.substr(0, column.name.indexOf('.'))] ? entity[column.name.substr(0, column.name.indexOf('.'))][column.name.substr(column.name.indexOf('.')+1, column.name.length)] : '-') + colDelim;
                    } else {
                        var propArray = column.name.split('|');
                        var currentProp = entity;
                        for (var i = propArray.length - 1; i >= 0; i--) {
                            //propArray[i]
                            // Get the second property from first property
                            currentProp = currentProp[propArray[0]];
                            if (currentProp === undefined) {
                                currentProp = '-';
                                i = 0;
                            }
                            propArray.shift();
                        };
                        // It must be pipe delimited
                        thisString = thisString + currentProp + colDelim;
                    }
                });
                thisString = thisString + rowDelim;
            }
        });

        blob = new Blob([thisString], { type: 'text/csv' }); //new way
        var csvUrl = URL.createObjectURL(blob);

        $(a).attr({
            'download': 'exported_report.csv',
            'href': csvUrl
        });
        a.click();
    };

    return ctor;
});