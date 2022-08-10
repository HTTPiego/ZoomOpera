using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class ImageMap : IImageMap
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public ImageMapShape ImageMapShape { get; init; }

        [Required]
        public virtual OperaImage OperaImage { get; set; }
        public Guid OperaImageId { get; set; }
        public virtual ICollection<ImageMapCoordinate> ImageMapCoordinates { get; set; }
        public string Name { get; set; }

        public string DetailedDescription { get; set; }

        public ImageMap() { }

        public ImageMap(ImageMapShape imageMapShape, 
                        string name,
                        string description,
                        Guid operaImageId)
        {
            Name = name;
            DetailedDescription = description;
            ImageMapShape = imageMapShape;
            OperaImageId = operaImageId;
        }

        public override bool Equals(object? obj)
        {
            return obj is ImageMap map &&
                   ImageMapShape == map.ImageMapShape &&
                   OperaImageId.Equals(map.OperaImageId) &&
                   Name == map.Name;
        }


        public bool EqualsTo(ImageMapDTO dto)
        {
            if (dto == null)
                return false;
            if (!dto.OperaImageId.Equals(OperaImageId)
                || !dto.ImageMapShape.Equals(ImageMapShape)
                || dto.Title.ToLower() != Name.ToLower() )
                return false;
            return true;
        }

    }
}
