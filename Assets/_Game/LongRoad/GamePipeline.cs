using LongRoad.Core.GameEvent;
using LongRoad.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace LongRoad
{
    public enum GameEndState
    {
        None,
        Won,
        Lost
    }

    public enum GamePhase
    {
        Player,
        Modifiers,
        Event
    }

    public class GamePipeline
    {
        private static List<(Type Type, float Probability)> _eventCatalog;

        private readonly PersonService _people;
        private readonly GameTimeService _time;
        private readonly TravelService _travel;

        private bool _continueRequested;

        public GameEndState EndState { get; private set; } = GameEndState.None;

        public event Action<GameEndState> OnEnded;
        public event Action<GamePhase> OnPhaseChanged;

        public GamePipeline(PersonService people, GameTimeService time, TravelService travel)
        {
            _people = people;
            _time = time;
            _travel = travel;
        }

        public IEnumerator Run(MonoBehaviour listener)
        {
            while (EndState == GameEndState.None)
            {
                yield return PlayerPhase();
                if (EndState != GameEndState.None)
                    break;

                yield return ModifiersPhase(listener);
                if (EndState != GameEndState.None)
                    break;

                yield return EventPhase(listener);
                if (EndState != GameEndState.None)
                    break;

                _time?.AdvanceTurn();
                _travel?.AdvanceTravel();
            }
        }

        public void Continue()
        {
            _continueRequested = true;
        }

        public void Win()
        {
            End(GameEndState.Won);
        }

        public void Lose()
        {
            End(GameEndState.Lost);
        }

        private void End(GameEndState state)
        {
            if (EndState != GameEndState.None)
                return;

            EndState = state;
            _continueRequested = true;
            OnEnded?.Invoke(state);
        }

        private IEnumerator PlayerPhase()
        {
            OnPhaseChanged?.Invoke(GamePhase.Player);
            _continueRequested = false;

            while (!_continueRequested && EndState == GameEndState.None)
                yield return null;
        }

        private IEnumerator ModifiersPhase(MonoBehaviour listener)
        {
            OnPhaseChanged?.Invoke(GamePhase.Modifiers);
            _people?.ApplyPhaseModifiers();

            if (_people == null)
                yield break;

            for (var p = 0; p < _people.People.Count; p++)
            {
                if (EndState != GameEndState.None)
                    yield break;

                var person = _people.People[p];
                var statuses = person.LiveStatuses;
                for (var s = 0; s < statuses.Count; s++)
                {
                    if (EndState != GameEndState.None)
                        yield break;

                    var status = statuses[s];
                    if (status == null || string.IsNullOrEmpty(status.Tag))
                        continue;

                    yield return BoundEventRunner.Run(
                        BoundGameEventKind.Status,
                        status.Tag,
                        listener,
                        source: person);
                }
            }

            for (var p = 0; p < _people.People.Count; p++)
            {
                if (EndState != GameEndState.None)
                    yield break;

                var person = _people.People[p];
                var traits = person.Traits;
                if (traits == null)
                    continue;

                for (var t = 0; t < traits.Count; t++)
                {
                    if (EndState != GameEndState.None)
                        yield break;

                    var trait = traits[t];
                    if (trait == null || string.IsNullOrEmpty(trait.Tag))
                        continue;

                    yield return BoundEventRunner.Run(
                        BoundGameEventKind.Trait,
                        trait.Tag,
                        listener,
                        source: person);
                }
            }
        }

        private IEnumerator EventPhase(MonoBehaviour listener)
        {
            OnPhaseChanged?.Invoke(GamePhase.Event);
            var pool = new List<(Type Type, float Probability)>(GetEventCatalog());

            while (pool.Count > 0 && EndState == GameEndState.None)
            {
                var index = UnityEngine.Random.Range(0, pool.Count);
                var candidate = pool[index];
                pool.RemoveAt(index);

                if (UnityEngine.Random.value > candidate.Probability)
                    continue;

                if (!(Activator.CreateInstance(candidate.Type) is GameEventBase gameEvent))
                    continue;

                if (!gameEvent.EventCanExecute())
                    continue;

                yield return gameEvent.Invoke(listener);
                yield break;
            }
        }

        private static List<(Type Type, float Probability)> GetEventCatalog()
        {
            if (_eventCatalog != null)
                return _eventCatalog;

            _eventCatalog = new List<(Type, float)>();

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    types = ex.Types;
                }

                if (types == null)
                    continue;

                foreach (var type in types)
                {
                    if (type == null || type.IsAbstract || !typeof(GameEventBase).IsAssignableFrom(type))
                        continue;

                    var attribute = type.GetCustomAttribute<UseGameEventAttribute>();
                    if (attribute == null)
                        continue;

                    _eventCatalog.Add((type, attribute.Probability));
                }
            }

            return _eventCatalog;
        }
    }
}
