using LongRoad.Core;
using LongRoad.GamePipeline.GamePhaseStrategies;
using UnityEngine;

namespace LongRoad.Managers
{
    public class CarModelManager : LongRoadBehaviour
    {
        [SerializeField]
        private Transform CarContainer;

        private CarEntity Car => Local?.Data?.Car;

        private CarModel model;
        public CarModel Model => model;

        public CarModelState State => model != null ? model.State : CarModelState.Off;

        public override void Init()
        {
            if (Car == null)
            {
                Debug.LogError($"{nameof(CarModelManager)}: no car in session data.", this);
                return;
            }

            model = Car.SpawnModel(CarContainer);
        }

        public override void Dispose()
        {
            model.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }
    }
}
