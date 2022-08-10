using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class ImageMapCoordinate : IImageMapCoordinate
    {
        public Guid Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int Position { get; set; }
        public virtual ImageMap ImageMap { get; set; }
        public Guid ImageMapId { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is ImageMapCoordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y &&
                   ImageMapId.Equals(coordinate.ImageMapId);
        }
    }
}
