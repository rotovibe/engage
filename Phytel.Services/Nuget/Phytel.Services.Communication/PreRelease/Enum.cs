using System;
using System.Collections.Generic;
using System.Text;

namespace Phytel.API.DataDomain.Communication.DTO
{
    public enum ScheduleTypes
    {
        AppointmentReminders = 1,
        OutreachRecallSendQueue = 2,
        OutreachSendQueue = 3,
        NewFound = 4,
        AppointmentRemindersIntro = 5
    }

    public enum CommunicationTypes
    {
        NONE = 0,
        PHONE = 1,
        EMAIL = 2

    }

    public enum CommunicationScheduleTypes
    {
        NONE = 0,
        SendQueue = 1,
        Communication = 2

    }

    public enum UpdateFlags
    {
        Complete = 0,
        Inprocess = 1
    }

    public enum AlertFlags
    {
        NotRequired = 0,
        Required = 1
    }

    public enum Status
    {
        Complete = 0,
        Inprocess = 1
    }

    public enum TemplateTypes
    {
        ACDefault = 1,
        ACIntroDefault = 2,
    }

   

    public enum CampaignTypes
    {
        ACDefault = 1,
        OutreachDefault = 2,
    }

  
    public enum SendQueueInprocessStatus
    {
        Ready = 0,
        AssignToXMLBatchError = 5,
        AssignToXMLBatch = 10,
        BuildXMLError = 15,
        BuildXML = 20,
        AssignToNFBatchError = 25,
        AssignToNFBatch = 30,
        SentForCommError = 35,
        SentForComm = 40,
        Error = 45,
        Complete = 50,
        SendResend = 60,
        SendTest = 70
    }

    public enum ActivityNotifySender
    {
        EmailQueued	=170,
        EmailSent	=171,
        EmailOpened	=172,
        EarlierAppointmentUsed	=173,
        EmailConfirm	=174,
        EmailClicked	=175,
        EmailBounced	=176,
        EmailOptedOut	=177,
        EmailComplaint=	178,
        EmailDeliveryFailure	=179,

        NoNotification = 0,
        CancelledByUser = 4
    }

    public enum Prompts
    {
       AppointmentSpecificMessage = 1832
    }

    public enum EmailType
    {
        NONE = 0,
        TEXT = 1,
        HTML = 2
    }

}
