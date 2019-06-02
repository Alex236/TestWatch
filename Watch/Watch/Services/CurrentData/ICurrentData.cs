using System.Collections.Generic;
using Watch.Models;


namespace Watch.Services.CurrentData
{
    public interface ICurrentData
    {
        ClockProfile CurrentProfile { get; set; }
        List<ClockProfile> Profiles { get; set; }
        event Update OnUpdate;
        void ExecuteUpdate();
    }
}
