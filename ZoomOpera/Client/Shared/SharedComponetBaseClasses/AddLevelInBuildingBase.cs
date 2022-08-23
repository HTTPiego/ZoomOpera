using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddLevelInBuildingBase : ComponentBase
    {
        //[Parameter]
        public LevelDTO LevelToAdd { get; set; } = new LevelDTO();

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public EventCallback<ILevel> LevelAdded { get; set; }

        [Inject]
        protected IService<ILevel, LevelDTO> Service { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        public async Task OnUploadedPlanimetry(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var resizedImage = await e.File.RequestImageFileAsync(format, 300, 300);
            var buffer = new byte[resizedImage.Size];
            await resizedImage.OpenReadStream().ReadAsync(buffer);
            var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            LevelToAdd.Planimetry = imageData;
        }

        public async Task AddLevel()
        {
            try
            {
                LevelToAdd.BuildingId = FatherBuildingId;
                ILevel AddedLevel = await Service.AddEntity(LevelToAdd);
                await LevelAdded.InvokeAsync(AddedLevel);
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Piano non valido");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            LevelToAdd.Planimetry = string.Empty;
        }
    }
}
