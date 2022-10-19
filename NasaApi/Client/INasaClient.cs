using NasaApi.Domain;

namespace NasaApi.Client
{
    public interface INasaClient
    {
        Task<List<NasaLineItem>?> GetAsync(Dictionary<string, string?> queryParameters);
    }
}
