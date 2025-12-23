using Contracts;

namespace SiteCrestFrontend.API
{
    public interface IChemicalApi
    {
        Task<List<ChemicalDto>> GetAllAsync();
        Task<EditChemicalValuesDto?> GetByIdAsync(int id);
        Task<HttpResponseMessage> UpdateAsync(int id, EditChemicalValuesDto dto);
        Task<HttpResponseMessage> CreateAsync(ChemicalDto dto);
        Task<HttpResponseMessage> DeleteAsync(int id);
    }
}
