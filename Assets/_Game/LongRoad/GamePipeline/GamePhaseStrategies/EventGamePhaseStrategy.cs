using LongRoad.Core.GameEvent.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    public class EventGamePhaseStrategy : IGamePhaseStrategy
    {
        private List<(Type Type, float Probability)> _eventCatalog;

        public int Order => 2;

        public void Init()
        {
            GetEventCatalog();
        }

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener)
        {
            var pool = new List<(Type Type, float Probability)>(GetEventCatalog());

            while (pool.Count > 0 && pipeline.EndState == GameEndState.None)
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

        private List<(Type Type, float Probability)> GetEventCatalog()
        {
            if (_eventCatalog != null)
                return _eventCatalog;

            _eventCatalog = new List<(Type, float)>();

            Type[] types = GetType().Assembly.GetTypes()
                .Where((type) => !type.IsAbstract || typeof(GameEventBase).IsAssignableFrom(type)).ToArray();

            foreach (var type in types)
            {
                var attribute = type.GetCustomAttribute<UseGameEventAttribute>();
                if (attribute == null)
                    continue;

                _eventCatalog.Add((type, attribute.Probability));
            }

            return _eventCatalog;
        }
    }
}
