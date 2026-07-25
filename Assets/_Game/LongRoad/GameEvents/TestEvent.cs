using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent]
    public class TestEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            Debug.Log("Event is working");

            yield return new WaitForSeconds(1);

            Debug.Log("Ended");
        }
    }
}
