using ZoomOpera.Client.Entities.Interfaces;
using ZoomOpera.Client.Utils.Interfaces;

namespace ZoomOpera.Client.Utils.EventHandlers
{
    public class ModifyBuildingHandler : IEventHandler<IBuilding>
    {
        public event EventHandler<IBuilding> EventHandler;
        public void FireEvent(IBuilding modified)
        {
            EventHandler?.Invoke(this, modified);
        }

    }
}
