using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class MonitorPlatformLoginBase : ComponentBase
    {
        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        protected AuthenticationStateProvider StateProvider { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILoginService<IMonitorPlatform> Service { get; set; }

        [Parameter]
        public LoginDTO LoginCredentials { get; set; } = new LoginDTO();

        public async Task Login()
        {
            var jwt = await Service.Login(LoginCredentials);

            if (jwt.Token != string.Empty)
            {
                await LocalStorage.SetItemAsStringAsync("jwt", jwt.Token);
                await StateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/home-platform");
            }
            else
            {
                Console.WriteLine("MALE");
                //mostra errore 
                //dare occhiata a "toast service"
            }
        }

        public async Task<IMonitorPlatform?> GetPlatformByJwt()
        {

            var jwt = await LocalStorage.GetItemAsStringAsync("jwt");

            if (jwt == string.Empty)
                return null;

            var monitorPlatform = await Service.GetAccountByJwt(new JwtDTO(jwt));

            if (monitorPlatform == null)
                return null;

            return await Task.FromResult(monitorPlatform);
        }

    }
}
