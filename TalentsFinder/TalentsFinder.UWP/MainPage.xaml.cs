using Core.TalentsFinder;
using Core.TalentsFinder.Interfaces;
using Core.TalentsFinder.Resources;
using Core.TalentsFinder.Services.SQLiteProvider;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using TalentsFinder.UWP.Services;
using TalentsFinder.UWP.Utils;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace TalentsFinder.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            // == Plugins Initialization ==
            Rg.Plugins.Popup.Popup.Init();

            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            Core.TalentsFinder.App.ScreenWidth = bounds.Width;
            Core.TalentsFinder.App.ScreenHeight = bounds.Height;

            LoadApplication(Startup.Init(ConfigureServices));

            // Set Title bar
            var appView = ApplicationView.GetForCurrentView();
            //appView.Title = AppResources.MAIN_VIEW_TITLE;

            //var appTitleBar = ApplicationView.GetForCurrentView().TitleBar;
            //appTitleBar.BackgroundColor = Colors.LightBlue;
            //appTitleBar.ButtonBackgroundColor = Colors.LightBlue;
            //appTitleBar.ButtonForegroundColor = Colors.Black;
        }

        void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            // Specific iOS services
            services.AddSingleton<IStorage, Storage>();
            services.AddTransient<ISQLiteDatabase, SQLiteDatabase>();
        }
    }
}
