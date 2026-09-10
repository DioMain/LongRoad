using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.Core.Scriptables;

namespace LongRoad.Core.GameEvent.SpriptableEvents
{
    public abstract class ItemGameEventBase : ContextualGameEventBase
    {
        public Item ItemSource => (Item)Source;
    }
}
