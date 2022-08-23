using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ModifyLocationsInLevelBase : ComponentBase
    {
        [Inject]
        protected IService<ILocation, LocationDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<ILocation> EventHandler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid LocationToModifyId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        public LocationDTO UpdatingLocation { get; set; } = new LocationDTO();

        public async void ModifyLocation()
        {
            try
            {
                ILocation updatedLocation = await Service.UpdateEntity(UpdatingLocation, LocationToModifyId);
                EventHandler.FireEvent(updatedLocation);
                NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere");
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Modifica non valida");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            ILocation dbLocation = await Service.GetEntity(LocationToModifyId);
            UpdatingLocation = new LocationDTO(dbLocation.LocationCode, dbLocation.Notes);
        }
    }
}
