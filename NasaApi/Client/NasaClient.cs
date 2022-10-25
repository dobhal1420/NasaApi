using Microsoft.AspNetCore.WebUtilities;
using NasaApi.Domain;
using NasaApi.Policies.RequestService.Policies;
using NasaApi.Service;
using Polly;

namespace NasaApi.Client
{
    public class NasaClient : INasaClient
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string baseUrl;
        private readonly ClientPolicy _policy;


        /// <summary>
        /// Nasa client.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="httpClient"></param>
        /// <param name="policy"> </param>
        public NasaClient(ILogger<NasaClient> logger,
                          IConfiguration configuration,
                          HttpClient httpClient,
                          ClientPolicy policy)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
            baseUrl = _configuration.GetValue<string>("ApiConfiguration:BaseUrl");
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new ArgumentNullException("ApiConfiguration:BaseUrl");
            }
            _httpClient.BaseAddress = new Uri(baseUrl);
        }

        public async Task<List<NasaLineItem>?> GetAsync(Dictionary<string,string?> queryParameters)
        {
            var queryString = QueryHelpers.AddQueryString(baseUrl, queryParameters);

            var httpResponse = await _policy.ExponentialHttpRetry.ExecuteAsync(() => _httpClient.GetAsync(queryString));


            if (httpResponse.IsSuccessStatusCode)
            {
                var response = await httpResponse.Content.ReadFromJsonAsync<NasaDataModel>();
                var nasaResponse = response?.Collection?.Items;
                _logger.LogDebug("Nasa Client Response", nasaResponse);
                return nasaResponse;
            }

            return null;
            
        }
    }
}
