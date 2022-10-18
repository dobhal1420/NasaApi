using Microsoft.AspNetCore.WebUtilities;
using NasaApi.Domain;
using NasaApi.Service;

namespace NasaApi.Client
{
    public class NasaClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseUrl;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="httpClient"></param>
        public NasaClient(ILogger<NasaImageRetriever> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClient = httpClient;
            
            baseUrl = _configuration["ApiConfiguration:BaseUrl"];
            if (baseUrl == null)
            {
                throw new ArgumentNullException("ApiConfiguration:BaseUrl");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<List<NasaLineItem>?> GetAsync(Dictionary<string,string?> queryParameters)
        {
            var queryString = QueryHelpers.AddQueryString(baseUrl, queryParameters);

            var nasaResponse = await _httpClient.GetFromJsonAsync<NasaDataModel>(queryString);

            _logger.LogDebug("Nasa Client Response", nasaResponse);

            return nasaResponse?.Collection?.Items;
        }
    }
}
