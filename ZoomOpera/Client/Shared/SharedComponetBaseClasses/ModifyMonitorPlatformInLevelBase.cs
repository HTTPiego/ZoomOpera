using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        protected IJSRuntime JSRuntime { get; set; }

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

        public MonitorPlatformDTO ManagedPlatform { get; set; } = new MonitorPlatformDTO();

        public bool ModifyOperation { get; set; } = false;

        public async void ManageRequest()
        {
            try
            {
                IMonitorPlatform dbPlatform;

                if (ModifyOperation) 
                    dbPlatform = await Service.UpdateEntity(ManagedPlatform, PlatformToModifyId);
                else
                    dbPlatform = await Service.AddEntity(ManagedPlatform);
                    
                EventHandler.FireEvent(dbPlatform);
                NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/piattaforme-monitor");
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Operazione non valida");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            if (! PlatformToModifyId.Equals(Guid.Empty))
            {
                ModifyOperation = true;
                IMonitorPlatform dbPlatform = await Service.GetEntity(PlatformToModifyId);
                ManagedPlatform = new MonitorPlatformDTO(dbPlatform.MonitorCode, dbPlatform.Name, dbPlatform.Password);
            }
            ManagedPlatform.LevelId = FatherLevelId;
        }
    }
}
