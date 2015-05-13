
        var unitComboChanged = false;
        var selectColumnsComboChanged = false;

        //Telerik Code
        var cancelDropDownClosing = false;

        $(document).ready(function () {
            OnLoad();
            /* Home Page Related */
            $('#divAssesmentCheckboxes input[type="checkbox"]').click(function () {
                $get("<%=btnLeftSideFilter.ClientID%>").click();


            });

            $('#divFollowupCheckboxes input[type="checkbox"]').click(function () {
                if (document.getElementById("<%= btnUpdate.ClientID %>").value != "View Results") {
                    ShowMessage("The filter selection has changed. Click <strong>Update Results</strong> to view new results.");
                    $('div.gridSection').css({ 'top': '190px' });

                }
            });

            /* Discharge Patients Details Page Related JQuery*/

            //Show Notes related Section, when clicked on Add Notes Div
            //            $("#AddNotes").live("click", function () {
            //                $(".addNotes").show();
            //            });

            //            $("#Cancel").live("click", function () {
            //                var txtNote = $('.txtNoteVal');
            //                txtNote.val("");
            //                $(".addNotes").hide();

            //            });

            //navigation to parent details
            $('.navToParentDetails').live('click', function () {
                $get("<%= btnShowParentGrid.ClientID %>").click();
            });

            // Show the Rad Pan Sections on the basis of Radio Button Click
            $('.dischargeLeftSideFilter input[type="radio"]').live('click', function () {
                $(".show").click();

            });
        });

        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args) {      //Method called when Update panel postback.
            OnLoad();
        }

        function OnLoad() {

            /* Home Page Related */
            CheckFilterChanged();
            CheckGridTotalRecords();

            /* Discharge Patients Details Page Related */
            DisableAddScheduleFollowupDataControls();
            $('.options').height($('.leftContent').height() + 25);
            /*adjust parent div height based on grid height-start here*/
            var biggestHeight = parseInt($('div.gridSection').height() + 200);
            if (biggestHeight > 680) {
                $('.content').css('min-height', biggestHeight + 'px');
            } else {
                $('.content').css('min-height', '680px');

            }

        }

        String.prototype.startsWith = function (str) {
            return (this.match("^" + str) == str);
        };

        function StopPropagation(e) {
            //cancel bubbling
            e.cancelBubble = true;
            if (e.stopPropagation) {
                e.stopPropagation();
            }
        }


        // This client side method is used to check the Total Number of Followup Grid Records
        function CheckGridTotalRecords() {
            if ($get("<%= hdnGridTotalRecords.ClientID %>") != null) {
                var hidGridTotalRecords = $get("<%= hdnGridTotalRecords.ClientID %>").value;
                var hdnMaxRecordsFlagHideInDetailsPage = $get("<%= hdnMaxRecordsFlagHideInDetailsPage.ClientID %>").value;
                var hidMaxRecordsWarning = $get("<%= hdnMaxRecordsMessage.ClientID %>").value;

                if ((hidGridTotalRecords == "True") && (hdnMaxRecordsFlagHideInDetailsPage == "True")) {
                    if ($('#grid-maxrecords').is(':hidden')) {
                        $('#lblMaxRecords').text(hidMaxRecordsWarning);
                        $('#grid-maxrecords').slideDown('fast');
                    }
                }
                else {
                    if ($('#grid-maxrecords').is(':visible')) {
                        $('#grid-maxrecords').slideUp('fast');
                    }
                }
            }
        }

        //This Method Collapse the Blue Alert Message on Home page
        function CollapseBlueAlertMessage() {
            if ($('#grid-maxrecords').is(':visible')) {
                $('#lblMaxRecords').text("");
                $('#grid-maxrecords').slideUp('fast');
            }
        }

        function CollapseAlertMessage() {
            if ($('#filter-update').is(':visible')) {
                $('#lblFilterChange').text("");
                $('#filter-update').slideUp('fast');
            }
        }

        // This Method is used when Check box inside the Combo box changes i.e for Units and Column Combo
        function onCheckBoxClickLocal(chk, filterSelected) {
            //hide user control content
            $('#divDischargeDetails').css({ 'display': 'none' });
            if (filterSelected == 'Unit') {
                unitComboChanged = true;
            }

            else if (filterSelected == 'Column') {
                selectColumnsComboChanged = true;
            }

            var combo = GetCombo(filterSelected, false);

            //prevent second combo from closing
            cancelDropDownClosing = true;
            //holds the number of programs selected
            var itemCount = 0;
            //holds the text of all checked items
            var text = "";
            //holds the values of all checked items
            var values = "";
            //get the collection of all items
            var items = combo.get_items();

            var parentChk = $get(combo.get_id() + "_Header_chkAll");
            //enumerate all items
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_childCheckbox");
                if (chk1.checked) {
                    text += item.get_text() + ",";
                    values += item.get_value() + ",";
                    itemCount += 1; // This will help maintain the selected items count
                }
            }


            if (itemCount == 0) {
                text = "Select " + filterSelected;

                if (filterSelected == 'Unit') {
                    parentChk.checked = false;
                }
                if (filterSelected == 'Column') {
                    text = "All " + filterSelected + "s";
                    chk.checked = true;
                    if ($('#grid-atleastcolumn').is(':hidden')) {
                        $('#lblAtleastOneColumn').text("You must select at least one column to display.");
                        $('#grid-atleastcolumn').slideDown('fast');
                        $('div[id$=_physicianComparisonMeasures_DropDown]').css('top', '60px');
                    }
                    return false;

                }
                combo.set_text(text);


            }
            else if (itemCount > 0) {
                if ($('#grid-atleastcolumn').is(':visible')) {
                    $('#grid-atleastcolumn').slideUp('fast');
                    $('div[id$=_physicianComparisonMeasures_DropDown]').css('top', '0px');
                }
                if (itemCount == items.get_count()) {
                    text = "All " + filterSelected + "s";
                    if (parentChk != null)
                        parentChk.checked = true;
                    GetCombo(filterSelected, true);
                }
                else if (itemCount == 1) {
                    text = RemoveLastComma(text);
                    if (parentChk != null)
                        parentChk.checked = false;
                    GetCombo(filterSelected, false);
                }
                else if (itemCount > 1 && itemCount != items.get_count()) {
                    text = itemCount + " " + filterSelected + "s";
                    if (parentChk != null)
                        parentChk.checked = false;
                    GetCombo(filterSelected, false);
                }
                combo.set_text(text);
            }
            else {
                combo.set_text("");
            }
        }

        //Capture the combo details

        function GetCombo(filterName, selectAllFlag) {
            var comboBoxControl = null;
            switch (filterName) {
                case 'Hospital':
                    comboBoxControl = $find("<%= drpDischargeLoc.ClientID %>");
                    $get("<%= hdnIsHospitalChange.ClientID %>").value = selectAllFlag;
                    break;
                case 'Unit':
                    comboBoxControl = $find("<%= drpDischargeUnits.ClientID %>");
                    $get("<%= hdnIsUnitsSelectAll.ClientID %>").value = selectAllFlag;
                    break;

                case 'Column':
                    comboBoxControl = $find("<%= radCmbDischargePatientGridColumns.ClientID %>");
                    $get("<%= hdnIsGridColumnsSelectAll.ClientID %>").value = selectAllFlag;
                    break;

            }
            return comboBoxControl;
        }

        //Select All code
        function onchkStatusAllClick(parentChk, filterSelected) {

            if (filterSelected == 'Unit') {
                unitComboChanged = true;
            }

            else if (filterSelected == 'Column') {
                selectColumnsComboChanged = true;
            }

            var combo = GetCombo(filterSelected);
            var items = combo.get_items();
            var text = "";
            var values = "";
            //enumerate all items
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combo.get_id() + "_i" + i + "_childCheckbox");

                if (parentChk.checked) {
                    chk1.checked = true;
                    text += item.get_text() + ",";
                    values += item.get_value() + ",";
                }
                else {
                    chk1.checked = false;
                }
            }

            text = RemoveLastComma(text);
            values = RemoveLastComma(values);
            //Message to be displayed as All Selected when all the checkboxes are selected
            if (parentChk.checked) {
                combo.set_text("All " + filterSelected + "s");
            }
            else {
                combo.set_text("Select " + filterSelected);

            }
        }

        // Check On Update Result Button Click
        function onUpdateResultsClick() {
            var hospitalText = $find("<%= drpDischargeLoc.ClientID %>").get_text();
            var unitsText = $find("<%= drpDischargeUnits.ClientID %>").get_text();

            if (hospitalText.startsWith("Select") || unitsText.startsWith("Select")) {
                ShowMessage("You must select one Hospital and at least one Unit, before you can view your results.\r\n\r\nPlease review your criteria and make the appropriate changes.");
                return false;
            }
            UpdateDaisyText('Processing');
            return true;
        }





        function ChangeDispalyMessage() {
            $("#lblFilterChange").html("You must select one Hospital and at least one Unit, before you can view your results.\r\n\r\nPlease review your criteria and make the appropriate changes.");
        }

        //For Showing the Warning Message
        function ShowMessage(message) {
            if ($('#filter-update').is(':hidden')) {
                $('#filter-update').slideDown('fast');
                $("#lblFilterChange").html(message);
            }
            else {
                $("#lblFilterChange").html(message);
            }
            $('#divDischargeDetails').css({ 'display': 'none' });

        }

        //For Hiding the Warning Message
        function HideMessage() {
            if ($('#filter-update').is(':visible')) {
                $('#filter-update').slideUp('fast');
            }
            if ($('#grid-maxrecords').is(':visible')) {
                $('#grid-maxrecords').hide();
            }
            $('#filter-update').css('background', '#ffffcc !important');
            $("#lblFilterChange").html("The filter selection has changed. After making your selections, click <strong>Update Results</strong>to view new results.");
        }

        //Method Checks Filter Changes
        function CheckFilterChanged() {
            var hidFiltersChangedID = $get("<%=hdnFiltersChanged.ClientID %>");
            var hdnFiltersChangeFlagHideInDetailsPage = $get("<%= hdnFiltersChangeFlagHideInDetailsPage.ClientID %>");

            if ((hidFiltersChangedID.value == "True") && (hdnFiltersChangeFlagHideInDetailsPage.value == "True")) {
                if ($('#filter-update').is(':hidden')) {
                    $('#filter-update').slideDown('fast');
                    $('#divPrint').removeAttr("onclick");
                    $('#divPrint').addClass('buttonH');
                    $('#divPrint').removeClass('buttonHL-arrow');

                    $('#divPrint').css({ "cursor": "default" });
                    $('div.gridSection').css({ 'top': '190px' });


                }
            } else {
                if ($('#filter-update').is(':visible')) {
                    $('#filter-update').slideUp('fast');
                }
            }
        }

        //Common Method for Drop Down opening
        function OnClientDropDownOpening(combobox) {
            HideMessage();
        }

        //Method for Handling Select Columns Combo Closing Event
        function OnGridColumnsClosingHandler(combobox, args) {
            var items = combobox.get_items();
            var text = "";
            var values = "";
            var count = 0;
            //enumerate all items
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combobox.get_id() + "_i" + i + "_childCheckbox");

                if (chk1.checked) {
                    count++;
                    text += item.get_text() + ",";
                    values += item.get_value() + ",";
                }
            }

            text = RemoveLastComma(text);
            values = RemoveLastComma(values);
            var messageText = "";
            if (count == 0) {
                messageText = "Select Columns";
            }
            else if (count == 1) {
                messageText = text;
            }
            else if (count > 1) {
                if (count == items.get_count()) {
                    //Message to be displayed as All Selected when all the checkboxes are selected
                    messageText = "All Columns";
                }
                else {
                    messageText = count + " Columns";
                }
            }
            combobox.set_text(messageText);

            //If the group combo was not updated do not postback
            if (!selectColumnsComboChanged) {
                return;
            }
            $get("<%= btnSelectColumnsOK.ClientID %>").click();
            selectColumnsComboChanged = false;

            if ($('#grid-atleastcolumn').is(':visible')) {
                $('#grid-atleastcolumn').slideUp('fast');
            }

        }

        //Method for handling Hospital Combo Closing Event
        function drpHospitalsClientClosingHandler(sender, args) {
            if (args != null) {
                var item = args.get_item();
                var selectedItem;
                if (item != null) {
                    selectedItem = sender.get_selectedItem();
                    var previouslySelectedItemText = selectedItem != null ? selectedItem.get_text() : sender.get_text();

                    if (previouslySelectedItemText != item.get_text()) {
                        //Hide user control content
                        $('#divDischargeDetails').css({ 'display': 'none' });
                        return true;
                    }
                }
                else {
                    selectedItem = sender.get_selectedItem();
                    if (selectedItem != null)
                        selectedItem.select();
                    else {
                        sender.set_selectedIndex(-1);
                    }
                }
            }
            return false;
        }


        // Method to handle Units Combo Closing event
        function drpDischargeUnitsClientClosingHandler(combobox, args) {
            var items = combobox.get_items();

            var text = "";
            var values = "";
            var count = 0;
            //enumerate all items
            for (var i = 0; i < items.get_count(); i++) {
                var item = items.getItem(i);
                //get the checkbox element of the current item
                var chk1 = $get(combobox.get_id() + "_i" + i + "_childCheckbox");

                if (chk1.checked) {
                    count++;
                    text += item.get_text() + ",";
                    values += item.get_value() + ",";
                }
            }

            text = RemoveLastComma(text);
            values = RemoveLastComma(values);
            var messageText = "";
            if (count == 0) {
                messageText = "Select Units";
            }
            else if (count == 1) {
                messageText = text;
                items.getItem(0).checked;
            }
            else if (count > 1) {
                if (count == items.get_count()) {
                    //Message to be displayed as All Selected when all the checkboxes are selected
                    messageText = "All Units";
                }
                else {
                    messageText = count + " Units";
                }
            }
            combobox.set_text(messageText);
            if (!unitComboChanged) {
                return;      //If the group combo was not updated do not postback
            }
            $get("<%= btnUpdateSelectedUnits.ClientID %>").click();
            unitComboChanged = false;
        }

        function HnadlePostbackStyle() {
            $('#Panel1').css('z-index', '-1');
        }


        function HideParentGridDetails() {
            $('.leftSidebar').css({ "display": "none" });
            $('.content').addClass('marginForChildDetails');
            $('#top-filter').css({ "display": "none" });
            $('.topButtonRow').css({ "display": "none" });
            $('.columnSelectionGrid').css({ "display": "none" });
            $('.gridSection').css({ "display": "none" });
            $('#divDischargeDetails').removeClass('displayNone');
            $('#filter-update').hide();
        }


        //For Showing Home Grid
        function ShowParentGridDetails() {
            $('.leftSidebar').css({ "display": "block" });
            $('.content').removeClass('marginForChildDetails');
            $('#top-filter').css({ "display": "block" });
            $('.topButtonRow').css({ "display": "block" });
            $('.columnSelectionGrid').css({ "display": "block" });
            $('.gridSection').css({ "display": "block" });
            $('#divDischargeDetails').addClass('displayNone');
        }

        var column = null;
        // Menu Options for Filter Controls
        function MenuShowing(sender, args) {
            if (column == null) return;
            var menu = sender; var items = menu.get_items();
            var item;
            if (column.get_dataType() == "System.String") {
                var i = 0;
                while (i < items.get_count()) {
                    if (!(items.getItem(i).get_value() in { 'NoFilter': '', 'Contains': '', 'DoesNotContain': '', 'StartsWith': '' })) {
                        item = items.getItem(i);
                        if (item != null)
                            item.set_visible(false);
                    }
                    else {
                        item = items.getItem(i);
                        if (item != null)
                            item.set_visible(true);
                    } i++;
                }
            }

            if (column.get_dataType() == "System.DateTime") {
                var j = 0; while (j < items.get_count()) {

                    if (!(items.getItem(j).get_value() in { 'NoFilter': '', 'EqualTo': '', 'NotEqualTo': '', 'GreaterThan': '', 'LessThan': '', 'GreaterThanOrEqualTo': '', 'LessThanOrEqualTo': '' })) {
                        item = items.getItem(j);
                        if (item != null)
                            item.set_visible(false);
                    }
                    else {
                        item = items.getItem(j);
                        if (item != null) item.set_visible(true);
                    } j++;
                }
            }

            column = null;
        }

        //For Grid Filter Menu 
        function filterMenuShowing(sender, eventArgs) {
            column = eventArgs.get_column();
        }



        //Toggling for Export Button for ExportAll and Current options
        function TogglePopup(e) {
            e = e || window.event;
            e.cancelBubble = true;
            if (e.stopPropagation) e.stopPropagation();
            if ($('#divPrint').hasClass('buttonHL-arrow-hover')) {
                $('#<%=pnlPrint.ClientID%>').hide();
                $('#divPrint').removeClass('buttonHL-arrow-hover');
                $('#divPrint').addClass('buttonHL-arrow');
            } else {
                $('#<%=pnlPrint.ClientID%>').show();
                if ($('#divPrint').hasClass('buttonHL-arrow')) {
                    $('#divPrint').removeClass('buttonHL-arrow');
                    $('#divPrint').addClass('buttonHL-arrow-hover');
                }
            }
        }

        function CallUpdateDaisyText() {
            UpdateDaisyText('Processing');
            if ($('#divPrint').hasClass('buttonHL-arrow-hover')) {
                $('#<%=pnlPrint.ClientID%>').hide();
                $('#divPrint').removeClass('buttonHL-arrow-hover');
                $('#divPrint').addClass('buttonHL-arrow');
            }
        }

        // Collapse Export Button
        function CollapseExportDropdown() {
            if ($('#divPrint').hasClass('buttonHL-arrow-hover')) {
                $('#<%=pnlPrint.ClientID%>').hide();
                $('#divPrint').removeClass('buttonHL-arrow-hover');
                $('#divPrint').addClass('buttonHL-arrow');
            }
        }



        /* Discharge Patients Details Page Related Methods */


        //Method to hide Add Note Section
        function HideAddNotes() {
            $(".addNotes").hide();
            $('.options').height($('.leftContent').height() + 25);
        }

        //Method to Show Add Note Section
        function ShowAddNotes() {
            $(".addNotes").show();
            $('.options').height($('.leftContent').height() + 25);
        }

        function OnClientBlurHandler(sender, eventArgs) {
            var item = sender.findItemByText(sender.get_text());
            if (!item) {
                sender.clearSelection();
                var drpSelectValue = $('#ctl00_SplitMainContentBody_ucDischargePatientsDetails_drpActionLoc_Input').val('Select Action');

            }
        }

        function OnClientItemsRequestedHandler(sender, eventArgs) {
            //set the max allowed height of the combo


            if (($.browser.msie) && ($.browser.version == '9.0')) {
                var SINGLE_ITEM_HEIGHT = 20;
                var MAX_ALLOWED_HEIGHT = 200;
            } else {
                var SINGLE_ITEM_HEIGHT = 19;
                var MAX_ALLOWED_HEIGHT = 190;
            }
            //this is the single item's height  
            var calculatedHeight = sender.get_items().get_count() * SINGLE_ITEM_HEIGHT;


            var dropDownDiv = sender.get_dropDownElement();

            if (calculatedHeight > MAX_ALLOWED_HEIGHT) {
                setTimeout(
            function () {
                dropDownDiv.firstChild.style.height = MAX_ALLOWED_HEIGHT + "px";
            }, 20
        );
            }
            else {
                setTimeout(
            function () {
                dropDownDiv.firstChild.style.height = calculatedHeight + "px";
            }, 20
        );
            }
        }


        //Field Validation method
        function fieldValidation() {
            var txtNote = $('.txtNoteVal');
            var lblMessageErrorMessageVal = $('.lblMessageErrorMessageVal');

            var drpAction = $("#ctl00_SplitMainContentBody_ucDischargePatientsDetails_drpActionLoc_Input").val();

            //  $('#ctl00_ctl00_ctl00_Content_ContentMain_SplitMainContentBody_ucDischargePatientsDetails_drpActionLoc_Input option[value=0]').attr('selected', 'selected');
            var errorMsg = $('#SplitMainContentBody_ucDischargePatientsDetails_actionErrMsg');

            // alert($('#drpActionLoc').val());
            var filterAction = $("#lblFilterChange1");
            //            if ($.trim(txtNote.val()) == "") {
            //                lblMessageErrorMessageVal.text("* Notes field can't be empty. ");
            //                return false;
            // }
            //  else {
            var charLength = $(".txtNoteVal").val().length;
            if (charLength > 4000) {
                lblMessageErrorMessageVal.text("* Notes should not exceed 4000 characters.");
                return false;
            }
            else {
                var noteText = $(".txtNoteVal").val();
                var replaceLessThan = txtNote.val(noteText.replace(RegExp("<", "g"), '&lt;'));
                var replacedNoteText = replaceLessThan.val();
                var replaceGreaterThan = replacedNoteText.replace(/>/g, '&gt;');
            }

            if (drpAction == 'Select Action') {
                errorMsg.show();
                $('.RadComboBox').addClass("drpRequiredStyle");
                errorMsg.html("The following fields are required: <strong>Action</strong>");
                return false;
            }

            else
                errorMsg.hide();
            $('.RadComboBox').removeClass("drpRequiredStyle");
            return true;
            //alert(drpAction);

            window.scrollTo(0, 0);
            UpdateDaisyText('Processing...');
            return true;
        }

        //navigation to next patient details
        $('.navToNextParentDetails').live('click', function () {
            var txtNote = $('.txtNoteVal');

            if ($.trim(txtNote.val()) != "") {
                if ($('#divMustSaveNote').is(':hidden')) {
                    $('#spnMustSaveNote').text("You have not saved your changes. You must save or cancel your changes before you can view another patient's discharge details.");
                    $('#divMustSaveNote').slideDown('fast');
                }
            }
            else {
                $get("<%= btnNextPatientDetails.ClientID %>").click();
            }
        });

        //navigation to prev patient details
        $('.navToPrevParentDetails').live('click', function () {
            var txtNote = $('.txtNoteVal');

            if ($.trim(txtNote.val()) != "") {
                if ($('#divMustSaveNote').is(':hidden')) {
                    $('#spnMustSaveNote').text("You have not saved your changes. You must save or cancel your changes before you can view another patient's discharge details.");
                    $('#divMustSaveNote').slideDown('fast');
                }
            }
            else {

                $get("<%= btnPrevPatientDetails.ClientID %>").click();
            }
        });

        // validate schedule followup 
        function AddFollowupValidation() {
            var lblMessageErrorMessageVal = $('.addfollowupErrorMessage');
            lblMessageErrorMessageVal.text("");
            if ($('.addFolloupFromToDay input[type="radio"]').attr('checked') == false && $('.addFollowupSpecificDate input[type="radio"]').attr('checked') == false) {
                lblMessageErrorMessageVal.text("You must select one of the options: 'From Today' or 'Specific Date'.");
                return false;
            }
            if ($('.addFolloupFromToDay input[type="radio"]').attr('checked') == true) {
                var selectedVal = $('.addFolloupFromToDay').val();
                if (selectedVal == "Select Option") {
                    lblMessageErrorMessageVal.text("You must input a Follow-up Due Date and a Reason.");
                    return false;
                }
            }
            if ($('.addFollowupSpecificDate input[type="radio"]').attr('checked') == true) {
                var datetext = $('.addFollowupDatePicker').find('.riTextBox');
                if ($.trim(datetext.val()) == "") {
                    lblMessageErrorMessageVal.text("You must input a Follow-up Due Date and a Reason.");
                    return false;
                }
            }
            var CurrDate = new Date().format("MM/dd/yyyy");
            if ($('.addFollowupSpecificDate input[type="radio"]').attr('checked') == true) {
                var datetext = $('.addFollowupDatePicker').find('.riTextBox');
                if (Date.parse(datetext.val()) < Date.parse(CurrDate)) {
                    lblMessageErrorMessageVal.text("A follow-up cannot be scheduled for a date in the past. Please input a follow-up due date that is today or in the future.");
                    return false;
                }
            }
            var reason = $("[id$=_ucRadCmbReasons_cmbBoxCheckable]");
            if (reason.val() == "Select Reason") {
                lblMessageErrorMessageVal.text("You must input a Follow-up Due Date and a Reason.");
                return false;
            }
            if ($('.addFolloupFromToDay input[type="radio"]').attr('checked') == true) {
                var lblFutureDate = $('.addFollowupDisplayDetailsDays').text();
                var followUpDates = $(".followUpDatesToValidate").val();
                if (followUpDates.length > 0) {
                    var splitResult = followUpDates.split(",");
                    for (i = 0; i < splitResult.length; i++) {
                        if (Date.parse(lblFutureDate) == Date.parse(splitResult[i])) {
                            lblMessageErrorMessageVal.text("A follow-up has already been scheduled for this date. Please input a different date.");
                            return false;
                        }
                    }
                }
            }
            if ($('.addFollowupSpecificDate input[type="radio"]').attr('checked') == true) {
                var followUpDates = $(".followUpDatesToValidate").val();
                if (followUpDates.length > 0) {
                    var splitResult = followUpDates.split(",");
                    var datetext = $('.addFollowupDatePicker').find('.riTextBox');
                    for (i = 0; i < splitResult.length; i++) {
                        if (Date.parse(datetext.val()) == Date.parse(splitResult[i])) {
                            lblMessageErrorMessageVal.text("A follow-up has already been scheduled for this date. Please input a different date.");
                            return false;
                        }
                    }
                }
            }
            var txtNote = $('.addFollowupScheduledNote');
            if ($.trim(txtNote.val()) == "") {
                return true;
            }
            else {
                var charLength = $(".addFollowupScheduledNote").val().length;
                if (charLength > 4000) {
                    lblMessageErrorMessageVal.text("* Notes should not exceed 4000 characters.");
                    return false;
                }
                else {
                    var noteText = $(".addFollowupScheduledNote").val();
                    var replaceLessThan = txtNote.val(noteText.replace(RegExp("<", "g"), '&lt;'));
                    var replacedNoteText = replaceLessThan.val();
                    var replaceGreaterThan = replacedNoteText.replace(/>/g, '&gt;');
                }
            }
            return true;
        }

        // clear fields in add schedule
        function ClearNewSchedule() {
            var lblMessageErrorMessageVal = $('.addfollowupErrorMessage');
            lblMessageErrorMessageVal.text("");
            var txtNote = $('.addFollowupScheduledNote');
            txtNote.text("");
            $('.addFollowupDatePicker').find('.riTextBox').val("");
            $('.addFolloupFromToDay input[type="radio"]').attr('checked', false)
            $('.addFollowupSpecificDate input[type="radio"]').attr('checked', false)
            var calculatedDate = $('.addFollowupDisplayDetailsDays');
            calculatedDate.text("");
            var ddlFromToDay = $("[id$=ddlFromToDay_DropDown]");
            ddlFromToDay.find('li').each(function () {
                if ($(this).hasClass('rcbHovered')) {
                    $(this).removeClass('rcbHovered').addClass('rcbItem');
                }
                if ($(this).text() == 'Select Option') {
                    $(this).addClass('rcbHovered').removeClass('rcbItem');
                }
            });

            $("[id$=ddlFromToDay]").find("input [type=text]").val('Select Option');

            return false;
        }

        // Show popup
        function PopUpShowDivToggle(divPopUp) {

            popup(divPopUp);
            return false;
        }

        // hide popup
        function PopUpDivToggle() {

            ClearNewSchedule();
            var el = document.getElementById('divPopUp');
            if (el.style.display == 'none') { el.style.display = 'block'; }
            else { el.style.display = 'none'; }
            return true;
        }


        // validations for Edit followup
        function EditFollowupValidation() {
            var lblEditMessageErrorMessageVal = $('.editFollowupErrorMessage');
            lblEditMessageErrorMessageVal.text("");
            var datetext = $('.editDatePicker').find('.riTextBox');
            if ($.trim(datetext.val()) == "") {
                lblEditMessageErrorMessageVal.text("You must input a Follow-up Due Date and a Reason.");
                return false;
            }
            var currentDateFromServer = $(".currentDateToValidate").val();
            var CurrDate = new Date().format("MM/dd/yyyy");
            var datetext = $('.editDatePicker').find('.riTextBox');
            if (Date.parse(datetext.val()) < Date.parse(currentDateFromServer)) {
                lblEditMessageErrorMessageVal.text("A follow-up cannot be scheduled for a date in the past. Please input a follow-up due date that is today or in the future.");
                return false;
            }
            var followUpDates = $(".followUpDatesToValidate").val();
            var currentFollowUpDate = $(".followUpDateForEditValidate").val();
            if (Date.parse(datetext.val()) != Date.parse(currentFollowUpDate)) {
                if (followUpDates.length > 0) {
                    var splitResult = followUpDates.split(",");
                    var datetext = $('.editDatePicker').find('.riTextBox');
                    for (i = 0; i < splitResult.length; i++) {
                        if (Date.parse(datetext.val()) == Date.parse(splitResult[i])) {
                            lblEditMessageErrorMessageVal.text("A follow-up has already been scheduled for this date. Please input a different date.");
                            return false;
                        }
                    }
                }
            }
            var reason = $("[id$=_ucRadCmbEditReasons_cmbBoxCheckable]");
            if (reason.val() == "Select Reason") {
                lblEditMessageErrorMessageVal.text("You must input a Follow-up Due Date and a Reason.");
                return false;
            }
            var txtNote = $('.editFollowupScheduledNote');
            if ($.trim(txtNote.val()) == "") {
                return true;
            }
            else {
                var charLength = $(".editFollowupScheduledNote").val().length;
                if (charLength > 4000) {
                    lblEditMessageErrorMessageVal.text("* Notes should not exceed 4000 characters.");
                    return false;
                }
                else {
                    var noteText = $(".editFollowupScheduledNote").val();
                    var replaceLessThan = txtNote.val(noteText.replace(RegExp("<", "g"), '&lt;'));
                    var replacedNoteText = replaceLessThan.val();
                    var replaceGreaterThan = replacedNoteText.replace(/>/g, '&gt;');
                }
            }
            return true;
        }

        // validations for Cancel followup
        function CancelFollowupValidation() {
            var lblCancelMessageErrorMessageVal = $('.cancelFollowupErrorMessage');
            lblCancelMessageErrorMessageVal.text("");
            var txtReasonForCancelling = $('.reasonForCancelling');
            if ($.trim(txtReasonForCancelling.val()) == "") {
                lblCancelMessageErrorMessageVal.text("You must input a Reason for Cancelling.");
                return false;
            }
            var charReasonForCancellingLength = $(".reasonForCancelling").val().length;
            if (charReasonForCancellingLength > 4000) {
                lblCancelMessageErrorMessageVal.text("* Reason for Cancelling should not exceed 4000 characters.");
                return false;
            }
            else {
                var charReasonForCancelling = $(".reasonForCancelling");
                var noteText = $(".reasonForCancelling").val();
                var replaceLessThan = charReasonForCancelling.val(noteText.replace(RegExp("<", "g"), '&lt;'));
                var replacedNoteText = replaceLessThan.val();
                var replaceGreaterThan = replacedNoteText.replace(/>/g, '&gt;');
            }
            return true;
        }



        // for edit schedule popup to hide
        function PopUpDivToggleForEdit() {
            var el = document.getElementById('divEditFollowup');
            if (el.style.display == 'none') { el.style.display = 'block'; }
            else { el.style.display = 'none'; }

            var e2 = document.getElementById('divBlanket');
            if (e2.style.display == 'none') { e2.style.display = 'block'; }
            else { e2.style.display = 'none'; }

            return false;
        }

        // for Cancel schedule popup to hide
        function CancelPopUpDivToggle() {
            var txtReasonForCancelling = $('.reasonForCancelling');
            txtReasonForCancelling.text("");
            var el = document.getElementById('divCancelFollowup');
            if (el.style.display == 'none') { el.style.display = 'block'; }
            else { el.style.display = 'none'; }

            var e2 = document.getElementById('divBlanket');
            if (e2.style.display == 'none') { e2.style.display = 'block'; }
            else { e2.style.display = 'none'; }
            return false;
        }

        //Method for Followup Date from Today
        function EnableOrDisableScheduleFollowupDatesFromToday() {
            $('.addFolloupFromToDay input[type="radio"]').attr('checked', true)
            $('.addFollowupSpecificDate input[type="radio"]').attr('checked', false)
            return false;
        }

        function EnableOrDisableScheduleFollowupDatesSpecificDate() {
            $('.addFolloupFromToDay input[type="radio"]').attr('checked', false)
            $('.addFollowupSpecificDate input[type="radio"]').attr('checked', true)
            return false;
        }

        //This method is used to Disable the Add Followup Model Dialog Controls
        function DisableAddScheduleFollowupDataControls() {
            $('#rdpDatePickerDivLayer').css('display', 'block');
            $('#ddlFromToDayDivLayer').css('display', 'block');
            $("[id$=_rdpDatePicker_popupButton]").css('display', 'none');
            $("[id$=_ucDischargePatientsDetails_ddlFromToDay_Arrow]").css('display', 'none');
        }

        //This Method set the Number of Days on Selection change of From To Day drop down list of Add Followup Model Dialog
        function ddlFromToDayC_OnClientSelectedIndexChanged(sender, args) {
            if (args != null) {
                var item = args.get_item();
                var selectedVal = item.get_text();
                var days = 0;
                if (selectedVal == "2 Days") {
                    days = 2;
                }
                else if (selectedVal == "1 Week") {
                    days = 7;
                }
                else if (selectedVal == "2 Weeks") {
                    days = 14;
                }
                else if (selectedVal == "1 Month") {
                    days = 30;
                }

                var addDays = $('.addFollowupDisplayDetailsDays');
                var today = new Date();
                var tomorrow = new Date();
                tomorrow.setDate(today.getDate() + parseInt(days));
                var dd = tomorrow.getDate();
                var mm = tomorrow.getMonth() + 1; //January is 0!
                var yyyy = tomorrow.getFullYear();
                if (dd < 10) { dd = '0' + dd }
                if (mm < 10) { mm = '0' + mm }
                var tomorrow = mm + '/' + dd + '/' + yyyy;
                addDays.text(tomorrow);
            }
        }
