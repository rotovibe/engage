/*!	
	Licensed Materials - Property of IBM
	PID : 5725-Z49
	Copyright IBM Corp. 2013, 2016
	US Government Users Restricted Rights- Use, duplication or disclosure restricted by GSA ADP Schedule Contract with IBM Corp.
*/
function toggle(div_id) {

    if ($('#' + div_id).css('display') == 'none') { $('#' + div_id).css('display', 'block'); }
    else { $('#' + div_id).css('display', 'none'); }
}
function blanket_size(popUpDivVar) {
    // get the screen height and width  
    var maskHeight = $(window).height();
    var maskWidth = $(window).width();

    // calculate the values for center alignment
    var dialogTop = ((maskHeight - $('#' + popUpDivVar).height()) / 2);   //Middle from top
    var dialogLeft = ((maskWidth - $('#' + popUpDivVar).width()) / 2) - 100;  //Center from left

    $('#' + popUpDivVar).css({ top: 0, left: dialogLeft});  //applying the top and left positions
    $('#divBlanket').css({ left: 0, height: 750, top: 0 });  //applying the top and left positions

}
function window_pos(popUpDivVar) {

    // get the screen height and width  
    var maskHeight = $(window).height();
    var maskWidth = $(window).width(); 

    // calculate the values for center alignment
    var dialogTop = ((maskHeight - $('#' + popUpDivVar).height()) / 2);   //Middle from top
    var dialogLeft = ((maskWidth - $('#' + popUpDivVar).width()) / 2) - 100;  //Center from left

    $('#' + popUpDivVar).css({ top: 170, left: dialogLeft });  //applying the top and left positions
    

}

function popup(windowname) {
   blanket_size('divBlanket');
   window_pos(windowname);
    toggle('divBlanket');
    toggle(windowname);
}

function DisableDatePicker(ddlFromToDayDivLayer, rdpDatePickerDivLayer) {
    $('#' + rdpDatePickerDivLayer).css('display', 'block');
    $('#' + ddlFromToDayDivLayer).css('display', 'none');
    $("[id$=_rdpDatePicker_popupButton]").css('display', 'none');
    $("[id$=_ucDischargePatientsDetails_ddlFromToDay_Arrow]").css('display', 'block');   
    EnableOrDisableScheduleFollowupDatesFromToday();
}

function DisableFromToDayRadDropDown(ddlFromToDayDivLayer, rdpDatePickerDivLayer) {
    $('#' + ddlFromToDayDivLayer).css('display', 'block');
    $('#' + rdpDatePickerDivLayer).css('display', 'none');
    $("[id$=_rdpDatePicker_popupButton]").css('display', 'block');
    $("[id$=_ucDischargePatientsDetails_ddlFromToDay_Arrow]").css('display', 'none');
    EnableOrDisableScheduleFollowupDatesSpecificDate();
}

function ajustLeftAndRightDivs() {
    $('.options').height($('.leftContent').height() + 25);
}


function OnClientMouseOut(sender, args) {
    $('.options').height($('.leftContent').height() + 25);
}

function OnClientItemCollapse(sender, args) {
    $('.options').height($('.leftContent').height() + 25);
}
