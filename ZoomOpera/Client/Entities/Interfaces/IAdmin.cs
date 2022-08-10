namespace ZoomOpera.Client.Entities.Interfaces
{
    public interface IAdmin : IAccount, IUser
    {
        public Guid Id { get; set; }
    }
}
