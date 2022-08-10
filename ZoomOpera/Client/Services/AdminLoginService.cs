using System.Net.Http.Json;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services
{
    public class AdminLoginService : ILoginService<IAdmin>
    {
        private readonly HttpClient _httpClient;

        //private readonly Uri _url;

        public AdminLoginService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
            //this._url = new Uri("https://localhost:7288/api/AdminLogin");
        }
        
        public async Task<JwtDTO?> Login(LoginDTO login)
        {
            var response = await this._httpClient.PostAsJsonAsync<LoginDTO>("https://localhost:7288/api/AdminLogin", login);
            return await response.Content.ReadFromJsonAsync<JwtDTO>();
        }
        public async Task<IAdmin?> GetAccountByJwt(JwtDTO jwt)
        {
            var response = await this._httpClient.PostAsJsonAsync<JwtDTO>("https://localhost:7288/api/AdminLogin/get-admin", jwt);
            return await response.Content.ReadFromJsonAsync<IAdmin>();
        }

    }
}
