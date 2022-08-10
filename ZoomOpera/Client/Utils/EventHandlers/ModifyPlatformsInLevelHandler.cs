using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;

namespace ZoomOpera.Client.Utils.EventHandlers
{
    public class ModifyPlatformsInLevelHandler : IEventHandler<IMonitorPlatform>
    {
        public event EventHandler<IMonitorPlatform> EventHandler;

        public void FireEvent(IMonitorPlatform modified)
        {
            EventHandler?.Invoke(this, modified);
        }
    }
}
