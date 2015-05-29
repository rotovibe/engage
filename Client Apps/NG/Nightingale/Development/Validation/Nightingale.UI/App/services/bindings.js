﻿/**
*	custom bindings definitions
*
*	@module bindings
*/

define([],
    function () {

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
					// console.log('datepicker init setDate to :' + strDate);
				}
                //handle the field changing
                ko.utils.registerEventHandler(element, "change", function () {					
                    // Get the new date
                    var newValue = $el.datepicker("getDate");
                    var datepickerMoment = moment.utc(newValue);//.toISOString();
                    if (!datepickerMoment.isValid()) {
						// console.log('datepicker registerEventHandler cleared the observable');
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
						if( !isSameDate(datepickerMoment, observableMoment) ){
							// console.log('datepicker registerEventHandler calling setDateValue src = ' +datepickerMoment.toISOString() + ' dest='+ observableMoment.toISOString());
							observableMoment = setDateValue( datepickerMoment, observableMoment );
						}
					}
					else{
						observableMoment = datepickerMoment;
					}	
					// console.log('datepicker registerEventHandler set observable= ' + observableMoment.toISOString());	
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
               if ( observableMoment.isValid() && datepickerMoment.isValid() && !isSameDate(observableMoment, datepickerMoment) ) {					 					
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

		function isSameDate(moment1, moment2){
			//compare date parts only:			
			return moment(moment1.format('MM/DD/YYYY')).isSame(moment2.format('MM/DD/YYYY'))
		}
		
		function setDateValue( momentSrc, momentDest ){
			// console.log('datepicker setDateValue starts: src='+ momentSrc.toISOString() + ' dest=' +momentDest.toISOString());
			momentDest.date( momentSrc.date() );
			momentDest.month( momentSrc.month() );
			momentDest.year( momentSrc.year() );
			// console.log('datepicker setDateValue returns: ' + momentDest.toISOString());
			return momentDest;
		}
		
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
                var observable = valueAccessor();
				var initialized = false;
				//instantiate the timepicker
                var thisElement = $(element);
				thisElement.timepicker({ 
					zindex:'999', 	//the drop is invisible if not set!
					interval: 15, 					
					scrollbar: true,					
					startTime: new Date(0,0,0,8,0,0),	
					change: onTimeChange});
				
				// console.log('jqtimepicker init');				
				if( observable && observable() && moment( observable() ).isValid() ){
					var observableMoment = moment( observable() );
					observableMoment.local(); //move from utc to local time
					
					//set then initial timepicker to the observable time:					
					var initHour = padZeroLeft(observableMoment.hours(), 2);
					var initMinute = padZeroLeft(observableMoment.minutes(), 2);
					// console.log('jqtimepicker init setting the timepicker to the observable: ' + initHour + ':' + initMinute);
					thisElement.timepicker().setTime(initHour + ':' + initMinute);
				}
				else{
					initialized = true;
				}
				if( observable ){
					observable.extend({ notify: 'always' });
				}				
				/**
				*			defines a subscription callback to timepicker changes, in order to sync the timepicker time 
				*			onto the given data-binded observable 
				*	@method onTimeChange 
				*/
				function onTimeChange(time){
					var element = $(this);
					var observable = valueAccessor();
					if( !initialized ){
						//ignore the first time change that is set by init:
						// console.log('jqtimepicker onTimeChange starts: initialized = false => ignoring change');
						initialized = true;
						return;
					}
					// console.log('jqtimepicker onTimeChange starts: time=' + time + ' observable =' + observable());
					var newHour = 0;
					var newMinutes = 0;					
					if(time){										
						var timepickerMoment = moment.utc( time );
						if( !timepickerMoment.isValid() ){
							//$(this).addClass("invalid");	//TBD
							//todo: get an observable to send the invalid state out to the parent							
						}
						else{
							//$(this).removeClass("invalid"); //TBD	
							timeMoment = moment().hours( time.getHours() ).minutes( time.getMinutes() );	//timepicker is not DST aware !
							//timeMoment.utc();	// !!!! issue if the conversion flips a date, we need to know that 
							newHour = timeMoment.hours(); //time.getHours();
							newMinutes = timeMoment.minutes();//time.getMinutes();														
						}
					}
						
					//copy the timepicker time(only) on to the observable datetime value:
					//note: if the time field has been cleared (time = false) the observable gets a time of 00:00
					setTimeValue(observable, newHour, newMinutes);					
					// console.log('jqtimepicker onTimeChange ends: time=' + time + ' observable =' + observable());
				}
				function setTimeValue(dateObservable, hour, minute){					
					var observableLocalMoment = moment(dateObservable());
					observableLocalMoment.local();
					// console.log('jqtimepicker setTimeValue starts with dateObservable in local time =' + observableLocalMoment.toISOString());
					if(observableLocalMoment.isValid()){
						if(observableLocalMoment.hours() !== hour || observableLocalMoment.minutes() !== minute){
							observableLocalMoment.hours(hour).minutes(minute).seconds(0);
							// console.log('jqtimepicker setTimeValue dateObservable in local time =' + observableLocalMoment.toISOString());
							observableLocalMoment.utc();
							dateObservable(observableLocalMoment.toISOString());
							// console.log('jqtimepicker setTimeValue set observable=' + observableLocalMoment.toISOString());
						}						
					}
				}
			}
		};
		
		function padZeroLeft(num, size){
			var s = num+"";
			while (s.length < size) s = "0" + s;
			return s;
		}
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
                var thisMoment = new moment(value).format('HH:mm');
                thisElement[0].value = thisMoment;
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
                if (isChrome() || isIe11()) {
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
				
				//console.log('columnSizer update starts');
				if (isChrome() || isIe11()){
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

        function isChrome() {
            var isChromium = window.chrome,
                vendorName = window.navigator.vendor;
            if (isChromium !== null && isChromium !== undefined && vendorName === "Google Inc.") {
                return true;
            } else {
                return false;
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

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });