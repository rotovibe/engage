using System;

namespace Phytel.Services.Dates
{
    public interface IDateTimeProxy
    {
        DateTime GetCurrentDate();

        DateTime GetCurrentDateTime();

        DateTime GetCurrentDateTimeAdd(double minutes);

        TimeSpan GetTimeSpanFromMinutes(double minutes);

        string GetTimestamp();

        string GetTimestampDate();

        string GetTimestampTime();
    }
}