using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ModifyOperaInLocationBase : ComponentBase
    {
        [Inject]
        protected IService<IOpera, OperaDTO> OperaService { get; set; }

        [Inject]
        protected IService<IOperaImage, OperaImageDTO> OperaImageService { get; set; }

        [Inject]    
        protected IEventHandler<IOpera> EventHandler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid FatherLocationId { get; set; }

        [Parameter]
        public Guid OperaToModifyId { get; set; }

        public OperaDTO OperaToModify { get; set; } = new OperaDTO();

        public OperaImageDTO OperaImageToModify { get; set; } = new OperaImageDTO();

        public async Task OnUploadedOperaImage(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var resizedImage = await e.File.RequestImageFileAsync(format, 300, 300);
            var buffer = new byte[resizedImage.Size];
            await resizedImage.OpenReadStream().ReadAsync(buffer);
            var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            OperaImageToModify.Image = imageData;
        }

        public async void ModifyOpera()
        {
            try
            {
                OperaToModify.OperaImage = OperaImageToModify;
                IOpera updatedOpera = await OperaService.UpdateEntity(OperaToModify, OperaToModifyId);
                EventHandler.FireEvent(updatedOpera);
                NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{FatherLocationId}/opera");
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Modifica non valida");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            IOpera dbOpera = await OperaService.GetEntity(OperaToModifyId);
            OperaToModify = new OperaDTO(dbOpera.Name,
                                              dbOpera.ItalianDescription,
                                              dbOpera.AuthorFirstName,
                                              dbOpera.AuthorLastName);

            IOperaImage dbOperaImage = await OperaImageService.GetEntityByfatherRelationshipId(dbOpera.Id);
            OperaImageToModify = new OperaImageDTO(dbOperaImage.Image, 0, 0);
        }
    }
}
