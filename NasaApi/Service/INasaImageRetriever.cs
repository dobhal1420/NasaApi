using NasaApi.Domain;

namespace NasaApi.Service
{
    public interface INasaImageRetriever
    {
        Task<List<NasaLineItem>?> FetchAllData();
    }
}
