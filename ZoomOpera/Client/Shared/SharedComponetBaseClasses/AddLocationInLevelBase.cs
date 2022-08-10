using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddLocationInLevelBase : ComponentBase
    {
        [Inject]
        protected IService<ILocation, LocationDTO> Service { get; set; }

        [Parameter]
        public EventCallback<ILocation> LocationAdded { get; set; }

        //[Parameter]
        //public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        public LocationDTO LocationToAdd { get; set; } = new LocationDTO();

        public async void AddLocation()
        {
            LocationToAdd.LevelId = FatherLevelId;
            ILocation addedLocation = await Service.AddEntity(LocationToAdd);
            await LocationAdded.InvokeAsync(addedLocation);
        }

    }
}
