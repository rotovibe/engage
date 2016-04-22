/**
*	@module	viewmodel - chsnsingle view model wrapps the chosen plugin (see "chosen" binding handler in bindings.js)
*
*	@param	isCancel - optional: if set to true it will show x button to clear the selected value.
*	@param	isSearch - optional: if set to true it will show a search box (typeahea).
*	@param	stickeySearchItemText - optional: string, must have isSearch set, expected string should be the text of one of the selection options.
*			this will force the stickey item to keep showing in the search results. (example usage - "add new" item).
*
*	@param	selectedValue
*	@param	options
*	@param	text - the property holding the text to present for every option object.
*	@param	caption - optional: text to show when no selection.
*	@param	isFocused - boolean/observable optional: to dynamically control the focus set an observable. statically - boolean value.
*	@param	searchQueryText - optional, observable: to expose the value of the typed search query.
*/
define(['durandal/composition'], function (composition) {

	var subscriptionTokens= [];
	var self = this;

	var ctor = function () {
    };

	/**
	*	@method compositionComplete - after data binding and composition completed.
	*			purpose: when a search box is applied (isSearch = true), and if there is a stickey option applied (stickeySearchItemText),
	*					we need to keep the sticky item in the chosen-results ul list, even if it does not match the search token.
	*
	*	@param view - the given chsnsingle element (built by chosen plugin) now carries all the underlined dom.
	*/
	ctor.prototype.compositionComplete = function(view, parent){
		var self = this;
		if(ko.utils.unwrapObservable(self.isFocused)){
			self.selectElm = $(view).find('select');
			self.selectElm.trigger('chosen:activate');
		}
		if(!self.isSearch || !self.stickeySearchItemText){
			return;
		}
		self.searchTokenInput = $(view).find('div.chosen-search input');
		self.ulSearchResults = $(view).find('ul.chosen-results');

		if(!self.searchTokenInput){
			return;
		}

		var stickeyOption = $(view).children('select').children('option:contains('+ self.stickeySearchItemText +')');
		self.stickeyIndex = stickeyOption.index();

		//subscribe to the search box content changes
		$(self.searchTokenInput).on('change keyup paste', function(event){
			var newValue = $(this).val();
			if(ko.isObservable(self.searchQueryText)){
				self.searchQueryText(newValue);	//expose the query text
			}
			if(newValue && newValue.length > 0 || event.type === 'paste' || event.type === 'cut'){
				//query changed
				var highlightIt = false;
				var keyCode = event.which || event.keyCode;
				if(self.ulSearchResults.children('.no-results').length == 1){
					//there are no matching results
					highlightIt = true;
					if(keyCode == 40){ //down

					}
				}
				if(!addStickySearchResultItem(highlightIt)){
					//stickey item was already in the results
					if(keyCode == 13){ //enter
						if(self.ulSearchResults.children().length == 2 && highlightIt){
							//select the sticky item: self.stickeyIndex
							if(self.stickeyIndex && self.stickeyIndex > 0 && self.options && self.options.length > 0){
								if(self.idValue){
									self.selectedValue(self.options[self.stickeyIndex -1][self.idValue]);
								}
								else{
									self.selectedValue(self.options[self.stickeyIndex -1]);
								}
							}
						}
					}
				}
			}
			if(event.type === 'paste' || event.type === 'cut'){
				setTimeout(function(){ addStickySearchResultItem(); }, 100);
			}
		});

		function addStickySearchResultItem(highlightIt){
			var added = false;
			var addNewElm = $(self.ulSearchResults).children('li:contains(' + self.stickeySearchItemText + ')');
			if(addNewElm.length == 0){
				var highlight = highlightIt? 'highlighted' : '';
				//add the stickey "-add new-" option to the end of the chosen-results box:
				self.ulSearchResults.append('<li class="active-result ' + highlight + '" data-option-array-index="' + self.stickeyIndex + '">' + self.stickeySearchItemText + '</li>');
				added = true;
			}
			return added;
		}
	}

    ctor.prototype.activate = function (settings) {
      var self = this;
      self.settings = settings;
      self.options = self.settings.options;
      self.selectedValue = self.settings.value;
      self.text = self.settings.text;
      self.label = self.settings.label;
      self.idValue = self.settings.idValue;
      self.disabled = self.settings.disabled;
      self.isRequired = self.settings.isRequired;
      self.caption = self.settings.caption ? self.settings.caption : 'Choose one';
			self.isCancel = self.settings.isCancel ? self.settings.isCancel : false;
			self.isSearch = self.settings.isSearch ? self.settings.isSearch : false;
			self.searchQueryText = self.settings.searchQueryText;
			self.stickeySearchItemText = self.settings.stickeySearchItemText? self.settings.stickeySearchItemText : null;	//"-Add New-";
			self.title = self.settings.title? self.settings.title: null;
			self.isFocused = self.settings.isFocused? self.settings.isFocused : null;
      self.isInvalid = ko.computed(function () {
        return self.isRequired && !self.selectedValue();
      });
      self.computedOptions = ko.computed(function () {
        var thisList = ko.unwrap(self.options);
        ko.utils.arrayForEach(thisList, function (item) {
	        // Create a property to dynamically set the showing property
          if (ko.isObservable(item[self.text])) {
            item.thisText = ko.computed(item[self.text]);
          } else {
            item.thisText = ko.computed(function () { return item[self.text]; });
          }
        });
        return thisList;
      });
    };

    ctor.prototype.attached = function () {
    };

	ctor.prototype.detached = function() {
		var self = this;
		ko.utils.arrayForEach(subscriptionTokens, function (token) {
			token.dispose();
		});
		self.isInvalid.dispose();
		$(self.searchTokenInput).remove();
	}
    return ctor;
});