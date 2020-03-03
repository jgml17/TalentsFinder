using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Interfaces
{
    public interface IStorage
    {
        Task WriteTimestamp(string key, string json);

        Task<string> ReadTimestamp(string key);
    }
}
