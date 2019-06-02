using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Watch.ViewModels;
using Watch.Models;
using Watch.Services.ServerConnection;
using Watch.Services.CurrentData;
using Watch.PageParametersForNavigation;
using Newtonsoft.Json;


namespace Watch.Views
{
    public partial class ClockPage : ContentPage
    {
        private readonly IServerConnection serverConnection;
        private readonly ICurrentData currentData;
        private Page ProfileConfigurePage = new ProfileConfigurePage();
        private readonly Dictionary<string, string> ColorsAndHex = new Dictionary<string, string>()
        {
            {"Yellow", "#f1f442"},
            {"Blue", "#5d9afc"},
            {"White", "#ffffff"},
            {"Green", "#6dff6d"}
        };

        public ClockPage(IServerConnection serverConnection, ICurrentData currentData)
        {
            this.serverConnection = serverConnection;
            this.currentData = currentData;
            InitializeComponent();
            Device.StartTimer(TimeSpan.FromMilliseconds(500), () =>
            {
                clockCanvas.InvalidateSurface();
                return true;
            });
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await serverConnection.Connect();
            await serverConnection.Request("GetAllProfilesWithCurrent");
        }

        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            await serverConnection.Disconnect();
        }

        private void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            if (BindingContext is ClockViewModel clockViewModel)
            {
                if (currentData.CurrentProfile == null) return;
                bool handsParsed = SKColor.TryParse(ColorsAndHex[currentData.CurrentProfile.HandsColor], out SKColor handsColor);
                bool faceParsed = SKColor.TryParse(ColorsAndHex[currentData.CurrentProfile.FaceColor], out SKColor faceColor);
                DateTime? time = clockViewModel.GetTimeByTimezone();
                if (handsParsed && faceParsed && time != null)
                {
                    SKCanvas canvas = e.Surface.Canvas;
                    clockViewModel.DrawClock(handsColor, faceColor, canvas, (DateTime)time, e.Info.Height, e.Info.Width);
                }
            }
        }

        private void AddProfile(object sender, EventArgs e)
        {
            if (BindingContext is ClockViewModel clockViewModel)
            {
                clockViewModel.OpenProfileConfigure(AddOrEdit.Add);
            }
        }

        private void EditProfile(object sender, EventArgs e)
        {
            if (BindingContext is ClockViewModel clockViewModel)
            {
                clockViewModel.OpenProfileConfigure(AddOrEdit.Edit);
            }
        }
    }
}
