using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Entities
{
    public class ImageMap : IImageMap
    {
        public Guid Id { get; set; }
        public ImageMapShape ImageMapShape { get; init; }

        public virtual OperaImage OperaImage { get; set; }
        public Guid OperaImageId { get; set; }
        public virtual ICollection<ImageMapCoordinate> ImageMapCoordinates { get; set; }
        public string Name { get; set; }

        public string DetailedDescription { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ImageMap map &&
                   ImageMapShape == map.ImageMapShape &&
                   OperaImageId.Equals(map.OperaImageId) &&
                   Name == map.Name;
        }
    }
}
