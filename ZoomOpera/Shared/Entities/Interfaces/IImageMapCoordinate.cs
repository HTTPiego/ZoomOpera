
using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IImageMapCoordinate
    {
        Guid Id { get; set; }
        double X { get; set; }

        double Y { get; set; }

        int Position { get; set; }

        ImageMap ImageMap { get; set; }
        
        Guid ImageMapId  { get; set; }

        bool Equals(object? obj);

        bool EqualsTo(ImageMapCoordinateDTO dto);
    }
}
