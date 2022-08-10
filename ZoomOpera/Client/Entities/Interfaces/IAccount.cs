namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IAccount
    {
        string Name { get; set; }
        string Password { get; set; }
        string Role { get; init; }
    }
}
