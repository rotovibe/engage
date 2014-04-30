using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JangoEmailResultsService
{
    public static class NotifySender
    {
        public const int EmailPending = 181;
        public const int EmailSent = 182;
        public const int EmailBounced = 183;
        public const int Unsuccessful_InvalidEmailAddress = 184;
        public const int Unsuccessful_MissingEmailAddress = 185;
        public const int Unsuccessful_Other = 186;
        public const int IntroductoryEmail_ResponsePending = 187;
        public const int NoCommunication_Inactive = 188;
        public const int NoCommunication_Deceased = 189;
        public const int NoCommunication_BadDebt = 190;
        public const int NoCommunication_Delisted = 191;

        public const int EmailOpened = 172;
        public const int EmailClicked = 175;
        public const int EmailOptedOut = 177;
        public const int EmilComplaint = 178;
    }

    public static class ResultStatus
    {
        public const string Complete = "Complete";
    }
}