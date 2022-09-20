using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class MonitorPlatform : IMonitorPlatform
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string MonitorCode { get; set; }

        [Required]
        public virtual Level Level { get; set; }
        public Guid LevelId { get; set; }

        //Il nome della piattaforma e' composto da 
        // {codiceEdificio}_{piano}_{codiceLocazione}
        public string Name { get; set; }

        public string Password { get; set; }

        [NotMapped]
        public string Role { get; init; } = "MonitorPlatform";

        public MonitorPlatform() { }

        public MonitorPlatform(string monitorCode,
                               Guid levelId,
                               string name,
                               string password)
        {
            MonitorCode = monitorCode;
            LevelId = levelId;
            Name = name;
            Password = password;
        }

        //TODO: sistemare
        public bool ShouldSerializePassword()
        {
            // don't serialize the Manager property if an employee is their own manager
            return true;
        }

        public override bool Equals(object? obj)
        {
            return obj is MonitorPlatform platform &&
                   MonitorCode == platform.MonitorCode &&
                   EqualityComparer<Level>.Default.Equals(Level, platform.Level) &&
                   Name == platform.Name;
        }

        public bool EqualsTo(MonitorPlatformDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.MonitorCode != MonitorCode 
                || dto.LevelId != LevelId)
                return false;
            return true;
        }


    }
}
