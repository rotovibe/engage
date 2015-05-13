
// Compares two arrays and determines if they are the same (true) or different (false)

jQuery.extend({
    compareArrays: function (arrayA, arrayB) {
        if (arrayA.length != arrayB.length) {
            return false;
        }
        var a = jQuery.extend(true, [], arrayA);
        var b = jQuery.extend(true, [], arrayB);
        a.sort();
        b.sort();
        for (var i = 0, l = a.length; i < l; i++) {
            if (a[i] !== b[i]) {
                return false;
            }
        }
        return true;
    }
});