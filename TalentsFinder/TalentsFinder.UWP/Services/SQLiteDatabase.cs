using Core.TalentsFinder.Services.SQLiteProvider;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace TalentsFinder.UWP.Services
{
    public class SQLiteDatabase : ISQLiteDatabase
    {
        private StorageFolder localFolder = ApplicationData.Current.LocalFolder;

        string ISQLiteDatabase.GetConnection()
        {
            return Path.Combine(localFolder.Path, "TalentsFinder.sqlite");
        }
    }
}
