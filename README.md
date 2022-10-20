# NasaApi
Nasa Api is a gateway web API on top of "https://api.nasa.gov/".
This service offers an endpoint that query on the basis of some query parameters. It returns either an HTTP error code, or a Nasa Data in JSON form.

An example Nasa Api query:

**GET Endpoint:** `NasaApi?SearchQuery=mars`

Application Web API is designed in Asp Dot Net core levearaging in memory cache to avoid frequent hits to the original Nasa api. It also has polly retries implemented so in case of failures/outages our api will retry.

## Contents
  * [How to Run](#how-to-run)
  * [API Reference](#api-reference)
  * [Project Architecture](#project-architecture)
  * [Code Flow](#code-flow)
  * [Tests](#tests)

## How to Run
Traverse to - NetCore\NasaApiBE\NasaApi\NasaApi

```
>> dotnet build
>> dotnet run
```

Browse Url
http:/localhost:8080/swagger/index.html

## API Reference

#### Swagger Page:

   http://localhost:5248/swagger/index.html
   
   ## Project Architecture


    <!--![Architecture Diagram](TODO) -->


## Code Flow

  * There are 3 sections of the code 
    - Controller
    - Domain
    - Service

#### Controller:

  * *NasaApiController.cs* handles incoming HTTP requests and sends response back to the caller.
  * An HTTP get method is defined in the controller class.

#### Model:

  * A Model *NasaDataModel.cs* represents data that is being transferred between controller components or any other related business logic.

#### Service:

  * This is the logical layer which aims at organizing business logic.
  * *NasaImageRetriever.cs* is responsible for evaluating the query parameters passed via GET method.
  * *NasaClient.cs* is a http client which calls the Nasa api and gets the data based on query parameters.


## Tests

  * The *NUnit* testing framework has been used for integration/unit test.
  * *Moq* framework has been used for mocking the service.
  * This framework includes following packages of classes that provides support for developing and executing unit tests:
    - *NasaClientTest.cs*
      -  GivenMockHandlerWhenNasaClientIsCalledReturnsSuccess - This method validates the test case when api return success.
      -  GivenMockHandlerWithNoConfigurationWhenNasaClientIsCalledReturnsFaiure - This method checks for failure when no configuration is passed.
    - *NasaImageRetrieverTest.cs*
      - GivenMockHandlerWhenNasaImageRetrieverIsCalledReturnsSuccess - This method validates when api return success."
  * *MockJsonResponse.cs* is a test data class that contains the mock data.

