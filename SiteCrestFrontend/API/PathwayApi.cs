using Contracts;
using System.Net.Http.Json;

namespace SiteCrestFrontend.API
{
    public class PathwayApi : IPathwayApi
    {
        private readonly HttpClient _http;

        public PathwayApi(HttpClient http)
        {
            _http = http;
        }

        public Task<List<PathwayDto>> GetAllAsync() => _http.GetFromJsonAsync<List<PathwayDto>>("/manage/pathways");

        public Task<PathwayDto?> GetByIdAsync(int id) => _http.GetFromJsonAsync<PathwayDto>($"/manage/pathways/{id}");
    }
}
