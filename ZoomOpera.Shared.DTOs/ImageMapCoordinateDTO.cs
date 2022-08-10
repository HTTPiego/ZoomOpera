namespace ZoomOpera.DTOs
{
    public class ImageMapCoordinateDTO
    {
        public double X { get; set; }

        public double Y { get; set; }

        public int Position { get; set; }

        public Guid ImageMapId { get; set; }

        public ImageMapCoordinateDTO() { }

        public ImageMapCoordinateDTO(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageMapCoordinateDTO dTO &&
                   X == dTO.X &&
                   Y == dTO.Y;
        }
    }
}
