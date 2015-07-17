/**
 * dropdown with "add new" option.
 * @module dropdown.w.addnew
 * 
 * 
 */


define([],
	function(){
        var subscriptionTokens= [];
		
		/**
		*
		*	@class dropdown.w.addnew
		*/
		var ctor = function () {
            var self = this;					
        };
		ctor.prototype.cancelChanges = function(data, event){
			var self = this;
			//clear the selected value, edit box text and search text:
			if(self.newCustomValue) self.newCustomValue('');
			self.selectedValue(null);
			if(self.searchQueryText){
				self.searchQueryText(null);
			}
			if(!self.isEditModeOnly()){
				//change back to dropdown		
				self.isCreateNew(false); 
				self.isDropDownFocused(true);				
			}			
		}
		
		ctor.prototype.checkEscape = function(data, event){
			var self = this;
			var keyCode = event.which || event.keyCode;
			if(keyCode === 27){ //escape				
				self.cancelChanges();	//back to dropdown				
			}	
		}
		
		ctor.prototype.getCustomValue = function(){
			var self = this;
			return self.newCustomValue? self.newCustomValue : self.selectedValue;
		}		
		
		/**
		 * 	@method activationData
		 *  @param selectionLabel: string - the dropdown field label text.
		 *  @param optionsLabel: css class name for the label (chsnsingle - label)
		 *	@param placeholderText: text for the field title tooltip.
		 *  @param selectedValue: observable-string/object - to bind the selected value or the new text value. 
		 *	@param optionTextProp: string - the property of the option object that holds the text to be presented.
		 *	@param optionIdValueProp: string - property name: if the selected value needs to be the value of a property of the option and not all the option object. 
		 *  @param AddNewOptionText: the add new option text will be added as an option to the dropdown. 
		 *              once selected, it will turn to a text box and remove the selection from the dom.
		 *	@param isAddNewOption: boolean. if true: the "Add New" option will be automatically added to the selectOptions.
		 *				if false: the "Add New" option will not be added. it is assumed that the option is already in the given selectOptions.
		 *				intention: use false when the options are breeze entities, and "add new" option is already there.
		 *							use true when the options are just plain string array and "add new" is not given in it.
		 *  @param isCreateNew: observable- bool - bind the state of the control so the parent will be aware of it.
		 *              note: when creating a new option the medication.edit screen state gets notified. 
		 *              this allows it to call initialize for creating a new medicationMap record and attach the id as familyId on the patientMedication.
		 *	@param isEditModeOnly: observable/ not - bool optional: true: to lock the control showing only in edit mode.
		 *							example usage: rote/form/strength show as edit box in a new medication. with isEditModeOnly()=true - the cancel/escape will not turn it into dropdown.
		 *  @param isCreateNewEnabled: observable / not observable - bool - bind to allow the parent module to control the creation of the "add new" option as necessary.
		 *              note: in medication - if no medication is selected the control should be disabled.
		 *  @param isEnabled: lets the parent module enable/disable this control.
		 *  @param selectOptions: accepting an observable array for dropdown options - each option object has {Text, Value}
		 *              note: this module will automatically add the AddNewOptionText option.
		 *	@param newCustomValue: an observable to keep/send the custom new added value to the containing parent for saving.
		 *					its optional to set this parameter - and only needed when the selectOptions are breeze entities.
		 * 
		*/
        ctor.prototype.activate = function (settings) {        	
            var self = this;
            //activationData:
            self.selectionLabel = settings.selectionLabel;
            self.optionsLabel = settings.optionsLabel;
            self.placeholderText = settings.placeholderText;
            self.selectedValue = settings.selectedValue;
			self.optionTextProp = settings.optionTextProp? settings.optionTextProp : 'Text';
			self.optionIdValueProp = settings.optionIdValueProp? settings.optionIdValueProp : null;
            self.AddNewOptionText = settings.AddNewOptionText;
			self.isAddNewOption = settings.isAddNewOption;
            self.isCreateNew = settings.isCreateNew;
            self.isCreateNewEnabled = settings.isCreateNewEnabled;
            self.isEnabled = settings.isEnabled;            
            self.selectOptions = settings.selectOptions;			
			self.newCustomValue = settings.newCustomValue? settings.newCustomValue : null;
			self.isSearch = true;
			self.isEditModeOnly = settings.isEditModeOnly? settings.isEditModeOnly : ko.observable(false);
            //
			
			self.searchQueryText = ko.observable(); //get the typed search query from chosen search input
			self.isFocused = ko.observable();			
			self.isDropDownFocused = ko.observable();
			self.getSelectedValueText = function(){				
				var text = '';
				if(self.selectedValue() && self.selectedValue().hasOwnProperty(self.optionTextProp)){
					//the selected value is an object
					text = self.selectedValue()[self.optionTextProp]();						
				}
				else{
					text = self.selectedValue();
				}
				return text;
			}
			
            var addNewOption = {};
			addNewOption[self.optionTextProp] = self.AddNewOptionText;
			addNewOption.Value = self.AddNewOptionText;
			addNewOption.id = -1;			
			
            //add "add new" option:
            self.addNewOptionToSelection = function() {
				if(self.isAddNewOption){
					if( self.selectOptions && ko.utils.arrayIndexOf(self.selectOptions(), addNewOption) === -1){
						if(self.isCreateNewEnabled()){
							self.selectOptions.push(addNewOption);
						}
					}
				}
            }

            //listener: when the dropdown options populates - add the "add new" option if it is allowed:
            var opToken = settings.selectOptions.subscribe(function(newValue){            	
            	if(self.selectOptions() ){
                    self.addNewOptionToSelection();
	            }
            });

            //if the dropdown options is already populated we may need to add the "add new" option right when activated:
            if(self.selectOptions().length > 0){
                self.addNewOptionToSelection();
            } 
            subscriptionTokens.push(opToken);
     
            //when a selection is made - if the "add new" option is enabled and it was selected - change it to textbox:
            var sbsToken = self.selectedValue.subscribe(function(newValue){            	
                if(self.isCreateNewEnabled()){
					var newValueText = '';
					if(newValue && newValue.hasOwnProperty(self.optionTextProp)){
						//the selected value is an object
						newValueText = newValue[self.optionTextProp]();						
					}
					else{
						newValueText = newValue;
					}
                	if(newValueText === self.AddNewOptionText){                		
                        if(self.isCreateNew){
                            self.isCreateNew(true);  //change to text box 
							//set the search query as the new custom value:
							var query = self.searchQueryText? self.searchQueryText() : '';	
                            //delay the change since the selectedValue subscription may be still listening to its change. 
                            setTimeout(function(){									
								if(self.selectedValue && !self.selectedValue.hasOwnProperty(self.optionTextProp)){
									//selectedValue, selectOptions are strings and not breeze objects.
									//in this case the selectedValue is used also as the custom value holder:
									self.selectedValue(query);
								}
								else{
									//clear the selection (its "-Add New"- id = -1 for a short while)
									self.selectedValue(null);
								}
							}, 5); 
							if(self.newCustomValue) self.newCustomValue(query);
							
							self.isFocused(true);
                        }                                                       	
                	}
                }	
            });
            subscriptionTokens.push(sbsToken);						
        }

        ctor.prototype.detached = function() {
            
            ko.utils.arrayForEach(subscriptionTokens, function (token) {
                token.dispose();
            });
        }
		return ctor;
	});