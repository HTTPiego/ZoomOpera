using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class LocationService : IService<ILocation, LocationDTO>
    {
        private readonly HttpClient _httpClient;

        public LocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ILocation?> AddEntity(LocationDTO dto)
        {
            var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/Locations", dto);
            return await response.Content.ReadFromJsonAsync<Location>();
        }

        public async Task<ILocation?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/Locations/{id}");
            return await response.Content.ReadFromJsonAsync<Location>();
        }

        public async Task<IEnumerable<ILocation>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Location>>("https://localhost:7288/api/Locations/all");
        }

        public async Task<IEnumerable<ILocation>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<Location>>($"https://localhost:7288/api/Locations?levelId={fatherId}");
        }

        public async Task<ILocation?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<Location>($"https://localhost:7288/api/Locations/{entityId}");
        }

        public Task<ILocation?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<ILocation?> UpdateEntity(LocationDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/Locations/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<Location>();
        }
    }
}
