using Prism.Navigation;
using System;
using System.Linq;
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


namespace Watch.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        private readonly ICanvasDrawing canvasDrawing;
        private readonly IGetTime getTime;
        private readonly IServerConnection serverConnection;
        private readonly ICurrentData currentData;

        public ClockViewModel(INavigationService navigationService, ICanvasDrawing canvasDrawing, IGetTime getTime, IServerConnection serverConnection, ICurrentData currentData)
            : base(navigationService)
        {
            this.canvasDrawing = canvasDrawing;
            this.getTime = getTime;
            this.serverConnection = serverConnection;
            this.currentData = currentData;

            HubConnection connection = serverConnection.Connection;
                
            connection.On<string>("GetAllProfiles", async (messege) =>
            {
                await GetAllProfiles(messege);
            });

            connection.Closed += async (error) =>
            {
                await Task.Delay(2000);
                await connection.StartAsync();
            };
        }

        private ClockProfile GetDefaultProfile
        {
            get
            {
                return new ClockProfile()
                {
                    Name = "Default",
                    FaceColor = "Blue",
                    HandsColor = "White",
                    Timezone = "America/Managua"
                };
            }
        }

        public async Task GetAllProfiles(string messege)
        {
            ClockProfile[] profiles = ParseMessege(messege);
            if (profiles != null)
            {
                if (profiles.Length > 0)
                {
                    currentData.CurrentProfile = profiles[0];
                    currentData.Profiles = profiles.ToList();
                    currentData.ExecuteUpdate();
                    return;
                }
            }
            await SetDefaultProfile();
        }

        private ClockProfile[] ParseMessege(string messege)
        {
            ClockProfile[] profiles;
            try
            {
                profiles = JsonConvert.DeserializeObject<ClockProfile[]>(messege);
            }
            catch
            {
                profiles = null;
            }
            return profiles;
        }

        private async Task SetDefaultProfile()
        {
            var profiles = new ClockProfile[]
            {
                GetDefaultProfile
            };
            await serverConnection.Request("UpdateAllProfiles", JsonConvert.SerializeObject(profiles));
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
            NavigationService.NavigateAsync(nameof(ProfileConfigurePage), parameters);
        }
    }
}
