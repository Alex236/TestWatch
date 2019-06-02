using System.Collections.Generic;


namespace Watch.Models
{
    public class AllProfilesWithCurrent
    {
        public ClockProfile CurrentProfile { get; set; }
        public List<ClockProfile> AllProfiles { get; set; }
    }
}
