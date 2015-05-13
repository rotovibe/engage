
define([], function(){	
	
	var initialize = function(gaId, contractId, userName){								
		if(window.ga){
			//window.ga('create', gaId, 'auto'); //'UA-XXXX-Y' //the 3'rd can be 'auto' or exact serverDomain 
			window.ga('set', 
					{
						//need to set in the google analytics account the custom dimensions to track contractId / userName 
						//and use their indexes (1 / 2 respectively):
						'dimension1': contractId, 
					    'dimension2': userName		
					}
				);
			console.log('analytics initialized : gaId=' + gaId + ', contractId= ' + contractId + ', ' + 'userName= ' + userName);
		}				
		
	}
		
	var sendPageView = function(path){
		console.log('analytics sendPageView : path=' + path);
			if(window.ga){						
				window.ga('send', 'pageview',
					{
						'page': path						
					}
				);			
			}		
	}
	
	return{
		initialize: initialize,
		sendPageView: sendPageView		
	}
});
