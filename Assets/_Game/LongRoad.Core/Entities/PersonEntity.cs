using LongRoad.Core.Entities.Abstraction;
using LongRoad.Core.Scriptables;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LongRoad.Core
{
    public class PersonEntity : LongRoadEntityBase<Person>
    {
        private readonly List<LiveStatus> _liveStatutes = new();

        public int Heal { get; private set; }
        public int Hunger { get; private set; }
        public int Mood { get; private set; }

        public IReadOnlyList<LiveStatus> LiveStatuses => _liveStatutes;

        public IReadOnlyList<Trait> Traits => Entity.Traits;

        public event Action<PersonEntity> OnStatsChanged;
        public event Action<PersonEntity, LiveStatus> OnStatusAdded;
        public event Action<PersonEntity, LiveStatus> OnStatusRemoved;

        public PersonEntity(Person entity) : base(entity)
        {
            Heal = entity.DefaultHeal;
            Hunger = entity.DefaultHunger;
            Mood = entity.DefaultMood;
        }

        public void SetHeal(int value)
        {
            value = Math.Max(0, value);
            if (Heal == value)
                return;

            Heal = value;
            OnStatsChanged?.Invoke(this);
        }

        public void SetHunger(int value)
        {
            value = Math.Max(0, value);
            if (Hunger == value)
                return;

            Hunger = value;
            OnStatsChanged?.Invoke(this);
        }

        public void SetMood(int value)
        {
            value = Math.Max(0, value);
            if (Mood == value)
                return;

            Mood = value;
            OnStatsChanged?.Invoke(this);
        }

        public void AddStatus(LiveStatus status)
        {
            if (HasStatus(status)) return;

            _liveStatutes.Add(status);
            OnStatusAdded?.Invoke(this, status);
        }

        public void RemoveStatus(LiveStatus status)
        {
            if (!HasStatus(status)) return;

            _liveStatutes.RemoveAll(i => i.Tag == status.Tag);
            OnStatusRemoved?.Invoke(this, status);
        }

        public bool HasStatus(LiveStatus status)
        {
            return _liveStatutes.Any(i => i.Tag == status.Tag);
        }
    }
}
