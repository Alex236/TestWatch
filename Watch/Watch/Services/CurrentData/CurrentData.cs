using System;
using System.Collections.Generic;
using Watch.Models;


namespace Watch.Services.CurrentData
{
    public class CurrentData : ICurrentData
    {
        public ClockProfile CurrentProfile { get; set; }
        public List<ClockProfile> Profiles { get; set; }
    }
}
