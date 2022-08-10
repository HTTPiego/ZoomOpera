using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class OperaService : IService<IOpera, OperaDTO>
    {
        private readonly HttpClient _httpClient;

        public OperaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IOpera?> AddEntity(OperaDTO dto)
        {
            var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/Operas", dto);
            return await response.Content.ReadFromJsonAsync<Opera>();
        }

        public async Task<IOpera?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/Operas/{id}");
            return await response.Content.ReadFromJsonAsync<Opera>();
        }

        public async Task<IEnumerable<IOpera>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Opera>>("https://localhost:7288/api/Operas/all");
        }

        public Task<IEnumerable<IOpera>?> GetAllByfatherRelationshipId(Guid fatherId)
        {   
            throw new NotImplementedException();
        }

        public async Task<IOpera?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<Opera>($"https://localhost:7288/api/Operas/{entityId}");
        }

        public async Task<IOpera?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<Opera>($"https://localhost:7288/api/Operas?locationId={fatherId}");
        }

        public async Task<IOpera?> UpdateEntity(OperaDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/Operas/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<Opera>();
        }
    }
}
