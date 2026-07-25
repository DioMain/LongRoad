using LongRoad.Core;
using LongRoad.Core.Scriptables;
using LongRoad.Domain.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LongRoad.Services
{
    public class PersonService : IService
    {
        private const int HungerDrainPerTurn = 5;
        private const int StarvationDamagePerTurn = 5;

        private readonly List<PersonEntity> _people = new();

        public IReadOnlyList<PersonEntity> People => _people;

        public event Action<PersonEntity> OnPersonAdded;
        public event Action<PersonEntity> OnPersonRemoved;
        public event Action OnModifiersApplied;

        public void LoadRoster(IEnumerable<Person> roster)
        {
            _people.Clear();

            if (roster == null)
                return;

            foreach (var person in roster)
            {
                if (person == null)
                    continue;

                var entity = new PersonEntity(person);
                _people.Add(entity);
                OnPersonAdded?.Invoke(entity);
            }
        }

        public PersonEntity Add(Person person)
        {
            if (person == null)
                return null;

            var entity = new PersonEntity(person);
            _people.Add(entity);
            OnPersonAdded?.Invoke(entity);
            return entity;
        }

        public bool Remove(PersonEntity person)
        {
            if (person == null || !_people.Remove(person))
                return false;

            OnPersonRemoved?.Invoke(person);
            return true;
        }

        public void ApplyPhaseModifiers()
        {
            for (var i = 0; i < _people.Count; i++)
            {
                var person = _people[i];
                if (person.Heal <= 0)
                    continue;

                person.SetHunger(Mathf.Max(0, person.Hunger - HungerDrainPerTurn));

                if (person.Hunger == 0)
                    person.SetHeal(Mathf.Max(0, person.Heal - StarvationDamagePerTurn));
            }

            OnModifiersApplied?.Invoke();
        }
    }
}
