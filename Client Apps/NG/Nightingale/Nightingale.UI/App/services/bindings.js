/**
*	custom bindings definitions
*
*	@module bindings
*/

define(['services/formatter', 'services/dateHelper'],
    function (formatter, dateHelper) {

        var datacontext;

        // Convert any date to a MM-DD-YYYY format with moment.js
        ko.bindingHandlers.Date = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format('MM/DD/YYYY');
                $(element).text(strDate);
            }
        };

        // Convert any date to a MM-DD-YYYY format with moment.js
        ko.bindingHandlers.DateTime = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format("MM/DD/YYYY h:mm A");
                $(element).text(strDate);
            }
        };

        // Convert any date to a MM-DD-YYYY format with moment.js or string empty
        ko.bindingHandlers.DateOrNothing = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format('MM/DD/YYYY');
                strDate = strDate == 'Invalid date' ? '' : strDate;
                $(element).text(strDate);
            }
        };

        // Convert any date to a MM-DD-YYYY format with moment.js or a dash
        ko.bindingHandlers.DateOrDash = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var strDate = '-';
                if (value && value()) {
                    var date = moment(value());
                    strDate = date.format('MM/DD/YYYY');
                    strDate = strDate == 'Invalid date' ? '-' : strDate;
                }

                $(element).text(strDate);
            }
        };

        // Convert any empty value to dash
        ko.bindingHandlers.StringOrDash = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var str = '-';
                if (value && value()) {
                    str = !(value().trim()) ? '-' : value();
                }
                $(element).text(str);
            }
        };

        ko.bindingHandlers.Time = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format('HH:mm');
                $(element).text(strDate);
            }
        };

        ko.bindingHandlers.LongTime = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format('hh:mm A');
                $(element).text(strDate);
            }
        };

        ko.bindingHandlers.clickRadio = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();

                //initialize this binding with some optional options
                var setValue = allBindingsAccessor().setValue || null;

                // Register a handler to run on click
                $(element).on('click', function () {
                    // If setValue is equal to 'null',
                    if (setValue === 'null') {
                        // Clear the observable's value
                        observable(null);
                    }
                        // If there is a special value to set,
                    else if (setValue) {
                        // Set it
                        observable(setValue);
                    }
                        // Else set it equal to the context
                    else {
                        observable(bindingContext);
                    }
                });
            }
        };

        // Add an object to an array of objects
        ko.bindingHandlers.clickToggleInArray = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {

                checkDataContext();

                var observable = valueAccessor();

                //initialize this binding with some optional options
                var setValue = allBindingsAccessor().setValue || null;

                // Need to either get the current matching entity, or create a new one to use
                var thisComplexType = ko.utils.arrayFirst(observable(), function (obs) {
                    return obs.id() === setValue;
                });

                // If none was found,
                if (!thisComplexType) {
                    // Inverse the number so you have a negative id
                    //setValue = setValue * -1;
                    // Create a complex type to use when toggling
                    var theseParameters = { id: setValue };
                    // Create one to use
                    thisComplexType = datacontext.createComplexType('Identifier', theseParameters);
                }

                // Register a handler to run on click
                $(element).on('click', function () {
                    // If setValue is not already in the array,
                    var theIndex = observable().indexOf(thisComplexType);
                    if (theIndex === -1) {
                        // Create a new complex type of identifier
                        // And add it to the array
                        observable.push(thisComplexType);
                    }
                        // Else remove it from the array
                    else {
                        observable.remove(thisComplexType);
                    }
                });
            }
        };

        // This binding is very specific and only exists for use with Language and Contact Mode in the contact modal
        ko.bindingHandlers.clickToggleInLanguageArray = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                // Make sure the datacontext exists in the current module
                checkDataContext();

                // Observable array which the option should be added to / removed from
                var thisObservableArray = valueAccessor();

                // The value that will be set
                var setValue = allBindingsAccessor().setValue || null;
                var nameValue = allBindingsAccessor().nameValue || null;
                var lookupPropertyName = allBindingsAccessor().lookupPropertyName || null;

                // Need to either get the current matching entity, or create a new one to use
                var thisComplexType = ko.utils.arrayFirst(thisObservableArray(), function (obs) {
                    return obs[lookupPropertyName]() === setValue;
                });

                // If none was found,
                if (!thisComplexType) {
                    // Inverse the number so you have a negative id
                    //setValue = setValue * -1;
                    // Create a complex type to use when toggling
                    var theseParameters = {};   //id: 'id' + nameValue
                    theseParameters[lookupPropertyName] = setValue;
                    // Create one to use
                    thisComplexType = datacontext.createComplexType('ContactMode', theseParameters);
                }

                // Register a handler to run on click
                $(element).on('click', function () {
                    // If setValue is not already in the array,
                    var theIndex = thisObservableArray().indexOf(thisComplexType);
                    if (theIndex === -1) {
                        // Create a new complex type of identifier
                        // And add it to the array
                        thisObservableArray.push(thisComplexType);
                    }
                        // Else remove it from the array
                    else {
                        thisObservableArray.remove(thisComplexType);
                    }
                });
            }
        };

        ko.bindingHandlers.clickTrue = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var observable = valueAccessor();
                $(element).on('click', function () {
                    observable('true');
                });
            }
        };

        ko.bindingHandlers.clickFalse = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var observable = valueAccessor();
                $(element).on('click', function () {
                    observable('false');
                });
            }
        };

        ko.bindingHandlers.hoverToggle = {
            update: function (element, valueAccessor) {
                var css = valueAccessor();

                ko.utils.registerEventHandler(element, "mouseover", function () {
                    ko.utils.toggleDomNodeCssClass(element, ko.utils.unwrapObservable(css), true);
                });

                ko.utils.registerEventHandler(element, "mouseout", function () {
                    ko.utils.toggleDomNodeCssClass(element, ko.utils.unwrapObservable(css), false);
                });
            }
        };

        ko.bindingHandlers.clickToggle = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var value = valueAccessor();
                var canToggle = allBindingsAccessor().canToggle || null;
                ko.utils.registerEventHandler(element, "click", function (event) {
                    if (canToggle) {
                        var thisCanToggle = ko.unwrap(canToggle);
                        if (thisCanToggle) {
                            event.preventDefault();
                            value(!value());
                        }
                    }
                    else {
                        event.preventDefault();
                        value(!value());
                    }
                });
            }
        };

        ko.bindingHandlers.setActiveTab = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var value = valueAccessor();
                ko.utils.registerEventHandler(element, "click", function (event) {
                    event.preventDefault();
                    bindingContext.activeTab(value);
                });
            }
        };

        ko.bindingHandlers.updateActionElementState = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();
                // Register to any changes in the observable value so we can set the element state of the action
                var thisToken = observable.subscribe(function () {
                    if (bindingContext.step().action().elementState() !== 4) {
                        bindingContext.step().action().stateUpdatedOn(new moment().toISOString());
                        bindingContext.step().action().elementState(4);
                    }
                });
                // Handle disposal
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    thisToken.dispose();
                });
            }
        };

		ko.bindingHandlers.numericDecimal = {
			/**
			*	@param valueAccessor expecting an observable that holds/binds to the number.
			*	@method numericDecimal.init allows digits only with a decimal point
			*/
			init: function(element, valueAccessor){
				var observableNumber = valueAccessor();
				blockNonNumeric(element);
				blockExtraDecimal(element);
				$(element).on('keydown paste change', function(e){
					var key = e.which || e.keyCode;
					if( e.type == 'paste' ){
						e.preventDefault();
					}
					// key 190 is the decimal point

					var position = element.selectionStart;
					var type = e.type;
					setTimeout( function(){
					    var number = $(element).val();
					    var newNumber = number;					    
					    if (number == ".") {
					        newNumber = "0.";
					        if (observableNumber) {
					            $(element).val(newNumber);
					            observableNumber(newNumber);
					        }
					    }					    
						else if( isNaN(+number) || ( number.match(/\D/) && !number.match(/\./) ) ){	//dont remove if its the decimal point "."
							//clean out non numeric	chars:
							newNumber = number.replace(/\D/,'');
							if( observableNumber ){
								$(element).val(newNumber);
								observableNumber( newNumber );
							}
						}
						//fix cursor position as the observable binding update changes it:
						if( key === 46 || (key === 8 && position-1 < newNumber.length)){	//46=delete or 8=bkspc not on last char: return the cursor to its original position
							if( key == 8 ){
								position = position > 0? position -1 : position;
							}
							element.setSelectionRange(position, position);
						}
						else if( key >= 48 && key <= 57  && position < newNumber.length ){	//digit added in the middle
							element.setSelectionRange(position +1, position +1);
						}
					}, 5);
				});

			}
		};
		
		ko.bindingHandlers.numeric = {
			/**
			*	@param valueAccessor expecting an observable that holds/binds to the number.
			*	@method numeric.init
			*/
			init: function(element, valueAccessor){
				var observableNumber = valueAccessor();
				blockNonNumeric(element);
				$(element).on('keydown paste change', function(e){
					var key = e.which || e.keyCode;
					if( e.type == 'paste' ){
						e.preventDefault();
					}
					if( e.type == 'keydown' && key == 190 ){
						e.preventDefault();
					}
					var position = element.selectionStart;
					var type = e.type;
					setTimeout( function(){
						var number = $(element).val();
						var newNumber = number;
						if( isNaN(+number) || number.match(/\D/) ){
							//clean out non numeric	chars:
							newNumber = number.replace(/\D/,'');
							if( observableNumber ){
								$(element).val(newNumber);
								observableNumber( newNumber );
							}
						}
						//fix cursor position as the observable binding update changes it:
						if( key === 46 || (key === 8 && position-1 < newNumber.length)){	//46=delete or 8=bkspc not on last char: return the cursor to its original position
							if( key == 8 ){
								position = position > 0? position -1 : position;
							}
							element.setSelectionRange(position, position);
						}
						else if( key >= 48 && key <= 57  && position < newNumber.length ){	//digit added in the middle
							element.setSelectionRange(position +1, position +1);
						}
					}, 5);
				});

			}
		};

		/**
		*	masking and validating a social security number (SSN).
		*	@class ssn social security number
		*/
		ko.bindingHandlers.ssn = {
			/**
			*	@param valueAccessor expecting an observable that holds/binds to the ssn number.
			*	@method ssn.init
			*/
			init: function(element, valueAccessor, allBindingsAccessor, bindingContext){
				$(element).attr('maxlength', 11);
				$(element).attr('placeholder', "XXX-XX-XXXX");
				$(element).attr('title', "XXX-XX-XXXX");

				//format initial value
				var ssn = valueAccessor();
				var number = ssn();
				if(number && number.length > 0){
					number = formatter.formatSeparators(number, 'XXX-XX-XXXX', '-');
					ssn(number);
				}

				blockNonNumeric(element);

				//mask ssn number to : XXX-XX-XXXX
				$(element).on('keydown paste', function(e){
					setTimeout(function(){
						var key = e.which || e.keyCode;
						var number = $(element).val();
						var position = element.selectionStart;
						if( number && key !== 37 && key !== 39 && key !== 9){ //exclude <- , ->, Tab
							if( position === number.length && key === 8 ){
								return;	//bkspc on the last char - dont rearrange and dont add dash.
							}
							var newNumber = formatter.formatSeparators(number.replace( /\D/g, ''), 'XXX-XX-XXXX', '-');
							ssn(newNumber);
							if( key === 46 || (key === 8 && position < newNumber.length)){	//46=delete or 8=bkspc not on last char: return the cursor to its original position
								element.setSelectionRange(position, position);
							}
							else if( key >= 48 && key <= 57  && position < number.length ){	//digit added in the middle
								element.setSelectionRange(position, position);
							}
						}
					}, 5);
				});
			}
		};

		/**
		*	masking and validating phone number fields
		*	@class phone
		*
		*/
		ko.bindingHandlers.phone = {
			/**
			*	@method phone.init
			*	@param valueAccessor expecting an object (complex value) with a validate function.
			*	@example in models\contacts.js "Phone" entity instance is sent for each of the contactCard - phones tab (views\templates\contactcard.html, views\templates\phone.edit.html).
			*/
			init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
				var phone = valueAccessor();
				$(element).on('blur', function(){
					if( !phone.validate() ){
						$(element).addClass("invalid");
					}
					else{
						$(element).removeClass("invalid");
					}
				});
				$(element).attr('maxlength', 12);
				$(element).attr('placeholder', "XXX-XXX-XXXX");
				$(element).attr('title', "XXX-XXX-XXXX");

				var number = phone.number();
				if(number && number.length > 0){
					number = formatter.formatSeparators(number, 'XXX-XXX-XXXX', '-');
					phone.number(number);
				}

				//prevent typing non numerics:
				$(element).on('keypress', function(e){
					var key = e.which || e.keyCode;
					if( (key < 48 || key > 57) && key !== 116 && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46 && !(key == 118 && e.ctrlKey) ){	//exclude 116 (=F5), 8(=bkspc), 9(=tab) , 37,39 (<-, ->), 46(=del), ctrl+V (118) on firefox!
						e.preventDefault();
					}
				});

				//mask phone number to : XXX-XXX-XXXX
				$(element).on('keydown paste', function(e){
					setTimeout(function(){
						var key = e.which || e.keyCode;
						var number = $(element).val();
						var position = element.selectionStart;
						if( number && key !== 37 && key !== 39 && key !== 9){ //exclude <- , ->, Tab
							if( position === number.length && key === 8 ){
								return;	//bkspc on the last char - dont rearrange and dont add dash.
							}

							var newNumber = formatter.formatSeparators(number.replace( /\D/g, ''), 'XXX-XXX-XXXX', '-');
							phone.number(newNumber);
							if( key === 46 || (key === 8 && position < newNumber.length)){	//46=delete or 8=bkspc not on last char: return the cursor to its original position
								element.setSelectionRange(position, position);
							}
							else if( key >= 48 && key <= 57  && position < number.length ){	//digit added in the middle
								element.setSelectionRange(position, position);
							}
						}
					}, 5);
				});
			}
		};
		/**
		*	adds keyboard masking for date fields. used for editable non datepicker date field (like DOB) or datepickerEditable
		*	@method bindEditableDate
		*/
		function bindEditableDate( element, observable ){
			$(element).attr('maxlength', 10);
			$(element).attr('placeholder', "MM/DD/YYYY");
			$(element).attr('title', "MM/DD/YYYY");

			//masking: start: prevent typing non numerics:
			$(element).on('keypress', function(e){
				var key = e.which || e.keyCode;
				if( (key < 48 || key > 57) && key !== 47 && key !== 116 && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46 && !(key == 118 && e.ctrlKey) ){	//exclude 47(/), 116 (=F5), 9(=tab) , 37,39 (<-, ->)	\\&& key !== 8, 8(=bkspc)
					e.preventDefault();
				}
			});

			//mask: optimize / auto complete year YYYY
			$(element).on('blur', function(){
				var date = $(element).val();
				if(date){
					date = formatter.date.optimizeDate( date );
					date = formatter.date.optimizeYear( date );
					if( date !== $(element).val() ){
						$(element).val(date);
					}
					if( observable() !== date ){
						observable(date);
					}
				}
			});
			//mask: MM/DD/....
			$(element).on('keydown paste', function(e){
				setTimeout(function(){
					var key = e.which || e.keyCode;
					var date = $(element).val();
					//hide the datepicker if binded: while typing we may have fields movements due to validation errors going on and off.
					//	the picker is not needed when typing and its position is fixed.
					if( $(element).datepicker ){
						$(element).datepicker( "hide" ); //TODO: show/hide on arrow down/ up / enter
					}
					if( e.shiftKey || e.ctrlKey || key == 37 || key == 39 || key == 9 || key == 35 || key == 36 || key === 46 || key == 8 ){ //exclude <- , ->, Tab, home, end //|| key == 8 (bkspc)
						return;
					}
					var position = element.selectionStart;
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
							$(element).val(newDate);
							observable(newDate);
						}
					}
				}, 5);
			});
		}

		/**
		*	input date field that allows keyboard entry with masking. intended for observable string only.
		*	note this binding is to be used for non datepicker editable date (patient DOB).
		*	@class date
		*	@param valueAccessor() {String} observable
		*/
		ko.bindingHandlers.date = {
			init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
				var observable = valueAccessor();	//made for an observable string date !
				bindEditableDate( element, observable );
			}
		};
		/**
		*
		*	@class datepicker
		*/
        ko.bindingHandlers.datepicker = {
			/**
			*
			* 	@method datepicker.init
			*/
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();
                var $el = $(element);
				$(element).attr('maxlength', 10);

                //initialize datepicker with some optional options
                var options = allBindingsAccessor().datepickerOptions || {};
                // Track dynamic options if available
                var dynoptions = allBindingsAccessor().datepickerDynamicOptions || {};
                $el.datepicker(options);

				var observableMoment = moment.utc(observable());
				if( observableMoment.isValid() ){
					//set the initial date to the datepicker
					observableMoment.local(); //move from utc to local time
					var strDate = observableMoment.format('MM/DD/YYYY');
					$el.datepicker("setDate", strDate);
				}
                //handle the field changing
                ko.utils.registerEventHandler(element, "change", function () {
                    // Get the new date
                    var newValue = $el.datepicker("getDate");
                    var datepickerMoment = moment.utc(newValue);//.toISOString();
                    if (!datepickerMoment.isValid()) {
                        observable('');
                        return null;
                    }
                    if (bindingContext.step) {	//TBD program logic that should not be here
                        if (bindingContext.step().action().elementState() !== 4) {
                            bindingContext.step().action().stateUpdatedOn(new moment().toISOString());
                            bindingContext.step().action().elementState(4);
                        }
                    }
					//set the datepicker date value on to the observable date (only! without changing its time)
					var observableMoment = moment(observable());
					if( observableMoment.isValid() ){
						if( !dateHelper.isSameDate(datepickerMoment, observableMoment) ){
							observableMoment = dateHelper.setDateValue( datepickerMoment, observableMoment );
						}
					}
					else{
						observableMoment = datepickerMoment;
					}
                    observable(observableMoment.toISOString());
                });

                // If there is a datepicker options with a mindate that is an observable,
                if (dynoptions && dynoptions.minDate && ko.isObservable(dynoptions.minDate)) {
                    if (dynoptions.minDate()) {
                        // Set it on initialization
                        var initMinDate = new Date(dynoptions.minDate().getTime());
                        $el.datepicker("option", "minDate", initMinDate);
                    }
                    // Subscribe to the value
                    dynoptions.minDate.subscribe(function (newValue) {
                        var newMinDate = new Date(newValue.getTime());
                        newMinDate.setDate(newMinDate.getDate());
                        $el.datepicker("option", "minDate", newMinDate);
                        if (observable() && observable() < newMinDate) {
                            observable(newMinDate);
                        }
                    });
                }

                // If there is a datepicker dynoptions with a mindate that is an observable,
                if (dynoptions && dynoptions.maxDate && ko.isObservable(dynoptions.maxDate)) {
                    if (maxDate.maxDate()) {
                        // Set it on initialization
                        var initMaxDate = new Date(dynoptions.maxDate().getTime());
                        $el.datepicker("option", "maxDate", initMaxDate);
                    }
                    // Subscribe to the value
                    dynoptions.maxDate.subscribe(function (newValue) {
                        var newMaxDate = new Date(newValue.getTime());
                        newMaxDate.setDate(newMaxDate.getDate() - 1);
                        $el.datepicker("option", "maxDate", newMaxDate);
                        if (observable() && observable() > newMaxDate) {
                            observable(newMaxDate);
                        }
                    });
                }

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $el.datepicker("destroy");
                });
            },
			/**
			*
			* 	@method datepicker.update
			*/
            update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();	//ko.utils.unwrapObservable(valueAccessor()),
                    $el = $(element),
                    datepickerMoment = moment.utc($el.datepicker("getDate"));
				var observableMoment = moment.utc(observable());
               if ( observableMoment.isValid() && datepickerMoment.isValid() && !dateHelper.isSameDate(observableMoment, datepickerMoment) ) {
						//the observable date has changed - update the datepicker with the date parts only:
					observableMoment.local(); //move from utc to local time
					var strDate = observableMoment.format('MM/DD/YYYY');
					$el.datepicker("setDate", strDate);
					$el.blur().change();	//note: this is part of an IE fix for datepicker re-opens itself after a selection is made.
                }

                // jQuery Datepicker doesn't allow clearing dates
                // so this is a hack - set the value to clear to
                // let the binding clear the value
                // If the value was set to clear,
                if (observable === 'clear' || !observableMoment.isValid()) {
                    // Clear the control
                    var observable = valueAccessor();
                    observable(null);
                    $.datepicker._clearDate($el);
                }
                //initialized = true;
            }
        };

		/**
		*	new datepicker binding that adds editability and keyboard entry masking.
		*	works together with value binding to the same observable, the observable is expected to be a string (and not a Date).
		*	this binding will (gradually) replace the datepicker binding.
		*	@class datepickerEditable
		*/
        ko.bindingHandlers.datepickerEditable = {
			/**
			*
			* 	@method datepickerEditable.init
			*	@param 	valueAccessor {string} observable string to hold the date value.
			*/
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();
                var $el = $(element);
				bindEditableDate( element, observable );

				//initialize datepicker with some optional options
                var options = allBindingsAccessor().datepickerOptions || {};
                // Track dynamic options if available
                var dynoptions = allBindingsAccessor().datepickerDynamicOptions || {};
                $el.datepicker(options);

				if( observable()  && moment( observable() ).isValid() ){
					if( typeof(observable()) === 'string' && observable().length > 9 || typeof(observable()) !== 'string'){ //accept iso string from back end
						var observableMoment = moment.utc(observable());
						//set the initial date to the datepicker
						observableMoment.local(); //move from utc to local time
						var strDate = observableMoment.format('MM/DD/YYYY');
						$el.datepicker("setDate", strDate);
					}
				}

                // If there is a datepicker options with a mindate that is an observable,
                if (dynoptions && dynoptions.minDate && ko.isObservable(dynoptions.minDate)) {
                    if (dynoptions.minDate()) {
                        // Set it on initialization
                        var initMinDate = moment(dynoptions.minDate());
						if( initMinDate.isValid() ){
							$el.datepicker("option", "minDate", initMinDate.toDate() );
						}
                    }
                    // Subscribe to the value
                   dynoptions.minDate.subscribe(function (newValue) {
						if( newValue && dateHelper.isValidDate(newValue, true) ){
							var newMinDate = moment(newValue);
							var date = $el.val();
							if( !date || dateHelper.isValidDate(date) && !moment(date).isBefore(newValue) ){	//dont set mindate if current value is out of range, since datepicker will also update the current date to the new minDate.
								$el.datepicker("option", "minDate", newMinDate.toDate() );
							}
							// if (observable() && dateHelper.isValidDate(observable()) && moment(observable()).isBefore(newMinDate)) {
								// observable( newMinDate.format("MM/DD/YYYY") );
							// }
						}
                    });
                }

                // If there is a datepicker dynoptions with a mindate that is an observable,
               if (dynoptions && dynoptions.maxDate && ko.isObservable(dynoptions.maxDate)) {
                    if (dynoptions.maxDate()) {
                        // Set it on initialization
                        var initMaxDate = moment( dynoptions.maxDate() );
						if( initMaxDate.isValid() ){
							$el.datepicker("option", "maxDate", initMaxDate.toDate() );
						}
                    }
                 // Subscribe to the value
                    dynoptions.maxDate.subscribe(function (newValue) {
						if( newValue && dateHelper.isValidDate(newValue, true) ){
							var newMaxDate = moment(newValue);
							var date = $el.val();
							if( !date || dateHelper.isValidDate(date) && !moment(date).isAfter(newValue) ){	//dont set maxdate if current value is out of range, since datepicker will also update the current date to the new minDate.
								$el.datepicker("option", "maxDate", newMaxDate.toDate() );
							}
							// if (observable() && dateHelper.isValidDate(observable()) && moment(observable()).isAfter(newMaxDate) ) {
								// observable( newMaxDate.format("MM/DD/YYYY") );
							// }
						}
                    });
                }

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $el.datepicker("destroy");
                });
            },
			/**
			*
			* 	@method datepickerEditable.update
			*/
            update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor(), $el = $(element);
				var selectedDate = $el.datepicker("getDate");
				selectedDate = selectedDate? selectedDate.toString() : null;
                var datepickerMoment = moment.utc(selectedDate);
				if( dateHelper.isValidDate(observable(), true) ){
					var observableMoment = moment.utc(observable());
					if ( observableMoment.isValid() && datepickerMoment.isValid() && !dateHelper.isSameDate(observableMoment, datepickerMoment) ) {
							//the observable date has changed - update the datepicker with the date parts only:
						observableMoment.local(); //move from utc to local time
						var strDate = observableMoment.format('MM/DD/YYYY');
						$el.datepicker("setDate", strDate);
						$el.blur().change();	//note: this is part of an IE fix for datepicker re-opens itself after a selection is made.
					}
					if (bindingContext.step) {	//TODO: TBD program logic that should not be here
                        if (bindingContext.step().action().elementState() !== 4) {
                            bindingContext.step().action().stateUpdatedOn(new moment().toISOString());
                            bindingContext.step().action().elementState(4);
                        }
                    }
				}
				else{

				}
                // jQuery Datepicker doesn't allow clearing dates
                // so this is a hack - set the value to clear to
                // let the binding clear the value
                // If the value was set to clear,
                if (observable === 'clear') {
                    // Clear the control
                    observable(null);
                    $.datepicker._clearDate($el);
                }
            }
        };

		/**
		*			this binding wraps jquery.timepicker plugin (jQuery Timepicker - v1.3.2 - 2014-09-13 http://timepicker.co)
		*			it should be applied within: <!-- ko ifnot: Modernizr.inputtypes.time -->
		*			note: for html5 supporting input type=time use the other binding: timepicker.
		* 	@class jqtimepicker
		*/
        ko.bindingHandlers.jqtimepicker = {
			/**
			* 	@method jqtimepicker.init
			*
			*	@param valueAccessor expecting a date observable that the timepicker manipulates its time fields only.
			*	@example data-bind="timepicker: contactedOn"
			*/
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                // Get the value of the observable binding
                var observable = valueAccessor().observableDateTime;
				var showDate = valueAccessor().showDate;
				var initialized = false;
				//instantiate the timepicker
                var thisElement = $(element);
				thisElement.timepicker({
					zindex:'999', 	//the drop is invisible if not set!
					interval: 15,
					scrollbar: true,
					startTime: new Date(0,0,0,8,0,0),
					dynamic: false,
					change: onTimeChange});

				if( observable && observable() && moment( observable() ).isValid() ){
					var observableMoment = moment( observable() );
					observableMoment.local(); //move from utc to local time

					//set then initial timepicker to the observable time:
					var initHour = formatter.padZeroLeft(observableMoment.hours(), 2);
					var initMinute = formatter.padZeroLeft(observableMoment.minutes(), 2);
					thisElement.timepicker('setTime', initHour + ':' + initMinute);
				}
				else{
					initialized = true;
				}
				if( observable ){
					observable.extend({ notify: 'always' });
				}

				//mask time keyboard entry:
				$(element).attr('maxlength', 8);
				$(element).attr('placeholder', "HH:mm AM/PM");
				$(element).attr('title', "HH:mm AM/PM");

				//masking: prevent typing non numerics:
				$(element).on('keypress', function(e){
					var key = e.which || e.keyCode;
					if( (key < 48 || key > 57) && key !== 47 && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46 && !(key == 118 && e.ctrlKey) && key !== 97 && key !== 65 && key !== 109 && key !== 77 && key !== 112 && key !== 80 && key !== 58){	//exclude 47(/), 116 (=F5), 9(=tab) , 37,39 (<-, ->)	\\&& key !== 8, 8(=bkspc) and also AMamPMpm (97,65,109,77,112,80), ':' (58)
						e.preventDefault();
					}
				});

				//masking:
				$(element).on('keydown paste', function(e){
					setTimeout(function(){
						var key = e.which || e.keyCode;
						var timeStr = $(element).val();
						if( e.shiftKey || e.ctrlKey || key == 37 || key == 39 || key == 9 || key == 35 || key == 36 || key === 46 || key == 8 ){ //exclude <- , ->, Tab, home, end //|| key == 8 (bkspc)
							if( timeStr ){
								return;
							}
						}
						var position = element.selectionStart;
						if( timeStr ){
							//do the mask:
							if( e.type === 'paste'){
								timeStr = timeStr.replace( /\D/g, '');	//clean any separators before re-formatting
							}
							var newTime = timeStr;
							if( position == timeStr.length || e.type === 'paste' || timeStr.replace( /\D/g, '').length >= 8 ){
								newTime = formatter.date.optimizeTime(timeStr);
							}
							if( newTime && newTime !== timeStr || e.type === 'paste'){
								$(element).val(newTime);
								//observable(newTime);
							}
						}
						else{
							//content cleared or deleted
							if( showDate ){
								// the time field was emptied. we will set zero time (00:00/ 12:00 AM will be the time on the observable!)
								//	note: at this point - we do not invalidate when empty.
								observableMoment = moment(observable());
								if( observableMoment.isValid() && observableMoment.hour() !== 0 || observableMoment.minute() !== 0 ){
									observableMoment.hour(0);
									observableMoment.minute(0);
									observable( observableMoment.toISOString() )
								}
							}
							else{	//time control only without date, when cleared:
								observable(null);
							}
						}
					}, 5);
				});
				/**
				*			defines a subscription callback to timepicker changes, in order to sync the timepicker time
				*			onto the given data-binded observable
				*	@method onTimeChange
				*/
				function onTimeChange(time){
					var element = $(this);
					if( !initialized ){
						//ignore the first time change that is set by init:
						initialized = true;
						return;
					}
					var newHour = 0;
					var newMinutes = 0;
					if(time){
						var timepickerMoment = moment.utc( time );
						if( timepickerMoment.isValid() ){
							timeMoment = moment().hours( time.getHours() ).minutes( time.getMinutes() );	//timepicker is not DST aware !
							//timeMoment.utc();	// !!!! issue if the conversion flips a date, we need to know that
							newHour = timeMoment.hours();
							newMinutes = timeMoment.minutes();
						}
						//copy the timepicker time(only) on to the observable datetime value:
						//note: if the time field has been cleared (time = false) the observable gets a time of 00:00
						setTimeValue(observable, newHour, newMinutes);
					}
					else if( !showDate ){
						//time control only without date, when cleared:
						observable(null);
					}
				}
				function setTimeValue(dateObservable, hour, minute){
					if( !dateObservable() ){
						dateObservable(moment('1/1/1970').toISOString());
					}
					var observableLocalMoment = moment(dateObservable());
					observableLocalMoment.local();
					if(observableLocalMoment.isValid()){
						if(observableLocalMoment.hours() !== hour || observableLocalMoment.minutes() !== minute){
							observableLocalMoment.hours(hour).minutes(minute).seconds(0);
							observableLocalMoment.utc();
							dateObservable(observableLocalMoment.toISOString());
						}
					}
				}
			}
		};

		/**
		*	timepicker - bind on HTML5 input type=time and behind if Modernizr.inputtypes.time to verify browser support.
		*
		*/
        ko.bindingHandlers.timepicker = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                // Get the value of the value binding
                var value = valueAccessor();
                // The current element
                var thisElement = $(element);

                //handle the field changing
                ko.utils.registerEventHandler(element, "change", function () {
                    var observable = valueAccessor();
                    var thisMoment = new Date(moment(observable()).toString());
                    var dstOffset = (moment(thisMoment).isDST() ? 60 : 0);
					if( moment($(element)[0].valueAsDate).isValid() ){
						var adjustedTime = new Date($(element)[0].valueAsDate.getTime() + ((thisMoment.getTimezoneOffset() + dstOffset) * 60 * 1000)); //
						var adjustedDate = thisMoment;
						adjustedDate.setHours(adjustedTime.getHours(), adjustedTime.getMinutes(), adjustedTime.getSeconds());
						if (bindingContext.step && bindingContext.step() && bindingContext.step().action().elementState() !== 4) {
							bindingContext.step().action().stateUpdatedOn(new moment().toISOString());
							bindingContext.step().action().elementState(4);
						}
						observable(adjustedDate.toISOString());
					}
                });
            },
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.utils.unwrapObservable(valueAccessor());
                var thisElement = $(element);
                var thisMoment = new moment(value);
				if( thisMoment.isValid() ){
					thisElement[0].value = thisMoment.format('HH:mm');
				}
            }
        };

        // Timeout binding for hiding alert after x number of seconds
        ko.bindingHandlers.timeOut = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                // Get the other options
                var options = allBindingsAccessor().timeOutOptions || {};
                var value = valueAccessor();

                // If there are options and a delay property exists,
                if (options && options.delay) {
                    // After n number of seconds, toggle the visible value
                    setTimeout(function () { value(!value); }, options.delay);
                }

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    // $el.datepicker("destroy");
                });
            }
        };

        ko.bindingHandlers.fullCalendar = {
            // This method is called to initialize the node, and will also be called again if you change what the grid is bound to
            update: function (element, valueAccessor) {
                var viewModel = valueAccessor();
                element.innerHTML = "";

                $(element).fullCalendar({
                    defaultView: viewModel.defaultView,
					allDayText: 'All day',
                    events: ko.utils.unwrapObservable(viewModel.events),
                    header: viewModel.header,
                    editable: viewModel.editable,
                    eventClick: viewModel.eventClick
                });
                $(element).fullCalendar('gotoDate', ko.utils.unwrapObservable(viewModel.viewDate));
            }
        };

        ko.bindingHandlers.typeahead = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                // The options to use when initializing the typeahead
                var options = ko.unwrap(allBindingsAccessor().initOptions) || {};
                // the data set to use
                var dataset = ko.unwrap(valueAccessor()) || {},
                    // A local instance of the element using jQuery
                    $el = $(element),
                    // The default callback when triggering change
                    triggerChange = function () {
                        $el.change();
                    },
                    // The default callback when updating
                    triggerUpdate = function () {
                        var selectedValue = allBindingsAccessor().value();
                        // If no value is selected,
                        if (!selectedValue) {
                            // Clear the value of the type ahead
                            $el.typeahead('val', '');
                        } else {
                            // If the value changed but wasn't cleared, notify of changes
                            triggerChange();
                        }
                    }

                // What is the property name to use?
                var displayKey = dataset.displayKey;
                // Returns the value of the property listed above on the item
                dataset.displayKey = function (item) {
                    return ko.unwrap(item[displayKey]);
                };
                // Function to detect duplicates (we return false for now)
                dataset.dupDetector = function (remoteMatch, localMatch) {
                    return false;
                };
                // Use the callback override or the default callback if not
                var valueChangedEvent = options.overrideSelected ? options.overrideSelected : triggerChange;
                // The dataset's source (in this case it is a typeahead adapter from bloodhound)
                dataset.source = dataset.taOptions.ttAdapter();
                var thisTypeAhead = $el.typeahead(options, dataset)
                    .on("typeahead:selected", valueChangedEvent)
                    .on("typeahead:autocompleted", valueChangedEvent)
                    .on("typeahead:closed", triggerUpdate);

                // When the node is removed from the DOM, destroy it and clear the local references
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $el.typeahead("destroy");
                    $el = null;
                });
            },
            update: function (element, valueAccessor) {
                var value = valueAccessor();
            }
        };

        ko.bindingHandlers.autoHeight = {
            update: function (element, valueAccessor) {
                // Subscribe to changes
                var value = valueAccessor();
                $(element).height($(element)[0].scrollHeight);
            }
        };

        ko.bindingHandlers.chosen = {
            init: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                ko.bindingHandlers.options.init(element);
                var allBindings = ko.unwrap(allBindingsAccessor())
                var caption = allBindings.chosenOptions ? allBindings.chosenOptions.caption : 'NO CAPTION SET YET NOT GOOD!!!!';
                var isSearch = allBindings.chosenOptions ? allBindings.chosenOptions.isSearch : false;
                var isCancel = allBindings.chosenOptions ? allBindings.chosenOptions.isCancel : false;
                var title = null;
                if (allBindings.chosenOptions && allBindings.chosenOptions.title) {
                    title = allBindings.chosenOptions.title;  //title is not mandatory
                }

                var options = {
                    disable_search: !isSearch,
                    allow_single_deselect: isCancel,
                    search_contains: true,
                    display_selected_options: true,
                    inherit_select_classes: true,
                    placeholder_text_multiple: ' ',
                    placeholder_text_single: caption,
                    width: 'auto'
                };
                $(element).prepend('<option></option>');  //workaround for allow_single_deselect to work in ko binded.
                $(element).chosen(options);
            },
            update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                ko.bindingHandlers.options.update(element, valueAccessor, allBindingsAccessor);
                $(element).trigger('chosen:updated');

                if (viewModel.title) {
                    //chosen select wont show a title if its set directly on it. workaround: targeting the chosen-single:
                    element.parentElement.children[1].children[0].title = viewModel.title;
                }
            }
        };

        ko.bindingHandlers.chosenUpdater = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor());
                var disabled = ko.unwrap(allBindingsAccessor().disable);
                if (disabled) {
                    $(element).attr("disabled", "disabled");
                } else {
                    $(element).removeAttr("disabled");
                }
                $(element).trigger('chosen:updated');
            }
        };

        // Moves the arrow on the plan view to the right area on the page
        ko.bindingHandlers.arrowMover = {
            init: function (element, valueAccessor, allBindingsAccessor) {

                var value = ko.unwrap(valueAccessor());
                // $('.arrow').remove();
                $(element).on('click', function () {
                    positionArrow(element);
                });
            }
        };

        ko.bindingHandlers.setHeightFromPrev = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var thisValue = ko.unwrap(valueAccessor());
                var $el = $(element);
                var height = $el.parent().height() - $el.prev('.fixed').outerHeight();
                $el.css('height', (height + "px"));
            }
        };

        ko.bindingHandlers.pieChart = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var chartItems = ko.utils.unwrapObservable(valueAccessor());
                var chartOptions = allBindingsAccessor().chartOptions;

                $(element).highcharts({
                    chart: {
                        plotBackgroundColor: null,
                        plotShadow: false
                    },
                    title: {
                        text: null
                    },
                    tooltip: {
                        pointFormat: '<b>{point.percentage:.1f}%</b>'
                    },
                    plotOptions: {
                        pie: {
                            animation: false,
                            allowPointSelect: true,
                            cursor: 'pointer',
                            dataLabels: {
                                enabled: true,
                                format: '{point.name}',
                                style: {
                                    color: (Highcharts.theme && Highcharts.theme.contrastTextColor) || 'black'
                                }
                            }
                        }
                    },
                    colors: ['#d73027', '#fc8d59', '#fee090', '#e0f3f8', '#91bfdb', '#4575b4', '#ffffbf'],
                    credits: { enabled: false },
                    series: [{
                        type: 'pie',
                        name: chartOptions.title,
                        data: chartItems
                    }]
                });
            }
        };

        ko.bindingHandlers.showResultsCount = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor());
                setTimeout(function () {
                    var typeaheadOpts = $('.tt-dataset-' + ko.unwrap(allBindingsAccessor().typeahead).name + ' .tt-suggestions')[0];
                    var firstchild = $('.tt-dataset-' + ko.unwrap(allBindingsAccessor().typeahead).name + ' .tt-suggestions > .tt-suggestion')[0];
                    if (typeaheadOpts && value) {
                        var p = document.createElement("div");
                        p.innerHTML = '(' + value + ')';
                        typeaheadOpts.insertBefore(p, firstchild);
                    }
                }, 150);
            }
        };

		/**
		*	columnSizer is a binding fix for firefox and ie (< 11) for the "widget" elements in engage.
		*	this is a hack that sets the heights of the .content's and .body elements inside the widget's structure.
		*	in these browsers/versions the widgets dont show. the hack also deals with expand-collaps-fullscreen /resize behaviors.
		*	these are set as triggers in the binding that are subscribed for updates.
		* 	the binding should be applied on
		*
		*		<div class="widget"
		*
		*	@example "columnSizer: true" - when the widget does not have any expand-collaps-fullscreen.
		*	@example "columnSizer: {triggers: myToDoListOpen}" - widget have expand-collaps-fullscreen and myToDoListOpen is the observable.
		*	@example "columnSizer: {triggers: {a: isOpen, b: selectedPatient()}}" - in addition to expand-collapse we subscribe to patient selection changes.
		*	@example "columnSizer: {triggers: {a: selectedView, b: filtersHeaderOpen(), c: filtersOpen()}}" - this widget (home-todos) needs to trigger a re-set heights
		*				on these changes:
		*					1. when the filter toggles shows/hides - filtersHeaderOpen.
		*					2. the expanded filter content toggles show/hide - filtersOpen.
		*					3. a specific todo filter value changes in: selectedView
		* 	@class columnSizer
		*/
        ko.bindingHandlers.columnSizer = {
			/**
			*	@method columnSizer.init
			*/
            init: function (element, valueAccessor, allBindingsAccessor) {
                if (isIe11()) {
					return;
				}
				else{
					var $el = $(element);
                    $(document).ready(function () {
                        setTimeout(function () {
							var nobody = valueAccessor() === 'nobody' ? true : false;
                            setHeights($el, nobody);
                        }, 1)
                    })
                    $(window).on('resize', function (event) {
                        setHeights($el);
                    });
                }
            },
			/**
			*	@method columnSizer.update
			*/
			update: function (element, valueAccessor, allBindingsAccessor){
				var triggers = ko.unwrap(allBindingsAccessor().columnSizer.triggers); //track/subscribe to any widget hight altering parameter/s. the widgets may have the expand-collaps toggle observable (isOpen) and/or filters show/hide toggle and/or filter content.

				if (isIe11()){
					return;
				}
				else{
					var $el = $(element);
                    setTimeout(function () {
							var nobody = valueAccessor() === 'nobody' ? true : false;
                            setHeights($el, nobody);
					}, 1);
				}
			}
        };

		/**
		* 	for columnSizer binding fix for firefox and ie < 11
		*	@method setHeights
		*	@for columnSizer
		*/
		function setHeights($el, nobody) {
			var $contents, $widgets;

			//find all widget class elements on the current column:
			$widgets = $el.parent().find('.widget');
			var fullscreen = false;
			if($el.parent()){
				fullscreen = !!$el.parent().find('.widget.fullscreen').length;	//do we have a fullscreeen widget in this column?
			}

			//find body class elements of this column:
			$bodys = $widgets.find('.content > .table-row > .table-cell > .body');
			var fullScreenHeight;
			if( fullscreen ){
				//one of the widgets in the column is in fullscreen.
				if( !$el.hasClass('fullscreen') ){
					return;	//the current widget is not visible so dont set heights.
				}
				$widgets.find('.content').css('height', '');
				$bodys.css('height', '');
				//calc fullscreen height: take the widget's parent column height:
				fullScreenHeight = $el.closest('.column').height() + "px";
				//set the heights only on the one fullscreen widget:
				$widgets = $el.parent().find('.widget.fullscreen');
				$bodys = $widgets.find('.content > .table-row > .table-cell > .body');
			}
			else{
				//clear all heights:
				//clear the current height on all content class elements in the current column:
				//	note - it is critical to re calculate and set these heights for all content's of the entire column.
				$widgets.find('.content').css('height', '');
				$bodys.css('height', '');	//remove heights from all body 's in the widgets of this column.
			}

			var height;
			$widgets.each(function (index, widget) {
				var $content, $widget;
				$widget = $(this);
				if( !fullscreen ){
					//if none of the widgets is in fullscreeen: take the widget height:
					height = this.offsetHeight + "px";	//note using native for speed
				}
				else{
					height = fullScreenHeight;
				}
				$contents = $widget.children('.content');
				$contents.css('height', height);
				$contents.find('.wrapper > .content').css('height', height);
			});
			if( !nobody ){
				//set heights for the body elements of this column
				//	'nobody' is set to skip this for patients.list.flyout as there is another logic there for that case (setHeightFromPrev)
				$bodys.each(function (index, body) {
					var parentHeight = this.parentElement.offsetHeight ? this.parentElement.offsetHeight : 0;	//note using native for speed
					height = fullscreen? parentHeight - 10 : parentHeight;
					height += "px";
					$(body).css('height', height );
				});
			}
        }

		function isIe11() {
			return !(window.ActiveXObject) && "ActiveXObject" in window;
		}

        function positionArrow(element) {
            var el = $(element);
            var top = el.offset().top;
            top -= el.parents('.widget').offset().top;
            top += el.outerHeight() / 2;
            $('.arrow').css('top', top);
        }

		/**
		*	prevent typing non numerics
		*	@method	blockNonNumeric
		*/
		function blockNonNumeric(element){
			$(element).on('keypress', function(e){
				var key = e.which || e.keyCode;
				if ((key < 48 || key > 57) && key !== 116 && key !== 8 && key !== 9 && key !== 37 && key !== 39 && key !== 46 && !(key == 118 && e.ctrlKey)) {	//exclude 116 (=F5), 8(=bkspc), 9(=tab) , 37,39 (<-, ->), 46(=del), ctrl+V (118) on firefox!
					e.preventDefault();
				}
			});
		}

		function blockExtraDecimal(element) {
		    $(element).off('keypress.extraDecimal').on('keypress.extraDecimal', function (e) {
		        var key = e.which || e.keyCode;
		        if (key == 46 && $(element).val().indexOf(".") > -1){
		            e.preventDefault();
		        }
		    });
		}

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });
