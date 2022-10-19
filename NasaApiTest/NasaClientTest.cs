using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using NasaApi.Client;
using NasaApi.Service;
using NasaApiTest.MockObjects;
using Newtonsoft.Json;
using NUnit.Framework.Internal;
using System.Net;
using System.Text;

namespace NasaApiTest
{
    public class NasaClientTest
    {
        private NasaClient? _nasaClient;
        private ILogger<NasaClient> _mocklogger;
        private Mock<IConfiguration> _mockconfiguration;
        private HttpClient _mockHttpClient;
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
            var configurationSectionMock = new Mock<IConfigurationSection>();
            _mockconfiguration = new Mock<IConfiguration>();

            configurationSectionMock
               .Setup(x => x.Value)
               .Returns("http://someservice:81");

            _mockconfiguration
               .Setup(x => x.GetSection("ApiConfiguration:BaseUrl"))
               .Returns(configurationSectionMock.Object);
        }

        [Test]
        public async Task GivenMockHandlerWhenNasaClientIsCalled()
        {
            _nasaClient = new NasaClient(_mocklogger,_mockconfiguration.Object, _mockHttpClient);
            Dictionary<string,string?> paramaters = new Dictionary<string,string?>();

            var response = await _nasaClient.GetAsync(paramaters);

            Assert.NotNull(response);
            Assert.True(response?.Count == MockJsonResponse.Get().Collection?.Items?.Count);
        }
    }
}