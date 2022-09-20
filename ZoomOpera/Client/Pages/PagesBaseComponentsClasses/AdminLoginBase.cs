using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class AdminLoginBase : ComponentBase
    {

        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        protected AuthenticationStateProvider StateProvider { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILoginService<IAdmin> Service { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public LoginDTO LoginCredentials { get; set; } = new LoginDTO();

        //public AdminLoginBase(ILocalStorageService localStorage) 
        //{
        //    _localStorage = localStorage;
        //    LoginDatas = new LoginDTO();
        //}

        public async Task Login()
        {
            var jwt = await Service.Login(LoginCredentials);

            if (jwt.Token != string.Empty)
            {
                await LocalStorage.SetItemAsStringAsync("jwt", jwt.Token);
                await StateProvider.GetAuthenticationStateAsync();
                NavigationManager.NavigateTo("/home-admin");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Credenziali non corrette, riprova perfavore");
            }
        }

        public async Task<IAdmin?> GetAdminByJwt()
        {

            var jwt = await LocalStorage.GetItemAsStringAsync("jwt");

            if (jwt == string.Empty)
                return null;

            var admin = await Service.GetAccountByJwt(new JwtDTO(jwt));

            if (admin == null)
                return null;

            return await Task.FromResult(admin);
        }


    }
}
