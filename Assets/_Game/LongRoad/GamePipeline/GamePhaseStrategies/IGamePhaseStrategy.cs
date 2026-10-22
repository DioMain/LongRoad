using LongRoad.Managers;
using LongRoad.Services;
using System.Collections;
using UnityEngine;

namespace LongRoad.GamePipeline.GamePhaseStrategies
{
    public class GamePhaseStrategyServices
    {
        public PersonService People;
        public GameTimeService Time;
        public TravelService Travel;
        public BackgroundManager Background;
        public GameData GameData;
    }

    public interface IGamePhaseStrategy
    {
        public int Order { get; }

        public void Init();

        public IEnumerator Invoke(GamePipeline pipeline, GamePhaseStrategyServices services, MonoBehaviour listener);
    }
}
