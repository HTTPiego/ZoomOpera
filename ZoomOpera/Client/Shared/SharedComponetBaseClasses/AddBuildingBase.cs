using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddBuildingBase : ComponentBase
    {
        //public BuildingDTO BuildingToAdd { get; set; } = new BuildingDTO();

        //[Parameter]
        //public EventCallback<IBuilding> BuildingAdded { get; set; }

        //[Inject]
        //protected IService<IBuilding, BuildingDTO> Service { get; set; }

        //[Inject]
        //protected IJSRuntime JSRuntime { get; set; }

        //public async Task AddBuilding()
        //{
        //    if (String.IsNullOrEmpty(BuildingToAdd.BuildingCode)
        //        || String.IsNullOrEmpty(BuildingToAdd.Name))
        //    {
        //        await JSRuntime.InvokeVoidAsync("Alert", "Edificio non valido");
        //        return;
        //    }
        //    IBuilding AddedBuilding = await Service.AddEntity(BuildingToAdd);
        //    await BuildingAdded.InvokeAsync(AddedBuilding);
        //}

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

        public BuildingDTO BuildingToAdd { get; set; } = new BuildingDTO();

        public async Task AddBuilding()
        {
            try
            {
                IBuilding addBuilding = await Service.AddEntity(BuildingToAdd);
                Handler.FireEvent(addBuilding);
                NavigationManager.NavigateTo("/strutture");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Aggiunta non valida");
            }

        }

        //protected override async Task OnInitializedAsync()
        //{
        //    var dbBuilding = await Service.GetEntity(BuildingToModifyId);
        //    ManagedBuilding = new BuildingDTO(dbBuilding.Name, dbBuilding.BuildingCode);
        //}

    }
}
