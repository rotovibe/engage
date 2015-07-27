using System;

namespace Phytel.Services.Dates
{
    public class DateTimeLocalProxy : DateTimeProxy, IDateTimeProxy
    {
        public DateTimeLocalProxy(string dateFormat = DateFormat, string dateTimeFormatString = DateTimeFormatString, string timeFormat = TimeFormat)
            : base(dateFormat, dateTimeFormatString, timeFormat)
        {
        }

        public override DateTime GetCurrentDate()
        {
            return DateTime.Today;
        }

        public override DateTime GetCurrentDateTime()
        {
            return DateTime.Now;
        }
    }
}