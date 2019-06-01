using System;


namespace Watch.Services.GetTime
{
    public interface IGetTime
    {
        DateTime? GetTimeByTimezone(string timezone);
    }
}
