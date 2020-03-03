using Core.TalentsFinder.Helpers;
using Core.TalentsFinder.Services.IFS;
using Core.TalentsFinder.Services.Navigation;
using Core.TalentsFinder.Services.RequestProvider;
using Core.TalentsFinder.Services.SQLiteProvider;
using Core.TalentsFinder.ViewModels;
using Core.TalentsFinder.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace Core.TalentsFinder
{
    public static class Startup
    {
        public static App Init(Action<HostBuilderContext, IServiceCollection> nativeConfigureServices)
        {
            var systemDir = FileSystem.CacheDirectory;
            Utils.ExtractSaveResource("Core.TalentsFinder.appsettings.json", systemDir);
            var fullConfig = Path.Combine(systemDir, "Core.TalentsFinder.appsettings.json");

            var host = new HostBuilder()
                            .ConfigureHostConfiguration(c =>
                            {
                                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });
                                c.AddJsonFile(fullConfig);
                            })
                            .ConfigureServices((c, x) =>
                            {
                                nativeConfigureServices(c, x);
                                ConfigureServices(c, x);
                            })
                            .ConfigureLogging(l => l.AddConsole(o =>
                            {
                                o.DisableColors = true;
                            }))
                            .Build();

            App.ServiceProvider = host.Services;

            return App.ServiceProvider.GetService<App>();
        }


        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services)
        {
            if (ctx.HostingEnvironment.IsDevelopment())
            {
                //var world = ctx.Configuration["Hello"];
            }

            // Views  
            services.AddTransient<MainView>();
            services.AddTransient<TalentsView>();

            // ViewModels  
            services.AddTransient<MainViewModel>();
            services.AddTransient<TalentsViewModel>();

            // Services
            services.AddHttpClient();
            services.AddDbContext<SQLiteDbContext>();
            //services.AddDbContext<SQLiteDbContext>(opt => opt.UseSqlite(string.Empty), ServiceLifetime.Transient);

            services.AddTransient<IIFSService, IFSService>();
            services.AddTransient<IRequestProvider, RequestProvider>();
            services.AddTransient(typeof(INavigationService<>), typeof(NavigationService<>));

            // Singletons
            services.AddSingleton<App>();
        }
    }
}
