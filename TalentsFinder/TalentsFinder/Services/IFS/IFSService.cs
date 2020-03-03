using Core.Models.TalentsFinderModels;
using Core.TalentsFinder.Services.RequestProvider;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.TalentsFinder.Services.IFS
{
    public class IFSService : IIFSService
    {
        private const string APIURI = "https://v1.ifs.aero";

        private readonly IRequestProvider _requestProvider;
        private readonly ILogger<IFSService> _logger;
        // 10 seconds 
        protected const int TimeOut = 20000;

        public IFSService(IRequestProvider requestProvider, ILogger<IFSService> logger)
        {
            _requestProvider = requestProvider;
            _logger = logger;
        }

        /// <summary>
        /// Get all technologies
        /// </summary>
        /// <returns></returns>
        public async Task<List<TecModel>> GetTechnologies()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            List<TecModel> response;

            string endpoint = $"{APIURI}/technologies";
            Task<List<TecModel>> task = _requestProvider.GetAsync<List<TecModel>>(endpoint, tokenSource.Token);

            if (task == await Task.WhenAny(task, Task.Delay(TimeOut, tokenSource.Token)))
            {
                response = await task;
            }
            else
            {
                // TIMEOUT
                tokenSource.Cancel(); // Send Cancellation to thread
                throw new HttpRequestExceptionEx(HttpStatusCode.RequestTimeout, "TIMEOUT");
            }

            return response;
        }

        /// <summary>
        /// Get all talents
        /// </summary>
        /// <returns></returns>
        public async Task<List<TalentModel>> GetTalents()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            List<TalentModel> response;

            string endpoint = $"{APIURI}/candidates";
            Task<List<TalentModel>> task = _requestProvider.GetAsync<List<TalentModel>>(endpoint, tokenSource.Token);

            if (task == await Task.WhenAny(task, Task.Delay(TimeOut, tokenSource.Token)))
            {
                response = await task;
            }
            else
            {
                // TIMEOUT
                tokenSource.Cancel(); // Send Cancellation to thread
                throw new HttpRequestExceptionEx(HttpStatusCode.RequestTimeout, "TIMEOUT");
            }

            return response;
        }
    }
}
