using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NasaApi.Client;
using NasaApi.Policies.RequestService.Policies;
using NasaApiTest.MockObjects;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using System.Net;

namespace NasaApiTest
{
    public class NasaClientTest
    {
        private INasaClient? _nasaClient;
        private ILogger<NasaClient> _mocklogger;
        private Mock<IConfiguration> _mockconfiguration;
        private HttpClient _mockHttpClient;
        private Mock<IConfigurationSection> _mockconfigurationSection;
        private ClientPolicy _clientPolicy;

        [SetUp]
        public void Setup()
        {
            var response = MockJsonResponse.Get();
            var mockResponse = new HttpResponseMessage()
            {
                Content = new StringContent(JsonConvert.SerializeObject(response)),
                StatusCode = HttpStatusCode.OK
            };

            var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            _mockHttpClient = new HttpClient(mockHttpMessageHandler.Object);
            _mocklogger = Mock.Of<ILogger<NasaClient>>();
            _mockconfigurationSection = new Mock<IConfigurationSection>();
            _mockconfiguration = new Mock<IConfiguration>();

            _mockconfigurationSection
               .Setup(x => x.Value)
               .Returns("http://someservice:81");

            _mockconfiguration
               .Setup(x => x.GetSection("ApiConfiguration:BaseUrl"))
               .Returns(_mockconfigurationSection.Object);

            _clientPolicy = new ClientPolicy();
        }

        [Test]
        public async Task GivenMockHandlerWhenNasaClientIsCalledReturnsSuccess()
        {
            _nasaClient = new NasaClient(_mocklogger, _mockconfiguration.Object, _mockHttpClient, _clientPolicy);
            Dictionary<string, string?> paramaters = new Dictionary<string, string?>();

            var response = await _nasaClient.GetAsync(paramaters);

            Assert.NotNull(response);
            Assert.True(response?.Count == MockJsonResponse.Get().Collection?.Items?.Count);
        }

        [Test]
        public void GivenMockHandlerWithNoConfigurationWhenNasaClientIsCalledReturnsFaiure()
        {
            _mockconfigurationSection
               .Setup(x => x.Value)
               .Returns("");

            Assert.Throws<ArgumentNullException>(() => _nasaClient = new NasaClient(_mocklogger, _mockconfiguration.Object, _mockHttpClient, _clientPolicy));
        }

    }
}