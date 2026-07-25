using LongRoad.Core.Scriptables.Abstractions;
using UnityEngine;

namespace LongRoad.Core.Scriptables
{
    [CreateAssetMenu(fileName = "Car", menuName = "Entities/Car")]
    public class Car : LongRoadScriptable
    {
        public int FuelConsumption = 1;

        public int Durability = 100;

        public float MaxWeight = 100f;

        public float DistancePerTurn = 10f;

        public CarModel Model;
    }
}
