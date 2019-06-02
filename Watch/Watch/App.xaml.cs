using Prism;
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
