
using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class OperaImageDTO
    {
        [Required]
        public string Image { get; set; }

        public Guid OperaId { get; set; }

        public OperaImageDTO() { }

        public OperaImageDTO(string image)
        {
            Image = image;
        }
    }
}
