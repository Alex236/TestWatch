using System;
using System.IO;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Watch.ViewModels;
using Watch.Models;
using Newtonsoft.Json;


namespace Watch.Views
{
    public partial class Clock : ContentPage
    {
        private readonly List<ClockProfile> clocksProfiles = new List<ClockProfile>();
        private readonly Dictionary<string, Color> Colors = new Dictionary<string, Color>()
        {
            {"Yellow", Color.Yellow},
            {"Blue", Color.Blue},
            {"White", Color.White}
        };
        private ClockProfile currentProfile;
        private ClockProfile getDefaultProfile
        {
            get { return new ClockProfile() { Name = "Default", FaceColorHex = "White", HandsColorHex = "Blue" }; }
        }

        public Clock()
        {
            InitializeComponent();
            GetCurrentProfile();
            FillPickerWithProfiles();
        }

        private void GetCurrentProfile()
        {
            //make request to server
            string json = "";
            try
            {
                currentProfile = JsonConvert.DeserializeObject<ClockProfile>(json);
            }
            catch
            {
                currentProfile = getDefaultProfile;
            }
        }

        private void FillPickerWithProfiles()
        {
                //make request to server
                clocksProfiles.Add(new ClockProfile() { Name = "prof1" });
                clocksProfiles.Add(new ClockProfile() { Name = "prof2" });
                clocksProfiles.Add(new ClockProfile() { Name = "prof3" });

            foreach(var profile in clocksProfiles)
            {
                ProfilePicker.Items.Add(profile.Name);
            }
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (BindingContext is ClockViewModel clockViewModel)
            {
                bool handsParsed = SKColor.TryParse(clockViewModel.CurrentClockProfile.HandsColorHex, out SKColor handsColor);
                bool faceParsed = SKColor.TryParse(clockViewModel.CurrentClockProfile.FaceColorHex, out SKColor faceColor);
                DateTime? time = clockViewModel.GetTimeByTimezone();
                if (handsParsed && faceParsed && time != null)
                {
                    SKCanvas canvas = e.Surface.Canvas;
                    clockViewModel.DrawClock(handsColor, faceColor, canvas, (DateTime)time, e.Info.Height, e.Info.Width);
                }
            }
        }
    }
}
