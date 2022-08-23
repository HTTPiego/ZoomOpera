using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddPlatformInLevelBase : ComponentBase
    {
        [Inject]
        protected IService<IMonitorPlatform, MonitorPlatformDTO> Service { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public MonitorPlatformDTO PlatformToAdd { get; set; } = new MonitorPlatformDTO();

        [Parameter]
        public EventCallback<IMonitorPlatform> PlatformAdded { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        public async void AddPlatform()
        {
            try
            {
                PlatformToAdd.LevelId = FatherLevelId;
                IMonitorPlatform addedPlatform = await Service.AddEntity(PlatformToAdd);
                PlatformAdded.InvokeAsync(addedPlatform);
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Piattaforma-Monitor non valida");
            }

        }

    }
}
