using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GameEvents
{
    [UseGameEvent(0.75f)]
    public class WeatherEvent : GameEventBase
    {
        public override IEnumerator Event()
        {
            Debug.Log("WeatherEvent: a storm rolls in");

            yield return new WaitForSeconds(1);

            Debug.Log("WeatherEvent: ended");
        }
    }
}
