using DynamicImageMapping;
using System.Drawing;
using ZoomOpera.Shared.DynamicImageMapping.Interfaces;

namespace ZoomOpera.Shared.DynamicImageMapping
{
    public class RectImageMap : IImageMap
    {
        public LinkedList<Point> ImageMapPoints { get; set; }
        public ImageMapShape MapShape => ImageMapShape.Rect; 


    }
}
