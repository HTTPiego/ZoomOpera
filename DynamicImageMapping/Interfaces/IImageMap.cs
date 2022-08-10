using DynamicImageMapping;
using System.Drawing;

namespace ZoomOpera.Shared.DynamicImageMapping.Interfaces
{
    public interface IImageMap
    {
        LinkedList<Point> ImageMapPoints { get; set; }

        ImageMapShape MapShape { get; }

    }
}
