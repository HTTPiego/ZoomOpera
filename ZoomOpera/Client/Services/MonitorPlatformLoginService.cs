using System.Net.Http.Json;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class MonitorPlatformLoginService : ILoginService<IMonitorPlatform>
    {
        private readonly HttpClient _httpClient;

        public MonitorPlatformLoginService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IMonitorPlatform?> GetAccountByJwt(JwtDTO jwt)
        {
            var response = await this._httpClient.PostAsJsonAsync<JwtDTO>("https://localhost:7288/api/MonitoPlatformLogin/get-platfrom", jwt);
            return await response.Content.ReadFromJsonAsync<IMonitorPlatform>();
        }

        public async Task<JwtDTO?> Login(LoginDTO login)
        {
            var response = await this._httpClient.PostAsJsonAsync<LoginDTO>("https://localhost:7288/api/MonitoPlatformLogin", login);
            return await response.Content.ReadFromJsonAsync<JwtDTO>();
        }
    }
}
