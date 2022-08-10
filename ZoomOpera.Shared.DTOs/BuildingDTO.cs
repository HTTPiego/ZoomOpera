using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class BuildingDTO
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string BuildingCode { get; set; }

        public BuildingDTO() { }

        public BuildingDTO (string name, string buildingCode)
        {
            Name = name;
            BuildingCode = buildingCode;
        }
    }
}
