$(document).ready(function () {

  init(document);
});

function init(context) {

    var hover = false;
}
/* TJD Commented this out, Not needed right now

  // Buttons
  $('.button', context).button();
  $('.button.more', context).button({
    icons: { secondary: 'ui-icon-down' }
  });
  $('.button.next', context).button({
    icons: { secondary: 'ui-icon-right' }
  });
  $('.button.prev', context).button({
    icons: { secondary: 'ui-icon-left' }
  });

  // Send Notifications
  $('#send-notifications', context).click(function () {
    $(this).toggleClass('active');
    $('#notifications').animate({height: 'toggle'}, {
      step: function (value) {
        $('#primary-content .headon').css('paddingTop', $('#primary-content .header').height());
      },
      complete: function (value) {
        $('#primary-content .headon').css('paddingTop', $('#primary-content .header').height());
      }
    });
  });

  // Notifications form
  $('#notifications .cancel', context).click(function () {
    $('#send-notifications').click();
    $('.appointment').fadeIn();
  });
  $('#notifications', context).submit(function () {
    alert('messages sent');
    $('#send-notifications').click();
    $('.appointment').fadeIn();

    return false;
  });

  $('input[name=notification]', context).click(function () {
    switch ($(this).val()) {
    case 'resend':
      $('.appointment').each(function () {
        $(this).is('.unconfirmed') ? $(this).show() : $(this).hide();
      });
      break;
    case 'office-closed':
      $('.appointment').each(function () {
        $(this).is('.cancelled, .rescheduled') ? $(this).show() : $(this).hide();
      });
      break;
    }
  });
  
  // Callback for filtering table results
  $('.filter', context).bind('change multiselectclick', function () {
    setTimeout(function () {
      $('.tbody').load('callback.php', $('.filter').serialize(), function () {
        init(this);
      });
    }, 0);
  });
  
  // Expand row
  $('.tr .update', context).click(function () {
    $(this).closest('.tr').addClass('active').removeClass('inactive').siblings('.tr').addClass('inactive').removeClass('active');
    return false;
  });

  $('.tr', context)
    .bind('clickoutside', function () {
      if ($(this).is('.active')) {
        $(this).closest('.tr').removeClass('active').siblings('.tr').removeClass('inactive');
      }
    })
    .find('form').submit(function () {
      $(this).closest('.tr').removeClass('active').siblings('.tr').removeClass('inactive');
      return false;
    })
    .find('.cancel').click(function () {
      $(this).closest('.tr').removeClass('active').siblings('.tr').removeClass('inactive');
      return false;
    });
  
  $('.thead input[type=checkbox]').click(function () {
    $(this).attr('checked') ? $('.tbody input[type=checkbox]').attr('checked', 'checked') : $('.tbody input[type=checkbox]').removeAttr('checked');
  });
  
  // Show Search Results dialog
  $('[name=search]', context).bind('search', function () {
    if ($(this).val()) {
      $('#search-results').dialog({
        draggable: false,
        modal: true,
        width: 790,
        resizable: false,
        title: 'Search Results',
      });
      $(window).resize();
    }
  });
  
  // Show Patient Summary dialog
  $('.patient', context).click(function () {
    $('#patient-summary').dialog({
      draggable: false,
      modal: true,
      width: 790,
      resizable: false,
      title: 'Patient Summary',
    });
    $('#patient-summary-tabs').tabs();
    $(window).resize();
    return false;
  });
  
  // Show Patient Opt Out Settings dialog
  $('.opt-out', context).click(function () {
    $('#patient-opt-out').dialog({
      draggable: false,
      modal: true,
      width: 730,
      resizable: false,
      title: 'Opt-Out Settings'
    });
    $('#patient-opt-out .date').datepicker();
    $(window).resize();
    return false;
  });
  
  // fixed position scrolling 
  $('#primary-content', context).headon({header: '.header', buffer: $('.tbody')});

  $('.dialog-buttons .cancel', context).click(function () {
    $(this).closest('.dialog').dialog('close');
  });
  
}

*/

//$(window).scroll(function () {
//  scrollTop = $(window).scrollTop();
//  if (scrollTop > 160) {
//    $('#left-nav').css({top: scrollTop - 160});
//  }
//  else {
//    $('#left-nav').css({top: 0});
//  }
//});

//$(window).bind('resize', function () {
//  $('.ui-dialog').position({
//    my: 'center top',
//    at: 'center bottom',
//    of: $('.thead'),
//    offset: '0 20'
//  });
//});
