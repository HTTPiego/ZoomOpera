using System.ComponentModel.DataAnnotations;
using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class OperaImage : IOperaImage
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public virtual Opera Opera { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }
        public Guid OperaId { get; set; }
        public virtual ICollection<ImageMap> ImageMaps { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is OperaImage image &&
                   Image == image.Image;
        }
    }
}
