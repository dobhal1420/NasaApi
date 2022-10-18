using Microsoft.Net.Http.Headers;
using NasaApi.Client;
using NasaApi.Domain;

namespace NasaApi.Service
{
    public class NasaImageRetriever : INasaImageRetriever
    {
        private readonly ILogger _logger;
        private readonly NasaClient _nasaClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="httpClient"></param>
        public NasaImageRetriever(ILogger<NasaImageRetriever> logger, NasaClient nasaClient)
        {
            _logger = logger;
            _nasaClient = nasaClient;
        }

        public async Task<List<NasaLineItem>?> FetchAllData() {

            var query = "search?q=mars";
            _logger.LogDebug("NasaRetriever querying ", query);
            var nasaResponse = await _nasaClient.GetAsync(query);

            

            return nasaResponse;
        }
    }
}
