using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Services.Interfaces
{
    public interface ILoginService<I>
    {
        Task<JwtDTO?> Login(LoginDTO login);

        Task<I?> GetAccountByJwt(JwtDTO jwt);
    }
}
