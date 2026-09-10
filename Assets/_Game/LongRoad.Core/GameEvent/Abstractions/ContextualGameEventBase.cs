using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.Core.Scriptables.Abstractions;

namespace LongRoad.Core.GameEvent.Abstractions
{
    public abstract class ContextualGameEventBase : GameEventBase
    {
        public LongRoadScriptable Source { get; set; }

        public PersonEntity Target { get; set; }
    }
}
