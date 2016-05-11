/*
*	a widget control for multiselect.
*
*	@module	multiselect
*/

define(['durandal/composition'], function (composition) {

    var datacontext;

    var ctor = function () {
    };

    ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
        self.canClose = ko.observable(false);
        self.showing = ko.observable(false);
		self.isInFocus = ko.observable(false);	//used for closing the chosen-drop options box when blur. the focus is bound to the input box.
        self.options = self.settings.options;
        self.selectedValues = self.settings.values;
        self.isRequired = self.settings.isRequired;
        self.text = self.settings.text;
        self.label = self.settings.label;
        self.idValue = self.settings.idValue;
        self.stringValue = self.settings.stringValue;
        self.computedOptions = ko.computed(function () {
            var thisList = self.options();
            ko.utils.arrayForEach(thisList, function (item) {
                item.thisText = ko.computed(item[self.text]);
            });
            return thisList;
        }).extend({throttle: 50});
        self.checkForIdValue = function (idvalue) {
            var thisMatch = ko.utils.arrayFirst(self.selectedValues(), function (selectedValue) {
                return selectedValue.id() === idvalue;
            });
            if (thisMatch) {
                return true;
            }
            return false;
        };
        self.computedLabel = ko.computed(function () {
            if (self.selectedValues()) {
                return self.selectedValues().length + ' selected.';
            }
            return 'Choose...';
        }).extend({throttle: 50});
        self.selectOption = function (sender) {
			self.stopClosing();	//prevent the chosen-drop from closing		
            checkDataContext();
            // Use array first here to see if it is a duplicate
            var foundComplexType = ko.utils.arrayFirst(self.selectedValues(), function (selectedValue) {
                return selectedValue.id() === sender.id();
            });
            if (foundComplexType) {
                var thisIndex = self.selectedValues().indexOf(foundComplexType);
                if (thisIndex > -1) {
                    self.selectedValues.splice(thisIndex, 1);
                } else {
                    self.selectedValues.remove(foundComplexType);
                }
            }
            else {
                var theseParameters = { id: sender.id() };
                // Create one to use from the sender
                thisComplexType = datacontext.createComplexType('Identifier', theseParameters);
                self.selectedValues.push(thisComplexType);
            }
			//since the selection of an option is also a blur (lost focus) on the input box of the control,
			//we need to keep the focus on the control so the chosen-drop will stay open:
			self.isInFocus(true);						
        };
		/*
		*	computed to track the blur (lost focus) of the multiselect in order to close the chosen-drop if it is visible (showing).
		*	@method inFocusTracker
		*/
		self.inFocusTracker = ko.computed( function(){			
			var isInFocus = self.isInFocus();
			var showing = self.showing();
			if( !isInFocus && showing ){								
				self.startClosing();
			}			
		}).extend({throttle: 50});

        self.isDisabled = ko.computed(function () {
            var thisState = false;
            if (self.settings.disabled) {
                thisState = self.settings.disabled;
            }
            return thisState;
        }).extend({throttle: 50});
        self.isInvalid = ko.computed(function () {
            return self.isRequired && self.selectedValues().length === 0;
        }).extend({throttle: 50});
    };

    ctor.prototype.toggleDropdown = function () {
        var self = this;
        if (self.computedOptions().length !== 0 && !self.isDisabled()) {
            self.showing(!self.showing());
        }
    };

	/*
	*	close the chosen-drop after a blur. the closing is delayed by a timeout since when clicking to select an option, 
	*	we have a temporary short lost focus event, and we do not want it to close. this is corrected by the selectOption 
	*	function as it calls stopClosing to reset canClose(false) and sets the focus back isInFocus(true) during this delay.
	*	the isInFocus is bound to the input element in the control, as other elements will not track it very well.
	*	@method startClosing
	*/
    ctor.prototype.startClosing = function () {
        var self = this;
        self.canClose(true);
		setTimeout( function(){
			if( self.canClose() ){
				self.showing(false);
			}	
		}, 200);        
    };

    ctor.prototype.stopClosing = function () {
        var self = this;
        self.canClose(false);	
    };

    ctor.prototype.attached = function () {
    };

	ctor.prototype.detached = function() {
		var self = this;
		self.isInvalid.dispose();
		self.isDisabled.dispose();
		self.inFocusTracker.dispose();
		self.computedLabel.dispose();	
		ko.utils.arrayForEach(self.computedOptions, function (item) {
			item.thisText.dispose();
		});
		self.computedOptions.dispose();
		// ko.utils.arrayForEach(subscriptionTokens, function (token) {
			// token.dispose();
		// });
	}
    return ctor;
    
    function checkDataContext() {
        if (!datacontext) {
            datacontext = require('services/datacontext');
        }
    }
});