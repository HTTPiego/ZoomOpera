namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IOpera
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string ItalianDescription { get; set; }
        string AuthorFirstName { get; set; }
        string AuthorLastName { get; set; }
        OperaImage OperaImage { get; set; }
        Location Location { get; set; }
        Guid LocationId { get; set; }
    }
}
