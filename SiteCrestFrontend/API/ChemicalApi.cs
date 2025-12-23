using Contracts;
using System.Net.Http.Json;
using static System.Net.WebRequestMethods;

namespace SiteCrestFrontend.API
{
    public class ChemicalApi : IChemicalApi
    {
        private readonly HttpClient _http;

        public ChemicalApi(HttpClient http)
        {
            _http = http;
        }

        public Task<List<ChemicalDto>?> GetAllAsync() =>
            _http.GetFromJsonAsync<List<ChemicalDto>>("/manage/chemicals");

        public Task<EditChemicalValuesDto?> GetByIdAsync(int id) =>
            _http.GetFromJsonAsync<EditChemicalValuesDto>($"/manage/chemicals/{id}");

        public async Task<HttpResponseMessage> UpdateAsync(int id, EditChemicalValuesDto dto)
        {
            var response = await _http.PutAsJsonAsync($"/manage/chemicals/{id}", dto);
            return response;
        }
        public async Task<HttpResponseMessage> CreateAsync(ChemicalDto dto)
        {
            var response = await _http.PostAsJsonAsync($"/manage/chemicals/", dto);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            var response = await _http.DeleteAsync($"/manage/chemicals/{id}");
            return response;
        }
    }
}
