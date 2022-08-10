using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class BuildingsBase : ComponentBase, IDisposable
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IService<IBuilding, BuildingDTO> BuidingService { get; set; }
        //protected BuildingService BuidingService { get; set; }

        [Inject]
        protected IEventHandler<IBuilding> Handler { get; set; }

        [Parameter]
        public BuildingDTO Building { get; set; } = new BuildingDTO();

        //public IEnumerable<IBuilding> Buildings { get; set; } = Enumerable.Empty<IBuilding>();

        public List<IBuilding> Buildings { get; set; } = new List<IBuilding>();

        public async Task<IEnumerable<IBuilding>> GetBuildings()
        {
            return await BuidingService.GetAll();
        }

        //public async void GoToAddingBuilding()
        //{
        //    NavigationManager.NavigateTo("/strutture/aggiungi-struttura");
        //}

        public async void GoToLevels(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{id}/piani");
        }

        public void ModifyBuilding(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{id}/modifica-struttura");
        }

        public async void DeleteBuilding(Guid id)
        {
            var deletedBuilding = await BuidingService.DeleteEntity(id);
            Buildings.Remove(deletedBuilding);
            StateHasChanged();
        }

        public async Task HandleOnAddedBuilding(IBuilding addedBuilding)
        {
            Buildings.Add(addedBuilding);
        }

        public async void HandleOnModifiedBuilding(object sender, IBuilding modifiedBuilding)
        {
            var index = Buildings.FindIndex(b => b.Id.Equals(modifiedBuilding.Id));

            if (index != -1)
                Buildings[index] = modifiedBuilding;

        }

        protected override async Task OnInitializedAsync()
        {
            var buildings = await GetBuildings();
            Buildings = buildings.ToList();
            Handler.EventHandler += HandleOnModifiedBuilding;
        }

        public void Dispose()
        {
            Handler.EventHandler -= HandleOnModifiedBuilding;
        }
    }
}
