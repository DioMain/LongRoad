using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.5f)]
    public class EncounterEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            Debug.Log("EncounterEvent: travelers appear on the road");

            yield return new WaitForSeconds(1);

            Debug.Log("EncounterEvent: ended");
        }
    }
}
