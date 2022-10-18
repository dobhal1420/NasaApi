using NasaApi.Domain;
using NasaApi.Service;

namespace NasaApi.Client
{
    public class NasaClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

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
            _httpClient.BaseAddress = new Uri(_configuration["ApiConfiguration:BaseUrl"]);
        }

        public async Task<List<NasaLineItem>?> GetAsync(string queryString)
        {

            queryString = "search?q=mars";
            var nasaResponse = await _httpClient.GetFromJsonAsync<NasaDataModel>(queryString);

            _logger.LogDebug("Nasa Client Response", nasaResponse);

            return nasaResponse?.collection?.items;
        }
    }
}
