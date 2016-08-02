//TODO: Inject dependencies
define(['viewmodels/insight/reports/index'],
    function (insightReportIndex) {

        function Count(num) {
            var self = this;
            self.Number = num;
        }

        var numberOfContainers = ko.observableArray([
            new Count(1),
            new Count(2),
            new Count(3),
            new Count(4),
            new Count(5)
        ]);
        var selectedCount = ko.observable(numberOfContainers()[0]);

        function Container(name) {
            var self = this;
            self.Name = ko.observable(name);
        }

        var reportContainers = ko.computed(function () {
            var thisCount = selectedCount().Number;
            var thisArray = [];
            for (var i = thisCount - 1; i >= 0; i--) {
                thisArray.push(new Container('Container'+i+1));
            };
            return thisArray;
        });

        setActiveReportContainer(reportContainers()[0]);

        var activereport = {
            numberOfContainers: numberOfContainers,
            selectedCount: selectedCount,
            reportContainers: reportContainers,
            setActiveReportContainer: setActiveReportContainer
        };
        return activereport;

        // Set this report container as the active one to add report
        function setActiveReportContainer (sender) {
            insightReportIndex.containerToLoad('#' + sender.Name());
        }

    });