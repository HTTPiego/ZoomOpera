using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Timers;
using ZoomOpera.CartersianPlane;
using ZoomOpera.Client.Entities;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Services.Interfaces;
using ZoomOpera.Client.Utils;
using ZoomOpera.DTOs;
using Timer = System.Timers.Timer;

namespace ZoomOpera.Client.Shared.SharedComponetBaseClasses
{
    public class AddOperaDetailsBase : ComponentBase
    {
        [Inject]
        IService<IOperaImage, OperaImageDTO> OperaImageService { get; set; }

        [Inject]
        IService<IImageMap, ImageMapDTO> ImageMapService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }

        [Parameter]
        public Guid FatherBuildingId { get; set; }

        [Parameter]
        public Guid FatherLevelId { get; set; }

        [Parameter]
        public Guid FatherLocationId { get; set; }

        [Parameter]
        public Guid OperaToDetailId { get; set; }

        public IOperaImage OperaImageToDetail { get; set; } = new OperaImage();

        public ImageMapShape SelectedImageMapShape { get; set; }

        public ImageMapShape[] ImageMapShapes = (ImageMapShape[])Enum.GetValues(typeof(ImageMapShape));

        public Message NotValidCoordinate = new Message();

        public Message MaxNumberOfCoordinates = new Message();  

        public Message ImageMapIsOverlapped = new Message();    
        public ImageMapDTO ImageMapToAdd { get; set; }
            
        public LinkedList<ImageMapCoordinateDTO> imageMapCoordinates { get; set; }

        public void ManageShapeSelection(ChangeEventArgs e)
        {
            SelectedImageMapShape = (ImageMapShape)Enum.Parse(typeof(ImageMapShape), e.Value.ToString());
            if (imageMapCoordinates.Count != 0)
                imageMapCoordinates.Clear();
        }

        private void ShowMessage(Message message)
        {
            message.ShowMessage = true;
            Timer timer = new Timer(3000);
            timer.Elapsed += (s, e) => OnTimerElapsed(s, e, message);
            timer.Enabled = true;
            timer.AutoReset = false;
            timer.Start();
        }

        private void OnTimerElapsed(object sender, ElapsedEventArgs e, Message message)
        {
            message.ShowMessage = false;
        }

        public void AddImageMapCoordinate(MouseEventArgs e)
        {
            var imageMapCoordinte = new ImageMapCoordinateDTO(e.ClientX, e.ClientY);
            if (imageMapCoordinates.Contains(imageMapCoordinte))
            {
                ShowMessage(NotValidCoordinate);
                return;
            }
            if ( !SelectedImageMapShape.Equals(ImageMapShape.Poly) ) //Circle or Rect
            {
                if (imageMapCoordinates.Count == 2)
                {
                    ShowMessage(MaxNumberOfCoordinates);
                    return;
                }
                if (SelectedImageMapShape.Equals(ImageMapShape.Rect))
                {
                    var firstCoordinate = imageMapCoordinates.First();
                    if (imageMapCoordinte.X == firstCoordinate.X
                        || imageMapCoordinte.Y == firstCoordinate.Y)
                    {
                        ShowMessage(NotValidCoordinate);
                        return;
                    }
                }
                imageMapCoordinte.Position = imageMapCoordinates.Count + 1;
                imageMapCoordinates.AddLast(imageMapCoordinte);
            }
            else //Poly
            {
                if (ThereIsOverlappingInPoly(imageMapCoordinte))
                {
                    ShowMessage(NotValidCoordinate);
                    return;
                }
                imageMapCoordinte.Position = imageMapCoordinates.Count + 1;
                imageMapCoordinates.AddLast(imageMapCoordinte);
            }
        }

        private bool ThereIsOverlappingInPoly(ImageMapCoordinateDTO coordinateToAdd)
        {
           if(imageMapCoordinates.Count < 2)
                return false;
            imageMapCoordinates.OrderBy(c => c.Position);
            List<ImageMapCoordinateDTO[]> listOfCouples = GetCoordinatesCouples(coordinateToAdd);
            bool thereIsOverlapping = false;
            for (int i = 0; i < listOfCouples.Count; i++)
            {
                if (thereIsOverlapping)
                    break;
                var coupleToCheck = listOfCouples[i];
                var straightLineInCouple = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(coupleToCheck[0].X, coupleToCheck[0].Y),
                                                                new CartesianPoint(coupleToCheck[1].X, coupleToCheck[1].Y));
                for (int j = 0; j < listOfCouples.Count; j++)
                {
                    if (thereIsOverlapping)
                        break;
                    if (i == j)
                        continue;
                    var coupleOfHypotheticalIntersection = listOfCouples[j];    
                    var straightLineInIntersectionCouple = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(coupleOfHypotheticalIntersection[0].X, coupleOfHypotheticalIntersection[0].Y),
                                                                new CartesianPoint(coupleOfHypotheticalIntersection[1].X, coupleOfHypotheticalIntersection[1].Y));
                    var intersectionPoint = IntersectionFinder
                                            .IntesectionBetween(straightLineInCouple, straightLineInIntersectionCouple);
                    if (IntersectionPointIsNotValid(intersectionPoint, coupleOfHypotheticalIntersection))
                        thereIsOverlapping = true;
                }
            }
            return thereIsOverlapping;
        }

        //Ottengo tutte le coppie di vertici in cui posso individuare rette
        private List<ImageMapCoordinateDTO[]> GetCoordinatesCouples(ImageMapCoordinateDTO coordinateToAdd)
        {
            List<ImageMapCoordinateDTO[]> listOfCouples = new List<ImageMapCoordinateDTO[]>();
            var coordinates = imageMapCoordinates.ToArray();
            for (int i = 0; i < coordinates.Length; i++)
            {
                ImageMapCoordinateDTO[] couple = new ImageMapCoordinateDTO[2];
                if (i == coordinates.Length - 1)
                {
                    couple[0] = coordinates[i];
                    couple[1] = coordinateToAdd;
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

        private bool IntersectionPointIsNotValid(CartesianPoint intersectionPoint, 
                                                    ImageMapCoordinateDTO[] coupleOfHypotheticalIntersection)
        {
            var firstCoordinate = coupleOfHypotheticalIntersection[0];
            var secondCoordinate = coupleOfHypotheticalIntersection[1];
            double biggerX;
            double smallerX;
            //non e necessario controllare le Y
            if (firstCoordinate.X >= secondCoordinate.X)
            {
                biggerX = firstCoordinate.X;
                smallerX = secondCoordinate.X;
            }
            else
            {
                biggerX = secondCoordinate.X;
                smallerX= firstCoordinate.X;
            }
            if (intersectionPoint.X >= smallerX && intersectionPoint.X <= biggerX)
                return true;
            return false;
        }

        public async Task AddImageMap()
        {
            ImageMapToAdd.ImageMapShape = SelectedImageMapShape;
            ImageMapToAdd.OperaImageId = OperaImageToDetail.Id;
            ImageMapToAdd.ImgeMapCoordinates = imageMapCoordinates;
            if (ImageMapOverlapsWithOthers(ImageMapToAdd))
            {
                ShowMessage(ImageMapIsOverlapped);
                return;
            }
            await ImageMapService.AddEntity(ImageMapToAdd);
            ImageMapToAdd = new ImageMapDTO();
            imageMapCoordinates.Clear();
        }

        private bool ImageMapOverlapsWithOthers(ImageMapDTO imageMapToAdd)
        {
            var operaToDetailImageMaps = OperaImageToDetail.ImageMaps;
            foreach (var imageMap in operaToDetailImageMaps)
            {
                var shape = imageMap.ImageMapShape;
                if (shape.Equals(ImageMapShape.Circle))
                {

                }
                else
                {

                }
            }

            return true;
        }

        protected override async Task OnInitializedAsync()
        {
            ImageMapToAdd = new ImageMapDTO();
            imageMapCoordinates = new LinkedList<ImageMapCoordinateDTO>();
            OperaImageToDetail = await OperaImageService.GetEntityByfatherRelationshipId(OperaToDetailId);
        }



    }
}
