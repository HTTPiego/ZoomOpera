using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class LevelService : IService<ILevel, LevelDTO>
    {
        private readonly HttpClient _httpClient;
        public LevelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ILevel?> AddEntity(LevelDTO dto)
        {
            var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/Levels", dto);
            return await response.Content.ReadFromJsonAsync<Level>();
        }

        public async Task<ILevel?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/Levels/{id}");
            return await response.Content.ReadFromJsonAsync<Level>();
        }

        public async Task<IEnumerable<ILevel>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Level>>("https://localhost:7288/api/Levels/all");
        }

        public async Task<IEnumerable<ILevel>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Level>>($"https://localhost:7288/api/Levels?buildingId={fatherId}");
        }

        public async Task<ILevel?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<Level>($"https://localhost:7288/api/Levels/{entityId}");
        }

        public Task<ILevel?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<ILevel?> UpdateEntity(LevelDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/Levels/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<Level>();
        }
    }
}
