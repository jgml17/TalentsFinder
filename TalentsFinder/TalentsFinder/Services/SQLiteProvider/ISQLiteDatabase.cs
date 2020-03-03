using System;
using System.Collections.Generic;
using System.Text;

namespace Core.TalentsFinder.Services.SQLiteProvider
{
    public interface ISQLiteDatabase
    {
        string GetConnection();
    }
}
