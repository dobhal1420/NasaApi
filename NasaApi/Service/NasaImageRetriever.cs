using NasaApi.Client;
using NasaApi.Domain;
using System.Collections.Specialized;

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

        public async Task<List<NasaLineItem>?> FetchAllData(NasaRequestParameter parameter)
        {

            var queryParameters = GetQueryString(parameter);

            _logger.LogDebug("NasaRetriever querying ", queryParameters);
            var nasaResponse = await _nasaClient.GetAsync(queryParameters);



            return nasaResponse;
        }

        private Dictionary<string, string?> GetQueryString(NasaRequestParameter parameter)
        {
            Dictionary<string, string?> queryString = new();

            queryString.Add("q", parameter.SearchQuery);


            queryString.Add("year_start", parameter.YearStart);

            queryString.Add("year_end", parameter.YearEnd);

            if (parameter.MediaType != null)
            {
                queryString.Add("media_type", parameter.MediaType.ToString());
            }

            if (parameter.Page.HasValue)
            {
                queryString.Add("page", parameter.Page.Value.ToString());
            }

            return queryString;
        }
    }
}
