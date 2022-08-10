using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ModifyMonitorPlatformInLevelBase : ComponentBase
    {
        [Inject]
        protected IService<IMonitorPlatform, MonitorPlatformDTO> Service { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IEventHandler<IMonitorPlatform> EventHandler { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid PlatformToModifyId { get; set; }

        public MonitorPlatformDTO UpdatingPlatform { get; set; } = new MonitorPlatformDTO();

        public async void ModifyPlatform()
        {
            IMonitorPlatform updatedPlatform = await Service.UpdateEntity(UpdatingPlatform, PlatformToModifyId);
            EventHandler.FireEvent(updatedPlatform);
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/piattaforme-monitor");
        }

        protected override async Task OnInitializedAsync()
        {
            IMonitorPlatform dbPlatform = await Service.GetEntity(PlatformToModifyId);
            UpdatingPlatform = new MonitorPlatformDTO(dbPlatform.MonitorCode, dbPlatform.Name, dbPlatform.Password);
        }
    }
}
