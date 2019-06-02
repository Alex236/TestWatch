using System;
using Watch.Models;
using Watch.Services.CurrentData;
using Watch.Services.ColorsRepository;
using Watch.Services.ServerConnection;
using Watch.ViewModels;

using Xamarin.Forms;

namespace Watch.Views
{
    public partial class ProfileConfigurePage : ContentPage
    {
        private readonly ICurrentData currentData;
        private readonly IColorsRepository colorsRepository;
        private readonly IServerConnection serverConnection;
        public ProfileConfigurePage(ICurrentData currentData, IColorsRepository colorsRepository, IServerConnection serverConnection)
        {
            InitializeComponent();
            this.currentData = currentData;
            this.colorsRepository = colorsRepository;
            this.serverConnection = serverConnection;
            InitializeControls();
        }

        private void InitializeControls()
        {
            foreach (var timeZoneInfo in TimeZoneInfo.GetSystemTimeZones())
            {   
                timezonePicker.Items.Add(timeZoneInfo.Id);
            }
            foreach (var color in colorsRepository.ColorAndHex)
            {
                faceColorPicker.Items.Add(color.Key);
                hansdColorPicker.Items.Add(color.Key);
            }
        }

        public void SaveProfile(object sender, EventArgs e)
        {
            if (nameEntry.Text != null
                && timezonePicker.SelectedItem != null
                && faceColorPicker.SelectedItem != null
                && hansdColorPicker.SelectedItem != null
                && BindingContext is ProfileConfigureViewModel profileConfigureViewModel)
            {
                profileConfigureViewModel.SaveProfile(new ClockProfile()
                {
                    Name = nameEntry.Text,
                    Timezone = timezonePicker.SelectedItem as string,
                    FaceColor = faceColorPicker.SelectedItem as string,
                    HandsColor = hansdColorPicker.SelectedItem as string
                });

            }
        }
    }
}
