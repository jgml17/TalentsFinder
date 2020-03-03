using Core.TalentsFinder.Services.Navigation;
using Core.TalentsFinder.ViewModels;
using Core.TalentsFinder.Views;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Core.TalentsFinder
{
    public partial class App : Application
    {
        public static double ScreenHeight;
        public static double ScreenWidth;
        public static IServiceProvider ServiceProvider { get; set; }

        // TODO - App Center Key
        private const string AppCenterWindows = "TODO";
        private const string AppCenteriOS = "TODO";

        public App()
        {
            InitializeComponent();

            // Start page
            var navigationService = ServiceProvider.GetService<INavigationService<MainViewModel>>();
            navigationService.InitializeAsync();
        }

        protected  override void OnStart()
        {
            // Handle when your app starts

                AppCenter.Start($"windows={AppCenterWindows};ios={AppCenteriOS}",
                typeof(Analytics),
                typeof(Crashes),
                typeof(Distribute));
            
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
