define(['models/base', 'config.services', 'services/datacontext', 'services/session', 'viewmodels/patients/medications/index', 'viewmodels/shell/shell', 'viewmodels/patients/data/index', 'viewmodels/patients/index'],
    function (modelConfig, servicesConfig, datacontext, session, medicationsIndex, shell, dataIndex, patientsIndex) {

        var ctor = function () {
            var self = this;
        };

        var descendingDateSort = function (l, r) { return (l.medSortDate() == r.medSortDate()) ? (l.medSortDate() < r.medSortDate() ? 1 : -1) : (l.medSortDate() < r.medSortDate() ? 1 : -1) };

        ctor.prototype.activate = function (settings) {
            var self = this;
            self.settings = settings;
            self.selectedPatient = self.settings.selectedPatient;
            self.widget = self.settings.widget;
            self.defaultSort = self.settings.defaultSort;
            self.medicationGroups = self.widget.medicationGroups;
            self.allowAdd = self.widget.allowAdd;
                patientsIndex.getPatientMedications();
            };
            self.addMedication = function () {
                dataIndex.addData();
            };
            self.medicationSaving = ko.computed(datacontext.medicationSaving);
            self.selectedSortColumn = ko.observable();
            self.activeColumns = ko.computed(function () {
                return ['expand','name', 'sortdate', 'status'];
            });
            self.toggleSort = function (sender) {
                if (self.selectedSortColumn() && self.selectedSortColumn().indexOf(sender.sortProperty) !== -1) {
                    if (self.selectedSortColumn() && self.selectedSortColumn().substr(self.selectedSortColumn().length - 4, 4) === 'desc' ) {
                        self.selectedSortColumn(null);
                    } else {
                        self.selectedSortColumn(sender.sortProperty + ' desc');
                    }
                } else {
                    self.selectedSortColumn(sender.sortProperty);
                }
            };
            self.isOpen = self.widget.isOpen;
            self.isFullScreen = self.widget.isFullScreen;
            self.filtersHeaderOpen = ko.observable(true);
            self.filtersOpen = ko.observable(false);
            self.toggleHeaderOpen = function  (sender, widgetOpen) {
                if (widgetOpen()) {
                    sender(!sender());
                }
            }
            self.toggleOpenColumn = function () {
                self.widget.column.isOpen(!self.widget.column.isOpen());
            }
            self.toggleFullScreen = function (sender) {
                self.isFullScreen(!self.isFullScreen());
            }
            self.toggleWidgetOpen = function (sender) {
                var openwidgets = ko.utils.arrayFilter(self.widget.column.widgets(), function (wid) {
                    return wid.isOpen();
                });
                if (openwidgets.length === 1 && openwidgets[0] === self.widget) {
                } else {
                    sender.isOpen(!sender.isOpen());
                }
            };
            self.addMedication = function (goal) {
                dataIndex.addData();
            };
            self.minimizeThisColumn = function () {
                self.widget.column.isOpen(false);
            }
            self.maximizeThisColumn = function () {
                self.widget.column.isOpen(true);
            }

        };

        ctor.prototype.toggleExpand = function (sender) {
            sender.isExpanded(!sender.isExpanded());
            if (!sender.isOpen()) {
                sender.isOpen(true);
            }
        };

        ctor.prototype.detached = function(){
            var self = this;
            self.medicationSaving.dispose();
            self.activeColumns.dispose();
        }
        return ctor;

    });;