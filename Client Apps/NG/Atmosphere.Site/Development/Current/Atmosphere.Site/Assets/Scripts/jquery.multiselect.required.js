/*
 * jQuery MultiSelect UI Widget Filtering Plugin 1.1
 * Copyright (c) 2010 Eric Hynds
 *
 * http://www.erichynds.com/jquery/jquery-ui-multiselect-widget/
 *
 * Depends:
 *   - jQuery UI MultiSelect widget
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 *
*/
(function($){
	var rEscape = /[\-\[\]{}()*+?.,\\^$|#\s]/g;
	
	$.widget("ech.multiselectrequired", {
		
		_create: function(){
			var self = this,
				opts = this.options,
				instance = (this.instance = $(this.element).data("multiselect")),
				
				// store header; add filter class so the close/check all/uncheck all links can be positioned correctly
				header = (this.header = instance.menu.find(".ui-multiselect-header").addClass("ui-multiselect-hasfilter")),
				headerLinkContainer = (this.headerLinkContainer = instance.menu.find('.ui-multiselect-header ul')).remove();

			  // reference to the actual inputs
        var inputs = instance.menu.find('input[type="checkbox"], input[type="radio"]');
        var selectAll = (this.selectAll = $('<div class="ui-multiselect-required"><label><input type="checkbox" value="">All</label></div>')).appendTo(header);


      inputs.change(function () {
        if (inputs.not(':checked').length == 0) {
          selectAll.find('input').attr('checked', 'checked');
        }
        else {
          selectAll.find('input').removeAttr('checked');
        }
      });
      
      selectAll.find('input').change(function (event) {
        if ($(this).is(':checked')) {
          inputs.attr('checked', 'checked');
        }
      });
			
			headerLinkContainer.empty();
			
		},
		destroy: function(){
			$.Widget.prototype.destroy.call( this );
			this.input.val('').trigger("keyup");
			this.wrapper.remove();
		}
	});
})(jQuery);
		