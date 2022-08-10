using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;

namespace ZoomOpera.Client.Utils.EventHandlers
{
    public class ModifyLocationsInLevelHandler : IEventHandler<ILocation>
    {
        public event EventHandler<ILocation> EventHandler;

        public void FireEvent(ILocation modified)
        {
            EventHandler?.Invoke(this, modified);
        }
    }
}
