using Contracts;

namespace SiteCrestFrontend.API
{
    public interface IValueApi
    {
        Task<HttpResponseMessage> UpdateValuesAsync(EditChemicalValuesDto dto);
    }
}
