using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class MonitorPlatformsInLevelBase : ComponentBase, IDisposable
    {
        [Inject]
        protected IService<IMonitorPlatform, MonitorPlatformDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<IMonitorPlatform> EventHandler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        public List<IMonitorPlatform> MonitorPlatforms { get; set; } = new List<IMonitorPlatform>();

        public void GoToAddPlatform()
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/piattaforme-monitor/aggiungi-piattaforma");
        }

        public void ModifyPlatform(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/piattaforme-monitor/{id}/modifica-piattaforma");
        }

        public async void DeletePlatform(Guid id)
        {
            IMonitorPlatform deletedPlatform = await Service.DeleteEntity(id);
            MonitorPlatforms.Remove(deletedPlatform);
            StateHasChanged();
        }

        public void HandleOnAddedPlatform(IMonitorPlatform addedPlatform)
        {
            MonitorPlatforms.Add(addedPlatform);
        }

        public void HandleOnModifiedPlatform(object sender, IMonitorPlatform updatedPlatform)
        {
            var index = MonitorPlatforms.FindIndex(p => p.Equals(updatedPlatform));

            if (index == -1)
                MonitorPlatforms[index] = updatedPlatform;

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var platforms = await Service.GetAllByfatherRelationshipId(FatherLevelId);
            MonitorPlatforms = platforms.ToList();
            EventHandler.EventHandler += HandleOnModifiedPlatform;
        }

        public void Dispose()
        {
            EventHandler.EventHandler -= HandleOnModifiedPlatform;
        }
    }
}
