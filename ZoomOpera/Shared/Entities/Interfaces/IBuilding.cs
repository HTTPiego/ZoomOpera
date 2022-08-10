using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface IBuilding
    {
        Guid Id { get; set; }
        string Name{ get; set; }
        //Address Address { get; set; }
        string BuildingCode { get; set; }
        ICollection <Level> Levels { get; set; }
        bool Equals(object? obj);
        bool EqualsTo(BuildingDTO dto);
    }
}
