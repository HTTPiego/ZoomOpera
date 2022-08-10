namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IOperaImage
    {
        Guid Id { get; set; }

        string Image { get; set; }

        Opera Opera { get; set; }

        Guid OperaId { get; set; }

        ICollection<ImageMap> ImageMaps { get; set; }

        bool Equals(object? obj);
    }
}
