using System.Collections;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    internal class PersonAIPhaseStrategy : IGamePhaseStrategy
    {
        public int Order => 3;

        public void Init() { }

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener)
        {
            foreach (var person in services.People)
            {
                if (person.AI != null)
                {
                    yield return new WaitForSeconds(0.2f);

                    yield return person.AI.Think(services.GameData);
                }
            }
        }
    }
}
