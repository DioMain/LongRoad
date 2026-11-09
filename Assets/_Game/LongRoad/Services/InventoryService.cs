using LongRoad.Core;
using LongRoad.Core.GameEvent;
using LongRoad.Core.Inventory;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using LongRoad.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LongRoad.Services
{
    public class InventoryService : IService
    {
        private readonly Inventory _inventory = new();
        private readonly GameData _data;
        private readonly PersonService _persons;

        public IReadOnlyList<InventoryStack> Stacks => _inventory.Stacks;
        public float CurrentWeight => _inventory.CurrentWeight;

        public event Action OnChanged;
        public event Action<Item, int> OnItemAdded;
        public event Action<string, int> OnItemRemoved;
        public event Action<Item, PersonEntity> OnItemUsed;

        public InventoryService(GameData data, PersonService persons)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            _persons = persons ?? throw new ArgumentNullException(nameof(persons));
        }

        public bool CanAdd(Item item, int count)
        {
            return _inventory.CanAdd(item, count, GetMaxWeight());
        }

        public bool TryAdd(Item item, int count)
        {
            if (!_inventory.TryAdd(item, count, GetMaxWeight()))
                return false;

            OnItemAdded?.Invoke(item, count);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryRemove(Item item, int count)
        {
            if (item == null || !_inventory.TryRemove(item, count))
                return false;

            OnItemRemoved?.Invoke(item.Tag, count);
            OnChanged?.Invoke();
            return true;
        }

        public bool TryRemove(string tag, int count)
        {
            if (!_inventory.TryRemove(tag, count))
                return false;

            OnItemRemoved?.Invoke(tag, count);
            OnChanged?.Invoke();
            return true;
        }

        public int GetCount(Item item)
        {
            return _inventory.GetCount(item);
        }

        public int GetCount(string tag)
        {
            return _inventory.GetCount(tag);
        }

        public IEnumerator UseItem(
            Item item,
            MonoBehaviour host,
            params PersonEntity[] targets)
        {
            if (item == null || host == null || string.IsNullOrEmpty(item.Tag))
                yield break;

            if (GetCount(item) < 1)
                yield break;

            if (!TryRemove(item, 1))
                yield break;

            var hasBoundEvent = BoundEventCatalog.HasAny(BoundGameEventKind.Item, item.Tag);

            if (item.IsFuel)
            {
                _data.Car.SetFuel(_data.Car.Fuel + item.FuelResotre);
            }

            if (targets.Length == 0)
            {
                OnItemUsed?.Invoke(item, null);

                if (hasBoundEvent)
                {
                    yield return BoundEventRunner.Run(
                        BoundGameEventKind.Item,
                        item,
                        null,
                        host);
                }
            }
            else
            {
                foreach (PersonEntity target in targets)
                {
                    OnItemUsed?.Invoke(item, target);

                    if (item.IsMedicine)
                    {
                        target.SetHeal(target.Heal + item.MedicineRestore);
                    }

                    if (item.IsFood)
                    {
                        target.SetHunger(target.Hunger + item.HungerResotre);

                        GameManager.Instance.Audio.PlaySfx(AudioManager.CommonSfxClip.Eat);
                    }

                    if (hasBoundEvent)
                    {
                        yield return BoundEventRunner.Run(
                            BoundGameEventKind.Item,
                            item,
                            target,
                            host);
                    }
                }
            }
        }

        private float GetMaxWeight()
        {
            return _data.Car?.MaxWeight ?? 0f;
        }
    }
}
