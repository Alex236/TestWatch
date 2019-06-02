using System;
using System.Collections.Generic;
using Prism.Navigation;
using Watch.Models;
using Watch.PageParametersForNavigation;
using Watch.Services.CurrentData;
using Watch.Services.ServerConnection;
using Newtonsoft.Json;
using Watch.Views;


namespace Watch.ViewModels
{
    public class ProfileConfigureViewModel : ViewModelBase
    {
        private INavigationService navigationService;
        private readonly ICurrentData currentData;
        private readonly IServerConnection serverConnection;
        private ClockProfile profile;


        public ProfileConfigureViewModel(INavigationService navigationService, ICurrentData currentData, IServerConnection serverConnection)
            : base(navigationService)
        {
            this.navigationService = navigationService;
            this.currentData = currentData;
            this.serverConnection = serverConnection;
        }

        public ClockProfile Profile
        {
            get { return profile; }
            set { SetProperty(ref profile, value); }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            switch (parameters.GetValue<string>(nameof(AddOrEdit)))
            {
                case nameof(AddOrEdit.Add):
                    Profile = null;
                    break;
                case nameof(AddOrEdit.Edit):
                    Profile = currentData.CurrentProfile;
                    break;
            }
        }

        public void SaveProfile(ClockProfile profile)
        {
            if(Profile == null)
            {
                currentData.Profiles.Insert(0, profile);
            }
            else
            {
                int index = currentData.Profiles.IndexOf(Profile);
                currentData.Profiles[index] = profile;
            }
            serverConnection.Request("UpdateAllProfiles", JsonConvert.SerializeObject(currentData.Profiles));
            navigationService.NavigateAsync(nameof(ClockPage));
        }
    }
}
