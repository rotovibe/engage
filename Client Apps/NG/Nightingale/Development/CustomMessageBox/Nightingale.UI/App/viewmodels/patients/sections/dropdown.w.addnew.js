/**
 * dropdown with "add new" option.
 * activationdata:
 *  selectionLabel: string - the dropdown field label text.
 *  selectedValue: observable-string - to bind the selected value or the new text value. 
 *  AddNewOptionText: the add new option text will be added as an option to the dropdown. 
 *              once selected, it will turn to a text box and remove the selection from the dom.
 *  isCreateNew: observable- bool - bind the state of the control so the parent will be aware of it.
 *              note: when creating a new option the medication.edit screen state gets notified. 
 *              this allows it to call initialize for creating a new medicationMap record and attach the id as familyId on the patientMedication.
 *  isCreateNewEnabled: observable- bool - bind to allow the parent module to disable the control as necessary.
 *              note: in medication - if no medication is selected the control should be disabled.
 *  isEnabled: lets the parent module enable/disable this control.
 *  selectOptions: accepting an observable array for dropdown options - each option object has {Text, Value}
 *              note: this module will automatically add the AddNewOptionText option.
 *  optionsLabel: css class name for the label (chsnsingle - label)
 *
 * @module dropdown.w.addnew
 */


define([],
	function(){
        var subscriptionTokens= [];

		var ctor = function () {
            var self = this;
        };

        ctor.prototype.activate = function (settings) {        	
            var self = this;
            //activationData:
            self.selectionLabel = settings.selectionLabel;
            self.optionsLabel = settings.optionsLabel;
            self.placeholderText = settings.placeholderText;
            self.selectedValue = settings.selectedValue;
            self.AddNewOptionText = settings.AddNewOptionText;
            self.isCreateNew = settings.isCreateNew;
            self.isCreateNewEnabled = settings.isCreateNewEnabled;
            self.isEnabled = settings.isEnabled;            
            self.selectOptions = settings.selectOptions;
            //

            var addNewOption = {Text: self.AddNewOptionText, Value: self.AddNewOptionText};

            //add "add new" option:
            self.addNewOptionToSelection = function() {
                if( self.selectOptions && self.selectOptions.indexOf(addNewOption) === -1){
                    if(self.isCreateNewEnabled()){
                        self.selectOptions.push(addNewOption);
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
            	console.log(self.selectionLabel + ' changed to: ' + newValue? newValue : '');
                if(self.isCreateNewEnabled()){
                	if(newValue === self.AddNewOptionText){                		
                        if(self.isCreateNew){
                            self.isCreateNew(true);  //change to text box   
                            //delay the change since the selectedValue subscription may be still listenning to its change. 
                            setTimeout(function(){ self.selectedValue(''); }, 5);     
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