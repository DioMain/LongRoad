using LongRoad.Core.Scriptables.Abstractions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Route", menuName = "Entities/Route")]
    public class Route : LongRoadScriptable
    {
        public List<RouteStop> Stops = new();
    }

    [Serializable]
    public struct RouteStop
    {
        public Location Location;
        public float DistanceFromStartKm;
    }
}
