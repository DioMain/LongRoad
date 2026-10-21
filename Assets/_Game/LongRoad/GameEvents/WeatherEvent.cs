using LongRoad.Core.GameEvent.Abstractions;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.75f)]
    public class WeatherEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            LocalManager.Instance.UI.Notification.Notify("A storm rolls in!");

            yield return new WaitForSeconds(1);
        }
    }
}
