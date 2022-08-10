using Microsoft.AspNetCore.Components;
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

        [Parameter]
        public Guid BuildingToModifyId { get; set; }

        public BuildingDTO UpdatingBuilding { get; set; } = new BuildingDTO();

        public async Task ModifyBuilding()
        {
            IBuilding modifiedBuilding = await Service.UpdateEntity(UpdatingBuilding, BuildingToModifyId);
            Handler.FireEvent(modifiedBuilding);
            NavigationManager.NavigateTo("/strutture");
        }

        protected override async Task OnInitializedAsync()
        {
            var dbBuilding = await Service.GetEntity(BuildingToModifyId);
            UpdatingBuilding = new BuildingDTO(dbBuilding.Name, dbBuilding.BuildingCode);
        }
    }
}
