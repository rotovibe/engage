var fixedStateManager = function () {

    initHorizontalFixed = function (fixedObject, parentObject, parentOffset, rightMargin, leftBoundary, overrideTop) {

        // parentOffset = margin + padding of the element in relation to the form/page
        // rightMargin = how far (in pixels) you wish to displace the element from the right of the viewport when position is set to fixed
        // leftBoundary = minimum distance allowed between the left of the fixedObject and the left of the viewport
        // overrideTop = define top in pixels in case there is an issue with the calculation

        var topMargin = 0;

        if (overrideTop == 0) {
            var offset = fixedObject.offset();
            topMargin = offset.top;
        } else {
            topMargin = overrideTop;
        }

        var defaultPos1 = getPositions(fixedObject),
            defaultPos2 = getPositions(parentObject);

        if (comparePositions(defaultPos1[0], defaultPos2[0]) && $(window).width() < (parentObject.width() + parentOffset)) {
            $(fixedObject).css('position', 'fixed');
            $(fixedObject).css('right', rightMargin);
        }

        $(fixedObject).css('top', topMargin - $(window).scrollTop());

        $(window).scroll(function () {
            if ($(fixedObject).css('position') != 'static') {
                $(fixedObject).css('top', topMargin - $(window).scrollTop());
            }
        });

        $(window).resize(function () {
            if ($(fixedObject).css('position') == 'static') {
                if (overrideTop == 0) {
                    var offset = fixedObject.offset();
                    topMargin = offset.top;
                } else {
                    topMargin = overrideTop;
                }
                $(fixedObject).css('top', topMargin - $(window).scrollTop());
            }
            if ($(window).width() < leftBoundary) {
                $(fixedObject).css('right', $(window).width() - leftBoundary + rightMargin);
            } else {
                setFixedObjectPosition();
            }
        });

        function setFixedObjectPosition() {
            var pos1 = getPositions(fixedObject),
                pos2 = getPositions(parentObject);

            if (pos1 != null && pos2 != null) {
                if (pos1[0][0] != 0) {
                    if (comparePositions(pos1[0], pos2[0]) && $(window).width() < (parentObject.width() + parentOffset)) {
                        $(fixedObject).css('position', 'fixed');
                        $(fixedObject).css('right', rightMargin);
                    } else {
                        $(fixedObject).css('position', 'static');
                    }
                }
            }
        }
    },

    getPositions = function (elem) {
        var pos, width, height;

        try {
            pos = $(elem).position();
            width = $(elem).width();
            height = $(elem).height();
            return [[pos.left, pos.left + width], [pos.top, pos.top + height]];
        } catch (err) {
            return null;
        }
    },

    comparePositions = function (p1, p2) {
        var r1, r2;

        r1 = p1[0] < p2[0] ? p1 : p2;
        r2 = p1[0] < p2[0] ? p2 : p1;
        return r1[1] > r2[0] || r1[0] === r2[0];
    }

    return {
        initHorizontalFixed: initHorizontalFixed
    };

} ();


