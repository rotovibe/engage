/**
*	editable date and time controls with masking and validation
*	note the subscriptionTokens are managed per instance of a datetimepicker.
*	if this is not done the subscriptions of other live instances are cleared when the first one is detached.
*	@module datetimePicker
*/
define(['durandal/composition','services/dateHelper', 'services/formatter'], 
	function(composition, dateHelper, formatter){
			
	var self = this;	
	
	var ctor = function () {		
    };
	
	ctor.prototype.compositionComplete = function(view, parent){
		var self = this;
		self.dateElm = $(view).find("[name='" + self.dateName +"']");
		bindEditableDate( self.dateElm, self.dateStr );
		if( self.isDatepicker ){			
			self.dateElm.datepicker(self.datepickerOptions);
			if( self.observableDateTime()  && moment( self.observableDateTime() ).isValid() ){ 
				var observableMoment = moment.utc(self.observableDateTime());
				//set the initial date to the datepicker
				observableMoment.local(); //move from utc to local time
				var strDate = observableMoment.format('MM/DD/YYYY');					
				self.dateElm.datepicker("setDate", strDate);
			}
			//handle disposal (if KO removes by the template binding)
			ko.utils.domNodeDisposal.addDisposeCallback(self.dateElm, function () {
				self.dateElm.datepicker("destroy");								
			});
			
			// If there is a datepicker options with a mindate that is an observable,
			if (self.dynoptions && self.dynoptions.minDate && ko.isObservable(self.dynoptions.minDate)) {
				if (self.dynoptions.minDate()) {
					// Set it on initialization
					var initMinDate = moment(self.dynoptions.minDate());
					if( initMinDate.isValid() ){
						self.dateElm.datepicker("option", "minDate", initMinDate.toDate() );
					}                        
				}
				// Subscribe to the value
				var minDateToken = self.dynoptions.minDate.subscribe(function (newValue) {
				   var newMinDate = moment(newValue);
					if( newMinDate.isValid() ){
						var date = self.observableDateTime();								
						if( !date || moment(date).isValid() && !moment(date).isBefore(newMinDate) ){	//dont set mindate if current value is out of range, since datepicker will also update the current date to the new minDate. 
							self.dateElm.datepicker("option", "minDate", newMinDate.toDate() );						
						}
					}
				});
				self.subscriptionTokens.push(minDateToken);
			}
			 // If there is a datepicker dynoptions with a mindate that is an observable,
			if (self.dynoptions && self.dynoptions.maxDate && ko.isObservable(self.dynoptions.maxDate)) {
				if (self.dynoptions.maxDate()) {
					// Set it on initialization
					var initMaxDate = moment( self.dynoptions.maxDate() );
					if( initMaxDate.isValid() ){
						self.dateElm.datepicker("option", "maxDate", initMaxDate.toDate() );	
					}                        
				}			 
				var maxDateToken = self.dynoptions.maxDate.subscribe(function (newValue) {						
					var newMaxDate = moment(newValue);
					if( newMaxDate.isValid() ){																							
						var date = self.observableDateTime();								
						if( !date || moment(date).isValid() && !moment(date).isAfter(newMaxDate) ){	//dont set maxdate if current value is out of range, since datepicker will also update the current date to the new minDate. 
							self.dateElm.datepicker("option", "maxDate", newMaxDate.toDate() );					
						}
					}
				});
				self.subscriptionTokens.push(maxDateToken);
			}
		}		
		self.timeElm = $(view).find("[name='"+ self.timeName +"']");
		// if( self.showTime ){
			// self.timeElm[0].value = thisMoment.format('HH:mm');
		// }
	};
	
	ctor.prototype.activate = function (settings) {
        var self = this;
        self.settings = settings;
		self.subscriptionTokens = [];	//correct management of tokens per instance of a datetimepicker 
		//date
		self.dateName = self.settings.dateName ? self.settings.dateName : 'date';		
		self.showDate = self.settings.showDate !== undefined ? self.settings.showDate : true;
		self.dateCss = self.settings.dateCss ? self.settings.dateCss : "";
		self.observableDateTime = self.settings.observableDateTime;
		self.emptyDateIsValid = true;
		self.dateErrors = self.settings.dateErrors;
		self.showInvalid = self.settings.showInvalid ? self.settings.showInvalid : false;
		self.showInvalidTime = self.settings.showInvalidTime ? self.settings.showInvalidTime : false;
		self.isDatepicker = self.settings.isDatepicker !== false ? true : false;
		self.datepickerOptions = self.settings.datepickerOptions || {};
		self.dynoptions = self.settings.datepickerDynamicOptions || {};
		self.minDate = self.settings.minDate ? self.settings.minDate : null;
		self.maxDate = self.settings.maxDate ? self.settings.maxDate : null;
		
		//time
		self.timeName = self.settings.timeName ? self.settings.timeName : 'time';
		self.showTime = self.settings.showTime ? self.settings.showTime : false;
		self.timeCss = self.settings.timeCss ? self.settings.timeCss : "";

		//
		self.dateStr = ko.observable();
		self.timeStr = ko.observable();
		if( self.showDate ){
			var observableMoment = moment( self.observableDateTime() );
			if( observableMoment.isValid() ){
				self.dateStr( observableMoment.format('MM/DD/YYYY') );
			}
		}
		self.isDisableTime = ko.computed( function(){
			var result = false;			
			if( self.showTime && self.showDate ){
				var observableDateTime = self.observableDateTime();
				result = !observableDateTime;
			}
			else if( self.ShowTime ){
				result = false;
			}
			return result;
		});
		
		if( self.showTime && Modernizr.inputtypes.time ){
			var observableMoment = moment( self.observableDateTime() );
			if( observableMoment.isValid() ){
				self.timeStr( observableMoment.format('HH:mm') );
			}
		}
		self.isValidDate = ko.computed( function(){
			var enteredDateStr = self.dateStr();			
			var dateError = dateHelper.isInvalidDate(enteredDateStr, {minDate: self.minDate, maxDate: self.maxDate},!self.emptyDateIsValid);
			var errorsList = [];
			if( dateError != null ){				
				errorsList.push(dateError);					
			}
			self.dateErrors( errorsList );			
			return ( errorsList.length === 0 ); 			
		});
		self.disableTime = ko.computed( function(){
			return !self.observableDateTime();
		});

		self.datetimeWatcher = ko.computed( function(){
			var enteredDateStr = self.dateStr();
			var enteredTimeStr = self.timeStr();
			if( !self.showDate ){
				//a time control without related date control got some time value
				if( enteredTimeStr ){
					if( !enteredDateStr ){	
						//seed the datetime value
						self.dateStr('1/1/1970');
						enteredDateStr = self.dateStr();						
					}
				}
				else{
					if( Modernizr.inputtypes.time ){						
						//clear the datetime value
						self.dateStr(null);
						enteredDateStr = null;
						self.observableDateTime(null);
					}
				}
			}
			if( dateHelper.isValidDate(enteredDateStr) ){
				var observableMoment;
				var enteredMoment = moment(enteredDateStr);	
				var needsUpdate = false;
				if( self.observableDateTime() ){
					observableMoment = moment(self.observableDateTime());											
					if( !dateHelper.isSameDate(observableMoment, enteredMoment) ){					
						observableMoment = dateHelper.setDateValue( enteredMoment, observableMoment );
						needsUpdate = true;						
					}
					if( self.showTime && Modernizr.inputtypes.time ){
						if( enteredTimeStr ){
							var time = parseTime( enteredTimeStr )
							if( time && ( observableMoment.hour() !== time.hour || observableMoment.minute() !== time.minute ) ){
								//time has changed - update observable
								observableMoment.hour( time.hour );
								observableMoment.minute( time.minute );
								needsUpdate = true;							
							}
						}
						else{
							if( self.showDate ){
								self.timeStr( observableMoment.format('HH:mm') );
							}
							else{
								//time only control, the time was cleared.
								self.observableDateTime(null);
								self.timeStr(null);
							}
						}
					}
					if( needsUpdate ){
						self.observableDateTime( observableMoment.toISOString() );
					}
				}
				else{
					if( self.showTime && Modernizr.inputtypes.time && enteredTimeStr ){
						var time = parseTime( enteredTimeStr )
						if( time && ( enteredMoment.hour() !== time.hour || enteredMoment.minute() !== time.minute ) ){
							//time has changed - update observable
							enteredMoment.hour( time.hour );
							enteredMoment.minute( time.minute );
						}
					}
					self.observableDateTime( enteredMoment.toISOString() );
				}						
			}							
			else{
				if( self.observableDateTime() && !enteredDateStr && self.showDate ){
					self.observableDateTime(null); 	//the date field was cleared
					self.dateElm.datepicker("setDate", null);
				}
			}	
		});
				
	};
	
	function parseTime( timeStr ){
		var result = null;
		var parsed = false;
		if( timeStr ){
			var parts = timeStr.split(':');
			if( parts && parts.length == 2 ){
				var hour = 0, minute = 0;
				if( !isNaN(parts[0]) ){
					hour = Number( parts[0] );
				}
				if( !isNaN(parts[1]) ){
					minute = Number( parts[1] );
					parsed = true;
				}
				else if( parts[1].length > 0 && parts[1].indexOf(' ') > 0 && ( parts[1].indexOf("AM") !== -1 || parts[1].indexOf("PM") !== -1 ) ){
					var tails = parts[1].split(' ');
					if( tails && tails.length > 1 ){
						if( !isNaN( tails[0] ) ){
							minute = Number( tails[0] );
							parsed = true;
						}
						if( tails[1] && tails[1].indexOf("PM") !== -1 ){
							if( hour < 12 ){
								hour = hour + 12;
							}
						}
					}
				}
			}
		}
		if( parsed ){
			result = {hour: hour, minute: minute};
		}
		return result;
	}
	
	function bindEditableDate( element, observable ){
		element.attr('maxlength', 10);
		element.attr('placeholder', "MM/DD/YYYY");
		element.attr('title', "MM/DD/YYYY");
		
		//masking: start: prevent typing non numerics:
		element.on('keypress', function(e){
			var key = e.which || e.keyCode;
			if( (key < 48 || key > 57) && key !== 47 && key !== 116 && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46 && !(key == 118 && e.ctrlKey) ){	//exclude 47(/), 116 (=F5), 9(=tab) , 37,39 (<-, ->)	\\&& key !== 8, 8(=bkspc)
				e.preventDefault();												
			}
		});
		
		//mask: optimize / auto complete year YYYY
		element.on('blur', function(){
			var date = element.val();
			if(date){					
				date = formatter.date.optimizeDate( date );
				date = formatter.date.optimizeYear( date );
				//console.log('date is blurred ! and optimized to:' + date);
				if( date !== element.val() ){
					element.val(date);
				}	
				if( observable() !== date ){
					observable(date);
				}					
			}
			else{
				if( observable() ){
					observable(null);
				}
			}
		});	
		//mask: MM/DD/....
		element.on('keydown paste', function(e){
			setTimeout(function(){						
				var key = e.which || e.keyCode;						
				var date = element.val();
				//hide the datepicker if binded: while typing we may have fields movements due to validation errors going on and off.
				//	the picker is not needed when typing and its position is fixed.
				if( element.datepicker ){
					element.datepicker( "hide" ); //TODO: show/hide on arrow down/ up / enter
				}						
				if( e.shiftKey || e.ctrlKey || key == 37 || key == 39 || key == 9 || key == 35 || key == 36 || key === 46 || key == 8 ){ //exclude <- , ->, Tab, home, end //|| key == 8 (bkspc)
					return;
				}						
				var position = element[0].selectionStart;
				if( date ){ 
					//do the mask:
					if( e.type === 'paste'){
						date = date.replace( /\D/g, '');	//clean any separators before re-formatting
					}
					var newDate = date;
					if( position == date.length || e.type === 'paste' || date.replace( /\D/g, '').length >= 8 ){
						newDate = formatter.date.optimizeDate(date);
					}						
					if( newDate && newDate !== date || e.type === 'paste'){
						element.val(newDate);						
						observable(newDate);								
					}
				}						
			}, 5);
		});			
	}
	ctor.prototype.detached = function() { 
		var self = this;
		ko.utils.arrayForEach(self.subscriptionTokens, function (token) {
			token.dispose();
		});
		self.subscriptionTokens = [];	//tokens collection per instance of a datetimepicker 
		//computed cleanup:
		self.datetimeWatcher.dispose();
		self.disableTime.dispose();
		self.isValidDate.dispose();			
	}
    return ctor;
});