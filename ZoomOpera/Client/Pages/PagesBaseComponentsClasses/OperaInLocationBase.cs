using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.JSInterop;
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

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid FatherLocationId { get; set; }

        public IOpera Opera { get; set; } //= new Opera();

        public IOperaImage OperaImage { get; set; }// = new OperaImage();


        private int ImageWidth { get; set; }

        private int ImageHeight { get; set; }

        public async void AddDetailedDescriptions()
        {
            //PRIMO
            //var query = new Dictionary<string, string> { { "Width" , ImageWidth.ToString()}, { "Height", ImageHeight.ToString() } };
            //NavigationManager.NavigateTo(QueryHelpers.AddQueryString($"/strutture/{FatherBuildingId:guid}/piani/{FatherLevelId:guid}/locazioni-opere/{FatherLocationId:guid}/opera/{Opera.Id:guid}/descrizioni-dettagliate", query));
            //SECONDO
            //ImageWidth = await JSRuntime.InvokeAsync<int>("GetWidth");
            //ImageHeight = await JSRuntime.InvokeAsync<int>("GetHeight");
            //NavigationManager.
            //    NavigateTo($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{FatherLocationId}/opera/{Opera.Id}/descrizioni-dettagliate?Width={ImageWidth}&Height={ImageHeight}");
            //TERZO
            ImageWidth = await JSRuntime.InvokeAsync<int>("GetWidth");
            ImageHeight = await JSRuntime.InvokeAsync<int>("GetHeight");
            var query = new Dictionary<string, string>
            {
                {"imageWidht", ImageWidth.ToString() },
                {"imageHeight", ImageHeight.ToString() }
            };
            NavigationManager.NavigateTo(QueryHelpers.AddQueryString($"/strutture/{FatherBuildingId}/piani/{FatherLevelId}/locazioni-opere/{FatherLocationId}/opera/{Opera.Id}/descrizioni-dettagliate", query));
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

        //protected override async Task OnAfterRenderAsync(bool firstRender)
        //{
        //    if (firstRender)
        //    {
        //        ImageWidth = await JSRuntime.InvokeAsync<int>("GetWidth");
        //        ImageHeight = await JSRuntime.InvokeAsync<int>("GetHeight");
        //    }
            
        //}

        public void Dispose()
        {
            EventHandler.EventHandler -= HandleOnModifiedOpera;
        }
    }
}
