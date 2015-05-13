define(['services/navigation'],
    function (navigation) {

        var widgets = ko.observableArray();
        var initialized = false;

        var columns = ko.computed(function () {
            var self = this;
            self.columns = navigation.currentSubRoute().columns;
            ko.utils.arrayForEach(self.columns, function (thisColumn) {
                var theseWidgets = [];
                ko.utils.arrayForEach(thisColumn.widgets, function (thisWidget) {
                    thisWidget.column = thisColumn;
                    theseWidgets.push(thisWidget);
                });
                thisColumn.activeWidgets = ko.observableArray(theseWidgets);
            });
            return self.columns;
        });

        var computedOpenColumn = ko.observable();

        var vm = {
            activate: activate,
            attached: attached,
            computedOpenColumn: computedOpenColumn,
            columns: columns,
            widgets: widgets,
            title: 'index'
        };

        return vm;

        function activate() {
        }

        function attached() {
            if (!initialized) {
                initialized = true;
            }
        }
    });