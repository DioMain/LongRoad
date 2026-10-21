using LongRoad.Core.GameEvent.Abstractions;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.5f)]
    public class EncounterEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            LocalManager.Instance.UI.Notification.Notify(@"Travelers appear on the road!");

            yield return new WaitForSeconds(1);
        }
    }
}
