/**
 * medication.edit manages patientMedication (add/edit) and also adding a new medication (medicationMap).
 * notes:
 *		1. the medication name is also used as a typeahead search and this needs to be changed when we will have to redesign for
 *			adding multiple medications. the preffered design is to follow allergies so that the search box is a separate tool
 *			and not part of the med record binding.
 *
 * @module medication.edit
 * @class medication.edit
 */

define(['models/base', 'config.services', 'services/datacontext', 'services/session'],
    function (models, servicesConfig, datacontext, session) {

    var subscriptionTokens= [];
    var newMedPostfix = ' (New)';

	//screen state management:
	var screenModes = {
		NoMedSelected: 'NoMedSelected',
		MedSelected: 'MedSelected',
		AddNewMed: 'AddNewMed',
		AddNewMedValues: 'AddNewMedValues'
	};

    // Create an end point to use
    var endpoint = new servicesConfig.createEndPoint('1.0', session.currentUser().contracts()[0].number(), 'Search/Meds/ProprietaryNames', 'PatientMedication');

    function DropDownValue (val, id) {
      var self = this;
      self.Text = val;
	  if(id){
		self.Value = id;
	  }
	  else{
		self.Value = val;
	  }
    }

    var ctor = function () {
      var self = this;
      self.dosageForms = ko.observableArray();
      self.strengths = ko.observableArray();
      self.units = ko.observableArray();
      self.routes = ko.observableArray();
	  self.frequency = ko.observableArray();

      self.checkForDropdownValues = function (med) {
        // Check if each of the dropdowns have values that can be set to prevent nulling the values when empty
        // If they are empty add the value as a possible value
        if (self.dosageForms.peek().length === 0 && med && med.form.peek()) {
          self.dosageForms([new DropDownValue(med.form.peek())]);
        }
        if (self.strengths.peek().length === 0 && med && med.strength.peek()) {
          self.strengths([new DropDownValue(med.strength.peek())]);
        }
        if (self.routes.peek().length === 0 && med && med.route.peek()) {
          self.routes([new DropDownValue(med.route.peek())]);
        }
      };
    };

    //ctor.prototype.freqHowOftens = ko.computed(datacontext.enums.freqHowOftens);	//old freq options

    ctor.prototype.freqWhens = ko.computed(datacontext.enums.freqWhens);
    ctor.prototype.sources = ko.computed(datacontext.enums.allergySources);
    ctor.prototype.types = ko.computed(datacontext.enums.medSuppTypes);
    ctor.prototype.categories = ko.computed(datacontext.enums.medicationCategories);
    ctor.prototype.statuses = ko.computed(datacontext.enums.medicationStatuses);
    ctor.prototype.durationUnits = ko.computed(datacontext.enums.durationUnits);
    ctor.prototype.refusalReasons = ko.computed(datacontext.enums.refusalReasons);
    ctor.prototype.medicationReviews = ko.computed(datacontext.enums.medicationReviews);

    ctor.prototype.removeMedication = function (sender) {
      sender.entityAspect.rejectChanges();
      sender.deleteFlag(true);
    };
	ctor.prototype.getFrequencyOptions = function (frequencies, selectedPatient){
		return datacontext.getPatientFrequencies(frequencies, selectedPatient().id());
	}
    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.initialized = false;
	  self.lastMedName = '';
      self.selectedPatient = self.settings.selectedPatient;
      // Passed in medication
      self.medication = self.settings.medication;
      self.showing = ko.computed({
        read: function () { return true; },
      });
      self.getFrequencyOptions(self.frequency, self.selectedPatient);
	  self.isCreateNewFrequencyEnabled = ko.observable(true);

      /**
       * computed - get/observe the new medication model object for this patient
       *
       * @method newPatientMedication
       * @return {Object} the new patient medication for this screen
       */
      self.newPatientMedication = ko.computed({
        read: function () {

          // if a medication gets passed in, use that
          if (self.medication && self.medication()) {
            self.checkForDropdownValues(self.medication());
            return self.medication();
          } else {
            // Else use the patients medications to find one
            var patientMedications = self.selectedPatient().medications();
            // Filter the list only to patientMedications that are new
            var filteredPatMed = ko.utils.arrayFirst(patientMedications, function (item) {
              // If the item has a type, return if it matches problem, else if there is no type return false
              return !item.deleteFlag.peek() && item.isNew.peek();
            });
            self.checkForDropdownValues(filteredPatMed);
            return filteredPatMed;
          }
        }
      }).extend({ throttle: 50 });


      // The selected observation
      self.trimmedMedication = ko.computed({
        read: function () {
          var thismed = self.newPatientMedication;
          if (thismed()) {
            return thismed().name();
          } else {
            return '';
          }
        }
      }).extend({ throttle: 75 });

	  self.trimmedForm = ko.computed({
        read: function () {
          var thismed = self.newPatientMedication;
          if (thismed() && thismed().form()) {
            return thismed().form();
          } else {
            return '';
          }
        }
      }).extend({ throttle: 75 });

	  self.trimmedStrength = ko.computed({
        read: function () {
          var thismed = self.newPatientMedication;
          if (thismed() && thismed().strength()) {
            return thismed().strength();
          } else {
            return '';
          }
        }
      }).extend({ throttle: 75 });

	  self.trimmedRoute = ko.computed({
        read: function () {
          var thismed = self.newPatientMedication;
          if (thismed() && thismed().route()) {
            return thismed().route();
          } else {
            return '';
          }
        }
      }).extend({ throttle: 75 });

      self.resultsMessage = ko.observable('');
      if (self.medication && !self.medication()) {
        // Initialize this widget
        self.initializeMedSearch();
      }

	  self.isDuplicate = ko.observable(false);
	  self.isAddingNewRouteValue = ko.observable(false);
      self.isAddingNewFormValue = ko.observable(false);
      self.isAddingNewStrengthValue = ko.observable(false);
	  self.isAddingNewFrequencyValue = ko.observable(false);
      self.isCreateNewEnabled = ko.observable(false);
      self.isDropdownEnabled = ko.observable(false);
	  self.isNewMedicationName = ko.observable(false);

		/**
		*	computed. to allow forcing the datetimepicker control to set the start date as invalid.
		*	this is needed when the date is valid but range is wrong.
		*	@method setInvalidStartDate
		*/
	  self.setInvalidStartDate = ko.computed( function(){
			return (!self.newPatientMedication() || self.newPatientMedication() && self.newPatientMedication().validationErrorsArray().indexOf('startDate') !== -1);
	  });
      self.setInvalidPrescribedDate = ko.computed( function(){
            return (!self.newPatientMedication() || self.newPatientMedication() && self.newPatientMedication().validationErrorsArray().indexOf('prescribedDate') !== -1);
      });
      self.setInvalidOrderedDate = ko.computed( function(){
            return (!self.newPatientMedication() || self.newPatientMedication() && self.newPatientMedication().validationErrorsArray().indexOf('orderedDate') !== -1);
      });
      self.setInvalidRxDate = ko.computed( function(){
            return (!self.newPatientMedication() || self.newPatientMedication() && self.newPatientMedication().validationErrorsArray().indexOf('rxDate') !== -1);
      });
		/**
		*	computed. to allow forcing the datetimepicker control to set the end date as invalid.
		*	this is needed when the date is valid but range is wrong.
		*	@method setInvalidEndDate
		*/
	  self.setInvalidEndDate = ko.computed( function(){
			return (!self.newPatientMedication() || self.newPatientMedication() && self.newPatientMedication().validationErrorsArray().indexOf('endDate') !== -1);
	  });

	  if( self.newPatientMedication() && self.newPatientMedication().name() && self.newPatientMedication().canSave() ){
		//a med is selected, going back to medications screen
		self.lastMedName = self.newPatientMedication().name();
		self.screenMode = ko.observable(screenModes.MedSelected);
	  }
	  else{
		self.lastMedName = '';
		self.screenMode = ko.observable(screenModes.NoMedSelected);
	  }

      /**
       * computed - controls if the save will skip and ignor or call the api services
       * the value tracks the screen mode and will allow saving only if a medication
       * (new or existing) was selected from the typeahead medication name suggestions.
       *
       * @method canAdd
       * @return boolean
       */
      self.canAdd = ko.computed({
        read: function () {

          var thisValue = false;
          var medSaving = datacontext.medicationSaving();
          var trimMed = self.trimmedMedication();
          var patientmedications = self.selectedPatient().medications();
		  var mode = self.screenMode;
		  var isDup = self.isDuplicate();

          // If a value has been selected,
          if (trimMed) {
              thisValue = true;
          }
          if(mode && mode() == screenModes.NoMedSelected){
              thisValue = false;  //dont save if no medication selected or if not in add new mode
          }
          else{
            thisValue = true;
          }
          // If med is already saving, return false
          if (medSaving) { thisValue = false; }
          if (self.newPatientMedication.peek()) {
            if(isDup){
                thisValue = false;
            }
            self.newPatientMedication().canSave(thisValue);
          }
          return thisValue;
        }
      }).extend({ throttle: 200 });

	  self.medicationBloodhound = new Bloodhound({
        datumTokenizer: function (d) {
          return Bloodhound.tokenizers.whitespace(d.name());
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        limit: 17,
        remote: {
          url: servicesConfig.remoteServiceName + '/' + endpoint.ResourcePath,
          replace: function (url, query) {
            searchQuery = query;
            return url + '?Take=15&Term=' + searchQuery;
          },
          ajax: {
            beforeSend: function (jqXhr, settings) {
              jqXhr.setRequestHeader('Content-Type', 'application/json; charset=UTF-8');
              jqXhr.setRequestHeader('Token', session.currentUser().aPIToken());
            },
            type: 'GET'
          },
          // Filter out what is returned from the database
          filter: function (parsedResponse) {
            // If there is a message returned (such as 15 of 1205 results),
            if (parsedResponse.Message) {
              self.resultsMessage('');
              // Set it as the results message
              self.resultsMessage(parsedResponse.Message);
            } else {
              // Else clear it
              self.resultsMessage('');
            }
            self.medicationBloodhound.clear();
            var selectedmedication = self.trimmedMedication().trim();	//full trim the typed value
            self.newPatientMedication().name(self.ltrim(self.newPatientMedication().name()));	//force left trim to prevent duplicates, but allow space from the right for entering multiple words.

            ko.utils.arrayForEach(parsedResponse.ProprietaryNames, function (med) {
              med.DisplayName = med.Text;
              // Check if the medication has already been added to the individual
              var matchingMedication = ko.utils.arrayFirst(self.selectedPatient().medications(), function (patmed) {
                return patmed.name().toLowerCase().trim() === med.Text.toLowerCase().trim() && patmed.statusId() == 1;
              });
              // Make sure this is actually a duplicate!!!!!
              if ((matchingMedication && matchingMedication !== self.newPatientMedication())) {
                med.DisplayName = '(Duplicate) ' + med.DisplayName;
                med.isDuplicate = true;
              }
            });
            //prepare a new medication suggestion: use the full trimmed typed value:
            var newMedication = { Id: -1, Text: selectedmedication, DisplayName: selectedmedication + newMedPostfix, Value: selectedmedication };
            // Check if the results match the text of the fully trimmed typed value:
            var matchingResponse = ko.utils.arrayFirst(parsedResponse.ProprietaryNames, function (med) {
                return med.Text.toLowerCase().trim() === selectedmedication.toLowerCase();
            });
            //If it doesn't match anything else, add a new one
            if (!matchingResponse) {
                parsedResponse.ProprietaryNames.push(newMedication);
            }
            return parsedResponse.ProprietaryNames;
          },
          rateLimitWait: 500,
          cache: false
        }
      });

	  /**
	  *	left trim a string. (trimLeft() wont work on ie)
	  *	@method self.ltrim
	  *	@param str {string}
	  *	@return the trimmed result string.
	  */
	  self.ltrim = function(str){
			if(str){
				return str.replace(/^\s+/, "");
			}
			else return str;
	  }
      self.addNewOptionText = '-Add New-';

      self.resetDropdowns = function () {
          if(self.newPatientMedication && self.newPatientMedication()){
              self.newPatientMedication().strength('');
              self.newPatientMedication().form('');
              self.newPatientMedication().route('');
              self.newPatientMedication().familyId('');
			  self.newPatientMedication().customFrequency('');
			  self.newPatientMedication().frequency(null);
			  self.isAddingNewFrequencyValue(false);	//frequency gets back to dropdown regardless if its new/edit med.
          }
          if(self.screenMode() !== screenModes.AddNewMed){
              self.isAddingNewRouteValue(false);
              self.isAddingNewStrengthValue(false);
              self.isAddingNewFormValue(false);
              if(self.screenMode() === screenModes.AddNewMedValues){
                  self.screenMode(screenModes.MedSelected);
				  self.newPatientMedication().isCreateNewMedication(false);
              }
          }
      };

      self.clearDropdowns = function() {
        self.resetDropdowns();
        self.dosageForms([]);
        self.strengths([]);
        self.routes([]);
      };

      self.ignoreMedicationNameChange = function(currentName, newName){
          //scenarios to ignore the medication name change:
		  //ignore if edit mode assignes the medication name.
		  //do not ignore if it was changed by the user after the medication was selected.
          var ignore = false;
		  if(currentName === null || currentName === '' || newName === null){
			  return true;
		  }
          if(currentName)
          {
              if(newName === currentName + newMedPostfix){
                  ignore = true; //a new med has been selected so its not new user input typing.
              }
              if(newName === currentName.toUpperCase()){
                  ignore = true;  //the medication name got to uppercase.
              }
          }
          return ignore;
      }

      /**
       * computed - tracks medication name changes and updates the screenMode value accordingly
       * when a name changes by the user it will clear and disable route/form/strength dropdowns
	   *
       * @method medicationNameWatcher
       * @return no return value.
       */

      self.medicationNameWatcher = ko.computed({
          read: function(){
			var medName = self.trimmedMedication();//newPatientMedication().name();
			var mode = self.screenMode;
			if( mode && mode() !== screenModes.NoMedSelected && self.initialized ){
				//we have a medication selected
				//track med name changes:
                if(self.ignoreMedicationNameChange(self.lastMedName, medName)){
                    return false;
                }
                self.lastMedName = medName;
				self.screenMode(screenModes.NoMedSelected);
				self.clearDropdowns();
				self.isCreateNewEnabled(false); //route/form/strength back to dropdown if any of them was in textbox (=add new) mode.
				self.isDropdownEnabled(false);
				return false;
            }
          }
      }).extend({throttle: 200});


	  self.isValidCustomOptionValue = function(optionValue){
		  var isValid = false;
		  if(optionValue && optionValue.trim().length > 0 && optionValue !== self.addNewOptionText){
			  isValid = true;
		  }
		  return isValid;
	  }

	  self.evaluateIsCreateNewMedication =  function(mode, thisMed,
													isAddingNewRouteValue,
													isAddingNewStrengthValue,
													isAddingNewFormValue,
													form,
													strength,
													route){
		    var isCustomValueCreated = null;
			if(mode === screenModes.AddNewMedValues){
				isCustomValueCreated = false;
				if(isAddingNewRouteValue === true && self.isValidCustomOptionValue(route)){
					isCustomValueCreated = true;
				}
				if(isAddingNewStrengthValue === true && self.isValidCustomOptionValue(strength)){
					isCustomValueCreated = true;
				}
				if(isAddingNewFormValue === true && self.isValidCustomOptionValue(form)){
					isCustomValueCreated = true;
				}
				if(isCustomValueCreated === true){
					//one or more of the dropdowns route/form/strength is turned to textbox -custom mode and it has a value.
					//note - some other value could be in a textbox mode but empty.
					thisMed.isCreateNewMedication(true);
				}
				else{
					thisMed.isCreateNewMedication(false);
				}
			}
			return isCustomValueCreated;
	  };

	  self.medicationParameters = ko.computed(function(){
			var thisMed = self.newPatientMedication;
			var form = self.trimmedForm();
			var strength = self.trimmedStrength();
			var route = self.trimmedRoute();
			var mode = self.screenMode;
			var isAddingNewRouteValue = self.isAddingNewRouteValue();
			var isAddingNewStrengthValue = self.isAddingNewStrengthValue();
			var isAddingNewFormValue = self.isAddingNewFormValue();

			if(thisMed && mode && self.initialized){
				var isCreated = self.evaluateIsCreateNewMedication(mode(), thisMed(),
													isAddingNewRouteValue,
													isAddingNewStrengthValue,
													isAddingNewFormValue,
													form,
													strength,
													route);
			}
	  }).extend({ throttle: 75 });

      /**
	   *	computed - in editing mode of a patient medication, we may add new strength/form/route:
       *  listen /track if form/strength/route dropdowns turn to "add new" (textbox) mode.
       *  if/when one of them does:
       *  	update the screenMode value AddNewMedValues.
       *  if any of them has a custom value entered: this means we will have to create a medicationMap record and get a familyId when saved:
       * 	set isCreateNewMedication
	   *
       * @method addingNewValue
       * @return no return value.
       */
      self.addingNewValue = ko.computed({
          read: function(){
				var mode = self.screenMode();
				var isDup = self.isDuplicate();		//is duplicate selected
				var isAddingNewRouteValue = self.isAddingNewRouteValue();	//is the dropdown field turned into textbox (route)
				var isAddingNewStrengthValue = self.isAddingNewStrengthValue();	//is the dropdown field turned into textbox (strength)
				var isAddingNewFormValue = self.isAddingNewFormValue();			//is the dropdown field turned into textbox (form)

				if(mode === screenModes.MedSelected && isDup !== true && self.initialized){
					if(isAddingNewRouteValue === true){
						self.screenMode(screenModes.AddNewMedValues);
					}
					if(isAddingNewStrengthValue === true){
						self.screenMode(screenModes.AddNewMedValues);
					}
					if(isAddingNewFormValue === true){
						self.screenMode(screenModes.AddNewMedValues);
					}
				}
          }
      }).extend({ throttle: 75 });

	  self.initialMedicationValues = ko.observable({
				form: self.trimmedForm(),
				strength: self.trimmedStrength(),
				route: self.trimmedRoute()
	  });

	  /**
	  * watch for any route/form/strength changes in edit medication scenario.
	  * and if any of them change from their original values - set recalculateNDC flag on patient medication.
	  *
	  * @function recalculateNDC
	  */

	  self.recalculateNDCWatcher = ko.computed(function(){
		  var thisStrength = self.trimmedStrength();
		  var thisForm = self.trimmedForm();
		  var thisRoute = self.trimmedRoute();
		  var canAdd = self.canAdd();
		  if (!canAdd || !self.initialized || self.newPatientMedication().isNew()) {
		      return false;
		  }
		  if(thisStrength !== self.initialMedicationValues().strength ||
			thisForm !== self.initialMedicationValues().form ||
			thisRoute !== self.initialMedicationValues().route){
				self.newPatientMedication().recalculateNDC(true);
				return true;
		  }
		  else{
			  self.newPatientMedication().recalculateNDC(false);
			  return false;
		  }
	  }).extend({ throttle: 200 });

      self.medicationBloodhound.initialize();

      self.populateDropDowns = function(){
          var thisMed = self.newPatientMedication();
          var thisName = thisMed.name();

          //set a clean search criteria:
          var thisRoute = '';
          var thisForm = '';
          var thisStrength = '';

          //if(self.screenMode() === screenModes.MedSelected){
              //criteria **remarked, do not delete**:
              //  the criteria can be used to restrict form/strength/route selections
              //  to match existing medication combinations.
              //  currently we remark it since we allow any combination to be created/ selected,
              //  and back end should handle it if it does not exist yet.
              // thisRoute = thisMed.route();
              // thisForm = thisMed.form();
              // thisStrength = thisMed.strength();
          //}
          if (thisMed && thisName) {
            datacontext.getRemoteMedicationFields(thisMed.name(), thisRoute, thisForm, thisStrength).then(fieldsReturned);
          }

          function fieldsReturned(data) {
			self.isCreateNewEnabled(true);
            self.isDropdownEnabled(true);
            self.dosageForms(data.DosageForms);
            self.strengths(data.Strengths);
            self.routes(data.Routes);
            self.units(data.Units);
			self.initialized = true;
          }
      }

	  self.initialized = true;
      if(self.newPatientMedication.peek() && !self.newPatientMedication().isNew()){
		  //edit medication scenario:
          //enable route/form/strength dropdowns
          self.isDropdownEnabled(true);
          //enable route/form/strength dropdowns to reveal the "add new" option:
          self.isCreateNewEnabled(true);
          //read dropdowns options related to the selected medication:
          self.populateDropDowns();
          //set screenMode: edit = selected
          self.screenMode(screenModes.MedSelected);
		  //clear new medication flag
		  self.newPatientMedication().isCreateNewMedication(false);
		  self.newPatientMedication().recalculateNDC(false);
		  self.lastMedName = self.newPatientMedication().name();
		  self.initialized = false;
      }


      //override the typeahead selected callback:
      self.overrideSelected = function (event, suggestion, dataset) {

        // We can access the initialized typeahead on the element like this -
        var $el = $(event.target);
        // Then, like in the binding, we can make sure the change event is triggered on select no matter what
        $el.change();
        self.newPatientMedication().isDuplicate(false);
		self.isDuplicate(false);
		self.isNewMedicationName(false);
		self.newPatientMedication().isCreateNewMedication(false);
		self.newPatientMedication().recalculateNDC(true);	//new medication needs to review its NDC.
		self.lastMedName = suggestion.Text.toUpperCase(); //keep med name so the medicationNameWatcher will ignore this change.
        // Then we can grab the suggestion -
        if( suggestion.Id && suggestion.Id == -1){
            //this is a new med
			self.isNewMedicationName(true);
            $el.typeahead('val', suggestion.Text.toUpperCase());  //workaround: since typeahead is overriding the text on blur.
			//flag to start a new medication map record
			self.newPatientMedication().isCreateNewMedication(true);

            //set the screen state:
            self.screenMode(screenModes.AddNewMed);
            //reveal the "-Add New-" option in form/strength/route dropdowns:
            self.isCreateNewEnabled(true);
            self.isDropdownEnabled(true);
            self.isAddingNewRouteValue(true);
            self.isAddingNewStrengthValue(true);
            self.isAddingNewFormValue(true);
        }
        else{
          self.screenMode(screenModes.MedSelected);
          if(suggestion.isDuplicate){
              self.newPatientMedication().isDuplicate(true);
			  self.isDuplicate(true);
          }
          else{
              self.populateDropDowns();
          }
        }
      }
    };	//activate ends here

    ctor.prototype.initializeMedSearch = function (name) {
        var self = this;
        // Check for an already created medication
        var foundMed = ko.utils.arrayFirst(self.selectedPatient().medications(), function (med) {
            return med.isNew();
        });
        if (foundMed) {
        } else {
          //build a blank patientmedication model for the current patient:
          datacontext.initializeNewPatientMedication(self.selectedPatient(), name);
        }
    }

    ctor.prototype.detached = function() {
		var self = this;

		//dispose computeds:
		//ctor.prototype.freqHowOftens.dispose();
		ctor.prototype.freqWhens.dispose();
		ctor.prototype.sources.dispose();
		ctor.prototype.types.dispose();
		ctor.prototype.categories.dispose();
		ctor.prototype.statuses.dispose();
		self.showing.dispose();
		self.newPatientMedication.dispose();
		self.trimmedMedication.dispose();
		self.trimmedForm.dispose();
		self.trimmedRoute.dispose();
		self.trimmedStrength.dispose();
		self.canAdd.dispose();
		self.medicationNameWatcher.dispose();
		self.medicationParameters.dispose();
		self.addingNewValue.dispose();
		self.recalculateNDCWatcher.dispose();
		//dispose subscriptions:
		ko.utils.arrayForEach(subscriptionTokens, function (token) {
			token.dispose();
		});
    }
    return ctor;
  });
