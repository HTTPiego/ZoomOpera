using Microsoft.AspNetCore.Components;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class ViewOperaBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IService<IOpera,OperaDTO> OperaToViewService { get; set; }

        [Inject]
        protected IService<IOperaImage, OperaImageDTO> ImageService { get; set; }

        [Inject]
        protected IService<IImageMap, ImageMapDTO> ImageMapsService { get; set; }

        [Inject]
        protected IService<IImageMapCoordinate, ImageMapCoordinateDTO> CoordsService { get; set; }

        [Parameter]
        public Guid OperaToViewId { get; set; }

        public IOpera OperaToView { get; set; } = new Opera();

        public IOperaImage Image { get; set; } = new OperaImage();

        public IEnumerable<IImageMap> ImageMaps { get; set; } = new List<IImageMap>();

        public List<IImageMapCoordinate> Coords { get; set; } = new List<IImageMapCoordinate>();

        public Guid SelectedImageMapId { get; set; }

        public bool ReadDetails { get; set; }

        public string linkDetails = string.Empty;

        public object OnReadDetails(Guid imageMapId)
        {
            this.ReadDetails = true;
            SelectedImageMapId = imageMapId;
            NavigationManager.NavigateTo($"https://localhost:7288/descrizione-dettagliata/{imageMapId}");
            //linkDetails = $"https://localhost:7288/descrizione-dettagliata/{imageMapId}";
            return null;
        }


        public string Prova(IImageMap imageMap)
        {
            var coords = Coords.FindAll(c => c.ImageMapId.Equals(imageMap.Id)).OrderBy(c => c.Position).ToArray();
            string coordToString = string.Empty;
            //foreach (var coord in coords)
            //{
            //    coordToString += coord.X.ToString() + "," + coord.Y.ToString() + ", ";
            //}
            for(int i = 0; i < coords.Length; i++)
            {
                if (i == coords.Length - 1) //ultimo giro
                {
                    coordToString += coords[i].X.ToString() + "," + coords[i].Y.ToString();
                }
                else
                {
                    coordToString += coords[i].X.ToString() + "," + coords[i].Y.ToString() + ",";
                }
            }

            return coordToString;
        }

        public void ChiudiDettagli()
        {
            ReadDetails = false;
        }

        //public void ReadDetailedOperaDescription(Guid imageMapId)
        //{
        //    NavigationManager.NavigateTo($"/visualizza-opere-piano/{OperaToViewId}/descrizione-dettagliata_{imageMapId}");
        //}

        protected override async Task OnInitializedAsync()
        {
            OperaToView = await OperaToViewService.GetEntity(OperaToViewId);
            Image = await ImageService.GetEntityByfatherRelationshipId(OperaToViewId);
            ImageMaps = await ImageMapsService.GetAllByfatherRelationshipId(Image.Id);
            Coords = await GetCoordinates();
            ReadDetails = false;
        }

        private async Task<List<IImageMapCoordinate>> GetCoordinates()
        {
            List<IImageMapCoordinate> coords = new List<IImageMapCoordinate>();

            foreach(var imageMap in ImageMaps)
            {
                var imageMapCoords = await CoordsService.GetAllByfatherRelationshipId(imageMap.Id);
                coords.AddRange(imageMapCoords);
            }

            return coords;
        }
    }
}
