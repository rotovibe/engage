
define(['services/analytics'], function(analytics){
	var isLogging = false;			
	return {
		initialize: function(gaId, contractId, userName){			
			if(gaId && gaId.length > 0){
				isLogging = true;
				console.log('google analytics logger initialized: gaId=' + gaId);
				analytics.initialize(gaId, contractId, userName);
			}
			else{
				console.log('google analytics logger is turned off. (gaId is empty)');
			}
		},
		logNavigation: function(route){	
			if(isLogging){				
				analytics.sendPageView(route);			
			}
		}    		
	};
});