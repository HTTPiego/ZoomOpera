
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class ImageMapCoordinate : IImageMapCoordinate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public int Position { get; set; }

        [Required]
        public virtual ImageMap ImageMap { get; set; }
        public Guid ImageMapId { get; set; }
        

        public ImageMapCoordinate() { }

        public ImageMapCoordinate(double x, double y, int positon ,Guid imageMapId)
        {
            X = x;
            Y = y;
            Position = positon;
            ImageMapId = imageMapId;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageMapCoordinate coordinate &&
                   X == coordinate.X &&
                   Y == coordinate.Y &&
                   ImageMapId.Equals(coordinate.ImageMapId);
        }

        public bool EqualsTo(ImageMapCoordinateDTO dto)
        {
            if (dto == null)
                return false;
            if ( !ImageMapId.Equals( dto.ImageMapId )
                || X != dto.X
                || Y != dto.Y)
                return false;
            return true;
        }
    }
}
