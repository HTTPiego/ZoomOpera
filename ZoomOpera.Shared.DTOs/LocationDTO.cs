using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class LocationDTO
    {
        [Required]
        public string LocationCode { get; set; }
        public Guid LevelId { get; set; }

        [Required]
        public string Notes { get; set; }

        public LocationDTO() { }

        public LocationDTO(string locationCode, string notes)
        {
            LocationCode = locationCode;
            Notes = notes;
        }
    }
}
