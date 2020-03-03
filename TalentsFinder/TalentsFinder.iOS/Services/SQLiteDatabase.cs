using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Core.TalentsFinder.Services.SQLiteProvider;
using Foundation;
using UIKit;

namespace TalentsFinder.iOS.Services
{
    public class SQLiteDatabase : ISQLiteDatabase
    {
        string ISQLiteDatabase.GetConnection()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(documentsPath, "TalentsFinder.sqlite");
        }
    }
}