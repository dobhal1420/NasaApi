using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NasaApi.Domain;
using NasaApi.Service;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NasaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NasaApiController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly INasaImageRetriever _imageRetriever;
        private readonly IMemoryCache _cache;
        private const string nasaApiCacheKey = "nasaApiKey";

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        public NasaApiController(INasaImageRetriever nasaImageRetriever, ILogger<NasaApiController> logger, IMemoryCache cache)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
            _imageRetriever = nasaImageRetriever ?? throw new ArgumentNullException(nameof(nasaImageRetriever));
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        // GET: api/<NasaApiController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NasaLineItem))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] NasaRequestParameter parameter)
        {
            var parameterCacheKey = nasaApiCacheKey + parameter.SearchQuery + parameter.YearStart + parameter.YearEnd + parameter.MediaType + parameter.Page;
            _logger.Log(LogLevel.Information, "Trying to fetch the list of Nasa Image data from cache.");
            if (_cache.TryGetValue(parameterCacheKey, out List<NasaLineItem> nasaLineItems))
            {
                _logger.Log(LogLevel.Information, "Nasa Image data found in cache.");
            }
            else
            {
                try
                {
                    // For the race condition
                    await semaphore.WaitAsync();
                    if (_cache.TryGetValue(parameterCacheKey, out nasaLineItems))
                    {
                        _logger.Log(LogLevel.Information, "Nasa Image data found in cache.");
                    }
                    else
                    {
                        _logger.Log(LogLevel.Information, "Nasa Image not found in cache. Fetching from Internal Api.");
                        var response = await _imageRetriever.FetchAllData(parameter);
                        nasaLineItems = response!;
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                                .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                                .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                                .SetPriority(CacheItemPriority.Normal)
                                .SetSize(1024);
                        _cache.Set(parameterCacheKey, nasaLineItems, cacheEntryOptions);
                    }
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, ex.StackTrace);
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return Ok(nasaLineItems);
        }
    }
}
