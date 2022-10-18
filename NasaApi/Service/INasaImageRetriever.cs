using NasaApi.Domain;

namespace NasaApi.Service
{
    public interface INasaImageRetriever
    {
        Task<NasaDataModel?> FetchAllData();
    }
}
