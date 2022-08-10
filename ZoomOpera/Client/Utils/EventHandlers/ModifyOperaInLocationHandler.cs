using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;

namespace ZoomOpera.Client.Utils.EventHandlers
{
    public class ModifyOperaInLocationHandler : IEventHandler<IOpera>
    {
        public event EventHandler<IOpera> EventHandler;

        public void FireEvent(IOpera itemToInvoke)
        {
            EventHandler?.Invoke(this, itemToInvoke);
        }
    }
}
