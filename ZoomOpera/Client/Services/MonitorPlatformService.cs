using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;
namespace ZoomOpera.Client.Services
{
    public class MonitorPlatformService : IService<IMonitorPlatform, MonitorPlatformDTO>
    {
        private readonly HttpClient _httpClient;

        public MonitorPlatformService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IMonitorPlatform?> AddEntity(MonitorPlatformDTO dto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7288/api/MonitorPlatforms", dto);
            return await response.Content.ReadFromJsonAsync<MonitorPlatform>();
        }

        public async Task<IMonitorPlatform?> DeleteEntity(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"https://localhost:7288/api/MonitorPlatforms/{id}");
            return await response.Content.ReadFromJsonAsync<MonitorPlatform>();
        }

        public async Task<IEnumerable<IMonitorPlatform>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<MonitorPlatform>>("https://localhost:7288/api/MonitorPlatforms/all");
        }

        public async Task<IEnumerable<IMonitorPlatform>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<MonitorPlatform>>($"https://localhost:7288/api/MonitorPlatforms?levelId={fatherId}");
        }

        public async Task<IMonitorPlatform?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<MonitorPlatform>($"https://localhost:7288/api/MonitorPlatforms/{entityId}");
        }

        public Task<IMonitorPlatform?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IMonitorPlatform?> UpdateEntity(MonitorPlatformDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/MonitorPlatforms/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<MonitorPlatform>();
        }
    }
}
