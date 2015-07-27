using System;

namespace Phytel.Services.Dates
{
    public class DateTimeUtcProxy : DateTimeProxy, IDateTimeProxy
    {
        public DateTimeUtcProxy(string dateFormat = DateFormat, string dateTimeFormatString = DateTimeFormatString, string timeFormat = TimeFormat)
            : base(dateFormat, dateTimeFormatString, timeFormat)
        {
        }

        public override DateTime GetCurrentDate()
        {
            return DateTime.UtcNow.Date;
        }

        public override DateTime GetCurrentDateTime()
        {
            return DateTime.UtcNow;
        }
    }
}