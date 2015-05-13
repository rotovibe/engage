//Encapsulates data calls to server (AJAX calls)

// Not making AJAX calls so this will not be used

//var dataService = new function () {
//    var serviceBase = '/DataService/',
//        getAccount = function(acctNumber, callback) {
//            $.getJSON(serviceBase + 'GetAccount', {acctNumber:acctNumber}, function(data) {
//                callback(data);
//            });
//        },

//        getMarketIndexes = function(callback) {
//            $.getJSON(serviceBase + 'GetMarketIndexes', function(data) {
//                callback(data);
//            });
//        },

//        getQuote = function(sym, callback) {
//            $.getJSON(serviceBase + 'GetQuote', { symbol: sym }, function(data) {
//                callback(data);
//            });
//        },

//        getTickerQuotes = function(callback) {
//            $.getJSON(serviceBase + 'GetTickerQuotes', function(data) {
//                callback(data);
//            });
//        };

//    return {
//        getAccount: getAccount,
//        getMarketIndexes: getMarketIndexes,
//        getQuote: getQuote,
//        getTickerQuotes: getTickerQuotes
//    };

//} ();