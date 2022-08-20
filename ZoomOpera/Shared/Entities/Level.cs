using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class Level : ILevel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public int LevelNumber { get; set; }
        public string Planimetry { get; set; }
        public virtual ICollection<Location> Locations { get; set; } 
        public virtual ICollection<MonitorPlatform> MonitorPlatforms { get; set; } 

        [Required]
        public virtual Building Building { get; set; }
        public Guid BuildingId { get; set; }
        public Level() { }

        public Level(int levelNumber,
                     string planimetry,
                     Guid buildingId)
        {
            LevelNumber = levelNumber;
            Planimetry = planimetry;
            BuildingId = buildingId;
        }

        public override bool Equals(object? obj)
        {
            return obj is Level level &&
                   LevelNumber == level.LevelNumber &&
                   BuildingId.Equals(level.BuildingId);
        }

        public bool EqualsTo(LevelDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.LevelNumber != LevelNumber 
                //|| dto.Planimetry != Planimetry 
                || ! dto.BuildingId.Equals(BuildingId))
                return false;
            return true;
        }

    }
}
