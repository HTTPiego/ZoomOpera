using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class ImageMapService : IService<IImageMap, ImageMapDTO>
    {
        private readonly HttpClient _httpClient;

        public ImageMapService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IImageMap?> AddEntity(ImageMapDTO dto)
        {
            var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/ImageMaps", dto);
            return await response.Content.ReadFromJsonAsync<ImageMap>();
        }

        public async Task<IImageMap?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/ImageMaps/{id}");
            return await response.Content.ReadFromJsonAsync<ImageMap>();
        }

        public async Task<IEnumerable<IImageMap>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<ImageMap>>("https://localhost:7288/api/ImageMaps/all");
        }

        public async Task<IEnumerable<IImageMap>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<ImageMap>>($"https://localhost:7288/api/ImageMaps?operaImageId={fatherId}");
        }

        public async Task<IImageMap?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<ImageMap>($"https://localhost:7288/api/ImageMaps/{entityId}");
        }

        public Task<IImageMap?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IImageMap?> UpdateEntity(ImageMapDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/ImageMaps/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<ImageMap>();
        }
    }
}
