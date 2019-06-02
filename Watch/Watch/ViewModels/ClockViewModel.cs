using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Watch.Services.CanvasDrawing;
using Watch.Services.GetTime;
using Watch.Services.ServerConnection;
using Watch.Services.CurrentData;
using Watch.Models;
using Watch.Views;
using Watch.PageParametersForNavigation;
using Xamarin.Forms;

namespace Watch.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        private readonly ICanvasDrawing canvasDrawing;
        private readonly IGetTime getTime;
        private readonly INavigationService navigationService;
        private readonly IServerConnection serverConnection;
        private readonly ICurrentData currentData;
        private HubConnection connection;

        public ClockViewModel(INavigationService navigationService, ICanvasDrawing canvasDrawing, IGetTime getTime, IServerConnection serverConnection, ICurrentData currentData)
            : base(navigationService)
        {
            this.navigationService = navigationService;
            this.canvasDrawing = canvasDrawing;
            this.getTime = getTime;
            this.serverConnection = serverConnection;
            this.currentData = currentData;

            connection = serverConnection.Connection;
                
            connection.On<string>("GetAllProfilesWithCurrent", async (messege) =>
            {
                await GetAllProfilesWithCurrent(messege);
            });

            connection.Closed += async (error) =>
            {
                await Task.Delay(2000);
                await connection.StartAsync();
            };
        }

        public async Task GetAllProfilesWithCurrent(string messege)
        {
            AllProfilesWithCurrent allProfilesWithCurrent = ParseMessege(messege);
            if (allProfilesWithCurrent == null)
            {
                await SetProfile();
            }
            else
            {
                currentData.CurrentProfile = allProfilesWithCurrent.CurrentProfile;
                currentData.Profiles = allProfilesWithCurrent.AllProfiles;
            }
        }

        private AllProfilesWithCurrent ParseMessege(string messege)
        {
            AllProfilesWithCurrent allProfilesWithCurrent = null;
            try
            {
                allProfilesWithCurrent = JsonConvert.DeserializeObject<AllProfilesWithCurrent>(messege);
            }
            finally
            {
                if (allProfilesWithCurrent != null)
                {
                    currentData.CurrentProfile = allProfilesWithCurrent.CurrentProfile;
                    currentData.Profiles = allProfilesWithCurrent.AllProfiles;
                }
            }
            return allProfilesWithCurrent;
        }

        private async Task SetProfile()
        {
            if(currentData.Profiles != null)
            {
                if(currentData.Profiles.Count > 0)
                {
                    currentData.CurrentProfile = currentData.Profiles[0];
                }
                else
                {
                    currentData.Profiles.Add(GetDefaultProfile);
                    currentData.CurrentProfile = currentData.Profiles[0];
                }
            }
            else
            {
                currentData.Profiles = new List<ClockProfile>()
                {
                    GetDefaultProfile
                };
                currentData.CurrentProfile = currentData.Profiles[0];
            }
            await serverConnection.Request("UpdateAllData", JsonConvert.SerializeObject(new AllProfilesWithCurrent()
            {
                AllProfiles = currentData.Profiles,
                CurrentProfile = currentData.CurrentProfile
            }));
        }

        private ClockProfile GetDefaultProfile
        {
            get
            {
                return new ClockProfile()
                {
                    Name = "def",
                    FaceColor = "Blue",
                    HandsColor = "White",
                    Timezone = "America/Managua"
                };
            }
        }

        public void DrawClock(SKColor handsColor, SKColor faceColor, SKCanvas canvas, DateTime time, int heigh, int width)
        {
            canvasDrawing.DrawClock(handsColor, faceColor, canvas, time, heigh, width);
        }

        public DateTime? GetTimeByTimezone()
        {
            return getTime.GetTimeByTimezone(currentData.CurrentProfile.Timezone);
        }

        public void OpenProfileConfigure(AddOrEdit addOrEdit)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add(nameof(AddOrEdit), addOrEdit.ToString());
            NavigationService.NavigateAsync(nameof(ProfileConfigurePage));
        }
    }
}
