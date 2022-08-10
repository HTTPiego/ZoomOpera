
using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs 
{ 
    public class OperaDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string? ItalianDescription { get; set; }

        [Required]
        public string AuthorFirstName { get; set; }

        [Required]
        public string AuthorLastName { get; set; }

        //public string Photo { get; set; }

        [Required]
        public OperaImageDTO OperaImage { get; set; }

        public Guid LocationId { get; set; }

        public OperaDTO() { }

        public OperaDTO(string name, string? italianDescription, string authorFirstName, string authorLastName)
        {
            Name = name;
            ItalianDescription = italianDescription;
            AuthorFirstName = authorFirstName;
            AuthorLastName = authorLastName;
        }
    }
}
