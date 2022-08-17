using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class ImageMapDTO
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string DetailedDescription { get; set; }

        [Required]
        public string ImageMapShape { get; set; }
        public Guid OperaImageId { get; set; }

        public LinkedList<ImageMapCoordinateDTO> ImageMapCoordinates { get; set; }  

        public ImageMapDTO() { }

        public ImageMapDTO(string Name,
                            string DetailedDescription, 
                            string ImageMapShape)
        {
            this.Title = Name;
            this.DetailedDescription = DetailedDescription;
            this.ImageMapShape = ImageMapShape;
        }
    }
}
