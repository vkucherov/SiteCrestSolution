using Contracts;
using System.Net.Http.Json;

namespace SiteCrestFrontend.API
{
    public class ValueApi : IValueApi
    {
        private readonly HttpClient _http;

        public ValueApi(HttpClient http)
        {
            _http = http;
        }

        public async Task<HttpResponseMessage> UpdateValuesAsync(EditChemicalValuesDto dto)
        {
            var response = await _http.PostAsJsonAsync("/manage/values", dto);
            return response;
        }
    }
}
