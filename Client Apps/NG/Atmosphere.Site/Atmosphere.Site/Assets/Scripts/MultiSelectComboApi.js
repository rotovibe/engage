function onCheckBoxClick(selectAllCheckBoxID, isSelectAll, comboID, hdnFilterChangedId, hdnCheckedNamesId, hdnCheckedIdsId, hdnComboTextTemplateId, hdnAllSelectedTextId, enableSelectionCountValidation, maxSelectionCountAllowed, onClientSelectionCountExceeded, minSelectionCount, onClientInvalidMinSelection, onClientCheckboxClick, isAutoPostBack) {
    var text = "", values = "";
    var itemsCount = 0;
    var selectedItemCount = 0;
    var combo = $find(comboID);
    if (isSelectAll === 'False') {
        //get the collection of all items 
        var items = combo.get_items();
        itemsCount = items.get_count();
        var selectedCheckBoxPosition = 0;
        //enumerate all items 
        for (var i = 0; i < itemsCount; i++) {
            var item = items.getItem(i);
            //get the checkbox element of the current item 
            var checkBox = $get(combo.get_id() + "_i" + i + "_chkBx");
            if (checkBox.checked) {
                recentSelectedIndex = i;
                selectedItemCount++;
                text += item.get_text() + "|";
                values += item.get_value() + ",";
            }
        }
    }
    else {

        var items = combo.get_items();
        itemsCount = items.get_count();
        var selectAllChk = $get(selectAllCheckBoxID);
        if (selectAllChk) {
            for (var i = 0; i < itemsCount; i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item 
                var checkBox = $get(combo.get_id() + "_i" + i + "_chkBx");
                if (selectAllChk.checked) {
                    selectedItemCount++;
                    checkBox.checked = true;
                    text += item.get_text() + "|";
                    values += item.get_value() + ",";
                } else if (minSelectionCount != 1) {
                    checkBox.checked = false;
                }
            }
        }
    }
    //Set Combo Text
    if (selectedItemCount == 1) {
        combo.set_text(RemoveLastDelimiter(text));
    }
    else if (selectedItemCount == itemsCount && itemsCount > 0) {
        //All selected text
        combo.set_text($('#' + hdnAllSelectedTextId).val());
    }
    else if (selectedItemCount > 0) {
        combo.set_text($('#' + hdnComboTextTemplateId).val().replace("{#}", selectedItemCount));
    }
    else {
        combo.set_text(combo._emptyMessage);
    }

    //Set SelectAll checkbox state
    var selectAllCheckBox = $get(combo.get_id() + "_Header_chkBxSelectAll");
    if (selectAllCheckBox) {
        if (selectedItemCount == itemsCount && itemsCount > 0) {
            selectAllCheckBox.checked = true;
        } else {
            selectAllCheckBox.checked = false;
        }
    }

    //Selection Count validation
    if (enableSelectionCountValidation) {
        if (selectedItemCount > maxSelectionCountAllowed) {
            var onClientSelectionCountExceededFunction = window[onClientSelectionCountExceeded];
            if (typeof onClientSelectionCountExceededFunction === 'function') {
                onClientSelectionCountExceededFunction();

                //Uncheck current checkbox
                var currentCheckbox = $get(selectAllCheckBoxID);
                if (currentCheckbox)
                    currentCheckbox.checked = false;

                //get the collection of all items 
                var items = combo.get_items();
                itemsCount = items.get_count();
                //enumerate all items 
                for (var i = 0; i < itemsCount; i++) {
                    var item = items.getItem(i);
                    //get the checkbox element of the current item 
                    var chk1 = $get(combo.get_id() + "_i" + i + "_chkBx");
                    if (chk1.checked) {
                        selectedItemCount++;
                        text += item.get_text() + "|";
                        values += item.get_value() + ",";
                    }
                }
                setComboText(comboID, hdnAllSelectedTextId, hdnComboTextTemplateId);
            }
        }

        if (selectedItemCount < minSelectionCount) {
            var minValidationFunction = window[onClientInvalidMinSelection];
            if (typeof minValidationFunction === 'function') {
                minValidationFunction();

                //Uncheck current checkbox
                var currentCheckbox = $get(selectAllCheckBoxID);
                if (currentCheckbox)
                    currentCheckbox.checked = true;

                //get the collection of all items 
                var items = combo.get_items();
                itemsCount = items.get_count();
                //enumerate all items 
                for (var i = 0; i < itemsCount; i++) {
                    var item = items.getItem(i);
                    //get the checkbox element of the current item 
                    var checkBox = $get(combo.get_id() + "_i" + i + "_chkBx");
                    if (checkBox.checked) {
                        selectedItemCount++;
                        text += item.get_text() + "|";
                        values += item.get_value() + ",";
                    }
                }
                setComboText(comboID, hdnAllSelectedTextId, hdnComboTextTemplateId);
            }
        }
    }

    //Raise Client Checkbox click event
    if (onClientCheckboxClick != '') {
        var onClientCheckboxClickFunction = window[onClientCheckboxClick];
        if (typeof onClientCheckboxClickFunction === 'function') {
            onClientCheckboxClickFunction(selectedItemCount);
        }
    }

    //remove the last comma from the string 
    text = RemoveLastDelimiter(text);
    values = RemoveLastComma(values);
    $('#' + hdnFilterChangedId).val("true");
    $('#' + hdnCheckedNamesId).val(text);
    $('#' + hdnCheckedIdsId).val(values);
}

function setComboText(comboID, hdnAllSelectedTextId, hdnComboTextTemplateId) {
    var text = "", values = "";
    var itemsCount = 0;
    var selectedItemCount = 0;
    var combo = $find(comboID);
    //get the collection of all items 
    var items = combo.get_items();
    itemsCount = items.get_count();
    //enumerate all items 
    for (var i = 0; i < itemsCount; i++) {
        var item = items.getItem(i);
        //get the checkbox element of the current item 
        var checkBox = $get(combo.get_id() + "_i" + i + "_chkBx");
        if (checkBox.checked) {
            selectedItemCount++;
            text += item.get_text() + "|";
            values += item.get_value() + ",";
        }
    }

    //Set Combo Text
    if (selectedItemCount == 1) {
        combo.set_text(RemoveLastDelimiter(text));
    }
    else if (selectedItemCount == itemsCount && itemsCount > 0) {
        //All selected text
        combo.set_text($('#' + hdnAllSelectedTextId).val());
    }
    else if (selectedItemCount > 0) {
        combo.set_text($('#' + hdnComboTextTemplateId).val().replace("{#}", selectedItemCount));
    }
    else {
        combo.set_text(combo._emptyMessage);
    }
}

function RemoveLastComma(stringValue) {
    return stringValue.replace(/,$/, "").replace(/|$/, "").trim();
}

function RemoveLastDelimiter(stringValue) {
    var stringLength = stringValue.length;
    if (stringLength > 1)
        return stringValue.slice(0, stringLength - 1);
    else
        return stringValue;
}

function SetMultiSelectDropDownWidth(combo) {
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
        var longestStringWidth = longestString.GetMultiSelectItemWidth() + 20;
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

String.prototype.GetMultiSelectItemWidth = function () {
    var element = $('<div class="RadComboBoxDropDown RadComboBoxDropDown_Atmosphere "><div class="rcbScroll rcbWidth"><ul style="list-style: none; margin: 0px; padding: 0px; zoom: 1;" class="rcbList"><li class="rcbItem  rcbTemplate"><span class="filterCheckBox" name="10"><input type="checkbox"><label>' + this + '</label></span></li></ul></div></div>')
                .css({ 'position': 'absolute', 'float': 'left', 'white-space': 'nowrap', 'visibility': 'hidden' })
                .appendTo($('body'));
    var elementWidth = element.width();
    element.remove();
    return elementWidth;
}
