define(['durandal/app', 'viewmodels/shell/shell', 'models/base', 'services/datacontext', 'services/formatter'],
	function (app, shell, modelConfig, datacontext, formatter) {

		var ctor = function () {

		};

		ctor.prototype.activate = function (settings) {
			var self = this;
			self.settings = settings;
			self.selectedPatient = self.settings.selectedPatient;
			self.editModalShowing = ko.observable(false);
			var modalSettings = {
				title: 'Edit Individual',
				showSelectedPatientInTitle: true,
				entity: self.selectedPatient, 
				templatePath: 'templates/patient.html', 
				showing: self.editModalShowing,
				classOverride: null
			}
			self.modal = new modelConfig.modal(modalSettings);
			self.isOpen = ko.observable(true);
			self.isEditing = ko.observable(false);
			self.toggleEditing = function () {
				if (self.isEditing()) {
					self.editModalShowing(false);
				}
				else {
					shell.currentModal(self.modal);
					self.editModalShowing(true);
					var editingToken = self.editModalShowing.subscribe(function () {
						self.isEditing(false);
						editingToken.dispose();
					});
				}
				self.isEditing(!self.isEditing());
				self.isOpen(true);
			};
			self.toggleFlagged = function () {
				// Get the current value of the flagged attribute of the patient
				var currentValue = self.selectedPatient.flagged();
				// Toggle the property
				currentValue = !currentValue;
				// And set the flagged property to the inverse
				self.selectedPatient.flagged(currentValue);
				var flagged = currentValue ? 1 : 0;
				datacontext.saveChangesToPatientProperty(self.selectedPatient, 'flagged', flagged, []);
			};
			self.showFullSSN = function () {
				datacontext.getFullSSN(self.selectedPatient.id()).then(ssnReturned);
				function ssnReturned(data) {
					if(data && data.SSN){
						var formattedNumber = formatter.formatSeparators(data.SSN.replace( /\D/g, ''), 'XXX-XX-XXXX', '-');
						alert('Full SSN: ' + formattedNumber);
					} else{
						alert('Full SSN is not available'); //1227
					}
				}
			};
			self.canViewFullSSN = ko.computed(function () {
				// Always return true for now,
				// until permissions are added
				return true;
			});

			self.primaryIdLabel = ko.observable();
			self.primaryIdTitle = ko.observable();
			self.primaryIdValue = ko.observable();
			self.primaryId = ko.computed( function(){
				var patient = self.selectedPatient;
				var patientSystemIds = self.selectedPatient.patientSystems();
				var primary = self.selectedPatient.getPrimaryPatientSystem();
				if(primary){
					self.primaryIdLabel( primary.system().displayLabel() );
					self.primaryIdTitle( primary.system().displayLabel() + ': '+ primary.value() );
					self.primaryIdValue( primary.value() );
				}
				return primary;
			});
		};

		return ctor;
	});
