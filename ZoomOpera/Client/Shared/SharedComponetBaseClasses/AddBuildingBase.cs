using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddBuildingBase : ComponentBase
    {
        //[Parameter]
        public BuildingDTO BuildingToAdd { get; set; } = new BuildingDTO();

        [Parameter]
        public EventCallback<IBuilding> BuildingAdded { get; set; }

        [Inject]
        protected IService<IBuilding, BuildingDTO> Service { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        //[Inject]
        //protected NavigationManager NavigationManager { get; set; }

        //public async Task OnAddedBuilding(MouseEventArgs e)
        //{
        //    await BuildingAdded.InvokeAsync(AddedBuilding);
        //    //NavigationManager.NavigateTo("/strutture");
        //}

        public async Task AddBuilding()
        {
            if (String.IsNullOrEmpty(BuildingToAdd.BuildingCode)
                || String.IsNullOrEmpty(BuildingToAdd.Name))
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Edificio non valido");
                return;
            }
            IBuilding AddedBuilding = await Service.AddEntity(BuildingToAdd);
            await BuildingAdded.InvokeAsync(AddedBuilding);
        }

    }
}
