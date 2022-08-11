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

        //public double X { get; set; }

        //public double Y { get; set; }

        //public void verifica(MouseEventArgs e)
        //{
        //    X = e.ClientX;
        //    Y = e.ClientY;
        //}

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
                        || imageMapCoordinte.Y == firstCoordinate.Y) //altrimenti esce un segmento
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
            List<ImageMapCoordinateDTO[]> listOfCouples = GetVertexesCouples(coordinateToAdd);
            for (int i = 0; i < listOfCouples.Count; i++)
            {
                var coupleToCheck = listOfCouples[i];
                var straightLineInCouple = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(coupleToCheck[0].X, coupleToCheck[0].Y),
                                                                new CartesianPoint(coupleToCheck[1].X, coupleToCheck[1].Y));
                for (int j = 0; j < listOfCouples.Count; j++)
                {
                    if (i == j)
                        continue;
                    var coupleOfHypotheticalIntersection = listOfCouples[j];    
                    var straightLineInIntersectionCouple = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(coupleOfHypotheticalIntersection[0].X, coupleOfHypotheticalIntersection[0].Y),
                                                                new CartesianPoint(coupleOfHypotheticalIntersection[1].X, coupleOfHypotheticalIntersection[1].Y));
                    var intersectionPoint = IntersectionFinder
                                            .IntesectionBetween(straightLineInCouple, straightLineInIntersectionCouple);
                    if (IntersectionPointIsNotValid(intersectionPoint, coupleOfHypotheticalIntersection))
                        return true;
                }
            }
            return false;
        }

        //Ottengo tutte le coppie di vertici in cui posso individuare rette
        private List<ImageMapCoordinateDTO[]> GetVertexesCouples(ImageMapCoordinateDTO coordinateToAdd)
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
            ImageMapToAdd.ImageMapCoordinates = imageMapCoordinates;
            if ( ! ImageMapToAddOverlapsWithOthers() ) // se l'image map e' valido
            {
                await ImageMapService.AddEntity(ImageMapToAdd);
            }
            else
            {
                ShowMessage(ImageMapIsOverlapped);
            }
            ImageMapToAdd = new ImageMapDTO();
            imageMapCoordinates.Clear();
        }

        private bool ImageMapToAddOverlapsWithOthers()
        {
            if (this.ImageMapToAdd.ImageMapShape.Equals(ImageMapShape.Circle))
            {
                return CircleOverlappingSearch();
            }
            else
            {
                return PolyRectOverlappingSearch();
            }
        }


        private bool CircleOverlappingSearch()
        {
            var circleToAdd = GetCircleFrom(this.ImageMapToAdd);
            var operaToDetailImageMaps = OperaImageToDetail.ImageMaps;
            foreach (ImageMap imageMap in operaToDetailImageMaps)
            {
                if (imageMap.ImageMapShape.Equals(ImageMapShape.Circle))
                {
                    var intersectionPoints = IntersectionFinder.IntesectionBetween(circleToAdd, GetCircleFrom(imageMap));
                    if (intersectionPoints.Count != 0)
                        return true;
                }
                else
                {
                    List<IImageMapCoordinate[]> vertexesCouples;
                    var straightLinesInImageMapVertexes = GetStraightLinesFrom(imageMap, out vertexesCouples);
                    for (int i = 0; i < straightLinesInImageMapVertexes.Count; i++)
                    {
                        double biggerX;
                        double smallerX;
                        AssignBiggerSmallerX(out biggerX, out smallerX, vertexesCouples[i]);
                        var intersectionPoints = IntersectionFinder.IntesectionBetween(straightLinesInImageMapVertexes[i], 
                                                                                        circleToAdd);
                        foreach(var point in intersectionPoints)
                        {
                            if (point.X >= smallerX && point.X <= biggerX)
                                return true;
                        }
                    }
                }
            }
            return false;
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

        private bool PolyRectOverlappingSearch()
        {
            var operaToDetailImageMaps = OperaImageToDetail.ImageMaps;

            List<ImageMapCoordinateDTO[]> vertexesCouples;
            var linesInImageMapToAdd = GetStraightLinesFrom(this.ImageMapToAdd, out vertexesCouples);

            for (int i = 0; i < linesInImageMapToAdd.Count; i++)
            {
                double biggerX;
                double smallerX;
                AssignBiggerSmallerX(out biggerX, out smallerX, vertexesCouples[i]);
                foreach (ImageMap imageMap in operaToDetailImageMaps)
                {
                    if (imageMap.ImageMapShape.Equals(ImageMapShape.Circle))
                    {
                        var intersectionPoints = IntersectionFinder.IntesectionBetween(linesInImageMapToAdd[i], 
                                                                                        GetCircleFrom(imageMap));
                        foreach (var point in intersectionPoints)
                        {
                            if (point.X >= smallerX && point.X <= biggerX)
                                return true;
                        }
                    }
                    else
                    {
                        var linesInDBImageMap = GetStraightLinesFrom(imageMap);
                        foreach(var lineDBImageMap in linesInDBImageMap)
                        {
                            var intersectionPoint = IntersectionFinder.IntesectionBetween(linesInImageMapToAdd[i],
                                                                                            lineDBImageMap);
                            if (intersectionPoint.X >= smallerX && intersectionPoint.X <= biggerX)
                                return true;
                        }
                    }
                }

            }

            return false;
        }

        private void AssignBiggerSmallerX(out double biggerX,
                                            out double smallerX,
                                            ImageMapCoordinateDTO[] imageMapCoordinates)
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



        private ImplicitFormcCircumference GetCircleFrom(IImageMap imageMap)
        {
            ICollection<ImageMapCoordinate> coordinates = imageMap.ImageMapCoordinates;
            coordinates.OrderBy(c => c.Position);
            var center = coordinates.First();
            var circumferencePoint = coordinates.Last();
            var circle = CircumferenceFinder.FindWith(new CartesianPoint(center.X, center.Y),
                                                        new CartesianPoint(circumferencePoint.X, circumferencePoint.Y));
            return circle;
        }
        

        private List<ImplicitFormStraightLine> GetStraightLinesFrom(IImageMap imageMap, 
                                                                    out List<IImageMapCoordinate[]> vertexesCouples)
        {
            List<ImplicitFormStraightLine> straightLines = new List<ImplicitFormStraightLine>();

            //List<IImageMapCoordinate[]> vertexesCouples;

            ICollection<ImageMapCoordinate> vertexes = imageMap.ImageMapCoordinates;
            vertexes.OrderBy(c => c.Position);
            if (imageMap.ImageMapShape.Equals(ImageMapShape.Rect))
            {
                vertexesCouples = GetVertexesCouples(AddTwoMissingVertexesToRect(vertexes));
            }
            else
            {
                vertexesCouples = GetVertexesCouples(vertexes);
            }

            foreach(var couple in vertexesCouples)
            {
                var straightLine = StraightLineInTwoPointFinder
                                    .FindStraightLine(new CartesianPoint(couple[0].X, couple[0].Y),
                                                        new CartesianPoint(couple[1].X, couple[1].Y));
                straightLines.Add(straightLine);
            }

            return straightLines;

        }

        private List<ImplicitFormStraightLine> GetStraightLinesFrom(IImageMap imageMap)
        {
            List<ImplicitFormStraightLine> straightLines = new List<ImplicitFormStraightLine>();

            List<IImageMapCoordinate[]> vertexesCouples;

            ICollection<ImageMapCoordinate> vertexes = imageMap.ImageMapCoordinates;
            vertexes.OrderBy(c => c.Position);
            if (imageMap.ImageMapShape.Equals(ImageMapShape.Rect))
            {
                vertexesCouples = GetVertexesCouples(AddTwoMissingVertexesToRect(vertexes));
            }
            else
            {
                vertexesCouples = GetVertexesCouples(vertexes);
            }

            foreach (var couple in vertexesCouples)
            {
                var straightLine = StraightLineInTwoPointFinder
                                    .FindStraightLine(new CartesianPoint(couple[0].X, couple[0].Y),
                                                        new CartesianPoint(couple[1].X, couple[1].Y));
                straightLines.Add(straightLine);
            }

            return straightLines;

        }

        private ICollection<ImageMapCoordinate> AddTwoMissingVertexesToRect(ICollection<ImageMapCoordinate> rectVertexes)
        {
            ICollection<ImageMapCoordinate> vertexes = new LinkedList<ImageMapCoordinate>();

            var firstVertex = rectVertexes.First();
            firstVertex.Position = 1;
            var oppositeVertex = rectVertexes.Last();
            oppositeVertex.Position = 3;

            var fistMissingVertex = new ImageMapCoordinate(oppositeVertex.X, firstVertex.Y);
            fistMissingVertex.Position = 2;
            var secondMissingVertex = new ImageMapCoordinate(firstVertex.X, oppositeVertex.Y);
            secondMissingVertex.Position = 4;

            vertexes.Append(firstVertex);
            vertexes.Append(fistMissingVertex);
            vertexes.Append(oppositeVertex);
            vertexes.Append(secondMissingVertex);

            return vertexes;
        }

        private List<IImageMapCoordinate[]> GetVertexesCouples(ICollection<ImageMapCoordinate> imageMapsCoordinates)
        {
            List<IImageMapCoordinate[]> listOfCouples = new List<IImageMapCoordinate[]>();

            var coordinates = imageMapsCoordinates.ToArray();
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




        private ImplicitFormcCircumference GetCircleFrom(ImageMapDTO imageMapToAdd)
        {
            LinkedList<ImageMapCoordinateDTO> coordinates = imageMapToAdd.ImageMapCoordinates;
            coordinates.OrderBy(c => c.Position);
            var center = coordinates.First();
            var circumferencePoint = coordinates.Last();
            var circle = CircumferenceFinder.FindWith(new CartesianPoint(center.X, center.Y),
                                                        new CartesianPoint(circumferencePoint.X, circumferencePoint.Y));
            return circle;
        }

        private List<ImplicitFormStraightLine> GetStraightLinesFrom(ImageMapDTO imageMapToAdd, 
                                                                    out List<ImageMapCoordinateDTO[]> vertexesCouples)
        {
            List<ImplicitFormStraightLine> straightLines = new List<ImplicitFormStraightLine>();

            //List<ImageMapCoordinateDTO[]> vertexesCouples;

            LinkedList<ImageMapCoordinateDTO> vertexes = imageMapToAdd.ImageMapCoordinates;
            vertexes.OrderBy(c => c.Position);
            if (imageMapToAdd.ImageMapShape.Equals(ImageMapShape.Rect))
            {
                vertexesCouples = GetVertexesCouples(AddTwoMissingVertexesToRect(vertexes));
            }
            else
            {
                vertexesCouples = GetVertexesCouples(vertexes);
            }

            foreach (var couple in vertexesCouples)
            {
                var straightLine = StraightLineInTwoPointFinder
                                    .FindStraightLine(new CartesianPoint(couple[0].X, couple[0].Y),
                                                        new CartesianPoint(couple[1].X, couple[1].Y));
                straightLines.Add(straightLine);
            }

            return straightLines;

        }


        private LinkedList<ImageMapCoordinateDTO> AddTwoMissingVertexesToRect(LinkedList<ImageMapCoordinateDTO> rectVertexes)
        {
            LinkedList<ImageMapCoordinateDTO> vertexes = new LinkedList<ImageMapCoordinateDTO>();

            var firstVertex = rectVertexes.First();
            firstVertex.Position = 1;
            var oppositeVertex = rectVertexes.Last();
            oppositeVertex.Position = 3;

            var fistMissingVertex = new ImageMapCoordinateDTO(oppositeVertex.X, firstVertex.Y);
            fistMissingVertex.Position = 2;
            var secondMissingVertex = new ImageMapCoordinateDTO(firstVertex.X, oppositeVertex.Y);
            secondMissingVertex.Position = 4;

            vertexes.AddLast(firstVertex);
            vertexes.AddLast(fistMissingVertex);
            vertexes.AddLast(oppositeVertex);
            vertexes.AddLast(secondMissingVertex);

            return vertexes;
        }



        private List<ImageMapCoordinateDTO[]> GetVertexesCouples(LinkedList<ImageMapCoordinateDTO> imageMapsCoordinates)
        {
            List<ImageMapCoordinateDTO[]> listOfCouples = new List<ImageMapCoordinateDTO[]>();

            var coordinates = imageMapsCoordinates.ToArray();
            for (int i = 0; i < coordinates.Length; i++)
            {
                ImageMapCoordinateDTO[] couple = new ImageMapCoordinateDTO[2];
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

        protected override async Task OnInitializedAsync()
        {
            ImageMapToAdd = new ImageMapDTO();
            imageMapCoordinates = new LinkedList<ImageMapCoordinateDTO>();
            OperaImageToDetail = await OperaImageService.GetEntityByfatherRelationshipId(OperaToDetailId);
        }



    }
}
