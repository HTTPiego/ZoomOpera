using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class Location : ILocation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; }
        public string LocationCode { get; set; }

        //TODO: Aggiungere altre lingue
        public string Notes { get; set; }
        public  virtual Opera Opera { get; set; }

        [Required]
        public virtual Level Level { get; set; }
        public Guid LevelId  { get; set; }
        public Location() { }

        public Location(string locationCode,
                        Guid levelId,
                        string whereTheOperaIs)
        {
            this.LocationCode = locationCode;
            this.LevelId = levelId;
            this.Notes = whereTheOperaIs;
        }

        public override bool Equals(object? obj)
        {
            return obj is Location location &&
                   LocationCode == location.LocationCode &&
                   EqualityComparer<Opera>.Default.Equals(Opera, location.Opera) &&
                   EqualityComparer<Level>.Default.Equals(Level, location.Level);
        }

        public bool EqualsTo(LocationDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.LocationCode != LocationCode 
                || dto.LevelId != LevelId)
                //|| dto.WhereTheOperaIs != Notes)
                return false;
            return true;
        }

    }
}
