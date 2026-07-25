using LongRoad.Core;

namespace LongRoad {
    public abstract class LongRoadBehaviour : LongRoadBehaviourCore
    {
        protected GameManager Game => GameManager.Instance;
        protected LocalManager Local => LocalManager.Instance;
        protected GameUIManager UI => Local?.UI ?? GameUIManager.Instance;
    }
}