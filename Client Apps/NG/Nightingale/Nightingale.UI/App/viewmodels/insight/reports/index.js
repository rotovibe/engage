//TODO: Inject dependencies
define(['services/session'],
    function (session) {

        // Give some initial values to speed up testing / demo'ing

        var userSettings = session.currentUser().settings();
        console.log(userSettings);

        var activeSecondColumn = ko.observable();
        var un = ko.utils.arrayFirst(userSettings, function (setting) {
            console.log(setting.Key)
            return setting.Key === "ReportingUser";
        }).Value;
        var org = ko.utils.arrayFirst(userSettings, function (setting) {
            return setting.Key === "ReportingOrg";
        }).Value;
        var pw = ko.utils.arrayFirst(userSettings, function (setting) {
            return setting.Key === "ReportingPW";
        }).Value;
        var repo = ko.utils.arrayFirst(userSettings, function (setting) {
            return setting.Key === "ReportingRepository";
        }).Value;
        var userName = ko.observable(un);
        var pwOrToken = ko.observable(pw);
        var organization = ko.observable(org);
        var reportPath = ko.observable();
        var containerToLoad = ko.observable();
        var repoName = ko.observable(repo);
        var availableReports = ko.observableArray();

        // Can the user get reports currently?
        var canGetReports = ko.computed(function () {
            // If there is a username, pw, and repo name they can
            return userName() && pwOrToken() && repoName();
        });

        var fetchingReport = ko.observable();

        // Reveal the bindable properties and functions
        var vm = {
            activate: activate,
            activeSecondColumn: activeSecondColumn,
            getReportsList: getReportsList,
            userName: userName,
            pwOrToken: pwOrToken,
            organization: organization,
            canGetReports: canGetReports,
            reportPath: reportPath, 
            containerToLoad: containerToLoad,
            showDynamicReport: showDynamicReport,
            repoName: repoName,
            availableReports: availableReports,
            fetchingReport: fetchingReport
        };

        return vm;
        
        // When we activate,
        function activate() {
            // Go get a list of reports from the current repo
            getReportsList();
            // And show the active report widget
            activeSecondColumn('viewmodels/insight/widgets/active.report');
            return true;
        }

        // Report object
        function Report(data) {
            var self = this;
            self.Name = ko.observable(data.label);
            self.Path = ko.observable(data.uri);
        }

        // Get a list of reports to choose to display
        function getReportsList () {
            visualize({
                auth: {
                    name: userName(),
                    password: pwOrToken(),
                    organization: organization()
                }
            }, function (v) {
                v.resourcesSearch({
                    folderUri: repoName(),
                    recursive: false,
                    success: function (repo) {
                        // Clear the reports each time we get data back
                        availableReports([]);
                        // For each returned item,
                        $.each(repo, function (index, item) {
                            // If it is a report 
                            if (item.resourceType === "reportUnit") {
                                // Add it to the list of available reports
                                availableReports.push(new Report(item));
                            }
                        });
                    }
                });
            });
        }

        // Show a dynamic report based on the current selections
        function showDynamicReport () {
            fetchingReport(true);
            visualize({
                auth: {
                    name: userName(),
                    password: pwOrToken(),
                    organization: organization()
                }
            }, function (v) {
                var report = v.report({
                    resource: reportPath(),
                    container: containerToLoad(),
                    success: function () {
                        fetchingReport(false);
                    },
                    error: function (err) {
                        // Display if errors occur
                        console.log(err.message);
                    }
                });
            });
        }

    });