using LongRoad.Core;

namespace LongRoad {
    public abstract class LoadRoadBehaviour : LoadRoadBehaviourCore
    {
        protected GameManager Game => GameManager.Instance;
        protected LocalManager Local => LocalManager.Instance;
    }
}