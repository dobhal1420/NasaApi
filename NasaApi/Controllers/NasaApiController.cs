using Microsoft.AspNetCore.Mvc;
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

        public NasaApiController(INasaImageRetriever nasaImageRetriever, ILogger<NasaApiController> logger)
        {
            _logger = logger;
            _imageRetriever = nasaImageRetriever;
        }
        // GET: api/<NasaApiController>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NasaLineItem))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Get([FromQuery] NasaRequestParameter parameter)
        {
            try
            {
                var response = await _imageRetriever.FetchAllData(parameter);
                if (response == null)
                {
                    return NotFound();
                }
                return Ok(response!);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.StackTrace);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
