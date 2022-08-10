using ZoomOpera.Client.Entities.Interfaces;

namespace ZoomOpera.Client.Entities
{
    public class Admin : IAdmin
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; init; } = "Admin";
        public string Email { get; set; }
        public string Surname { get; set; }
        public string GivenName { get; set; }
        
    }
}
