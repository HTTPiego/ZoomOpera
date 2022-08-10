using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities

{
    public class Opera : IOpera
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ItalianDescription { get; set; }

        //public string? EnglishDescription { get; set; }
        //public string? GermanDescription { get; set; }
        //public string? FrenchDescription { get; set; }

        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }
        //public string Photo { get; set; }

        public virtual OperaImage Image { get; set; }

        [Required]
        public virtual Location Location { get; set; }

        public Guid LocationId { get; set; }
        public Opera() { }

        public Opera(string name, 
                     string italianDescription,
                     string authorFirstName,
                     string authorLastName,
                     //string photo,
                     Guid locationId)
        {
            Name = name;
            ItalianDescription = italianDescription;
            AuthorFirstName = authorFirstName;
            AuthorLastName = authorLastName;
            //Photo = photo;
            LocationId = locationId;
        }

        public override bool Equals(object? obj)
        {
            return obj is Opera opera &&
                   Name == opera.Name &&
                   AuthorFirstName == opera.AuthorFirstName &&
                   AuthorLastName == opera.AuthorLastName;
        }

        public bool EqualsTo(OperaDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.Name != Name 
                || dto.AuthorFirstName != AuthorFirstName 
                || dto.AuthorLastName != AuthorLastName)
                return false;
            return true;
        }
    }
}
