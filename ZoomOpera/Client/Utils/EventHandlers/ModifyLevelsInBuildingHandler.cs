using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;

namespace ZoomOpera.Client.Utils.EventHandlers
{
    public class ModifyLevelsInBuildingHandler : IEventHandler<ILevel>
    {
        public event EventHandler<ILevel> EventHandler;

        public void FireEvent(ILevel modified)
        {
            EventHandler?.Invoke(this, modified);
        }
    }
}
