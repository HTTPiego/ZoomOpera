using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class LevelDTO
    {
        [Required]
        public int LevelNumber { get; set; }

        public string Planimetry { get; set; }

        public Guid BuildingId { get; set; }

        public LevelDTO() { }
        public LevelDTO(int levelNumber, string planimetry)
        {
            LevelNumber = levelNumber;
            Planimetry = planimetry;
        }
    }
}
