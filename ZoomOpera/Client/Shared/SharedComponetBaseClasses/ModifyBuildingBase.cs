using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ModifyBuildingBase : ComponentBase
    {
        [Inject]
        protected IService<IBuilding, BuildingDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<IBuilding> Handler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid BuildingToModifyId { get; set; }

        public BuildingDTO ManagedBuilding { get; set; } = new BuildingDTO();

        public bool ModifyOperation = false;

        public async Task ManageRequest()
        {
            try
            {
                IBuilding dbBuilding;

                if (ModifyOperation)
                {
                    dbBuilding = await Service.UpdateEntity(ManagedBuilding, BuildingToModifyId);
                }
                else
                {
                    dbBuilding = await Service.AddEntity(ManagedBuilding);
                }

                Handler.FireEvent(dbBuilding);
                NavigationManager.NavigateTo("/strutture");
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Operazione non valida");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            if (! BuildingToModifyId.Equals(Guid.Empty))
            {
                ModifyOperation = true;
                var dbBuilding = await Service.GetEntity(BuildingToModifyId);
                ManagedBuilding = new BuildingDTO(dbBuilding.Name, dbBuilding.BuildingCode);
            }
        }
    }
}
