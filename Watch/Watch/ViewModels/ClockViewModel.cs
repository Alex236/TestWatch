using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SkiaSharp;
using Watch.Services.CanvasDrawing;
using Watch.Services.GetTime;
using Watch.Models;

namespace Watch.ViewModels
{
    public class ClockViewModel : ViewModelBase
    {
        private readonly ICanvasDrawing CanvasDrawing;
        private readonly IGetTime GetTime;
        private ClockProfile currentClockProfile;

        public ClockViewModel(INavigationService navigationService, ICanvasDrawing canvasDrawing, IGetTime getTime)
            : base(navigationService)
        {
            CanvasDrawing = canvasDrawing;
            GetTime = getTime;
            GetCurrentProfile();
        }

        public ClockProfile CurrentClockProfile
        {
            get { return currentClockProfile; }
            set { SetProperty(ref currentClockProfile, value); }
        }

        private void GetCurrentProfile()
        {
            //request to server
            currentClockProfile = new ClockProfile() { Name="def", FaceColorHex= "#42f4aa", HandsColorHex= "#4441f4", Timezone= "America/Managua" };
        }

        public void DrawClock(SKColor handsColor, SKColor faceColor, SKCanvas canvas, DateTime time, int heigh, int width)
        {
            CanvasDrawing.DrawClock(handsColor, faceColor, canvas, time, heigh, width);
        }

        public DateTime? GetTimeByTimezone()
        {
            return GetTime.GetTimeByTimezone(CurrentClockProfile.Timezone);
        }
    }
}
