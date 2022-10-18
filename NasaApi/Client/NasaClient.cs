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
        /// Nasa client.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="httpClient"></param>
        public NasaClient(ILogger<NasaImageRetriever> logger, IConfiguration configuration, HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
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
