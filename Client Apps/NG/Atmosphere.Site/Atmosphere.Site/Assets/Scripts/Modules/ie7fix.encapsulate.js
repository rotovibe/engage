var ie7fixEncapsulate = function () {

    init = function (childElement, parentContainer, padding) {

        var width = $(childElement).width();
        if (width < 990) {
            width = 990;
        }

        $(parentContainer).css('width', (width + (padding * 2)));

        $(window).scroll(function () {
            var width = $(childElement).width();
            if (width < 990) {
                width = 990;
            }
            $(parentContainer).css('width', (width + (padding * 2)));
        });

        $(window).resize(function () {
            var width = $(childElement).width();
            if (width < 990) {
                width = 990;
            }
            $(parentContainer).css('width', (width + (padding * 2)));
        });
    }

    return {
        init: init
    };

} ();


