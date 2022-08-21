using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ViewOperaDetailsBase : ComponentBase
    {

        [Inject]
        protected IService<IImageMap, ImageMapDTO> ImageMapsService { get; set; }

        [Parameter]
        public Guid ImageMapId { get; set; }

        public IImageMap Details { get; set; } = new ImageMap();

        protected override async Task OnInitializedAsync()
        {
            Details = await ImageMapsService.GetEntity(ImageMapId);
        }

    }
}
