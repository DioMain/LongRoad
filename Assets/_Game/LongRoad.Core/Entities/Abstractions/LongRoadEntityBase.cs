using LongRoad.Core.Scriptables.Abstractions;

namespace LongRoad.Core.Entities.Abstraction
{
    public abstract class LongRoadEntityBase<T> 
        where T : LongRoadScriptable
    {
        public T Entity { get; private set; }

        public LongRoadEntityBase(T entity)
        {
            Entity = entity;
        }
    }
}
