using LongRoad.Core.GameEvent.Abstractions;
using LongRoad.Core.Scriptables.Abstractions;
using System;
using System.Collections;
using UnityEngine;

namespace LongRoad.Core.GameEvent
{
    public static class BoundEventRunner
    {
        public static IEnumerator Run(
            BoundGameEventKind kind,
            LongRoadScriptable source,
            PersonEntity target,
            MonoBehaviour host)
        {
            if (host == null)
                yield break;

            var types = BoundEventCatalog.Get(kind, source.Tag);
            for (var i = 0; i < types.Count; i++)
            {
                if (!(Activator.CreateInstance(types[i]) is GameEventBase gameEvent))
                    continue;

                if (gameEvent is ContextualGameEventBase contextual)
                {
                    contextual.Source = source;
                    contextual.Target = target;
                }

                if (!gameEvent.EventCanExecute())
                    continue;

                yield return gameEvent.Invoke(host);
            }
        }
    }
}
