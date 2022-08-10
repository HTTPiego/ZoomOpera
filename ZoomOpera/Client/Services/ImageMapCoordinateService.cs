using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class ImageMapCoordinateService : IService<IImageMapCoordinate, ImageMapCoordinateDTO>
    {
        private readonly HttpClient _httpClient;
        public ImageMapCoordinateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IImageMapCoordinate?> AddEntity(ImageMapCoordinateDTO dto)
        {
            throw new NotImplementedException();
            //var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/ImageMapCoordinates", dto);
            //return await response.Content.ReadFromJsonAsync<ImageMapCoordinate>();
        }

        public async Task<IImageMapCoordinate?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/ImageMapCoordinates/{id}");
            return await response.Content.ReadFromJsonAsync<ImageMapCoordinate>();
        }

        public async Task<IEnumerable<IImageMapCoordinate>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<ImageMapCoordinate>>($"https://localhost:7288/api/ImageMapCoordinates/all");
        }

        public async Task<IEnumerable<IImageMapCoordinate>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<ImageMapCoordinate>>($"https://localhost:7288/api/ImageMapCoordinates?imageMapId={fatherId}");
        }

        public async Task<IImageMapCoordinate?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<ImageMapCoordinate>($"https://localhost:7288/api/ImageMapCoordinates/{entityId}");
        }

        public Task<IImageMapCoordinate?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IImageMapCoordinate?> UpdateEntity(ImageMapCoordinateDTO updatingDTO, Guid idEntity)
        {
            throw new NotImplementedException();
            //var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/ImageMapCoordinates/{idEntity}", updatingDTO);
            //return await response.Content.ReadFromJsonAsync<ImageMapCoordinate>();
        }
    }
}
