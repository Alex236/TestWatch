﻿using Prism;
using Prism.Ioc;
using Watch.ViewModels;
using Watch.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Watch.Services.CanvasDrawing;
using Watch.Services.GetTime;
using Watch.Services.ServerConnection;
using Watch.Services.CurrentData;
using Watch.Services.ColorsRepository;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Watch
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/ClockPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<ClockPage, ClockViewModel>();
            containerRegistry.RegisterForNavigation<ProfileConfigurePage, ProfileConfigureViewModel>();

            containerRegistry.Register<ICanvasDrawing, CanvasDrawing>();
            containerRegistry.Register<IGetTime, GetTime>();
            containerRegistry.RegisterSingleton<IServerConnection, ServerConnection>();
            containerRegistry.RegisterSingleton<ICurrentData, CurrentData>();
            containerRegistry.RegisterSingleton<IColorsRepository, ColorsRepository>();
        }
    }
}
