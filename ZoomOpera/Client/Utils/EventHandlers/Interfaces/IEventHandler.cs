namespace ZoomOpera.Client.Utils.Interfaces
{
    public interface IEventHandler<I>
    {
        event EventHandler<I> EventHandler;

        void FireEvent(I itemToInvoke);
    }
}