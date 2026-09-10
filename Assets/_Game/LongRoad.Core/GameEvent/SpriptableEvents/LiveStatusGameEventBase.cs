using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.Core.Scriptables;

namespace LongRoad.Core.GameEvent.SpriptableEvents
{
    public abstract class LiveStatusGameEventBase : ContextualGameEventBase
    {
        public LiveStatus StatusSource => (LiveStatus)Source;
    }
}
