namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IBuilding
    {
        public Guid Id { get; set; }
        string Name { get; set; }

        //Address Address { get; set; }
        string BuildingCode { get; set; }
        ICollection<Level> Levels { get; set; }
        bool Equals(object? obj);
    }
}
