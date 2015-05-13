/**
*	@module customMessageBox - a custom message box based on durandal dialog plugin and its MessageBox.
* 					this dialog is generic and can be set with any buttons / message / title.
*	@function customMessageBox constructor:
*	@param message: the content on the body - usually text is expected.
*	@param title: text for dialog header. 
* 	@param settings: an array of objects - each object represent a button to be configured on the footer. 
*			when clicked - the "value" is what should be the dialog response.
*
*	@example use of this message box: a confirmation modal with yes/no/(and a cancel x):
*		
*		the caller js:
*		
*		//set up a message content:	
*		var message = 'You are about to delete: ' + medication.name() +' from this individual.  Press OK to continue, or cancel to return without deleting.';                
*		
*		//construct:		
*		this.dialog = new customMessageBox(message, 'are you sure?', [
*							{text:'Yes', value:true, css:'btn color'},		//each object represent a button on the footer. when clicked - the "value" is what should be the dialog response
*							{text:'No', value: false, css:'btn cancel'}
*							], false);
*		//show:
*		this.dialog.show().then(function(response){						
*			if(response){
*				//the expected response from clicking on the 'Yes' button has a value: true.
*				checkDataContext();
*				datacontext.deletePatientMedication(medication).then(deleted);
*				function deleted () {
*					return true;//self.medications().remove(medication);						                       
*				}
*			}
*			else{
*				//the expected response from clicking on the 'No' button has a value: false.
*				//note this will also whats returned when cancel (upright "X") is clicked to close the dialog.
*				return false;		
*			}	
*		});		
*	
*/
define(['plugins/dialog'], 
	function(dialog){
		var ctor = function(message, title, options, autoclose, settings) {
			var self = this;
			self.message = message;
			self.title = title || dialog.MessageBox.defaultTitle;
			self.options = options || dialog.MessageBox.defaultOptions;
			self.autoclose = autoclose || false;
			self.settings = $.extend({}, 
							{	
								buttonClass: "btn cancel", 
								primaryButtonClass: "btn color autofocus", 
								secondaryButtonClass: "btn cancel", 
								class: "content"								
							}, 
							settings);
			self.compositionComplete = function(child, parent, context) {				
				for (i = 0; i < child.children.length; i++){
					var div = child.children[i];
					if(div.nodeName == 'DIV' && div.className.indexOf("content") !== -1){
						div.style["margin-top"] = "auto";						
					}	
				}	
				child.style["visibility"] = "visible !important";	
				
				/**
				*	@note: disable the durandal dialog plugin reposition function so it wont mess up :)
				*	we need this workaround since we do not have the durandal.css embedded in our css.
				*	with this setup - our html view can define our own css to the modal parts
				*/
				var theDialog = dialog.getDialog(context.model);
				theDialog.context.reposition = function(){};				
			}			
		};
		ctor.prototype.show = function(){
			return dialog.show(this);
		}
		ctor.prototype.cancel = function(){
			return dialog.close(this, false);
		}		
		ctor.prototype.selectOption = function (dialogResult) {
			dialog.close(this, dialogResult);
		};		
		return ctor;
	}
);