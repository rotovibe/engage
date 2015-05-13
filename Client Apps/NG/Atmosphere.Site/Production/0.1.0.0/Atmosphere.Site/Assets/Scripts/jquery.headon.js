/*
 * jquery.headon.js
 * @author: Bartram Nason
 * @version: 1.0
 */
(function($){

  var methods = {
    init: function (options) {

      var settings = {
        offset: 0,
        offsetParent: undefined,
        adjacentSibling: undefined
      };
  
      if (options) {
        $.extend(settings, options);
      }
      
      var elements = this.each(function () {

        var data = $(this).data('headon');
        if (!data) {

          var position = $(this).position();
          var width = $(this).width();
          var height = $(this).height();
          
          var offsetParent = settings.offsetParent ? $(this).closest(settings.offsetParent) : $(this).offsetParent();
          var adjacentSibling = undefined;
          
          if (settings.adjacentSibling) {
            adjacentSibling = offsetParent.find(settings.adjacentSibling).css({paddingTop: height});
          }
          else if ($(this).css('position') != 'absolute') {
            
            var marginTop = $(this).css('marginTop');
            var marginBottom = $(this).css('marginBottom');
            
            adjacentSibling = $('<div></div>')
              .css({
                height: 0,
                paddingTop: height,
                marginTop: marginTop,
                marginBottom: marginBottom
              })
              .insertAfter(this);
          }

          $(this).data('headon', {
            position: position,
            offsetParent: offsetParent,
            adjacentSibling: adjacentSibling
          });

          $(this).css({
            position: 'absolute',
            top: 0,
            width: width,
            marginTop: 0,
            marginBottom: 0
          });

        }

      });

      $(window).bind('scroll resize', function (event) {
        var scrollTop = $(window).scrollTop();
  
        elements.each(function () {
          var data = $(this).data('headon');

          var offsetParentOffset = data.offsetParent.offset();
          var offsetParentHeight = data.offsetParent.height();
          
          var offset = $.isFunction(settings.offset) ? settings.offset.call(this) : settings.offset;
  
          // if the window scrolltop is between the top and the bottom of the parent element
          if (scrollTop + offset > offsetParentOffset.top && scrollTop + offset < offsetParentOffset.top + offsetParentHeight) {
  
            // if the difference between the window scroll and the bottom of the parent element is less than the height of the header
            if ((offsetParentOffset.top + offsetParentHeight) - (scrollTop + offset) < $(this).height()) {
              $(this).css({position: 'absolute', top: '', bottom: 0, left: data.position.left});
            }
            else {
              $(this).css({position: 'fixed', top: offset, bottom: '', left: offsetParentOffset.left + data.position.left});
            }
          }
          else {
            if (scrollTop + offset > offsetParentOffset.top) {
              $(this).css({position: 'absolute', top: '', bottom: 0, left: data.position.left});
            } else {
              // TODO: put the header at the bottom if it's above the currently selected element
              $(this).css({position: 'absolute', top: 0, bottom: '', left: data.position.left});
            }
          }
        });
      });
      return this;

    },
    update: function () {
      return this.each(function () {
        var data = $(this).data('headon');
        if (data) {
          data.adjacentSibling.css({
            paddingTop: $(this).height()
          });
        }
      })
    }
  }

  $.fn.headon = function(method) {
    if (methods[method]) {
      return methods[method].apply(this);
    }
    else {
      return methods.init.apply(this, arguments)
    }
  }
})(jQuery);