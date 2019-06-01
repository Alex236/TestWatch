using System;


namespace Watch.Services.GetTime
{
    public class GetTime : IGetTime
    {
        public DateTime? GetTimeByTimezone(string timezone)
        {
            DateTime? currentTime;
            try
            {
                TimeZoneInfo checkedTimezone = TimeZoneInfo.FindSystemTimeZoneById(timezone);
                return currentTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, checkedTimezone);
            }
            catch
            {
                return null;
            }
        }
    }
}
