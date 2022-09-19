using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.DTOs;

namespace ZoomOpera.Client.Entities
{
    public class Building : IBuilding
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BuildingCode { get; set; }
        public virtual ICollection<Level> Levels { get; set; }
        public override bool Equals(object? obj)
        {
            return obj is Building building &&
                   Name == building.Name &&
                   BuildingCode == building.BuildingCode;
        }

        //public bool EqualsTo(BuildingDTO dto)
        //{
        //    if (dto.Name.Equals(this.Name) 
        //        || dto.BuildingCode.Equals(this.BuildingCode))
        //        return true;
        //    return false;
        //}
    }
}
