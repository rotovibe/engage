using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phytel.Services.Dates
{
    public abstract class DateTimeProxy : IDateTimeProxy
    {
        public const string DateFormat = "MMddyyyy";
        public const string DateTimeFormatString = "{0} {1}";
        public const string TimeFormat = "hhmm tt";

        protected readonly string _dateFormat;
        protected readonly string _dateTimeFormatString;
        protected readonly string _timeFormat;

        public DateTimeProxy(string dateFormat = DateFormat, string dateTimeFormatString = DateTimeFormatString, string timeFormat = TimeFormat)
        {
            _dateFormat = dateFormat;
            _dateTimeFormatString = dateTimeFormatString;
            _timeFormat = timeFormat;
        }

        public abstract DateTime GetCurrentDate();

        public abstract DateTime GetCurrentDateTime();

        public virtual DateTime GetCurrentDateTimeAdd(double minutes)
        {
            return GetCurrentDateTime().AddMinutes(minutes);
        }

        public virtual TimeSpan GetTimeSpanFromMinutes(double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        public virtual string GetTimestamp()
        {
            string time = GetTimestampTime();
            string date = GetTimestampDate();

            return string.Format(_dateTimeFormatString, date, time);
        }

        public virtual string GetTimestampDate()
        {
            DateTime now = GetCurrentDateTime();

            return now.ToString(_dateFormat);
        }

        public virtual string GetTimestampTime()
        {
            DateTime now = GetCurrentDateTime();

            return now.ToString(_timeFormat);
        }
    }
}
