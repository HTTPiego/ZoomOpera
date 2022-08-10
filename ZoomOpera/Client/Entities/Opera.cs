using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class Opera : IOpera
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? ItalianDescription { get; set; }
        public string AuthorFirstName { get; set; }
        public string AuthorLastName { get; set; }

        public virtual OperaImage OperaImage { get; set; }
        public virtual Location Location { get; set; }
        public Guid LocationId { get; set; }
    }
}
