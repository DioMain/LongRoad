using LongRoad.Core.Scriptables.Abstractions;

namespace LongRoad.Core.Entities.Abstraction
{
    public abstract class LongRoadEntityBase<T> 
        where T : LongRoadScriptable
    {
        public T Prototype { get; private set; }

        public LongRoadEntityBase(T prototype)
        {
            Prototype = prototype;
        }
    }
}
