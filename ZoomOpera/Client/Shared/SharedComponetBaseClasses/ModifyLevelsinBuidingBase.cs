using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class ModifyLevelsinBuidingBase : ComponentBase
    {
        [Inject]
        protected IService<ILevel, LevelDTO> Service { get; set; }

        [Inject]
        protected IEventHandler<ILevel> Handler { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid LevelToModifyId { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        public LevelDTO UpdatingLevel { get; set; } = new LevelDTO();

        public async Task OnUploadedPlanimetry(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var resizedImage = await e.File.RequestImageFileAsync(format, 300, 300);
            var buffer = new byte[resizedImage.Size];
            await resizedImage.OpenReadStream().ReadAsync(buffer);
            var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            UpdatingLevel.Planimetry = imageData;
        }

        public async void ModifyLevel()
        {
            ILevel updatedLevel = await Service.UpdateEntity(UpdatingLevel, LevelToModifyId);
            Handler.FireEvent(updatedLevel);
            NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani");
        }

        protected override async Task OnInitializedAsync()
        {
            var dbLevel = await Service.GetEntity(LevelToModifyId);
            UpdatingLevel = new LevelDTO(dbLevel.LevelNumber, dbLevel.Planimetry);
        }

    }
}
