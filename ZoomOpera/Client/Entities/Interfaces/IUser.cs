namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IUser
    {
        string Email { get; set; }
        string Surname { get; set; }
        string GivenName { get; set; }
    }
}
