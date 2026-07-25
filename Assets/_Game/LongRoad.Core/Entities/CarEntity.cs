using LongRoad.Core.Entities.Abstraction;
using LongRoad.Core.Scriptables;
using System;
using UnityEngine;

namespace LongRoad.Core
{
    public class CarEntity : LongRoadEntityBase<Car>
    {
        public int Fuel { get; private set; }
        public int Durability { get; private set; }

        public int FuelConsumption => Entity.FuelConsumption;

        public float MaxWeight => Entity.MaxWeight;

        public float DistancePerTurn => Entity.DistancePerTurn;

        public CarModel ModelPrefab => Entity.Model;

        public CarModel ModelInstance { get; private set; }

        public event Action<CarEntity, int> OnFuelChanged;
        public event Action<CarEntity, int> OnDurabilityChanged;

        public CarEntity(Car entity) : base(entity)
        {
            Durability = entity.Durability;
        }

        public void SetFuel(int value)
        {
            value = Mathf.Max(0, value);
            if (Fuel == value)
                return;

            Fuel = value;
            OnFuelChanged?.Invoke(this, Fuel);
        }

        public void SetDurability(int value)
        {
            value = Mathf.Max(0, value);
            if (Durability == value)
                return;

            Durability = value;
            OnDurabilityChanged?.Invoke(this, Durability);
        }

        public CarModel SpawnModel(Transform parent = null)
        {
            if (Entity.Model == null)
                return null;

            ModelInstance = GameObject.Instantiate(Entity.Model, parent);
            ModelInstance.Init();
            return ModelInstance;
        }
    }
}
