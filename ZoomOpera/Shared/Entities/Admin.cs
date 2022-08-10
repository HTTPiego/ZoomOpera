using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ZoomOpera.DTOs;
using ZoomOpera.Shared.Entities.Interfaces;

namespace ZoomOpera.Shared.Entities
{
    public class Admin : IAdmin
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Password { get; set; }

        [NotMapped]
        public string Role { get; init; } = "Admin";
        public string Email { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }

        public Admin() { }

        public Admin(string userName, string password, string email, string surname, string givenName)
        {
            Name = userName;
            Password = password;
            Email = email;
            Surname = surname;
            GivenName = givenName;
        }

        public override bool Equals(object? obj)
        {
            return obj is Admin admin &&
                   Name == admin.Name &&
                   Role == admin.Role &&
                   Email == admin.Email &&
                   Surname == admin.Surname &&
                   GivenName == admin.GivenName;
        }

        public bool Equals(AdminDTO dto)
        {
            if (dto == null)
                return false;
            if (dto.GivenName != GivenName 
                || dto.Surname != Surname
                || dto.Email != Email)
                return false;
            return true;
        }
    }
}
