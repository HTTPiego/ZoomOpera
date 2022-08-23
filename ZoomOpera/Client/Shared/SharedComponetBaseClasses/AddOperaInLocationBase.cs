using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddOperaInLocationBase : ComponentBase
    {
        [Inject]
        protected IService<IOpera, OperaDTO> OperaService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

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

        public OperaDTO OperaToAdd { get; set; } = new OperaDTO();

        public OperaImageDTO OperaImageToAdd { get; set; } = new OperaImageDTO();

        public async Task OnUploadedOperaImage(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var resizedImage = await e.File.RequestImageFileAsync(format, 300, 300);
            var buffer = new byte[resizedImage.Size];
            await resizedImage.OpenReadStream().ReadAsync(buffer);
            var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            OperaImageToAdd.Image = imageData;
        }

        public async void AddOpera()
        {
            try
            {
                OperaToAdd.LocationId = FatherLocationId;
                OperaToAdd.OperaImage = OperaImageToAdd;
                IOpera addedOpera = await OperaService.AddEntity(OperaToAdd);
                EventHandler.FireEvent(addedOpera);
                NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere");
            }
            catch (Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Opera non valida");
            }
            
        }

    }
}
