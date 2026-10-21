using LongRoad.Core.GameEvent;
using System.Collections;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    public class ModifiersGamePhaseStrategy : IGamePhaseStrategy
    {
        public int Order => 1;

        public void Init() { }

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener)
        {
            if (services.GameData.Car.Fuel > 0)
            {
                services.Background.StartScroll();
            }

            services.People?.ApplyPhaseModifiers();

            if (services.People == null)
                yield break;

            for (var p = 0; p < services.People.People.Count; p++)
            {
                if (pipeline.EndState != GameEndState.None)
                    yield break;

                var person = services.People.People[p];
                var statuses = person.LiveStatuses;
                for (var s = 0; s < statuses.Count; s++)
                {
                    if (pipeline.EndState != GameEndState.None)
                        yield break;

                    var status = statuses[s];
                    if (status == null || string.IsNullOrEmpty(status.Tag))
                        continue;

                    yield return BoundEventRunner.Run(
                        BoundGameEventKind.LiveStatus,
                        status,
                        person,
                        listener);
                }
            }

            for (var p = 0; p < services.People.People.Count; p++)
            {
                if (pipeline.EndState != GameEndState.None)
                    yield break;

                var person = services.People.People[p];
                var traits = person.Traits;
                if (traits == null)
                    continue;

                for (var t = 0; t < traits.Count; t++)
                {
                    if (pipeline.EndState != GameEndState.None)
                        yield break;

                    var trait = traits[t];
                    if (trait == null || string.IsNullOrEmpty(trait.Tag))
                        continue;

                    yield return BoundEventRunner.Run(
                        BoundGameEventKind.Trait,
                        trait,
                        person,
                        listener);
                }
            }
        }
    }
}
