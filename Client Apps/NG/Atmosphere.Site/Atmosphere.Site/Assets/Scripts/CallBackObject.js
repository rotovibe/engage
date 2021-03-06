/*!	
	Licensed Materials - Property of IBM
	PID : 5725-Z49
	Copyright IBM Corp. 2013, 2016
	US Government Users Restricted Rights- Use, duplication or disclosure restricted by GSA ADP Schedule Contract with IBM Corp.
*/
function CallBackObject()
{
  this.XmlHttp = this.GetHttpObject();
}
 
CallBackObject.prototype.GetHttpObject = function()
{ 
  var xmlhttp;
  
  // branch for native XMLHttpRequest object
    if(window.XMLHttpRequest && !(window.ActiveXObject)) {
	    try {
		    xmlhttp = new XMLHttpRequest();
        } catch(e) {
		    xmlhttp = false;
        }
    // branch for IE/Windows ActiveX version
    } else if(window.ActiveXObject) {
       
   	    try {
    	    xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
  	    } catch(e) {
    	    try {
      		    xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
    	    } catch(e) {
      		    xmlhttp = false;
    	    }
	    }
    }


  /*@cc_on
  @if (@_jscript_version >= 5)
    try {
      xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");
    } catch (e) {
      try {
        xmlhttp = new ActiveXObject("Microsoft.XMLHTTP");
      } catch (E) {
        xmlhttp = false;
      }
    }
  @else
  xmlhttp = false;
  @end @*/
  
  /*
  if (!xmlhttp && typeof XMLHttpRequest != 'undefined') {
    try {
      xmlhttp = new XMLHttpRequest();
    } catch (e) {
      xmlhttp = false;
    }
  }
  */
  
  return xmlhttp;
}
 //alert(window.location.hostname)
CallBackObject.prototype.DoCallBack = function(eventTarget, eventArgument, callbackPage, enableViewState)
{
  var theData = '';
  var theform = document.forms[0];
  var thePath = "";
  
  var colPath = window.location.pathname.split('/');
  
  //this will create the url path with out the page name
  for(var j=0; j<colPath.length-1;j++)
  {
    thePath += colPath[j] + '/';
  }
  
  //var thePage = 'PQRIForm.aspx'; //window.location.pathname + window.location.search;
  if(callbackPage == undefined || callbackPage == '')
    var thePage = window.location.protocol + '//' + window.location.hostname + '/' + window.location.pathname + window.location.search;
  else
    var thePage = window.location.protocol + '//' + window.location.hostname + thePath + callbackPage;    
    
  var eName = '';

  theData  = '__EVENTTARGET='  + escape(eventTarget.split("$").join(":")) + '&';
  theData += '__EVENTARGUMENT=' + eventArgument + '&';
  
  if(enableViewState == true)
    theData += '__VIEWSTATE=' + escape(theform.__VIEWSTATE.value).replace(new RegExp('\\+', 'g'), '%2b') + '&';
  
  theData += 'IsCallBack=true&';

  for( var i=0; i<theform.elements.length; i++ )
  {
    eName = theform.elements[i].name;
    if( eName && eName != '')
    {
      if( eName == '__EVENTTARGET' || eName == '__EVENTARGUMENT' || eName == '__VIEWSTATE' )
      {
        // Do Nothing
      }
      else
      {
        theData = theData + escape(eName.split("$").join(":")) + '=' + theform.elements[i].value;
        if( i != theform.elements.length - 1 )
          theData = theData + '&';
      }
    }
  }

  if( this.XmlHttp )
  {
    if( this.XmlHttp.readyState == 4 || this.XmlHttp.readyState == 0 )
    {
      var oThis = this;
      this.XmlHttp.open('POST', thePage, false);
      this.XmlHttp.onreadystatechange = function(){ oThis.ReadyStateChange(); };
      this.XmlHttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
      this.XmlHttp.send(theData);
      //this.XmlHttp.send();
    }
  }
}
 
CallBackObject.prototype.AbortCallBack = function()
{
  if( this.XmlHttp )
    this.XmlHttp.abort();
}
 
CallBackObject.prototype.OnLoading = function()
{
  // Loading
}
 
CallBackObject.prototype.OnLoaded = function()
{
  // Loaded
}
 
CallBackObject.prototype.OnInteractive = function()
{
  // Interactive
}
 
CallBackObject.prototype.OnComplete = function(responseText, responseXml)
{
  // Complete
}
 
CallBackObject.prototype.OnAbort = function()
{
  // Abort
}
 
CallBackObject.prototype.OnError = function(status, statusText)
{
  // Error
}
 
CallBackObject.prototype.ReadyStateChange = function()
{
  if( this.XmlHttp.readyState == 1 )
  {
    this.OnLoading();
  }
  else if( this.XmlHttp.readyState == 2 )
  {
    this.OnLoaded();
  }
  else if( this.XmlHttp.readyState == 3 )
  {
    this.OnInteractive();
  }
  else if( this.XmlHttp.readyState == 4 )
  {
    if( this.XmlHttp.status == 0 )
      this.OnAbort();
    else if( this.XmlHttp.status == 200 && this.XmlHttp.statusText == "OK" )
    {
      //alert(this.XmlHttp.statusText )
      this.OnComplete(this.XmlHttp.responseText, this.XmlHttp.responseXML);
    }
    else
      this.OnError(this.XmlHttp.status, this.XmlHttp.statusText, this.XmlHttp.responseText);   
  }
}
