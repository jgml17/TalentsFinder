using Core.Models.TalentsFinderModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Services.IFS
{
    public interface IIFSService
    {
        Task<List<TecModel>> GetTechnologies();
        Task<List<TalentModel>> GetTalents();
    }
}
