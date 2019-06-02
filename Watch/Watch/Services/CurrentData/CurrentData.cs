using System;
using System.Collections.Generic;
using Watch.Models;
using Xamarin.Forms;
using System.Threading;
using Xamarin.Essentials;


namespace Watch.Services.CurrentData
{
    public delegate void Update();

    public class CurrentData : ICurrentData
    {
        public ClockProfile CurrentProfile { get; set; }
        public List<ClockProfile> Profiles { get; set; }
        public event Update OnUpdate;
        public void ExecuteUpdate()
        {
            MainThread.BeginInvokeOnMainThread(() => OnUpdate());
        }
    }
}
