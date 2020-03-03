using Core.TalentsFinder.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TalentsFinder.UWP.Utils
{
    public class Storage : IStorage
    {
        // This example code can be used to read or write to an ApplicationData folder of your choice.

        // Change this to Windows.Storage.StorageFolder roamingFolder = Windows.Storage.ApplicationData.Current.RoamingFolder;
        // to use the RoamingFolder instead, for example.
        StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        public Storage()
        {

        }

        // Write data to a file
        public async Task WriteTimestamp(string key, string json)
        {
            StorageFile sampleFile = await localFolder.CreateFileAsync($"{key}.txt", CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(sampleFile, json);
        }

        // Read data from a file
        public async Task<string> ReadTimestamp(string key)
        {
            string timestamp = string.Empty;
            try
            {
                StorageFile sampleFile = await localFolder.GetFileAsync($"{key}.txt");
                timestamp = await FileIO.ReadTextAsync(sampleFile);
                // Data is contained in timestamp
            }
            catch (FileNotFoundException e)
            {
                // Cannot find file
                throw e;
            }
            catch (IOException e)
            {
                // Get information from the exception, then throw
                // the info to the parent method.
                if (e.Source != null)
                {
                    Debug.WriteLine("IOException source: {0}", e.Source);
                }
                throw;
            }

            return timestamp;
        }
    }
}
