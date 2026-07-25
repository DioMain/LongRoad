using System.Collections;
using UnityEngine;

namespace LongRoad.Core.GameEvent
{
    public abstract class GameEventBase
    {
        public Coroutine Invoke(MonoBehaviour listener)
        {
            return listener.StartCoroutine(Event());
        }

        public virtual bool EventCanExecute()
        {
            return true;
        }

        public abstract IEnumerator Event();
    }
}
