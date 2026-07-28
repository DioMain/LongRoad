using LongRoad.Core;
using LongRoad.UI;

namespace LongRoad {
    public abstract class LongRoadBehaviour : LongRoadBehaviourCore
    {
        protected GameManager Game => GameManager.Instance;
        protected LocalManager Local => LocalManager.Instance;
    }
}