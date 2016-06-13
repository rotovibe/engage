/**
*	string format helper utility
*	@module formatter
*/
define([],
	function(){
		var formatter = function(){};
		
		/**
		*	zero left pad a number string
		*	@method padZeroLeft
		*	@param num {String} a string number
		*	@param size {int} number of digits of the padded result 	
		*/
		formatter.padZeroLeft = function (num, size){
			var s = num+"";
			while (s.length < size) s = "0" + s;
			return s;
		}
		
		/**
		*	@param newDetails {String} observable
		*	@param details {String} observable
		*	@method appendNewDetails
		*/
		formatter.appendNewDetails = function( newDetails, details, userName ){
            if( newDetails() ){
                var append = '';
                if( details() && details().length ){
                    append = '\n';
                }
                append += moment().format('MM-DD-YYYY h:mm A') + ' ';
                append += (' ' + userName);
                append += (' - ' + newDetails());
                details(details() ? details() + append : append);
                newDetails('');
            }
        }
		
		/**
		*	
		*	@method formatSeparators
		*	@param inp the input {string} can contain any characters, including the separator.
		*	@param xPattern {string} expecting a string with a distinct character/s that are different from the separator.
		*	@param separator {string} a char to be added/positioned according to its appearence in the xPattern.
		*	@example 
		*		formatter.formatSeparators('1234567890', 'XXX-XXX-XXXX', '-')			
		*	result:		
		*		"123-456-7890"
		*/
		formatter.formatSeparators = function(inp, xPattern, separator){	//'1234567890', 'XXX-XXX-XXXX', '-' => 123-456-7890
			var parts = xPattern.split(separator);
			inp = inp.replace(new RegExp(separator, 'g'), '');	//remove all separators
			var relevantLength = xPattern.replace(new RegExp(separator, 'g'), '').length; //length of pattern content without separators	
			var i = 0, pos = 0;
			var result = '';
			while (i < parts.length && pos < inp.length){
				var section =  inp.slice(pos, pos + parts[i].length);
				pos += section.length;
				result += section;
				if( section.length === parts[i].length && pos < relevantLength -1 ){
					//full section added and there is more
					result += separator;
				}
				i += 1;
			}
			return result;
		};
		
		/**
		*	formatting utilities specifically for a typable date field.
		*	@class formatter.date
		*/
		formatter.date = function(){};
		
		/**
		*	given a date string including a year part, the year needs to be auto-completed (century / decade).
		*	@method optimizeYear
		*	@param date {string} MM/DD/Y...
		*/
		formatter.date.optimizeYear = function( date ){			
			var dateParts = date.split('/');
			//optimize year only if we have all date parts in place:
			if( dateParts.length > 2 && dateParts[2].length > 0 && dateParts[2].length <= 2){																				
				//fix year if year entered partially:	"15" => "2015" ; "69" => "1969" ; "1" => "2001"
				var year = dateParts[2];
				if( year.length == 1 ){
					//auto-complete decade:
					year = '0' + year;
				}			
				//auto-complete century:	(since moment does not do it perfectly)
				var thisYear = Number(String(moment().year()).slice(2,4));
				if( Number(year) > (thisYear) ){
					year = '19' + year;
				}
				else{
					year = '20' + year;
				}
				var newDate = dateParts[0] + '/' + dateParts[1] + '/' + year;
				date = newDate;							
				if( moment(date).isValid() ){
					date = moment(date).format("MM/DD/YYYY");
				}
			}			
			return date;
		};
		
		/**
		*	given a date string (or an incomplete part of it, while it is typed in), format it to MM/DD/....
		*	except for the year part. the year part should be kept intact at this point.
		*	@method optimizeDate
		*	@param date {string} a date string - combination of digits with or without separator '/'
		*/
		formatter.date.optimizeDate = function( date ){			
			var newDate = date;
			if( date ){
				var month, day;				
				var dateParts = date.split('/');													
				if( dateParts.length >= 1 ){
					newDate = optimizeMonth(dateParts[0], dateParts.length > 1);					
				}					
				if( dateParts.length >= 2 ){
					newDate += optimizeDay(dateParts[1], dateParts.length > 2);
					if( dateParts.length > 2 ){
						newDate += dateParts[2];									
					}
				}																																 
			}
			return newDate;
			
			function optimizeMonth( monthStr, complete ){				
				var result = monthStr;
				if( monthStr.length > 0 ){
					if( monthStr.length >= 2 ){
						//first date part (month) entered, and now added another digit without separator:												
						result = monthStr.slice(0, 2);
						if( Number(result) > 12 ){
							result = '12';
						}
						if( Number(result) < 1 ){
							result = '01';
						}
						result += '/';
						if( monthStr.length > 2 ){
							var day = monthStr.slice(2,4);							
							result += optimizeDay(day, monthStr.length < 4);
							if( monthStr.length > 4 ){
								result += monthStr.slice(4,8);
							}
						}
					}
					else if( monthStr.length === 1 && ( Number(monthStr) > 1 || complete ) ){
						//single digit month: left pad 0 
						if( Number(monthStr) === 0 ){
							result = '01/';
						}
						else{
							result = '0' + monthStr + '/';	
						}						
					}
				}
				return result;
			}
			
			function optimizeDay( dayStr, complete ){
				var result = dayStr;
				if( dayStr.length > 0 ){					
					//optimize day if needed:
					if( dayStr.length == 1 ) {
						if( complete || Number(dayStr) > 3 ){
							result = '0' + dayStr + '/';
						}
						if( complete && Number(dayStr) < 1 ){
							result = '01/'; 
						}
					}
					else if( dayStr.length >= 2 ){
						result = dayStr.slice(0,2);
						if( Number(result) > 31 ){
							result = '31';							
						}
						if( Number(result) < 1 ){
							result = '01';
						}
						result += '/'; 
						if( dayStr.length > 2 ){
							result += dayStr.slice(2);
						}
					}
				}
				return result;
			}
		};
		
		/**
		*	masking time string keyboard entry. intended as helper fo jqtimepicker
		*	@method optimizeTime
		*	@param	time {String}
		*/
		formatter.date.optimizeTime = function( time ){
			var newTime = time;
			if( time ){
				var timeParts = time.split(':');				
				if( timeParts.length >= 1 ){
					var part = timeParts[0];
					part = part.replace( /\D/g, '');					
					if( part.length === 1 && timeParts.length > 1){
						newTime = '0' + part + ':';											
					}
					else if( part.length === 2 ){
						newTime = part + ':';					
					}					
					if( timeParts.length > 1 ){			
						var part = timeParts[1];					
						newTime = newTime + timeParts[1];
					}
				}				
			}
			return newTime;
		};
		
		return formatter;
});