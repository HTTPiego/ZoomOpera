using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class OperaImage : IOperaImage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Image { get; set; }

        [Required]
        public virtual Opera Opera { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public Guid OperaId { get; set; }
        public virtual ICollection<ImageMap> ImageMaps { get; set; }

        public OperaImage() { }

        public OperaImage(string image, int height, int width, Guid operaId)
        {
            Image = image;
            Height = height;
            Width = width;
            OperaId = operaId;
        }

        public override bool Equals(object? obj)
        {
            return obj is OperaImage image &&
                   Image == image.Image;
        }

        public bool EqualsTo(OperaImageDTO dto)
        {
            if (dto == null)
                return false;
            if (Image != dto.Image)
                return false;
            return true;
        }
    }
}
