using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class MonitorPlatformDTO
    {
        [Required]
        public string MonitorCode { get; set; }
        public Guid LevelId { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        public MonitorPlatformDTO() { }

        public MonitorPlatformDTO(string monitorCode, string name, string password)
        {
            MonitorCode = monitorCode;
            Name = name;
            Password = password;
        }
    }
}
