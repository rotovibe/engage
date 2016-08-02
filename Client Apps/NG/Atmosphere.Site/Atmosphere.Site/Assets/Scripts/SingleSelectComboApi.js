function SetSingleSelectDropDownWidth(combo) {
    if (combo) {
        var comboWidth = $(combo._element).width();
        var maxDropDownWidth = window[combo.get_id() + '_maxDropDownWidth'];
        var comboItems = combo.get_items();
        var longestString = '';
        for (var index = 0; index < comboItems.get_count(); index++) {
            var currentText = comboItems.getItem(index).get_text();
            if (currentText.length > longestString.length)
                longestString = currentText;
        }
        var longestStringWidth = longestString.GetSingleSelectItemWidth() + 20;
        if (longestStringWidth <= comboWidth)
            combo._dropDownWidth = comboWidth;
        else {
            if (maxDropDownWidth == 0) {
                combo._dropDownWidth = longestStringWidth;
            } else {
                if (maxDropDownWidth <= longestStringWidth)
                    combo._dropDownWidth = maxDropDownWidth;
                else {
                    combo._dropDownWidth = longestStringWidth;
                }
            }
        }
    }
}

String.prototype.GetSingleSelectItemWidth = function () {
    var element = $('<div class="RadComboBoxDropDown RadComboBoxDropDown_Atmosphere "><div class="rcbScroll rcbWidth"><ul style="list-style: none; margin: 0px; padding: 0px; zoom: 1;" class="rcbList"><li class="rcbItem ">' + this + '</li></ul></div></div>')
                .css({ 'position': 'absolute', 'float': 'left', 'white-space': 'nowrap', 'visibility': 'hidden' })
                .appendTo($('body'));
    var elementWidth = element.width();
    element.remove();
    return elementWidth;
}