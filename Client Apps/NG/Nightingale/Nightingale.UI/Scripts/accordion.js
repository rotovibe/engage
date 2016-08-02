(function () {
    $(document).on('ready page:load', function () {
        return $('.accordion-toggle').click(function () {
            var accordion;
            accordion = $(this).parents('.accordion');
            accordion.toggleClass('open');
            accordion.find('.accordion-content').slideToggle();
            return false;
        });
    });

}).call(this);