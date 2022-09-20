using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class LocationsInLevelBase : ComponentBase, IDisposable
    {
        [Inject]
        protected IService<ILocation, LocationDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<ILocation> EventHandlerModifiedLocation { get; set; }

        [Inject]
        protected IEventHandler<IOpera> EventHandlerAddOpera { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        public List<ILocation> Locations { get; set; } = new List<ILocation>();

        public void GoAddLocation()
        {
            NavigationManager
                .NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/aggiungi-locazione");
        }

        public void GoToOpera(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{id}/opera");
        }

        public void AddNewOpera(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{id}/aggiungi-opera");
        }

        public void ModifyLocation(Guid id)
        {
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni/{id}/modifica-locazione");
        }

        public async void DeleteLocation(Guid id)
        {
            ILocation deletedLocation = await Service.DeleteEntity(id);
            Locations.Remove(deletedLocation);
            StateHasChanged();
        }

        public void HandleOnAddedLocation(ILocation addedLocation)
        {
            Locations.Add(addedLocation);
        }

        public void HandleOnAddedOpera(object sender, IOpera addedOpera)
        {
            //var locationOfTheAddedOpera = Locations.Find(l => l.Equals(addedOpera.Location));
            //locationOfTheAddedOpera.Opera = (Opera)addedOpera;
            StateHasChanged();
        }

        public void HandleOnModifiedLocation(object sender, ILocation modifiedLocation)
        {
            var index = Locations.FindIndex(l => l.Equals(modifiedLocation));

            if (index != -1)
                Locations[index] = modifiedLocation;

            StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            var locations = await Service.GetAllByfatherRelationshipId(FatherLevelId);
            Locations = locations.ToList();
            EventHandlerModifiedLocation.EventHandler += HandleOnModifiedLocation;
            EventHandlerAddOpera.EventHandler += HandleOnAddedOpera;
        }

        public void Dispose()
        {
            EventHandlerModifiedLocation.EventHandler -= HandleOnModifiedLocation;
            EventHandlerAddOpera.EventHandler -= HandleOnAddedOpera;
        }
    }
}
