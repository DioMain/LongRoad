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

        public int FuelConsumption => Prototype.FuelConsumption;

        public float MaxWeight => Prototype.MaxWeight;

        public float DistancePerTurn => Prototype.DistancePerTurn;

        public CarModel ModelPrefab => Prototype.Model;

        public CarModel ModelInstance { get; private set; }

        public event Action<CarEntity, int> OnFuelChanged;
        public event Action<CarEntity, int> OnDurabilityChanged;

        public CarEntity(Car entity) : base(entity)
        {
            Durability = entity.Durability;
            Fuel = Mathf.Max(0, entity.DefaultFuel);
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
            if (Prototype.Model == null)
                return null;

            ModelInstance = GameObject.Instantiate(Prototype.Model, parent.position, parent.rotation, parent);
            ModelInstance.Init();
            return ModelInstance;
        }
    }
}
