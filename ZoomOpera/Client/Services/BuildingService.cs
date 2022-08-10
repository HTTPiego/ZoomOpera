using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class BuildingService : IService<IBuilding, BuildingDTO>
    {
        private readonly HttpClient _httpClient;

        public BuildingService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IBuilding?> AddEntity(BuildingDTO dto)
        {
            var response = await this._httpClient.PostAsJsonAsync<BuildingDTO>("https://localhost:7288/api/Buildings", dto);
            return await response.Content.ReadFromJsonAsync<Building>();
        }

        public async Task<IBuilding?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/Buildings/{id}");
            return await response.Content.ReadFromJsonAsync<Building>();
        }

        public async Task<IEnumerable<IBuilding>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Building>>("https://localhost:7288/api/Buildings/all");
        }

        public Task<IEnumerable<IBuilding>> GetAllByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }
            
        public async Task<IBuilding?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<Building?>($"https://localhost:7288/api/Buildings/{entityId}");
        }

        public Task<IBuilding?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IBuilding?> UpdateEntity(BuildingDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/Buildings/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<Building>();
        }
    }
}
