using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IOpera
    {
        Guid Id { get; set; } 
        string Name { get; set; }
        string ItalianDescription { get; set; }

        //string EnglishDescription { get; set; }
        //string GermanDescription { get; set; }
        //string FrenchDescription { get; set; }
        //DateTime Year { get; set; }

        string AuthorFirstName { get; set; }
        string AuthorLastName { get; set; }
        //string Photo { get; set; }

        OperaImage Image { get; set; }
        Location Location { get; set; }
        Guid LocationId { get; set; }
        public bool Equals(object? obj);
        public bool EqualsTo(OperaDTO dto);

    }
}
