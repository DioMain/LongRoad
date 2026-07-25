namespace LongRoad.Core.GameEvent
{
    public abstract class ContextualGameEventBase : GameEventBase
    {
        public PersonEntity Source { get; set; }

        public PersonEntity Target { get; set; }

        public bool HasTarget => Target != null;
    }
}
