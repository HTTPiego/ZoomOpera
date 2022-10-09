using System.Net.Http.Json;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class OperaImageService : IService<IOperaImage, OperaImageDTO>
    {
        private readonly HttpClient _httpClient;

        public OperaImageService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IOperaImage?> AddEntity(OperaImageDTO dto)
        {
            throw new NotImplementedException();
            //var response = await this._httpClient.PostAsJsonAsync("https://localhost:7288/api/OperaImages", dto);
            //return await response.Content.ReadFromJsonAsync<OperaImage>();
        }

        public async Task<IOperaImage?> DeleteEntity(Guid id)
        {
            var response = await this._httpClient.DeleteAsync($"https://localhost:7288/api/OperaImages/{id}");
            return await response.Content.ReadFromJsonAsync<OperaImage>();  
        }

        public async Task<IEnumerable<IOperaImage>?> GetAll()
        {
            return await this._httpClient.GetFromJsonAsync<IEnumerable<OperaImage>>("https://localhost:7288/api/OperaImages/all");
        }

        public Task<IEnumerable<IOperaImage>?> GetAllByfatherRelationshipId(Guid fatherId)
        {
            throw new NotImplementedException();
        }

        public async Task<IOperaImage?> GetEntity(Guid entityId)
        {
            return await this._httpClient.GetFromJsonAsync<OperaImage>($"https://localhost:7288/api/OperaImages/{entityId}");
        }

        public async Task<IOperaImage?> GetEntityByfatherRelationshipId(Guid fatherId)
        {
            return await this._httpClient.GetFromJsonAsync<OperaImage>($"https://localhost:7288/api/OperaImages?operaId={fatherId}");
        }

        public async Task<IOperaImage?> UpdateEntity(OperaImageDTO updatingDTO, Guid idEntity)
        {
            var response = await this._httpClient.PutAsJsonAsync($"https://localhost:7288/api/OperaImages/{idEntity}", updatingDTO);
            return await response.Content.ReadFromJsonAsync<OperaImage>();
        }
    }
}
