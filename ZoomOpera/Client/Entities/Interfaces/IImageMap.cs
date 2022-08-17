using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IImageMap
    {
        Guid Id { get; set; }

        string Name { get; set; }

        string DetailedDescription { get; set; }
        string ImageMapShape { get; init; }
        OperaImage OperaImage { get; set; }
        Guid OperaImageId { get; set; }
        ICollection<ImageMapCoordinate> ImageMapCoordinates { get; set; }

        bool Equals(object? obj);
    }
}
