using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.35f)]
    public class SupplyFindEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            Debug.Log("SupplyFindEvent: found abandoned supplies");

            yield return new WaitForSeconds(1);

            Debug.Log("SupplyFindEvent: ended");
        }
    }
}
