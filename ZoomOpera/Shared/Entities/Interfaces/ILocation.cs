using ZoomOpera.DTOs;

namespace ZoomOpera.Shared.Entities.Interfaces
{
    public interface ILocation
    {
        Guid Id { get; set; }
        string LocationCode { get; set; }
        string Notes { get; set; }
        Opera Opera { get; set; }
        Level Level { get; set; }
        Guid LevelId { get; set; }
        public bool Equals(object? obj);
        public bool EqualsTo(LocationDTO dto);


    }
}