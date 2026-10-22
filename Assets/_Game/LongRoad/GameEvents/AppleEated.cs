using LongRoad.Core.GameEvent.Abstractions;
using System.Collections;

namespace LongRoad.GameEvents
{
    [BoundGameEvent(Core.GameEvent.BoundGameEventKind.Item, "apple")]
    public class AppleEated : GameEventBase
    {
        public override IEnumerator Event()
        {
            LocalManager.Notification.Notify("Вы съели <sprite name=\"apple\"> яблоко", UI.Elements.NotificationUI.WarnLevel.Warning);

            yield break;
        }
    }
}
