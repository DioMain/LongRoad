using System.Collections;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    public class PlayerGamePhaseStrategy : IGamePhaseStrategy
    {
        public int Order => 0;

        public void Init() { }

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener)
        {
            services.Background.StopScroll();

            pipeline.Pause();

            yield return new WaitWhile(() => pipeline.PipelinePaused && pipeline.EndState == GameEndState.None);
        }
    }
}
