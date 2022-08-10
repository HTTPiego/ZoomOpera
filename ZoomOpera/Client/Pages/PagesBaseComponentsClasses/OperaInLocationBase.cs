using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class OperaInLocationBase : ComponentBase, IDisposable
    {
        [Inject]
        protected IService<IOpera, OperaDTO> OperaService { get; set; }

        [Inject]
        protected IService<IOperaImage, OperaImageDTO> OperaImageService { get; set; }

        [Inject]
        protected IEventHandler<IOpera> EventHandler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid FatherLocationId { get; set; }

        public IOpera Opera { get; set; } //= new Opera();

        public IOperaImage OperaImage { get; set; }// = new OperaImage();

        public void AddDetailedDescriptions()
        {
            NavigationManager.
                NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{FatherLocationId}/opera/{Opera.Id}/descrizioni-dettagliate");
        }

        public void ModifyOpera()
        {
            NavigationManager.
                NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{FatherLocationId}/opera/{Opera.Id}/modifica-opera");
        }

        public void HandleOnModifiedOpera(object sender, IOpera modifiedOpera)
        {
            StateHasChanged();
        }

        public async void DeleteOpera()
        {
            await OperaService.DeleteEntity(Opera.Id);
            //Opera = null;
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere");
        }

        protected override async Task OnInitializedAsync()
        {
            Opera = await OperaService.GetEntityByfatherRelationshipId(FatherLocationId);
            OperaImage = await OperaImageService.GetEntityByfatherRelationshipId(Opera.Id);
            EventHandler.EventHandler += HandleOnModifiedOpera;
        }

        public void Dispose()
        {
            EventHandler.EventHandler -= HandleOnModifiedOpera;
        }
    }
}
