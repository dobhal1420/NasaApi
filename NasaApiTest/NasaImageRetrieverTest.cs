using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq.Protected;
using Moq;
using NasaApi.Client;
using NasaApiTest.MockObjects;
using Newtonsoft.Json;
using System.Net;
using NasaApi.Service;
using NasaApi.Domain;

namespace NasaApiTest
{
    public class NasaImageRetrieverTest
    {
        private INasaImageRetriever? _imageRetriever;
        private Mock<INasaClient> _mockNasaClient;
        private ILogger<NasaImageRetriever> _mocklogger;


        [SetUp]
        public void Setup()
        {
            var response = MockJsonResponse.Get().Collection?.Items;
            _mocklogger = Mock.Of<ILogger<NasaImageRetriever>>();
            _mockNasaClient = new Mock<INasaClient>();
            Dictionary<string, string?> parameters = new Dictionary<string, string?>();
            parameters.Add("q", null);
            parameters.Add("year_start", null);
            parameters.Add("year_end", null);
            _mockNasaClient.Setup(m => m.GetAsync(parameters)).Returns(Task.FromResult(response));

        }

        [Test]
        public async Task GivenMockHandlerWhenNasaImageRetrieverIsCalledReturnsSuccess()
        {
            _imageRetriever = new NasaImageRetriever(_mocklogger, _mockNasaClient.Object);
            NasaRequestParameter parameter = new NasaRequestParameter();
            var response = await _imageRetriever.FetchData(parameter);

            Assert.NotNull(response);
            Assert.True(response?.Count == MockJsonResponse.Get().Collection?.Items?.Count);
        }

    }
}
