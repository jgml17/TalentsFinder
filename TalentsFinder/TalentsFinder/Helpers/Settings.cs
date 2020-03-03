using Core.TalentsFinder.Interfaces;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Xamarin.Forms;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Helpers
{
    public static class Settings<T>
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public async static Task<T> GetValue(string key)
        {
            try
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    string value = string.Empty;

                    // Windows -> There is a size limit to use this plugin
                    value = await App.ServiceProvider.GetService<IStorage>().ReadTimestamp(key);

                    return string.IsNullOrEmpty(value) ? default : JsonSerializer.Deserialize<T>(value);
                }
                else
                {
                    // iOS and Android
                    string val = AppSettings.GetValueOrDefault(key, string.Empty);
                    return string.IsNullOrEmpty(val) ? default : JsonSerializer.Deserialize<T>(val);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"### ERROR => {ex.StackTrace} ###");
                return default;
            }
        }

        public async static Task SetValue(string key, T value)
        {
            try
            {
                if (Device.RuntimePlatform == Device.UWP)
                {
                    // Windows -> There is a size limit to use this plugin
                    await App.ServiceProvider.GetService<IStorage>().WriteTimestamp(key, JsonSerializer.Serialize(value));
                }
                else
                {
                    // iOS and Android
                    AppSettings.AddOrUpdateValue(key, JsonSerializer.Serialize(value));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"### ERROR => {ex.StackTrace} ###");
            }
        }
    }
}
