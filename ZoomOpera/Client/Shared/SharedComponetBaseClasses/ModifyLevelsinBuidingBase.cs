using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
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

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid LevelToModifyId { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        public LevelDTO ManagedLevel { get; set; } = new LevelDTO();

        public bool ModifyOperation = false;

        public async Task OnUploadedPlanimetry(InputFileChangeEventArgs e)
        {
            var format = "image/png";
            var resizedImage = await e.File.RequestImageFileAsync(format, 300, 300);
            var buffer = new byte[resizedImage.Size];
            await resizedImage.OpenReadStream().ReadAsync(buffer);
            var imageData = $"data:{format};base64,{Convert.ToBase64String(buffer)}";
            ManagedLevel.Planimetry = imageData;
        }

        public async void ManageRequest()
        {
            try
            {
                ILevel dbLevel;
                
                if (ModifyOperation)
                    dbLevel = await Service.UpdateEntity(ManagedLevel, LevelToModifyId);
                else
                    dbLevel = await Service.AddEntity(ManagedLevel);

                Handler.FireEvent(dbLevel);
                NavigationManager.NavigateTo($"/strutture/{FatherBuildingId}/piani");
            }
            catch(Exception ex)
            {
                await JSRuntime.InvokeVoidAsync("Alert", "Operazione non valida");
            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            if (! LevelToModifyId.Equals(Guid.Empty))
            {
                ModifyOperation = true;
                var dbLevel = await Service.GetEntity(LevelToModifyId);
                ManagedLevel = new LevelDTO(dbLevel.LevelNumber, dbLevel.Planimetry);
            }
            ManagedLevel.BuildingId = FatherBuildingId;
            ManagedLevel.Planimetry = String.Empty;
        }

    }
}
