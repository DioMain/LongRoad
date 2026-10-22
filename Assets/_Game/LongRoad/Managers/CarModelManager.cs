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
            Subscribe();
            RefreshState();
        }

        public void SetState(CarModelState state)
        {
            model?.SetState(state);
        }

        public void RefreshState()
        {
            if (model == null || Local?.Data == null)
                return;

            var car = Car;
            if (car == null || car.Fuel <= 0)
            {
                SetState(CarModelState.Off);
                return;
            }

            if (Local.Pipeline.Phase is PlayerGamePhaseStrategy || Local.Data.CurrentLocation != null)
            {
                SetState(CarModelState.Idle);
                return;
            }

            if (Local.Pipeline.Phase is ModifiersGamePhaseStrategy || Local.Pipeline.Phase is EventGamePhaseStrategy)
            {
                SetState(CarModelState.Drive);
                return;
            }

            SetState(CarModelState.Idle);
        }

        public override void Dispose()
        {
            Unsubscribe();
            model.Dispose();
        }

        private void OnDestroy()
        {
            Dispose();
        }

        private void Subscribe()
        {
            Local.Pipeline.OnPhaseChanged += HandlePhaseChanged;

            if (Car != null)
                Car.OnFuelChanged += HandleFuelChanged;
        }

        private void Unsubscribe()
        {
            Local.Pipeline.OnPhaseChanged -= HandlePhaseChanged;

            if (Car != null)
                Car.OnFuelChanged -= HandleFuelChanged;
        }

        private void HandlePhaseChanged(IGamePhaseStrategy _)
        {
            RefreshState();
        }

        private void HandleFuelChanged(CarEntity _, int __)
        {
            RefreshState();
        }
    }
}
