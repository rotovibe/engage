/*!	
	Licensed Materials - Property of IBM
	PID : 5725-Z49
	Copyright IBM Corp. 2013, 2016
	US Government Users Restricted Rights- Use, duplication or disclosure restricted by GSA ADP Schedule Contract with IBM Corp.
*/
function enableLoading() {
    var busy = document.getElementById("divLoading")
    if (busy != null)
        busy.className = "divLoading show";
}

function disableLoading() {
    var busy = document.getElementById("divLoading")
    if (busy != null)
        busy.className = "divLoading hide";
}