using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using ZoomOpera.CartersianPlane;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils;
using ZoomOpera.DTOs;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Shared.Entities;
using ImageMapCoordinate = ZoomOpera.Client.Entities.ImageMapCoordinate;
using Opera = ZoomOpera.Client.Entities.Opera;
using OperaImage = ZoomOpera.Client.Entities.OperaImage;

namespace ZoomOpera.Client.Pages.PagesBaseComponentsClasses
{
    public class ViewOperaBase : ComponentBase
    {
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected IService<IOpera, OperaDTO> OperaToViewService { get; set; }

        [Inject]
        protected IService<IOperaImage, OperaImageDTO> ImageService { get; set; }

        [Inject]
        protected IService<IImageMap, ImageMapDTO> ImageMapsService { get; set; }

        [Inject]
        protected IService<IImageMapCoordinate, ImageMapCoordinateDTO> CoordsService { get; set; }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Parameter]
        public Guid OperaToViewId { get; set; }

        public IOpera OperaToView { get; set; } = new Opera();

        public IOperaImage Image { get; set; } = new OperaImage();

        public List<IImageMap> ImageMaps { get; set; } = new List<IImageMap>();

        public List<IImageMapCoordinate> Coords { get; set; } = new List<IImageMapCoordinate>();

        public Guid SelectedImageMapId { get; set; }

        public bool ReadDetails { get; set; }

        public bool CanvasIsOpen { get; set; }

        public ElementReference OperaImage { get; set; }

        public int ImageHeight { get; set; }

        public int ImageWidth { get; set; }

        protected BECanvasComponent? _canvasReference;

        private CanvasDrawer _canvasDrawer;

        public string linkDetails = string.Empty;

        public bool HideOperaImage { get; set; }

        public bool ScopriDove { get; set; }

        public void OnReadDetails(Guid imageMapId)
        {
            this.ReadDetails = true;
            SelectedImageMapId = imageMapId;
            //NavigationManager.NavigateTo($"https://localhost:7288/descrizione-dettagliata/{imageMapId}");
            //linkDetails = $"https://localhost:7288/descrizione-dettagliata/{imageMapId}";
        }   


        public string GetImageMapCoords(IImageMap imageMap)
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

        public void OpenDetails(MouseEventArgs e)
        {
            var selectedImgMap = ClickIsInsideImgMap(e);
            if (selectedImgMap != null)
            {
                SelectedImageMapId = selectedImgMap.Id;
                ReadDetails = true;
                _canvasDrawer = new CanvasDrawer(_canvasReference, OperaImage, ImageHeight, ImageWidth);
                _canvasDrawer.ZoomOnPoint(new CartesianPoint(e.OffsetX, e.OffsetY));
            }
        }

        private IImageMap? ClickIsInsideImgMap(MouseEventArgs e)
        {
            Console.WriteLine("X="+e.OffsetX+" Y="+e.OffsetY);
            for (int i = 0; i < ImageMaps.Count; i++)
            {
                var imgmap = ImageMaps[i];
                var clickPoint = new CartesianPoint(e.OffsetX, e.OffsetY);
                var linePoint = new CartesianPoint(e.OffsetX - 1, e.OffsetY);
                var lineFormClick = StraightLineInTwoPointFinder.FindStraightLine(linePoint, clickPoint);
                Console.WriteLine("a=" + lineFormClick.a + " b=" + lineFormClick.b + " c=" + lineFormClick.c);
                if (imgmap.ImageMapShape.Equals("Circle"))
                {
                    var center = new CartesianPoint(imgmap.ImageMapCoordinates.First().X, imgmap.ImageMapCoordinates.First().Y);
                    var circPoint = new CartesianPoint(imgmap.ImageMapCoordinates.Last().X, imgmap.ImageMapCoordinates.Last().Y);
                    var circumference = CircumferenceFinder.FindWith(center, circPoint);

                    var intersectionPoints = IntersectionFinder.IntesectionBetween(lineFormClick, circumference);
                    if (intersectionPoints.Count == 1)
                    {
                        Console.WriteLine("YES");
                        return imgmap;
                    }
                        
                }
                else
                {
                    int intersections = 0;
                    List<IImageMapCoordinate[]> vertexesCouplesDbImageMap;
                    var linesInDBImageMap = GetStraightLines(imgmap, out vertexesCouplesDbImageMap);
                    int index = 0;
                    foreach (var line in linesInDBImageMap)
                    {
                        if (intersections == 2)
                            break;
                        var intersectionPoint = IntersectionFinder.IntesectionBetween(lineFormClick, line);
                        Console.WriteLine("Giro --> " + index + " / intersezione - X=" + intersectionPoint.X + " Y=" + intersectionPoint.Y);
                        double biggerX;
                        double smallerX;
                        AssignBiggerSmallerX(out biggerX, out smallerX, vertexesCouplesDbImageMap[index]);
                        if ((intersectionPoint.X > smallerX && intersectionPoint.X < biggerX) 
                            && intersectionPoint.X > clickPoint.X
                            && IntersectionPointIsNotAVertex(intersectionPoint, imgmap))
                        {
                            intersections++;
                        }
                        index++;
                    }
                    if (intersections == 1)
                    {
                        Console.WriteLine("YES");
                        return imgmap;
                    }
                }
            }
            Console.WriteLine("NOO");
            return null;
        }

        private bool IntersectionPointIsNotAVertex(CartesianPoint intersectionPoint, IImageMap imgmap)
        {
            foreach(var vertex in imgmap.ImageMapCoordinates)
            {
                if (vertex.Equals(intersectionPoint))
                    return false;
            }
            return true;
        }

        private List<ImplicitFormStraightLine> GetStraightLines(IImageMap imageMap,
                                                                    out List<IImageMapCoordinate[]> vertexesCouples)
        {
            List<ImplicitFormStraightLine> straightLines = new List<ImplicitFormStraightLine>();

            //List<IImageMapCoordinate[]> vertexesCouplesImageMapToAdd;

            var vertexes = imageMap.ImageMapCoordinates;

            if (imageMap.ImageMapShape.Equals("Rect"))
            {
                vertexesCouples = GetVertexesCouples(AddTwoMissingVertexesToRect(vertexes));
            }
            else
            {
                vertexesCouples = GetVertexesCouples(vertexes);
            }

            foreach (var couple in vertexesCouples)
            {
                var firstPoint = new CartesianPoint(couple[0].X, couple[0].Y);
                var secondPoint = new CartesianPoint(couple[1].X, couple[1].Y);
                var straightLine = StraightLineInTwoPointFinder.FindStraightLine(firstPoint, secondPoint);
                straightLines.Add(straightLine);
            }

            return straightLines;

        }

        private List<IImageMapCoordinate[]> GetVertexesCouples(ICollection<ImageMapCoordinate> imageMapsCoordinates)
        {
            List<IImageMapCoordinate[]> listOfCouples = new List<IImageMapCoordinate[]>();

            var coordinates = imageMapsCoordinates.OrderBy(c => c.Position).ToArray();

            for (int i = 0; i < coordinates.Length; i++)
            {
                IImageMapCoordinate[] couple = new IImageMapCoordinate[2];
                if (i == coordinates.Length - 1)
                {
                    couple[0] = coordinates[i];
                    couple[1] = coordinates[0];

                    listOfCouples.Add(couple);
                }
                else
                {
                    couple[0] = coordinates[i];
                    couple[1] = coordinates[i + 1];

                    listOfCouples.Add(couple);
                }
            }
            return listOfCouples;
        }

        private ICollection<ImageMapCoordinate> AddTwoMissingVertexesToRect(ICollection<ImageMapCoordinate> rectVertexes)
        {
            ICollection<ImageMapCoordinate> vertexes = new LinkedList<ImageMapCoordinate>();

            var firstVertex = rectVertexes.OrderBy(c => c.Position).First();
            firstVertex.Position = 1;
            var oppositeVertex = rectVertexes.OrderBy(c => c.Position).Last();
            oppositeVertex.Position = 3;

            var fistMissingVertex = new ImageMapCoordinate(oppositeVertex.X, firstVertex.Y);
            fistMissingVertex.Position = 2;
            var secondMissingVertex = new ImageMapCoordinate(firstVertex.X, oppositeVertex.Y);
            secondMissingVertex.Position = 4;

            vertexes.Add(firstVertex);
            vertexes.Add(fistMissingVertex);
            vertexes.Add(oppositeVertex);
            vertexes.Add(secondMissingVertex);

            return vertexes;
        }

        private void AssignBiggerSmallerX(out double biggerX,
                                            out double smallerX,
                                            IImageMapCoordinate[] imageMapCoordinates)
        {
            var firstVertex = imageMapCoordinates[0];
            var secondVertex = imageMapCoordinates[1];

            if (firstVertex.X >= secondVertex.X)
            {
                biggerX = firstVertex.X;
                smallerX = secondVertex.X;
            }
            else
            {
                biggerX = secondVertex.X;
                smallerX = firstVertex.X;
            }

        }

        public async Task OpenCanvasAsync()
        {
            CanvasIsOpen = true;
            StateHasChanged();
            await Task.Yield();
            var x = _canvasReference;
            _canvasDrawer = new CanvasDrawer(x, OperaImage, ImageHeight, ImageWidth);
            _canvasDrawer.Draw(ImageMaps.ToList());
        }

        public void ShowAreas()
        {
            ScopriDove = true;
            _canvasDrawer = new CanvasDrawer(_canvasReference, OperaImage, 500, 500);
            _canvasDrawer.Draw(this.ImageMaps);
        }

        public void CancelAreas()
        {
            ScopriDove = false;
            _canvasDrawer = new CanvasDrawer(_canvasReference, OperaImage, 500, 500);
            _canvasDrawer.DrawJustImage();
        }

        public void CloseCanvas()
        {
            CanvasIsOpen = false;
        }

        public void CloseDetails()
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
            ImageMaps = await ImageMapsService.GetAllByfatherRelationshipId(Image.Id)
                                                .ContinueWith(imgmaps => ImageMaps = imgmaps.Result.ToList());
            Coords = await GetCoordinates();
            ReadDetails = false;
            CanvasIsOpen = false;
            HideOperaImage = true;
            ScopriDove = false;

        //ImageHeight = await JSRuntime.InvokeAsync<int>("GetHeight");
        //ImageWidth = await JSRuntime.InvokeAsync<int>("GetWidth");

        }   

        protected override void OnAfterRender(bool firstRender)
        {
            //var x = _canvasReference;
            _canvasDrawer = new CanvasDrawer(_canvasReference, OperaImage, 500, 500);
            _canvasDrawer.DrawJustImage();
            //ImageHeight = await JSRuntime.InvokeAsync<int>("GetHeight");
            //ImageWidth = await JSRuntime.InvokeAsync<int>("GetWidth");
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
