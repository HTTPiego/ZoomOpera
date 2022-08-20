using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class ViewOperasInLevelBase : ComponentBase
    {

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected ILocalStorageService LocalStorage { get; set; }

        [Inject]
        protected ILoginService<IMonitorPlatform> Service { get; set; }

        [Inject]
        protected IService<ILevel, LevelDTO> LevelsService { get; set; }    

        [Inject]
        protected IService<IOpera, OperaDTO> OperasService { get; set; }

        public LinkedList<IOpera> OperasInLevel { get; set; } = new LinkedList<IOpera>();

        public ILevel MonitorPlatformLevel { get; set; } = new Level();

        private IMonitorPlatform LoggedInPlatform { get; set; } = new MonitorPlatform();

        

        protected override async Task OnInitializedAsync()
        {
            LoggedInPlatform = await GetPlatformByJwt();
            MonitorPlatformLevel = await LevelsService.GetEntity(LoggedInPlatform.LevelId);
            OperasInLevel = await GetOperas(MonitorPlatformLevel);
        }

        public void ViewOpera(Guid operaId)
        {
            NavigationManager.NavigateTo($"/visualizza-opere-piano/{operaId}");
        }

        private async Task<IMonitorPlatform?> GetPlatformByJwt()
        {

            var jwt = await LocalStorage.GetItemAsStringAsync("jwt");

            if (jwt == string.Empty)
                return null;

            var monitorPlatform = await Service.GetAccountByJwt(new JwtDTO(jwt));

            if (monitorPlatform == null)
                return null;

            return await Task.FromResult(monitorPlatform);
        }

        private async Task<LinkedList<IOpera>> GetOperas(ILevel monitorPlatformLevel)
        {
            var operas = new LinkedList<IOpera>();

            foreach(var location in monitorPlatformLevel.Locations)
            {
                var opera = await OperasService.GetEntityByfatherRelationshipId(location.Id);
                operas.AddLast(opera);
            }

            return operas;
        }

    }
}
