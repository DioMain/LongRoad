using LongRoad.Core.Scriptables.Abstractions;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Route", menuName = "Entities/Route")]
    public class Route : LongRoadScriptable
    {
        [Serializable]
        public struct RouteStop
        {
            public Location Location;
            public float DistanceFromStartKm;
        }

        [Serializable]
        public class EndlessModeRouteStop
        {
            public Location Location;
            [Range(0f, 1f)]
            public float Probability = 1f;
        }

        [Header("Companing settings:")]
        [ReorderableList]
        public List<RouteStop> Stops = new();
        [Header("Endless mode settings:")]
        public float MinAvarangeStopDistance = 40f;
        public float MaxAvarangeStopDistance = 80f;
        [Space]
        public List<EndlessModeRouteStop> EndlessModeStops = new();
    }
}
