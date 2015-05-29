///////// Start AJAX POST implementation //////////////////////////
/* Functions for allowing Breeze to use POST for queries when
   special parameters are passed using the .withParameters function.
Special parameters:
•	$method: ‘POST’ or ‘GET’ (the default)
•	$encoding: ‘JSON’ or x-www-form-urlencoded (the default)
•	$data: contains the data to be sent to the server

Example:
   var query = breeze.EntityQuery.from('SimilarCustomersPOST')
            .withParameters({ 
                $method: 'POST',
                $encoding: 'JSON',
                $data: { CompanyName: 'Hilo Hattie', ContactName: 'Donald', City: 'Duck', Country: 'USA', Phone: '808-234-5678' } 
            });


*/
breeze.ajaxpost = (function (ajaxAdapter) {

    divertAjaxImpl(ajaxAdapter);

    // CHANGE: 
    //      Added different return type for configAjaxAdapter here

    return {
        configAjaxAdapter: divertAjaxImpl
    };

    // Add processSettings to ajaxAdapter
    function divertAjaxImpl(ajaxAdapter) {
        if (!ajaxAdapter) {
            ajaxAdapter = breeze.config.getAdapterInstance("ajax");
        }

        var ajaxFunction = ajaxAdapter.ajax;


        // CHANGE: 
        //      grab the custom headers to use in the AJAX POST

        var customHeaders = ajaxAdapter.defaultSettings.headers;
        if (ajaxFunction) {
            ajaxAdapter.ajax = function (settings) {
                processSettings(settings, customHeaders);
                return ajaxFunction(settings);
            };
        }
    }

    // Handle the POST-specific properties in the settings - $method, $data, $encoding
    function processSettings(settings, customHeaders) {
        if (settings) {
            var parameters = settings.params;
            if (parameters) {
                // wrapped data; handle the special properties
                settings.type = parameters.$method || settings.type; // GET is default method

                var data = parameters.$data;
                if (data) {
                    // if $data exists, assume all of other parameters are guidance for building a POST
                    if (parameters.$encoding === 'JSON') {
                        // JSON encoding 
                        settings.processData = false; // don't let JQuery form-encode it 
                        settings.contentType = "application/json; charset=UTF-8";

                        // CHANGE
                        //      Changed params to be data instead of parameters
                        //      This puts the JSON into the body of the request instead of query string

                        if (typeof (data) === 'object') {
                            settings.data = JSON.stringify(data); // encode parameters as JSON
                        } else {
                            settings.data = data;
                        }
                    } else {
                        settings.data = data;
                    }
                    // CHANGE
                    //      must be null or jQuery ajax adapter won't see settings.data
                    settings.params = null;
                }
            }
        }
        // CHANGE
        //      Set the custom headers
        if (customHeaders) {
            settings.headers = {  };
            // Check for headers option
            $.each(customHeaders, function (index, item) {
                settings.headers[index]= item;
            });
        }

        return settings;
    }

})();

///////// End AJAX POST implementation //////////////////////////