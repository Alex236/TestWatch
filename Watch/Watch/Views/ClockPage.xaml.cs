using System;
using System.Collections.Generic;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Watch.ViewModels;
using Watch.Models;
using Watch.Services.ServerConnection;
using Watch.Services.CurrentData;
using Watch.Services.ColorsRepository;
using Watch.PageParametersForNavigation;
using Newtonsoft.Json;


namespace Watch.Views
{
    public partial class ClockPage : ContentPage
    {
        private readonly IServerConnection serverConnection;
        private readonly ICurrentData currentData;
        private readonly IColorsRepository colorsRepository;

        public ClockPage(IServerConnection serverConnection, ICurrentData currentData, IColorsRepository colorsRepository)
        {
            this.serverConnection = serverConnection;
            this.currentData = currentData;
            this.colorsRepository = colorsRepository;
            InitializeComponent();
            currentData.OnUpdate += UpdateProfilesPicker;

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
            await serverConnection.Request("GetAllProfiles");
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
                bool handsParsed = SKColor.TryParse(colorsRepository.ColorAndHex[currentData.CurrentProfile.HandsColor], out SKColor handsColor);
                bool faceParsed = SKColor.TryParse(colorsRepository.ColorAndHex[currentData.CurrentProfile.FaceColor], out SKColor faceColor);
                DateTime? time = clockViewModel.GetTimeByTimezone();
                if (handsParsed && faceParsed && time != null)
                {
                    SKCanvas canvas = e.Surface.Canvas;
                    clockViewModel.DrawClock(handsColor, faceColor, canvas, (DateTime)time, e.Info.Height, e.Info.Width);
                }
            }
        }

        private void UpdateProfilesPicker()
        {
            if (currentData.Profiles != null)
            {
                profilePicker.Items.Clear();
                foreach (var profile in currentData.Profiles)
                {
                    profilePicker.Items.Add(profile.Name);
                }
                profilePicker.SelectedItem = currentData.CurrentProfile.Name;
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

        public void SelectProfile(object sender, EventArgs e)
        {
            currentData.CurrentProfile = currentData.Profiles[profilePicker.SelectedIndex];
        }
    }
}
