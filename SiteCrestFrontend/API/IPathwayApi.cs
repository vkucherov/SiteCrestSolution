using Contracts;

namespace SiteCrestFrontend.API
{
    public interface IPathwayApi
    {
        Task<List<PathwayDto>> GetAllAsync();
        Task<PathwayDto?> GetByIdAsync(int id);
    }
}
