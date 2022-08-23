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

        public IEnumerable<IImageMap> OperaImageToDetailImageMaps { get; set; } = new List<IImageMap>();

        public string SelectedImageMapShape { get; set; } = ImageMapShape.Poly.ToString();

        public ImageMapShape[] ImageMapShapes = (ImageMapShape[])Enum.GetValues(typeof(ImageMapShape));

        public Message NotValidCoordinate = new Message();

        public Message MaxNumberOfCoordinates = new Message();  

        public Message ImageMapIsNotValid = new Message();    
        public ImageMapDTO ImageMapToAdd { get; set; }
            
        public LinkedList<ImageMapCoordinateDTO> imageMapCoordinates { get; set; }

        public void ManageShapeSelection(ChangeEventArgs e)
        {
            SelectedImageMapShape = e.Value.ToString();
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

        public double X { get; set; }

        public double Y { get; set; }

        public int Counter { get; set; } = 0;

        //public void verifica(MouseEventArgs e)
        //{
        //    X = e.ClientX;
        //    Y = e.ClientY;
        //}

        public void AddImageMapCoordinate(MouseEventArgs e)
        {   
            X = e.OffsetX;
            Y = e.OffsetY;
            Counter++;
            var imageMapCoordinate = new ImageMapCoordinateDTO(e.OffsetX, e.OffsetY);
            if (imageMapCoordinates.Contains(imageMapCoordinate))
            {
                ShowMessage(NotValidCoordinate);
                return;
            }
            if ( ! SelectedImageMapShape.Equals("Poly")) //Circle or Rect
            {
                if (imageMapCoordinates.Count == 2)
                {
                    ShowMessage(MaxNumberOfCoordinates);
                    return;
                }
                if (SelectedImageMapShape.Equals("Rect") && imageMapCoordinates.Count != 0)
                {
                    var firstCoordinate = imageMapCoordinates.First();
                    if (imageMapCoordinate.X == firstCoordinate.X
                        || imageMapCoordinate.Y == firstCoordinate.Y) //altrimenti esce un segmento
                    {
                        ShowMessage(NotValidCoordinate);
                        return;
                    }
                }
                imageMapCoordinate.Position = imageMapCoordinates.Count + 1;
                Console.WriteLine("coordinata aggiunta");   
                imageMapCoordinates.AddLast(imageMapCoordinate);
            }
            else //Poly
            {
                if (ThereIsOverlappingInPoly(imageMapCoordinate))
                {
                    Console.WriteLine("overlap");
                    ShowMessage(NotValidCoordinate);
                    return;
                }
                imageMapCoordinate.Position = imageMapCoordinates.Count + 1;
                Console.WriteLine("coordinata aggiunta -> x: " + imageMapCoordinate.X + "; y: " + imageMapCoordinate.Y);
                imageMapCoordinates.AddLast(imageMapCoordinate);
            }
        }



        private bool ThereIsOverlappingInPoly(ImageMapCoordinateDTO coordinateToAdd)
        {
            if (imageMapCoordinates.Count < 2)
                return false;

            if (imageMapCoordinates.Count == 2)
            {
                ThirdPointCase(coordinateToAdd);    
                return false;
            }

            imageMapCoordinates.OrderBy(c => c.Position);
            List<ImageMapCoordinateDTO[]> listOfCouples = GetVertexesCouples(coordinateToAdd);
            
            //var secondLastCouple = listOfCouples[listOfCouples.Count - 2];
            //Console.WriteLine("Penultima Coppia -> " + "Prima coordinata - X=" + secondLastCouple[0].X + ", Y=" + secondLastCouple[0].Y + " - Seconda Coordinata: - X=" + secondLastCouple[1].X + ", Y=" + secondLastCouple[1].Y);
            //var secondLastLine = StraightLineInTwoPointFinder
            //                        .FindStraightLine(new CartesianPoint(secondLastCouple[0].X, secondLastCouple[0].Y),
            //                                            new CartesianPoint(secondLastCouple[1].X, secondLastCouple[1].Y));
            //Console.WriteLine("Retta penultima coppia -> " + "a:" + secondLastLine.a + " b:" + secondLastLine.b + " c:" + secondLastLine.c);


            //var lastCouple = listOfCouples[listOfCouples.Count - 1];
            //Console.WriteLine("Ultima Coppia -> " + "Prima coordinata - X=" + lastCouple[0].X + ", Y=" + lastCouple[0].Y + " - Seconda Coordinata: - X=" + lastCouple[1].X + ", Y=" + lastCouple[1].Y);
            //var lastLine = StraightLineInTwoPointFinder
            //                .FindStraightLine(new CartesianPoint(lastCouple[0].X, lastCouple[0].Y),
            //                                    new CartesianPoint(lastCouple[1].X, lastCouple[1].Y));
            //Console.WriteLine("Retta ultima coppia -> " + "a:" + lastLine.a + " b:" + lastLine.b + " c:" + lastLine.c);


            for (int i = 0; i < listOfCouples.Count - 2; i++)
            {
                var intersectionCouple = listOfCouples[i];
                Console.WriteLine("Coppia intersezione -> " + "Prima coordinata - X=" + intersectionCouple[0].X + ", Y=" + intersectionCouple[0].Y + " - Seconda Coordinata: - X=" + intersectionCouple[1].X + ", Y=" + intersectionCouple[1].Y);
                var lineInIntersectionCouple = StraightLineInTwoPointFinder
                                                .FindStraightLine(new CartesianPoint(intersectionCouple[0].X, intersectionCouple[0].Y),
                                                                    new CartesianPoint(intersectionCouple[1].X, intersectionCouple[1].Y));
                Console.WriteLine("Retta coppia intersezione -> " + "a:" + lineInIntersectionCouple.a + " b:" + lineInIntersectionCouple.b + " c:" + lineInIntersectionCouple.c);

                if (i == 0) //primo cliclo
                {
                    Console.WriteLine("caso primo giro");

                    var secondLastCouple = listOfCouples[listOfCouples.Count - 2];
                    Console.WriteLine("Penultima Coppia -> " + "Prima coordinata - X=" + secondLastCouple[0].X + ", Y=" + secondLastCouple[0].Y + " - Seconda Coordinata: - X=" + secondLastCouple[1].X + ", Y=" + secondLastCouple[1].Y);
                    var secondLastLine = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(secondLastCouple[0].X, secondLastCouple[0].Y),
                                                                new CartesianPoint(secondLastCouple[1].X, secondLastCouple[1].Y));
                    Console.WriteLine("Retta penultima coppia -> " + "a:" + secondLastLine.a + " b:" + secondLastLine.b + " c:" + secondLastLine.c);

                    var intersectionPoint = IntersectionFinder
                                                .IntesectionBetween(secondLastLine, lineInIntersectionCouple);
                    Console.WriteLine("Punto intersezione ->" + "X=" + intersectionPoint.X + ", Y=" + intersectionPoint.Y );
                    //Console.WriteLine("numero lista coppie=" + listOfCouples.Count);
                    if (IntersectionPointIsNotValid(intersectionPoint, intersectionCouple))
                        return true;
                }
                else if (i == listOfCouples.Count - 3) //ultimo ciclo
                {
                    Console.WriteLine("caso ultimo giro");

                    var lastCouple = listOfCouples[listOfCouples.Count - 1];
                    Console.WriteLine("Ultima Coppia -> " + "Prima coordinata - X=" + lastCouple[0].X + ", Y=" + lastCouple[0].Y + " - Seconda Coordinata: - X=" + lastCouple[1].X + ", Y=" + lastCouple[1].Y);
                    var lastLine = StraightLineInTwoPointFinder
                                    .FindStraightLine(new CartesianPoint(lastCouple[0].X, lastCouple[0].Y),
                                                        new CartesianPoint(lastCouple[1].X, lastCouple[1].Y));
                    Console.WriteLine("Retta ultima coppia -> " + "a:" + lastLine.a + " b:" + lastLine.b + " c:" + lastLine.c);

                    var intersectionPoint = IntersectionFinder
                                               .IntesectionBetween(lastLine, lineInIntersectionCouple);
                    Console.WriteLine("Punto intersezione ->" + "X=" + intersectionPoint.X + ", Y=" + intersectionPoint.Y);
                    if (IntersectionPointIsNotValid(intersectionPoint, intersectionCouple))
                        return true;
                }
                else
                {
                    Console.WriteLine("altri casi");

                    var lastCouple = listOfCouples[listOfCouples.Count - 1];
                    Console.WriteLine("Ultima Coppia -> " + "Prima coordinata - X=" + lastCouple[0].X + ", Y=" + lastCouple[0].Y + " - Seconda Coordinata: - X=" + lastCouple[1].X + ", Y=" + lastCouple[1].Y);
                    var lastLine = StraightLineInTwoPointFinder
                                    .FindStraightLine(new CartesianPoint(lastCouple[0].X, lastCouple[0].Y),
                                                        new CartesianPoint(lastCouple[1].X, lastCouple[1].Y));
                    Console.WriteLine("Retta ultima coppia -> " + "a:" + lastLine.a + " b:" + lastLine.b + " c:" + lastLine.c);

                    var firstIntersectionPoint = IntersectionFinder
                                                    .IntesectionBetween(lastLine, lineInIntersectionCouple);
                    Console.WriteLine("Primo Punto intersezione ->" + "X=" + firstIntersectionPoint.X + ", Y=" + firstIntersectionPoint.Y);

                    var secondLastCouple = listOfCouples[listOfCouples.Count - 2];
                    Console.WriteLine("Penultima Coppia -> " + "Prima coordinata - X=" + secondLastCouple[0].X + ", Y=" + secondLastCouple[0].Y + " - Seconda Coordinata: - X=" + secondLastCouple[1].X + ", Y=" + secondLastCouple[1].Y);
                    var secondLastLine = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(secondLastCouple[0].X, secondLastCouple[0].Y),
                                                                new CartesianPoint(secondLastCouple[1].X, secondLastCouple[1].Y));
                    Console.WriteLine("Retta penultima coppia -> " + "a:" + secondLastLine.a + " b:" + secondLastLine.b + " c:" + secondLastLine.c);

                    var secondIntersectionPoint = IntersectionFinder
                                                    .IntesectionBetween(secondLastLine, lineInIntersectionCouple);
                    Console.WriteLine("Secondo Punto intersezione ->" + "X=" + secondIntersectionPoint.X + ", Y=" + secondIntersectionPoint.Y);
                    if (IntersectionPointIsNotValid(firstIntersectionPoint, intersectionCouple)
                        || IntersectionPointIsNotValid(secondIntersectionPoint, intersectionCouple))
                        return true;
                }

            }
            return false;

        }

        //se il terzo punto con il secondo formano la stessa retta delineata dal primo e secondo punto
        //elimino il secondo punto
        private void ThirdPointCase(ImageMapCoordinateDTO coordinateToAdd)
        {
            var coords = imageMapCoordinates.OrderBy(c => c.Position);
            var lineInAlreadyPresentPoints = StraightLineInTwoPointFinder
                                                .FindStraightLine(new CartesianPoint(coords.First().X, coords.First().Y),
                                                                    new CartesianPoint(coords.Last().X, coords.Last().Y));
            var lineInSecondAndNewPoint = StraightLineInTwoPointFinder
                                            .FindStraightLine(new CartesianPoint(coords.Last().X, coords.Last().Y),
                                                                new CartesianPoint(coordinateToAdd.X, coordinateToAdd.Y));
            if (lineInAlreadyPresentPoints.Equals(lineInSecondAndNewPoint))
                imageMapCoordinates.RemoveLast();
        }


        //Ottengo tutte le coppie di vertici in cui posso individuare rette
        private List<ImageMapCoordinateDTO[]> GetVertexesCouples(ImageMapCoordinateDTO coordinateToAdd)
        {
            List<ImageMapCoordinateDTO[]> listOfCouples = new List<ImageMapCoordinateDTO[]>();
            var coordinates = imageMapCoordinates.ToArray();
            coordinates.OrderBy(c => c.Position);
            for (int i = 0; i < coordinates.Length; i++)
            {
                ImageMapCoordinateDTO[] couple = new ImageMapCoordinateDTO[2];
                if (i == coordinates.Length - 1)
                {
                    couple[0] = coordinates[i];
                    couple[1] = coordinateToAdd;
                    listOfCouples.Add(couple);

                    ImageMapCoordinateDTO[] lastCouple = new ImageMapCoordinateDTO[2];
                    lastCouple[0] = coordinateToAdd;
                    lastCouple[1] = coordinates[0];
                    listOfCouples.Add(lastCouple);
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
                ShowMessage(ImageMapIsNotValid);
            }
            OperaImageToDetail = await OperaImageService.GetEntityByfatherRelationshipId(OperaToDetailId);
            OperaImageToDetailImageMaps = await ImageMapService.GetAllByfatherRelationshipId(OperaImageToDetail.Id);
            ImageMapToAdd = new ImageMapDTO();
            imageMapCoordinates.Clear();
        }

        private bool ImageMapToAddOverlapsWithOthers()
        {
            Console.WriteLine("faccio controllo");
            if (this.ImageMapToAdd.ImageMapShape.Equals("Circle"))
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
            Console.WriteLine("controllo circle overlapping");
            //var circleToAdd = GetCircleFrom(this.ImageMapToAdd);
            var operaToDetailImageMaps = OperaImageToDetail.ImageMaps;
            Console.WriteLine("elementi image map " + OperaImageToDetailImageMaps.Count());
            foreach (ImageMap imageMap in OperaImageToDetailImageMaps)
            {
                if (imageMap.ImageMapShape.Equals("Circle"))
                {
                    Console.WriteLine("due cerchi");
                    var circleFromImageMap = GetCircleFrom(imageMap);
                    var circleToAdd = GetCircleFrom(this.ImageMapToAdd);
                    var intersectionPoints = IntersectionFinder.IntesectionBetween(circleToAdd, circleFromImageMap);
                    
                    if (intersectionPoints.Count != 0)
                    {
                        Console.WriteLine("overlap 2 circonferenze");
                        return true;
                    }
                        
                }
                else
                {
                    Console.WriteLine("cerchio e qualcos'altro");
                    List<IImageMapCoordinate[]> vertexesCouples;
                    var straightLinesInImageMapVertexes = GetStraightLinesFrom(imageMap, out vertexesCouples);
                    for (int i = 0; i < straightLinesInImageMapVertexes.Count; i++)
                    {
                        double biggerX;
                        double smallerX;
                        AssignBiggerSmallerX(out biggerX, out smallerX, vertexesCouples[i]);
                        var circleToAdd = GetCircleFrom(this.ImageMapToAdd);
                        var intersectionPoints = IntersectionFinder.IntesectionBetween(straightLinesInImageMapVertexes[i], 
                                                                                        circleToAdd);
                        foreach(var point in intersectionPoints)
                        {
                            if (point.X >= smallerX && point.X <= biggerX)
                            {
                                Console.WriteLine("overlap tra cerchio e rect/poly");
                                return true;
                            }
                                
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
                    if (imageMap.ImageMapShape.Equals("Circle"))
                    {
                        var intersectionPoints = IntersectionFinder.IntesectionBetween(linesInImageMapToAdd[i], 
                                                                                        GetCircleFrom(imageMap));
                        foreach (var point in intersectionPoints)
                        {
                            if (point.X >= smallerX && point.X <= biggerX)
                            {
                                Console.WriteLine("overlap tra rect/poly e cerchio");
                                return true;
                            }
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
                            {
                                Console.WriteLine("overlap tra rect/poly e rect/poly");
                                return true;
                            }
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
            //coordinates.OrderBy(c => c.Position);
            var center = coordinates.OrderBy(c => c.Position).First();
            var circumferencePoint = coordinates.OrderBy(c => c.Position).Last();
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

            if (imageMap.ImageMapShape.Equals("Rect"))
            {
                vertexesCouples = GetVertexesCouples(AddTwoMissingVertexesToRect(vertexes));
            }
            else
            {
                vertexesCouples = GetVertexesCouples(vertexes);
            }

            foreach(var couple in vertexesCouples)
            {
                var firstPoint = new CartesianPoint(couple[0].X, couple[0].Y);
                var secondPoint = new CartesianPoint(couple[1].X, couple[1].Y);
                var straightLine = StraightLineInTwoPointFinder.FindStraightLine(firstPoint,secondPoint);
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

            var firstVertex = rectVertexes.OrderBy(c=>c.Position).First();
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

        private List<IImageMapCoordinate[]> GetVertexesCouples(ICollection<ImageMapCoordinate> imageMapsCoordinates)
        {
            List<IImageMapCoordinate[]> listOfCouples = new List<IImageMapCoordinate[]>();

            var coordinates = imageMapsCoordinates.OrderBy(c=>c.Position).ToArray();
            
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
            OperaImageToDetailImageMaps = await ImageMapService.GetAllByfatherRelationshipId(OperaImageToDetail.Id);
        }



    }
}
