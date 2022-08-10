using System.ComponentModel.DataAnnotations;

namespace ZoomOpera.DTOs
{
    public class LoginDTO
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public LoginDTO() { }

    }
}
