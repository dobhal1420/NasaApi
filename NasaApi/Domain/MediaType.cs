using System.Text.Json.Serialization;

namespace NasaApi.Domain
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum MediaType
    {
        audio,
        image
    }
}
