using Blazor.Extensions.Canvas.Canvas2D;
using Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.CartersianPlane;
using ZoomOpera.DTOs;
using System.Collections.Generic;

namespace ZoomOpera.Client.Utils
{
    public class CanvasDrawer
    {
        Canvas2DContext _context;
        private BECanvasComponent _canvasReference;
        private ElementReference OperaImage;
        private int ImageHeight;
        private int ImageWidth;

        public CanvasDrawer(BECanvasComponent canvasReference, ElementReference operaImage, int imageHeight, int imageWidth)
        {
            _canvasReference = canvasReference;
            OperaImage = operaImage;
            ImageHeight = imageHeight;
            ImageWidth = imageWidth;
        }

        public async void Draw(List<IImageMap> imgmps)
        {
            _context = await _canvasReference.CreateCanvas2DAsync();

            await _context.DrawImageAsync(OperaImage, 0, 0);


            await DrawCanvasAreaBordes();


            await DrawDBImageMaps(imgmps);
        }

        private async Task DrawCanvasAreaBordes()
        {
            await _context.BeginPathAsync();

            await _context.MoveToAsync(0, 0);
            await _context.LineToAsync(0, ImageHeight);
            await _context.LineToAsync(ImageWidth, ImageHeight);
            await _context.LineToAsync(ImageWidth, 0);
            await _context.ClosePathAsync();
            await _context.StrokeAsync();
        }

        private async Task DrawDBImageMaps(List<IImageMap> imgmps)
        {
            foreach (var imageMap in imgmps)
            {
                DrawImageMap(imageMap);
            }
        }

        private void DrawImageMap(IImageMap imageMap)
        {
            if (imageMap.ImageMapShape.Equals("Circle"))
            {
                DrawCircle(imageMap);
            }
            else if (imageMap.ImageMapShape.Equals("Rect"))
            {
                DrawRect(imageMap);
            }
            else // Poly
            {
                DrawPoly(imageMap);
            }
        }

        private async void DrawCircle(IImageMap imageMap)
        {
            var circleCoords = imageMap.ImageMapCoordinates.OrderBy(c => c.Position).ToArray();
            var center = circleCoords[0];
            var circPoint = circleCoords[1];

            await _context.BeginPathAsync();
            await _context.ArcAsync(center.X, center.Y,
                                    CircumferenceFinder.FindRadius(new CartesianPoint(center.X, center.Y),
                                                                   new CartesianPoint(circPoint.X, circPoint.Y)),
                                    0, 2 * Math.PI, false);
            await _context.ClosePathAsync();
            await _context.StrokeAsync();
        }

        private async void DrawRect(IImageMap imageMap)
        {
            var rectCoords = imageMap.ImageMapCoordinates.OrderBy(c => c.Position).ToArray();
            var firstVertex = rectCoords[0];
            var secondVertex = rectCoords[1];
            var rectWidth = firstVertex.X - secondVertex.X;
            var rectHeight = firstVertex.Y - secondVertex.Y;

            await _context.StrokeRectAsync(firstVertex.X, firstVertex.Y, -rectWidth, -rectHeight);
        }

        private async void DrawPoly(IImageMap imageMap)
        {
            var polyCoords = imageMap.ImageMapCoordinates.OrderBy(c => c.Position).ToArray();

            await _context.BeginPathAsync();

            for (int i = 0; i < polyCoords.Length; i++)
            {
                if (i == 0)
                {
                    await _context.MoveToAsync(polyCoords[i].X, polyCoords[i].Y);
                }
                else
                {
                    await _context.LineToAsync(polyCoords[i].X, polyCoords[i].Y);
                }
            }

            await _context.ClosePathAsync();
            await _context.StrokeAsync();
        }

        public async void DrawFirstPoint(Double x, Double y, List<IImageMap> imgmps)
        {
            var firstPointX = x;
            var firstPointY = y;

            var seconPointX = firstPointX + 3;

            _context = await this._canvasReference.CreateCanvas2DAsync();

            await _context.ClearRectAsync(1, 1, ImageWidth, ImageHeight);

            await DrawCanvasAreaBordes();

            await _context.DrawImageAsync(OperaImage, 0, 0);

            await DrawDBImageMaps(imgmps);

            await _context.BeginPathAsync();

            await _context.ArcAsync(firstPointX, firstPointY,
                                    CircumferenceFinder.FindRadius(new CartesianPoint(firstPointX, firstPointY),
                                                                   new CartesianPoint(seconPointX, firstPointY)),
                                    0, 2 * Math.PI, false);

            await _context.ClosePathAsync();
            await _context.SetFillStyleAsync("red");
            await _context.FillAsync();
            await _context.StrokeAsync();

        }

        public async void DrawNewCircleOrRect(String SelectedImageMapShape, List<IImageMap> imgmps, LinkedList<ImageMapCoordinateDTO> coords)
        {
            _context = await this._canvasReference.CreateCanvas2DAsync();

            await _context.ClearRectAsync(1, 1, ImageWidth, ImageHeight);

            await DrawCanvasAreaBordes();

            await _context.DrawImageAsync(OperaImage, 0, 0);

            await DrawDBImageMaps(imgmps);

            ImageMapCoordinateDTO[] orderedCoords;

            if (SelectedImageMapShape.Equals("Circle"))
            {
                await _context.BeginPathAsync();

                orderedCoords = coords.OrderBy(c => c.Position).ToArray();

                var center = orderedCoords[0];

                var circPoint = orderedCoords[1];

                await _context.ArcAsync(center.X, center.Y,
                                        CircumferenceFinder.FindRadius(new CartesianPoint(center.X, center.Y),
                                                                        new CartesianPoint(circPoint.X, circPoint.Y)),
                                        0, 2 * Math.PI, false);

                await _context.StrokeAsync();

                await _context.ClosePathAsync();
            }
            else
            {
                orderedCoords = coords.OrderBy(c => c.Position).ToArray();
                var firstVertex = orderedCoords[0];
                var secondVertex = orderedCoords[1];
                var rectWidth = firstVertex.X - secondVertex.X;
                var rectHeight = firstVertex.Y - secondVertex.Y;

                await _context.StrokeRectAsync(firstVertex.X, firstVertex.Y, -rectWidth, -rectHeight);
            }
        }

        public async void DrawNewPoly(List<IImageMap> imgmps, LinkedList<ImageMapCoordinateDTO> coords)
        {
            _context = await this._canvasReference.CreateCanvas2DAsync();

            await _context.ClearRectAsync(1, 1, ImageWidth, ImageHeight);

            await DrawCanvasAreaBordes();

            await _context.DrawImageAsync(OperaImage, 0, 0);

            await DrawDBImageMaps(imgmps);

            await _context.BeginPathAsync();

            var orderedCoords = coords.OrderBy(c => c.Position).ToArray();

            for (int i = 0; i < orderedCoords.Length; i++)
            {
                if (i == 0)
                {
                    await _context.MoveToAsync(orderedCoords[i].X, orderedCoords[i].Y);
                }
                else
                {
                    await _context.LineToAsync(orderedCoords[i].X, orderedCoords[i].Y);
                }
            }

            await _context.ClosePathAsync();
            await _context.StrokeAsync();

        }
    }
}
