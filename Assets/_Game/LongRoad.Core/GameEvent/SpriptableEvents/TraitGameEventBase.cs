using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.Core.Scriptables;

namespace LongRoad.Core.GameEvent.SpriptableEvents
{
    public abstract class TraitGameEventBase : ContextualGameEventBase
    {
        public Trait TraitSource => (Trait)Source;
    }
}
