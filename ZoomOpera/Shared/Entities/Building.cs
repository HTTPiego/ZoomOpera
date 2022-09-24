using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class Building : IBuilding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string BuildingCode { get; set; }

        public virtual ICollection<Level> Levels { get; set; }
        public Building() { }

        public Building(string name,
                        string buildingCode)
        {
            Name = name;
            BuildingCode = buildingCode;
        }

        public override bool Equals(object? obj)
        {
            return obj is Building building &&
                   Name == building.Name &&
                   BuildingCode == building.BuildingCode;
        }

        public bool EqualsTo(BuildingDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.Name != this.Name
                || dto.BuildingCode != this.BuildingCode)
                return false;
            return true;
        }

    }
}
