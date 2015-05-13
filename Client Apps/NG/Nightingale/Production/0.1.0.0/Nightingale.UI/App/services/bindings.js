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

        ko.bindingHandlers.Time = {
            update: function (element, valueAccessor) {
                var value = valueAccessor();
                var date = moment(value());
                var strDate = date.format('HH:mm');
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

        // This binding is very specific and only exists for use with Language and Contact Mode in the contact mode
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
                    theseParameters[lookupPropertyName]= setValue;
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
                    bindingContext.step().action().elementState(4);
                });
                // Handle disposal
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    thisToken.dispose();
                });
            }
        };

        ko.bindingHandlers.datepicker = {
            init: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var observable = valueAccessor();
                var $el = $(element);

                //initialize datepicker with some optional options
                var options = allBindingsAccessor().datepickerOptions || {};
                $el.datepicker(options);
                
                //handle the field changing
                ko.utils.registerEventHandler(element, "change", function () {
                    // Get the new date
                    var newValue = $el.datepicker("getDate");
                    var thisMoment = moment(newValue).toISOString();
                    if (thisMoment === 'Invalid date') {
                        observable('clear');
                        return null;
                    }
                    if (bindingContext.step) {
                        bindingContext.step().action().elementState(4);
                    }
                    observable(thisMoment);
                });

                //handle disposal (if KO removes by the template binding)
                ko.utils.domNodeDisposal.addDisposeCallback(element, function () {
                    $el.datepicker("destroy");
                });

            },
            update: function (element, valueAccessor, allBindingsAccessor, bindingContext) {
                var value = ko.utils.unwrapObservable(valueAccessor()),
                    $el = $(element),
                    current = $el.datepicker("getDate");
                if (value - current !== 0) {
                    var thisMomento = moment(value);
                    var strDate = thisMomento.format('MM/DD/YYYY');
                    $el.datepicker("setDate", strDate);
                    $el.blur();
                }
                // jQuery Datepicker doesn't allow clearing dates
                // so this is a hack - set the value to clear to 
                // let the binding clear the value
                // If the value was set to clear,
                if (value === 'clear' || value === 'Invalid date') {
                    // Clear the control
                    $.datepicker._clearDate($el);
                }
                //initialized = true;
            }
        };

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
                    var dstOffset = (moment(thisMoment).isDST() ? 60: 0);
                    var adjustedTime = new Date($(element)[0].valueAsDate.getTime() + ((thisMoment.getTimezoneOffset() + dstOffset) * 60 * 1000)); //
                    var adjustedDate = thisMoment;
                    adjustedDate.setHours(adjustedTime.getHours(), adjustedTime.getMinutes(), adjustedTime.getSeconds());
                    bindingContext.step().action().elementState(4);
                    observable(adjustedDate.toISOString());
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
                    editable: viewModel.editable
                });
                $(element).fullCalendar('gotoDate', ko.utils.unwrapObservable(viewModel.viewDate));
            }
        };

        ko.bindingHandlers.typeahead = {
            init: function (element, valueAccessor, allBindingsAccessor) {
                var options = ko.unwrap(valueAccessor()) || {},
                    $el = $(element),
                    triggerChange = function () {
                        $el.change();
                    }

                var displayKey = options.displayKey;
                options.displayKey = function (item) {
                    return item[displayKey]();
                };
                options.dupDetector = function (remoteMatch, localMatch) {
                    return false;
                };
                options.source = options.taOptions.ttAdapter();
                var thisTypeAhead = $el.typeahead(null, options)
                    .on("typeahead:selected", triggerChange)
                    .on("typeahead:autocompleted", triggerChange);
                
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
                var options = {
                    disable_search: true,
                    allow_single_deselect: true,
                    display_selected_options: true,
                    inherit_select_classes: true,
                    placeholder_text_multiple: ' ',
                    placeholder_text_single: 'Choose One',
                    width: 'auto'
                };
                $(element).chosen(options);
            },
            update: function (element, valueAccessor, allBindingsAccessor, viewModel) {
                ko.bindingHandlers.options.update(element, valueAccessor, allBindingsAccessor);
                $(element).trigger('chosen:updated');
            }
        };

        ko.bindingHandlers.chosenUpdater = {
            update: function (element, valueAccessor, allBindingsAccessor) {
                var value = ko.unwrap(valueAccessor());

                var disabled = allBindingsAccessor().disable;
                if (disabled) {
                    $(element).attr("disabled", "disabled");
                } else {
                    $(element).removeAttr("disabled");
                }
                $(element).trigger('chosen:updated');                
            }
        }

        function checkDataContext() {
            if (!datacontext) {
                datacontext = require('services/datacontext');
            }
        }
    });