using System;

namespace LongRoad.Core.GameEvent
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class BoundGameEventAttribute : Attribute
    {
        public BoundGameEventKind Kind { get; }
        public string Tag { get; }

        public BoundGameEventAttribute(BoundGameEventKind kind, string tag)
        {
            Kind = kind;
            Tag = tag?.Trim() ?? string.Empty;
        }
    }
}
